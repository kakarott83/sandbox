using System;
using System.Collections.Generic;
using System.Linq;
using Cic.One.Web.DAO;
using Cic.One.DTO;
using System.Globalization;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.One.Workflow.BO;
using CIC.ASS.Common.BO;
using System.Text;
using AutoMapper;
using Cic.One.DTO.BN;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util;

namespace Cic.One.Web.BO
{
    /// <summary>
    /// Helper-Class for day/month from ptask
    /// </summary>
    class TagMonat
    {
        public long id { get; set; }
        public long day { get; set; }
        public long month { get; set; }
    }

    public class EntityBo : AbstractEntityBo
    {
        public static bool INDICATOR_DISABLED = true;

        private IAppSettingsBo appBo;
        private ICASBo casBo;
        protected long sysWfUser = 0;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<long, String> months = new System.Collections.Generic.Dictionary<long, string>();

        public EntityBo(IEntityDao dao)
            : base(dao)
        {
            initMonths();
        }

        public EntityBo(IEntityDao dao, IAppSettingsBo appBo, ICASBo casBo)
            : base(dao)
        {
            this.appBo = appBo;
            this.casBo = casBo;
            initMonths();
        }

        private void initMonths()
        {
            if (months.Count == 0)
            {
                months.Add(1, "jan");
                months.Add(2, "feb");
                months.Add(3, "mar");
                months.Add(4, "apr");
                months.Add(5, "mai");
                months.Add(6, "jun");
                months.Add(7, "jul");
                months.Add(8, "aug");
                months.Add(9, "sep");
                months.Add(10, "okt");
                months.Add(11, "nov");
                months.Add(12, "dez");
            }
        }

        public void setSysWfUser(long sysWfUser)
        {
            this.sysWfUser = sysWfUser;
        }

        public long getSysWfUser()
        {
            return this.sysWfUser;
        }
        private void insertRecent(EntityDto dto)
        {
            appBo.insertRecent(dto, sysWfUser);
        }

        /// <summary>
        /// Assigns all indicator defaults to the list
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entities"></param>
        public static void assignIndicatorDefault<R>(List<EntityDto> entities)
        {
            if (INDICATOR_DISABLED) return;
            if (typeof(R).Equals(typeof(Cic.One.DTO.AccountDto)) || typeof(R).Equals(typeof(Cic.One.DTO.ItDto)) || typeof(R).Equals(typeof(Cic.One.DTO.PartnerDto)))
            //TODO- enable for all entities?
            {

                long[] ids = (from t in entities
                              select t.entityId).ToArray();

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(null);
                igetExpdefDto expdefInput = new igetExpdefDto();
                expdefInput.area = AreaEntityMapperBO.getInstance().getArea(typeof(R));
                expdefInput.areaids = ids;
                List<ExpdefDto> defaults = bo.getExpdefDetails(expdefInput);
                foreach (ExpdefDto defs in defaults)
                {
                    EntityDto dto = (from t in entities
                                     where t.entityId == defs.areaid
                                     select t).FirstOrDefault();

                    dto.indicatorContent = defs.output;
                }
            }
        }
        
        /// <summary>
        /// Assigns the indicator default to the entity
        /// </summary>
        /// <param name="entity"></param>
        public static void assignIndicatorDefault(EntityDto entity)
        {
            if (INDICATOR_DISABLED) return;
            if (entity == null) return;
            if (entity.GetType().Equals(typeof(Cic.One.DTO.AccountDto)) || entity.GetType().Equals(typeof(Cic.One.DTO.ItDto)) || entity.GetType().Equals(typeof(Cic.One.DTO.PartnerDto)))
            //TODO- enable for all entities?
            {

                long[] ids = new long[] { entity.entityId };

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(null);
                igetExpdefDto expdefInput = new igetExpdefDto();
                expdefInput.area = AreaEntityMapperBO.getInstance().getArea(entity.GetType());
                expdefInput.areaids = ids;
                List<ExpdefDto> defaults = bo.getExpdefDetails(expdefInput);
                foreach (ExpdefDto defs in defaults)
                {
                    entity.indicatorContent = defs.output;
                }
            }
        }

        #region CREATEORUPDATE

        /// <summary>
        /// updates/creates Gelesen/Ungelesen Flag
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
		override public PreadDto createOrUpdatePread (PreadDto pread)
        {
			PreadDto rval = dao.createOrUpdatePread (pread);
            return rval;
        }

        /// <summary>
        /// updates/creates Gview
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        override public GviewDto createOrUpdateGview(GviewDto gview)
        {
            GviewDto rval = dao.createOrUpdateGview(gview);
            return rval;
        }
        
