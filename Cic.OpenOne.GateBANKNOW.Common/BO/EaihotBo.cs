using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST;
using Cic.OpenOne.GateBANKNOW.Common.BO.Score;
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
    /// BO for handling eaihots (like incoming calls delegated to a service)
    /// 
    /// All Factories that create adapters to call a method from eaihot/qin must be registered here!
    /// </summary>
    public class EaihotBo : IEaihotBo
    {
        private IEaihotDao dao;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<IEaiBOSAdapterFactory> factories = new List<IEaiBOSAdapterFactory>();

        public EaihotBo(IEaihotDao dao)
        {
            this.dao = dao;
            //register currently available factories
            registerFactory(new TestBOSAdapterFactory());
            registerFactory(new FoodasEaiBOSAdapterFactory());
            registerFactory(new DMSEaiBOSAdapterFactory());
            registerFactory(new BCAVINBOSAdapterFactory());
            registerFactory(new B2CBOSAdapterFactory());

            registerFactory(new DMREaiBOSAdapterFactory());
			registerFactory (new SCOREEaiBOSAdapterFactory ());
        }

        /// <summary>
        /// Registers a factory being able to process webservices by eaihot-codes
        /// </summary>
        /// <param name="fac"></param>
        public void registerFactory(IEaiBOSAdapterFactory fac)
        {
            factories.Add(fac);
        }

        /// <summary>
        /// Creates an eaihot for the generic bos call Eventengine feature
        /// </summary>
        /// <param name="code"></param>
        /// <param name="gebiet"></param>
        /// <param name="gebietId"></param>
        /// <param name="syswfuser"></param>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <param name="input3"></param>
        public void createEAIBosCall(String code, String gebiet, long gebietId, long syswfuser, String input1, String input2, String input3)
        {
            EAIART eaiart = dao.getEaiArt("#GenericBOSCall");
            EaihotDto eaihotInput = new EaihotDto();
            eaihotInput.CODE = code;
            eaihotInput.PROZESSSTATUS = 0;
            eaihotInput.HOSTCOMPUTER = "*";
            eaihotInput.EVE = 0;
            eaihotInput.OLTABLE = gebiet;
            eaihotInput.SYSOLTABLE = gebietId;
            eaihotInput.SYSWFUSER = syswfuser;
            eaihotInput.SYSEAIART = eaiart.SYSEAIART;
            eaihotInput.INPUTPARAMETER1 = input1;
            eaihotInput.INPUTPARAMETER2 = input2;
            eaihotInput.INPUTPARAMETER3 = input3;
            eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
            eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
            eaihotInput = dao.createEaihot(eaihotInput);

            //activate now
            dao.activateEaihot(eaihotInput, 1);
        }
        /// <summary>
        /// Execute the eaihot for the given id
        /// must be of code CALL_BOS
        /// </summary>
        /// <param name="sysEaiHOT"></param>
        public void execEAIHOT(int sysEaiHOT)
        {
            EaihotDto eaihot = dao.getEaihot(sysEaiHOT);
           /* Cic.OpenOne.Common.Model.DdOw.EAIART eaiart = dao.getEaiArt("#GenericBOSCall");
            if(eaiart==null)
            {
                _log.Error("exec EAIHOT " + sysEaiHOT + " impossible - no EAIART.CODE=='#GenericBOSCall' found");
                throw new Exception("exec EAIHOT " + sysEaiHOT + " impossible - no EAIRAT.CODE=='#GenericBOSCall' found");
                
            }*/
            if(eaihot==null)
            {
                throw new Exception("exec EAIHOT " + sysEaiHOT + " impossible - no EAIHOT failed");
            }
            if (eaihot.EVE == 0)
            {
                eaihot.PROZESSSTATUS = 1;
                eaihot.STARTDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                eaihot.STARTTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                eaihot = dao.updateEaihot(eaihot);
            }
        /*    if(eaihot.SYSEAIART!=eaiart.SYSEAIART)
            {
                throw new Exception("exec EAIHOT " + sysEaiHOT + " failed - not of EAIART.CODE=='#GenericBOSCall'");
            }*/
            bool found = false;
            foreach(IEaiBOSAdapterFactory fac in factories)
            {
                IEaiBOSAdapter adapter = fac.getEaiBOSAdapter(eaihot.CODE);
                if(adapter!=null)
                {
                    _log.Debug("Call Service via #GenericBOSCall, found " + adapter + " for " + eaihot.CODE);
                    try
                    {
                        
                        adapter.processEaiHot(dao, eaihot);
                        if (eaihot.EVE == 0)
                        {
                            eaihot.PROZESSSTATUS = 2;
                            eaihot.FINISHDATE = DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
                            eaihot.FINISHTIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                            eaihot = dao.updateEaihot(eaihot);
                        }
                    }catch(Exception e)
                    {
                        if (eaihot.EVE == 0)
                        {
                            eaihot.PROZESSSTATUS = 3;
                            eaihot.FINISHDATE = DateTimeHelper.DateTimeToClarionDate(DateTime.Now);
                            eaihot.FINISHTIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                           
                            eaihot = dao.updateEaihot(eaihot);
                        }
                        _log.Error("#GenericBOSCall in " + adapter + " for code " + eaihot.CODE + " failed: ", e);
                        throw new Exception("#GenericBOSCall in " + adapter + " for code " + eaihot.CODE + " failed", e);
                    }
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                throw new Exception("Call Service via #GenericBOSCall failed, no Factory registered for " + eaihot.CODE);
            }
            
        }

      
    }

    /// <summary>
    /// Adapter for a always successful TEST
    /// </summary>
    public class TestBOSAdapterFactory : IEaiBOSAdapterFactory
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IEaiBOSAdapter getEaiBOSAdapter(String method)
        {
            if("TEST".Equals(method))
            {
                return new TestEaiBOSAdapter();
            }
            return null;
        }
        /// <summary>
        /// Adapter for calling createOrUpdateDMSAkte from eaihot
        /// </summary>
        private class TestEaiBOSAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                _log.Debug("execEAIHOT successful with method TEST");
                eaihot.OUTPUTPARAMETER1 = "Hello World from BOS 1";
                eaihot.OUTPUTPARAMETER2 = "Hello World from BOS 2";
                eaihot.OUTPUTPARAMETER3 = "Hello World from BOS 3";
                eaihot.OUTPUTPARAMETER4 = "Hello World from BOS 4";
                eaihot.OUTPUTPARAMETER5 = "Hello World from BOS 5";
                dao.updateEaihot(eaihot);

            }
        }
    }
}
