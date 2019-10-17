
namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// ZEK Web Service Data Access Object Interface
    /// </summary>
    public interface IZekWSDao
    {
        /// <summary>
        /// Calls ZEK Webservice KreditgesuchNeu (EC1)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="zielverein"></param>
        /// <param name="anfragegrund"></param>
        /// <param name="previousKreditgesuchID"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse kreditgesuchNeu(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, int zielverein, int anfragegrund, string previousKreditgesuchID);
        /// <summary>
        /// Calls ZEK Webservice Informativabfrage (EC2)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntity"></param>
        /// <param name="zielverein"></param>
        /// <param name="anfragegrund"></param>
        /// <returns></returns>
        ZEKRef.InfoResponse informativabfrage(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity requestEntity, int zielverein, int anfragegrund);
        /// <summary>
        /// Calls ZEK Webservice KreditgesuchAblehnen (EC3)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntity"></param>
        /// <param name="datumAblehnung"></param>
        /// <param name="ablehnungsgrund"></param>
        /// <param name="kreditGesuchID"></param>
        /// <returns></returns>
        ZEKRef.CreditClaimRejectionResponse kreditgesuchAblehnen(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity requestEntity,
            string datumAblehnung, int ablehnungsgrund, string kreditGesuchID);
        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerBardarlehen (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="bardarlehen"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse registerBardarlehen(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities,
            ZEKRef.BardarlehenDescription bardarlehen, string kreditgesuch, int zielverein);
        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerFestkredit (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="festkredit"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse registerFestkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities,
            ZEKRef.Festkredit festkredit, string kreditgesuch, int zielverein);
        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerKontokorrentkredit (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kontokorrentkredit"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse registerKontokorrentkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities,
            ZEKRef.KontokorrentkreditDescription kontokorrentkredit, string kreditgesuch, int zielverein);
        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerLeasingMietvertrag (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="leasingMietvertrag"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse registerLeasingMietvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities,
            ZEKRef.LeasingMietvertragDescription leasingMietvertrag, string kreditgesuch, int zielverein);
        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerTeilzahlungskredit (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="teilzahlungskredit"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse registerTeilzahlungskredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities,
            ZEKRef.TeilzahlungskreditDescription teilzahlungskredit, string kreditgesuch, int zielverein);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="teilzahlungsvertrag"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse registerTeilzahlungsvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities,
            ZEKRef.TeilzahlungsvertragDescription teilzahlungsvertrag, string kreditgesuch, int zielverein);
        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method meldungKartenengagement (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kartenengagement"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse meldungKartenengagement(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities,
            ZEKRef.KartenengagementDescription kartenengagement, string kreditgesuch, int zielverein);
        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method meldungUeberziehungskredit (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="ueberziehungskredit"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse meldungUeberziehungskredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities,
            ZEKRef.UeberziehungskreditDescription ueberziehungskredit, string kreditgesuch, int zielverein);
        /// <summary>
        /// Calls ZEK Webservice Mutation Grunddaten, method grunddatenMutieren (EC6)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntity"></param>
        /// <param name="requestEntityNew"></param>
        /// <returns></returns>
        ZEKRef.UpdateAddressResponse updateAddress(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity requestEntity, ZEKRef.RequestEntity requestEntityNew);
        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateBardarlehen (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="bardarlehen"></param>
        /// <param name="bardarlehenNew"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse updateBardarlehen(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.BardarlehenDescription bardarlehen,
            ZEKRef.BardarlehenDescription bardarlehenNew);
        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateFestkredit (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kredit"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse updateFestkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.Festkredit kredit, ZEKRef.Festkredit kreditNew);
        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateKontokorrentkredit (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kredit"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse updateKontokorrentkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.KontokorrentkreditDescription kredit,
            ZEKRef.KontokorrentkreditDescription kreditNew);
        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateLeasingMietvertragkredit (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kredit"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse updateLeasingMietvertragkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.LeasingMietvertragDescription kredit,
            ZEKRef.LeasingMietvertragDescription kreditNew);
        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateTeilzahlungskredit (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kredit"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse updateTeilzahlungskredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungskreditDescription kredit,
            ZEKRef.TeilzahlungskreditDescription kreditNew);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="vertrag"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse updateTeilzahlungsvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungsvertragDescription vertrag,
            ZEKRef.TeilzahlungsvertragDescription kreditNew);
        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeBardarlehen (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="bardarlehen"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse closeBardarlehen(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.BardarlehenDescription bardarlehen);

        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeLeasingMietvertrag (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="leasingMietvertrag"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse closeLeasingMietvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.LeasingMietvertragDescription leasingMietvertrag);

        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeFestkredit (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="circumstantialCreditDescription"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse closeFestkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.Festkredit circumstantialCreditDescription);

        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeTeilzahlungskredit (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="teilzahlungskredit"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse closeTeilzahlungskredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungskreditDescription teilzahlungskredit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="teilzahlungsvertrag"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse closeTeilzahlungsvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungsvertragDescription teilzahlungsvertrag);


        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeKontokorrentkredit (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kontokorrentkredit"></param>
        /// <returns></returns>
        ZEKRef.CommonMultiResponse closeKontokorrentkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.KontokorrentkreditDescription kontokorrentkredit);

        
       /// <summary>
       /// eCode178Anmelden
       /// </summary>
       /// <param name="idDesc"></param>
       /// <param name="requestEntity"></param>
       /// <param name="eCode178"></param>
       /// <param name="kreditGesuchID"></param>
       /// <param name="contractId"></param>
       /// <returns></returns>
       ZEKRef.CommonMultiResponse eCode178Anmelden(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity requestEntity, ZEKRef.eCode178 eCode178, string kreditGesuchID, string contractId);

       /// <summary>
       /// eCode178Mutieren
       /// </summary>
       /// <param name="idDesc"></param>
       /// <param name="eCode178"></param>
       /// <returns></returns>
       ZEKRef.CommonMultiResponse eCode178Mutieren(ZEKRef.IdentityDescriptor idDesc, ZEKRef.eCode178 eCode178);


       /// <summary>
       /// eCode178Abmelden
       /// </summary>
       /// <param name="idDesc"></param>
       /// <param name="eCode178"></param>
       /// <returns></returns>
       ZEKRef.CommonMultiResponse eCode178Abmelden(ZEKRef.IdentityDescriptor idDesc, ZEKRef.eCode178 eCode178);

       
       /// <summary>
       /// eCode178Abfrage
       /// </summary>
       /// <param name="idDesc"></param>
       /// <param name="eCode178"></param>
       /// <returns></returns>
       ZEKRef.CommonMultiResponse eCode178Abfrage(ZEKRef.IdentityDescriptor idDesc, ZEKRef.eCode178 eCode178);
        

        /// <summary>
       /// getARMs
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="dateLastSuccessfullRequest"></param>
        /// <returns></returns>
        ZEKRef.ArmResponse getARMs(ZEKRef.IdentityDescriptor idDesc, string dateLastSuccessfullRequest);

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto();

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto">soapXMLDto</param>
        void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);

        
    }
}