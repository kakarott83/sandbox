using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Adapts DMS-Service to be called from EAI
    /// </summary>
    public class BCAVINBOSAdapterFactory: IEaiBOSAdapterFactory
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IEaiBOSAdapter getEaiBOSAdapter(String method)
        {
            //make it case-insensitive
            String imethod = method.ToUpper();
            switch (imethod)
            {
                case ("BCA_SYSOBTYP_UPDATE"):
                    return new bcaVINUpdateAdapter();
            }
            return null;
        }

        /// <summary>
        /// Adapter for calling createOrUpdateDMSAkte from eaihot
        /// </summary>
        private class bcaVINUpdateAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                try
                {
                    long sysob = eaihot.SYSOLTABLE.Value;

                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.vinsearchSoapPortClient vs = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.vinsearchSoapPortClient();
                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ETGHeaderType header = getVinHeader();
                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.VinDecodeInputType vinInput = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.VinDecodeInputType();

                    vinInput.VinCode = eaihot.INPUTPARAMETER1;
                    vinInput.ServiceId = getVinServiceId();
                    vinInput.ExtendedOutput = true;
                    vinInput.Settings = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ETGsettingType();
                    vinInput.Settings.ISOcountryCode = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ISOcountryType.DE;
                    vinInput.Settings.ISOlanguageCode = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ISOlanguageType.DE;


                    Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.VinDecodeOutputType outData = vs.VinDecode(ref header, vinInput);
                    if (outData != null && outData.StatusCode == 0)
                    {
                        
                        Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.VehicleType v = outData.Vehicle[0];
                        String schwacke = v.TypeETGCode;
                        eaihot.OUTPUTPARAMETER1 = schwacke;
                        eaihot.OUTPUTPARAMETER2 = "0";
                        eaihot.OUTPUTPARAMETER3 = "Successful";
                        using (PrismaExtended ctx = new PrismaExtended())
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = schwacke });
                            long sysobtyp = ctx.ExecuteStoreQuery<long>("select sysobtyp from obtyp where schwacke=:schwacke", parameters.ToArray()).FirstOrDefault();

                            String bezeichnung = ctx.ExecuteStoreQuery<String>("select bezeichnung from obtyp where sysobtyp="+ sysobtyp).FirstOrDefault();
                            eaihot.OUTPUTPARAMETER4 = "" + sysobtyp;

                            parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = schwacke });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = sysob });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bezeichnung", Value = bezeichnung });
                            ctx.ExecuteStoreCommand("update ob set schwacke=:schwacke, sysobtyp=:sysobtyp, typ=:bezeichnung where sysob=:sysob", parameters.ToArray());
                        }
                    }
                    else
                    {
                        eaihot.OUTPUTPARAMETER2 = "1";
                        if(outData!=null)
                        {
                            eaihot.OUTPUTPARAMETER3 = "WS-Error "+outData.StatusCode;
                        }
                        else
                        {
                            eaihot.OUTPUTPARAMETER3 = "WS no data";
                        }
                    }
                    dao.updateEaihot(eaihot);
                }catch(Exception e)
                {
                    eaihot.OUTPUTPARAMETER2 = "1";
                    eaihot.OUTPUTPARAMETER3 = "WS-Failure";
                    eaihot.OUTPUTPARAMETER4 = e.Message;
                    _log.Error("Error in IEaiBOSAdapter for BCA_SYSOBTYP_UPDATE", e);
                    dao.updateEaihot(eaihot);
                }

            }
            private Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ETGHeaderType getVinHeader()
            {
                EurotaxLoginDataDto accessData = new OpenOne.Common.DAO.Auskunft.AuskunftCfgDao().GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE);

                Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ETGHeaderType header = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.ETGHeaderType();
                Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.LoginDataType LoginData = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.LoginDataType();
                LoginData.Name = accessData.name;
                LoginData.Password = accessData.password;
                header.Originator = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.OriginatorType();
                header.Originator.LoginData = LoginData;
                header.Originator.Signature = accessData.signature;
                header.VersionRequest = Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.VinSearch.VersionType.Item110;
                return header;
            }

            private string getVinServiceId()
            {
                EurotaxLoginDataDto accessData = new EurotaxDBDao().GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE);
                return accessData.serviceId;
            }
        }

       
    }
}
