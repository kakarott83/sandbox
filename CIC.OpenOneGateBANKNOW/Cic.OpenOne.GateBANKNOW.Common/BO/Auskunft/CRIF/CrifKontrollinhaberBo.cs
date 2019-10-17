
namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF
{
    using AutoMapper;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.GateBANKNOW.Common.DTO;
    using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
    using CIC.Database.OL.EF6.Model;
    using CrifSoapService;
    using DAO;
    using DAO.Auskunft;
    using Devart.Data.Oracle;
    using DTO.Auskunft.Crif;
    using OpenOne.Common.DAO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages the retrieval of Kontrollinhaber from CRIF
    /// does NEVER write anything to IT or ITKNE, that will be done from outside using the new methods of this BO working on the CF-Data 
    /// 
    /// </summary>
    public class CrifKontrollinhaberBo : AbstractCrifBo<TypeGetReportRequest,TypeGetReportResponse>
    {
        
        private IMappingService kdTypMapping;
        private IMappingService landMapping;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crifWsDao"></param>
        /// <param name="crifDbDao"></param>
        /// <param name="auskunftDao"></param>
        /// <param name="auskunftTyp">If null or empty, CrifGetReport type is used</param>
        public CrifKontrollinhaberBo(ICrifWSDao crifWsDao, ICrifDBDao crifDbDao, IAuskunftDao auskunftDao, string auskunftTyp = null)
            : base(crifWsDao, crifDbDao, auskunftDao, string.IsNullOrEmpty(auskunftTyp)? AuskunfttypDao.CrifKontrollinhaber: auskunftTyp)
        {
            
            this.kdTypMapping = new DbMappingService<OLContext>(new ContextFactory(), (context) => ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<KeyValueLabel>("select syskdtyp as key, typ as value from kdtyp").AsQueryable());
            this.landMapping = new DbMappingService<OLContext>(new ContextFactory(), (context) => ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<KeyValueLabel>("select sysland as key, iso as value from land").AsQueryable());
            
        }

        /// <summary>
        /// Uses the current data of the given ANTRAG to determine the Auskunft-Inputdatam, perform the enquiry and fill the CF-Tables
        /// see 2.3.2.1
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            if (area.ToUpper() != "ANTRAG")
            {
                throw new ArgumentException(string.Format("The area '{0}' is not supported. Only 'ANTRAG' is currently supported.", area.ToUpper()));
            }

            ANTRAG antrag;
            CrifInDto inDto=null;

            using (DdOlExtended context = new DdOlExtended())
            {
                antrag = context.ANTRAG.FirstOrDefault(a => a.SYSID == sysId);
                if (antrag == null)
                {
                    throw new ArgumentException(string.Format("'{0}' with SYSID '{1}' was not found.", area.ToUpper(), sysId));
                }

               /* if (antrag.SYSKD > 0)//currently Kontrollinhaber has to work only via IT
                {
                    var person = context.PERSON.Single(a => a.SYSPERSON == antrag.SYSKD);
                    inDto = GetInDto(person.NAME, person.STRASSE, person.HSNR, person.PLZ, person.ORT, person.SYSLAND, person.SYSKDTYP);
                }
                else*/
                {
                    var it = context.IT.Single(a => a.SYSIT == antrag.SYSIT);
                    //adressmatch bereits gespeichert, diese Adresse für Kontrollinhaberanfrage verwenden
                    if (!String.IsNullOrEmpty(it.CRRSID))
                    {
                        inDto = new CrifInDto()
                        {
                            GetReport = new CrifGetReportInDto()
                            {
                                reportType = "CONTROL_PERSON_BUSINESS",
                                targetReportFormatSpecified = true,
                                targetReportFormat = TargetReportFormat.NONE,
                                identifier = new Identifier()
                                {
                                    identifierText = it.CRRSID,
                                    identifierType = IdentifierType.ADDRESS_ID

                                }

                            }
                        };
                      /*  //die korrekte letzte Auskunft ermitteln, welche die Adress-Id enthalten hatte, um diese Adresse zu verwenden

                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysId });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "adid", Value = it.CRRSID });

                            KundeDto itadr = context.ExecuteStoreQuery<KundeDto>(@"select l.sysland, a.street strasse, a.housenumber hsnr, a.zip plz, a.city ort, a.companyname name
                                    from auskunft,auskunfttyp,
                                    cfoutgetreport o, 
                                    cfaddress a
                                    left join land l on a.country = l.ahskennzeichen
                                    left join staat s on substr(a.regionCode, 4) = s.code, 
                                    cfcandidate c
                                    left join cfkeyvalue k on k.area = 'CFCANDIDATE' and k.keyval = 'ADDRESS_ID' and k.sysid = c.syscfcandidate
                                    where 
                                    auskunfttyp.sysauskunfttyp=auskunft.sysauskunfttyp and auskunfttyp.bezeichnung='CrifKontrollinhaber' and auskunft.area='ANTRAG' and auskunft.sysid=:sysid
                                    and o.sysauskunft = auskunft.sysauskunft
                                    and c.syscfaddressmatch = o.syscfaddressmatch 
                                    and dbms_lob.compare(value, to_clob(:adid)) = 0
                                    and a.syscfaddress = c.syscfaddress order by auskunft.sysauskunft desc", parameters.ToArray()).FirstOrDefault();
                            if (itadr != null && itadr.name != null)
                            {
                                inDto = GetInDto(itadr.name, itadr.strasse, itadr.hsnr, itadr.plz, itadr.ort, itadr.sysland, it.SYSKDTYP);
                            }*/
                    }
                    //falls keine Adresse gefunden, die des Hauptinteressenten verwenden
                    if(inDto==null)
                    {
                        inDto = GetInDto(it.NAME, it.STRASSE, it.HSNR, it.PLZ, it.ORT, it.SYSLAND, it.SYSKDTYP);
                    }
                }
            }


            var auskunft = doAuskunft(inDto);
            this.auskunftDao.setAuskunfAreaUndId(auskunft.sysAuskunft, area.ToUpper(), sysId);

            KontrollinhaberStatus aktStatus = getExtendedAuskunftStatus(auskunft.sysAuskunft);
            short statusnum = 1;
            //Auskunft ist "gültig" wenn kein technischer Fehler vorliegt und eine reproduzierbare Antwort von CRIF erhalten wird, die keine weiteren INputs benötigt (bei LIST wird. z.B. eine andere Adresse erwartet)
            if (long.Parse(auskunft.Fehlercode)>=0 && (aktStatus == KontrollinhaberStatus.FOUND || aktStatus == KontrollinhaberStatus.NO_COMP_FOUND_CRIF || aktStatus == KontrollinhaberStatus.NO_DATA_CRIF))
                statusnum = 0;

            this.auskunftDao.setAuskunfStatusNum(auskunft.sysAuskunft, statusnum);
            return auskunft;
        }

        private CrifInDto GetInDto(string personName, string personStrasse, string personHsnr, string personPlz, string personOrt, long? personSysland, long? personSyskdtyp)
        {
            var kdTyp = kdTypMapping.GetLongValue((personSyskdtyp ?? 0).ToString());
            var land = landMapping.GetValue((personSysland ?? 1).ToString());

            AddressDescription addressDescription = null;
            var location = new Location()
            {
                country = land,
                houseNumber = personHsnr,
                street = personStrasse,
                zip = personPlz,
                city = personOrt
            };

            if (kdTyp == 1)
            {
                throw new ArgumentException("You can only load control person from a company, not a private person.");
                ////Private Person
                //addressDescription = new PersonAddressDescription()
                //{
                //    firstName = personVorname,
                //    lastName = personName,
                //    location = location
                //};

            }
            else
            {
                // Firma 
                addressDescription = new CompanyAddressDescription()
                {
                    companyName = personName,
                    location = location
                };
            }

            return new CrifInDto()
            {
                GetReport = new CrifGetReportInDto()
                {
                    reportType = "CONTROL_PERSON_BUSINESS",
                    targetReportFormatSpecified = true,
                    targetReportFormat = TargetReportFormat.NONE,
                    searchedAddress = addressDescription,
                    
                }
            };
        }

        /// <summary>
        /// Delivers all Kontrollinhabers found in CREFO realm for the customer
        /// will later be used to create IT and ITKNE Entries
        /// </summary>
        /// <param name="sysauskunft"></param>
        /// <returns></returns>
        public List<AdresseDto> getKontrollinhaber(long sysauskunft)
        {
            string query = @"select t.value controlPersonType,a.firstname vorname, a.lastname name,s.sysstaat sysstaat, s.staat staatBezeichnung, l.sysland, l.countryname landBezeichnung, a.street strasse, a.housenumber hsnr, a.zip plz, a.city ort, a.companyname name, to_date(case when length(birthdate)=10 then birthdate else null end,'yyyy-MM-dd') gebdatum, k.value adressid
                            from 
                            cfoutgetreport o, 
                            cfaddress a
                            left join land l on a.country = l.ahskennzeichen
                            left join staat s on substr(a.regionCode, 4) = s.code
                            left join cfkeyvalue k on k.area = 'CFADDRESS' and k.context = 'identifiers' and k.keyval = 'ADDRESS_ID' and k.sysid = a.syscfaddress
                            left join cfkeyvalue t on t.area = 'CFADDRESS' and t.context = 'controlPerson' and t.keyval = 'controlPersonType' and t.sysid = a.syscfaddress
                            where o.sysauskunft = :pSysAuskunft 
                            and a.syscfoutgetreport = o.syscfoutgetreport
                            order by (case to_char('x'||t.value||'x')  when 'xx' then 0 when 'xPARTICIPATIONx' then 1 when 'xOTHERx' then 2 when 'xCEOx' then 3 when 'xHIGHEST_DECISION_MAKERx' then 3 END) asc";

            using (DdOlExtended context = new DdOlExtended())
            {
                return context.ExecuteStoreQuery<AdresseDto>(query, new OracleParameter("pSysAuskunft", sysauskunft)).ToList();
            }
        }

        /// <summary>
        /// returns the current status of the Kontrollinhaber Enquiry
        /// Detail about how to fetch the info is described in the enum
        /// </summary>
        /// <param name="sysauskunft"></param>
        /// <returns></returns>
        public KontrollinhaberStatus getExtendedAuskunftStatus(long sysauskunft)
        {
            string query = @"select 
case 
     when m.addressmatchresulttype = 'MATCH' and (select count(*) controlPersonsCount from cfaddress a where o.syscfoutgetreport = a.syscfoutgetreport) > 0 then 0
     when m.addressmatchresulttype = 'CANDIDATES' then 1
     when m.addressmatchresulttype = 'NO_MATCH' then 2 
     when nvl(auskunft.fehlercode, 0) != 0 then 3
     when m.addressmatchresulttype = 'MATCH' and (select count(*) controlPersonsCount from cfaddress a where o.syscfoutgetreport = a.syscfoutgetreport) = 0 then 0
else 3
end
from auskunft auskunft,
cfoutgetreport o, 
cfaddressmatch m
where 
auskunft.sysauskunft = :pSysAuskunft
and auskunft.sysauskunft = o.sysauskunft
and m.syscfaddressmatch = o.syscfaddressmatch";

            using (DdOlExtended  context = new DdOlExtended())
            {
                var result = context.ExecuteStoreQuery<KontrollinhaberStatus>(query, new OracleParameter("pSysAuskunft", sysauskunft)).ToList();
                if (result.Any())
                {
                    KontrollinhaberStatus rval= result.FirstOrDefault();
                    return rval;
                }

                return KontrollinhaberStatus.ERROR_CRIF;
            }
        }

        /// <summary>
        /// Determines if the customer as a legalform that requires KNE
        /// 
        /// see 3.4.2.1
        /// </summary>
        /// <param name="sysauskunft"></param>
        /// <returns></returns>
        public bool getFeststellungspflicht(long sysauskunft)
        {
            var query = @"select 
case when d.domainid = 'FESTPFLT_FALSE' then 0
else 1
end feststellungspflicht
from 
cfoutgetreport o
left join cfcompany c on o.syscfcompany = c.syscfcompany
left join ddlkppos d on d.code = 'CRIF_LEGALFORM' and value = c.LEGALFORMTYPEORIGINAL
where o.sysauskunft = :pSysAuskunft";

            using (DdOlExtended context = new DdOlExtended())
            {
                int cnt = context.ExecuteStoreQuery<int>(@"select count(*) from(
                                select 
                                case when d.domainid = 'FESTPFLT_FALSE' then 0
                                else 1
                                end feststellungspflicht
                                from
                                cfoutgetreport o
                                left
                                join cfcompany c on o.syscfcompany = c.syscfcompany
                                left
                                join ddlkppos d on d.code = 'CRIF_LEGALFORM' and value = c.LEGALFORMTYPEORIGINAL
                                where o.sysauskunft = :pSysAuskunft)", new OracleParameter("pSysAuskunft", sysauskunft)).FirstOrDefault();
                if (cnt == 0) return true;//no data yet, set true
                return context.ExecuteStoreQuery<bool>(query, new OracleParameter("pSysAuskunft", sysauskunft)).FirstOrDefault();
            }
        }

        /// <summary>
        /// returns the crefo adress id of the customer
        /// </summary>
        /// <param name="sysauskunft"></param>
        /// <returns></returns>
        public string getCustomerAdressId(long sysauskunft)
        {
            string query = @"select k.value from
cfoutgetreport o, 
cfaddressmatch m,
cfaddress a,
cfkeyvalue k
where o.sysauskunft = :pSysAuskunft
and o.syscfaddressmatch = m.syscfaddressmatch
and a.syscfaddress = m.syscfaddress
and k.area = 'CFADDRESS'
and k.context = 'identifiers' 
and k.keyval = 'ADDRESS_ID' and k.sysid = a.syscfaddress";

            using (DdOlExtended context = new DdOlExtended())
            {
                return context.ExecuteStoreQuery<string>(query, new OracleParameter("pSysAuskunft", sysauskunft)).FirstOrDefault();
            }
        }

        /// <summary>
        /// For unidentified customer return a possible hitlist to identify the customer in crefo-realm
        /// </summary>
        /// <param name="sysauskunft"></param>
        /// <returns></returns>
        public List<AdresseDto> getTrefferliste(long sysauskunft)
        {
            string query = @"select s.sysstaat sysstaat, s.staat staatBezeichnung, l.sysland, l.countryname landBezeichnung, a.street strasse, a.housenumber hsnr, a.zip plz, a.city ort, a.companyname name, to_date(case when length(birthdate)=10 then birthdate else null end,'yyyy-MM-dd') gebdatum, k.value adressid
from 
cfoutgetreport o, 
cfaddress a
left join land l on a.country = l.ahskennzeichen
left join staat s on substr(a.regionCode, 4) = s.code, 
cfcandidate c
left join cfkeyvalue k on k.area = 'CFCANDIDATE' and k.keyval = 'ADDRESS_ID' and k.sysid = c.syscfcandidate
where o.sysauskunft = :pSysAuskunft 
and c.syscfaddressmatch = o.syscfaddressmatch 
and a.syscfaddress = c.syscfaddress";

            using (DdOlExtended context = new DdOlExtended())
            {
                return context.ExecuteStoreQuery<AdresseDto>(query, new OracleParameter("pSysAuskunft", sysauskunft)).ToList();
            }
        }

        protected override TypeGetReportRequest MapFromInput(long sysAuskunft, CrifInDto inDto)
        {
            return Mapper.Map(inDto.GetReport, new TypeGetReportRequest());
        }

        protected override TypeGetReportResponse ExecuteRequest(long sysAuskunft, TypeGetReportRequest request)
        {
            return crifWsDao.GetReport(request);
        }

        protected override void MapToOutput(CrifOutDto crifOutDto, TypeGetReportResponse response)
        {
            crifOutDto.GetReport = Mapper.Map(response, new CrifGetReportOutDto());
        }

        protected override long GetSysCfHeader(CrifInDto inDto)
        {
            return inDto.GetReport.SysCfHeader;
        }
    }
}
