using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public class B2CBOSAdapterFactory : IEaiBOSAdapterFactory
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const String CMD_B2C_NEWANGEBOT= "B2C_NEWANGEBOT";

        public IEaiBOSAdapter getEaiBOSAdapter(String method)
        {
            switch (method)
            {
                case (CMD_B2C_NEWANGEBOT):
                    return new B2CNewAngebotAdapter();
            }
            return null;
        }

        /// <summary>
        /// Adapter for creating the bpe job and setting abwicklungsort for an offer from b2c
        /// </summary>
        private class B2CNewAngebotAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                try
                {

                    IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                    DTO.AngebotDto angebot = bo.getAngebot(eaihot.SYSOLTABLE.Value);
                    try
                    {
                        using (DdOwExtended context = new DdOwExtended())
                        {
                            long sysabwicklung = AngAntDao.getAbwicklungsortB2C(angebot, dao);
                            context.ExecuteStoreCommand("update angebot set sysabwicklung=" + sysabwicklung + " where sysid=" + angebot.sysid);
                        }
                    }catch(Exception ex)
                    {
                        _log.Error("Abwicklungsort not found", ex);
                    }
                    
                    bo.processAngebotEinreichung(angebot, eaihot.SYSWFUSER.Value, "de-CH");
                    eaihot.OUTPUTPARAMETER1 = "OK";

                    dao.updateEaihot(eaihot);
                }
                catch (Exception e)
                {
                    
                    eaihot.OUTPUTPARAMETER1 = "Failure";
                    eaihot.OUTPUTPARAMETER2 = stripString(e.Message);
                    _log.Error("Error in IEaiBOSAdapter for B2C_NEWANGEBOT", e);
                    dao.updateEaihot(eaihot);
                }
            }
            private static String stripString(String s)
            {
                if (s == null)
                {
                    return s;
                }
                if (s.Length > 255)
                {
                    return s.Substring(0, 255);
                }
                return s;
            }
        }
    }
}