        /// <summary>
        /// updates/creates Staffelpositionstyp
        /// </summary>
        /// <param name="staffelpositionstyp"></param>
        /// <returns></returns>
        override public StaffelpositionstypDto createOrUpdateStaffelpositionstyp(StaffelpositionstypDto staffelpositionstyp)
        {
            StaffelpositionstypDto rval = dao.createOrUpdateStaffelpositionstyp(staffelpositionstyp);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Staffeltyp
        /// </summary>
        /// <param name="staffeltyp"></param>
        /// <returns></returns>
        override public StaffeltypDto createOrUpdateStaffeltyp(StaffeltypDto staffeltyp)
        {
            StaffeltypDto rval = dao.createOrUpdateStaffeltyp(staffeltyp);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Rolle
        /// </summary>
        /// <param name="rolle"></param>
        /// <returns></returns>
        override public RolleDto createOrUpdateRolle(RolleDto rolle)
        {
            RolleDto rval = dao.createOrUpdateRolle(rolle);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Rollentyp
        /// </summary>
        /// <param name="rollentyp"></param>
        /// <returns></returns>
        override public RollentypDto createOrUpdateRollentyp(RollentypDto rollentyp)
        {
            RollentypDto rval = dao.createOrUpdateRollentyp(rollentyp);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Handelsgruppe
        /// </summary>
        /// <param name="handelsgruppe"></param>
        /// <returns></returns>
        override public HandelsgruppeDto createOrUpdateHandelsgruppe(HandelsgruppeDto handelsgruppe)
        {
            HandelsgruppeDto rval = dao.createOrUpdateHandelsgruppe(handelsgruppe);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Vertriebskanal
        /// </summary>
        /// <param name="vertriebskanal"></param>
        /// <returns></returns>
        override public VertriebskanalDto createOrUpdateVertriebskanal(VertriebskanalDto vertriebskanal)
        {
            VertriebskanalDto rval = dao.createOrUpdateVertriebskanal(vertriebskanal);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Brand
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        override public BrandDto createOrUpdateBrand(BrandDto brand)
        {
            BrandDto rval = dao.createOrUpdateBrand(brand);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Rechnung
        /// </summary>
        /// <param name="rechnung"></param>
        /// <returns></returns>
        override public RechnungDto createOrUpdateRechnung(RechnungDto rechnung)
        {
            RechnungDto rval = dao.createOrUpdateRechnung(rechnung);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Angobbrief
        /// </summary>
        /// <param name="angobbrief"></param>
        /// <returns></returns>
        override public AngobbriefDto createOrUpdateAngobbrief(AngobbriefDto angobbrief)
        {
            AngobbriefDto rval = dao.createOrUpdateAngobbrief(angobbrief);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Zahlungsplan
        /// </summary>
        /// <param name="zahlungsplan"></param>
        /// <returns></returns>
        override public ZahlungsplanDto createOrUpdateZahlungsplan(ZahlungsplanDto zahlungsplan)
        {
            ZahlungsplanDto rval = dao.createOrUpdateZahlungsplan(zahlungsplan);
            insertRecent(rval);
            return rval;
        }

		/// <summary>
		/// updates/creates Zahlungsplan
		/// </summary>
		/// <param name="zahlungsplan"></param>
		/// <returns></returns>
		override public KreditlinieDto createOrUpdateKreditlinie (KreditlinieDto kreditlinie)
		{
			KreditlinieDto rval = dao.createOrUpdateKreditlinie (kreditlinie);
			insertRecent (rval);
			return rval;
		}

		/// <summary>
        /// updates/creates Fahrzeugbrief
        /// </summary>
        /// <param name="fahrzeugbrief"></param>
        /// <returns></returns>
        override public FahrzeugbriefDto createOrUpdateFahrzeugbrief(FahrzeugbriefDto fahrzeugbrief)
        {
            FahrzeugbriefDto rval = dao.createOrUpdateFahrzeugbrief(fahrzeugbrief);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Kalk
        /// </summary>
        /// <param name="kalk"></param>
        /// <returns></returns>
        override public KalkDto createOrUpdateKalk(KalkDto kalk)
        {
            KalkDto rval = dao.createOrUpdateKalk(kalk);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        override public PersonDto createOrUpdatePerson(PersonDto person)
        {
            PersonDto rval = dao.createOrUpdatePerson(person);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates expval
        /// </summary>
        /// <param name="expval"></param>
        /// <returns></returns>
        override public ExpvalDto createOrUpdateExpval(ExpvalDto expval)
        {
            return dao.createOrUpdateExpval(expval);
        }
        /// <summary>
        /// updates/creates finanzierung
        /// </summary>
        /// <param name="finanzierung"></param>
        /// <returns></returns>
        override public FinanzierungDto createOrUpdateFinanzierung(FinanzierungDto finanzierung, int saveMode)
        {

            return dao.createOrUpdateFinanzierung(finanzierung,saveMode);
        }

        /// <summary>
        /// updates/creates RechnungFaellig
        /// </summary>
        /// <param name="rechnungFaellig"></param>
        /// <returns></returns>
        override public RechnungFaelligDto createOrUpdateRechnungFaellig(RechnungFaelligDto rechnungFaellig)
        {
            return dao.createOrUpdateRechnungFaellig(rechnungFaellig);
        }

        /// <summary>
        /// updates/creates Tilgung
        /// </summary>
        /// <param name="tilgung"></param>
        /// <returns></returns>
        override public TilgungDto createOrUpdateTilgung(TilgungDto tilgung)
        {
            return dao.createOrUpdateTilgung(tilgung);
        }


        /// <summary>
        /// updates/creates Objekt
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        override public ObDto createOrUpdateObjekt(ObDto objekt)
        {
            return dao.createOrUpdateOb(objekt);
        }

        /// <summary>
        /// updates/creates Objekt
        /// </summary>
        /// <param name="objekt"></param>
        /// <returns></returns>
        override public ObDto createOrUpdateHEKObjekt(ObDto objekt)
        {
            return dao.createOrUpdateHEKOb(objekt);
        }

        // <summary>
        /// updates/creates Recalc
        /// </summary>
        /// <param name="recalc"></param>
        /// <returns></returns>
        override public RecalcDto createOrUpdateRecalc(RecalcDto recalc)
        {
            RecalcDto rval = recalc;//WE DONT DO THAT! thats done by cas dao.createOrUpdateRecalc(recalc);
            insertRecent(rval);
            dao.createOrUpdateRecalc(recalc);//Just to call SearchCache.entityChanged("RECALC"); ALPHAONE-926            
            return rval;
        }

        // <summary>
        /// updates/creates Mycalc
        /// </summary>
        /// <param name="mycalc"></param>
        /// <returns></returns>
        override public MycalcDto createOrUpdateMycalc(MycalcDto mycalc)
        {
            MycalcDto rval = dao.createOrUpdateMycalc(mycalc);
            insertRecent(rval);
            return rval;
        }

        // <summary>
        /// updates/creates Mycalcfs
        /// </summary>
        /// <param name="mycalcfs"></param>
        /// <returns></returns>
        override public MycalcfsDto createOrUpdateMycalcfs(MycalcfsDto mycalcfs)
        {
            return dao.createOrUpdateMycalcfs(mycalcfs);
        }

        /// <summary>
        /// updates/creates Rahmen
        /// </summary>
        /// <param name="rahmen"></param>
        /// <returns></returns>
        override public RahmenDto createOrUpdateRahmen(RahmenDto rahmen)
        {
            RahmenDto rval = dao.createOrUpdateRahmen(rahmen);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Haendler
        /// </summary>
        /// <param name="haendler"></param>
        /// <returns></returns>
        override public HaendlerDto createOrUpdateHaendler(HaendlerDto haendler)
        {
            return dao.createOrUpdateHaendler(haendler);
        }

        /// <summary>
        /// updates/creates Kunde
        /// </summary>
        /// <param name="kunde"></param>
        /// <returns></returns>
        override public KundeDto createOrUpdateKunde(KundeDto kunde)
        {
            return dao.createOrUpdateKunde(kunde);
        }

        /// <summary>
        /// updates/creates It
        /// </summary>
        /// <param name="it"></param>
        /// <returns></returns>
        override public ItDto createOrUpdateIt(ItDto it)
        {
            ItDto rval = dao.createOrUpdateIt(it);
            insertRecent(rval);
            return rval;

        }

        /// <summary>
        /// updates/creates itadresse
        /// </summary>
        /// <param name="itadresse"></param>
        /// <returns></returns>
        override public ItadresseDto createOrUpdateItadresse(ItadresseDto itadresse)
        {
            return dao.createOrUpdateItadresse(itadresse);
        }

        /// <summary>
        /// updates/creates itkonto
        /// </summary>
        /// <param name="itkonto"></param>
        /// <returns></returns>
        override public ItkontoDto createOrUpdateItkonto(ItkontoDto itkonto)
        {
            return dao.createOrUpdateItkonto(itkonto);
        }

        /// <summary>
        /// Returns the image-url parameters for the given entity
        /// </summary>
        /// <param name="entity">The GUI Area, e.g. Account, Wktaccount</param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        override public String getEntityLink(String entity, long sysid, long syswfuser, String vlmcode)
        {
            String url = "entity=" + entity;

            if (sysid == 0)
            {
                url += "&checked=false";
                return url;
            }
            using (PrismaExtended ctx = new PrismaExtended())
            {

                //TODO Queries in DAO
                if (entity.ToLower().Equals("ptask"))
                {
                    List<long> chk = ctx.ExecuteStoreQuery<long>("select sysptask from ptask where completeflag=1 and sysptask =" + sysid, null).ToList<long>();

                    if (!chk.Contains(sysid))
                    {
                        url += "&checked=false";
                    }

                }
                if (entity.ToLower().Equals("apptmt"))
                {
                    List<TagMonat> tagmonat = ctx.ExecuteStoreQuery<TagMonat>("select sysapptmt id,TO_CHAR(startdate, 'dd') day,TO_CHAR(startdate, 'MM') month   from apptmt where sysapptmt =" + sysid, null).ToList<TagMonat>();


                    TagMonat tm = (from t in tagmonat
                                   where t.id == sysid
                                   select t).FirstOrDefault();
                    if (tm != null)
                    {
                        url += "&day=" + tm.day + "&month=" + months[tm.month];

                    }

                }

                List<long> fav = ctx.ExecuteStoreQuery<long>("select sysid from regvar,regsec where regvar.code='fav'  and regsec.code='" + vlmcode + "' and regsec.sysregsec=regvar.sysregsec and UPPER(area)=UPPER('" + entity + "') and sysid =" + sysid + " and syswfuser=" + syswfuser, null).ToList<long>();

                if (fav.Contains(sysid))
                {
                    url += "&fav=true";
                }
                url += "&fkey=" + sysid;
            }
            return url;

        }

        private EntityIconDto getIcon(long sysid, String entity)
        {
            EntityIconDto ei = new EntityIconDto();
            ei.entity = entity;
            ei.key = sysid;
            ei.ischecked = true;
            ei.isfav = false;
            ei.day = "";
            ei.month = "";

            if (sysid == 0)
            {
                ei.ischecked = false;
            }
            return ei;
        }
        /// <summary>
        /// Returns the icon for the given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        override public List<EntityIconDto> getEntityIcons(String entity, List<long> ids, long syswfuser, String vlmcode)
        {
            StringBuilder sb = new StringBuilder();
            foreach (long s in ids)
            {
                if (sb.Length > 0)
                    sb.Append(",");
                sb.Append(s);
            }
            String idstr = sb.ToString();
            List<EntityIconDto> rval = new List<EntityIconDto>();

            entity = AreaEntityMapperBO.getInstance().getGuiEntityFromLucene(entity);

            List<long> chk = null;
            List<TagMonat> tagmonat = null;
            List<long> fav = null;
            List<long> fileatt = null;
            using (PrismaExtended ctx = new PrismaExtended())
            {

                //TODO Queries in DAO
                if (entity.ToLower().Equals("ptask"))
                {
                    List<long> chks = ctx.ExecuteStoreQuery<long>("select sysptask from ptask where completeflag=1 and sysptask in (" + idstr + ")", null).ToList<long>();
                }

                if (entity.ToLower().Equals("apptmt"))
                {
                    tagmonat = ctx.ExecuteStoreQuery<TagMonat>("select sysapptmt id,TO_CHAR(startdate, 'dd') day,TO_CHAR(startdate, 'MM') month   from apptmt where sysapptmt in (" + idstr + ")", null).ToList<TagMonat>();
                }
                fav = ctx.ExecuteStoreQuery<long>("select sysid from regvar,regsec where regvar.code='fav' and regsec.code='" + vlmcode + "' and regsec.sysregsec=regvar.sysregsec and UPPER(area)=UPPER('" + entity + "') and                      sysid in (" + idstr + ") and syswfuser=" + syswfuser, null).ToList<long>();

                 List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = entity.ToUpper() });
                fileatt = ctx.ExecuteStoreQuery<long>("select sysid from cic.fileatt fileatt where sysid in (" + idstr + ") and upper(area)=upper(:area) and activeflag = 1 and typcode='icon'", parameters.ToArray()).ToList();

                
            }

            foreach (long sysid in ids)
            {
                EntityIconDto ei = getIcon(sysid, entity);

                if (tagmonat != null)
                {
                    TagMonat tm = (from t in tagmonat
                                   where t.id == sysid
                                   select t).FirstOrDefault();
                    if (tm != null)
                    {
                        ei.day = "" + tm.day;
                        ei.month = "" + months[tm.month];
                    }
                }
                if (chk != null)
                {
                    if (!chk.Contains(sysid))
                    {
                        ei.ischecked = false;
                    }
                }
                if (fav.Contains(sysid))
                {
                    ei.isfav = true;
                }
                if (fileatt.Contains(sysid))
                {
                    FileattDto f = dao.getFileattDetails(entity, sysid);
                    if (f != null)
                    {
                        ei.iconimg = f.content;
                    }
                }

               
                rval.Add(ei);
            }


            return rval;
        }

        /// <summary>
        /// Returns the Icon for a given Entity
        /// </summary>
        /// <param name="entity">GUI Entity OR Lucene Entity</param>
        /// <param name="sysid"></param>
        /// <param name="syswfuser"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        override public EntityIconDto getEntityIcon(String entity, long sysid, long syswfuser, String vlmcode)
        {

            EntityIconDto ei = new EntityIconDto();
            entity = AreaEntityMapperBO.getInstance().getGuiEntityFromLucene(entity);
            ei.entity = entity;
            ei.key = sysid;
            ei.ischecked = true;
            ei.isfav = false;
            ei.day = "";
            ei.month = "";

            if (sysid == 0)
            {
                ei.ischecked = false;
            }
            else
            {
                using (PrismaExtended ctx = new PrismaExtended())
                {

                    //TODO Queries in DAO
                    if (entity.ToLower().Equals("ptask"))
                    {
                        List<long> chk = ctx.ExecuteStoreQuery<long>("select sysptask from ptask where completeflag=1 and sysptask =" + sysid, null).ToList<long>();

                        if (!chk.Contains(sysid))
                        {
                            ei.ischecked = false;
                        }

                    }

                    if (entity.ToLower().Equals("apptmt"))
                    {
                        List<TagMonat> tagmonat = ctx.ExecuteStoreQuery<TagMonat>("select sysapptmt id,TO_CHAR(startdate, 'dd') day,TO_CHAR(startdate, 'MM') month   from apptmt where sysapptmt =" + sysid, null).ToList<TagMonat>();


                        TagMonat tm = (from t in tagmonat
                                       where t.id == sysid
                                       select t).FirstOrDefault();
                        if (tm != null)
                        {
                            ei.day = "" + tm.day;
                            ei.month = "" + months[tm.month];
                        }
                    }

                    List<long> fav = ctx.ExecuteStoreQuery<long>("select sysid from regvar,regsec where regvar.code='fav' and regsec.code='" + vlmcode + "' and regsec.sysregsec=regvar.sysregsec and UPPER(area)=UPPER('" + entity + "') and sysid =" + sysid + " and syswfuser=" + syswfuser, null).ToList<long>();
                    if (fav.Contains(sysid))
                    {
                        ei.isfav = true;
                    }
                    FileattDto f = dao.getFileattDetails(entity, sysid);
                    if (f != null)
                    {
                        ei.iconimg = f.content;
                    }
                }

            }

            return ei;
            /*
            
            return url;
            */

        }

        /// <summary>
        /// updates/creates Contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        override public ContactDto createOrUpdateContact(ContactDto contact)
        {
            ContactDto rval = dao.createOrUpdateContact(contact);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        override public void createOrUpdateContacts(ContactDto contact, String personSQL)
        {
            IEnumerable<long> persons = dao.getSQLResults(personSQL);
            int i = 0;
            foreach (long sysperson in persons)
            {
                contact.sysPerson = sysperson;
                try
                {
                    contact.sysContact = 0;
                    createOrUpdateContact(contact);
                    i++;
                }
                catch (Exception e)
                {
                    _log.Error("Error creating Contact for Person " + sysperson, e);
                }
            }
            _log.Debug("Created " + i + " contacts from query " + personSQL);
        }

        /// <summary>
        /// updates/creates Adresse
        /// </summary>
        /// <param name="adresse"></param>
        /// <returns></returns>
        override public AdresseDto createOrUpdateAdresse(AdresseDto adresse)
        {
            AdresseDto rval = dao.createOrUpdateAdresse(adresse);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Angvar
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        override public AngvarDto createOrUpdateAngvar(AngvarDto angvar)
        {
            AngvarDto rval = dao.createOrUpdateAngvar(angvar);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Angebot
        /// </summary>
        /// <param name="angebot"></param>
        /// <returns></returns>
        override public AngebotDto createOrUpdateAngebot(AngebotDto angebot)
        {
            AngebotDto rval = dao.createOrUpdateAngebot(angebot);
            insertRecent(rval);
            LuceneBO.getInstance().queueForIndexUpdate("Angebot", rval.sysID);
            return rval;
        }

        /// <summary>
        /// updates/creates Antrag
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        override public AntragDto createOrUpdateAntrag(AntragDto antrag)
        {
            AntragDto rval = dao.createOrUpdateAntrag(antrag);
            insertRecent(rval);
            LuceneBO.getInstance().queueForIndexUpdate("Antrag", rval.sysID);
            return rval;
        }

        /// <summary>
        /// updates/creates Angob
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        override public AngobDto createOrUpdateAngob(AngobDto angob)
        {
            AngobDto rval = dao.createOrUpdateAngob(angob);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Antob
        /// </summary>
        /// <param name="antob"></param>
        /// <returns></returns>
        override public AntobDto createOrUpdateAntob(AntobDto antob)
        {
            AntobDto rval = dao.createOrUpdateAntob(antob);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Angkalk
        /// </summary>
        /// <param name="angkalk"></param>
        /// <returns></returns>
        override public AngkalkDto createOrUpdateAngkalk(AngkalkDto angkalk)
        {
            AngkalkDto rval = dao.createOrUpdateAngkalk(angkalk);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Antkalk
        /// </summary>
        /// <param name="antkalk"></param>
        /// <returns></returns>
        override public AntkalkDto createOrUpdateAntkalk(AntkalkDto antkalk)
        {
            AntkalkDto rval = dao.createOrUpdateAntkalk(antkalk);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Camp
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        override public CampDto createOrUpdateCamp(CampDto camp)
        {
            CampDto rval = dao.createOrUpdateCamp(camp);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Oppo
        /// </summary>
        /// <param name="oppo"></param>
        /// <returns></returns>
        override public OpportunityDto createOrUpdateOppo(OpportunityDto oppo)
        {
            OpportunityDto rval = dao.createOrUpdateOppo(oppo);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Oppotask
        /// </summary>
        /// <param name="oppotask"></param>
        /// <returns></returns>
        override public OppotaskDto createOrUpdateOppotask(OppotaskDto oppotask)
        {
            OppotaskDto rval = dao.createOrUpdateOppotask(oppotask);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        override public AccountDto createOrUpdateAccount(AccountDto account)
        {
            AccountDto rval = dao.createOrUpdateAccount(account);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Dokvalidation
        /// </summary>
        /// <param name="dokval"></param>
        /// <returns></returns>
        override public DokvalDto createOrUpdateDokval(DokvalDto dokval)
        {
            DokvalDto rval = new DokvalDto();
            
            CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[1];
            vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
            vars[0].LookupVariableName = "SPRACHE";
            vars[0].VariableName = "GUI";
            vars[0].Value = dao.getISOLanguage();
            
            long antragid=0;
            if(dokval.area=="ANTRAG")
                antragid = dokval.entityId;
            else{
               
            }
            long angebotid=0;
            if(dokval.area=="ANGEBOT")
                angebotid = dokval.entityId;
            else{

            }
            //Map containing all fields/values for a certain table/id
            Dictionary<Tuple<String, String>, List<Tuple<String, Cic.One.DTO.Mediator.QueueRecordDto>>> tableData = new Dictionary<Tuple<String, String>, List<Tuple<String, Cic.One.DTO.Mediator.QueueRecordDto>>>();
            List<String> cmds = new List<string>();
            foreach(Cic.One.DTO.Mediator.QueueRecordDto rec in dokval.data.records)
            {
                var iart = (from f in rec.values
                             where f.VariableName.Equals("F16")
                             select f.Value).FirstOrDefault();

                var table = (from f in rec.values
                                where f.VariableName.Equals("F17")
                                select f.Value).FirstOrDefault();
                var field = (from f in rec.values
                                where f.VariableName.Equals("F18")
                                select f.Value).FirstOrDefault();
                var tableid = (from f in rec.values
                                where f.VariableName.Equals("F19")
                                select f.Value).FirstOrDefault();
                var fieldvalue = (from f in rec.values
                             where f.VariableName.Equals("F13")
                             select f.Value).FirstOrDefault();
                
                Tuple<String,String> t = new Tuple<String,String>(table,tableid);//Datensatz
                Tuple<String, Cic.One.DTO.Mediator.QueueRecordDto> v = new Tuple<String, Cic.One.DTO.Mediator.QueueRecordDto>(field, rec);//Datensatz
                if(!tableData.ContainsKey(t))
                {
                    tableData[t] = new List<Tuple<string, Cic.One.DTO.Mediator.QueueRecordDto>>();
                }
                tableData[t].Add(v);

                if(iart!=null && iart.Length>0)
                    cmds.Add("_SCRIPT('ANTRAG_RESET','START', '"+iart+"', " +antragid+ ", "+angebotid+",'','') ");
            }

            if (cmds.Count > 0)
            {
                ICASBo bo = Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo();

                if (bo != null)
                {
                    foreach (String cmd in cmds)
                    {
                        Cic.OpenOne.Common.DTO.iCASEvaluateDto ieval = new Cic.OpenOne.Common.DTO.iCASEvaluateDto();
                        ieval.area = dokval.area;

                        ieval.expression = new String[] { cmd };
                        ieval.sysID = new long[] { dokval.entityId };
                        Cic.OpenOne.Common.DTO.CASEvaluateResult[] er = null;
                        try
                        {
                            er = bo.getEvaluation(ieval, getSysWfUser());
                        }
                        catch (Exception e)
                        {
                            _log.Error("Error Resetting Validations for "+dokval.area+"/"+dokval.entityId+": "+cmd,e);
                        }
                    }
                }
            }
//            "update tab set field=value, field=value where pkey=id";

            Dictionary<String, String> pkeys = new Dictionary<string, string>();
            pkeys["PERSON"] = "SYSPERSON";
            pkeys["IT"] = "SYSIT";
            pkeys["ANGEBOT"] = "SYSID";
            pkeys["ANTRAG"] = "SYSID";
            pkeys["ANTOB"] = "SYSOB";
            pkeys["ANTOBBRIEF"] = "SYSANTOBBRIEF";
            pkeys["ANGOB"] = "SYSOB";
            pkeys["ANGOBBRIEF"] = "SYSANGOBBRIEF";
            pkeys["PKZ"] = "SYSPKZ";
            pkeys["ITPKZ"] = "SYSITPKZ";
            pkeys["UKZ"] = "SYSUKZ";
            pkeys["ITUKZ"] = "SYSITUKZ";

            String queueName = "";
            using (PrismaExtended ctx = new PrismaExtended())
            {
                foreach(Tuple<String,String> tab in tableData.Keys)
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                
                    String QUERY = "UPDATE " + tab.Item1 + " SET ";
                    List<Tuple<string, Cic.One.DTO.Mediator.QueueRecordDto>> fields = tableData[tab];
                    int nr = 0;
                    foreach (Tuple<string, Cic.One.DTO.Mediator.QueueRecordDto> fi in fields)
                    {
                        if (nr > 0) QUERY += ", ";
                        QUERY += " " + fi.Item1 + "=:" + fi.Item1;

                        String fieldvalue = (from f in fi.Item2.values
                                          where f.VariableName.Equals("F13")
                                          select f.Value).FirstOrDefault();
                        var fieldtype = (from f in fi.Item2.values
                                          where f.VariableName.Equals("F20")
                                          select f.Value).FirstOrDefault();
                        
                        if("DATE".Equals(fieldtype))
                        {
                            
                            DateTime datevalue = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDateTime(int.Parse(fieldvalue));
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = fi.Item1, Value = datevalue, OracleDbType = Devart.Data.Oracle.OracleDbType.Date });
                        }
                        else if(fieldtype.IndexOf("COMBOBOX")>-1)
                        {
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = fi.Item1, Value = long.Parse(fieldvalue), OracleDbType = Devart.Data.Oracle.OracleDbType.Long });
                        }
                        else
                        {
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = fi.Item1, Value = fieldvalue});
                        }

                        

                        nr++;
                    }
                    if(!pkeys.ContainsKey(tab.Item1))
                    {
                        _log.Error("Table Primary key not yet mapped: " + tab.Item1 + " Query not executed: " + QUERY);
                        continue;
                    }
                    QUERY += " WHERE "+pkeys[tab.Item1]+"=" + tab.Item2;
                    try
                    {
                        ctx.ExecuteStoreCommand(QUERY, parameters.ToArray());
                    }catch(Exception e)
                    {
                        _log.Error("ERROR WRITING DOK-VALIDATION: " + e.Message + " for " + QUERY, e);
                    }
                }
                ctx.SaveChanges();

                queueName = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "VALIDATION", "VALIQUEUE", "TEST_QUEUE_250");
                    
                
            }

            QueueResultDto qr = BOS.getQueueData(dokval.entityId, dokval.area, new String[]{"VALI_DATA"}, queueName,  vars, sysWfUser);
            rval.data = qr.queues[0];
            rval.area = dokval.area;
            rval.sysid = dokval.sysid;
            return rval;
            
        }


        /// <summary>
        /// updates/creates WktAccount
        /// </summary>
        /// <param name="wktaccount"></param>
        /// <returns></returns>
        public override WktaccountDto createOrUpdateWktAccount(WktaccountDto wktaccount)
        {
            WktaccountDto rval = dao.createOrUpdateWktAccount(wktaccount);
            insertRecent(rval);
            LuceneBO.getInstance().queueForIndexUpdate("WKTAccount", rval.entityId);
            return rval;
        }

        /// <summary>
        /// updates/creates Partner
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        override public PartnerDto createOrUpdatePartner(PartnerDto partner)
        {
            PartnerDto rval = dao.createOrUpdatePartner(partner);
            insertRecent(rval);
            LuceneBO.getInstance().queueForIndexUpdate("Account", rval.sysperson);
            return rval;
        }

        /// <summary>
        /// updates/creates Beteiligter
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        override public BeteiligterDto createOrUpdateBeteiligter(BeteiligterDto partner)
        {
            BeteiligterDto rval = dao.createOrUpdateBeteiligter(partner);
            insertRecent(rval);
            LuceneBO.getInstance().queueForIndexUpdate("Opportunity", rval.sysidparent);
            LuceneBO.getInstance().queueForIndexUpdate("Account", rval.sysperson);
            return rval;
        }

        /// <summary>
        /// updates/creates Konto
        /// </summary>
        /// <param name="konto"></param>
        /// <returns></returns>
        override public KontoDto createOrUpdateKonto(KontoDto konto)
        {
            KontoDto rval = dao.createOrUpdateKonto(konto);
            insertRecent(rval);
            return rval;
        }

        /// <summary>
        /// updates/creates Prkgroup
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        override public PrkgroupDto createOrUpdatePrkgroup(PrkgroupDto prkgroup)
        {
            return dao.createOrUpdatePrkgroup(prkgroup);
        }

        /// <summary>
        /// updates/creates Kundengruppenzuordnungen
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        override public PrkgroupmDto createOrUpdatePrkgroupm(PrkgroupmDto[] prkgroup)
        {
            return dao.createOrUpdatePrkgroupm(prkgroup);
        }

        /// <summary>
        /// updates/creates Zusatzdaten
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        override public Cic.One.DTO.DdlkpsposDto[] createOrUpdateDdlkpspos(Cic.One.DTO.DdlkpsposDto[] ddlkpspos)
        {
            return dao.createOrUpdateDdlkpspos(ddlkpspos);
        }

        /// <summary>
        /// updates/creates PartnerRelation
        /// </summary>
        /// <param name="ptrelate"></param>
        /// <returns></returns>
        public override PtrelateDto[] createOrUpdatePtrelate(PtrelateDto[] ptrelate)
        {
            return dao.createOrUpdatePtrelate(ptrelate);
        }

        /// <summary>
        /// updates/creates BeteiligterRelation
        /// </summary>
        /// <param name="ptrelate"></param>
        /// <returns></returns>
        public override CrmnmDto[] createOrUpdateCrmnm(CrmnmDto[] crmnm)
        {
            return dao.createOrUpdateCrmnm(crmnm);
        }

        /// <summary>
        /// updates/creates CrmProdukte
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public override CrmprDto createOrUpdateCrmProdukte(CrmprDto crmpr)
        {
            return dao.createOrUpdateCrmProdukte(crmpr);
        }

        /// <summary>
        /// updates/creates Checklist
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public override PrunDto createOrUpdatePrun(PrunDto prun)
        {
            return dao.createOrUpdatePrun(prun);
        }

        /// <summary>
        /// updates/creates Checklisttype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public override PtypeDto createOrUpdatePtype(PtypeDto ptype)
        {
            return dao.createOrUpdatePtype(ptype);
        }

        /// <summary>
        /// updates/creates Check
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public override PrunstepDto createOrUpdatePrunstep(PrunstepDto prunstep)
        {
            return dao.createOrUpdatePrunstep(prunstep);
        }

        /// <summary>
        /// updates/creates Checktype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public override PstepDto createOrUpdatePstep(PstepDto pstep)
        {
            return dao.createOrUpdatePstep(pstep);
        }

        /// <summary>
        /// updates/creates Segment
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public override SegDto createOrUpdateSeg(SegDto seg)
        {
            return dao.createOrUpdateSeg(seg);
        }

        /// <summary>
        /// updates/creates SegmentKampagne
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public override SegcDto createOrUpdateSegc(SegcDto segc)
        {
            return dao.createOrUpdateSegc(segc);
        }

        /// <summary>
        /// updates/creates Stickynote
        /// </summary>
        /// <param name="stickynotes"></param>
        /// <returns></returns>
        public override StickynoteDto[] createOrUpdateStickynotes(StickynoteDto[] stickynotes)
        {
            StickynoteDto[] rval = dao.createOrUpdateStickynotes(stickynotes);
            foreach (StickynoteDto note in rval)
                insertRecent(note);

            return rval;
        }

        /// <summary>
        /// updates/creates Stickytype
        /// </summary>
        /// <param name="stickynote"></param>
        /// <returns></returns>
        public override StickytypeDto createOrUpdateStickytype(StickytypeDto stickytype)
        {
            return dao.createOrUpdateStickytype(stickytype);
        }

        /// <summary>
        /// updates/creates Besuchsbericht
        /// </summary>
        /// <param name="besuchsbericht"></param>
        /// <returns></returns>
        public override BesuchsberichtDto createOrUpdateBesuchsbericht(BesuchsberichtDto besuchsbericht)
        {
            return dao.createOrUpdateBesuchsbericht(besuchsbericht);
        }

        ///// <summary>
        ///// updates/creates Dmsdoc
        ///// </summary>
        ///// <param name="dmsdoc"></param>
        ///// <returns></returns>
        public override DmsdocDto createOrUpdateDmsdoc(DmsdocDto dmsdoc)
        {
            return dao.createOrUpdateDmsdoc(dmsdoc);
        }

        /// <summary>
        /// updates/creates ZEK
        /// </summary>
        /// <param name="zek"></param>
        /// <returns></returns>
        public override ZekDto createOrUpdateZek(ZekDto zek)
        {
            return dao.createOrUpdateZek(zek);
        }


        #region Mail

        ///// <summary>
        ///// updates/creates Kategorien
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public override ItemcatDto createOrUpdateItemcat(ItemcatDto itemcat)
        //{
        //    return dao.createOrUpdateItemcat(itemcat);
        //}

        ///// <summary>
        ///// updates/creates ItemKategorien
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public override ItemcatmDto createOrUpdateItemcatm(ItemcatmDto itemcatm)
        //{
        //    return dao.createOrUpdateItemcatm(itemcatm);
        //}

        ///// <summary>
        ///// updates/creates Attachement
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public override FileattDto createOrUpdateFileatt(FileattDto fileatt)
        //{
        //    return dao.createOrUpdateFileatt(fileatt);
        //}

        ///// <summary>
        ///// updates/creates Reminder
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public override ReminderDto createOrUpdateReminder(ReminderDto reminder)
        //{
        //    return dao.createOrUpdateReminder(reminder);
        //}

        ///// <summary>
        ///// updates/creates Recurrence
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public override RecurrDto createOrUpdateRecurr(RecurrDto recurr)
        //{
        //    return dao.createOrUpdateRecurr(recurr);
        //}

        ///// <summary>
        ///// updates/creates Mailmsg
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public override MailmsgDto createOrUpdateMailmsg(MailmsgDto mailmsg)
        //{
        //    return dao.createOrUpdateMailmsg(mailmsg);
        //}

        ///// <summary>
        ///// updates/creates Apptmt
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public override ApptmtDto createOrUpdateApptmt(ApptmtDto apptmt)
        //{
        //    return dao.createOrUpdateApptmt(apptmt);
        //}

        ///// <summary>
        ///// updates/creates Ptask
        ///// </summary>
        ///// <param name="sysob"></param>
        ///// <returns></returns>
        //public override PtaskDto createOrUpdatePtask(PtaskDto ptask)
        //{
        //    return dao.createOrUpdatePtask(ptask);
        //}

        #endregion Mail

        
        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende
        /// </summary>
        /// <param name="memo">sysid, notiz, wftableSyscode, kurzbez, kategorie, ...</param>
        /// <returns>saved memo</returns>
        public override MemoDto createOrUpdateMemo(MemoDto memo, String refTable = null)
        {
            return dao.createOrUpdateMemo(memo, refTable);
        }


        /// <summary>
        /// Creates or updates a puser
        /// </summary>
        /// <param name="puser"></param>
        /// <returns></returns>
        public override PuserDto createOrUpdatePuser(PuserDto puser)
        {
            return dao.createOrUpdatePuser(puser);
        }


        /// <summary>
        /// Creates or updates a clarification
        /// </summary>
        /// <param name="clarification"></param>
        /// <returns></returns>
        public override ClarificationDto createOrUpdateClarification(ClarificationDto clarification)
        {
            return dao.createOrUpdateClarification(clarification);
        }
        #endregion CREATEORUPDATE

        #region GET

        /// <summary>
        /// Returns all Gviewtyp Details
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="gviewId"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        override public GviewDto getGviewDetails(long sysId, String gviewId, WorkflowContext ctx)
        {
            return dao.getGviewDetails(sysId, gviewId,ctx);
        }

        /// <summary>
        /// Returns all Staffelpositionstyp Details
        /// </summary>
        /// <param name="sysslpostyp"></param>
        /// <returns></returns>
        override public StaffelpositionstypDto getStaffelpositionstypDetails(long sysslpostyp)
        {
            StaffelpositionstypDto rval = dao.getStaffelpositionstypDetails(sysslpostyp);

            return rval;
        }

        /// <summary>
        /// Returns all Staffeltyp Details
        /// </summary>
        /// <param name="syssltyp"></param>
        /// <returns></returns>
        override public StaffeltypDto getStaffeltypDetails(long syssltyp)
        {
            StaffeltypDto rval = dao.getStaffeltypDetails(syssltyp);

            return rval;
        }

        /// <summary>
        /// Returns all Rolle Details
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        override public RolleDto getRolleDetails(long sysperole)
        {
            RolleDto rval = dao.getRolleDetails(sysperole);

            return rval;
        }

        /// <summary>
        /// Returns all Rollentyp Details
        /// </summary>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        override public RollentypDto getRollentypDetails(long sysroletype)
        {
            RollentypDto rval = dao.getRollentypDetails(sysroletype);

            return rval;
        }

        /// <summary>
        /// Returns all Handelsgruppe Details
        /// </summary>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        override public HandelsgruppeDto getHandelsgruppeDetails(long sysprhgroup)
        {
            HandelsgruppeDto rval = dao.getHandelsgruppeDetails(sysprhgroup);

            return rval;
        }

        /// <summary>
        /// Returns all Vertriebskanal Details
        /// </summary>
        /// <param name="sysbchannel"></param>
        /// <returns></returns>
        override public VertriebskanalDto getVertriebskanalDetails(long sysbchannel)
        {
            VertriebskanalDto rval = dao.getVertriebskanalDetails(sysbchannel);

            return rval;
        }

        /// <summary>
        /// Returns all Brand Details
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        override public BrandDto getBrandDetails(long sysbrand)
        {
            BrandDto rval = dao.getBrandDetails(sysbrand);

            return rval;
        }

        /// <summary>
        /// Returns all Rechnung Details
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        override public RechnungDto getRechnungDetails(long sysid)
        {
            RechnungDto rval = dao.getRechnungDetails(sysid);

            return rval;
        }

        /// <summary>
        /// Returns all Angobbrief Details
        /// </summary>
        /// <param name="sysangobbrief"></param>
        /// <returns></returns>
        override public AngobbriefDto getAngobbriefDetails(long sysangobbrief)
        {
            AngobbriefDto rval = dao.getAngobbriefDetails(sysangobbrief);

            return rval;
        }

        /// <summary>
        /// Returns all Zahlungsplan Details
        /// </summary>
        /// <param name="sysslpos"></param>
        /// <returns></returns>
        override public ZahlungsplanDto getZahlungsplanDetails(long sysslpos)
        {
            ZahlungsplanDto rval = dao.getZahlungsplanDetails(sysslpos);

            return rval;
        }

        /// <summary>
        /// Returns all Fahrzeugbrief Details
        /// </summary>
        /// <param name="sysobbrief"></param>
        /// <returns></returns>
        override public FahrzeugbriefDto getFahrzeugbriefDetails(long sysobbrief)
        {
            FahrzeugbriefDto rval = dao.getFahrzeugbriefDetails(sysobbrief);

            return rval;
        }

        /// <summary>
        /// Returns all Kalk Details
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        override public KalkDto getKalkDetails(long syskalk)
        {
            KalkDto rval = dao.getKalkDetails(syskalk);

            return rval;
        }

        /// <summary>
        /// Returns all Person Details
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        override public PersonDto getPersonDetails(long sysperson)
        {
            PersonDto rval = dao.getPersonDetails(sysperson);

            return rval;
        }

        /// <summary>
        /// Returns all or only one Indicator Value Details for the area
        /// updating necessary details if configured
        /// </summary>
        /// <param name="expdisp"></param>
        /// <returns></returns>
        override public List<ExpdispDto> getExpdispDetails(igetExpdispDto expdisp)
        {
            IWorkflowService ws = WorkflowServiceFactory.getInstance().getWorkflowService();
            //step one, check values, 
            //QUERYEXPCALCIDS - alle zu berechnenden exptypen + value id
            //value berechnen anhand exptyp-expression
            //wenn value id schon da, updaten
            //sonst neuen value
            //wenn archivflag, neuer archiveintrag
            List<ExpUpdDto> updates = null;
            if (expdisp.sysexptyp == 0)
                updates = dao.getExpUpdates(expdisp.area, expdisp.areaid);
            else
                updates = dao.getExpUpdate(expdisp.area, expdisp.areaid, expdisp.sysexptyp);
            WorkflowContext ctx = null;
            foreach (ExpUpdDto update in updates)
            {
                ExpvalDto expval = new ExpvalDto();
                expval.sysexpval = update.sysexpval.HasValue ? update.sysexpval.Value : 0;
                if (update.method == (int)ExptypMethod.USER)
                {
                    expval.val = expdisp.userValue;
                }
                else
                {
                    try
                    {
                        expval.val = Double.Parse(ws.evaluate(update.expression, expdisp.area, expdisp.areaid, ref ctx));
                    }
                    catch (Exception) { }
                }

                expval.crtdate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(DateTime.Now).Value;
                expval.crttime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                expval.area = expdisp.area;
                expval.sysid = expdisp.areaid;
                expval.sysexptyp = update.sysexptyp;

                dao.createOrUpdateExpval(expval);
                if (update.archivflag == 1)
                {
                    expval.sysexpval = 0;
                    dao.createOrUpdateExpvalar(expval);
                }
            }
            if (expdisp.sysexptyp == 0)
                return dao.getExpdisps(expdisp.area, expdisp.areaid);
            else
                return dao.getExpdisp(expdisp.area, expdisp.areaid, expdisp.sysexptyp);
        }

		/// <summary>
		/// Returns all or only one SLA Value Details for the sysid (Ang/Ant)
		/// </summary>
		/// <param name="slaid"></param>
		/// <returns></returns>
		override public List<SlaDto> getSlaDetails (igetSlaDto slaid)
		{
			List<SlaDto> listSLA = dao.getSlaDetails (slaid.sysid, slaid.isoCode);
			return listSLA;
			// return dao.getSlaDetails (slaid.sysid, slaid.isoCode);
		}

        /// <summary>
        /// Returns all Indicator Value Details for the area
        /// </summary>
        /// <param name="expdef"></param>
        /// <returns></returns>
        override public List<ExpdefDto> getExpdefDetails(igetExpdefDto expdef)
        {
            long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);

            //step one, check values, 
            //QUERYEXPCALCIDS - alle zu berechnenden exptypen + value id
            //value berechnen anhand exptyp-expression
            //wenn value id schon da, updaten
            //sonst neuen value
            //wenn archivflag, neuer archiveintrag
            ExptypDto exptype = dao.getExptype(expdef.area, 1);
            if (exptype == null)//no defaults defined for this area
                return new List<ExpdefDto>();

            List<long> newIds = new List<long>(expdef.areaids.AsEnumerable());
            List<ExpUpdDto> updates = dao.getExpDefUpdates(expdef.area, expdef.areaids);
            IWorkflowService ws = WorkflowServiceFactory.getInstance().getWorkflowService();
            WorkflowContext wctx = null;
            foreach (ExpUpdDto update in updates)
            {
                newIds.Remove(update.sysid);
                if (update.expired > 0)//now we get all, expired or not to find the ids that are not in db at all
                {
                    ExpvalDto expval = new ExpvalDto();
                    expval.sysexpval = update.sysexpval.HasValue ? update.sysexpval.Value : 0;
                    try
                    {
                        expval.val = Double.Parse(ws.evaluate(exptype.expression, expdef.area, update.sysid, ref wctx));
                    }
                    catch (Exception) { }

                    expval.crtdate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(DateTime.Now).Value;
                    expval.crttime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    expval.area = expdef.area;
                    expval.sysid = update.sysid;
                    expval.sysexptyp = exptype.sysexptyp;

                    dao.createOrUpdateExpval(expval);
                    if (update.archivflag == 1)
                    {
                        expval.sysexpval = 0;
                        dao.createOrUpdateExpvalar(expval);
                    }
                }

            }
            WorkflowContext ctx = null;
            foreach (long sysid in newIds)
            {
                ExpvalDto expval = new ExpvalDto();
                expval.sysexpval = 0;
                try
                {
                    expval.val = Double.Parse(ws.evaluate(exptype.expression, expdef.area, sysid, ref ctx));
                }
                catch (Exception) { }

                expval.crtdate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(DateTime.Now).Value;
                expval.crttime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                expval.area = expdef.area;
                expval.sysid = sysid;
                expval.sysexptyp = exptype.sysexptyp;

                dao.createOrUpdateExpval(expval);
                if (exptype.archivflag == 1)
                {
                    expval.sysexpval = 0;
                    dao.createOrUpdateExpvalar(expval);
                }
            }
            List<ExpdefDto> rval = dao.getExpdef(expdef.area, expdef.areaids);
            long duration = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds - start);
            _log.Debug("Duration of EXPDEF: " + duration);
            return rval;
        }

        /// <summary>
        /// Returns all Indicator Details for the area
        /// </summary>
        /// <param name="exptyp"></param>
        /// <returns></returns>
        override public List<ExptypDto> getExptypDetails(igetExptypDto exptyp)
        {
            List<ExptypDto> rval = dao.getExptypes(exptyp.area);
            List<ExprangeDto> ranges = dao.getExpranges(exptyp.area);

            foreach (ExptypDto ex in rval)
            {
                ex.ranges = (from e in ranges
                             where e.sysexptyp == ex.sysexptyp
                             select e).ToList();
            }
            return rval;
        }

        /// <summary>
        /// Returns all Rub Infos for Zusatzdaten Detail
        /// </summary>
        /// <param name="iRub"></param>
        /// <returns></returns>
        override public List<Cic.One.DTO.DdlkprubDto> getRubInfo(Cic.One.DTO.igetRubDto iRub)
        {
            List<Cic.One.DTO.DdlkpsposDto> eintraege = dao.getDdlkpspos(iRub.area, iRub.areaid);

            // Alle Rubriken
            List<Cic.One.DTO.DdlkprubDto> rubriken = dao.getDdlkprubs(iRub.rubcode, iRub.rubarea);

            if(rubriken!=null)
            foreach (Cic.One.DTO.DdlkprubDto rubrik in rubriken)
            {
                rubrik.cols = dao.getDdlkpcols(rubrik.getEntityId());
                foreach (Cic.One.DTO.DdlkpcolDto col in rubrik.cols)
                {
                    if (eintraege != null && eintraege.Count > 0)
                    {
                        var query = eintraege.Where(a => a.sysddlkpcol == col.entityId);
                        if (query.Count() > 0)
                        {
                            Cic.One.DTO.DdlkpsposDto value = query.Select(a => a).First();
                            value = new Cic.One.DTO.DdlkpsposDto(value);
                            col.value = value;
                            if (col.type == 2)
                            {
                                //col.value.sysddlkppos = query.Where(a => a.value.Equals(value.value)).Select(a=>a.sysddlkppos).First();
                                //set the first value for selectboxes
                            }
                        }
                    }
                    if (col.type == 2)
                    {
                        col.selectItems = dao.getDdlkppos(col.entityId);
                        if (col.selectItems!=null && col.value != null && col.selectItems.Count > 0)
                        {
                            Cic.One.DTO.DdlkpposDto foundItem = col.selectItems.Where(a => a.value.Equals(col.value.value)).FirstOrDefault();
                            if (foundItem != null)
                                col.value.sysddlkppos = foundItem.sysddlkppos;
                        }
                    }
                }
            }
            return rubriken;
        }

        /// <summary>
        /// Returns all ObjektDetails
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        override public ObjektDto getObjektDetails(long sysob)
        {
            return dao.getObjektDetails(sysob);
        }

        /// <summary>
        /// Returns all ObjektDetails
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public override ObtypDto getObtypDetails(long sysobtyp)
        {
            return dao.getObtypDetails(sysobtyp);
        }

        /// <summary>
        /// Returns all Obkat Details
        /// </summary>
        /// <param name="sysobkat"></param>
        /// <returns></returns>
        override public ObkatDto getObkatDetails(long sysobkat)
        {
            return dao.getObkatDetails(sysobkat);
        }

        /// <summary>
        /// Returns all RecalcDetails
        /// </summary>
        /// <param name="sysrecalc"></param>
        /// <returns></returns>
        override public RecalcDto getRecalcDetails(long sysrecalc)
        {
            return dao.getRecalcDetails(sysrecalc);
        }

        /// <summary>
        /// Returns all Mycalc Details
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        override public MycalcDto getMycalcDetails(long sysmycalc)
        {
            return dao.getMycalcDetails(sysmycalc);
        }

        /// <summary>
        /// Returns all RahmenDetails
        /// </summary>
        /// <param name="sysrvt"></param>
        /// <returns></returns>
        override public RahmenDto getRahmenDetails(long sysrvt)
        {
            return dao.getRahmenDetails(sysrvt);
        }

        /// <summary>
        /// Returns all HaendlerDetails
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        override public HaendlerDto getHaendlerDetails(long sysperson)
        {
            return dao.getHaendlerDetails(sysperson);
        }

        /// <summary>
        /// Returns all KundeDetails
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        override public KundeDto getKundeDetails(long sysperson)
        {
            return dao.getKundeDetails(sysperson);
        }

        /// <summary>
        /// Returns all LogDumpDetails
        /// </summary>
        /// <param name="LogDump"></param>
        /// <returns></returns>
        override public LogDumpDto getLogDumpDetails(long LogDump)
        {
            return dao.getLogDumpDetails(LogDump);
        }

        /// <summary>
        /// Returns all ItDetails
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        override public ItDto getItDetails(long sysit)
        {
            ItDto rval = dao.getItDetails(sysit);
            assignIndicatorDefault(rval);
            return rval;
        }

        /// <summary>
        /// Returns all ItkontoDetails
        /// </summary>
        /// <param name="sysitkonto"></param>
        /// <returns></returns>
        override public ItkontoDto getItkontoDetails(long sysitkonto)
        {
            return dao.getItkontoDetails(sysitkonto);
        }

        /// <summary>
        /// Returns all itadresseDetails
        /// </summary>
        /// <param name="sysitadresse"></param>
        /// <returns></returns>
        override public ItadresseDto getItadresseDetails(long sysitadresse)
        {
            return dao.getItadresseDetails(sysitadresse);
        }

        /// <summary>
        /// Returns all AngvarDetails
        /// </summary>
        /// <param name="angvar"></param>
        /// <returns></returns>
        override public AngvarDto getAngvarDetails(long sysangvar)
        {
            return dao.getAngvarDetails(sysangvar);
        }

        /// <summary>
        /// Returns all AngobDetails
        /// </summary>
        /// <param name="angob"></param>
        /// <returns></returns>
        override public AngobDto getAngobDetails(long sysangob)
        {
            return dao.getAngobDetails(sysangob);
        }


        /// <summary>
        /// Returns  ObDto Details
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        override public ObDto getObDetails(long sysob)
        {
            return dao.getObDetails(sysob);
        }

        /// <summary>
        /// Returns all AngkalkDetails
        /// </summary>
        /// <param name="angkalk"></param>
        /// <returns></returns>
        override public AngkalkDto getAngkalkDetails(long sysangkalk)
        {
            return dao.getAngkalkDetails(sysangkalk);
        }

        /// <summary>
        /// Returns all AntkalkDetails
        /// </summary>
        /// <param name="antkalk"></param>
        /// <returns></returns>
        override public AntkalkDto getAntkalkDetails(long sysantkalk)
        {
            return dao.getAntkalkDetails(sysantkalk);
        }

        /// <summary>
        /// Returns all OpportunityDetails
        /// </summary>
        /// <param name="sysopportunity"></param>
        /// <returns></returns>
        override public OpportunityDto getOpportunityDetails(long sysopportunity)
        {
            return dao.getOpportunityDetails(sysopportunity);
        }

        /// <summary>
        /// Returns all OppotaskDetails
        /// </summary>
        /// <param name="sysOppotask"></param>
        /// <returns></returns>
        override public OppotaskDto getOppotaskDetails(long sysOppotask)
        {
            return dao.getOppotaskDetails(sysOppotask);
        }

        /// <summary>
        /// Returns all ContactDetails
        /// </summary>
        /// <param name="syscontact"></param>
        /// <returns></returns>
        override public ContactDto getContactDetails(long syscontact)
        {
            return dao.getContactDetails(syscontact);
        }

        /// <summary>
        /// Returns all KontoDetails
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        override public KontoDto getKontoDetails(long syskonto)
        {
            return dao.getKontoDetails(syskonto);
        }

        /// <summary>
        /// Returns all CampDetails
        /// </summary>
        /// <param name="syscamp"></param>
        /// <returns></returns>
        override public CampDto getCampDetails(long syscamp)
        {
            return dao.getCampDetails(syscamp);
        }

        /// <summary>
        /// Returns all WfuserDetails
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        override public WfuserDto getWfuserDetails(long syswfuser)
        {
            return dao.getWfuserDetails(syswfuser);
        }

        /// <summary>
        /// Returns all FinanzierungDetails
        /// </summary>
        /// <param name="sysnkk"></param>
        /// <returns></returns>
        override public FinanzierungDto getFinanzierungDetails(long sysnkk)
        {
            return dao.getFinanzierungDetails(sysnkk);
        }

        override public KreditlinieDto getKreditlinieDetail(long sysklinie)
        {
            return dao.getKreditlinieDetail(sysklinie);
        }

        /// <summary>
        /// Returns all AdresseDetails
        /// </summary>
        /// <param name="sysadresse"></param>
        /// <returns></returns>
        override public AdresseDto getAdresseDetails(long sysadresse)
        {
            return dao.getAdresseDetails(sysadresse);
        }


        /// <summary>
        /// Returns the Eaihot
        /// </summary>
        /// <param name="syseaihot"></param>
        /// <returns></returns>
        public override Cic.One.DTO.EaihotDto getEaihotDetails(long syseaihot)
        {
            return dao.getEaihotDetails(syseaihot);
        }

        /// <summary>
        /// Returns AdmaddDto Details
        /// </summary>
        /// <param name="sysAdmadd"></param>
        /// <returns></returns>
        override public AdmaddDto getAdmaddDetail(long sysAdmadd)
        {
            return dao.getAdmaddDetail(sysAdmadd);
        }



        /// <summary>
        /// Returns all PtaskDetails
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        override public PtaskDto getPtaskDetails(long sysptask)
        {
            return dao.getPtaskDetails(sysptask);
        }

        /// <summary>
        /// Returns all ApptmtDetails
        /// </summary>
        /// <param name="sysapptmt"></param>
        /// <returns></returns>
        override public ApptmtDto getApptmtDetails(long sysapptmt)
        {
            return dao.getApptmtDetails(sysapptmt);
        }

        /// <summary>
        /// Returns all ReminderDetails
        /// </summary>
        /// <param name="sysreminder"></param>
        /// <returns></returns>
        override public ReminderDto getReminderDetails(long sysreminder)
        {
            return dao.getReminderDetails(sysreminder);
        }

        /// <summary>
        /// Returns all Mailmsg Details
        /// </summary>
        /// <param name="sysmailmsg"></param>
        /// <returns></returns>
        override public MailmsgDto getMailmsgDetails(long sysmailmsg)
        {
            return dao.getMailmsgDetails(sysmailmsg);
        }

        /// <summary>
        /// Returns all Memo Details
        /// </summary>
        /// <param name="syswfmmemo"></param>
        /// <returns></returns>
        override public MemoDto getMemoDetails(long syswfmmemo)
        {
            return dao.getMemoDetails(syswfmmemo);
        }

        /// <summary>
        /// Returns all Prun Details
        /// </summary>
        /// <param name="sysprun"></param>
        /// <returns></returns>
        override public PrunDto getPrunDetails(long sysprun)
        {
            return dao.getPrunDetails(sysprun);
        }

        /// <summary>
        /// Returns all Fileatt Details
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        override public FileattDto getFileattDetails(long sysfileatt)
        {
            return dao.getFileattDetails(sysfileatt);
        }

        /// <summary>
        /// Returns all Fileatt Details by entity
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        override public FileattDto getFileattDetails(string area, long sysid)
        {
            return dao.getFileattDetails(area, sysid);
        }


        /// <summary>
        /// Returns all Dmsdoc Details
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        override public DmsdocDto getDmsdocDetails(long sysdmsdoc)
        {
            return dao.getDmsdocDetails(sysdmsdoc);
        }

        /// <summary>
        /// Returns all Dmsdoc Details by entity
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        override public List<DmsdocDto> getDmsdocDetails(string area, long sysid)
        {
            return dao.getDmsdocDetails(area, sysid);
        }

        /// <summary>
        /// Returns all Prproduct Details
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        override public PrproductDto getPrproductDetails(long sysprproduct)
        {
            return dao.getPrproductDetails(sysprproduct);
        }

        /// <summary>
        /// Returns all Itemcat Details
        /// </summary>
        /// <param name="sysitemcat"></param>
        /// <returns></returns>
        override public ItemcatDto getItemcatDetails(long sysitemcat)
        {
            return dao.getItemcatDetails(sysitemcat);
        }

        /// <summary>
        /// Returns all Itemcat Details
        /// </summary>
        /// <param name="sysctlang"></param>
        /// <returns></returns>
        override public CtlangDto getCtlangDetails(long sysctlang)
        {
            return dao.getCtlangDetails(sysctlang);
        }

        /// <summary>
        /// Returns all Land Details
        /// </summary>
        /// <param name="sysland"></param>
        /// <returns></returns>
        override public LandDto getLandDetails(long sysland)
        {
            return dao.getLandDetails(sysland);
        }

        /// <summary>
        /// Returns all Branche Details
        /// </summary>
        /// <param name="sysbranche"></param>
        /// <returns></returns>
        override public BrancheDto getBrancheDetails(long sysbranche)
        {
            return dao.getBrancheDetails(sysbranche);
        }

        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        override public AccountDto getAccountDetails(String area, long sysaccount)
        {
            if (!"IT".Equals(area))
            { 
                AccountDto rval= getAccountDetails(sysaccount);
                rval.sysid = sysaccount;
                if (rval.syskdtyp == 1)
                    rval.privatFlag = 1;
                return rval;
            }

            AccountDto rval2 = Mapper.Map<ItDto,AccountDto>(dao.getItDetails(sysaccount));
            rval2.area = "IT";
            rval2.sysid = sysaccount;
            if (rval2.syskdtyp == 1)
                rval2.privatFlag = 1;
            return rval2;
        }

        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        override public ogetFinanzDatenDto getFinanzdatenByArea(long syskd, string area, long sysid)
        {
            return dao.getFinanzdatenByArea(syskd, area, sysid);

        }

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        override public ogetZusatzdaten getZusatzdatenDetail(long syskd, long sysantrag)
        {
            return dao.getZusatzdatenDetail(syskd, sysantrag);
        }

        /// <summary>
        /// delivers the pkz or ukz for the it or person 
        /// optionally for the subarea like angebot/antrag and its id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="subarea"></param>
        /// <param name="subsysid"></param>
        /// <returns></returns>
        override public ogetZusatzdaten getZusatzdatenDetail(String area, long sysid, String subarea, long subsysid)
        {
            return dao.getZusatzdatenDetail(area,sysid,subarea,subsysid);
        }

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        override public ogetZusatzdaten getZusatzdatenDetailByAngebot(long sysit, long sysangebot)
        {
            return dao.getZusatzdatenDetailByAngebot(sysit, sysangebot);
        }

        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        override public AccountDto getAccountDetails(long sysaccount)
        {
            AccountDto rval = dao.getAccountDetails(sysaccount);
            rval.area = "PERSON";

            if (casBo != null)
            {
                iCASEvaluateDto casinput = new iCASEvaluateDto();
                casinput.area = "PERSON";
                casinput.expression = new String[3] { CASBo.OP, CASBo.OBLIGO, CASBo.OBLIGOKWG };
                casinput.sysID = new long[1] { rval.entityId };

                CASEvaluateResult[] casResults = casBo.getEvaluation(casinput, getSysWfUser());

                try
                {
                    if (casResults[0].clarionResult[0] != null && !"".Equals(casResults[0].clarionResult[0]))
                        rval.op = Double.Parse(casResults[0].clarionResult[0], CultureInfo.InvariantCulture);
                    if (casResults[0].clarionResult[1] != null && !"".Equals(casResults[0].clarionResult[1]))
                        rval.obligo = Double.Parse(casResults[0].clarionResult[1], CultureInfo.InvariantCulture);
                    if (casResults[0].clarionResult[2] != null && !"".Equals(casResults[0].clarionResult[2]))
                        rval.obligokwg = Double.Parse(casResults[0].clarionResult[2], CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    _log.Warn("CAS-Evaluation failed for Account op/obligo", e);
                }

            }
            assignIndicatorDefault(rval);
            return rval;
        }

        /// <summary>
        /// Returns all WktAccount Details
        /// </summary>
        /// <param name="syswkt"></param>
        /// <returns></returns>
        override public WktaccountDto getWktAccountDetails(long syswktaccount)
        {
            WktaccountDto rval = dao.getWktAccountDetails(syswktaccount);
            return rval;
        }

        /// <summary>
        /// Returns all  Partner Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        override public PartnerDto getPartnerDetails(long sysaccount)
        {
            PartnerDto rval = dao.getPartnerDetails(sysaccount);
            assignIndicatorDefault(rval);
            return rval;
        }

        /// <summary>
        /// Returns all  Beteiligter Details
        /// </summary>
        /// <param name="sysbeteiligter"></param>
        /// <returns></returns>
        override public BeteiligterDto getBeteiligterDetails(long sysbeteiligter)
        {
            return dao.getBeteiligterDetails(sysbeteiligter);
        }

        /// <summary>
        /// Returns all Adrtp Details
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        override public AdrtpDto getAdrtpDetails(long sysadrtp)
        {
            return dao.getAdrtpDetails(sysadrtp);
        }

        /// <summary>
        /// Returns Strasse Details
        /// </summary>
        /// <param name="systrasse"></param>
        /// <returns></returns>
        override public StrasseDto getStrasseDetails(long systrasse)
        {

            return dao.getStrasseDetails(systrasse);
        }

        /// <summary>
        /// Returns PUser Details
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public override PuserDto getPuserDetails(long syswfuser)
        {
            return dao.getPuserDetails(syswfuser);
        }

        /// <summary>
        /// Returns all Kontotp Details
        /// </summary>
        /// <param name="syskontotp"></param>
        /// <returns></returns>
        override public KontotpDto getKontotpDetails(long syskontotp)
        {
            return dao.getKontotpDetails(syskontotp);
        }

        /// <summary>
        /// Returns all Blz Details
        /// </summary>
        /// <param name="sysbranche"></param>
        /// <returns></returns>
        override public BlzDto getBlzDetails(long sysblz)
        {
            return dao.getBlzDetails(sysblz);
        }

        /// <summary>
        /// Returns all Ptrelate Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        override public PtrelateDto getPtrelateDetails(long sysptrelate)
        {
            return dao.getPtrelateDetails(sysptrelate);
        }


        /// <summary>
        /// Returns all Wfexec Details
        /// </summary>
        /// <param name="syswfexec"></param>
        /// <returns></returns>
        override public WfexecDto getWfexecDetails(long syswfexec)
        {
            return dao.getWfexecDetails(syswfexec);
        }
        /// <summary>
        /// Returns all Crmnm Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        override public CrmnmDto getCrmnmDetails(long syscrmnm)
        {
            return dao.getCrmnmDetails(syscrmnm);
        }

        /// <summary>
        /// Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        override public Cic.One.DTO.DdlkprubDto getDdlkprubDetails(long sysddlkprub)
        {
            return dao.getDdlkprubDetails(sysddlkprub);
        }

        /// <summary>
        /// Returns all Ddlkpcol Details
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        override public Cic.One.DTO.DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol)
        {
            return dao.getDdlkpcolDetails(sysddlkpcol);
        }

        /// <summary>
        /// Returns all Ddlkppos Details
        /// </summary>
        /// <param name="sysddlkppos"></param>
        /// <returns></returns>
        override public Cic.One.DTO.DdlkpposDto getDdlkpposDetails(long sysddlkppos)
        {
            return dao.getDdlkpposDetails(sysddlkppos);
        }

        /// <summary>
        /// Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        override public Cic.One.DTO.DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos)
        {
            return dao.getDdlkpsposDetails(sysddlkpspos);
        }

        /// <summary>
        /// Returns all camptp Details
        /// </summary>
        /// <param name="syscamptp"></param>
        /// <returns></returns>
        override public CamptpDto getCamptpDetails(long syscamptp)
        {
            return dao.getCamptpDetails(syscamptp);
        }

        /// <summary>
        /// Returns all zinstab Details
        /// </summary>
        /// <param name="syszinstab"></param>
        /// <returns></returns>
        override public ZinstabDto getZinstabDetails(long syszinstab)
        {
            return dao.getZinstabDetails(syszinstab);
        }

        /// <summary>
        /// Returns all Oppotp Details
        /// </summary>
        /// <param name="sysoppotp"></param>
        /// <returns></returns>
        override public OppotpDto getOppotpDetails(long sysoppotp)
        {
            return dao.getOppotpDetails(sysoppotp);
        }

        /// <summary>
        /// Returns all crmpr Details
        /// </summary>
        /// <param name="syscrmpr"></param>
        /// <returns></returns>
        override public CrmprDto getCrmprDetails(long syscrmpr)
        {
            return dao.getCrmprDetails(syscrmpr);
        }

        /// <summary>
        /// Returns all contacttp Details
        /// </summary>
        /// <param name="syscontacttp"></param>
        /// <returns></returns>
        override public ContacttpDto getContacttpDetails(long syscontacttp)
        {
            return dao.getContacttpDetails(syscontacttp);
        }

        /// <summary>
        /// Returns all itemcatm Details
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        override public ItemcatmDto getItemcatmDetails(long sysitemcatm)
        {
            return dao.getItemcatmDetails(sysitemcatm);
        }

        /// <summary>
        /// Returns all recurr Details
        /// </summary>
        /// <param name="sysrecurr"></param>
        /// <returns></returns>
        override public RecurrDto getRecurrDetails(long sysrecurr)
        {
            return dao.getRecurrDetails(sysrecurr);
        }

        /// <summary>
        /// Returns all Ptype Details
        /// </summary>
        /// <param name="sysptype"></param>
        /// <returns></returns>
        override public PtypeDto getPtypeDetails(long sysptype)
        {
            return dao.getPtypeDetails(sysptype);
        }

        /// <summary>
        /// Returns all Prunstep Details
        /// </summary>
        /// <param name="sysprunstep"></param>
        /// <returns></returns>
        override public PrunstepDto getPrunstepDetails(long sysprunstep)
        {
            return dao.getPrunstepDetails(sysprunstep);
        }

        /// <summary>
        /// Returns all pstep Details
        /// </summary>
        /// <param name="syspstep"></param>
        /// <returns></returns>
        override public PstepDto getPstepDetails(long syspstep)
        {
            return dao.getPstepDetails(syspstep);
        }

        /// <summary>
        /// Returns all Prkgroup Details
        /// </summary>
        /// <param name="sysPrkgroup"></param>
        /// <returns></returns>
        override public PrkgroupDto getPrkgroupDetails(long sysPrkgroup)
        {
            return dao.getPrkgroupDetails(sysPrkgroup);
        }

        /// <summary>
        /// Returns all Prkgroupm Details
        /// </summary>
        /// <param name="sysPrkgroupm"></param>
        /// <returns></returns>
        override public PrkgroupmDto getPrkgroupmDetails(long sysPrkgroupm)
        {
            return dao.getPrkgroupmDetails(sysPrkgroupm);
        }

        /// <summary>
        /// Returns all Prkgroupz Details
        /// </summary>
        /// <param name="sysPrkgroupz"></param>
        /// <returns></returns>
        override public PrkgroupzDto getPrkgroupzDetails(long sysPrkgroupz)
        {
            return dao.getPrkgroupzDetails(sysPrkgroupz);
        }

        /// <summary>
        /// Returns all Prkgroups Details
        /// </summary>
        /// <param name="sysPrkgroups"></param>
        /// <returns></returns>
        override public PrkgroupsDto getPrkgroupsDetails(long sysPrkgroups)
        {
            return dao.getPrkgroupsDetails(sysPrkgroups);
        }

        /// <summary>
        /// Returns all Seg Details
        /// </summary>
        /// <param name="sysSeg"></param>
        /// <returns></returns>
        override public SegDto getSegDetails(long sysSeg)
        {
            return dao.getSegDetails(sysSeg);
        }

        /// <summary>
        /// Returns all Segc Details
        /// </summary>
        /// <param name="sysSegc"></param>
        /// <returns></returns>
        override public SegcDto getSegcDetails(long sysSegc)
        {
            return dao.getSegcDetails(sysSegc);
        }

        /// <summary>
        /// Returns all Stickynote Details
        /// </summary>
        /// <param name="sysSegc"></param>
        /// <returns></returns>
        override public StickynoteDto getStickynoteDetails(long sysStickynote)
        {
            return dao.getStickynoteDetails(sysStickynote);
        }

        /// <summary>
        /// Returns all Stickytype Details
        /// </summary>
        /// <param name="sysSegc"></param>
        /// <returns></returns>
        override public StickytypeDto getStickytypeDetails(long sysStickytype)
        {
            return dao.getStickytypeDetails(sysStickytype);
        }

        /// <summary>
        /// Returns all Wfsignature Details
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override WfsignatureDto getWfsignatureDetail(long input)
        {
            return dao.getWfsignatureDetail(input);
        }



        /// <summary>
        /// Returns Angebot Details
        /// </summary>
        /// <param name="sysAngebot></param>
        /// <returns></returns>
        override public AngebotDto getAngebotDetails(long sysAngebot)
        {
            return dao.getAngebotDetails(sysAngebot);
        }

        /// <summary>
        /// returns product details for angebot
        /// </summary>
        /// <param name="sysang"></param>
        /// <returns></returns>
        override public ProduktInfoDto getProduktInfoAngebotDetails(long sysang)
        {
            return dao.getProduktInfoAngebotDetails(sysang);
        }

        /// <summary>
        /// returns product details for antrag
        /// </summary>
        /// <param name="sysant"></param>
        /// <returns></returns>
        override public ProduktInfoDto getProduktInfoAntragDetails(long sysant)
        {
         
           return dao.getProduktInfoAntragDetails(sysant);
            
        }

        /// <summary>
        /// Returns Angebot Details
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        override public BNAngebotDto getBNAngebotDetails(long sysAngebot)
        {
            return null;
        }

        /// <summary>
        /// Returns Antrag Details
        /// </summary>
        /// <param name="sysAntrag"></param>
        /// <returns></returns>
        override public BNAntragDto getBNAntragDetails(long sysAntrag)
        {
            return null;
        }


        /// <summary>
        /// Returns Antrag Details
        /// </summary>
        /// <param name="sysAntrag></param>
        /// <returns></returns>
        override public AntragDto getAntragDetails(long sysAntrag)
        {
            return dao.getAntragDetails(sysAntrag);
        }


        /// <summary>
        /// Returns Vertrag Details
        /// </summary>
        /// <param name="sysAngebot></param>
        /// <returns></returns>
        override public VertragDto getVertragDetails(long sysVertrag)
        {
            VertragDto rval = dao.getVertragDetails(sysVertrag);
            if (casBo != null)
            {
                /*iCASEvaluateDto casinput = new iCASEvaluateDto();
                casinput.area = "VT";
                casinput.expression = new String[1] { CASBo.OP_VT };
                casinput.sysID = new long[1] { rval.entityId };

                CASEvaluateResult[] casResults = casBo.getEvaluation(casinput, getSysWfUser());

                try
                {
                    if (casResults[0].clarionResult[0] != null && !"".Equals(casResults[0].clarionResult[0]))
                        rval.op = Double.Parse(casResults[0].clarionResult[0], CultureInfo.InvariantCulture);

                }
                catch (Exception e)
                {
                    _log.Warn("CAS-Evaluation failed for VT op", e);
                }*/

            }
            return rval;
        }

        /// <summary>
        /// Returns Vorgang Details
        /// </summary>
        /// <param name="sysAngebot></param>
        /// <returns></returns>
        override public VorgangDto getVorgangDetails(long sysId, string area)
        {
            return dao.getVorgangDetails(sysId, area);
        }

        /// <summary>
        /// Returns all Nkonto Details
        /// </summary>
        /// <param name="sysnkonto"></param>
        /// <returns></returns>
        override public NkontoDto getNkontoDetails(long sysnkonto)
        {
            return dao.getNkontoDetails(sysnkonto);
        }

        /// <summary>
        /// Returns all Printset Details
        /// </summary>
        /// <param name="sysprintset"></param>
        /// <returns></returns>
        override public PrintsetDto getPrintsetDetails(long sysprintset)
        {
            return dao.getPrintsetDetails(sysprintset);
        }

        /// <summary>
        /// Returns all Prtlgset Details
        /// </summary>
        /// <param name="sysprtlgset"></param>
        /// <returns></returns>
        override public PrtlgsetDto getPrtlgsetDetails(long sysprtlgset)
        {
            return dao.getPrtlgsetDetails(sysprtlgset);
        }

        /// <summary>
        /// Returns all ZEK requests
        /// </summary>
        /// <param name="syszek">primary key</param>
        /// <returns></returns>
        override public ZekDto getZek(long syszek)
        {
            return dao.getZek(syszek);
        }

        /// <summary>
        /// returns process data
        /// </summary>
        /// <param name="sysprocess">process id</param>
        /// <returns>process data</returns>
        public override ProcessDto getProcess(long sysprocess)
        {
            return dao.getProcess(sysprocess);
        }

        /// <summary>
        /// WaehrungDto 
        /// </summary>
        /// <param name="sysWaehrung"></param>
        /// <returns></returns>
        public override WaehrungDto getWaehrungDetails(long? sysWaehrung)
        {
            return dao.getWaehrungDetail(sysWaehrung);
        }



        #endregion GET

        /// <summary>
        /// Sucht Appointments inklusive den Recurrences
        /// </summary>
        /// <param name="search">Parameter</param>
        /// <returns></returns>
        public override List<ApptmtDto> searchApptmts(iSearchApptmtsWithRecurrDto search)
        {
            return new RecurrDao().searchApptmts(search, SysPerole);
        }

        /// <summary>
        /// Löscht eine Entity
        /// </summary>
        /// <param name="area">Area</param>
        /// <param name="sysid">Sysid</param>
        public override void deleteEntity(string area, long sysid)
        {
            dao.deleteEntity(area, sysid);
        }

        public override void updateLegitimationMethode(long sysangebot, long syswfuser, long sysit, string legitimationMethode)
        {

            dao.updateLegitimationMethode(sysangebot, syswfuser, sysit, legitimationMethode);
        }

        /// <summary>
        /// update abwicklungsort for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public override bool updateAbwicklungsort(iupdateAbwicklungsortDto input)
        {
            return dao.updateAbwicklungsort(input);
        }

        /// <summary>
        /// update smstext for ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public override bool updateSMSText(iupdateSMSTextDto input)
        {
            return dao.updateSMSText(input);
        }

        /// <summary>
        /// get anciliary details for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public override ogetAnciliaryDetailDto getAnciliaryDetail(igetAnciliaryDetailDto input)
        {
            return dao.getAnciliaryDetail(input);
        }

        /// <summary>
        /// accept EPOS Conditions of current user
        /// </summary>
        public override void acceptEPOSConditions()
        {
            dao.acceptEPOSConditions();
        }

        /// <summary>
        /// Returns the document validation Details
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override DokvalDto getDokvalDetails(String area,long sysId)
        {
            DokvalDto rval = new DokvalDto();

            CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[1];
            vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
            vars[0].LookupVariableName = "SPRACHE";
            vars[0].VariableName = "GUI";
            vars[0].Value = dao.getISOLanguage();

            String queueName = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "VALIDATION", "VALIQUEUE", "TEST_QUEUE_250");


            QueueResultDto qr = BOS.getQueueData(sysId, area, new String[] { "VALI_DATA" }, queueName, vars, sysWfUser);
            rval.data = qr.queues[0];

            rval.area = area;
            rval.sysid = sysId;
            return rval;
        }

        /// <summary>
        /// Returns the checklist art
        /// </summary>
        /// <param name="sysprunart"></param>
        /// <returns></returns>
        public override PrunartDto getPrunartDetails(long sysprunart)
        {
            return dao.getPrunartDetails(sysprunart);
        }

        /// <summary>
        /// Returns the checklist data for the antrag
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override ChklistDto getChklistDetails(igetChecklistDetailDto input)
        {


            return dao.getChklistDetails(input);
           
            
        }



        /// <summary>
        /// updates/creates Checklist data
        /// </summary>
        /// <param name="chklist"></param>
        /// <returns></returns>
        public override ChklistDto createOrUpdateChklist(ChklistDto chklist)
        {
            if (chklist.sysid == 0)
            {
                _log.Warn("Call of createOrUpdateChklist without primary key");
                return chklist;
            }

            return dao.createOrUpdateChklist(chklist);

           
        }

        /// <summary>
        /// updates/creates prunart data
        /// </summary>
        /// <param name="prunart"></param>
        /// <returns></returns>
        public override PrunartDto createOrUpdatePrunart(PrunartDto prunart)
        {
            return dao.createOrUpdatePrunart(prunart);
        }

        public override osendRiskmailDto sendRiskmail(isendRiskmailDto input)
        {
            return null;
        }

        /// <summary>
        /// Performs the 4-eyes principle for the area/id setting the given result of the current user
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public override void fourEyesPrinciple(ifourEyesDto input, ofourEyesDto output)
        {
            dao.fourEyesPrinciple(input, output);
        }
    
    }
}
