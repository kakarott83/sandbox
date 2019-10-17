using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Cic.One.DTO;
using Cic.OpenOne.Common.DTO;
using CIC.ASS.SearchService.DTO;
using Cic.OpenOne.Common.BO;
using Cic.One.Web.BO.Search;
using Cic.One.Web.BO;
using Cic.OpenOne.Common.DAO;

namespace Cic.One.Web.DAO
{
    public interface XproInfoBaseDao
    {
        string getBeschreibung(IHtmlReportBo reporter, object item, XproEntityDto rval);

        string getBezeichnung(IHtmlReportBo reporter, object item);

        XproEntityDto getXproItem(IXproLoaderDao loader, igetXproItemDto input);

        XproEntityDto[] getXproItems(IXproLoaderDao loader, igetXproItemsDto input);

        string TemplateBeschreibung { get; set; }

        string TemplateBezeichnung { get; set; }

        string QueryItem { get; set; }

        string QueryItems { get; set; }

        string Area { get; set; }
    }

    public class XproInfoDao : XproInfoDao<object>
    {
    }

    public class XproInfoDao<T> : XproInfoBaseDao
    {
        public string Area { get; set; }

        public string QueryItem { get; set; }

        public string QueryItems { get; set; }
        /// <summary>
        /// Deprecated, use Function2 !
        /// </summary>
        public Func<long, T> QueryItemFunction { get; set; }
        /// <summary>
        /// New Method taking the whole needed input
        /// </summary>
        public Func<igetXproItemDto, T> QueryItemFunction2 { get; set; }

        public Func<igetXproItemsDto, T[]> QueryItemsFunction { get; set; }

        public string TemplateBeschreibung { get; set; }

        public string TemplateBezeichnung { get; set; }

        public Func<T, string> CreateBezeichnung { get; set; }

        public Func<T,XproEntityDto, string> CreateBeschreibung { get; set; }

        public string getBeschreibung(IHtmlReportBo reporter, object item,XproEntityDto rval)
        {
           
            if (CreateBeschreibung == null || !(item is T))
            {
                return reporter.CreateHtmlReport(item, 1);
            }
            else
            {
                return CreateBeschreibung((T)item,rval);
            }
        }

        public string getBezeichnung(IHtmlReportBo reporter, object item)
        {
            if (CreateBezeichnung == null || !(item is T))
            {
                return reporter.CreateHtmlReport(item, 0);
            }
            else
            {
                return CreateBezeichnung((T)item);
            }
        }
        public XproEntityDto getXproItem(IXproLoaderDao loader, igetXproItemDto input)
        {
            if (QueryItemFunction2 != null)
            {
                return CreateDroplistItem(input.EntityId, QueryItemFunction2(input), input.sysvlm, input.Area);
            }
            else if (QueryItemFunction != null)
            {
                return CreateDroplistItem(input.EntityId, QueryItemFunction(input.EntityId), input.sysvlm, input.Area);
            }
            else
            {
                return CreateDroplistItem(input.EntityId, loader.LoadData(this, input.EntityId), input.sysvlm, input.Area);
                //CreateDroplistItem(info, input.EntityId, data);
            }
        }
        
    

