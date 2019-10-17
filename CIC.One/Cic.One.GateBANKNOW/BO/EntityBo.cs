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
using Cic.OpenOne.GateBANKNOW.Common.BO;
using AutoMapper;
using Cic.One.DTO.BN;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.One.GateBANKNOW.BO
{

    /// <summary>
    /// Overrides default getdetail/createorupdate logic for BANKNOW
    /// </summary>
    public class EntityBo : Cic.One.Web.BO.EntityBo
    {
     
        public EntityBo(IEntityDao dao, IAppSettingsBo appBo, ICASBo casBo)
            : base(dao, appBo, casBo)
        {
        }

        /// <summary>
        /// Fetches Auskunft Detail Infos
        /// </summary>
        /// <param name="inp"></param>
        /// <param name="rval"></param>
        public void getAuskunftDetail(igetAuskunftDetailDto inp, ogetAuskunftDetailDto rval)
        {
            ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getAuskunftDetail(inp,rval);          
        }
        
        
        /// <summary>
        /// Returns Vertrag Details
        /// </summary>
        /// <param name="sysAngebot></param>
        /// <returns></returns>
        override public VertragDto getVertragDetails(long sysVertrag)
        {
            VertragDto rval = dao.getVertragDetails(sysVertrag);
            if (rval.produkt != null)
            {
                rval.produkt.versicherungen = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getVTVersicherung(sysVertrag);
            }
            return rval;
        }

        /// <summary>
        /// Returns BN Angebot Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        override public BNAngebotDto getBNAngebotDetails(long sysAngebot)
        {
            IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
            Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto angebot = bo.getAngebot(sysAngebot);

            BNAngebotDto angebotOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto,BNAngebotDto>(angebot);
            angebotOutput.produkt = dao.getProduktInfoAngebotDetails(angebotOutput.getEntityId());
            if (angebotOutput.produkt != null)
            {
                angebotOutput.produkt.versicherungen = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getAngebotVersicherung(sysAngebot);
            }
            if (angebotOutput.mitantragsteller != null)
            {
                angebotOutput.sysMa = angebotOutput.mitantragsteller.syskd;
                angebotOutput.sysItMa = angebotOutput.mitantragsteller.sysit;
            }
            if (angebotOutput.kunde != null)
                angebotOutput.syskd = angebotOutput.kunde.syskd;

			//// ALT: if (angebotOutput.syscamptp == 0 && angebotOutput.syscamp > 0 && dao is Cic.One.GateBANKNOW.DAO.EntityDao)
			//// rh: 20170327: syscamptp immer verarbeiten, --> syscamp muss ab jetzt NICHT MEHR IMMER gesetzt sein!  
			//if (angebotOutput.syscamptp == 0 && dao is Cic.One.GateBANKNOW.DAO.EntityDao)
			//{
			//	// rh 20170327: bisher 1 : 1 realation
			//	angebotOutput.syscamptp = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getSysCampTp(angebotOutput.syscamp);
			//}

			// rh: 20170327: syscamptp immer verarbeiten, --> syscamp muss ab jetzt NICHT MEHR IMMER gesetzt sein!  
			if (angebotOutput.syscamptp == 0 && dao is Cic.One.GateBANKNOW.DAO.EntityDao)
			{
				// rh 20170327: until Automapping from EDMX
				angebotOutput.syscamptp = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getSysCampTp(angebotOutput.sysid);
			}



			//  GET description rh 20170216: SET beraterBezeichnung in detail too
			angebotOutput.beraterBezeichnung = bo.getWfUserBezeichnung (angebot.sysberater);


			// rh 20170405: GET SLA-Indicator
			angebotOutput.SLA = ((Cic.One.GateBANKNOW.DAO.EntityDao) dao).getSla (sysAngebot);

           /* if(angebotOutput.sysVM>0)
            {
                IKundeDao kddao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getKundeDao();
                angebotOutput.haendler = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto,BNKundeDto>(kddao.getKundeBySysKd(angebotOutput.sysVM));
            }*/
            return angebotOutput;
        }

        /// <summary>
        /// Returns BN Antrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        override public BNAntragDto getBNAntragDetails(long sysAntrag)
        {
            IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBoMA();
            IMwStBo mwstBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createMwstBo();
            Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antrag = bo.getAntrag(sysAntrag);
            double mwst =  mwstBo.getMehrwertSteuer(1, antrag.sysvart, DateTime.Now);
            
            BNAntragDto antragOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto,BNAntragDto>(antrag);
            antragOutput.produkt = dao.getProduktInfoAntragDetails(antragOutput.getEntityId());

            BNAntragDto tmp = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getKampagneinfo(sysAntrag);
            if (tmp != null)
            {
                antragOutput.kampagnencode = tmp.kampagnencode;
                antragOutput.eingangskanal = tmp.eingangskanal;
            }

            if (antragOutput.mitantragsteller != null)
            {
                antragOutput.sysMa = antragOutput.mitantragsteller.syskd;
                antragOutput.sysItMa = antragOutput.mitantragsteller.sysit;
            }

            if (antragOutput.kunde != null)
                antragOutput.syskd = antragOutput.kunde.syskd;

            antragOutput.auflagen = dao.getAuflagen(sysAntrag, dao.getISOLanguage());
            antragOutput.regeln = dao.getAuskunftRegeln(sysAntrag, dao.getISOLanguage());
            if (antragOutput.produkt != null) {
                antragOutput.produkt.versicherungen = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getAntragVersicherung(sysAntrag);
                antragOutput.produkt.intsband = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getIntsband(antragOutput.produkt.sysprproduct);
                antragOutput.produkt.indikativerRestwertBrutto = antragOutput.produkt.indikativerRestwert* ( 1 + mwst/100.0);
            }
            if (antragOutput.angAntObDto != null)
            {
                antragOutput.angAntObDto.zustand = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getObjektzustand(antragOutput.angAntObDto.sysobart);
                if (antragOutput.kalkulation != null && antragOutput.kalkulation.angAntKalkDto!=null)
                    antragOutput.angAntObDto.nutzungsart = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getObjektnutzungsart(antragOutput.kalkulation.angAntKalkDto.sysobusetype);
                antragOutput.angAntObDto.quellerw = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getQuellerw(antragOutput.angAntObDto.sysob);
            }
            antragOutput.finanzierungsart = ((Cic.One.GateBANKNOW.DAO.EntityDao)dao).getFinanzierungsart(sysAntrag);
			//  GET description rh 20170216: SET beraterBezeichnung in detail too
			antragOutput.beraterBezeichnung = bo.getWfUserBezeichnung (antrag.sysberater);

			// rh 20170405: GET SLA-Indicator
			antragOutput.SLA = ((Cic.One.GateBANKNOW.DAO.EntityDao) dao).getSla (sysAntrag);

            return antragOutput;
        }


        /// <summary>
        /// Returns all ZEK requests
        /// </summary>
        /// <param name="syszek">primary key</param>
        /// <returns></returns>
        override public ZekDto getZek(long syszek)
        {
            //new ZEKBO()....bla()
            return dao.getZek(syszek);
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
            return rval;
        }


        override public  void updateLegitimationMethode(long sysangebot, long syswfuser, long sysit, string legitimationMethode)
        {

            dao.updateLegitimationMethode(sysangebot, syswfuser, sysit, legitimationMethode);
        }
      

        override public osendRiskmailDto sendRiskmail(isendRiskmailDto input) 
        {
            try
            {
                dao.setRiskMailContact(ref input);
                osendRiskmailDto rval = new osendRiskmailDto();

                Cic.OpenOne.Common.DTO.EaihotDto eaiOutput = new Cic.OpenOne.Common.DTO.EaihotDto();

                String content = input.name + "\n" +
                                 input.vorname + "\n" +
                                 input.departament + "\n" +
                                 input.risikobereich + "\n" +
                                 "Antrags-/Vertragsnummer: " + input.antrag + "\n" +
                                 "Darlehnsbetrag: " + input.darlehnsbetrag + "\n" +
                                 input.vertriebsweg + "\n" +
                                 input.inhalt;

                eaiOutput = new Cic.OpenOne.Common.DTO.EaihotDto()
                {
                    CODE = "MAIL",
                    OLTABLE = "ANTRAG",
                    SYSOLTABLE = input.sysid,
                    SUBMITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 0,
                    INPUTPARAMETER1 = input.empfaenger,
                    INPUTPARAMETER4 = input.absender,
                    INPUTPARAMETER5 = input.betreff,

                    EVALEXPRESSION = content,
                    PROZESSSTATUS = (int)Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotStatusConstants.Pending,
                    HOSTCOMPUTER = "*",

                };
                EaihotDao eaihotDao = new EaihotDao();
                eaiOutput = eaihotDao.createEaihotWithoutEaiArt(eaiOutput);

                if (eaiOutput.SYSEAIHOT == 0)
                {
                    throw new ArgumentException("No DatabaseID was sent.");
                }

                INotificationGatewayBo Gateway = OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createNotificationGateway();

                int Code = Gateway.sendEAINotification((int)eaiOutput.SYSEAIHOT, ConfigurationBO.getServerDaten());

                if (Code == 2)
                {
                    rval.message.code = "True";
                    rval.message.detail = "DB-based Notification sent!";
                    rval.frontid = "SENDMAIL_SUCCESSFUL";
                    rval.success();
                }
                else
                {
                    switch (Code)
                    {
                        case 3:

                                rval.message.code = "True";
                                rval.message.detail = "Preparing DB Notification failed!!";
                                rval.frontid = "SENDMAIL_UNSUCCESSFUL";
                                break;

                          
                          
                        case 4:
                                rval.message.code = "True";
                                rval.message.detail = "Transmission to Server failed!";
                                rval.frontid = "SENDMAIL_UNSUCCESSFUL";
                                break;

                      
                        default:

                                rval.message.code = "True";
                                rval.message.detail = "Error occured sending DB Notification!";
                                rval.frontid = "SENDMAIL_UNSUCCESSFUL";
                                break;
                           
                    }
                }
                return rval;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Could not send riskmail. EAIHOT", ex);
            }


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
        /// Prüfung auf Timeout
        /// </summary>
        /// <param name="oldDate">Alter Zeitstempel</param>
        /// <param name="timeOut">NeuerZeitstempel</param>
        /// <returns>Timeout true= Timeout/false = kein Timeout</returns>
        public static bool isTimeOut(DateTime oldDate, TimeSpan timeOut) {
            TimeSpan ts = DateTime.Now - oldDate;

            if (ts > timeOut)
            {
                return true;
            }

            return false;
        }
    }
}