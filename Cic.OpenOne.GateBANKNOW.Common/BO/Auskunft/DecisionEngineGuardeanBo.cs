using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Model.DdOl;

using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003;

using Devart.Data.Oracle;
 
using MinimalApplicantStruct = Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.MinimalApplicantStruct;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    using System.Globalization;
    using System.Text;

    using GuardeanServiceReference;

    using GuardeanStatusUpdateServiceReference;

    using OpenOne.Common.Model.DdOd;

    using executeRequest = GuardeanServiceReference.executeRequest;
    using executeResponse = GuardeanServiceReference.executeResponse;
    using CIC.Database.OL.EF6.Model;

    using Extensions;

    using Newtonsoft.Json;

    using OpenOne.Common.DAO;
    using OpenOne.Common.Util;
    using SHS.W006;
    using SHS.W007;
    using CIC.Database.OW.EF6.Model;
    using CIC.Database.OD.EF6.Model;
    using CIC.Database.IC.EF6.Model;

    public enum ApplicantRole
    {
        Participants = 0,
        LimitedPartner,
        Complementary,
        UBO,
        Shareholder,
        Owner,
        AssociationCEO,
        FoundationCEO,
        Partner,
        CEO,
        mainApplicant,
        Guarantor,
        CoApplicant,
        authorizedRepresentative,
        CBU

    }


    /// <summary>
    /// Manages the SHS Guardean Interfaces
    /// INT1 = request Auskunft for customer
    /// INT2 = receive Auskunftresults for customer
    /// INT3 = aggregation auf customer data for Guardean
    /// INT4 = sends Data to Guardean SHS
    /// INT5 = 
    /// INT6 = returns the Liability Chain for the Guardean decision process
    /// INT7 = sets the customer check result from the Guardean decision process
    /// 
    /// </summary>
    public class DecisionEngineGuardeanBo : AbstractDecisionEngineGuardeanBo
    {
        private readonly ILandDao landDao;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const long codeTechExc = -2;
        private const long codeSerAufExc = -1;
        /// <summary>
        /// Maps SHS states to Attribute 
        /// </summary>
        private List<StateStruct> mapStates = new List<StateStruct>();

        private IMappingService kneTypMapping;
        
        private IMappingService relateTypeMapping;
        private const int S1REQUEST_HEADER_LAYOUTVERSION = 1;

        /// <summary>
        /// Initializes IAuskunftDao, IDecisionEngineDBDao, IDecisionEngineWSDao
        /// </summary>
        /// <param name="dewsdao"></param>
        /// <param name="desuwsdao"></param>
        /// <param name="dedao"></param>
        /// <param name="auskunftdao"></param>
        /// <param name="landDao"></param>
        public DecisionEngineGuardeanBo(IDecisionEngineGuardeanWSDao dewsdao, DecisionEngineGuardeanStatusUpdateWSDao desuwsdao, IDecisionEngineGuardeanDBDao dedao, IAuskunftDao auskunftdao, ILandDao landDao)
            : base(dewsdao, desuwsdao, dedao, auskunftdao)
        {
            this.landDao = landDao;
            this.mapStates = dedao.getStatesMap();
            
            this.kneTypMapping = new DbMappingService<OLContext>(new ContextFactory(), (context) => ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<KeyValueLabel>("select code as key, sysknetyp as value from knetyp").AsQueryable());
            this.relateTypeMapping = new DbMappingService<OLContext>(new ContextFactory(), (context) => ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<KeyValueLabel>("select value as key, id as value from ddlkppos where activeflag=1 and code='GUARDEAN_RELATETYPE'").AsQueryable());
         
        }


        /// <summary>
        /// Sends Data to Guardean SHS - INT1 Interface. CIC → SHS
        /// 
        /// input sysid==antrag id must always be filled!
        /// Gets a filled DecisionEngineDto and saves it to database,
        /// Creates a new Auskunft and saves it to database,
        /// Calls DecisionEngine Webservice and saves output to  database,
        /// Updates Auskunft
        /// </summary>
        /// <param name="inDto2"></param>
        /// <returns>DecisionEngineOutDto</returns>
        public override AuskunftDto execute(DecisionEngineGuardeanInDto inDto2)
        {
            // Save Auskunft
            long sysAuskunft = inDto2.sysauskunft;
            if (sysAuskunft == 0) //if empty create a new auskunft
            {
                sysAuskunft = this.auskunftdao.SaveAuskunft(AuskunfttypDao.CreditDecision);
            }

            // Get AuskunftDto
            AuskunftDto auskunftdto = this.auskunftdao.FindBySysId(sysAuskunft);
            auskunftdto.sysid = inDto2.sysid;

            try
            {
                DecisionEngineGuardeanOutDto outDto = new DecisionEngineGuardeanOutDto();
                DecisionEngineGuardeanInDto inDto = this.dedao.fillFromAntrag(inDto2.sysid, false);

                executeRequest request = new executeRequest();
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    request.application = MyExecuteQuery<ApplicationReducedStruct>(ctx, "application", DEGSql.APPLICATION, ":pANTRAGID", "" + inDto2.sysid);
                    request.application.contract = MyExecuteQuery<ContractStruct>(ctx, "contract", DEGSql.CONTRACT, ":pANTRAGID", "" + inDto2.sysid);

                    request.application.contract.replacement = MyExecuteQuery<replacementStruct>(ctx, "replacement", DEGSql.REPLACEMENT, ":pANTRAGID", "" + inDto2.sysid);
                    request.application.contract.premiums = MyExecuteQueryArray<PremiumsStruct>(ctx, "premiums", DEGSql.PREMIUMS, ":pANTRAGID", "" + inDto2.sysid);

                    request.application.vehicle = MyExecuteQuery<VehicleStruct>(ctx, "vehicle", DEGSql.VEHICLE, ":pANTRAGID", "" + inDto2.sysid);
                    request.application.vehicle.schwacke = MyExecuteQuery<SchwackeStruct>(ctx, "schwacke", DEGSql.SCHWACKE, ":pANTRAGID", "" + inDto2.sysid);

                    if (request.application.vehicle.schwacke != null)
                    {
                        request.application.vehicle.schwacke.residualValueTable = MyExecuteQueryArray<monthsStruct>(ctx, "residualValues", DEGSql.RESIDUALVALUES, ":pANTRAGID", "" + inDto2.sysid);
                    }

                    request.application.dealer = MyExecuteQuery<DealerStruct>(ctx, "dealer", DEGSql.DEALER, ":pANTRAGID", "" + inDto2.sysid);
                    request.application.dealer.contact = MyExecuteQuery<ContactStruct>(ctx, "dealerContact", DEGSql.DEALERCONTACT, ":pANTRAGID", "" + inDto2.sysid);

                    //APPLICANT
                    DecisionEngineGuardeanInDto inDtoMa = this.dedao.fillFromAntrag(inDto2.sysid, true);
                    int anzapp = 1;
                    if (inDtoMa != null && inDtoMa.sysid > 0)
                    {
                        anzapp++;
                    }

                    request.extendedApplicant = new ExtendedApplicantReducedStruct[anzapp];
                    request.extendedApplicant[0] = getApplicant(ctx, inDto);

                    if (inDtoMa != null)
                    {
                        request.extendedApplicant[1] = getApplicant(ctx, inDtoMa);
                    }
                }

                //For report
                this.dewsdao.setSoapXMLDto(this.auskunftdao.getEntitySoapLog(sysAuskunft));
                inDto.XMLREQUEST = XMLSerializer.Serialize(request, "UTF-8");
                this.dedao.SaveDecisionEngineInput(sysAuskunft, inDto);
                // _log.Debug(inDto.XMLREQUEST);
                this.dewsdao.setCredentials(this.dedao.getCredentials());
                // Send request away
                executeResponse response = this.dewsdao.execute(request);

                outDto.XMLRESPONSE = XMLSerializer.Serialize(response, "UTF-8");
                outDto.executionId = response.executionID;

                // Save Output
                this.dedao.SaveDecisionEngineOutput(sysAuskunft, outDto);

                this.auskunftdao.UpdateAuskunftDtoAuskunft(auskunftdto, 0);

                auskunftdto.requestXML = this.dewsdao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = this.dewsdao.getSoapXMLDto().responseXML;

                return auskunftdto;
            }
            catch (Exception e)
            {
                this.auskunftdao.UpdateAuskunft(sysAuskunft, codeTechExc);
                _log.Error("Exception in Credit Decision!");
                throw new ApplicationException("Exception in Credit Decision: " + e.Message, e);
            }
        }

        /// <summary>
        /// Creates the applicant shs structure
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="inDto"></param>
        /// <returns></returns>
        private ExtendedApplicantReducedStruct getApplicant(DdOlExtended ctx, DecisionEngineGuardeanInDto inDto)
        {
            string suffix = "";
            if (inDto.isMa > 0)
            {
                suffix = "2";
            }

            ExtendedApplicantReducedStruct rval = MyExecuteQuery<ExtendedApplicantReducedStruct>(ctx, "extendedApplicant" + suffix, GetQuery(inDto.isMa, DEGSql.EXTENDEDAPPLICANT, DEGSql.EXTENDEDAPPLICANT2), ":pANTRAGID", "" + inDto.sysid);
            
            
            rval.extendedAddress = MyExecuteQueryArray<ExtendedAddressStruct>(ctx, "extendedAddress" + suffix, GetQuery(inDto.isMa, DEGSql.EXTENDEDADDRESS, DEGSql.EXTENDEDADDRESS2), ":pANTRAGID", "" + inDto.sysid);
            
            if (inDto.syskdtyp == 1 || inDto.syskdtyp == 2)
            {
                rval.bankAccountInformation = MyExecuteQuery<BankAccountInformationStruct>(ctx, "bankAccountPrivate" + suffix, GetQuery(inDto.isMa, DEGSql.BANKACCOUNTPRIVATE, DEGSql.BANKACCOUNTPRIVATE2), ":pANTRAGID", "" + inDto.sysid);
                rval.contact = MyExecuteQuery<ContactStruct>(ctx, "contactPrivate" + suffix, GetQuery(inDto.isMa, DEGSql.CONTACTPRIVATE, DEGSql.CONTACTPRIVATE2), ":pANTRAGID", "" + inDto.sysid);
                rval.extendedApplicantPrivate = MyExecuteQuery<ExtendedApplicantPrivateStruct>(ctx, "extendedApplicantPrivate" + suffix, GetQuery(inDto.isMa, DEGSql.EXTENDEDAPPLICANTPRIVATE, DEGSql.EXTENDEDAPPLICANTPRIVATE2), ":pANTRAGID", "" + inDto.sysid);

                rval.extendedApplicantPrivate.applicantEmployment = new ApplicantEmploymentStruct[1];
                rval.extendedApplicantPrivate.applicantEmployment[0] = MyExecuteQuery<ApplicantEmploymentStruct>(ctx, "applicantEmployment" + suffix, GetQuery(inDto.isMa, DEGSql.APPLICANTEMPLOYMENT, DEGSql.APPLICANTEMPLOYMENT2), ":pANTRAGID", "" + inDto.sysid);
                rval.extendedApplicantPrivate.applicantFinancials = new ApplicantFinancialsStruct
                {
                    expense = MyExecuteQueryArray<ExpenseStruct>(ctx, "applicantFinancialsExpen" + suffix, GetQuery(inDto.isMa, DEGSql.APPLICANTFINANCIALSEXPEN, DEGSql.APPLICANTFINANCIALSEXPEN2),
                                                                 ":pANTRAGID", "" + inDto.sysid),
                    income = MyExecuteQueryArray<IncomeStruct>(ctx, "applicantFinancialsInc" + suffix, GetQuery(inDto.isMa, DEGSql.APPLICANTFINANCIALSINC, DEGSql.APPLICANTFINANCIALSINC2),
                                                               ":pANTRAGID", "" + inDto.sysid)
                };

                

                rval.extendedApplicantPrivate.applicantIdentity = MyExecuteQuery<ApplicantIdentityStruct>(ctx, "applicantIdentity" + suffix, GetQuery(inDto.isMa, DEGSql.APPLICANTIDENTITY, DEGSql.APPLICANTIDENTITY2), ":pANTRAGID", "" + inDto.sysid);
                
                rval.extendedApplicantPrivate.pepInformation = MyExecuteQuery<PepInformationStruct>(ctx, "pepInformation" + suffix, GetQuery(inDto.isMa, DEGSql.PEPINFORMATION, DEGSql.PEPINFORMATION2), ":pANTRAGID", "" + inDto.sysid);

            }
            else
            {
                rval.bankAccountInformation = MyExecuteQuery<BankAccountInformationStruct>(ctx, "bankAccountComp" + suffix, GetQuery(inDto.isMa, DEGSql.BANKACCOUNTCOMP, DEGSql.BANKACCOUNTCOMP2), ":pANTRAGID", "" + inDto.sysid);
                rval.contact = MyExecuteQuery<ContactStruct>(ctx, "contactComp" + suffix, GetQuery(inDto.isMa, DEGSql.CONTACTCOMP, DEGSql.CONTACTCOMP2), ":pANTRAGID", "" + inDto.sysid);
                rval.extendedApplicantCompany = MyExecuteQuery<ExtendedApplicantCompanyStruct>(ctx, "extendedApplicantComp" + suffix, GetQuery(inDto.isMa, DEGSql.EXTENDEDAPPLICANTCOMP, DEGSql.EXTENDEDAPPLICANTCOMP2), ":pANTRAGID", "" + inDto.sysid);

                rval.extendedApplicantCompany.complementaryApplicantRedu = GetComplementaryApplicants(ctx, inDto.sysid);
            }

            return rval;
        }

        private ComplementaryApplicantReduStruct[] GetComplementaryApplicants(DdOlExtended ctx, long sysAntrag)
        {
            var complementaryApplicants = MyExecuteQueryArray<ComplementaryApplicantReduStruct>(ctx, "compAppliList", DEGSql.COMPAPPLILIST, ":pANTRAGID", sysAntrag.ToString());

            foreach (var complementaryApplicant in complementaryApplicants)
            {
                complementaryApplicant.contact = MyExecuteQuery<ContactStruct>(ctx, "compAppliContact", DEGSql.COMPAPPLICONTACT, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID);
                complementaryApplicant.extendedAddress = MyExecuteQueryArray<ExtendedAddressStruct>(ctx, "compAppliAddress", DEGSql.COMPAPPLIADDRESS, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID);
                complementaryApplicant.complementaryApplicantPrivate = MyExecuteQuery<ComplementaryApplicantPrivateStruct>(ctx, "compAppliPrivate", DEGSql.COMPAPPLIPRIVATE, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID);
                complementaryApplicant.complementaryApplicantPrivate.pepInformation = MyExecuteQuery<PepInformationStruct>(ctx, "compPepInformation", DEGSql.COMPPEPINFORMATION, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID);
                complementaryApplicant.complementaryApplicantPrivate.applicantIdentity = MyExecuteQuery<ApplicantIdentityStruct>(ctx, "compAppliPrivateIdentit", DEGSql.COMPAPPLIPRIVATEIDENTIT, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID);
                
                complementaryApplicant.bankAccountInformation = MyExecuteQuery<BankAccountInformationStruct>(ctx, "compAppliBankAccInfo", DEGSql.COMPAPPLIBANKACCINFO, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID);
                complementaryApplicant.complementaryApplicantPrivate.applicantEmployment = MyExecuteQuery<ApplicantEmploymentStruct>(ctx, "compAppliPrivateEmpl", DEGSql.COMPAPPLIPRIVATEEMPL, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID);
                complementaryApplicant.complementaryApplicantPrivate.applicantFinancials = new ApplicantFinancialsStruct
                {
                    expense = MyExecuteQueryArray<ExpenseStruct>(ctx, "compAppliPrivateFinExp", DEGSql.COMPAPPLIPRIVATEFINEXP, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID),
                    income = MyExecuteQueryArray<IncomeStruct>(ctx, "compAppliPrivateFinInc", DEGSql.COMPAPPLIPRIVATEFININC, ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID)
                };
                complementaryApplicant.complementaryApplicantPrivate.driversLicense = MyExecuteQuery<DriversLicenseStruct>(ctx, "compAppliPrivateDrivLic", "SELECT 1 from dual where 1=0 ", ":pANTRAGID", sysAntrag.ToString(), ":pSYSIT", complementaryApplicant.applicantID);
            }

            return complementaryApplicants;
        }

        private string GetQuery(string suffix, string query, string queryMa)
        {
            return GetQuery(suffix == "2" ? 1 : 0, query, queryMa);
        }

        private string GetQuery(int isMa, string query, string queryMa)
        {
            if (isMa == 1)
            {
                return queryMa;
            }
            return query;
        }

   
        /// <summary>
        /// Finds Auskunft by Sysid,
        /// fills DecisionEngineInDto with values from database,
        /// calls DecisionEngine Webservice and saves Output, 
        /// updates Auskunft
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AuskunftDto</returns>
        public override AuskunftDto execute(long sysAuskunft)
        {
            long code = codeTechExc;

            try
            {
                return execute(this.dedao.FindBySysId(sysAuskunft));
            }
            catch (Exception e)
            {
                this.auskunftdao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in Decision Engine Guardean!", e);
                throw new ApplicationException("Unexpected Exception in Decision Engine Guardean!", e);
            }
        }

        /// <summary>
        /// Sends Data to Guardean SHS - INT4 Interface. CIC → SHS
        /// 
        /// </summary>
        /// <param name="inDto2"></param>
        /// <returns></returns>
        public override AuskunftDto executeStatusUpdate(DecisionEngineGuardeanInDto inDto2)
        {
            // Save Auskunft
            long sysAuskunft = inDto2.sysauskunft;
            if (sysAuskunft == 0)//if empty create a new auskunft
            {
                sysAuskunft = this.auskunftdao.SaveAuskunft(AuskunfttypDao.CreditDecisionStatusUpdate);
            }

            // Get AuskunftDto
            AuskunftDto auskunftdto = this.auskunftdao.FindBySysId(sysAuskunft);
            auskunftdto.sysid = inDto2.sysid;

            try
            {
                DecisionEngineGuardeanOutDto outDto = new DecisionEngineGuardeanOutDto();
                DecisionEngineGuardeanInDto inDto = this.dedao.fillFromAntrag(inDto2.sysid, false);

                var request = new GuardeanStatusUpdateServiceReference.executeRequest();
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    request.businessInformation = MyExecuteQuery<BusinessInformationStruct>(ctx, "businessInformation", DEGSql.BUSINESSINFORMATION, ":pANTRAGID", inDto2.sysid.ToString());
                    request.technicalInformation = MyExecuteQuery<TechnicalInformationStruct>(ctx, "technicalInformation", DEGSql.TECHNICALINFORMATION, ":pANTRAGID", inDto2.sysid.ToString());
                }

                //For report
                this.desuwsdao.setSoapXMLDto(this.auskunftdao.getEntitySoapLog(sysAuskunft));
                inDto.XMLREQUEST = XMLSerializer.Serialize(request, "UTF-8");
                this.dedao.SaveDecisionEngineInput(sysAuskunft, inDto);
                this.desuwsdao.setCredentials(this.dedao.getCredentials());
                var response = this.desuwsdao.execute(request);

                outDto.XMLRESPONSE = XMLSerializer.Serialize(response, "UTF-8");
                outDto.executionId = response.statusResponseText;

                int statusCode = 0;
                if (!response.statusResponse)
                {
                    statusCode = 1;
                }

                // Save Output
                this.dedao.SaveDecisionEngineOutput(sysAuskunft, outDto);

                this.auskunftdao.UpdateAuskunftDtoAuskunft(auskunftdto, statusCode);

                auskunftdto.requestXML = this.desuwsdao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = this.desuwsdao.getSoapXMLDto().responseXML;

                return auskunftdto;
            }
            catch (Exception e)
            {
                this.auskunftdao.UpdateAuskunft(sysAuskunft, codeTechExc);
                _log.Error("Exception in Credit Decision!");
                throw new ApplicationException("Exception in Credit Decision: " + e.Message, e);
            }
        }

        public override AuskunftDto executeStatusUpdate(long sysAuskunft)
        {
            long code = codeTechExc;

            try
            {
                return executeStatusUpdate(this.dedao.FindBySysId(sysAuskunft));
            }
            catch (Exception e)
            {
                this.auskunftdao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in Decision Engine Status Update Guardean!", e);
                throw new ApplicationException("Unexpected Exception in Decision Engine Status Update Guardean!", e);
            }
        }

        /// <summary>
        /// Creates a new person from the it
        /// </summary>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        private long createKundeFromCBU(CbuPartiesStruct cbu, long sysantrag)
        {
            _log.Debug("Create PERSON for CBU " + cbu.name1 + " for ANTRAG.sysid=" + sysantrag);
            IKundeBo kdbo = BOFactory.getInstance().createKundeBo();

            KundeDto kd = GetKundeFromIt(kdbo, 0, sysantrag);

            //nur updaten wenn neuer kunde und noch nicht da
            kd.strasse = cbu.street;
            kd.hsnr = cbu.number;
            kd.plz = cbu.zip;
            kd.ort = cbu.city;
            kd.vorname = cbu.name2;
            kd.name = cbu.name1;
            kd.sysland = this.landDao.GetSysLandFromIsoCountry(cbu.country);
            kd.gebdatum = cbu.dob;
            kd.gruendung = cbu.foundationDate;

            kd.privatflag = (cbu.isPrivate == "Yes") ? 1 : 0;
            kd.rechtsformCode = cbu.legalformCode;
            kd.rechtsform = cbu.legalformText;

            kd.crefoid = cbu.crefoId;
            kd.ficoid = cbu.ficoID;

            KundeDto kdPerson = kdbo.createOrUpdateKundePerson(kd, 0);
            _log.Debug("Created new PERSON " + kdPerson.syskd);
            return kdPerson.syskd;
        }

        /// <summary>
        /// Creates a new person from the it
        /// </summary>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        private long createKundeFromIT(long sysit, long sysantrag, MinimalApplicantStruct applicant, string schufaId = null, string crefoId = null, string ficoId = null)
        {
            _log.Debug("Create PERSON from IT " + sysit + " for ANTRAG.sysid=" + sysantrag + " with applicant-Data: " + applicant);
            IKundeBo kdbo = BOFactory.getInstance().createKundeBo();

            KundeDto kd = GetKundeFromIt(kdbo, sysit, sysantrag);

            kd.syskd = 0;
            kd.schufaid = schufaId;
            kd.crefoid = crefoId;
            kd.ficoid = ficoId;

            //nur updaten wenn neuer kunde und noch nicht da
            if (applicant != null)
            {
                if (applicant.address != null && applicant.address.Length > 0)
                {
                    kd.ort = applicant.address[0].city;
                    kd.hsnr = applicant.address[0].housenumber;
                    kd.plz = applicant.address[0].postalCode;
                    kd.strasse = applicant.address[0].street;
                    kd.sysland = this.landDao.GetSysLandFromIsoCountry(applicant.address[0].country);
                }
                if (applicant.minimalApplicantPrivate != null)
                {
                    kd.gebdatum = applicant.minimalApplicantPrivate.dateOfBirth;
                    kd.vorname = applicant.minimalApplicantPrivate.firstName;
                    kd.anredeCode = GetAnredeCode(applicant.minimalApplicantPrivate.gender);
                    kd.name = applicant.minimalApplicantPrivate.lastName;
                    kd.gebort = applicant.minimalApplicantPrivate.placeOfBirth;

                    if (kd.schufaid == null)
                    {
                        kd.schufaid = applicant.minimalApplicantPrivate.schufaID;
                    }
                }
                if (applicant.minimalApplicantCompany != null)
                {
                    kd.name = applicant.minimalApplicantCompany.companyName;
                    kd.rechtsform = applicant.minimalApplicantCompany.legalForm;
                    kd.rechtsformCode = applicant.minimalApplicantCompany.legalFormCode;
                }
            }

            KundeDto kdPerson = kdbo.createOrUpdateKundePerson(kd, 0);
            kdbo.transferITPKZUKZToPERSON(kdPerson.syskd, sysit, sysantrag);
            // Update It
            if (sysit != 0)
            {
                using (DdOdExtended ctx = new DdOdExtended())
                {
                    updateSysPersonOfIt(ctx, sysit, kdPerson.syskd);
                }
            }

            _log.Debug("Created new PERSON " + kdPerson.syskd);
            return kdPerson.syskd;
        }

        private KundeDto GetKundeFromIt(IKundeBo kdbo, long sysit, long sysantrag)
        {
            KundeDto kd = null;
            kd = sysit == 0 ? new KundeDto() : kdbo.getKundeViaAntragID(sysit, sysantrag);

            //Reset Zusatzdaten keys for Copy
            if (kd.zusatzdaten != null && kd.zusatzdaten.Length > 0)
            {
                foreach (ZusatzdatenDto zd in kd.zusatzdaten)
                {
                    if (zd.pkz != null && zd.pkz.Length > 0)
                    {
                        foreach (PkzDto p in zd.pkz)
                        {
                            p.syspkz = 0;
                            p.sysantrag = sysantrag;
                        }
                    }
                    if (zd.ukz != null && zd.ukz.Length > 0)
                    {
                        foreach (UkzDto p in zd.ukz)
                        {
                            p.sysukz = 0;
                            p.sysantrag = sysantrag;
                        }
                    }
                }
            }

            kd.aktivkz = 1;
            kd.flagkd = 1;
            return kd;
        }

        private string GetAnredeCode(string gender)
        {
            if (gender == "F")
            {
                return "1";
            }
            if (gender == "M")
            {
                return "2";
            }
            return null;
        }

        private void updateKundeFromIT(long syskd, long sysit, long sysantrag, string schufaId = null, string crefoId = null, string ficoId = null)
        {
            IKundeBo kdbo = BOFactory.getInstance().createKundeBo();

            UpdatePersonIds(syskd, schufaId, crefoId, ficoId);
            kdbo.transferITPKZUKZToPKZUKZ(syskd, sysit, sysantrag);

            _log.Debug("Update PKZ/UKZ from ITPKZ/ITUKZ with SYSPERSON " + syskd + ", SYSIT " + sysit);

            //using (var context = new DdOdExtended())
            //{
            //    updateSysPersonOfIt(context, sysit, syskd);
            //}
        }



        /// <summary>
        /// Update or create bonitaet with pd1 and pd2 and antrag
        /// </summary>
        /// <param name="sysperson"></param>
        /// <param name="sysantrag"></param>
        /// <param name="sysit"></param>
        /// <param name="dst"></param>
        private void checkBonitaetDecision(long sysantrag, DecisionStruct dst)
        {
            long sysit;
            if (dst.detail == null || !long.TryParse(dst.detail.applicantID, out sysit))
            {
                return;
            }

            using (DdOlExtended ctx = new DdOlExtended())
            {
                BONITAET b = null;
                int sysbonitaet = ctx.ExecuteStoreQuery<int>(string.Format("select sysbonitaet from bonitaet where sysit={0} AND sysantrag={1}", sysit, sysantrag), null).FirstOrDefault();
                if (sysbonitaet > 0)
                {
                    b = (from c in ctx.BONITAET
                         where c.SYSBONITAET == sysbonitaet
                         select c).FirstOrDefault();
                }
                else
                {
                    var sysperson = ctx.ExecuteStoreQuery<int>(string.Format("select sysperson from it where sysit={0}", sysit), null).FirstOrDefault();
                    b = new BONITAET
                    {
                        RANG = 1,
                        SYSPERSON=sysperson
                        
                    };
                    ctx.BONITAET.Add(b);
                }

                b.SYSANTRAG=sysantrag;
                if (sysit > 0)
                {
                    b.SYSIT=sysit;
                }

                b.RATINGWANN = dst.date;
                b.RATINGWER = dst.editor;

                if (dst.detail != null)
                {
                    b.PD1 = GetDefault(dst.detail.pd_lifetime, b.PD1);
                    b.PD2 = GetDefault(dst.detail.pd_6m, b.PD2);
                    b.RATING = GetDefault(dst.detail.riskClass, b.RATING);
                    b.BEMERKUNG = GetDefault(dst.detail.scoredescription, b.BEMERKUNG);
                    b.SCHUFAID = GetDefault(dst.detail.schufaID, b.SCHUFAID);
                    b.CREFONUMBER = GetDefault(dst.detail.crefoID, b.CREFONUMBER);
                }

                ctx.SaveChanges();
            }
        }

        private void createOrUpdateKne(decimal? quote, long sysober, long sysunter, int sysKneTyp, string relateTypeCode = null, string area = null, long? sysid = null)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                var query = ctx.KNE.Where(a => a.SYSOBER == sysober && a.SYSUNTER == sysunter && a.SYSKNETYP == sysKneTyp);

                if (relateTypeCode != null)
                {
                    query = query.Where(a => a.RELATETYPECODE == relateTypeCode);
                }
                else
                {
                    query = query.Where(a => a.RELATETYPECODE == null);
                }

                bool existsInDatabase = query.Any();

                if (existsInDatabase)
                {
                    return;
                }

                KNE b = new KNE
                {
                    SYSOBER = sysober,
                    SYSUNTER = sysunter,
                    QUOTE = quote,
                    SYSKNETYP = sysKneTyp,
                    RELATETYPECODE = relateTypeCode,
                    AREA = area,
                    SYSAREA = sysid
                };

                ctx.KNE.Add(b);

                ctx.SaveChanges();
            }
        }


        /// <summary>
        /// INT2
        /// 
        /// Updates the Auskunft with the Guardean Result. SHS → CIC
        /// 
        /// </summary>
        /// <param name="resp"></param>
        public override void setCreditDecisionResult(SHS.W002.executeRequest resp)
        {
            /*
             * -> wenn personenid, dann im antrag als syskd speichern
             * wenn keine personenid, dann it+itpkz -> kunde+pkz, dann syskd speichern
             * <decision>string</decision> -> antrag.attribut, zustand über:
                antrag.zustand= select statedef.zustand from attribut,statedef,attributdef where attribut.sysattributdef=attributdef.sysattributdef and attributdef.attribut='Genehmigt' and statedef.sysstatedef=attribut.sysstatedef;
             */

            if (resp.decision == null || resp.decision.Length == 0)
            {
                _log.Warn("No Decision in Result for Auskunft #" + resp.applicationId);
                throw new Exception("No Decision in Result for Auskunft #" + resp.applicationId);

            }
            if (resp.decision == null || resp.decision.Length == 0)
            {
                _log.Warn("No Decision in Result for Auskunft #" + resp.applicationId);
                throw new Exception("No Decision in Result for Auskunft #" + resp.applicationId);
            }
            if (resp.decision.Length > 1)
            {
                _log.Warn("Multiple Decisions Auskunft #" + resp.applicationId + " - using first");
            }
            if (resp.customerCheckResponse == null)
            {
                _log.Warn("No Customer Check Response for Auskunft #" + resp.applicationId);
            }

            long sysauskunft = 0;
            try
            {
                AuskunftDto auskunftdto;
                InteressentBo antragsteller;
                InteressentBo mitAntragsteller;
                InteressentBo[] antragstellerList;
                InteressentBo[] complementaryApplicants;
                var firstDecision = resp.decision[0];
                bool updateApplication = !string.IsNullOrWhiteSpace(firstDecision.statusUpdateText);

                //get required data from current antrag/auskunft:
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    sysauskunft = long.Parse(resp.externalReference);

                    if (sysauskunft == 0)
                    {
                        throw new InvalidOperationException(string.Format("No valid Auskunft found for application {0}.", resp.applicationId));
                    }

                    auskunftdto = this.auskunftdao.FindBySysId(sysauskunft);
                    antragsteller = ctx.ExecuteStoreQuery<InteressentBo>(
@"
select antrag.sysit, antrag.syskd as SysPerson, peoption.str04 as KeyId, 0 as IsMa 
from antrag, peoption 
where peoption.sysid(+) = antrag.syskd and antrag.sysid=" + auskunftdto.sysid, null).FirstOrDefault();

                    if (antragsteller == null)
                    {
                        throw new InvalidOperationException(string.Format("No main applicant found for application {0}.", auskunftdto.sysid));
                    }

                    mitAntragsteller = ctx.ExecuteStoreQuery<InteressentBo>(
@"
select antobsich.sysit, antobsich.sysmh as SysPerson, peoption.str04 as KeyId, 1 as IsMa 
from antobsich, sichtyp,  peoption 
where sichtyp.rang in (10,80,110) and antobsich.syssichtyp = sichtyp.syssichtyp and peoption.sysid(+) = antobsich.sysmh and antobsich.sysantrag = " + auskunftdto.sysid, null).FirstOrDefault();

                    antragstellerList = new[] { antragsteller, mitAntragsteller }.Where(a => a != null).ToArray();

                    complementaryApplicants = ctx.ExecuteStoreQuery<InteressentBo>(
@"select antobsich.sysit, antobsich.sysmh as SysPerson, peoption.str04 as KeyId, 1 as IsComplementary 
FROM antrag, antobsich, sichtyp, peoption 
WHERE antobsich.sysantrag = antrag.sysid AND antobsich.syssichtyp = sichtyp.syssichtyp AND antobsich.rang in (101, 102) AND peoption.sysid(+) = antobsich.sysmh AND antrag.sysid = " + auskunftdto.sysid, null).ToArray();
                }

                if (resp.customerCheckResponse != null)
                {
                    foreach (var customerCheckResponse in resp.customerCheckResponse)
                    {
                        var applicant = antragstellerList.Concat(complementaryApplicants)
                            .FirstOrDefault(a => a.SysIt.ToString() == customerCheckResponse.applicantID.Trim());

                        if (applicant == null)
                        {
                            _log.Debug(string.Format("Could not find any applicant for customer check response applicantId: {0}", customerCheckResponse.applicantID));
                            continue;
                        }

                        long sysperson = 0;
                        long.TryParse(customerCheckResponse.customerID, out sysperson); //wenn >0 dann schon sysperson

                        applicant.SchufaId = customerCheckResponse.schufaID;
                        applicant.CrefoId = customerCheckResponse.crefoID;
                        applicant.FicoId = customerCheckResponse.ficoID;
                        applicant.KeyId = customerCheckResponse.keyID;

                        if (sysperson == 0)
                        {
                            // Try to match from KeyId
                            sysperson = GetSysPersonFromKeyId(applicant.KeyId);
                        }

                        if (sysperson > 0)
                        {
                            if (applicant.SysPerson != 0 && applicant.SysPerson != sysperson)
                            {
                                _log.Debug(string.Format("Applicants SYSPERSON {0} is overriden by SYSPERSON {1} from CustomerCheckResponse", applicant.SysPerson, sysperson));
                            }

                            applicant.SysPerson = sysperson;

                            // Wenn SHS einen Kunde matcht, wird immer der IT zur Person übernommen, unabhängig ob die SHS den Status = 3 sendet.
                            // Somit werden auch ITPKZ zu PKZ übernommen
                            // if (customerCheckResponse.status == "3") //wenn status == 3, addressdaten auf person mappen
                            updateKundeFromIT(applicant.SysPerson, applicant.SysIt, auskunftdto.sysid, customerCheckResponse.schufaID, customerCheckResponse.crefoID, customerCheckResponse.ficoID);
                        }
                        else
                        {
                            applicant.SysPerson = createKundeFromIT(applicant.SysIt, auskunftdto.sysid, null);
                        }

                        UpdatePersonOption(applicant.SysPerson, applicant.KeyId);

                        LogApplicant(applicant, auskunftdto);

                        using (DdOdExtended ctx = new DdOdExtended())
                        {
                            applicant.UpdatePerson(ctx);

                            // SYSKD und SYSMH dürfen nur während dem CustomerCheck verändert werden.
                            // Nachdem ein IT Satz verändert wurde und ein Statusupdate zu einem Antrag rein kommt, darf die zugehörige Person nicht erneut übernommen werden,
                            // da diese sich geändert haben kann.
                            var parameters = new List<OracleParameter>
                                {
                                    new OracleParameter { ParameterName = "sysperson", Value = applicant.SysPerson },
                                    new OracleParameter { ParameterName = "sysit", Value = applicant.SysIt }
                                };
                            ctx.ExecuteStoreCommand("UPDATE ANTOBSICH SET SYSMH=:sysperson where SYSIT=:sysit and antobsich.sysantrag = " + auskunftdto.sysid, parameters.ToArray());

                            var parameters2 = new List<OracleParameter>
                                {
                                    new OracleParameter { ParameterName = "syskd", Value = antragsteller.SysPerson }, 
                                    new OracleParameter { ParameterName = "sysid", Value = auskunftdto.sysid }
                                };
                            ctx.ExecuteStoreCommand("UPDATE ANTRAG SET SYSKD=:syskd where sysid=:sysid", parameters2.ToArray());
                        }
                    }
                }


                //alle resp.UboStruct durchlaufen und entweder
                //wenn person schon da, neuer eintrag in kne mit sysperson/quote
                //wenn person nicht da, neuer eintrag in person +kne
                // UBO = Wirtschaftlich Berechtigter
                if (resp.uboResponse == null || resp.uboResponse.Length == 0 || (resp.uboResponse.Length == 1 && resp.uboResponse[0].ubo == null))
                {
                    _log.Warn("No UBOs in Result for Auskunft #" + auskunftdto.sysid);
                }
                else
                {
                    foreach (UboStruct ubo in resp.uboResponse.SelectMany(a => a.ubo).Where(a => a != null))
                    {
                        long sysperson = 0;

                        if (!long.TryParse(ubo.minimalApplicant.customerID, out sysperson))
                        {
                            _log.Warn(string.Format("UBO.minimalApplicant.customerID ({0}) not a valid long for Auskunft #{1}.", ubo.minimalApplicant.customerID, auskunftdto.sysid));
                        }

                        string schufaId = null;
                        if (ubo.minimalApplicant != null && ubo.minimalApplicant.minimalApplicantPrivate != null)
                        {
                            schufaId = ubo.minimalApplicant.minimalApplicantPrivate.schufaID;
                        }

                        string crefoId = null;
                        if (ubo.minimalApplicant.minimalApplicantCompany != null)
                        {
                            crefoId = ubo.minimalApplicant.minimalApplicantCompany.crefoID;
                        }

                        if (ubo.uboShare != null && string.IsNullOrEmpty(crefoId))
                        {
                            crefoId = ubo.uboShare.crefoId;
                        }

                        string ficoId = null;
                        if (ubo.uboShare != null)
                        {
                            ficoId = ubo.minimalApplicant.ficoID;
                        }

                        if (sysperson == 0)
                        {
                            // Try to match from KeyId
                            sysperson = GetSysPersonFromKeyId(ubo.minimalApplicant.keyID);
                        }

                        if (sysperson == 0) //kein vorhandener kunde, anlegen
                        {
                            _log.Debug("New Customer from UBO, because sysperson = 0");

                            sysperson = createKundeFromIT(0, auskunftdto.sysid, ubo.minimalApplicant, schufaId, crefoId, ficoId);
                        }
                        else
                        {
                            UpdatePersonIds(sysperson, schufaId, crefoId, ficoId);
                        }

                        UpdatePersonOption(sysperson, ubo.minimalApplicant.keyID);

                        if (ubo.minimalApplicant != null && ubo.minimalApplicant.minimalApplicantPrivate != null)
                        {
                            CreateOrUpdateBonitaet(auskunftdto.sysid, sysperson, new PersonStruct()
                            {
                                schufaID = ubo.minimalApplicant.minimalApplicantPrivate.schufaID
                            });
                        }
                        else
                        {
                            _log.Warn("No minimalApplicantPrivate in UBO in Result for Auskunft #" + auskunftdto.sysid);
                        }

                        decimal? quote = null;

                        if (ubo.uboShare != null)
                        {
                            quote = ubo.uboShare.totalShare;
                        }

                        long parentApplicantId = 0;
                        long sysOber = 0;

                        if (ubo.minimalApplicant != null && long.TryParse(ubo.minimalApplicant.parentApplicantID, out parentApplicantId))
                        {
                            if (parentApplicantId == antragsteller.SysIt)
                            {
                                sysOber = antragsteller.SysPerson;
                            }
                            else if (parentApplicantId == mitAntragsteller.SysIt)
                            {
                                sysOber = mitAntragsteller.SysPerson;
                            }
                        }

                        if (sysOber == 0)
                        {
                            _log.Warn(string.Format("Can not create KNE, because there is no sysOber for parentApplicantId = {0}", parentApplicantId));
                        }
                        else
                        {
                            createOrUpdateKne(quote, sysOber, sysperson, 3);
                        }
                    }
                }

                if (resp.cbuResponse == null || resp.cbuResponse.cbuGroup == null || resp.cbuResponse.cbuGroup.cbuParties == null || resp.cbuResponse.cbuGroup.cbuParties.Length == 0)
                {
                    _log.Warn("No CBU in Result for Auskunft #" + auskunftdto.sysid);
                    //   throw new Exception("No UBOs in Result for Auskunft #" + resp.applicationId);
                }
                else
                {
                    // CBU = Bürge (Wirtschaftlich Verbundener)
                    foreach (CbuPartiesStruct cbu in resp.cbuResponse.cbuGroup.cbuParties)
                    {
                        //KUNDE or MA
                        long sysperson;
                        long.TryParse(cbu.customerID, out sysperson);

                        if (sysperson == 0)
                        {
                            // Try to match from KeyId
                            sysperson = GetSysPersonFromKeyId(cbu.keyID);
                        }

                        if (sysperson == 0) //kein vorhandener kunde, anlegen
                        {
                            sysperson = createKundeFromCBU(cbu, auskunftdto.sysid);
                        }
                        else
                        {
                            UpdatePersonIds(sysperson, null, cbu.crefoId, cbu.ficoID);
                        }

                        UpdatePersonOption(sysperson, cbu.keyID);

                        CreateOrUpdateBonitaet(auskunftdto.sysid, sysperson, new PersonStruct()
                        {
                            crefoID = cbu.crefoId
                        });

                        long parentApplicantId = 0;
                        long sysOber = 0;
                        long.TryParse(cbu.parentApplicantID, out parentApplicantId);

                        if (parentApplicantId == antragsteller.SysIt)
                        {
                            sysOber = antragsteller.SysPerson;
                        }
                        else if (parentApplicantId == mitAntragsteller.SysIt)
                        {
                            sysOber = mitAntragsteller.SysPerson;
                        }

                        if (sysOber == 0)
                        {
                            _log.Warn(string.Format("Can not create KNE, because there is no sysOber for parentApplicantId = {0}", parentApplicantId));
                        }
                        else
                        {
                            if (antragsteller.SysPerson != sysperson && (mitAntragsteller == null || mitAntragsteller.SysPerson != sysperson))
                            {
                                createOrUpdateKne(cbu.weight, sysOber, sysperson, 2);
                            }
                        }
                    }
                }


                long sysdedefcon = 0;
                long sysdeoutexec = 0;

                //documentsAndCollaterals -> dedecon und Sicherheiten anlegen
                using (DdOlExtended ddOlContext = new DdOlExtended())
                using (DdIcExtended context = new DdIcExtended())
                {
                    // Deactivate existing Applications
                    foreach (var antobsich in ddOlContext.ANTOBSICH.Where(a => a.ANTRAG.SYSID == auskunftdto.sysid))
                    {
                        antobsich.AKTIVFLAG = 0;
                    }


                    // check if DEOUTEXEC already exists
                    AUSKUNFT auskunft = context.AUSKUNFT.Single(par => par.SYSAUSKUNFT == sysauskunft);
                    if (auskunft!=null && !context.Entry(auskunft).Collection(f => f.DEOUTEXECList).IsLoaded)
                        context.Entry(auskunft).Collection(f => f.DEOUTEXECList).Load();
                    

                    // NEW
                    DEOUTEXEC deOutExec = new DEOUTEXEC
                    {
                        AUSKUNFT = auskunft
                    };
                    context.DEOUTEXEC.Add(deOutExec);

                    Dictionary<long, DEDETAIL> detailCust = new Dictionary<long, DEDETAIL>();

                    foreach (var decision in resp.decision.Where(a => a.detail != null))
                    {
                        long sysIt;
                        long.TryParse(decision.detail.applicantID, out sysIt);
                        if (sysIt == 0)
                        {
                            sysIt = antragsteller.SysIt;
                        }

                        if (!detailCust.ContainsKey(sysIt))
                        {
                            DEDETAIL deDetail = new DEDETAIL();
                            context.DEDETAIL.Add(deDetail);
                            deDetail.DEOUTEXEC = deOutExec;

                            if (decision != null)
                            {
                                deDetail.SCORECODE = decision.detail.score;
                                deDetail.SCOREBEZEICHNUNG = decision.detail.scoredescription;
                                deDetail.FRAUDSCORE = (int)decision.detail.fraudscore;
                            }

                            deDetail.ANTRAGSTELLER = (antragsteller.SysIt == sysIt) ? 1 : 2;

                            detailCust[sysIt] = deDetail;
                        }

                        if (decision.residualValueInternalSpecified && decision.residualValueInternal > 0)
                        {
                            var rwBase = decision.residualValueInternal / 1.19m;
                            var parameters = new List<OracleParameter>
                            {
                                new OracleParameter { ParameterName = "pRwBase", Value = rwBase },
                                new OracleParameter { ParameterName = "pRwBaseBrutto", Value = decision.residualValueInternal },
                                new OracleParameter { ParameterName = "pSysAntrag", Value = auskunftdto.sysid }
                            };
                            context.ExecuteStoreCommand("UPDATE ANTKALK SET RWBASE=:pRwBase, RWBASEBRUTTO=:pRwBaseBrutto WHERE SYSANTRAG=:pSysAntrag", parameters.ToArray());
                        }

                        if (decision.fairMarketValueSpecified && decision.fairMarketValue > 0)
                        {
                            var parameters = new List<OracleParameter>
                            {
                                new OracleParameter { ParameterName = "pSchwackeRw", Value = decision.fairMarketValue },
                                new OracleParameter { ParameterName = "pSysAntrag", Value = auskunftdto.sysid }
                            };
                            context.ExecuteStoreCommand("UPDATE ANTOB SET SCHWACKERW=:pSchwackeRw WHERE SYSANTRAG=:pSysAntrag", parameters.ToArray());
                        }
                    }

                    if (resp.documentsAndCollaterals != null && resp.documentsAndCollaterals.Length > 0)
                    {
                        foreach (DocumentsAndCollateralsStruct collateral in resp.documentsAndCollaterals)
                        {
                            long sysIt;
                            long.TryParse(collateral.collateralApplicantId, out sysIt);
                            if (sysIt == 0)
                            {
                                sysIt = antragsteller.SysIt;
                            }

                            DEDETAIL deDetail = detailCust[sysIt];
                            var dedefcon = context.DEDEFCON.FirstOrDefault(a => a.EXTERNCODE == collateral.collateralName);

                            if (dedefcon != null)
                            {
                                DECON decon = new DECON
                                {
                                    DEDETAIL = deDetail,
                                    DEDEFCON = dedefcon,
                                    WERT = collateral.collateralNominalValue.ToString(CultureInfo.InvariantCulture)
                                };
                                context.DECON.Add(decon);

                                if (sysdedefcon == 0)
                                {
                                    sysdedefcon = dedefcon.SYSDEDEFCON;
                                }

                                // Check if the collateral is 'sicherheit' and the type is not 0 and if it is the main applicant and the correct application/contractId
                                if (dedefcon.CONTYPE == 1
                                    && dedefcon.DISPLAYTYPE.HasValue
                                    && dedefcon.DISPLAYTYPE.Value != 0
                                    && sysIt == antragsteller.SysIt
                                    && collateral.collateralContractId == resp.applicationId)
                                {
                                    long sysAntrag = auskunftdto.sysid;
                                    long sysPerson;
                                    long.TryParse(collateral.collateralCustomerId, out sysPerson);

                                    if (sysPerson == 0)
                                    {
                                        sysPerson = antragsteller.SysPerson;
                                    }

                                    var sicherheit = new ANTOBSICH
                                    {
                                        BEGINN = collateral.collateralDateOfValue,
                                        SICHERHEITENWERT = collateral.collateralEffectiveValue,
                                        WERT = collateral.collateralNominalValue,
                                        BEMERKUNG = collateral.collateralAdditionalText,
                                        FLAG01 = Convert.ToInt32(collateral.collateralContractFlag),
                                        SYSANTRAG = sysAntrag,
                                        FLAG02 = Convert.ToInt32(collateral.collateralCustomerFlag),
                                        SYSMH = sysPerson,
                                        OPTION1 = collateral.collateralScaling,
                                        STATUS = collateral.collateralStatus,
                                        ENDE = collateral.collateralTimeLimit,
                                        AKTIVFLAG = 1,
                                        SYSIT = sysIt,
                                        SYSSICHTYP = dedefcon.DISPLAYTYPE.Value
                                    };
                                    ddOlContext.ANTOBSICH.Add(sicherheit);
                                }
                            }
                            else
                            {
                                _log.Warn("No Mapped Result Decision-Code for " + collateral.collateralName + " found in DEDEFCON");
                            }
                        }
                    }

                    context.SaveChanges();
                    ddOlContext.SaveChanges();
                    sysdeoutexec = deOutExec.SYSDEOUTEXEC;
                }


                //Only first decision is saved in ANTRAG
                DDLKPPOS ddlkppos = null;

                //Write Log XML
                using (DdOwExtended ctxOw = new DdOwExtended())
                {
                    using (DdOdExtended ctx = new DdOdExtended())
                    {
                        string xmlResponse = XMLSerializer.Serialize(resp, "UTF-8");
                        DDLKPSPOS output = new DDLKPSPOS
                        {
                            AREA = "DEOUTEXEC",
                            SYSID = sysdeoutexec,
                            CONTENT = xmlResponse,
                            ACTIVEFLAG = 1
                        };
                        ctx.DDLKPSPOS.Add(output);
                        
                        if (updateApplication)
                        {
                            ddlkppos = ctx.DDLKPPOS.First(a => a.ID == firstDecision.statusUpdateText && a.CODE == "ANTRAGSZUSTAND");
                            DDLKPSPOS statusUpdate = new DDLKPSPOS()
                            {
                                AREA = "ANTRAG",
                                ACTIVEFLAG = 1,
                                SYSID = auskunftdto.sysid,
                                CONTENT = ddlkppos.ID,
                                DDLKPPOS = ddlkppos,
                                DDLKPCOL = ctx.DDLKPCOL.FirstOrDefault(a => a.CODE == "ANTRAGSZUSTAND")
                            };
                            ctx.DDLKPSPOS.Add(statusUpdate);
                            
                            var eaihot = new CIC.Database.OW.EF6.Model.EAIHOT()
                            {
                                CODE = "ANTRAG_CHG_STATUS",
                                OLTABLE = "ANTRAG",
                                SYSOLTABLE = auskunftdto.sysid,
                                PROZESSSTATUS = 0,
                                EAIART = ctxOw.EAIART.FirstOrDefault(a => a.CODE == "ANTRAG_CHG_STATUS"),
                                HOSTCOMPUTER = "*",
                                INPUTPARAMETER1 = ddlkppos.ID,
                                EVE = 1,
                                SUBMITDATE = OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                                SUBMITTIME = OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                                SYSWFUSER = ctx.ExecuteStoreQuery<long>("select syswfuser from wfuser where code='EAI'").FirstOrDefault()
                            };

                            ctxOw.EAIHOT.Add(eaihot);
                        }
                        ctx.SaveChanges();

                        string attributOrg = firstDecision.decision;
                        string attributcode = firstDecision.decisionCode;

                        if (string.IsNullOrEmpty(attributcode))
                        {
                            _log.Warn("Did not expect empty attribute code.");
                        }

                        var stateStruct = this.mapStates.FirstOrDefault(a => a.id == attributcode)
                                          //?? this.mapStates.FirstOrDefault(a => a.attribut == "Abgelehnt mit Auflagen")
                                          ?? new StateStruct()
                                          {
                                              attribut = "Technischer Fehler",
                                              id = attributcode,
                                              zustand = "Vorprüfung"
                                          };

                        _log.Debug("Mapped Original Decision " + attributOrg + " with code " + attributcode + " to final attribut " + stateStruct.attribut);

                        if (string.IsNullOrEmpty(stateStruct.zustand))
                        {
                            _log.Warn("No ZUSTAND found for Attribut " + stateStruct.attribut);
                        }
                        
                        // Update Antrag
                        var parameters = new List<OracleParameter>
                        {
                            new OracleParameter {ParameterName = "pZustand", Value = stateStruct.zustand},
                            new OracleParameter {ParameterName = "pAttribut", Value = stateStruct.attribut},
                            new OracleParameter {ParameterName = "sysid", Value = auskunftdto.sysid}
                        };
                        ctx.ExecuteStoreCommand("UPDATE ANTRAG SET ZUSTAND=:pZustand, ATTRIBUT=:pAttribut where sysid=:sysid", parameters.ToArray());

                        //HCERZWEI-392 Status an FO übertragen für copy/Wiedereinreichen-Button
                       /* String antragStatus = ctx.ExecuteStoreQuery<String>(@"SELECT extstate.zustand 
                                FROM attribut,attributdef,state,statedef extstate,statedef intstate,wftable,antrag 
                                WHERE attribut.sysstate = state.sysstate
                                AND attribut.sysattributdef= attributdef.sysattributdef
                                AND attribut.sysstatedef = extstate.sysstatedef
                                AND state.sysstatedef =intstate.sysstatedef
                                AND state.syswftable = wftable.syswftable
                                AND wftable.syscode = 'ANTRAG' and UPPER(attributdef.attribut) = upper(antrag.attribut) and upper(intstate.zustand)=upper(antrag.zustand)
                                and antrag.sysid=" + auskunftdto.sysid, null).FirstOrDefault();
                        long sysangebot = ctx.ExecuteStoreQuery<long>("select sysangebot from antrag where sysid=" + auskunftdto.sysid, null).FirstOrDefault();
                        if (sysangebot>0 &&antragStatus != null &&"Antragsänderung erfdl.".Equals(antragStatus))
                        {
                            ctx.ExecuteStoreCommand("UPDATE ANGEBOT SET ZUSTAND='Antragsänderung erfdl.' where sysid="+sysangebot,null);
                        }*/
                    }

                    foreach (var decision in resp.decision.Where(a => a.detail != null).Reverse())
                    {
                        checkBonitaetDecision(auskunftdto.sysid, decision);
                    }

                    //rating suchen+aktualisieren // select * from rating where flag1 = 0 and area = 'ANTRAG' and sysid = :pANTRAGID 
                    using (DdOwExtended ctx = new DdOwExtended())
                    {
                        long sysrating = ctx.ExecuteStoreQuery<long>("select sysrating from rating where flag1=0 and area='ANTRAG' and sysid=" + auskunftdto.sysid).FirstOrDefault();
                        int ratingstatus = 0;
                         
                        try
                        {
                            string RATINGQUERY = "select id from ddlkppos where code='ENTSCHEIDUNG' and value=:decision";
                            List<OracleParameter> parameters = new List<OracleParameter> {new OracleParameter {ParameterName = "decision", Value = firstDecision.decisionCode}};
                            ratingstatus = ctx.ExecuteStoreQuery<int>(RATINGQUERY, parameters.ToArray()).FirstOrDefault();
                        }
                        catch (Exception)
                        {
                            ratingstatus = 0;
                        }
                        List<OracleParameter> rupd = new List<OracleParameter> { new OracleParameter { ParameterName = "DATENTSCHEIDUNG", Value = firstDecision.date },
                                 new OracleParameter { ParameterName = "STATUS", Value = ratingstatus },
                                 new OracleParameter { ParameterName = "sysrating", Value = sysrating }};

                        ctx.ExecuteStoreCommand("update rating set status=:STATUS, DATENTSCHEIDUNG=:DATENTSCHEIDUNG where sysrating=:sysrating", rupd.ToArray());

                        var syswftable = (from t in ctx.WFTABLE
                            where t.SYSCODE.Equals("ANTRAG")
                            select t).Select(table => table.SYSWFTABLE).FirstOrDefault();


                        var maxRang = ctx.ExecuteStoreQuery<long>("select nvl(max(int03),0) from wfmmemo where STR01='EXT' and SYSLEASE=:sysAntrag and SYSWFMTABLE=:sysWftable",
                            new OracleParameter("sysAntrag", auskunftdto.sysid),
                            new OracleParameter("sysWftable", syswftable))
                            .FirstOrDefault();

                        var rang = maxRang + 1;

                        foreach (var decision in resp.decision)
                        {
                            var sysWfuser = ctx.ExecuteStoreQuery<long>("select syswfuser from wfuser where code=:pCode", new OracleParameter("pCode", (decision.editor ?? string.Empty).ToUpper())).FirstOrDefault();
                            if (sysWfuser == 0)
                            {
                                sysWfuser = ctx.ExecuteStoreQuery<long>("select syswfuser from wfuser where code='EAI'").FirstOrDefault();
                            }

                            //memos anlegen zu rating
                            createMemoIfNotEmpty(ctx, syswftable, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.conditionComment, sysdedefcon, "Auflagen", "AUFLAGE", null, null, rang);
                            createMemo(ctx, syswftable, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.decisionCommentExternal, sysrating, "externe Notiz", "EXT", null, null, rang);
                            createMemoIfNotEmpty(ctx, syswftable, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.decisionCommentInternal, sysrating, "interne Notiz", "INT", null, null, rang);
                            createMemoIfNotEmpty(ctx, syswftable, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.documentComment, sysrating, "Dokumentennotiz", "DOC", null, null, rang);
                            if (updateApplication && ddlkppos != null)
                            {
                                createMemoIfNotEmpty(ctx, syswftable, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", ddlkppos.VALUE, null, "Statusänderung", "CHGSTATE", "Guardean", decision.statusUpdateText, rang);
                            }
                        }

                        ctx.SaveChanges();
                    }
                    ctxOw.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _log.Error("Fehler beim Anlegen der Kreditentscheidung", e);
                throw e;

            }
        }

        //createMemoIfNotEmpty(ctx, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.conditionComment,          "AUFLAGE", sysdedefcon,      "Auflagen");
        //createMemoIfNotEmpty(ctx, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.decisionCommentExternal,   "EXT", rat.SYSRATING, "externe Notiz");
        //createMemoIfNotEmpty(ctx, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.decisionCommentInternal,   "INT", rat.SYSRATING, "interne Notiz");
        //createMemoIfNotEmpty(ctx, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.documentComment,           "DOC", rat.SYSRATING,         "Dokumentennotiz");
        //createMemoIfNotEmpty(ctx, auskunftdto.sysid, "ANTRAG", sysWfuser, "Entscheidung", decision.statusUpdateText,          "CHGSTATE", null,            "Statusänderung", "Guardean");
        private void createMemoIfNotEmpty(DdOwExtended ctx, long syswftable, long areaid, string area, long sysWfuser, string katBez, string memo, long? refId, string kurzbez, string str01, string str02 = null, string str03 = null, long int03 = 0)
        {
            if (string.IsNullOrWhiteSpace(memo))
            {
                return;
            }
            createMemo(ctx, syswftable, areaid, area, sysWfuser, katBez, memo, refId, kurzbez, str01, str02, str03, int03);
        }

        private void createMemo(DdOwExtended ctx, long syswftable, long areaid, string area, long sysWfuser, string katBez, string memo, long? refId, string kurzbez, string str01, string str02 = null, string str03 = null, long int03 = 0)
        {

            WFMMKAT kat = (from k in ctx.WFMMKAT
                where k.BESCHREIBUNG == katBez
                select k).FirstOrDefault();

            WFMMEMO wfmmemo = new WFMMEMO
            {
                CREATEDATE = OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                CREATETIME = OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                CREATEUSER = sysWfuser,
                SYSLEASE = areaid,
                SYSWFMTABLE = syswftable,
                WFMMKAT = kat,
                STR01 = str01,
                STR02 = str02,
                STR03 = str03,
                INT01 = refId,
                INT03 = int03,
                NOTIZMEMO = memo,
                KURZBESCHREIBUNG = kurzbez
            };

            ctx.WFMMEMO.Add(wfmmemo);
        }

        private void UpdatePersonIds(long sysperson, string schufaId, string crefoId, string ficoId)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter { ParameterName = "syskd", Value = sysperson }
            };

            string updateIds = string.Empty;

            if (schufaId != null)
            {
                parameters.Add(new OracleParameter { ParameterName = "schufaid", Value = schufaId });
                updateIds += ", SCHUFAID = :schufaid";
            }

            if (crefoId != null)
            {
                parameters.Add(new OracleParameter { ParameterName = "crefoid", Value = crefoId });
                updateIds += ", CREFOID = :crefoid";
            }

            if (ficoId != null)
            {
                parameters.Add(new OracleParameter { ParameterName = "ficoId", Value = ficoId });
                updateIds += ", FICOID = :ficoId";
            }

            if (updateIds != string.Empty)
            {
                updateIds = updateIds.TrimStart(',', ' ');
                using (DdOdExtended ctx = new DdOdExtended())
                {
                    ctx.ExecuteStoreCommand(string.Format("UPDATE PERSON SET {0} WHERE SYSPERSON=:syskd", updateIds), parameters.ToArray());
                }
            }
        }

        private void LogApplicant(InteressentBo applicant, AuskunftDto auskunftdto)
        {
            string addition = "";

            if (applicant.IsMa)
            {
                addition = " MA";
            }

            if (applicant.IsComplementary)
            {
                addition = " Complementary";
            }

            _log.Debug(string.Format("Antrag {0} has KD{1} with SYSPERSON {2}, SYSIT {3}", auskunftdto.sysid, addition, applicant.SysPerson, applicant.SysIt));

        }

        private void updateSysPersonOfIt(DdOdExtended ctx, long sysit, long syskd)
        {
            if (sysit != 0 && syskd != 0)
            {
                var parameters = new List<OracleParameter>
                        {
                            new OracleParameter { ParameterName = "sysit", Value = sysit },
                            new OracleParameter { ParameterName = "syskd", Value = syskd }
                        };
                ctx.ExecuteStoreCommand("UPDATE IT SET SYSPERSON=:syskd where sysit=:sysit", parameters.ToArray());
            }
        }



        /// <summary>
        /// Deliver Aggregation-Information for the Guardean decision process. SHS → CIC
        /// INT3
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public override SHS.W003.executeResponse getAggregation(SHS.W003.executeRequest req)
        {
            SHS.W003.executeResponse resp = new SHS.W003.executeResponse();
            if (req == null)
            {
                throw new ArgumentException("req not filled");
            }

            if (req.aggregatedRequest == null || req.aggregatedRequest.Length == 0)
            {
                throw new ArgumentException("req.aggregatedRequest not filled");
            }

            using (DdOlExtended ctx = new DdOlExtended())
            {
                resp.aggregatedResponse = new AggregatedResponseStruct[req.aggregatedRequest.Length];

                for (int index = 0; index < req.aggregatedRequest.Length; index++)
                {
                    var aggregatedRequest = req.aggregatedRequest[index];
                    if (aggregatedRequest.minimalApplicant == null)
                    {
                        _log.Warn("INT3: minimalApplicant not filled");
                        continue;
                    }

                    if (!aggregatedRequest.minimalApplicant.IDSpecified || aggregatedRequest.minimalApplicant.ID == 0)
                    {
                        _log.Warn("INT3: applicantID not filled");
                        continue;
                    }

                    var rs = new AggregatedResponseStruct();
                    resp.aggregatedResponse[index] = rs;
                    long sysKd = aggregatedRequest.minimalApplicant.ID;
                    _log.Debug("Aggregating sysperson " + sysKd);

                    rs.aggregatedApplicantData = new AggregatedApplicantDataStruct
                    {
                        applicantApplications = MyExecuteQuery<ApplicantApplicationsStruct>(ctx, "applicantApplications", DEGSql.APPLICANTAPPLICATIONS, ":pSysKd", "" + sysKd),
                        applicantContracts = MyExecuteQuery<ApplicantContractsStruct>(ctx, "applicantContracts", DEGSql.APPLICANTCONTRACTS, ":pSysKd", "" + sysKd),
                        applicantData = MyExecuteQuery<ApplicantDataStruct>(ctx, "applicantData", DEGSql.APPLICANTDATA, ":pSysKd", "" + sysKd)
                    };

                    CollateralInfoStruct[] infos = MyExecuteQueryArray<CollateralInfoStruct>(ctx, "collateralInfo", DEGSql.COLLATERALINFO, ":pSysKd", "" + sysKd);

                    if (infos != null && infos.Length > 0)
                    {
                        rs.aggregatedCollateralData = infos.Select(info =>
                            new AggregatedCollateralDataStruct()
                            {
                                collateralInfo = info
                            }).ToArray();

                    }


                    //für jeden ANTRAG
                    rs.existingApplications = MyExecuteQueryArray<ExistingApplicationsStruct>(ctx, "existingApplications", DEGSql.EXISTINGAPPLICATIONS, ":pSysKD", "" + sysKd);

                    //für jeden VT
                    rs.existingContracts = MyExecuteQueryArray<ExistingContractsStruct>(ctx, "existingContracts", DEGSql.EXISTINGCONTRACTS, ":pSysKD", "" + sysKd);
                    rs.applicantCreditlines = MyExecuteQueryArray<ApplicantCreditlinesStruct>(ctx, "applicantCreditlines", DEGSql.APPLICANTCREDITLINES, ":pSysKD", "" + sysKd);
                    rs.shares = MyExecuteQueryArray<SharesStruct>(ctx, "shares", DEGSql.SHARES, ":pSysKd", "" + sysKd);
                }
            }


            LogRequestResponse("SHS_INT3", "PERSON", req.aggregatedRequest.First().minimalApplicant.ID, req, resp);

            return resp;
        }

        private void LogRequestResponse(string description, string area, long id, object req, object resp)
        {
            DateTime currentDate = DateTime.Now;
            using (DdOwExtended ctx = new DdOwExtended())
            {
                ctx.LOGDUMP.Add(new LOGDUMP
                {
                    DESCRIPTION = description,
                    DUMPVALUE = XMLSerializer.Serialize(req, Encoding.UTF8.WebName),
                    DUMPDATE = currentDate,
                    DUMPTIME = DateTimeHelper.DateTimeToClarionTime(currentDate),
                    ART = 2,
                    INPUTFLAG = 1,
                    AREA = area,
                    SYSID = id
                });

                ctx.LOGDUMP.Add(new LOGDUMP
                {
                    DESCRIPTION = description,
                    DUMPVALUE = XMLSerializer.Serialize(resp, Encoding.UTF8.WebName),
                    DUMPDATE = currentDate,
                    DUMPTIME = DateTimeHelper.DateTimeToClarionTime(currentDate),
                    ART = 2,
                    INPUTFLAG = 0,
                    AREA = area,
                    SYSID = id
                });

                // dump response
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Returns the Liability Chain for the Guardean decision process. SHS → CIC
        /// INT6
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public override SHS.W006.executeResponse getLiabilityChain(SHS.W006.executeRequest req)
        {
            if (req == null)
            {
                throw new ArgumentException("req not filled");
            }

            long syskd;
            if (req == null || !long.TryParse(req.customerID, out syskd))
            {
                throw new ArgumentException("req.customerId not filled correctly");
            }


            SHS.W006.executeResponse response;

            using (DdOlExtended ctx = new DdOlExtended())
            {

                 response= new SHS.W006.executeResponse()
                {
                    groupRelation = new groupRelationStruct()
                    {
                        relationMember = MyExecuteQueryArray<relationMemberStruct>(ctx, "relationMembers", DEGSql.RELATIONMEMBERS, ":pSysKD", syskd.ToString())
                    }
                };
            }

            LogRequestResponse("SHS_INT6", "PERSON", syskd, req, response);
            return response;
        }

        /// <summary>
        /// Sets the customer check result from the Guardean decision process
        /// INT7
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public override SHS.W007.executeResponse setCustomerCheckResult(SHS.W007.executeRequest req)
        {
            long sysAuskunft;
            long.TryParse(req.application.externalReference, out sysAuskunft);
            var sysAntrag = GetSysAntrag(req.application.externalReference);

            // Prüfe ob ein SysAntrag referenziert ist
            if (sysAntrag == 0)
            {
                throw new ArgumentException("Did not find valid Antrag for externalReference: " + req.application.externalReference + ".");
            }

            if (!req.application.person.Any())
            {
                throw new ArgumentException("You need to specify at least one person.");
            }

            using (var context = new DdOdExtended())
            {
                foreach (var person in req.application.person.DistinctBy(pers => pers.keyId))
                {
                    long sysPerson;
                    long.TryParse(person.customerId, out sysPerson);
                    long sysIt;
                    long.TryParse(person.applicantId, out sysIt);

                    // Lädt SysPerson über Key ID
                    if (sysPerson == 0)
                    {
                        sysPerson = GetSysPersonFromKeyId(person.keyId.ToString());
                    }

                    // hier is sysPerson die übergeben wird als parameter nie 0
                    sysPerson = CreateOrUpdateKunde(sysAntrag, sysPerson, sysIt, person);

                    req.application.person.Where(a => a.keyId == person.keyId).ForEach((p, i) => { p.customerId = sysPerson.ToString(); });

                    updateSysPersonOfIt(context, sysIt, sysPerson);

                    // Update die Person Option mit der KeyId
                    UpdatePersonOption(sysPerson, person.keyId.ToString());

                    // Antragsteller UpdatePerson, Update ANTOBSICH, ANTRAG
                    // AKTIVKZ=1, FLAGKD=1
                    UpdateAntobsich(sysAntrag, sysPerson, sysIt);

                    if (HasRole(person, ApplicantRole.mainApplicant))
                    {
                        UpdateAntrag(sysAntrag, sysPerson);
                    }

                    // Erzeuge Bonität
                    CreateOrUpdateBonitaet(sysAntrag, sysPerson, person);
                }
            }

            // Zu diesem Zeitpunkt wurden alle Personen angelegt und die customerId ist von allen gesetzt.

            // Erzeuge KNE Einträge für  CBU
            CreateOrUpdateKneForCbus(sysAntrag, req.application.person.Where(p => HasRole(p, ApplicantRole.CBU)), req.application.person);

            // Erzeuge KNE Einträge für wirtschaftlich Berechtigte (wB) - UBO und Rest (Also alle complementaries)
            CreateOrUpdateKneForComplementary(sysAntrag, 
                req.application.person.Where(p => HasRole(p,
                    ApplicantRole.UBO,
                    ApplicantRole.CEO,
                    ApplicantRole.authorizedRepresentative,
                    ApplicantRole.AssociationCEO,
                    ApplicantRole.Complementary,
                    ApplicantRole.FoundationCEO,
                    ApplicantRole.LimitedPartner,
                    ApplicantRole.Owner,
                    ApplicantRole.Participants,
                    ApplicantRole.Partner,
                    ApplicantRole.Shareholder)),
                req.application.person);
            
            var response=  new SHS.W007.executeResponse()
            {
                personIds = req.application.person.Select(p => new PersonIdStruct()
                {
                    applicantId = p.applicantId,
                    keyId = p.keyId.ToString(),
                    customerID = p.customerId,
                }).ToArray()
            };

            LogRequestResponse("SHS_INT7", "AUSKUNFT", sysAuskunft, req, response);

            return response;
        }

        private long GetSysAntrag(string externalReference)
        {
            long sysAuskunft;
            long.TryParse(externalReference, out sysAuskunft);
            var auskunftdto = this.auskunftdao.FindBySysId(sysAuskunft);
            return auskunftdto.sysid;
        }

        private long GetSysPersonFromKeyId(string keyId)
        {
            using (var ctx = new DdOlExtended())
            {
                return ctx.PEOPTION.Where(a => a.STR04 == keyId).Select(a => a.SYSID).FirstOrDefault();
            }
        }

        private long GetSysPersonFromKeyId(long keyId)
        {
            return GetSysPersonFromKeyId(keyId.ToString());
        }
        
        private long CreateOrUpdateKunde(long sysAntrag, long sysPerson, long sysIt, PersonStruct person)
        {
            IKundeBo kdbo = BOFactory.getInstance().createKundeBo();
            if (sysPerson == 0)
            {
                _log.Debug("Creating PERSON for " + person.applicantRole + " for ANTRAG.sysid=" + sysAntrag + ": " + JsonConvert.SerializeObject(person));

                KundeDto kd = GetKundeFromIt(kdbo, sysIt, sysAntrag);

                kd.syskd = 0;

                //nur updaten wenn neuer kunde und noch nicht da
                kd.strasse = GetDefault(person.street, kd.strasse);
                kd.hsnr = GetDefault(person.housenumber, kd.hsnr);
                kd.plz = GetDefault(person.postalCode, kd.plz);
                kd.ort =  GetDefault(person.city, kd.ort);
                kd.sysland = this.landDao.GetSysLandFromIsoCountry(person.country);

                // falls person private, dann setze syskdtyp auf 1 (Privatperson)
                if (person.isPrivate)
                {
                    kd.syskdtyp = 1;
                }

                //kd.privatflag = (person.customerId.isPrivate == "Yes") ? 1 : 0;

                if (string.IsNullOrEmpty(person.companyName))
                {
                    kd.gebdatum = person.dateOfBirthSpecified ? person.dateOfBirth : kd.gebdatum;
                    kd.vorname = GetDefault(person.firstName, kd.vorname);
                    kd.name = GetDefault(person.lastName, kd.name);
                    //kd.anredeCode = GetAnredeCode(person.gender);
                    //kd.gebort = person.placeOfBirth;
                }
                else
                {
                    kd.name = GetDefault(person.companyName, kd.name);
                    kd.rechtsform = GetDefault(person.legalFormText, kd.rechtsform);
                    kd.rechtsformCode = GetDefault(person.legalFormCode, kd.rechtsformCode);
                    //kd.gruendung = person.foundationDate;
                }
                    
                kd.schufaid = GetDefault(person.schufaID, kd.schufaid);
                kd.crefoid = GetDefault(person.crefoID, kd.crefoid);
                kd.ficoid = GetDefault(person.ficoID, kd.ficoid);
                
                KundeDto kdPerson = kdbo.createOrUpdateKundePerson(kd, 0);

                if (sysIt > 0)
                {
                    kdbo.transferITPKZUKZToPERSON(kdPerson.syskd, sysIt, sysAntrag);
                }

                _log.Debug("Created new PERSON " + kdPerson.syskd);
                sysPerson = kdPerson.syskd;
            }
            else
            {
                if (sysIt > 0)
                {
                    kdbo.transferITPKZUKZToPKZUKZ(sysPerson, sysIt, sysAntrag);
                    kdbo.transferITPKZUKZToPERSON(sysPerson, sysIt, sysAntrag);
                    _log.Debug("Update PKZ/UKZ from ITPKZ/ITUKZ with SYSPERSON " + sysPerson + ", SYSIT " + sysIt);
                }

                UpdatePersonIds(sysPerson, person.schufaID, person.crefoID, person.ficoID);
            }
            
            
            return sysPerson;
        }

        private string GetDefault(string value, string valueIfNullOrEmpty)
        {
            return string.IsNullOrEmpty(value) ? valueIfNullOrEmpty : value;
        }

        private T GetDefault<T>(T value, T valueIfNullOrEmpty)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T)) ? valueIfNullOrEmpty : value;
        }


        private void UpdatePersonOption(long sysperson, string keyId)
        {
            if (string.IsNullOrEmpty(keyId))
                return;

            using (var ctx = new DdOlExtended())
            {
                var option = ctx.PEOPTION.FirstOrDefault(a => a.SYSID == sysperson);
                if (option == null)
                {
                    option = new PEOPTION()
                    {
                        SYSID = sysperson
                    };
                    ctx.PEOPTION.Add(option);
                }

                option.STR04 = keyId;
                ctx.SaveChanges();
            }
        }

        private void UpdateAntrag(long sysAntrag, long sysPerson)
        {
            using (var ctx = new DdOdExtended())
            {
                var parameters = new List<OracleParameter>
                {
                    new OracleParameter { ParameterName = "syskd", Value = sysPerson }, 
                    new OracleParameter { ParameterName = "sysid", Value = sysAntrag }
                };
                ctx.ExecuteStoreCommand("UPDATE ANTRAG SET SYSKD=:syskd where sysid=:sysid", parameters.ToArray());
            }
        }

        private void UpdateAntobsich(long sysAntrag, long sysPerson, long sysIt)
        {
            if (sysIt <= 0 || sysPerson <= 0 || sysAntrag <= 0) 
                return;

            using (var ctx = new DdOdExtended())
            {
                var parameters = new List<OracleParameter>
                {
                    new OracleParameter { ParameterName = "sysperson", Value = sysPerson },
                    new OracleParameter { ParameterName = "sysantrag", Value = sysAntrag },
                    new OracleParameter { ParameterName = "sysit", Value = sysIt }
                };
                ctx.ExecuteStoreCommand("UPDATE ANTOBSICH SET SYSMH=:sysperson where SYSIT=:sysit and antobsich.sysantrag = :sysantrag", parameters.ToArray());
            }
        }

        
        private void CreateOrUpdateBonitaet(long sysantrag, long sysPerson, PersonStruct person)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                BONITAET b = null;
                long sysbonitaet = ctx.ExecuteStoreQuery<int>(string.Format("select sysbonitaet from bonitaet where sysperson={0} AND sysantrag={1}", sysPerson, sysantrag), null).FirstOrDefault();
                long kdtyp = ctx.ExecuteStoreQuery<int>(string.Format("select typ from kdtyp k, person p where p.sysperson={0} and p.syskdtyp = k.syskdtyp", sysPerson), null).FirstOrDefault();
                if (sysbonitaet > 0)
                {
                    b = (from c in ctx.BONITAET
                        where c.SYSBONITAET == sysbonitaet
                        select c).FirstOrDefault();
                }
                else
                {
                    b = new BONITAET
                    {
                        RANG = 1,
                        SYSPERSON=sysPerson,
                        SYSANTRAG =sysantrag
                    };
                    ctx.BONITAET.Add(b);
                }
                long sysIt;
                long.TryParse(person.applicantId, out sysIt);
                b.SYSIT = sysIt;

                b.CREFONUMBER = GetDefault(b.CREFONUMBER, person.crefoID);
                b.SCHUFAID = GetDefault(b.SCHUFAID, person.schufaID);

                if (kdtyp == 1)
                {
                    long schufaPd;
                    long.TryParse(person.schufaScorecardScore, out schufaPd);

                    b.RATING = person.schufaSegment;
                    b.PD2 = schufaPd;
                }
                else if (kdtyp == 2)
                {
                    long pd2;
                    long.TryParse(GetDefault(person.crefoRatingPD, person.schufaScorecardScore), out pd2);
                    b.PD2 = pd2;
                    b.RATING = GetDefault(person.crefoRatingRiskClass, person.schufaSegment);
                }
                else if (kdtyp == 3)
                {
                    long pd2;
                    long.TryParse(person.crefoRatingPD, out pd2);
                    b.PD2 = pd2;
                    b.RATING = person.crefoRatingRiskClass;
                }

                ctx.SaveChanges();
            }
        }

        private void CreateOrUpdateKneForComplementary(long sysAntrag, IEnumerable<PersonStruct> personen, PersonStruct[] allPersonen)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                foreach (var person in personen)
                {
                    //var itkneQuote = ctx.ExecuteStoreQuery<decimal>("select quote from itkne where sysober = :pOber and sysunter = :pUnter and relatetypecode = :pCode and area = 'ANGEBOT' and sysarea = (select sysangebot from antrag where sysid = :pSysAntrag)", new OracleParameter());

                    var quote = person.totalShareSpecified ? (decimal?)person.totalShare : null;
                    
                    long parentKeyId = person.parentKeyId;

                    // wB zum Antragsteller steht über ihm. Deswegen ist der Antragsteller der Unter.
                    var sysUnterString = allPersonen.Where(a => a.keyId == parentKeyId).Select(a => a.customerId).FirstOrDefault();

                    long sysUnter;
                    long.TryParse(sysUnterString, out sysUnter);
                    long sysOber;
                    long.TryParse(person.customerId, out sysOber);


                    var sysKneTyp = kneTypMapping.GetIntValue("OHNE");

                    if (!sysKneTyp.HasValue)
                    {
                        throw new ArgumentException("CreateOrUpdateKneForUbos - Could not find KNETYP: 'OHNE'");
                    }

                    var relateTypeCode = relateTypeMapping.GetValue(person.applicantRole);

                    this.createOrUpdateKne(quote, sysOber, sysUnter, sysKneTyp.Value, relateTypeCode, "ANTRAG", sysAntrag);
                }
            }
        }

        private void CreateOrUpdateKneForCbus(long sysAntrag, IEnumerable<PersonStruct> cbus, PersonStruct[] personen)
        {
            var cbuList = cbus.ToList();
            var sysKneTyp = this.kneTypMapping.GetIntValue("KNE");

            if (!sysKneTyp.HasValue)
            {
                throw new ArgumentException("CreateOrUpdateKneForCbus - Could not find KNETYP: 'KNE'");
            }

            foreach (var grouping in cbuList.GroupBy(a=> a.parentKeyId))
            {
                var kneGroup = grouping.ToList();

                var heads = kneGroup.Where(a => a.cbuRelation == "HEAD").ToList();
                var members = kneGroup.Where(a => a.cbuRelation == "MEMBER").ToList();
                

                foreach (var head in heads)
                {
                    long sysOber;
                    long.TryParse(head.customerId, out sysOber);
                    
                    foreach (var member in members)
                    {
                        long sysUnter;
                        long.TryParse(member.customerId, out sysUnter);

                        createOrUpdateKne(null, sysOber, sysUnter, sysKneTyp.Value, null, "ANTRAG", sysAntrag);
                    }
                }
            }
        }
         
        

        private bool HasRole(PersonStruct person, params ApplicantRole[] roles)
        {
            ApplicantRole applicantRole;
            if (!Enum.TryParse(person.applicantRole, out applicantRole))
            {
                return false;
            }

            return roles.Contains(applicantRole);
        }


        private T MyExecuteQuery<T>(DdOlExtended context, string queryCfgId, string queryDefault, string paramName, string paramValue, string param2Name = null, string param2Value = null)
        {
            EaiparDao eaiParDao = new EaiparDao();
            string query = eaiParDao.getEaiParFileByCode(queryCfgId, queryDefault);
            query = query.Replace(paramName, paramValue);

            if (!string.IsNullOrEmpty(param2Name))
            {
                query = query.Replace(param2Name, param2Value ?? "");
            }

            int retries = 1 + Settings.Default.SQLRetryCount;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    DateTime startTime = DateTime.Now;
                    var result = context.ExecuteStoreQuery<T>(query, null).FirstOrDefault();
                    _log.Info(queryCfgId + " Query duration : " + (TimeSpan)(DateTime.Now - startTime));
                    return result;
                }
                catch (Exception ex)
                {
                    if (i == (retries - 1))
                    {
                        _log.Error("Retrying failed for " + query + ": ", ex);
                        throw new ServiceBaseException("1240", "Error in Query EAIPAR CODE " + queryCfgId, ex);
                    }
                    else
                    {
                        _log.Warn("Retrying query " + query + " because of " + ex.Message, ex);
                    }
                }
            }
            throw new Exception("Error in Query EAIPAR CODE " + queryCfgId);
        }

        private T[] MyExecuteQueryArray<T>(DdOlExtended context, string queryCfgId, string queryDefault, string paramName, string paramValue, string param2Name = null, string param2Value = null)
        {
            EaiparDao eaiParDao = new EaiparDao();
            string query = eaiParDao.getEaiParFileByCode(queryCfgId, queryDefault);
            query = query.Replace(paramName, paramValue);

            if (!string.IsNullOrEmpty(param2Name))
            {
                query = query.Replace(param2Name, param2Value ?? "");
            }

            int retries = 1 + Settings.Default.SQLRetryCount;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    DateTime startTime = DateTime.Now;
                    var result = context.ExecuteStoreQuery<T>(query, null).ToArray();
                    _log.Info(queryCfgId + " Query duration : " + (TimeSpan)(DateTime.Now - startTime));
                    return result;
                }
                catch (Exception ex)
                {
                    if (i == (retries - 1))
                    {
                        _log.Error("Retrying failed for " + query + ": ", ex);
                        throw new ServiceBaseException("1240", "Error in Query EAIPAR CODE " + queryCfgId, ex);
                    }
                    else
                    {
                        _log.Warn("Retrying query " + query + " because of " + ex.Message, ex);
                    }
                }
            }
            throw new Exception("Error in Query EAIPAR CODE " + queryCfgId);
        }
    }
}