        /// <summary>
        /// Items of type xproentitydto or droplistdto WONT have the templating for its fields!
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public XproEntityDto[] getXproItems(IXproLoaderDao loader, igetXproItemsDto input)
        {
            if (QueryItemsFunction != null)
            {
                var data = QueryItemsFunction(input);
                if (data == null)
                    return null;
                List<XproEntityDto> entities = new List<XproEntityDto>();
                int id = 0;
                Func<XproEntityDto, String> favFunc = new Func<XproEntityDto, String>(x => x.code);
                foreach (var item in data)
                {
                    if (item is XproEntityDto)
                    {
                        if (!string.IsNullOrEmpty((item as XproEntityDto).title))
                            entities.Add(item as XproEntityDto);
                    }
                    else if (item is DropListDto)
                    {
                        if(!string.IsNullOrEmpty((item as DropListDto).bezeichnung))
                            entities.Add(new XproEntityDto(item as DropListDto));
                    }
                    else if (item is EntityDto)
                    {
                        //üblicherweise wird ein entitdto typ enumeriert und als array geliefert, hier wird daraus das xproentitydto
                        entities.Add(CreateDroplistItem((item as EntityDto).entityId, item, input.sysvlm, input.Area));
                        favFunc = new Func<XproEntityDto, String>(x => x.bezeichnung.ToString());
                    }
                    else if (item is Enum)
                    {
                        entities.Add(CreateDroplistItem(Convert.ToInt32(item as Enum), item, input.sysvlm, input.Area));
                    }
                    else if (item is SearchResult)
                    {
                        entities.Add(CreateDroplistItem(long.Parse((item as SearchResult).id), item, input.sysvlm, input.Area));
                    }
                    else //TODO ID raus finden
                    {
                        entities.Add(CreateDroplistItem(id, item, input.sysvlm, input.Area));
                        id++;
                    }
                    
                }
                String fc = input.favCode!=null?input.favCode:input.Area.ToString();
                if("STRING".Equals(fc))
                    return entities.ToArray();
                return XproFavorite.sortFavorites<XproEntityDto>(fc, entities, favFunc).ToArray();
                
            }
         
            else
            {
                List<ExpandoObject> data = loader.LoadDatas(this, input.Filter);
                if (data == null)
                    return null;

                List<XproEntityDto> entities = new List<XproEntityDto>();
                foreach (ExpandoObject item in data)
                {
                    object itemid = item.FirstOrDefault((a) => a.Key == "id");
                    long entityId = 0;
                    if (itemid != null && itemid is KeyValuePair<string, object>)
                    {
                        var pair = (KeyValuePair<string, object>)itemid;
                        entityId = Convert.ToInt64(pair.Value);
                    }
                    else
                    {
                        //throw exception?
                        //Keine Id gefunden
                    }
                    entities.Add(CreateDroplistItem(entityId, item,input.sysvlm,input.Area));
                }
                return entities.ToArray();
            }
        }

        /// <summary>
        /// fills the xproentity with the corresponding data from the given data-object defined in the templates of the
        /// area-specific vlmtable
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sysvlm"></param>
        /// <param name="area"></param>
        /// <param name="rval"></param>
        /// <returns></returns>
        private XproEntityDto fillEntityInfos(object data, long sysvlm, XproEntityType area, XproEntityDto rval)
        {
         
            if (sysvlm > 0)
            {
                IWorkflowBo bo = BOFactoryFactory.getInstance().getWorkflowBo();
                OlArea olArea;
                if (Enum.TryParse<OlArea>(area.ToString(), true, out  olArea))
                {
                    VlmTableDto tab = bo.getVlmTable(sysvlm, olArea);
                    if (tab != null)
                    {
                        HtmlReportBo rep = new HtmlReportBo(new StringHtmlTemplateDao(null));
                        rval.desc1 = rep.ReplaceText(tab.template_description1, (T)data, true);
                        rval.desc2 = rep.ReplaceText(tab.template_description2, (T)data, true);
                        rval.desc3 = rep.ReplaceText(tab.template_description3, (T)data, true);
                        rval.title = rep.ReplaceText(tab.template_title, (T)data, true);
                        rval.bezeichnung = rep.ReplaceText(tab.template_short, (T)data, true);
                    }
                }
                else
                {
                    if(data is SearchResult)
                    {
                        SearchResult sr = (SearchResult)data;
                        rval.desc1 = sr.description1;
                        rval.desc2 = sr.description2;
                        rval.desc3 = sr.description3;
                        rval.title = sr.title;
                        rval.indicator = sr.indicator;
                        rval.bezeichnung = null;
                        rval.beschreibung = null;
                    }
                }
            }
            return rval;
        }
        /// <summary>
        /// Erzeugt aus den in QueryItemsFunction gelieferten daten mithilfe der in CreateBezeichnung und CreateBeschreibung definierten Funktionen die XproEntityDto's
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public XproEntityDto CreateDroplistItem(long entityId, object data, long sysvlm, XproEntityType area)
        {
            if (data == null)
                return null;

            XproEntityDto rval = null;

            if (data is XproEntityDto)//wenn bereits ein DropListDto geliefert wird, das so verwenden
            {
                rval =  data as XproEntityDto;
            }
            else if (data is DropListDto)//wenn bereits ein DropListDto geliefert wird, das so verwenden
            {
                rval = new XproEntityDto(data as DropListDto);
            }
            else
            {

                rval = new XproEntityDto();
                rval.sysID = entityId;
                rval = fillEntityInfos(data, sysvlm, area, rval);
                if (rval.bezeichnung == null || rval.beschreibung == null)
                {
                    HtmlReportBo reporter = new HtmlReportBo(new XproTemplateDao(this));
                    if (rval.bezeichnung == null)
                        rval.bezeichnung = getBezeichnung(reporter, data);
                    if (rval.beschreibung == null)
                        rval.beschreibung = getBeschreibung(reporter, data, rval);
                }
                    
            }
            
            return rval;

        }
    }
}