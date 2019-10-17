using System;
using System.Reflection;
using System.Web;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Decision Engine Business Object
    /// 
    /// 
    /// <![CDATA[
    /// Decision-Engine-Updates (neues Feld):
    /// 
    /// XSD:
    /// ----
    /// Visual Studio command prompt in C:\Development\CIC.BOS\CIC.OpenOneGateBANKNOW\Cic.OpenOne.GateBANKNOW.Common\DTO öffnen.
    /// Erhaltenes neues xsd (z.B. RISK.L0036V0142.xsd) nach StrategyOne.xsd umbenennen und dort ablegen.
    /// Ausführen von:
    /// xsd StrategyOne.xsd /c /namespace:Cic.OpenOne.GateBANKNOW.Common.DTO
    /// 
    /// 
    /// C:\Development\CIC.BOS\CIC.OpenOneGateBANKNOW\Cic.OpenOne.GateBANKNOW.Common\DTO\Auskunft\DecisionEngineInDto.cs
    /// -> hier ist das neue Feld aufzunehmen
    /// 
    /// C:\Development\CIC.BOS\CIC.OpenOneGateBANKNOW\Cic.OpenOne.GateBANKNOW.Common\DAO\Auskunft\DecisionEngineDBDao.cs
    /// -> hier ist die Zuweisung InDto auf DEOBJECT zurchzuführen (MyMapEntitiesToInDto)
    /// 	
    /// C:\Development\CIC.BOS\CIC.OpenOneGateBANKNOW\Cic.OpenOne.GateBANKNOW.Common\DTO\BankNowModelProfile.cs
    /// -> hier ist das Mapping anzupassen
    /// CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.DecisionEngineInDto, CIC.Database.DE.EF6.Model.DEOBJECT>
    /// 
    /// C:\Development\CIC.BOS\CIC.OpenOneGateBANKNOW\Cic.OpenOne.GateBANKNOW.Common\BO\Auskunft\DecisionEngineBo.cs
    /// -> Zuweisung inDto auf Stubs von StrategyOne
    /// S1Request.Body.RecordNR.I_O_Restwertabsicherung = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Restwertabsicherung>().Create(inDto.Restwertabsicherung);
    /// 	
    /// C:\Development\CIC.BOS\CIC.OpenOneGateBANKNOW\Cic.OpenOne.GateBANKNOW.Common\BO\TransactionRisikoBO.cs
    /// -> Zuweisung Antragsdaten auf InDto in getRequestDEFromAntrag()
    /// ]]>
    /// </summary>
    public class DecisionEngineBo : AbstractDecisionEngineBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const long codeTechExc = -2;
        private const long codeSerAufExc = -1;

        private const int S1REQUEST_HEADER_LAYOUTVERSION = 1;

        /// <summary>
        /// Initializes IAuskunftDao, IDecisionEngineDBDao, IDecisionEngineWSDao
        /// </summary>
        /// <param name="dewsdao"></param>
        /// <param name="dedao"></param>
        /// <param name="auskunftdao"></param>
        public DecisionEngineBo(IDecisionEngineWSDao dewsdao, IDecisionEngineDBDao dedao, IAuskunftDao auskunftdao)
            : base(dewsdao, dedao, auskunftdao)
        {
        }

        /// <summary>
        /// Gets a filled DecisionEngineDto and saves it to database,
        /// Creates a new Auskunft and saves it to database,
        /// Calls DecisionEngine Webservice and saves output to database,
        /// Updates Auskunft
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>DecisionEngineOutDto</returns>
        public override AuskunftDto execute(DecisionEngineInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
            long sysAuskunft = auskunftdao.SaveAuskunft(AuskunfttypDao.DecisionEngineExecute);
            try
            {
                DecisionEngineOutDto outDto = new DecisionEngineOutDto();
                StrategyOneRequest S1Request = new StrategyOneRequest();

                // Fill S1Request with data from DecisionInDto
                // Fill Header 
                S1Request.Header = new StrategyOneRequestHeader();
                MyFillHeader(S1Request, inDto);

                // Fill Body
                S1Request.Body = new StrategyOneRequestBody();
                MyFillBody(S1Request, inDto);

                // Save Input
                dedao.SaveDecisionEngineInput(sysAuskunft, inDto);

                // Serialize
                String request = XMLSerializer.SerializeUTF8WithoutNamespace(S1Request);

                code = codeSerAufExc;

                //For report
                dewsdao.setSoapXMLDto(auskunftdao.getEntitySoapLog(sysAuskunft));

                // Send request away
                DAO.Auskunft.DecisionEngineRef.StrategyOneResponse response = dewsdao.execute(request, _log);
                code = codeTechExc;

                // Set ErrorCode and Message 
                outDto.ErrorCode = response.errorCode;
                outDto.Message = response.message;

                outDto.inDto = inDto;
                outDto.rrDto = inDto.RecordRRDto[0];

                // Deserialize
                StrategyOneResponse S1Response = (StrategyOneResponse)XMLSerializer.DeserializeUTF8(response.message, typeof(DTO.StrategyOneResponse));

                // Fill DecisionEngineOutDto
                outDto.response = S1Response;
                if (S1Response.Body != null)
                {
                    MyFillOutDto(outDto, S1Response);
                }

                // Save Output
                dedao.SaveDecisionEngineOutput(sysAuskunft, outDto);
                if (inDto.FlagBonitaetspruefung > 0)
                {
                    this.dedao.saveRatingergebnis(outDto, sysAuskunft);
                }

                // Update Auskunft
                if (outDto.Fehlercode != null)
                {
                    this.auskunftdao.UpdateAuskunft(sysAuskunft, (long)outDto.Fehlercode);
                }
                else
                {
                    this.auskunftdao.UpdateAuskunft(sysAuskunft, outDto.ErrorCode);
                }

                AuskunftDto auskunftDto = auskunftdao.FindBySysId(sysAuskunft);
                auskunftDto.DecisionEngineOutDto = outDto;
                auskunftDto.requestXML = dewsdao.getSoapXMLDto().requestXML;
                auskunftDto.responseXML = dewsdao.getSoapXMLDto().responseXML;
                return auskunftDto;
            }
            catch (Exception e)
            {
                auskunftdao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in kreditgesuchNeu!");
                throw new ApplicationException("Unexpected Exception in DecisionEngine!", e);
            }
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
            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftdao.FindBySysId(sysAuskunft);
            try
            {
                DecisionEngineOutDto outDto = new DecisionEngineOutDto();
                StrategyOneRequest S1Request = new StrategyOneRequest();

                // Get DecisionEngineInDto
                DecisionEngineInDto inDto = dedao.FindBySysId(sysAuskunft);
                auskunftdto.DecisionEngineInDto = inDto;
                auskunftdto.RecordRRDto = inDto.RecordRRDto[0];
                S1Request.Header = new StrategyOneRequestHeader();
                MyFillHeader(S1Request, inDto);

                // Fill Body
                S1Request.Body = new StrategyOneRequestBody();
                MyFillBody(S1Request, inDto);

                // Serialize
                String request = XMLSerializer.SerializeUTF8WithoutNamespace(S1Request);

                code = codeSerAufExc;

                //For report
                dewsdao.setSoapXMLDto(auskunftdao.getEntitySoapLog(sysAuskunft));

                // Send request away
                DAO.Auskunft.DecisionEngineRef.StrategyOneResponse response = dewsdao.execute(request, _log);
                code = codeTechExc;
                // Set ErrorCode and Message 
                outDto.ErrorCode = response.errorCode;
                outDto.Message = response.message;

                // Deserialize
                StrategyOneResponse S1Response = (StrategyOneResponse)XMLSerializer.DeserializeUTF8(response.message, typeof(DTO.StrategyOneResponse));

                if (S1Response.Body != null)
                {
                    MyFillOutDto(outDto, S1Response);
                    auskunftdto.DecisionEngineOutDto = outDto;
                }

                // Save Output
                dedao.SaveDecisionEngineOutput(sysAuskunft, outDto);
                if (inDto.FlagBonitaetspruefung > 0)
                {
                    this.dedao.saveRatingergebnis(outDto, sysAuskunft);
                }
                // Update Auskunft
                if (outDto.Fehlercode != null)
                {
                    this.auskunftdao.UpdateAuskunftDtoAuskunft(auskunftdto, (long)outDto.Fehlercode);
                }
                else
                {
                    this.auskunftdao.UpdateAuskunftDtoAuskunft(auskunftdto, outDto.ErrorCode);
                }

                auskunftdto.requestXML = dewsdao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = dewsdao.getSoapXMLDto().responseXML;

                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftdao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in Decision Engine!");
                throw new ApplicationException("Unexpected Exception in Decision Engine!", e);
            }
        }

        /// <summary>
        /// executeWithOutSaveexecute
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto executeSimulation(DecisionEngineInDto inDto)
        {
            long code = codeTechExc;
            // Save Auskunft
           AuskunftDto auskunftDto = new AuskunftDto();
            try
            {
                DecisionEngineOutDto outDto = new DecisionEngineOutDto();
                StrategyOneRequest S1Request = new StrategyOneRequest();

                // Fill S1Request with data from DecisionInDto
                // Fill Header 
                S1Request.Header = new StrategyOneRequestHeader();
                MyFillHeader(S1Request, inDto);

                // Fill Body
                S1Request.Body = new StrategyOneRequestBody();
                MyFillBody(S1Request, inDto);

               

                // Serialize
                String request = XMLSerializer.SerializeUTF8WithoutNamespace(S1Request);

                code = codeSerAufExc;

                //For report
                //dewsdao.setSoapXMLDto(auskunftdao.getEntitySoapLog(sysAuskunft));

                // Send request away
                DAO.Auskunft.DecisionEngineRef.StrategyOneResponse response = dewsdao.execute(request, _log);
                code = codeTechExc;

                // Set ErrorCode and Message 
                outDto.ErrorCode = response.errorCode;
                outDto.Message = response.message;

                outDto.inDto = inDto;
                outDto.rrDto = inDto.RecordRRDto[0];

                // Deserialize
                StrategyOneResponse S1Response = (StrategyOneResponse)XMLSerializer.DeserializeUTF8(response.message, typeof(DTO.StrategyOneResponse));

                if (S1Response.Body != null)
                {
                    MyFillOutDto(outDto, S1Response);
                   auskunftDto.DecisionEngineOutDto = outDto;
                }
                if (outDto.Fehlercode != null)
                {
                    auskunftDto.Fehlercode = outDto.Fehlercode.ToString();
                }
                else
                {
                    auskunftDto.Fehlercode = outDto.ErrorCode.ToString();
                }

                return auskunftDto;
                
            }
            catch (Exception)
            {
                auskunftDto.Fehlercode = code.ToString();
                return auskunftDto;
            }
        }
        

        #region Private methods
        private void MyFillHeader(StrategyOneRequest S1Request, DecisionEngineInDto inDto)
        {
            S1Request.Header.InquiryCode = inDto.InquiryCode;
            S1Request.Header.ProcessCode = inDto.ProcessCode;
            S1Request.Header.OrganizationCode = inDto.OrganizationCode;

            // ProcessVersion wird jetzt (ab 2011-10-06) im AdminBox gepflegt,
            // wird also doch übergeben. LayoutVersion wird nicht übergeben.
            if (inDto.ProcessVersion != null)
            {
                S1Request.Header.Item = (int)inDto.ProcessVersion;
                S1Request.Header.ItemElementName = ItemChoiceType.ProcessVersion;
            }
        }

        private void MyFillBody(StrategyOneRequest S1Request, DecisionEngineInDto inDto)
        {
            S1Request.Body.RecordNR = new StrategyOneRequestBodyRecordNR();
            if (inDto.RecordRRDto.Length == 2)
            {
                S1Request.Body.RecordRR = new StrategyOneRequestBodyRecord[2];
                S1Request.Body.RecordRR[0] = new StrategyOneRequestBodyRecord();
                S1Request.Body.RecordRR[1] = new StrategyOneRequestBodyRecord();
            }
            else
            {
                S1Request.Body.RecordRR = new StrategyOneRequestBodyRecord[1];
                S1Request.Body.RecordRR[0] = new StrategyOneRequestBodyRecord();
            }

            S1Request.Body.RecordNR.I_INQUIRY_DATE = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_INQUIRY_DATE>().Create(inDto.InquiryDate);
            S1Request.Body.RecordNR.I_INQUIRY_TIME = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_INQUIRY_TIME>().Create(inDto.InquiryTime);

            // I_IO (DEEnvInp, Envelope)
            S1Request.Body.RecordNR.I_IO_VP = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_IO_VP>().Create(inDto.FlagVorpruefung);
            S1Request.Body.RecordNR.I_IO_BP = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_IO_BP>().Create(inDto.FlagBonitaetspruefung);
            S1Request.Body.RecordNR.I_IO_RP = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_IO_RP>().Create(inDto.FlagRisikopruefung);
            
            S1Request.Body.RecordNR.I_IO_Erfassung = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_IO_Erfassung>().Create(inDto.Erfassung);

            S1Request.Body.RecordNR.I_IO_CICONEVERSION = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_IO_CICONEVERSION>().Create(inDto.Ciconeversion);

            // I_O - OBJEKT
            S1Request.Body.RecordNR.I_O_1_Inverkehrssetzung = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_1_Inverkehrssetzung>().Create(inDto.Inverkehrssetzung);
            S1Request.Body.RecordNR.I_O_Fahrzeugpreis_Eurotax = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Fahrzeugpreis_Eurotax>().Create(inDto.Fahrzeugpreis_Eurotax);
            S1Request.Body.RecordNR.I_O_Katalogpreis_Eurotax = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Katalogpreis_Eurotax>().Create(inDto.Katalogpreis_Eurotax);
            S1Request.Body.RecordNR.I_O_KM_prJahr = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_KM_prJahr>().Create(inDto.KM_prJahr);
            S1Request.Body.RecordNR.I_O_KM_Stand = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_KM_Stand>().Create(inDto.KM_Stand);
            S1Request.Body.RecordNR.I_O_Marke = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Marke>().Create(inDto.Marke);
            S1Request.Body.RecordNR.I_O_Modell = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Modell>().Create(inDto.Modell);
            S1Request.Body.RecordNR.I_O_Objektart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Objektart>().Create(inDto.Objektart); // v = enum ObjektartV
            S1Request.Body.RecordNR.I_O_Restwert_BANK_now = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Restwert_BANK_now>().Create(inDto.Restwert_Banknow);
            S1Request.Body.RecordNR.I_O_Restwert_Eurotax = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Restwert_Eurotax>().Create(inDto.Restwert_Eurotax);
            S1Request.Body.RecordNR.I_O_Stammnummer = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Stammnummer>().Create(inDto.Stammnummer);
            S1Request.Body.RecordNR.I_O_Zubehoerpreis = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Zubehoerpreis>().Create(inDto.Zubehoerpreis);
            S1Request.Body.RecordNR.I_O_Zustand = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Zustand>().Create(inDto.Zustand);

            //*BNRACHT-615
            S1Request.Body.RecordNR.I_O_Marktwert_Cluster = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Marktwert_Cluster>().Create(inDto.Marktwert_Cluster);
            S1Request.Body.RecordNR.I_O_Expected_Loss = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Expected_Loss>().Create(inDto.Expected_Loss);
            S1Request.Body.RecordNR.I_O_Expected_Loss_Prozent = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Expected_Loss_Prozent>().Create(inDto.Expected_Loss_Prozent);
            S1Request.Body.RecordNR.I_O_Expected_Loss_LGD = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Expected_Loss_LGD>().Create(inDto.Expected_Loss_LGD);
            S1Request.Body.RecordNR.I_O_Profitabilitaet_Prozent = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Profitabilitaet_Prozent>().Create(inDto.Profitabilitaet_Prozent);

            
            // I_C - CONTRACT
            S1Request.Body.RecordNR.I_C_Anzahlung_Erste_Rate = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Anzahlung_Erste_Rate>().Create(inDto.Anzahlung_ErsteRate);
            
            S1Request.Body.RecordNR.I_C_Budgetueberschuss_1 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Budgetueberschuss_1>().Create(inDto.Budgetueberschuss_1);
            S1Request.Body.RecordNR.I_C_Budgetueberschuss_2 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Budgetueberschuss_2>().Create(inDto.Budgetueberschuss_2);
            S1Request.Body.RecordNR.I_C_Budgetueberschuss_gesamt = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Budgetueberschuss_gesamt>().Create(inDto.Budgetueberschuss_gesamt);
            
            S1Request.Body.RecordNR.I_C_Erfassungskanal = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Erfassungskanal>().Create(inDto.Erfassungskanal);     // v = enum ErfassungskanalV
            S1Request.Body.RecordNR.I_C_Finanzierungsbetrag = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Finanzierungsbetrag>().Create(inDto.Finanzierungsbetrag);
            S1Request.Body.RecordNR.I_C_Geschaeftsart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Geschaeftsart>().Create(inDto.Geschaeftsart); // v = enum GeschaeftsartV
            S1Request.Body.RecordNR.I_C_Kaution = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Kaution>().Create(inDto.Kaution);
            S1Request.Body.RecordNR.I_C_KKG_Pflicht = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_KKG_Pflicht>().Create(inDto.KKG_Pflicht);
            S1Request.Body.RecordNR.I_C_Laufzeit = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Laufzeit>().Create(inDto.Laufzeit);

            S1Request.Body.RecordNR.I_C_Nutzungsart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Nutzungsart>().Create(inDto.Nutzungsart);   // v = enum NutzungsartV 
            S1Request.Body.RecordNR.I_C_PPI_Flag_Paket1 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_PPI_Flag_Paket1>().Create(inDto.PPI_Flag_Paket1);
            S1Request.Body.RecordNR.I_C_PPI_Flag_Paket2 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_PPI_Flag_Paket2>().Create(inDto.PPI_Flag_Paket2);
            // BNRSZ-1371 StrategyOne XSD
            S1Request.Body.RecordNR.I_C_PPI_Betrag = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_PPI_Betrag>().Create(inDto.PPI_Betrag);
            S1Request.Body.RecordNR.I_C_Rate = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Rate>().Create(inDto.Rate);
            S1Request.Body.RecordNR.I_C_Restwert = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Restwert>().Create(inDto.Restwert);
            S1Request.Body.RecordNR.I_C_Riskflag = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Riskflag>().Create(inDto.Riskflag);
            S1Request.Body.RecordNR.I_C_Vertragsart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Vertragsart>().Create(inDto.Vertragsart);
            S1Request.Body.RecordNR.I_C_Zinssatz = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Zinssatz>().Create(inDto.Zinssatz);
            S1Request.Body.RecordNR.I_C_KREMO_Fehlercode = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_KREMO_Fehlercode>().Create(inDto.KremoCode);
            S1Request.Body.RecordNR.I_C_Auszahlungsart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Auszahlungsart>().Create(inDto.AuszahlungsArt);
            S1Request.Body.RecordNR.I_C_Buchwertgarantie = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Buchwertgarantie>().Create(inDto.BuchwertGarantie);
            // BNR17 
            S1Request.Body.RecordNR.I_C_Erneute_Pruefung  = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Erneute_Pruefung>().Create(inDto.Erneute_Pruefung);
            S1Request.Body.RecordNR.I_C_Finanzierungsbetrag_Bewilligt = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Finanzierungsbetrag_Bewilligt>().Create(inDto.Finanzierungsbetragbew);
            S1Request.Body.RecordNR.I_C_PPI_Betrag_Bewilligt = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_PPI_Betrag_Bewilligt>().Create(inDto.PPI_Betrag_Bewilligt);
            S1Request.Body.RecordNR.I_C_Toleranzbetrag_Risk_Decision = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Toleranzbetrag_Risk_Decision>().Create(inDto.Toleranzriskdec);






            // BNR7
            S1Request.Body.RecordNR.I_C_VTTYP = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_VTTYP>().Create(inDto.sysVTTYP);
            S1Request.Body.RecordNR.I_C_Umschreibung = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Umschreibung>().Create(inDto.Umschreibung);

            //BNR10
            S1Request.Body.RecordNR.I_C_Random_Number = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Random_Number>().Create(inDto.RandomNumber);

            // BNR11
            S1Request.Body.RecordNR.I_C_RW_Verl = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_RW_Verl>().Create(inDto.RW_Verl);
            S1Request.Body.RecordNR.I_C_Vertragsversion_NEU = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Vertragsversion_NEU>().Create(inDto.Vertragsversion_NEU);
            // BNR13
            S1Request.Body.RecordNR.I_C_Restwertgarant = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_C_Restwertgarant>().Create(inDto.Restwertgarant);

            // I_S - SALES ABLÖSEDATEN
            S1Request.Body.RecordNR.I_S_Anz_Abloesen = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Anz_Abloesen>().Create(inDto.Anz_Abloesen);
            S1Request.Body.RecordNR.I_S_Anz_Eigenabloesen = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Anz_Eigenabloesen>().Create(inDto.Anz_Eigenabloesen);
            S1Request.Body.RecordNR.I_S_Anz_Fremdabloesen = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Anz_Fremdabloesen>().Create(inDto.Anz_Fremdabloesen);
            S1Request.Body.RecordNR.I_S_Name_Abloesebank_1 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Name_Abloesebank_1>().Create(inDto.Name_Abloesebank_1);
            S1Request.Body.RecordNR.I_S_Name_Abloesebank_2 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Name_Abloesebank_2>().Create(inDto.Name_Abloesebank_2);
            S1Request.Body.RecordNR.I_S_Name_Abloesebank_3 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Name_Abloesebank_3>().Create(inDto.Name_Abloesebank_3);
            S1Request.Body.RecordNR.I_S_Name_Abloesebank_4 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Name_Abloesebank_4>().Create(inDto.Name_Abloesebank_4);
            S1Request.Body.RecordNR.I_S_Name_Abloesebank_5 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Name_Abloesebank_5>().Create(inDto.Name_Abloesebank_5);
            S1Request.Body.RecordNR.I_S_Summe_Abloesen = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Summe_Abloesen>().Create(inDto.Summe_Abloesen);
            S1Request.Body.RecordNR.I_S_Summe_Eigenabloesen = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Summe_Eigenabloesen>().Create(inDto.Summe_Eigenabloesen);
            S1Request.Body.RecordNR.I_S_Summe_Fremdabloesen = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Summe_Fremdabloesen>().Create(inDto.Summe_Fremdabloesen);

            S1Request.Body.RecordNR.I_S_Validabl = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Validabl>().Create(inDto.Validabl);



            //BNR13 
            S1Request.Body.RecordNR.I_S_Abl_Produkt = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Abl_Produkt>().Create(inDto.Abl_Produkt);
            S1Request.Body.RecordNR.I_S_Abl_Dauer_Total = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Abl_Dauer_Total>().Create(inDto.Abl_Dauer_Total);
            S1Request.Body.RecordNR.I_S_Abl_Anz_Vertragsart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Abl_Anz_Vertragsart>().Create(inDto.Abl_Anz_Vertragsart);
            S1Request.Body.RecordNR.I_S_Abl_LZ_Vorvertrag_Vertragsart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Abl_LZ_Vorvertrag_Vertragsart>().Create(inDto.Abl_LZ_Vorvertrag_Vertragsart);
            S1Request.Body.RecordNR.I_S_Abl_Dauer_Vertragsart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_S_Abl_Dauer_Vertragsart>().Create(inDto.Abl_Dauer_Vertragsart); 

            // I_VP - VERTRIEBSPARTNER
            S1Request.Body.RecordNR.I_VP_Anz_Antraege = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Anz_Antraege>().Create(inDto.Anz_Antraege);
            S1Request.Body.RecordNR.I_VP_Anz_lfd_Vertraege = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Anz_lfd_Vertraege>().Create(inDto.Anz_lfd_Vertraege);
            S1Request.Body.RecordNR.I_VP_Anz_pendente_Antraege = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Anz_pendente_Antraege>().Create(inDto.Anz_pendente_Antraege);
            S1Request.Body.RecordNR.I_VP_Anz_Vertraege = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Anz_Vertraege>().Create(inDto.Anz_Vertraege);
            S1Request.Body.RecordNR.I_VP_Eventualrestwertengagement = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Eventualrestwertengagement>().Create(inDto.Eventualrestwertengagement);
            S1Request.Body.RecordNR.I_VP_Eventualvolumenengagement = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Eventualvolumenengagement>().Create(inDto.Eventualvolumenengagement);
            S1Request.Body.RecordNR.I_VP_flagAktiv = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_flagAktiv>().Create(inDto.flagAktiv);
            S1Request.Body.RecordNR.I_VP_flagEPOS = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_flagEPOS>().Create(inDto.flagEPOS);
            S1Request.Body.RecordNR.I_VP_flagVSB = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_flagVSB>().Create(inDto.flagVSB);
            S1Request.Body.RecordNR.I_VP_Garantenlimite = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Garantenlimite>().Create(inDto.Garantenlimite);
            S1Request.Body.RecordNR.I_VP_PLZ = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_PLZ>().Create(inDto.PLZ);
            S1Request.Body.RecordNR.I_VP_Rechtsform = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Rechtsform>().Create(inDto.Rechtsform); // v = enum RechtsformV
            S1Request.Body.RecordNR.I_VP_Restwertengagement = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Restwertengagement>().Create(inDto.Restwertengagement);
            S1Request.Body.RecordNR.I_VP_Sprache = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Sprache>().Create(inDto.Sprache); // v = enum SpracheV
            S1Request.Body.RecordNR.I_VP_Vertriebspartnerart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Vertriebspartnerart>().Create(inDto.Vertriebspartnerart);
            S1Request.Body.RecordNR.I_VP_VertriebspartnerID = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_VertriebspartnerID>().Create(inDto.VertriebspartnerID);
            S1Request.Body.RecordNR.I_VP_Volumenengagement = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Volumenengagement>().Create(inDto.Volumenengagement);

            S1Request.Body.RecordNR.I_VP_UID = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_UID>().Create(inDto.UIDNummer);

            //CR453
            S1Request.Body.RecordNR.I_O_VIN_Nummer = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_VIN_Nummer>().Create(inDto.VIN_Nummer);

            //BNRAZ-449 CR 479
            S1Request.Body.RecordNR.I_O_Restwertabsicherung = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Restwertabsicherung>().Create(inDto.Restwertabsicherung);


            //BNRAZ-449 CR 480
            S1Request.Body.RecordNR.I_O_Ausstattung = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_O_Ausstattung>().Create(inDto.Ausstattung);
            S1Request.Body.RecordNR.I_VP_Demolimite = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Demolimite>().Create(inDto.Demolimit);
            S1Request.Body.RecordNR.I_VP_Demoengagement = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Demoengagement>().Create(inDto.Demoengagement);
            S1Request.Body.RecordNR.I_VP_Eventualdemoengagement = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Eventualdemoengagement>().Create(inDto.Eventualdemoengagement);
            

            //*BNRACHT-615
            S1Request.Body.RecordNR.I_VP_Strategic_Account = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Strategic_Account>().Create(inDto.Strategic_Account);
            S1Request.Body.RecordNR.I_VP_Badlisteintrag = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Badlisteintrag>().Create(inDto.Badlisteintrag);
            
            // BNR10
            S1Request.Body.RecordNR.I_VP_Vermittlerart = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_VP_Vermittlerart>().Create(inDto.Vermittlerart);

            // BNR11
            S1Request.Body.RecordNR.I_IO_Flagsimulation = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_IO_Flagsimulation>().Create(inDto.Flagsimulation);

            if (inDto.I_F_Values != null && inDto.I_F_Values.Length >= 101)
            { 
                    S1Request.Body.RecordNR.I_F_Value_001 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_001>().Create(inDto.I_F_Values[1]);
                    S1Request.Body.RecordNR.I_F_Value_002 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_002>().Create(inDto.I_F_Values[2]);
                    S1Request.Body.RecordNR.I_F_Value_003 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_003>().Create(inDto.I_F_Values[3]);
                    S1Request.Body.RecordNR.I_F_Value_004 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_004>().Create(inDto.I_F_Values[4]);
                    S1Request.Body.RecordNR.I_F_Value_005 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_005>().Create(inDto.I_F_Values[5]);
                    S1Request.Body.RecordNR.I_F_Value_006 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_006>().Create(inDto.I_F_Values[6]);
                    S1Request.Body.RecordNR.I_F_Value_007 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_007>().Create(inDto.I_F_Values[7]);
                    S1Request.Body.RecordNR.I_F_Value_008 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_008>().Create(inDto.I_F_Values[8]);
                    S1Request.Body.RecordNR.I_F_Value_009 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_009>().Create(inDto.I_F_Values[9]);
                    S1Request.Body.RecordNR.I_F_Value_010 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_010>().Create(inDto.I_F_Values[10]);

                    S1Request.Body.RecordNR.I_F_Value_011 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_011>().Create(inDto.I_F_Values[11]);
                    S1Request.Body.RecordNR.I_F_Value_012 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_012>().Create(inDto.I_F_Values[12]);
                    S1Request.Body.RecordNR.I_F_Value_013 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_013>().Create(inDto.I_F_Values[13]);
                    S1Request.Body.RecordNR.I_F_Value_014 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_014>().Create(inDto.I_F_Values[14]);
                    S1Request.Body.RecordNR.I_F_Value_015 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_015>().Create(inDto.I_F_Values[15]);
                    S1Request.Body.RecordNR.I_F_Value_016 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_016>().Create(inDto.I_F_Values[16]);
                    S1Request.Body.RecordNR.I_F_Value_017 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_017>().Create(inDto.I_F_Values[17]);
                    S1Request.Body.RecordNR.I_F_Value_018 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_018>().Create(inDto.I_F_Values[18]);
                    S1Request.Body.RecordNR.I_F_Value_019 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_019>().Create(inDto.I_F_Values[19]);
                    S1Request.Body.RecordNR.I_F_Value_020 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_020>().Create(inDto.I_F_Values[20]);

                    S1Request.Body.RecordNR.I_F_Value_021 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_021>().Create(inDto.I_F_Values[21]);
                    S1Request.Body.RecordNR.I_F_Value_022 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_022>().Create(inDto.I_F_Values[22]);
                    S1Request.Body.RecordNR.I_F_Value_023 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_023>().Create(inDto.I_F_Values[23]);
                    S1Request.Body.RecordNR.I_F_Value_024 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_024>().Create(inDto.I_F_Values[24]);
                    S1Request.Body.RecordNR.I_F_Value_025 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_025>().Create(inDto.I_F_Values[25]);
                    S1Request.Body.RecordNR.I_F_Value_026 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_026>().Create(inDto.I_F_Values[26]);
                    S1Request.Body.RecordNR.I_F_Value_027 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_027>().Create(inDto.I_F_Values[27]);
                    S1Request.Body.RecordNR.I_F_Value_028 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_028>().Create(inDto.I_F_Values[28]);
                    S1Request.Body.RecordNR.I_F_Value_029 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_029>().Create(inDto.I_F_Values[29]);
                    S1Request.Body.RecordNR.I_F_Value_030 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_030>().Create(inDto.I_F_Values[30]);

                    S1Request.Body.RecordNR.I_F_Value_031 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_031>().Create(inDto.I_F_Values[31]);
                    S1Request.Body.RecordNR.I_F_Value_032 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_032>().Create(inDto.I_F_Values[32]);
                    S1Request.Body.RecordNR.I_F_Value_033 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_033>().Create(inDto.I_F_Values[33]);
                    S1Request.Body.RecordNR.I_F_Value_034 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_034>().Create(inDto.I_F_Values[34]);
                    S1Request.Body.RecordNR.I_F_Value_035 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_035>().Create(inDto.I_F_Values[35]);
                    S1Request.Body.RecordNR.I_F_Value_036 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_036>().Create(inDto.I_F_Values[36]);
                    S1Request.Body.RecordNR.I_F_Value_037 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_037>().Create(inDto.I_F_Values[37]);
                    S1Request.Body.RecordNR.I_F_Value_038 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_038>().Create(inDto.I_F_Values[38]);
                    S1Request.Body.RecordNR.I_F_Value_039 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_039>().Create(inDto.I_F_Values[39]);
                    S1Request.Body.RecordNR.I_F_Value_040 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_040>().Create(inDto.I_F_Values[40]);

                    S1Request.Body.RecordNR.I_F_Value_041 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_041>().Create(inDto.I_F_Values[41]);
                    S1Request.Body.RecordNR.I_F_Value_042 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_042>().Create(inDto.I_F_Values[42]);
                    S1Request.Body.RecordNR.I_F_Value_043 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_043>().Create(inDto.I_F_Values[43]);
                    S1Request.Body.RecordNR.I_F_Value_044 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_044>().Create(inDto.I_F_Values[44]);
                    S1Request.Body.RecordNR.I_F_Value_045 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_045>().Create(inDto.I_F_Values[45]);
                    S1Request.Body.RecordNR.I_F_Value_046 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_046>().Create(inDto.I_F_Values[46]);
                    S1Request.Body.RecordNR.I_F_Value_047 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_047>().Create(inDto.I_F_Values[47]);
                    S1Request.Body.RecordNR.I_F_Value_048 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_048>().Create(inDto.I_F_Values[48]);
                    S1Request.Body.RecordNR.I_F_Value_049 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_049>().Create(inDto.I_F_Values[49]);
                    S1Request.Body.RecordNR.I_F_Value_050 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_050>().Create(inDto.I_F_Values[50]);

                    S1Request.Body.RecordNR.I_F_Value_051 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_051>().Create(inDto.I_F_Values[51]);
                    S1Request.Body.RecordNR.I_F_Value_052 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_052>().Create(inDto.I_F_Values[52]);
                    S1Request.Body.RecordNR.I_F_Value_053 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_053>().Create(inDto.I_F_Values[53]);
                    S1Request.Body.RecordNR.I_F_Value_054 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_054>().Create(inDto.I_F_Values[54]);
                    S1Request.Body.RecordNR.I_F_Value_055 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_055>().Create(inDto.I_F_Values[55]);
                    S1Request.Body.RecordNR.I_F_Value_056 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_056>().Create(inDto.I_F_Values[56]);
                    S1Request.Body.RecordNR.I_F_Value_057 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_057>().Create(inDto.I_F_Values[57]);
                    S1Request.Body.RecordNR.I_F_Value_058 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_058>().Create(inDto.I_F_Values[58]);
                    S1Request.Body.RecordNR.I_F_Value_059 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_059>().Create(inDto.I_F_Values[59]);
                    S1Request.Body.RecordNR.I_F_Value_060 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_060>().Create(inDto.I_F_Values[60]);

                    S1Request.Body.RecordNR.I_F_Value_061 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_061>().Create(inDto.I_F_Values[61]);
                    S1Request.Body.RecordNR.I_F_Value_062 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_062>().Create(inDto.I_F_Values[62]);
                    S1Request.Body.RecordNR.I_F_Value_063 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_063>().Create(inDto.I_F_Values[63]);
                    S1Request.Body.RecordNR.I_F_Value_064 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_064>().Create(inDto.I_F_Values[64]);
                    S1Request.Body.RecordNR.I_F_Value_065 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_065>().Create(inDto.I_F_Values[65]);
                    S1Request.Body.RecordNR.I_F_Value_066 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_066>().Create(inDto.I_F_Values[66]);
                    S1Request.Body.RecordNR.I_F_Value_067 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_067>().Create(inDto.I_F_Values[67]);
                    S1Request.Body.RecordNR.I_F_Value_068 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_068>().Create(inDto.I_F_Values[68]);
                    S1Request.Body.RecordNR.I_F_Value_069 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_069>().Create(inDto.I_F_Values[69]);
                    S1Request.Body.RecordNR.I_F_Value_070 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_070>().Create(inDto.I_F_Values[70]);

                    S1Request.Body.RecordNR.I_F_Value_071 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_071>().Create(inDto.I_F_Values[71]);
                    S1Request.Body.RecordNR.I_F_Value_072 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_072>().Create(inDto.I_F_Values[72]);
                    S1Request.Body.RecordNR.I_F_Value_073 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_073>().Create(inDto.I_F_Values[73]);
                    S1Request.Body.RecordNR.I_F_Value_074 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_074>().Create(inDto.I_F_Values[74]);
                    S1Request.Body.RecordNR.I_F_Value_075 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_075>().Create(inDto.I_F_Values[75]);
                    S1Request.Body.RecordNR.I_F_Value_076 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_076>().Create(inDto.I_F_Values[76]);
                    S1Request.Body.RecordNR.I_F_Value_077 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_077>().Create(inDto.I_F_Values[77]);
                    S1Request.Body.RecordNR.I_F_Value_078 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_078>().Create(inDto.I_F_Values[78]);
                    S1Request.Body.RecordNR.I_F_Value_079 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_079>().Create(inDto.I_F_Values[79]);
                    S1Request.Body.RecordNR.I_F_Value_080 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_080>().Create(inDto.I_F_Values[80]);

                    S1Request.Body.RecordNR.I_F_Value_081 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_081>().Create(inDto.I_F_Values[81]);
                    S1Request.Body.RecordNR.I_F_Value_082 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_082>().Create(inDto.I_F_Values[82]);
                    S1Request.Body.RecordNR.I_F_Value_083 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_083>().Create(inDto.I_F_Values[83]);
                    S1Request.Body.RecordNR.I_F_Value_084 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_084>().Create(inDto.I_F_Values[84]);
                    S1Request.Body.RecordNR.I_F_Value_085 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_085>().Create(inDto.I_F_Values[85]);
                    S1Request.Body.RecordNR.I_F_Value_086 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_086>().Create(inDto.I_F_Values[86]);
                    S1Request.Body.RecordNR.I_F_Value_087 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_087>().Create(inDto.I_F_Values[87]);
                    S1Request.Body.RecordNR.I_F_Value_088 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_088>().Create(inDto.I_F_Values[88]);
                    S1Request.Body.RecordNR.I_F_Value_089 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_089>().Create(inDto.I_F_Values[89]);
                    S1Request.Body.RecordNR.I_F_Value_090 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_090>().Create(inDto.I_F_Values[90]);

                    S1Request.Body.RecordNR.I_F_Value_091 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_091>().Create(inDto.I_F_Values[91]);
                    S1Request.Body.RecordNR.I_F_Value_092 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_092>().Create(inDto.I_F_Values[92]);
                    S1Request.Body.RecordNR.I_F_Value_093 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_093>().Create(inDto.I_F_Values[93]);
                    S1Request.Body.RecordNR.I_F_Value_094 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_094>().Create(inDto.I_F_Values[94]);
                    S1Request.Body.RecordNR.I_F_Value_095 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_095>().Create(inDto.I_F_Values[95]);
                    S1Request.Body.RecordNR.I_F_Value_096 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_096>().Create(inDto.I_F_Values[96]);
                    S1Request.Body.RecordNR.I_F_Value_097 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_097>().Create(inDto.I_F_Values[97]);
                    S1Request.Body.RecordNR.I_F_Value_098 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_098>().Create(inDto.I_F_Values[98]);
                    S1Request.Body.RecordNR.I_F_Value_099 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_099>().Create(inDto.I_F_Values[99]);
                    S1Request.Body.RecordNR.I_F_Value_100 = new DEBodyRecord<StrategyOneRequestBodyRecordNRI_F_Value_100>().Create(inDto.I_F_Values[100]);

            }



            // Fill RecordRR (198 optionale Inputparameter)
            // I_A
            for (int i = 0; i < S1Request.Body.RecordRR.Length; i++)
            {
                S1Request.Body.RecordRR[i].I_A_13_Monatslohn = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_13_Monatslohn>().Create(inDto.RecordRRDto[i].A_13_Montaslohn);
                S1Request.Body.RecordRR[i].I_A_A_PID = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_A_PID>().Create(inDto.RecordRRDto[i].A_A_PID);
                S1Request.Body.RecordRR[i].I_A_Anz_der_Betreibungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Anz_der_Betreibungen>().Create(inDto.RecordRRDto[i].A_Anz_der_Betreibungen);
                S1Request.Body.RecordRR[i].I_A_Anz_Kinder_bis_6 = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Anz_Kinder_bis_6>().Create(inDto.RecordRRDto[i].A_Anz_Kinder_bis_6);
                S1Request.Body.RecordRR[i].I_A_Anz_Kinder_ueber_10_bis_12 = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Anz_Kinder_ueber_10_bis_12>().Create(inDto.RecordRRDto[i].A_Anz_Kinder_ueber_10_bis_12);
                S1Request.Body.RecordRR[i].I_A_Anz_Kinder_ueber_6_bis_10 = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Anz_Kinder_ueber_6_bis_10>().Create(inDto.RecordRRDto[i].A_Anz_Kinder_ueber_6_bis_10);
                S1Request.Body.RecordRR[i].I_A_Anz_Mitarbeiter = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Anz_Mitarbeiter>().Create(inDto.RecordRRDto[i].A_Anz_Mitarbeiter);
                S1Request.Body.RecordRR[i].I_A_Anz_unterstuetzungsp_Kinder_ab_12 = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Anz_unterstuetzungsp_Kinder_ab_12>().Create(inDto.RecordRRDto[i].A_Anz_unterstuetzungsp_Kinder_ab_12);
                S1Request.Body.RecordRR[i].I_A_Arbeitgeber_Beschaeftigt_bis = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Arbeitgeber_Beschaeftigt_bis>().Create(inDto.RecordRRDto[i].A_Arbeitgeber_beschaeftigt_bis);
                S1Request.Body.RecordRR[i].I_A_Arbeitgeber_Seit_wann = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Arbeitgeber_Seit_wann>().Create(inDto.RecordRRDto[i].A_Arbeitgeber_seit_wann);

                if (inDto.RecordRRDto[i].A_Auslaenderausweis != null)
                {
                    inDto.RecordRRDto[i].A_Auslaenderausweis = inDto.RecordRRDto[i].A_Auslaenderausweis.Trim();
                    if (inDto.RecordRRDto[i].A_Auslaenderausweis.Length == 0)
                    {
                        inDto.RecordRRDto[i].A_Auslaenderausweis = null;
                    }
                }
                S1Request.Body.RecordRR[i].I_A_Auslaenderausweis = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Auslaenderausweis>().Create(inDto.RecordRRDto[i].A_Auslaenderausweis);      // v = enum

                S1Request.Body.RecordRR[i].I_A_Auslaenderausweis_Einreisedatum = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Auslaenderausweis_Einreisedatum>().Create(inDto.RecordRRDto[i].A_Auslaenderausweis_Einreisedatum);
                S1Request.Body.RecordRR[i].I_A_Auslaenderausweis_Gueltigkeitsdatum = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Auslaenderausweis_Gueltigkeitsdatum>().Create(inDto.RecordRRDto[i].A_Auslaenderausweis_Gueltigkeitsdatum);
                S1Request.Body.RecordRR[i].I_A_Auszahlungsart = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Auszahlungsart>().Create(inDto.RecordRRDto[i].A_Auszahlungsart); // v = enum
                S1Request.Body.RecordRR[i].I_A_Berufliche_Situation = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Berufliche_Situation>().Create(inDto.RecordRRDto[i].A_Berufliche_Situation); // v = enum
                S1Request.Body.RecordRR[i].I_A_Berufsauslagen = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Berufsauslagen>().Create(inDto.RecordRRDto[i].A_Berufsauslagen); // v = enum
                S1Request.Body.RecordRR[i].I_A_Berufsauslagen_Betrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Berufsauslagen_Betrag>().Create(inDto.RecordRRDto[i].A_Berufsauslagen_Betrag);
                S1Request.Body.RecordRR[i].I_A_Bestehende_Kreditrate = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Bestehende_Kreditrate>().Create(inDto.RecordRRDto[i].A_Bestehende_Kreditrate);
                S1Request.Body.RecordRR[i].I_A_Bestehende_Leasingrate = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Bestehende_Leasingrate>().Create(inDto.RecordRRDto[i].A_Bestehende_Leasingrate);
                S1Request.Body.RecordRR[i].I_A_Bilanzsumme = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Bilanzsumme>().Create(inDto.RecordRRDto[i].A_Bilanzsumme);
                S1Request.Body.RecordRR[i].I_A_CS_Einheit = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_CS_Einheit>().Create(inDto.RecordRRDto[i].A_CS_Einheit); // v = enum
                S1Request.Body.RecordRR[i].I_A_Datum_letzter_Jahresabschluss = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Datum_letzter_Jahresabschluss>().Create(inDto.RecordRRDto[i].A_Datum_letzter_Jahresabschluss);
                S1Request.Body.RecordRR[i].I_A_E_Mail = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_E_Mail>().Create(inDto.RecordRRDto[i].A_E_Mail);
                S1Request.Body.RecordRR[i].I_A_Eigenkapital = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Eigenkapital>().Create(inDto.RecordRRDto[i].A_Eigenkapital);
                S1Request.Body.RecordRR[i].I_A_Einkommen_Art = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Einkommen_Art>().Create(inDto.RecordRRDto[i].A_Einkommen_Art); // v = enum
                S1Request.Body.RecordRR[i].I_A_Fluessige_mtel = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Fluessige_mtel>().Create(inDto.RecordRRDto[i].A_fluessige_mtel);
                S1Request.Body.RecordRR[i].I_A_Geburtsdatum = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Geburtsdatum>().Create(inDto.RecordRRDto[i].A_Geburtsdatum);
                S1Request.Body.RecordRR[i].I_A_Haupteinkommen_Betrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Haupteinkommen_Betrag>().Create(inDto.RecordRRDto[i].A_Haupteinkommen_Betrag);
                S1Request.Body.RecordRR[i].I_A_hier_Wohnhaft_seit = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_hier_Wohnhaft_seit>().Create(inDto.RecordRRDto[i].A_hier_Wohnhaft_seit);
                S1Request.Body.RecordRR[i].I_A_Hoehe_der_Betreibungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Hoehe_der_Betreibungen>().Create(inDto.RecordRRDto[i].A_Hoehe_der_Betreibungen);
                S1Request.Body.RecordRR[i].I_A_In_Handelsregister_eingetragen = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_In_Handelsregister_eingetragen>().Create(inDto.RecordRRDto[i].A_In_Handelsregister_eingetragen);
                S1Request.Body.RecordRR[i].I_A_Instradierung = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Instradierung>().Create(inDto.RecordRRDto[i].A_Instradierung);
                S1Request.Body.RecordRR[i].I_A_Jaehrl_Gratifikation_Bonus = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Jaehrl_Gratifikation_Bonus>().Create(inDto.RecordRRDto[i].A_Jaehl_Gratifikation_Bonus);
                S1Request.Body.RecordRR[i].I_A_Jahresgewinn = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Jahresgewinn>().Create(inDto.RecordRRDto[i].A_Jahregewinn);
                S1Request.Body.RecordRR[i].I_A_Jahresumsatz = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Jahresumsatz>().Create(inDto.RecordRRDto[i].A_Jahresumsatz);
                S1Request.Body.RecordRR[i].I_A_Kanton = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Kanton>().Create(inDto.RecordRRDto[i].A_Kanton);
                S1Request.Body.RecordRR[i].I_A_Kundenart = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Kundenart>().Create(inDto.RecordRRDto[i].A_Kundenart); // v = enum
                S1Request.Body.RecordRR[i].I_A_KundenID = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_KundenID>().Create(inDto.RecordRRDto[i].A_KundenID);
                S1Request.Body.RecordRR[i].I_A_Kurzfristige_Verbindlichkeiten = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Kurzfristige_Verbindlichkeiten>().Create(inDto.RecordRRDto[i].A_Kurzfristige_Verbindlichkeiten);
                S1Request.Body.RecordRR[i].I_A_Land = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Land>().Create(inDto.RecordRRDto[i].A_Land);
                S1Request.Body.RecordRR[i].I_A_marbeiter_Credit_Suisse_Group = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_marbeiter_Credit_Suisse_Group>().Create(inDto.RecordRRDto[i].A_marbeiter_Credit_Suisse_Group);
                S1Request.Body.RecordRR[i].I_A_MO_Counter = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_MO_Counter>().Create(inDto.RecordRRDto[i].A_MO_Counter);
                S1Request.Body.RecordRR[i].I_A_Mobiltelefon = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Mobiltelefon>().Create(inDto.RecordRRDto[i].A_Mobiltelefon);
                S1Request.Body.RecordRR[i].I_A_Nationalitaet = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Nationalitaet>().Create(inDto.RecordRRDto[i].A_Nationalitaet);
                S1Request.Body.RecordRR[i].I_A_Nebeneinkommen_Betrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Nebeneinkommen_Betrag>().Create(inDto.RecordRRDto[i].A_Nebeneinkommen_Betrag);
                S1Request.Body.RecordRR[i].I_A_PLZ = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_PLZ>().Create(inDto.RecordRRDto[i].A_PLZ);
                S1Request.Body.RecordRR[i].I_A_Quellensteuerpflichtig = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Quellensteuerpflichtig>().Create(inDto.RecordRRDto[i].A_Quellensteuerpflichtig);
                S1Request.Body.RecordRR[i].I_A_Regelmaessige_Auslagen = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Regelmaessige_Auslagen>().Create(inDto.RecordRRDto[i].A_Regelmaessige_Auslagen); // v = enum
                S1Request.Body.RecordRR[i].I_A_Regelmaessige_Auslagen_Betrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Regelmaessige_Auslagen_Betrag>().Create(inDto.RecordRRDto[i].A_Regelmaessige_Auslagen_Betrag);
                S1Request.Body.RecordRR[i].I_A_Revisionsstelle_vorhanden = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Revisionsstelle_vorhanden>().Create(inDto.RecordRRDto[i].A_Revisionsstelle_vorhanden);
                S1Request.Body.RecordRR[i].I_A_Rueckzahlungsart = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Rueckzahlungsart>().Create(inDto.RecordRRDto[i].A_Rueckzahlungsart); // v = enum
                S1Request.Body.RecordRR[i].I_A_Sprache = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Sprache>().Create(inDto.RecordRRDto[i].A_Sprache); // v = enum
                S1Request.Body.RecordRR[i].I_A_Status = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Status>().Create(inDto.RecordRRDto[i].A_Status); // v = enum
                S1Request.Body.RecordRR[i].I_A_Telefon_1 = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Telefon_1>().Create(inDto.RecordRRDto[i].A_Telefon_1);
                S1Request.Body.RecordRR[i].I_A_Telefon_2 = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Telefon_2>().Create(inDto.RecordRRDto[i].A_Telefon_2);
                S1Request.Body.RecordRR[i].I_A_Telefon_geschaeftlich = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Telefon_geschaeftlich>().Create(inDto.RecordRRDto[i].A_Telefon_geschaeftlich);
                S1Request.Body.RecordRR[i].I_A_Telefon_privat = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Telefon_privat>().Create(inDto.RecordRRDto[i].A_Telefon_privat);
                S1Request.Body.RecordRR[i].I_A_Unterstuetzungsbeitraege = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Unterstuetzungsbeitraege>().Create(inDto.RecordRRDto[i].A_Unterstuetzungsbeitraege); // v = enum
                S1Request.Body.RecordRR[i].I_A_Unterstuetzungsbeitraege_Betrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Unterstuetzungsbeitraege_Betrag>().Create(inDto.RecordRRDto[i].A_Unterstuetzungsbeitraege_Betrag);
                S1Request.Body.RecordRR[i].I_A_Verlustscheine_Pfaendungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Verlustscheine_Pfaendungen>().Create(inDto.RecordRRDto[i].A_Verlustscheine_Pfaendungen);
                S1Request.Body.RecordRR[i].I_A_Wohnkosten_Miete = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Wohnkosten_Miete>().Create(inDto.RecordRRDto[i].A_Wohnkosten_Miete);
                S1Request.Body.RecordRR[i].I_A_Wohnverhaeltnis = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Wohnverhaeltnis>().Create(inDto.RecordRRDto[i].A_Wohnverhaeltnis); // v = enum
                S1Request.Body.RecordRR[i].I_A_Zivilstand = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Zivilstand>().Create(inDto.RecordRRDto[i].A_Zivilstand); // v = enum
                S1Request.Body.RecordRR[i].I_A_Zusatzeinkommen = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Zusatzeinkommen>().Create(inDto.RecordRRDto[i].A_Zusatzeinkommen); // v = enum
                S1Request.Body.RecordRR[i].I_A_Zusatzeinkommen_Betrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Zusatzeinkommen_Betrag>().Create(inDto.RecordRRDto[i].A_Zusatzeinkommen_Betrag);
                S1Request.Body.RecordRR[i].I_A_Ehepartner = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Ehepartner>().Create(inDto.RecordRRDto[i].A_EhePartnerFlag);
                S1Request.Body.RecordRR[i].I_A_Weitere_Verpflichtungen_Betrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Weitere_Verpflichtungen_Betrag>().Create(inDto.RecordRRDto[i].A_Weitere_Verpflichtungen_Betrag);
                S1Request.Body.RecordRR[i].I_A_Weitere_Verpflichtungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Weitere_Verpflichtungen>().Create(inDto.RecordRRDto[i].A_Weitere_Verpflichtungen);
                S1Request.Body.RecordRR[i].I_A_UID = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_UID>().Create(inDto.RecordRRDto[i].A_UID);

                //*BNRACHT-615
                S1Request.Body.RecordRR[i].I_A_AG_Name = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_AG_Name>().Create(inDto.RecordRRDto[i].A_AG_NAME);
                S1Request.Body.RecordRR[i].I_A_Nebeneinkommen_seit_wann = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Nebeneinkommen_seit_wann>().Create(inDto.RecordRRDto[i].A_Nebeneinkommen_seit_wann);
                
                //BNRNEUN
                S1Request.Body.RecordRR[i].I_A_Arbeitgeber_Beschaeftigt_bis_NEK = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Arbeitgeber_Beschaeftigt_bis_NEK>().Create(inDto.RecordRRDto[i].A_Arbeitgeber_Beschaeftigt_bis2);
                S1Request.Body.RecordRR[i].I_A_Berufliche_Situation_NEK = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Berufliche_Situation_NEK>().Create(inDto.RecordRRDto[i].A_Berufliche_Situation2);
                S1Request.Body.RecordRR[i].I_A_Geschlecht = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Geschlecht>().Create(inDto.RecordRRDto[i].A_Geschlecht);

                // I_DV
                S1Request.Body.RecordRR[i].I_DV_AG_Datum = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_Datum>().Create(inDto.RecordRRDto[i].DV_AG_Datum);
                S1Request.Body.RecordRR[i].I_DV_AG_Decision_Maker = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_Decision_Maker>().Create(inDto.RecordRRDto[i].DV_AG_Decision_Maker);

                S1Request.Body.RecordRR[i].I_DV_Datum_juengster_Eintrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Datum_juengster_Eintrag>().Create(inDto.RecordRRDto[i].DV_Datum_juengster_Eintrag);
                S1Request.Body.RecordRR[i].I_DV_kritischer_Glaeubiger = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_kritischer_Glaeubiger>().Create(inDto.RecordRRDto[i].DV_Kritischer_Glaeubiger);
                S1Request.Body.RecordRR[i].I_DV_Summe_offener_Betreigungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Summe_offener_Betreigungen>().Create(inDto.RecordRRDto[i].DV_Summe_offener_Betreibungen);
                S1Request.Body.RecordRR[i].I_DV_Anzahl_offene_Betreibungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anzahl_offene_Betreibungen>().Create(inDto.RecordRRDto[i].DV_Anzahl_offene_Betreibungen);
                S1Request.Body.RecordRR[i].I_DV_Datum_juengste_Betreibung = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Datum_juengste_Betreibung>().Create(inDto.RecordRRDto[i].DV_Datum_juengste_Betreibung);
                S1Request.Body.RecordRR[i].I_DV_Organisation_belastet = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Organisation_belastet>().Create(inDto.RecordRRDto[i].DV_Organisation_belastet);
                S1Request.Body.RecordRR[i].I_DV_Score = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Score>().Create(inDto.RecordRRDto[i].DV_Score);
                S1Request.Body.RecordRR[i].I_DV_Payment_Delay = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Payment_Delay>().Create(inDto.RecordRRDto[i].DV_Payment_Delay);
                S1Request.Body.RecordRR[i].I_DV_first_SHAB_Date = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_first_SHAB_Date>().Create(inDto.RecordRRDto[i].DV_First_SHAB_Date);
                S1Request.Body.RecordRR[i].I_DV_Risk_Alert = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Risk_Alert>().Create(inDto.RecordRRDto[i].DV_Risk_Alert);

                //BNRAZ-449 CR 480
                S1Request.Body.RecordRR[i].I_DV_Netzwerkbeziehungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Netzwerkbeziehungen>().Create(inDto.RecordRRDto[i].Netzwerkbeziehung );
                S1Request.Body.RecordRR[i].I_DV_Fluktuationsrate = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Fluktuationsrate>().Create(inDto.RecordRRDto[i].Fluktuationsrate );
                S1Request.Body.RecordRR[i].I_DV_Fraud_Feld_Gesellschafter = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Fraud_Feld_Gesellschafter>().Create(inDto.RecordRRDto[i].Fraudmngt );




                S1Request.Body.RecordRR[i].I_DV_AG_Firmenstatus = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_Firmenstatus>().Create(inDto.RecordRRDto[i].DV_AG_Firmenstatus);
                S1Request.Body.RecordRR[i].I_DV_AG_Gruendungsdatum = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_Gruendungsdatum>().Create(inDto.RecordRRDto[i].DV_AG_Gruendungsdatum);
                S1Request.Body.RecordRR[i].I_DV_AG_NOGA_Code = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_NOGA_Code>().Create(inDto.RecordRRDto[i].DV_AG_NOGA_Code);
                S1Request.Body.RecordRR[i].I_DV_AG_Rechtsform = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_Rechtsform>().Create(inDto.RecordRRDto[i].DV_AG_Rechtsform);
                S1Request.Body.RecordRR[i].I_DV_AG_Status = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_Status>().Create(inDto.RecordRRDto[i].DV_AG_Status);
                //BNR14
                S1Request.Body.RecordRR[i].I_DV_AG_ADDRESS_ID = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_ADDRESS_ID>().Create(inDto.RecordRRDto[i].DV_AG_ADDRESS_ID);
                S1Request.Body.RecordRR[i].I_DV_AG_Zeit = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_Zeit>().Create(inDto.RecordRRDto[i].DV_AG_Zeit);

                S1Request.Body.RecordRR[i].I_DV_AG_UID = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_UID>().Create(inDto.RecordRRDto[i].DV_AG_UID);

                //BNRNEUN
                S1Request.Body.RecordRR[i].I_A_AG_PLZ = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_AG_PLZ>().Create(inDto.RecordRRDto[i].A_AG_PLZ1);
                S1Request.Body.RecordRR[i].I_A_AG_PLZ_NEK = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_AG_PLZ_NEK>().Create(inDto.RecordRRDto[i].A_AG_PLZ2);

                //BR10
                S1Request.Body.RecordRR[i].I_A_AG_Name_NEK = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_AG_Name_NEK >().Create(inDto.RecordRRDto[i].A_AG_NAME2);
                


                //BNR11 
                S1Request.Body.RecordRR[i].I_A_Zulage_Kind = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Zulage_Kind>().Create(inDto.RecordRRDto[i].A_ZULAGEKIND);
                S1Request.Body.RecordRR[i].I_A_Zulage_Ausbildung = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Zulage_Ausbildung>().Create(inDto.RecordRRDto[i].A_ZULAGEAUSBILDUNG);
                S1Request.Body.RecordRR[i].I_A_Zulage_Sonst = new DEBodyRecord<StrategyOneRequestBodyRecordI_A_Zulage_Sonst>().Create(inDto.RecordRRDto[i].A_ZULAGESONST);
              

                S1Request.Body.RecordRR[i].I_DV_AG_Kapital = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_AG_Kapital>().Create(inDto.RecordRRDto[i].DV_AG_Kapital);

                S1Request.Body.RecordRR[i].I_DV_Anz_BPM = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM>().Create(inDto.RecordRRDto[i].DV_Anz_BPM);

                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_l12M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_l12m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_l24M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_l24m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_l36M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_l36m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_l48M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_l48M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_l48m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_l60M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_l60M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_l60m);

                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_01 = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_01>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_01);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_0_2 = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_0_2>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_02);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_03 = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_03>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_03);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_04 = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_04>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_04);

                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_01_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_01_l12M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_01_l12m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_01_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_01_l24M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_01_l24m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_01_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_01_l36M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_01_l36m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_01_l48M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_01_l48M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_01_l48m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_01_l60M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_01_l60M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_01_l60m);

                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_0_2_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_0_2_l12M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_02_l12m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_0_2_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_0_2_l24M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_02_l24m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_0_2_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_0_2_l36M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_02_l36m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_0_2_l48M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_0_2_l48M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_02_l48m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_0_2_l60M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_0_2_l60M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_02_l60m);

                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_03_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_03_l12M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_03_l12m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_03_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_03_l24M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_03_l24m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_03_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_03_l36M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_03_l36m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_03_l48M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_03_l48M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_03_l48m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_03_l60M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_03_l60M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_03_l60m);
                
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_04_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_04_l12M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_04_l12m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_04_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_04_l24M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_04_l24m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_04_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_04_l36M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_04_l36m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_04_l48M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_04_l48M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_04_l48m);
                S1Request.Body.RecordRR[i].I_DV_Anz_BPM_m_FStat_04_l60M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BPM_m_FStat_04_l60M>().Create(inDto.RecordRRDto[i].DV_Anz_BPM_m_FStat_04_l60m);
                
                S1Request.Body.RecordRR[i].I_DV_Anz_DV_Treffer_Adressvalidierung = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_DV_Treffer_Adressvalidierung>().Create(inDto.RecordRRDto[i].DV_Anz_DV_Treffer_Adressvalidierung);
                S1Request.Body.RecordRR[i].I_DV_Datum_an_der_aktuellen_Adresse_seit = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Datum_an_der_aktuellen_Adresse_seit>().Create(inDto.RecordRRDto[i].DV_Datum_an_der_aktuellen_Adresse_seit);
                S1Request.Body.RecordRR[i].I_DV_Datum_der_Auskunft = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Datum_der_Auskunft>().Create(inDto.RecordRRDto[i].DV_Datum_der_Auskunft);
                S1Request.Body.RecordRR[i].I_DV_Datum_der_ersten_Meld = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Datum_der_ersten_Meld>().Create(inDto.RecordRRDto[i].DV_Datum_der_ersten_Meld);
                S1Request.Body.RecordRR[i].I_DV_Firmenstatus = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Firmenstatus>().Create(inDto.RecordRRDto[i].DV_Firmenstatus);
                S1Request.Body.RecordRR[i].I_DV_Fraud_Feld = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Fraud_Feld>().Create(inDto.RecordRRDto[i].DV_Fraud_Feld);
                S1Request.Body.RecordRR[i].I_DV_Geburtsdatum = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Geburtsdatum>().Create(inDto.RecordRRDto[i].DV_Geburtsdatum);
                S1Request.Body.RecordRR[i].I_DV_Gruendungsdatum = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Gruendungsdatum>().Create(inDto.RecordRRDto[i].DV_Gruendungsdatum);
                S1Request.Body.RecordRR[i].I_DV_Kapital = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Kapital>().Create(inDto.RecordRRDto[i].DV_Kapital);
                S1Request.Body.RecordRR[i].I_DV_ADDRESS_ID = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_ADDRESS_ID>().Create(inDto.RecordRRDto[i].DV_ADDRESS_ID);

                S1Request.Body.RecordRR[i].I_DV_Land = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Land>().Create(inDto.RecordRRDto[i].DV_Land);
                S1Request.Body.RecordRR[i].I_DV_NOGA_Code_Branche = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_NOGA_Code_Branche>().Create(inDto.RecordRRDto[i].DV_NOGA_Code_Branche);
                S1Request.Body.RecordRR[i].I_DV_PLZ = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_PLZ>().Create(inDto.RecordRRDto[i].DV_PLZ);
                S1Request.Body.RecordRR[i].I_DV_Rechtsform = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Rechtsform>().Create(inDto.RecordRRDto[i].DV_Rechtform);

                S1Request.Body.RecordRR[i].I_DV_Schlechtester_Fstat = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Schlechtester_Fstat>().Create(inDto.RecordRRDto[i].DV_Schlechtester_FStat);
                S1Request.Body.RecordRR[i].I_DV_Schlechtester_FStat_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Schlechtester_FStat_l12M>().Create(inDto.RecordRRDto[i].DV_Schlechtester_FStat_l12m);
                S1Request.Body.RecordRR[i].I_DV_Schlechtester_FStat_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Schlechtester_FStat_l24M>().Create(inDto.RecordRRDto[i].DV_Schlechtester_FStat_l24m);
                S1Request.Body.RecordRR[i].I_DV_Schlechtester_FStat_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Schlechtester_FStat_l36M>().Create(inDto.RecordRRDto[i].DV_Schlechtester_FStat_l36m);
                S1Request.Body.RecordRR[i].I_DV_Schlechtester_FStat_l48M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Schlechtester_FStat_l48M>().Create(inDto.RecordRRDto[i].DV_Schlechtester_FStat_l48m);
                S1Request.Body.RecordRR[i].I_DV_Schlechtester_FStat_l60M = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Schlechtester_FStat_l60M>().Create(inDto.RecordRRDto[i].DV_Schlechtester_FStat_l60m);
                
                S1Request.Body.RecordRR[i].I_DV_Status_Auskunft_Adressvalidierung = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Status_Auskunft_Adressvalidierung>().Create(inDto.RecordRRDto[i].DV_Status_Auskunft_Adressvalidierung);
                S1Request.Body.RecordRR[i].I_DV_Zeit_der_Auskunft = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Zeit_der_Auskunft>().Create(inDto.RecordRRDto[i].DV_Zeit_der_Auskunft);

                S1Request.Body.RecordRR[i].I_DV_UID = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_UID>().Create(inDto.RecordRRDto[i].DV_UID);
                S1Request.Body.RecordRR[i].I_DV_Anzahl_Decision_Maker = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anzahl_Decision_Maker>().Create(inDto.RecordRRDto[i].DV_Anz_DecisionMaker);
                S1Request.Body.RecordRR[i].I_DV_Datum_juengster_Eintrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Datum_juengster_Eintrag>().Create(inDto.RecordRRDto[i].DV_Datum_juengster_Eintrag);
                S1Request.Body.RecordRR[i].I_DV_kritischer_Glaeubiger = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_kritischer_Glaeubiger>().Create(inDto.RecordRRDto[i].DV_Kritischer_Glaeubiger);
                S1Request.Body.RecordRR[i].I_DV_Summe_offener_Betreigungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Summe_offener_Betreigungen>().Create(inDto.RecordRRDto[i].DV_Summe_offener_Betreibungen);
                S1Request.Body.RecordRR[i].I_DV_Anzahl_offene_Betreibungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anzahl_offene_Betreibungen>().Create(inDto.RecordRRDto[i].DV_Anzahl_offene_Betreibungen);
                S1Request.Body.RecordRR[i].I_DV_Datum_juengste_Betreibung = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Datum_juengste_Betreibung>().Create(inDto.RecordRRDto[i].DV_Datum_juengste_Betreibung);
                S1Request.Body.RecordRR[i].I_DV_Organisation_belastet = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Organisation_belastet>().Create(inDto.RecordRRDto[i].DV_Organisation_belastet);
                S1Request.Body.RecordRR[i].I_DV_Score = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Score>().Create(inDto.RecordRRDto[i].DV_Score);
                S1Request.Body.RecordRR[i].I_DV_Payment_Delay = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Payment_Delay>().Create(inDto.RecordRRDto[i].DV_Payment_Delay);
                S1Request.Body.RecordRR[i].I_DV_first_SHAB_Date = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_first_SHAB_Date>().Create(inDto.RecordRRDto[i].DV_First_SHAB_Date);
                S1Request.Body.RecordRR[i].I_DV_Risk_Alert = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Risk_Alert>().Create(inDto.RecordRRDto[i].DV_Risk_Alert);





                S1Request.Body.RecordRR[i].I_DV_UIDSameAsName = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_UIDSameAsName>().Create(inDto.RecordRRDto[i].DV_Uid_SameAsName);
                //*BNRACHT-615
                S1Request.Body.RecordRR[i].I_DV_Anz_BM_m_FStat_00 = new DEBodyRecord<StrategyOneRequestBodyRecordI_DV_Anz_BM_m_FStat_00>().Create(inDto.RecordRRDto[i].DV_Anz_BM_m_FStat_00);
                // I_ZEK
                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_Eng = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_Eng>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_Eng_Bardarlehen = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_Eng_Bardarlehen>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_Bardarlehen);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_Eng_Festkredit = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_Eng_Festkredit>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_Festkredit);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_Eng_Kartenengagement = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_Eng_Kartenengagement>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_Kartenengagement);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_Eng_Kontokorrent = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_Eng_Kontokorrent>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_Kontokorrent);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_Eng_Leasing = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_Eng_Leasing>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_Leasing);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_Eng_TeilzVertrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_Eng_TeilzVertrag>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_Teilz);

                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_FremdGes = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_FremdGes>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_FremdGes);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_lfd_ZEK_EigenGes = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_lfd_ZEK_EigenGes>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_EigenGes);
                
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_AmtsMelden_01_05 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_AmtsMelden_01_05>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_AmtsMelden_01_05);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_AmtsMelden_01_05_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_AmtsMelden_01_05_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_AmtsMelden_01_05_l12M);

                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_03 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_03>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_03);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_03_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_03_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_03l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_03_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_03_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_03l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_03_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_03_l36M>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_03l36M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_04 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_04>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_04);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_04_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_04_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_04l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_04_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_04_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_04l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_04_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_04_l36M>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_04l36M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_05_06 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_05_06>().Create(inDto.RecordRRDto[i].ZEK_Anz_lfd_ZEK_Eng_BCode_0506);

                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M);

                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Gesuche = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Gesuche>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Gesuche);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_05_06_08_12 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_05_06_08_12>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_05060812);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Ges_m_AblCode_04_07_09 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Ges_m_AblCode_04_07_09>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Ges_m_AblCode_040709);

                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26);

                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Synonyme = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Synonyme>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Synonyme);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Vertraege = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Vertraege>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Vertraege);
                S1Request.Body.RecordRR[i].I_ZEK_Datum_der_Auskunft = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Datum_der_Auskunft>().Create(inDto.RecordRRDto[i].ZEK_Datum_der_Auskunft);
                S1Request.Body.RecordRR[i].I_ZEK_Kunde_Gesamtengagement = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Kunde_Gesamtengagement>().Create(inDto.RecordRRDto[i].ZEK_Kunde_Gesamtengagement);
                S1Request.Body.RecordRR[i].I_ZEK_schlechtester_ZEK_AblCode = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_schlechtester_ZEK_AblCode>().Create(inDto.RecordRRDto[i].ZEK_schlechtester_ZEK_AblCode);
                S1Request.Body.RecordRR[i].I_ZEK_schlechtester_ZEK_BCode = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_schlechtester_ZEK_BCode>().Create(inDto.RecordRRDto[i].ZEK_schlechtester_ZEK_Code);
                S1Request.Body.RecordRR[i].I_ZEK_Status = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Status>().Create(inDto.RecordRRDto[i].ZEK_Status);

                //*BNRACHT-615
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Eng_m_BCode_04_laufend = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Eng_m_BCode_04_laufend>().Create(inDto.RecordRRDto[i].ZEK_Anz_Eng_m_BCode_04_laufend);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Eng_m_BCode_04_saldiert = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Eng_m_BCode_04_saldiert>().Create(inDto.RecordRRDto[i].ZEK_Anz_Eng_m_BCode_04_saldiert);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Eng_m_BCode_03_laufend = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Eng_m_BCode_03_laufend>().Create(inDto.RecordRRDto[i].ZEK_Anz_Eng_m_BCode_03_laufend);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Eng_m_BCode_03_saldiert = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Eng_m_BCode_03_saldiert>().Create(inDto.RecordRRDto[i].ZEK_Anz_Eng_m_BCode_03_saldiert);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Ges_m_AblCode_04_07_09_99_BN = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Ges_m_AblCode_04_07_09_99_BN>().Create(inDto.RecordRRDto[i].ZEK_Anz_Ges_m_AblCode_04_07_09_99_BN);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Ges_m_AblCode_04_07_09_99_noBN = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Ges_m_AblCode_04_07_09_99_noBN>().Create(inDto.RecordRRDto[i].ZEK_Anz_Ges_m_AblCode_04_07_09_99_noBN);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Ges_m_AblCode_13_14_BN = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Ges_m_AblCode_13_14_BN>().Create(inDto.RecordRRDto[i].ZEK_Anz_Ges_m_AblCode_13_14_BN);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Ges_m_AblCode_13_14_noBN = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Ges_m_AblCode_13_14_noBN>().Create(inDto.RecordRRDto[i].ZEK_Anz_Ges_m_AblCode_13_14_noBN);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Ges_m_AblCode_10_BN = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Ges_m_AblCode_10_BN>().Create(inDto.RecordRRDto[i].ZEK_Anz_Ges_m_AblCode_10_BN);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Ges_m_AblCode_10_noBN = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Ges_m_AblCode_10_noBN>().Create(inDto.RecordRRDto[i].ZEK_Anz_Ges_m_AblCode_10_noBN);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_KMeld_m_ErCode_21_mit_Positiv = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_KMeld_m_ErCode_21_mit_Positiv>().Create(inDto.RecordRRDto[i].ZEK_Anz_KMeld_m_ErCode_21_mit_Positiv);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_KMeld_m_ErCode_21_ohne_Positiv = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_KMeld_m_ErCode_21_ohne_Positiv>().Create(inDto.RecordRRDto[i].ZEK_Anz_KMeld_m_ErCode_21_ohne_Positiv);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_KMeld_m_ErCode_22_mit_Positiv = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_KMeld_m_ErCode_22_mit_Positiv>().Create(inDto.RecordRRDto[i].ZEK_Anz_KMeld_m_ErCode_22_mit_Positiv);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_KMeld_m_ErCode_22_ohne_Positiv = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_KMeld_m_ErCode_22_ohne_Positiv>().Create(inDto.RecordRRDto[i].ZEK_Anz_KMeld_m_ErCode_22_ohne_Positiv);
                S1Request.Body.RecordRR[i].I_ZEK_Negativeintraege = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Negativeintraege>().Create(inDto.RecordRRDto[i].ZEK_Negativeintraege);
                S1Request.Body.RecordRR[i].I_ZEK_Positiveintraege = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Positiveintraege>().Create(inDto.RecordRRDto[i].ZEK_Positiveintraege);
                //BNR16 
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_61 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_61>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Eng_m_BCode_61);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_61_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_61_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Eng_m_BCode_61_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_61_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_61_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Eng_m_BCode_61_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_61_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_61_l36M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Eng_m_BCode_61_l36M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Eng_m_BCode_61_laufend = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Eng_m_BCode_61_laufend>().Create(inDto.RecordRRDto[i].ZEK_Anz_Eng_m_BCode_61_laufend);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Eng_m_BCode_61_saldiert = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Eng_m_BCode_61_saldiert>().Create(inDto.RecordRRDto[i].ZEK_Anz_Eng_m_BCode_61_saldiert);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_71 = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_71>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Eng_m_BCode_71);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_71_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_71_l12M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Eng_m_BCode_71_l12M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_71_l24M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_71_l24M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Eng_m_BCode_71_l24M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_ZEK_Eng_m_BCode_71_l36M = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_ZEK_Eng_m_BCode_71_l36M>().Create(inDto.RecordRRDto[i].ZEK_Anz_ZEK_Eng_m_BCode_71_l36M);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Eng_m_BCode_71_laufend = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Eng_m_BCode_71_laufend>().Create(inDto.RecordRRDto[i].ZEK_Anz_Eng_m_BCode_71_laufend);
                S1Request.Body.RecordRR[i].I_ZEK_Anz_Eng_m_BCode_71_saldiert = new DEBodyRecord<StrategyOneRequestBodyRecordI_ZEK_Anz_Eng_m_BCode_71_saldiert>().Create(inDto.RecordRRDto[i].ZEK_Anz_Eng_m_BCode_71_saldiert);


                // I_OL
                S1Request.Body.RecordRR[i].I_OL_Anz_Annulierungen_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Annulierungen_l12M>().Create(inDto.RecordRRDto[i].OL_Anz_Annulierungen_l12M);
                S1Request.Body.RecordRR[i].I_OL_Anz_Antraege = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Antraege>().Create(inDto.RecordRRDto[i].OL_Anz_Antraege);
                S1Request.Body.RecordRR[i].I_OL_Anz_KundenIDs = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_KundenIDs>().Create(inDto.RecordRRDto[i].OL_Anz_KundenIDs);
                S1Request.Body.RecordRR[i].I_OL_Anz_lfd_Vertraege = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_lfd_Vertraege>().Create(inDto.RecordRRDto[i].OL_Anz_lfd_Vertraege);
                S1Request.Body.RecordRR[i].I_OL_Anz_Mahnungen_1 = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Mahnungen_1>().Create(inDto.RecordRRDto[i].OL_Anz_Mahnungen_1);
                S1Request.Body.RecordRR[i].I_OL_Anz_Mahnungen_2 = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Mahnungen_2>().Create(inDto.RecordRRDto[i].OL_Anz_Mahnungen_2);
                S1Request.Body.RecordRR[i].I_OL_Anz_Mahnungen_3 = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Mahnungen_3>().Create(inDto.RecordRRDto[i].OL_Anz_Mahnungen_3);
                S1Request.Body.RecordRR[i].I_OL_Anz_Mehrfachantraege = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Mehrfachantraege>().Create(inDto.RecordRRDto[i].OL_Anz_Mehrfachantraege);
                S1Request.Body.RecordRR[i].I_OL_Anz_OP = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_OP>().Create(inDto.RecordRRDto[i].OL_Anz_OP);
                S1Request.Body.RecordRR[i].I_OL_Anz_Stundungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Stundungen>().Create(inDto.RecordRRDto[i].OL_Anz_Stundungen);
                S1Request.Body.RecordRR[i].I_OL_Anz_Vertraege = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Vertraege>().Create(inDto.RecordRRDto[i].OL_Anz_Vertraege);
                S1Request.Body.RecordRR[i].I_OL_Anz_Vertraege_im_Recovery = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Vertraege_im_Recovery>().Create(inDto.RecordRRDto[i].OL_Anz_Vertraege_im_Recovery);
                S1Request.Body.RecordRR[i].I_OL_Anz_Verzichte_l12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Verzichte_l12M>().Create(inDto.RecordRRDto[i].OL_Anz_Verzichte_l12M);
                S1Request.Body.RecordRR[i].I_OL_Anz_Zahlungsvereinbarungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Zahlungsvereinbarungen>().Create(inDto.RecordRRDto[i].OL_Anz_Zahlungsvereinbarungen);
                S1Request.Body.RecordRR[i].I_OL_Dauer_Kundenbeziehung = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Dauer_Kundenbeziehung>().Create(inDto.RecordRRDto[i].OL_Dauer_Kundenbeziehung);
                S1Request.Body.RecordRR[i].I_OL_Effektive_Kundenbeziehung = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Effektive_Kundenbeziehung>().Create(inDto.RecordRRDto[i].OL_Effektive_Kundenbeziehung);
                S1Request.Body.RecordRR[i].I_OL_Engagement = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Engagement>().Create(inDto.RecordRRDto[i].OL_Engagement);
                S1Request.Body.RecordRR[i].I_OL_Eventualengagement = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Eventualengagement>().Create(inDto.RecordRRDto[i].OL_Eventualengagement);
                S1Request.Body.RecordRR[i].I_OL_Gesamtengagement = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Gesamtengagement>().Create(inDto.RecordRRDto[i].OL_Gesamtengagement);
                S1Request.Body.RecordRR[i].I_OL_Haushaltengagement = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Haushaltengagement>().Create(inDto.RecordRRDto[i].OL_Haushaltsengagement);
                S1Request.Body.RecordRR[i].I_OL_Letzte_Miete = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letzte_Miete>().Create(inDto.RecordRRDto[i].OL_Letzte_Miete);
                S1Request.Body.RecordRR[i].I_OL_Letzte_Nationalitaet = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letzte_Nationalitaet>().Create(inDto.RecordRRDto[i].OL_Letzte_Nationalitaet);
                S1Request.Body.RecordRR[i].I_OL_Letzter_Bonus = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letzter_Bonus>().Create(inDto.RecordRRDto[i].OL_Letzter_Bonus);
                S1Request.Body.RecordRR[i].I_OL_Letzter_Zivilstand = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letzter_Zivilstand>().Create(inDto.RecordRRDto[i].OL_Letzter_Zivilstand); // v = enum
                S1Request.Body.RecordRR[i].I_OL_Letztes_Arbeitsverhaeltnis = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letztes_Arbeitsverhaeltnis>().Create(inDto.RecordRRDto[i].OL_Letztes_Arbeitsverhaeltnis); // v = enum
                S1Request.Body.RecordRR[i].I_OL_Letztes_Haupteinkommen = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letztes_Haupteinkommen>().Create(inDto.RecordRRDto[i].OL_Letztes_Haupteinkommen);
                S1Request.Body.RecordRR[i].I_OL_Letztes_Nebeneinkommen = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letztes_Nebeneinkommen>().Create(inDto.RecordRRDto[i].OL_Letztes_Nebeneinkommen);
                S1Request.Body.RecordRR[i].I_OL_Letztes_Wohnverhaeltnis = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letztes_Wohnverhaeltnis>().Create(inDto.RecordRRDto[i].OL_Letztes_Wohnverhaeltnis); // v = enum
                S1Request.Body.RecordRR[i].I_OL_Letztes_Zusatzeinkommen = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Letztes_Zusatzeinkommen>().Create(inDto.RecordRRDto[i].OL_Letztes_Zusatzeinkommen);
                S1Request.Body.RecordRR[i].I_OL_Maximale_akt_RKlasse_des_Kunden = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Maximale_akt_RKlasse_des_Kunden>().Create(inDto.RecordRRDto[i].OL_Maximale_akt_RKlasse_des_Kunden);
                S1Request.Body.RecordRR[i].I_OL_Maximale_Mahnstufe = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Maximale_Mahnstufe>().Create(inDto.RecordRRDto[i].OL_Maximale_Mahnstufe);
                S1Request.Body.RecordRR[i].I_OL_Maximale_Risikoklasse_des_Kunden = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Maximale_Risikoklasse_des_Kunden>().Create(inDto.RecordRRDto[i].OL_Maximale_Risikoklasse_des_Kunden);
                S1Request.Body.RecordRR[i].I_OL_Maximaler_Badlisteintrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Maximaler_Badlisteintrag>().Create(inDto.RecordRRDto[i].OL_Maximaler_Bandlisteintrag);
                S1Request.Body.RecordRR[i].I_OL_Minimales_Datum_Kunde_seit = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Minimales_Datum_Kunde_seit>().Create(inDto.RecordRRDto[i].OL_Minimales_Datum_Kunde);
                S1Request.Body.RecordRR[i].I_OL_OpenLease_Datum_der_Auskunft = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_OpenLease_Datum_der_Auskunft>().Create(inDto.RecordRRDto[i].OL_OpenLease_Datum_der_Anmeldung);
                S1Request.Body.RecordRR[i].I_OL_Status = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Status>().Create(inDto.RecordRRDto[i].OL_Status);
                S1Request.Body.RecordRR[i].I_OL_Summe_OP = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Summe_OP>().Create(inDto.RecordRRDto[i].OL_Summe_OP);

                //BNRACHT-615
                S1Request.Body.RecordRR[i].I_OL_Anz_manAblehnungen_l6M  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_manAblehnungen_l6M>().Create(inDto.RecordRRDto[i].OL_Anz_manAblehnungen_l6M);
                S1Request.Body.RecordRR[i].I_OL_Anz_manAblehnungen_l12M  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_manAblehnungen_l12M>().Create(inDto.RecordRRDto[i].OL_Anz_manAblehnungen_l12M);
                S1Request.Body.RecordRR[i].I_OL_Anz_Vertraege_mit_Spezialfall  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Vertraege_mit_Spezialfall>().Create(inDto.RecordRRDto[i].OL_Anz_Vertraege_mit_Spezialfall);
                S1Request.Body.RecordRR[i].I_OL_Anz_lfd_Vertraege_mit_Spezialfall  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_lfd_Vertraege_mit_Spezialfall>().Create(inDto.RecordRRDto[i].OL_Anz_lfd_Vertraege_mit_Spezialfall);
                 
                S1Request.Body.RecordRR[i].I_OL_Datum_Mahnung_1  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Datum_Mahnung_1>().Create(inDto.RecordRRDto[i].OL_Datum_Mahnung_1);
                S1Request.Body.RecordRR[i].I_OL_Datum_Mahnung_2  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Datum_Mahnung_2>().Create(inDto.RecordRRDto[i].OL_Datum_Mahnung_2);
                S1Request.Body.RecordRR[i].I_OL_Datum_Mahnung_3  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Datum_Mahnung_3>().Create(inDto.RecordRRDto[i].OL_Datum_Mahnung_3);
                S1Request.Body.RecordRR[i].I_OL_Datum_letzte_Stundung = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Datum_letzte_Stundung>().Create(inDto.RecordRRDto[i].OL_Datum_letzte_Stundung);
                S1Request.Body.RecordRR[i].I_OL_Datum_letzte_ZVB  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Datum_letzte_ZVB>().Create(inDto.RecordRRDto[i].OL_Datum_letzte_ZVB);

                S1Request.Body.RecordRR[i].I_OL_Anzahl_Aufstockungssperren = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anzahl_Aufstockungssperren>().Create(inDto.RecordRRDto[i].OL_Anzahl_Aufstockungssperren);
                S1Request.Body.RecordRR[i].I_OL_Datum_Aufstockungssperre = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Datum_Aufstockungssperre>().Create(inDto.RecordRRDto[i].OL_Datum_Aufstockungssperre);

                //BNRNEUN
                S1Request.Body.RecordRR[i].I_OL_letzte_Einkommen_Art = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_letzte_Einkommen_Art>().Create(inDto.RecordRRDto[i].OL_Letzte_Einkommensart);
                // BNR10
                S1Request.Body.RecordRR[i].I_OL_Anz_Ratenpausen = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anz_Ratenpausen>().Create(inDto.RecordRRDto[i].OL_Ratenpausen);

                S1Request.Body.RecordRR[i].I_OL_GBeziehungen = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_GBeziehungen>().Create(inDto.RecordRRDto[i].OL_Gbezeichnung);

                //BNR13 RISK V91
                S1Request.Body.RecordRR[i].I_OL_Datum_erster_Antrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Datum_erster_Antrag>().Create(inDto.RecordRRDto[i].OL_Datum_erster_Antrag);
                S1Request.Body.RecordRR[i].I_OL_Datum_letzter_Antrag = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Datum_letzter_Antrag>().Create(inDto.RecordRRDto[i].OL_Datum_letzter_Antrag);
                S1Request.Body.RecordRR[i].I_OL_Anzahl_Mahnstufe1_L6M = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anzahl_Mahnstufe1_L6M>().Create(inDto.RecordRRDto[i].OL_Anzahl_Mahnstufe1_L6M);
                S1Request.Body.RecordRR[i].I_OL_Anzahl_Mahnstufe2_L6M  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anzahl_Mahnstufe2_L6M>().Create(inDto.RecordRRDto[i].OL_Anzahl_Mahnstufe2_L6M);
                S1Request.Body.RecordRR[i].I_OL_Anzahl_Mahnstufe3_L6M = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anzahl_Mahnstufe3_L6M>().Create(inDto.RecordRRDto[i].OL_Anzahl_Mahnstufe3_L6M);
                S1Request.Body.RecordRR[i].I_OL_Anzahl_Einzahlungen_L12M  = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Anzahl_Einzahlungen_L12M>().Create(inDto.RecordRRDto[i].OL_Anzahl_Einzahlungen_L12M);
                S1Request.Body.RecordRR[i].I_OL_Saldoreduktion_L12M = new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Saldoreduktion_L12M >().Create(inDto.RecordRRDto[i].OL_Saldoreduktion_L12M);
                S1Request.Body.RecordRR[i].I_OL_Zahlungsrueckstand= new DEBodyRecord<StrategyOneRequestBodyRecordI_OL_Zahlungsrueckstand>().Create(inDto.RecordRRDto[i].OL_Zahlungsrueckstand);

                S1Request.Body.RecordRR[i].I_BU_Datum = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_Datum>().Create(inDto.RecordRRDto[i].BU_AnfrageDatum);
                S1Request.Body.RecordRR[i].I_BU_GRUNDBETRAG = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_GRUNDBETRAG>().Create(inDto.RecordRRDto[i].BU_Grundbetrag);
                S1Request.Body.RecordRR[i].I_BU_QUELLENSTEUER = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_QUELLENSTEUER>().Create(inDto.RecordRRDto[i].BU_Quellsteuer);
                S1Request.Body.RecordRR[i].I_BU_SOZIALAUSLAGEN = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_SOZIALAUSLAGEN>().Create(inDto.RecordRRDto[i].BU_Sozialauslagen);
                
                S1Request.Body.RecordRR[i].I_BU_BUDGETUEBERSCHUSS = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_BUDGETUEBERSCHUSS>().Create(inDto.RecordRRDto[i].BU_Budgetueberschuss);
                S1Request.Body.RecordRR[i].I_BU_BUDGETUEBERSCHUSS_GESAMT = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_BUDGETUEBERSCHUSS_GESAMT>().Create(inDto.RecordRRDto[i].BU_Budgetueberschuss_gesamt);
                
                S1Request.Body.RecordRR[i].I_BU_KREMO_Fehlercode = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_KREMO_Fehlercode>().Create(inDto.RecordRRDto[i].BU_Kremocode);
                S1Request.Body.RecordRR[i].I_BU_KRANKENKASSE = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_KRANKENKASSE>().Create(inDto.RecordRRDto[i].BU_Krankenkasse);

                //BNR10
                S1Request.Body.RecordRR[i].I_BU_NETTOEINKOMMEN_BERECHNET_1 = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_NETTOEINKOMMEN_BERECHNET_1>().Create(inDto.RecordRRDto[i].BU_Einknettoberech);

                S1Request.Body.RecordRR[i].I_BU_NETTOEINKOMMEN_BERECHNET_2 = new DEBodyRecord<StrategyOneRequestBodyRecordI_BU_NETTOEINKOMMEN_BERECHNET_2>().Create(inDto.RecordRRDto[i].BU_Einknettoberech2);

                //BNR13
                S1Request.Body.RecordRR[i].I_BU_Krankenkassenpraemie = new  DEBodyRecord<StrategyOneRequestBodyRecordI_BU_Krankenkassenpraemie>().Create(inDto.RecordRRDto[i].BU_Krankenkassenpraemie);
                S1Request.Body.RecordRR[i].I_BU_Arbeitswegpauschale = new  DEBodyRecord<StrategyOneRequestBodyRecordI_BU_Arbeitswegpauschale>().Create(inDto.RecordRRDto[i].BU_Arbeitswegpauschale);
                S1Request.Body.RecordRR[i].I_BU_Betreuungskosten_Extern = new  DEBodyRecord<StrategyOneRequestBodyRecordI_BU_Betreuungskosten_Extern>().Create(inDto.RecordRRDto[i].BU_Betreuungskosten_Extern);
    


            }
        }

        private void MyFillOutDto(DecisionEngineOutDto outDto, StrategyOneResponse response)
        {
            // NR
            if (response.Body.RecordNR != null)
            {
                // Envelope
                outDto.InquiryDate = (DateTime?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_INQUIRY_DATE>().GetVParameter(response.Body.RecordNR.O_INQUIRY_DATE);
                outDto.InquiryTime = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_INQUIRY_TIME>().GetVParameter(response.Body.RecordNR.O_INQUIRY_TIME));
                outDto.System_Decision = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_SYSTEM_DECISION>().GetVParameter(response.Body.RecordNR.O_SYSTEM_DECISION));
                outDto.System_Decision_Group = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_SYSTEM_DECISION_GROUP>().GetVParameter(response.Body.RecordNR.O_SYSTEM_DECISION_GROUP));

                // Decision
                outDto.DEC_StatusID = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Status_ID>().GetVParameter(response.Body.RecordNR.O_DEC_Status_ID);
                outDto.DEC_Ampel = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Ampel>().GetVParameter(response.Body.RecordNR.O_DEC_Ampel));
                outDto.DEC_ProzessschrittID = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Prozessschritt_ID>().GetVParameter(response.Body.RecordNR.O_DEC_Prozessschritt_ID);
                outDto.DEC_Regelwerk_Code = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Regelwerk_Code>().GetVParameter(response.Body.RecordNR.O_DEC_Regelwerk_Code));
                outDto.DEC_Status = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Status>().GetVParameter(response.Body.RecordNR.O_DEC_Status));
                outDto.DEC_Kundengruppe = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Kundengruppe>().GetVParameter(response.Body.RecordNR.O_DEC_Kundengruppe));
                outDto.DEC_Freibetrag = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Freibetrag>().GetVParameter(response.Body.RecordNR.O_DEC_Freibetrag);
                outDto.DEC_Mindestmiete = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Mindestmiete>().GetVParameter(response.Body.RecordNR.O_DEC_Mindestmiete);
                outDto.DEC_FaktorBP = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_FaktorBP>().GetVParameter(response.Body.RecordNR.O_DEC_FaktorBP);
                outDto.DEC_FaktorZ = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_FaktorZ>().GetVParameter(response.Body.RecordNR.O_DEC_FaktorZ);
                // BRN10
                outDto.DEC_Fraud_Flag = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Fraud_Flag>().GetVParameter(response.Body.RecordNR.O_DEC_Fraud_Flag);
                outDto.DEC_Fraud_Score = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Fraud_Score>().GetVParameter(response.Body.RecordNR.O_DEC_Fraud_Score);

                // BRN12
                outDto.DEC_TR_Segment = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_TR_Segment>().GetVParameter(response.Body.RecordNR.O_DEC_TR_Segment));
                // BNR13
                outDto.DEC_Betreuungskosten = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_DEC_Betreuungskosten_Extern>().GetVParameter(response.Body.RecordNR.O_DEC_Betreuungskosten_Extern);
      
              
                // Sonstige Felder
                outDto.Fehlercode = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_FEHLERCODE>().GetVParameter(response.Body.RecordNR.O_FEHLERCODE);
                outDto.RandomNumber = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RANDOM_NUMBER>().GetVParameter(response.Body.RecordNR.O_RANDOM_NUMBER);
                outDto.ReserveDate1 = (DateTime?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_DATE_1>().GetVParameter(response.Body.RecordNR.O_RESERVE_DATE_1);
                outDto.ReserveDate2 = (DateTime?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_DATE_2>().GetVParameter(response.Body.RecordNR.O_RESERVE_DATE_2);
                outDto.ReserveDate3 = (DateTime?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_DATE_3>().GetVParameter(response.Body.RecordNR.O_RESERVE_DATE_3);
                outDto.ReserveNumber1 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_NUMBER_1>().GetVParameter(response.Body.RecordNR.O_RESERVE_NUMBER_1);
                outDto.ReserveNumber2 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_NUMBER_2>().GetVParameter(response.Body.RecordNR.O_RESERVE_NUMBER_2);
                outDto.ReserveNumber3 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_NUMBER_3>().GetVParameter(response.Body.RecordNR.O_RESERVE_NUMBER_3);
                outDto.ReserveText1 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_TEXT_1>().GetVParameter(response.Body.RecordNR.O_RESERVE_TEXT_1));
                outDto.ReserveText2 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_TEXT_2>().GetVParameter(response.Body.RecordNR.O_RESERVE_TEXT_2));
                outDto.ReserveText3 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordNRO_RESERVE_TEXT_3>().GetVParameter(response.Body.RecordNR.O_RESERVE_TEXT_3));
              }

            // RR
            if (response.Body.RecordRR != null)
            {
                outDto.RecordRRResponseDto = new RecordRRResponseDto[response.Body.RecordRR.Length];
                for (int i = 0; i < response.Body.RecordRR.Length; i++)
                {
                    outDto.RecordRRResponseDto[i] = new RecordRRResponseDto();
                    // Detailergebnis Decision
                    outDto.RecordRRResponseDto[i].DET_Antragsteller = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_DET_Antragsteller>().GetVParameter(response.Body.RecordRR[i].O_DET_Antragsteller);
                    outDto.RecordRRResponseDto[i].DEC_Scorekarte_Code = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Scorekarte_Code>().GetVParameter(response.Body.RecordRR[i].O_DEC_Scorekarte_Code));
                    outDto.RecordRRResponseDto[i].DEC_Scorekarte_Bezeichnung = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Scorekarte_Bezeichnung>().GetVParameter(response.Body.RecordRR[i].O_DEC_Scorekarte_Bezeichnung));
                    outDto.RecordRRResponseDto[i].DEC_Scorewert_Total = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Scorewert_Total>().GetVParameter(response.Body.RecordRR[i].O_DEC_Scorewert_Total);
                    outDto.RecordRRResponseDto[i].DEC_Risikoklasse_ID = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Risikoklasse_ID>().GetVParameter(response.Body.RecordRR[i].O_DEC_Risikoklasse_ID);
                    outDto.RecordRRResponseDto[i].DEC_Risikoklasse_Bezeichnung = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Risikoklasse_Bezeichnung>().GetVParameter(response.Body.RecordRR[i].O_DEC_Risikoklasse_Bezeichnung));
                    //BNR12
                    outDto.RecordRRResponseDto[i].DEC_Score_PD = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Score_PD>().GetVParameter(response.Body.RecordRR[i].O_DEC_Score_PD);
                   
                    
                    outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_Auflagen = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Liste_getroffene_Regeln_Auflagen>().GetVParameter(response.Body.RecordRR[i].O_DEC_Liste_getroffene_Regeln_Auflagen));
                    outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_BP = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Liste_getroffene_Regeln_BP>().GetVParameter(response.Body.RecordRR[i].O_DEC_Liste_getroffene_Regeln_BP));
                    outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_Formalit = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Liste_getroffene_Regeln_Formalit>().GetVParameter(response.Body.RecordRR[i].O_DEC_Liste_getroffene_Regeln_Formalit));
                    outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_RP = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Liste_getroffene_Regeln_RP>().GetVParameter(response.Body.RecordRR[i].O_DEC_Liste_getroffene_Regeln_RP));
                    outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_VP = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Liste_getroffene_Regeln_VP>().GetVParameter(response.Body.RecordRR[i].O_DEC_Liste_getroffene_Regeln_VP));
                    outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_FP = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_DEC_Liste_getroffene_Regeln_FP>().GetVParameter(response.Body.RecordRR[i].O_DEC_Liste_getroffene_Regeln_FP));


                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_1 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_1>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_1));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_1 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_1>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_1));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_1 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_1>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_1));
                    outDto.RecordRRResponseDto[i].SC_ID_1 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_1>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_1);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_1 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_1>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_1);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_2 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_2>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_2));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_2 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_2>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_2));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_2 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_2>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_2));
                    outDto.RecordRRResponseDto[i].SC_ID_2 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_2>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_2);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_2 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_2>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_2);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_3 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_3>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_3));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_3 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_3>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_3));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_3 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_3>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_3));
                    outDto.RecordRRResponseDto[i].SC_ID_3 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_3>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_3);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_3 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_3>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_3);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_4 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_4>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_4));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_4 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_4>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_4));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_4 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_4>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_4));
                    outDto.RecordRRResponseDto[i].SC_ID_4 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_4>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_4);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_4 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_4>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_4);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_5 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_5>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_5));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_5 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_5>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_5));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_5 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_5>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_5));
                    outDto.RecordRRResponseDto[i].SC_ID_5 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_5>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_5);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_5 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_5>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_5);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_6 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_6>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_6));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_6 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_6>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_6));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_6 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_6>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_6));
                    outDto.RecordRRResponseDto[i].SC_ID_6 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_6>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_6);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_6 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_6>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_6);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_7 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_7>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_7));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_7 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_7>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_7));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_7 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_7>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_7));
                    outDto.RecordRRResponseDto[i].SC_ID_7 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_7>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_7);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_7 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_7>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_7);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_8 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_8>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_8));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_8 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_8>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_8));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_8 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_8>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_8));
                    outDto.RecordRRResponseDto[i].SC_ID_8 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_8>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_8);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_8 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_8>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_8);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_9 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_9>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_9));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_9 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_9>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_9));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_9 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_9>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_9));
                    outDto.RecordRRResponseDto[i].SC_ID_9 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_9>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_9);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_9 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_9>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_9);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_10 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_10>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_10));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_10 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_10>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_10));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_10 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_10>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_10));
                    outDto.RecordRRResponseDto[i].SC_ID_10 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_10>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_10);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_10 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_10>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_10);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_11 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_11>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_11));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_11 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_11>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_11));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_11 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_11>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_11));
                    outDto.RecordRRResponseDto[i].SC_ID_11 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_11>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_11);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_11 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_11>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_11);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_12 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_12>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_12));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_12 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_12>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_12));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_12 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_12>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_12));
                    outDto.RecordRRResponseDto[i].SC_ID_12 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_12>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_12);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_12 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_12>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_12);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_13 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_13>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_13));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_13 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_13>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_13));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_13 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_13>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_13));
                    outDto.RecordRRResponseDto[i].SC_ID_13 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_13>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_13);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_13 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_13>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_13);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_14 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_14>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_14));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_14 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_14>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_14));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_14 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_14>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_14));
                    outDto.RecordRRResponseDto[i].SC_ID_14 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_14>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_14);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_14 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_14>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_14);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_15 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_15>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_15));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_15 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_15>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_15));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_15 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_15>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_15));
                    outDto.RecordRRResponseDto[i].SC_ID_15 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_15>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_15);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_15 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_15>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_15);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_16 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_16>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_16));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_16 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_16>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_16));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_16 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_16>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_16));
                    outDto.RecordRRResponseDto[i].SC_ID_16 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_16>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_16);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_16 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_16>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_16);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_17 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_17>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_17));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_17 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_17>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_17));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_17 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_17>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_17));
                    outDto.RecordRRResponseDto[i].SC_ID_17 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_17>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_17);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_17 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_17>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_17);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_18 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_18>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_18));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_18 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_18>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_18));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_18 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_18>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_18));
                    outDto.RecordRRResponseDto[i].SC_ID_18 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_18>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_18);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_18 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_18>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_18);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_19 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_19>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_19));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_19 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_19>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_19));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_19 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_19>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_19));
                    outDto.RecordRRResponseDto[i].SC_ID_19 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_19>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_19);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_19 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_19>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_19);

                    outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_20 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Berechnungsvariablen_20>().GetVParameter(response.Body.RecordRR[i].O_SC_Berechnungsvariablen_20));
                    outDto.RecordRRResponseDto[i].SC_Bezeichnung_20 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Bezeichnung_20>().GetVParameter(response.Body.RecordRR[i].O_SC_Bezeichnung_20));
                    outDto.RecordRRResponseDto[i].SC_Eingabewert_20 = ConvertToString(new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Eingabewert_20>().GetVParameter(response.Body.RecordRR[i].O_SC_Eingabewert_20));
                    outDto.RecordRRResponseDto[i].SC_ID_20 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_ID_20>().GetVParameter(response.Body.RecordRR[i].O_SC_ID_20);
                    outDto.RecordRRResponseDto[i].SC_Resultatwert_20 = (decimal?)new DEBodyRecord<StrategyOneResponseBodyRecordO_SC_Resultatwert_20>().GetVParameter(response.Body.RecordRR[i].O_SC_Resultatwert_20);
                }
            }
        }

        private string ConvertToString(object obToConv)
        {
            return obToConv != null ? obToConv.ToString() : null;
        }

        #endregion              // Private methods
    }
}