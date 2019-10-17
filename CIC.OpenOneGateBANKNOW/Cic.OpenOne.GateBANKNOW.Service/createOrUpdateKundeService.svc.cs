using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "createOrUpdateKundeService" in code, svc and config file together.
    /// <summary>
    /// Der Service createOrUpdateKunde liefert Listen verschiedener Auswahlmöglichkeiten. Außerdem kann man die Kundendaten editieren und den Kunden Speichern.
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class createOrUpdateKundeService : IcreateOrUpdateKundeService
    {
        /// <summary>
        /// Liefert eine Liste der verfügbaren Anreden
        /// </summary>
        /// <returns>olistAnredenDto</returns>
        public olistAnredenDto listAnreden()
        {
            olistAnredenDto rval = new olistAnredenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.anreden = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listAnreden();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Ausweisarten
        /// </summary>
        /// <returns>olistAusweisartenDto</returns>
        public olistAusweisartenDto listAusweisarten()
        {
            olistAusweisartenDto rval = new olistAusweisartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.ausweisarten = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listAusweisarten();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Länder
        /// </summary>
        /// <returns>olistLaenderDto</returns>
        public olistLaenderDto listLaender()
        {
            olistLaenderDto rval = new olistLaenderDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();

                rval.laender = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listLaender();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Kantone
        /// </summary>
        /// <returns>olistKantoneDto</returns>
        public olistKantoneDto listKantone()
        {
            olistKantoneDto rval = new olistKantoneDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.kantone = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listKantone(cctx.getUserLanguange());

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Sprachen
        /// </summary>
        /// <returns>olistSprachenDto</returns>
        public olistSprachenDto listSprachen()
        {
            olistSprachenDto rval = new olistSprachenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.sprachen = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listSprachen();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Zivilstände
        /// </summary>
        /// <returns>olistZivilstaendeDto</returns>
        public olistZivilstaendeDto listZivilstaende()
        {
            olistZivilstaendeDto rval = new olistZivilstaendeDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.zivilstaende = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listZivilstaende();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Nationalitäten
        /// </summary>
        /// <returns>olistNationalitaetenDto</returns>
        public olistNationalitaetenDto listNationalitaeten()
        {
            olistNationalitaetenDto rval = new olistNationalitaetenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.nationalitaeten = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listNationalitaeten();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Kundentypen im Kontext
        /// </summary>
        /// <returns>olistAvailableKundentypenDto</returns>
        public olistAvailableKundentypenDto listAvailableKundentypen()
        {
            olistAvailableKundentypenDto rval = new olistAvailableKundentypenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.kundentypen = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableKundentypen(cctx.getUserLanguange());

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren beruflichen Situationen
        /// </summary>
        /// <returns>olistBeruflicheSituationDto</returns>
        public olistBeruflicheSituationenDto listBeruflicheSituationen()
        {
            olistBeruflicheSituationenDto rval = new olistBeruflicheSituationenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.beruflicheSituationen = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listBeruflicheSituationen();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Wohnsituationen
        /// </summary>
        /// <returns>olistWohnSituationenDto</returns>
        public olistWohnSituationenDto listWohnSituationen()
        {
            olistWohnSituationenDto rval = new olistWohnSituationenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.wohnSituationen = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listWohnSituationen();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <returns></returns>
        /// <summary>
        /// Liefert Kanton und Ort zur Postleitzahl
        /// </summary>
        /// <param name="input">ifindOrtByPlzDto</param>
        /// <returns>ofindOrtByPlzDto</returns>
        public ofindOrtByPlzDto findOrtByPlz(ifindOrtByPlzDto input)
        {
            ofindOrtByPlzDto rval = new ofindOrtByPlzDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                
                rval.plzDto = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.PlzDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.PlzDto[]>(BOFactory.getInstance().createAdresseBo().findOrtByPlz(input.plz));
                if (rval.plzDto == null || rval.plzDto.Count() == 0)
                {
                    throw new ArgumentException("Es gibt die Postleitzahl(" + input.plz + ") nicht");
                }

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval,new ServiceBaseException("F_00003_ArgumentException",e.Message,MessageType.Warn));
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt des Händlerkunden
        /// </summary>
        /// <param name="input">icreateKundeDto</param>
        /// <returns>ocreateKundeDto</returns>
        public ocreateKundeDto createOrUpdateKunde(icreateKundeDto input)
        {
            ocreateKundeDto rval = new ocreateKundeDto();
            CredentialContext cctx = new CredentialContext();
            if (input != null)
            {
                if (input.Kunde != null)
                {
                    try
                    {
                        cctx.validateService();
                        AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);
                        IKundeBo bo = BOFactory.getInstance().createKundeBo();

                        Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kundeInput = new Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto();
                        Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.Kunde, kundeInput);

                        Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto kundeOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto>(bo.createOrUpdateKunde(kundeInput, cctx.getMembershipInfo().sysPEROLE));
                        rval.kunde = kundeOutput;
                        rval.success();
                        return rval;
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                        return rval;
                    }
                    catch (ArgumentException e)
                    {
                        cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                        return rval;
                    }
                    catch (ServiceBaseException e)//expected service exceptions
                    {
                        cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                        return rval;
                    }
                    catch (Exception e)//unhandled exception - shoLdnt happen!
                    {
                        cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                        return rval;
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Kunde is set";
                    return rval;
                }
            }
            else
            {
                rval.message = new OpenOne.Common.DTO.Message();
                rval.message.code = "Missing Data";
                rval.message.detail = "No Inputdata is set";
                return rval;
            }
        }

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt der Adresse des Händlerkunden
        /// </summary>
        /// <param name="input">icreateAdresseDto</param>
        /// <returns>ocreateAdresseDto</returns>
        public ocreateAdresseDto createOrUpdateAdresse(icreateAdresseDto input)
        {
            ocreateAdresseDto rval = new ocreateAdresseDto();
            CredentialContext cctx = new CredentialContext();
            if (input != null)
            {
                if (input.Adresse != null)
                {
                    try
                    {
                        cctx.validateService();
                        AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                        IAdresseBo bo = new AdresseBo(CommonDaoFactory.getInstance().getAdresseDao());

                        Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto adresseInput = new Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto();
                        Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.AdresseDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto>(input.Adresse, adresseInput);

                        Cic.OpenOne.GateBANKNOW.Service.DTO.AdresseDto adresseOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AdresseDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AdresseDto>(bo.createOrUpdateAdresse(adresseInput));
                        rval.adresse = adresseOutput;
                        rval.success();
                        return rval;
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                        return rval;
                    }
                    catch (ServiceBaseException e)//expected service exceptions
                    {
                        cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                        return rval;
                    }
                    catch (Exception e)//unhandled exception - shoLdnt happen!
                    {
                        cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                        return rval;
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Adresse is set";
                    return rval;
                }
            }
            else
            {
                rval.message = new OpenOne.Common.DTO.Message();
                rval.message.code = "Missing Data";
                rval.message.detail = "No Inputdata is set";
                return rval;
            }
        }

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt der Kontoverbindung des Händlerkunden
        /// </summary>
        /// <param name="input">icreateKontoDto</param>
        /// <returns>ocreateKontoDto</returns>
        public ocreateKontoDto createOrUpdateKonto(icreateKontoDto input)
        {
            ocreateKontoDto rval = new ocreateKontoDto();
            CredentialContext cctx = new CredentialContext();
            if (input != null)
            {
                if (input.Konto != null)
                {
                    try
                    {
                        cctx.validateService();
                        AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                        IKontoBo bo = new KontoBo(new KontoDao());

                        Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto kontoInput = new Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto();
                        Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KontoDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto>(input.Konto, kontoInput);

                        Cic.OpenOne.GateBANKNOW.Service.DTO.KontoDto kontoOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KontoDto, Cic.OpenOne.GateBANKNOW.Service.DTO.KontoDto>(bo.createOrUpdateKonto(kontoInput));
                        rval.konto = kontoOutput;
                        rval.success();
                        return rval;
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                        return rval;
                    }
                    catch (ServiceBaseException e)//expected service exceptions
                    {
                        cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                        return rval;
                    }
                    catch (Exception e)//unhandled exception - shoLdnt happen!
                    {
                        cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                        return rval;
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Konto is set";
                    return rval;
                }
            }
            else
            {
                rval.message = new OpenOne.Common.DTO.Message();
                rval.message.code = "Missing Data";
                rval.message.detail = "No Inputdata is set";
                return rval;
            }
        }

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt der Zusatzdaten des Händlerkunden
        /// </summary>
        /// <param name="input">icreateZusatzdatenDto</param>
        /// <returns>ocreateZusatzdatenDto</returns>
        public ocreateZusatzdatenDto createOrUpdateZusatzdaten(icreateZusatzdatenDto input)
        {
            ocreateZusatzdatenDto rval = new ocreateZusatzdatenDto();
            CredentialContext cctx = new CredentialContext();
            if (input != null)
            {
                if (input.ZusatzdatenDto != null)
                {
                    if (input.ZusatzdatenDto.kdtyp != 0)
                    {
                        try
                        {
                            cctx.validateService();
                            AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);
                            IZusatzdatenBo bo = BOFactory.getInstance().createZusatzdatenBo();

                            Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto zusatzdatenInput = new Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto();
                            Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.ZusatzdatenDto, Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto>(input.ZusatzdatenDto, zusatzdatenInput);

                            Cic.OpenOne.GateBANKNOW.Service.DTO.ZusatzdatenDto zusatzdatenOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ZusatzdatenDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ZusatzdatenDto>(bo.createOrUpdateZusatzdaten(zusatzdatenInput, new Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto()));
                            rval.zusatzdaten = zusatzdatenOutput;
                            rval.success();
                            return rval;
                        }
                        catch (System.Data.Entity.Core.EntityException e)
                        {
                            cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                            return rval;
                        }
                        catch (ServiceBaseException e)//expected service exceptions
                        {
                            cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                            return rval;
                        }
                        catch (Exception e)//unhandled exception - shoLdnt happen!
                        {
                            cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                            return rval;
                        }
                    }
                    else
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "Missing Pflichtfeld";
                        rval.message.detail = "No KDTYP is set (KDTYP must be " + AngAntBo.KDTYPID_PRIVAT + " or " + AngAntBo.KDTYPID_FIRMA + ")";
                        return rval;
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Zusatzdaten is set";
                    return rval;
                }
            }
            else
            {
                rval.message = new OpenOne.Common.DTO.Message();
                rval.message.code = "Missing Data";
                rval.message.detail = "No Inputdata is set";
                return rval;
            }
        }

        /// <summary>
        /// Speichert alle Persistenzobjekte des Händlerkunden
        /// </summary>
        /// <param name="input">isaveKundeDto</param>
        /// <returns>osaveKundeDto</returns>
        public osaveKundeDto saveKunde(isaveKundeDto input)
        {
            osaveKundeDto rval = new osaveKundeDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input saveKundeDto is send");
                }
                cctx.validateService();
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IKundeBo bo = BOFactory.getInstance().createKundeBo();

                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kundeInput = new Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto();
                Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.KundeDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(input.kunde, kundeInput);

                bo.createOrUpdateKunde(kundeInput, cctx.getMembershipInfo().sysPEROLE);
                rval.message = new Cic.OpenOne.Common.DTO.Message();
                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// listZusatzeinkommen
        /// </summary>
        /// <returns></returns>
        public olistZusatzeinkommenDto listZusatzeinkommen()
        {
            olistZusatzeinkommenDto rval = new olistZusatzeinkommenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.zusatzeinkommen = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listZusatzeinkommen();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// listAuslagenarten
        /// </summary>
        /// <returns></returns>
        public olistAuslagenartenDto listAuslagenarten()
        {
            olistAuslagenartenDto rval = new olistAuslagenartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.auslagenarten = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listAuslagenarten();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// listUnterstuetzungsarten
        /// </summary>
        /// <returns></returns>
        public olistUnterstuetzungsartenDto listUnterstuetzungsarten()
        {
            olistUnterstuetzungsartenDto rval = new olistUnterstuetzungsartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.unterstuetzungsarten = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listUnterstuetzungsarten();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// listRechtsformen
        /// </summary>
        /// <returns></returns>
        public olistRechtsformenDto listRechtsformen()
        {
            olistRechtsformenDto rval = new olistRechtsformenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.rechtsformen = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listRechtsformen();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Listet alle Branchen auf
        /// </summary>
        /// <returns>olistBranchenDto</returns>
        public olistBranchenDto listBranchen()
        {
            olistBranchenDto rval = new olistBranchenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.branchen = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listBranchen();
                foreach (DropListDto dl in rval.branchen)
                    dl.beschreibung = null;
                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Berufsauslagenarten
        /// </summary>
        /// <returns></returns>
        public olistBerufsauslagenartenDto listBerufsauslagenarten()
        {
            olistBerufsauslagenartenDto rval = new olistBerufsauslagenartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.berufsauslagenarten = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listBerufsauslagenart();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Sucht alle Blz für den übergebenen Filter (BLZ|IBAN|BIC)
        /// </summary>
        /// <param name="input">ifindBlzDto</param>
        /// <returns>ofindBlzDto</returns>
        public ofindBlzDto findBlz(ifindBlzDto input)
        {
            ofindBlzDto rval = new ofindBlzDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                IKontoBo bo = new KontoBo(new KontoDao());

                rval.blz = Mapper.Map<List<Cic.OpenOne.GateBANKNOW.Common.DTO.BlzDto>, List<Cic.OpenOne.GateBANKNOW.Service.DTO.BlzDto>>(bo.findBlz(input.searchValue, input.searchType));

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Ermittelt Bankname und BankId für eine Kontonr und BLZ
        /// </summary>
        /// <param name="input">ifindBankByBlzDto</param>
        /// <returns>Bankinformationen</returns>
        public ofindBankByBlzDto findBankByBlz(ifindBankByBlzDto input)
        {
            ofindBankByBlzDto rval = new ofindBankByBlzDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                IKontoBo bo = new KontoBo(null);
                
                
                rval = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ofindBankByBlzDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ofindBankByBlzDto>(bo.findBankByBlz(input.kontoNr, input.bcpcNummer));

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }


        /// <summary>
        /// Ermittelt IBAN für eine Kontonr und BLZ
        /// </summary>
        /// <param name="input">ifindBankByBlzDto</param>
        /// <returns>IBAN</returns>
        public ofindIBANByBlzDto findIBANByBlz(ifindBankByBlzDto input)
        {
            ofindIBANByBlzDto rval = new ofindIBANByBlzDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                IKontoBo bo = new KontoBo(null);

                
                rval = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.ofindIBANByBlzDto, Cic.OpenOne.GateBANKNOW.Service.DTO.ofindIBANByBlzDto>(bo.findIBANByBlz(input.kontoNr, input.bcpcNummer));

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }


        /// <summary>
        /// Ermittelt Kontonummer für eine IBAN
        /// </summary>
        /// <param name="input">ifindBankByBlzDto</param>
        /// <returns>IBAN</returns>
        public ofindKontoNrByIBANDto findKontoNrByIBAN(ifindKontoNrByIBANDto input) 
        {
            ofindKontoNrByIBANDto rval = new ofindKontoNrByIBANDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                IKontoBo bo = new KontoBo(null);
                IBANValidator v = new IBANValidator();

                Cic.OpenOne.Common.DTO.IBANValidationError err = v.checkIBAN(input.iban);
                if (err.error == OpenOne.Common.DTO.IBANValidationErrorType.NoError)
                {
                    rval.kontoNr = v.getKontonummer(input.iban);

                }
                else
                    rval.kontoNr = "";
                rval.success();
                return rval;

            }

            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }


        /// <summary>
        /// Liefert eine Liste der verfügbaren Mitantragsteller Zustände
        /// </summary>
        /// <returns>olistMitantStatiDto</returns>
        public olistMitantStatiDto listMitantragstellerStati()
        {
            olistMitantStatiDto rval = new olistMitantStatiDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.mitantragstellerStati = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listMitantragstellerStati();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// weitere Auslagenarten Liste liefern
        /// </summary>
        /// <returns></returns>
        public olistAuslagenartenDto listWeitereAuslagenarten()
        {
            olistAuslagenartenDto rval = new olistAuslagenartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.auslagenarten = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listWeitereAuslagenarten();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Verwendungszweck Liste liefern
        /// </summary>
        /// <returns>olistVerwendungszweckDto</returns>
        public olistVerwendungszweckDto listVerwendungszweck()
        {
            olistVerwendungszweckDto rval = new olistVerwendungszweckDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.verwendungszweck = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listVerwendungszweck();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der Fremdbanken.
        /// Ticket#2012070910000019 — Neuer Webservice Ermittlung Fremdbanken.
        /// Dieser Service ist nur für die Kreditfinanzierung.
        /// </summary>
        /// <returns>olistFremdBankenDto</returns>
        public olistFremdBankenDto listFremdBanken()
        {
            olistFremdBankenDto rval = new olistFremdBankenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();

                DictionaryListsBo dictionaryListsBo = new DictionaryListsBo(
                                                            Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(),
                                                            Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), 
                                                            cctx.getUserLanguange()
                                                        );
                rval.fremdbanken = dictionaryListsBo.listFremdBanken();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }


        /// <summary>
        /// Attaches the given disclaimer to the given area/id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateDisclaimerDto createDisclaimer(icreateDisclaimerDto input)
        {
            ocreateDisclaimerDto rval = new ocreateDisclaimerDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                cctx.validateService();
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);


                BOFactory.getInstance().createDisclaimerBo().createDisclaimer(input.area.ToString(), input.disclaimerType, input.sysid, cctx.getMembershipInfo().sysWFUSER, input.inhalt);

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

    }
}
