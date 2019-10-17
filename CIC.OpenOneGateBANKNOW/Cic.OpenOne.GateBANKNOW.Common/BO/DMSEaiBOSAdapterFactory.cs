using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;
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
    public class DMSEaiBOSAdapterFactory: IEaiBOSAdapterFactory
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IEaiBOSAdapter getEaiBOSAdapter(String method)
        {
            //make it case-insensitive
            String imethod = method.ToUpper();
            switch (imethod)
            {
                case ("CREATEORUPDATEDMSAKTE"):
                    return new CreateOrUpdateDMSAkteAdapter();
                case ("CREATEORUPDATEDMSDOKUMENT"):
                    return new CreateOrUpdateDMSDokumentAdapter();
            }
            return null;
        }

        /// <summary>
        /// Adapter for calling createOrUpdateDMSAkte from eaihot
        /// </summary>
        private class CreateOrUpdateDMSAkteAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                
                if ("DMSBATCH".Equals(eaihot.OLTABLE))
                {
                    Cic.OpenOne.GateBANKNOW.Common.DTO.icreateOrUpdateDMSAkteBatchDto inputa = new icreateOrUpdateDMSAkteBatchDto();
                    inputa.sysdmsbatch = eaihot.SYSOLTABLE.Value;
                    try
                    {
                        ocreateOrUpdateDMSAkteBatchDto rval = BOFactory.getInstance().createDMSBo().createOrUpdateDMSAkteBatch(inputa);
                        eaihot.OUTPUTPARAMETER1 = rval.retcode;
                        eaihot.OUTPUTPARAMETER2 = rval.errcode;
                        eaihot.OUTPUTPARAMETER3 = rval.errmessage;
                    }
                    catch (Exception e)
                    {
                        eaihot.PROZESSSTATUS = 3;
                        eaihot.OUTPUTPARAMETER1 = "403";
                        eaihot.OUTPUTPARAMETER3 = e.Message;
                        _log.Error("Error in createOrUpdateDMSAkte Batchprocessing", e);
                    }
                    dao.updateEaihot(eaihot);
                }
                else
                {

                    Cic.OpenOne.GateBANKNOW.Common.DTO.icreateOrUpdateDMSAkteDto inputa = new icreateOrUpdateDMSAkteDto();
                    inputa.sysdmsakte = eaihot.SYSOLTABLE.Value;
                    try
                    {
                        DMSAKTE akte = BOFactory.getInstance().createDMSBo().createOrUpdateDMSAkte(inputa);
                        eaihot.OUTPUTPARAMETER1 = akte.RETCODE;
                        eaihot.OUTPUTPARAMETER2 = akte.ERRCODE;
                        eaihot.OUTPUTPARAMETER3 = akte.ERRMESSAGE;
                    }
                    catch (Exception e)
                    {
                        eaihot.PROZESSSTATUS = 3;
                        eaihot.OUTPUTPARAMETER1 = "403";
                        eaihot.OUTPUTPARAMETER3 = e.Message;
                        _log.Error("Error in createOrUpdateDMSAkte", e);
                    }
                    dao.updateEaihot(eaihot);
                }
            }
        }

        /// <summary>
        /// Adapter for calling CreateOrUpdateDMSDokument from eaihot
        /// </summary>
        private class CreateOrUpdateDMSDokumentAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                Cic.OpenOne.GateBANKNOW.Common.DTO.icreateOrUpdateDMSDokmentDto input = new icreateOrUpdateDMSDokmentDto();
                input.sysdmsakte = eaihot.SYSOLTABLE.Value;
                EaihfileDto eaihfile = dao.getEaiHotFile(eaihot.SYSEAIHOT);
                if (eaihfile == null)
                {
                    throw new Exception("createOrUpdateDMSDokument not possible for dmsakte " + input.sysdmsakte + " - no EAIHFILE found");
                }
                input.sysdmsdoc = eaihfile.SYSEAIHFILE;
                try { 
                    DMSAKTE akte = BOFactory.getInstance().createDMSBo().createOrUpdateDMSDokument(input);
                    eaihot.OUTPUTPARAMETER1 = akte.RETCODE;
                    eaihot.OUTPUTPARAMETER2 = akte.ERRCODE;
                    eaihot.OUTPUTPARAMETER3 = akte.ERRMESSAGE;
                }
                catch (Exception e)
                {
                    eaihot.PROZESSSTATUS = 3;
                    eaihot.OUTPUTPARAMETER1 = "403";
                    eaihot.OUTPUTPARAMETER3 = e.Message;
                    if(e is AconsoException)
                        _log.Warn("Error in createOrUpdateDMSAkte: "+e.Message);
                    else
                        _log.Error("Error in createOrUpdateDMSAkte: "+e.Message, e);
                }
                dao.updateEaihot(eaihot);
            }
        }


    }
}
