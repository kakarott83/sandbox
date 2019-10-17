using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Cic.One.Web.BO;
using Cic.One.DTO;
using Cic.One.Web.BO.Search;
using Cic.One.Web.DAO;
using Cic.One.Web.Contract;
using Cic.OpenOne.Common.Util.Security;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using System.ServiceModel.Web;

namespace Cic.One.Web.Service
{
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class xproSearchService : IxproSearchService
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "getItems?code={code}&sysperole={sysperole}&sysvlm={sysvlm}&isocode={isocode}&filter={filter}&filtername1={filtername1}&filtervalue1={filtervalue1}&domain={domain}")]
        public XproEntityDto[] getItems(String code, long sysperole, long sysvlm, String isocode, String filter, String filtername1, String filtervalue1, String domain)
        {
            WfvConfig config = WFVDao.getWfvConfig();
            if (config == null) return null;
            if (code == null) return null;
            String codeup = code.ToUpper();
            LUConfig luc = (from f in config.luconfigs
                            where f.code.Equals(codeup)
                            select f).FirstOrDefault();
            if (luc == null)
            {
                igetXproItemsDto input = new igetXproItemsDto();
                input.Filter = filter;
                input.sysperole = sysperole;
                input.areaCode = code;
                input.sysvlm = sysvlm;
                input.isoCode = isocode;
                input.domainId = domain;
                if(domain!=null && "KURZ".Equals(domain))
                     input.domain =     Cic.OpenOne.Common.DTO.DDLKPPOSDomain.KURZ;
                else if (domain != null && "LANG".Equals(domain))
                    input.domain = Cic.OpenOne.Common.DTO.DDLKPPOSDomain.LANG;

                try
                {
                    input.Area = (XproEntityType)Enum.Parse(typeof(XproEntityType), code, true);
                    input.areaCode = null;
                }catch(Exception)
                {
                    
                }
                if (filtername1 != null)
                {
                    input.filters = new Filter[1];
                    input.filters[0] = new Filter();
                    input.filters[0].fieldname = filtername1;
                    input.filters[0].value = filtervalue1;
                }
                //use old xpro-service
                return BOFactoryFactory.getInstance().getXproSearchBo().getXproItems(input);
                
            }

            return getGenericLookup(sysperole, sysvlm, isocode, filter, filtername1, filtervalue1, luc);
             
        }

        private static XproEntityDto[] getGenericLookup(long sysperole, long sysvlm, String isocode, String filter, String filtername1, String filtervalue1, LUConfig luc)
        {
            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                if (luc.query.Contains(":filter"))
                    par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + filter + "%" });
                if (luc.query.Contains(":sysperole"))
                    par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                if (luc.query.Contains(":sysvlm"))
                    par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvlm", Value = sysvlm });
                if (luc.query.Contains(":isocode"))
                    par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isocode });
                if (filtername1 != null)
                {
                    String[] filters = filtername1.Split(',');
                    String[] filtervalues = filtervalue1.Split(',');
                    for (int i = 0; i < filters.Length; i++)
                    {
                        if (luc.query.Contains(":filtername" + i))
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filtername" + i, Value = filters[i] });
                        String fv = filtervalues[0];
                        if (filtervalues.Length > i)
                            fv = filtervalues[i];
                        if (luc.query.Contains(":filterval" + i))
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filterval" + i, Value = fv });
                    }
                }

                return ctx.ExecuteStoreQuery<XproEntityDto>(luc.query, par.ToArray()).ToArray();
            }
        }

        /// <summary>
        /// Get List of Xproentites for xprocode and filter
        /// </summary>
        /// <param name="igetXproItems"></param>
        /// <returns>List of Xproentites</returns>
        public ogetXproItemsDto getXproItems(igetXproItemsDto igetXproItems)
        {
            ServiceHandler<igetXproItemsDto, ogetXproItemsDto> ew = new ServiceHandler<igetXproItemsDto, ogetXproItemsDto>(igetXproItems);
            return ew.process(delegate(igetXproItemsDto input, ogetXproItemsDto rval, CredentialContext ctx)
            {
              
                if (input == null )
                    throw new ArgumentException("No search input");

                MembershipUserValidationInfo vi = ctx.getMembershipInfo();
                input.sysperole = ctx.getMembershipInfo().sysPEROLE;
                input.isoCode = ctx.getMembershipInfo().ISOLanguageCode;

                //try to find in generic xpro search
                if (input.areaCode != null && input.areaCode.Length > 0)
                {
                    WfvConfig config = WFVDao.getWfvConfig();
                    if (config != null)
                    {
                        String codeup = input.areaCode.ToUpper();
                        LUConfig luc = (from f in config.luconfigs
                                        where f.code.Equals(codeup)
                                        select f).FirstOrDefault();
                        if(luc!=null)
                        {
                            String filtername1 = null;
                            String filtervalue1 = null;
                            if (input.filters!=null && input.filters.Length>0)
                            {
                                filtername1 = input.filters[0].fieldname;
                                filtervalue1 = input.filters[0].value;
                            }
                            rval.result = getGenericLookup(input.sysperole, input.sysvlm, input.isoCode, input.Filter, filtername1, filtervalue1, luc);
                            return;
                        }
                    }
                }

                DateTime start = DateTime.Now;
                rval.result = BOFactoryFactory.getInstance().getXproSearchBo().getXproItems(input);
                long duration = (long)(DateTime.Now - start).TotalMilliseconds;
                _log.Debug("Xpro Duration for " + igetXproItems.Area + ": " + duration);
                
                

            },false);
        }

        /// <summary>
        /// Gets XproEntity for xprocode and entityid
        /// </summary>
        /// <param name="igetXproItem"></param>
        /// <returns>XproEntity</returns>
        public ogetXproItemDto getXproItem(igetXproItemDto igetXproItem)
        {
            ServiceHandler<igetXproItemDto, ogetXproItemDto> ew = new ServiceHandler<igetXproItemDto, ogetXproItemDto>(igetXproItem);
            return ew.process(delegate(igetXproItemDto input, ogetXproItemDto rval, CredentialContext ctx)
            {

                if (input == null)
                    throw new ArgumentException("No search input");

                MembershipUserValidationInfo vi = ctx.getMembershipInfo();
                input.sysperole = ctx.getMembershipInfo().sysPEROLE;
                input.isoCode = ctx.getMembershipInfo().ISOLanguageCode;

                rval.result = BOFactoryFactory.getInstance().getXproSearchBo().getXproItem(input);

            },false);
        }
    }
}
