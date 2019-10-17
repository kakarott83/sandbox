using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{


    public class TransactionRisikoGUIBo
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Cic.OpenOne.GateBANKNOW.Common.BO.AngAntBo BO = new Cic.OpenOne.GateBANKNOW.Common.BO.AngAntBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDaoMA(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getKundeDao(), new PrismaParameterBo(PrismaDaoFactory.getInstance().getPrismaDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), PrismaParameterBo.CONDITIONS_BANKNOW), new TranslateBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()), OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao(), new VGDao(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao(), BOFactory.getInstance().createTransactionRisikoBO());
        Cic.OpenOne.GateBANKNOW.Common.DAO.IAngAntDao angAntDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDaoMA();
        

        public ocheckTrRiskByIdDto checkantragbyIdGUI(icheckTrRiskByIdDto inDto)
        {

             
            

            ocheckTrRiskByIdDto rval = new ocheckTrRiskByIdDto();
            try
            {


                ocheckAntAngDto rvalCheck = new ocheckAntAngDto();


                icheckTrRiskByIdDto icheckTR = new icheckTrRiskByIdDto();
                icheckTR.sysPEROLE = inDto.sysPEROLE;
                icheckTR.sysid = inDto.sysid;
                icheckTR.isoCode = inDto.isoCode;

                rvalCheck = BO.checkAntragByIdErweiterung(inDto.sysid, 1, "de-CH", false, false, 387, inDto.sysWFUSER, inDto.sysPEROLE);


                if (rvalCheck.status == Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_GREEN)
                {
                    // TransactionRisiko OK
                    // SimulationRisikoprüfung OK
                    // Die Produktprüfung ist erfolgreich und sie erhalten innerhalb kürzester Zeit die Antragsentscheidung 	CHECK_OK
                    // User kann erneut kalkulieren? nein


                    rval.frontid = "CHECK_OK_GRUEN";




                }

                else
                    if ((rvalCheck.status == Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_RED || rvalCheck.status == Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_YELLOW) && String.Join(",", rvalCheck.code).Equals("FEL1"))
                    {
                        // produktprüfung OK


                        ITransactionRisikoBo boT = BOFactory.getInstance().createTransactionRisikoBO();


                        rval = boT.checkTrRiskBySysid(icheckTR);


                        // TransactionRisiko OK?// TransactionRisiko OK?
                        if (rval.frontid != null && (rval.frontid.Equals("CHECK_OK") || rval.frontid.Equals("CHECK_AUSW_OK")))
                        {

                            if (rval.frontid.Equals("CHECK_AUSW_OK"))
                            {
                                rvalCheck.status = Cic.OpenOne.GateBANKNOW.Common.DTO.ocheckAntAngDto.STATUS_GREEN;
                            }

                            //  SimulationRisikoprüfung OK??
                            //  ja: 
                            //B2BzustandExtern = "Finanzierungsvorschlag";
                            rval.frontid = "CHECK_OK";


                            //  nein: 
                            //rval.frontid = "CHECK_DE_NOK";

                        }
                        else
                            // nein :
                        {
                            rval.frontid = "CHECK_TR_NOK";
                        }
                        // User kann erneut kalkulieren? JA



                    }
                    else
                    {

                        rval.frontid = "CHECK_NOK";

                    }

                return rval;
            }

            catch (System.Data.Entity.Core.EntityException e)
            {
                _log.Info("F_00004_DatabaseUnreachableException ", e);
                return rval;

            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                _log.Info("F_00001_GeneralError ", e);
                return rval;

            }


        }
            
           
    }
}
