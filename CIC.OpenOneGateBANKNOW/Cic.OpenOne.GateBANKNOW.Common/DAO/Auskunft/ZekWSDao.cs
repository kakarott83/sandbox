using System;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    
    /// <summary>
    /// ZEK Web service Data Access Object
    /// </summary>
    public class ZekWSDao : IZekWSDao
    {
        

        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();

        /// <summary>
        /// Calls ZEK Webservice KreditgesuchNeu (EC1)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="zielverein"></param>
        /// <param name="anfragegrund"></param>
        /// <param name="previousKreditgesuchID"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse kreditgesuchNeu(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, int zielverein, int anfragegrund, string previousKreditgesuchID)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-KreditgesuchNeu",soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.kreditgesuchNeu(idDesc, requestEntities, zielverein,anfragegrund, previousKreditgesuchID);
                });
                
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK KreditgesuchNeu Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Informativabfrage (EC2)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntity"></param>
        /// <param name="zielverein"></param>
        /// <param name="anfragegrund"></param>
        /// <returns></returns>11
        public ZEKRef.InfoResponse informativabfrage(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity requestEntity, int zielverein, int anfragegrund)
        {
            try
            {


                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.InfoResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.InfoResponse>();
                return t.call("ZEK-Informativabfrage", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    
                    return client.informativabfrage(idDesc, requestEntity, zielverein, anfragegrund);
                });
                
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-Informativabfrage Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice KreditgesuchAblehnen (EC3)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntity"></param>
        /// <param name="datumAblehnung"></param>
        /// <param name="ablehnungsgrund"></param>
        /// <param name="kreditGesuchID"></param>
        /// <returns></returns>
        public ZEKRef.CreditClaimRejectionResponse kreditgesuchAblehnen(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity requestEntity, string datumAblehnung, int ablehnungsgrund, string kreditGesuchID)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CreditClaimRejectionResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CreditClaimRejectionResponse>();
                return t.call("ZEK-KreditgesuchAblehnen", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.kreditgesuchAblehnen(idDesc, requestEntity, datumAblehnung, ablehnungsgrund, kreditGesuchID);
                });
               
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-KreditgesuchAblehnen Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerBardarlehen (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="bardarlehen"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse registerBardarlehen(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.BardarlehenDescription bardarlehen, string kreditgesuch, int zielverein)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-RegisterBardarlehen", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.registerBardarlehen(idDesc, requestEntities, bardarlehen, kreditgesuch, zielverein);
                });
               
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-RegisterBardarlehen Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerFestkredit (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="festkredit"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse registerFestkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.Festkredit festkredit, string kreditgesuch, int zielverein)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-RegisterFestkredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.registerFestkredit(idDesc, requestEntities, festkredit, kreditgesuch, zielverein);
                });
               
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-RegisterFestkredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerKontokorrentkredit (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kontokorrentkredit"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse registerKontokorrentkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.KontokorrentkreditDescription kontokorrentkredit, string kreditgesuch, int zielverein)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-RegisterKontokorrentkredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.registerKontokorrentkredit(idDesc, requestEntities, kontokorrentkredit, kreditgesuch, zielverein);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-RegisterKontokorrentkredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerLeasingMietvertrag (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="leasingMietvertrag"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse registerLeasingMietvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.LeasingMietvertragDescription leasingMietvertrag, string kreditgesuch, int zielverein)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-RegisterLeasingMietvertrag", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.registerLeasingMietvertrag(idDesc, requestEntities, leasingMietvertrag, kreditgesuch, zielverein);
                });


                
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-RegisterLeasingMietvertrag Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method registerTeilzahlungskredit (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="teilzahlungskredit"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse registerTeilzahlungskredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungskreditDescription teilzahlungskredit, string kreditgesuch, int zielverein)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-RegisterTeilzahlungskredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.registerTeilzahlungskredit(idDesc, requestEntities, teilzahlungskredit, kreditgesuch, zielverein);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-RegisterTeilzahlungskredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="teilzahlungsvertrag"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse registerTeilzahlungsvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungsvertragDescription teilzahlungsvertrag, string kreditgesuch, int zielverein)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-RegisterTeilzahlungsvertrag", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.registerTeilzahlungsvertrag(idDesc, requestEntities, teilzahlungsvertrag, kreditgesuch, zielverein);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-RegisterTeilzahlungskredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method meldungKartenengagement (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kartenengagement"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse meldungKartenengagement(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.KartenengagementDescription kartenengagement, string kreditgesuch, int zielverein)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-MeldungKartenengagement", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.meldungKartenengagement(idDesc, requestEntities, kartenengagement, kreditgesuch, zielverein);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-MeldungKartenengagement Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsanmeldung, method meldungUeberziehungskredit (EC4)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="ueberziehungskredit"></param>
        /// <param name="kreditgesuch"></param>
        /// <param name="zielverein"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse meldungUeberziehungskredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.UeberziehungskreditDescription ueberziehungskredit, string kreditgesuch, int zielverein)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-MeldungUeberziehungskredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.meldungUeberziehungskredit(idDesc, requestEntities, ueberziehungskredit, kreditgesuch, zielverein);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-MeldungUeberziehungskredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Mutation Grunddaten, method grunddatenMutieren (EC6)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntity"></param>
        /// <param name="requestEntityNew"></param>
        /// <returns></returns>
        public ZEKRef.UpdateAddressResponse updateAddress(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity requestEntity, ZEKRef.RequestEntity requestEntityNew)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.UpdateAddressResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.UpdateAddressResponse>();
                return t.call("ZEK-UpdateAddress", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.grunddatenMutieren(idDesc, requestEntity, requestEntityNew);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-UpdateAddress Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateBardarlehen (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="bardarlehen"></param>
        /// <param name="bardarlehenNew"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse updateBardarlehen(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.BardarlehenDescription bardarlehen, ZEKRef.BardarlehenDescription bardarlehenNew)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-UpdateBardarlehen", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.updateBardarlehen(idDesc, requestEntities, bardarlehen, bardarlehenNew);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-UpdateBardarlehen Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateFestkredit (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kredit"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse updateFestkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.Festkredit kredit, ZEKRef.Festkredit kreditNew)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-UpdateFestkredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.updateFestkredit(idDesc, requestEntities, kredit, kreditNew);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-UpdateFestkredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateKontokorrentkredit (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kredit"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse updateKontokorrentkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.KontokorrentkreditDescription kredit, ZEKRef.KontokorrentkreditDescription kreditNew)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-UpdateKontokorrentkredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.updateKontokorrentkredit(idDesc, requestEntities, kredit, kreditNew);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-UpdateKontokorrentkredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateLeasingMietvertragkredit (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kredit"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse updateLeasingMietvertragkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.LeasingMietvertragDescription kredit, ZEKRef.LeasingMietvertragDescription kreditNew)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-UpdateLeasingMietvertrag", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.updateLeasingMietvertrag(idDesc, requestEntities, kredit, kreditNew);


                });
             
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-UpdateLeasingMietvertrag Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Mutation Vertragsdaten, method updateTeilzahlungskredit (EC7)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kredit"></param>
        /// <param name="kreditNew"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse updateTeilzahlungskredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungskreditDescription kredit, ZEKRef.TeilzahlungskreditDescription kreditNew)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-UpdateTeilzahlungskredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.updateTeilzahlungskredit(idDesc, requestEntities, kredit, kreditNew);


                });
              
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-UpdateTeilzahlungskredit Serviceaufruf ", ex);
                throw ex;
            }
        }


        public ZEKRef.CommonMultiResponse updateTeilzahlungsvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungsvertragDescription vertrag, ZEKRef.TeilzahlungsvertragDescription vertragNew)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-UpdateTeilzahlungsvertrag", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.updateTeilzahlungsvertrag(idDesc, requestEntities, vertrag, vertragNew);
                });

            
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-UpdateTeilzahlungskredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeBardarlehen (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="bardarlehen"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse closeBardarlehen(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.BardarlehenDescription bardarlehen)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-closeBardarlehen", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.closeBardarlehen(idDesc, requestEntities, bardarlehen);
                });

               
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-closeBardarlehen Serviceaufruf ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeLeasingMietvertrag (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="leasingMietvertrag"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse closeLeasingMietvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.LeasingMietvertragDescription leasingMietvertrag)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-closeLeasingMietvertrag", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.closeLeasingMietvertrag(idDesc, requestEntities, leasingMietvertrag);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-closeLeasingMietvertrag Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeFestkredit (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="circumstantialCreditDescription"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse closeFestkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.Festkredit circumstantialCreditDescription)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-closeFestkredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.closeFestkredit(idDesc, requestEntities, circumstantialCreditDescription);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-closeFestkredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeTeilzahlungskredit (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="teilzahlungskredit"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse closeTeilzahlungskredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungskreditDescription teilzahlungskredit)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-closeTeilzahlungskredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.closeTeilzahlungskredit(idDesc, requestEntities, teilzahlungskredit);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-closeTeilzahlungskredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        public ZEKRef.CommonMultiResponse closeTeilzahlungsvertrag(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.TeilzahlungsvertragDescription teilzahlungsvertrag)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-closeTeilzahlungsvertrag", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.closeTeilzahlungsvertrag(idDesc, requestEntities, teilzahlungsvertrag);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-closeTeilzahlungsvertrag Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls ZEK Webservice Vertragsabmeldung, method closeKontokorrentkredit (EC5)
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntities"></param>
        /// <param name="kontokorrentkredit"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse closeKontokorrentkredit(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity[] requestEntities, ZEKRef.KontokorrentkreditDescription kontokorrentkredit)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-closeKontokorrentkredit", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.closeKontokorrentkredit(idDesc, requestEntities, kontokorrentkredit);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-closeKontokorrentkredit Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// eCode178Anmelden
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="requestEntity"></param>
        /// <param name="eCode178"></param>
        /// <param name="kreditGesuchID"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse eCode178Anmelden(ZEKRef.IdentityDescriptor idDesc, ZEKRef.RequestEntity requestEntity, ZEKRef.eCode178 eCode178, string kreditGesuchID, string contractId)
        {
              try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-eCode178Anmelden", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.eCode178Anmelden(idDesc, requestEntity, eCode178, kreditGesuchID, contractId);
                });



            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-eCode178Anmelden Serviceaufruf ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// eCode178Mutieren
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="eCode178"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse eCode178Mutieren(ZEKRef.IdentityDescriptor idDesc, ZEKRef.eCode178 eCode178)
        {
              try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-eCode178Mutieren", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.eCode178Mutieren(idDesc, eCode178);
                });

            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-eCode178Mutieren Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// eCode178Abnmelden
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="eCode178"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse eCode178Abmelden(ZEKRef.IdentityDescriptor idDesc, ZEKRef.eCode178 eCode178)
        {
            try
            {

                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-closeBardarlehen", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.eCode178Abmelden(idDesc, eCode178);
                });



            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-eCode178Anmelden Serviceaufruf ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// eCode178Abfrage
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="eCode178"></param>
        /// <returns></returns>
        public ZEKRef.CommonMultiResponse eCode178Abfrage(ZEKRef.IdentityDescriptor idDesc, ZEKRef.eCode178 eCode178)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.CommonMultiResponse>();
                return t.call("ZEK-eCode178Abfrage", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.eCode178Abfrage(idDesc, eCode178);
                });



            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-eCode178Anmelden Serviceaufruf ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// getARMs
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="dateLastSuccessfullRequest"></param>
        /// <returns></returns>
        public ZEKRef.ArmResponse getARMs(ZEKRef.IdentityDescriptor idDesc, string dateLastSuccessfullRequest)
        {
            try
            {
                ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.ArmResponse> t = new ServiceFaultHandler<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService, ZEKRef.ArmResponse>();
                return t.call("ZEK-getARMs", soapXMLDto, delegate(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef.ZEKService client)
                {
                    return client.getARMs(idDesc, dateLastSuccessfullRequest);
                });


            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-getARMs Serviceaufruf ", ex);
                throw ex;
            }
        }

        #region Get/Set Methods

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        /// <returns></returns>
        public Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto()
        {
            return this.soapXMLDto;
        }

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto">soapXMLDto</param>
        public void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }


        #endregion Get/Set Methods
    }
}