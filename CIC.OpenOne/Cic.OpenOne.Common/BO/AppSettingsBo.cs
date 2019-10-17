using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Concurrent;
using System.Threading.Tasks;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Registry BO
    /// </summary>
    public class AppSettingsBo : AbstractAppSettingsBo
    {
        private static ConcurrentQueue<Action> queue;
        private static BlockingCollection<Action> blockQueue;

        private static Task insertTask;

        /// <summary>
        /// Application Settings BO for Registry Settings
        /// </summary>
        /// <param name="dao"></param>
        public AppSettingsBo(IAppSettingsDao dao)
            : base(dao)
        {
            if (insertTask == null)//Worker-Thread for async value inserting
            {
                queue = new System.Collections.Concurrent.ConcurrentQueue<Action>();
                blockQueue = new System.Collections.Concurrent.BlockingCollection<Action>(queue);
                insertTask = Task.Factory.StartNew(() =>
                 {
                     while (true)
                     {
                         Action insertJob = blockQueue.Take();
                         insertJob();
                     }
                 });
            }

        }

        /// <summary>
        /// inserts a new entry into the recent list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sysWfUser"></param>
        public override void insertRecent(EntityDto item, long sysWfUser)
        {
            if (item.getEntityId() == 0)
            {
                
                return;
            }
            String code = "rc";// className + "_" + item.entityId;

            icreateOrUpdateAppSettingsItemDto input = new icreateOrUpdateAppSettingsItemDto();
            input.sysWfuser = sysWfUser;
            input.regVar = new RegVarDto();
            input.regVar.sysid = item.getEntityId();
            input.regVar.code = code;
            input.regVar.wert = item.getEntityBezeichnung();
            //input.regVar.path = ;
            input.regVar.syswfuser = sysWfUser;

            input.regVar.area = item.getArea();
            input.regVar.chgdate = DateTime.Now;
            input.regVar.bezeichnung = item.getEntityBezeichnung();// RegVarPaths.getInstance().RECENTLIST + code;

            input.regVar.completePath = RegVarPaths.getInstance().RECENTLIST + code;

            blockQueue.Add(()=>
            {
                dao.createOrUpdateAppSettingsItem(input);
                int MAX_RECENTS = 30;
                String DEL_RECENTS = "select  REGVAR.SYSREGVAR from cic.regsec, cic.regvar  where  regsec.sysregsec=regvar.sysregsec  AND (REGVAR.SYSWFUSER = :syswfuser) AND (REGSEC.CODE = 'RECENT') ORDER BY REGVAR.CHGDATE Desc";
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = sysWfUser });
                using (DdOwExtended ctx = new DdOwExtended())
                {
                    List<long> ids = ctx.ExecuteStoreQuery<long>(DEL_RECENTS, parameters.ToArray()).ToList();
                    if (ids != null)
                    {
                        for (int i = MAX_RECENTS; i < ids.Count; i++)
                        {
                            //rameters = new List<Devart.Data.Oracle.OracleParameter>();
                            //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysregvar", Value = ids[i] });
                            ctx.ExecuteStoreCommand("DELETE FROM REGVAR where sysregvar=" + ids[i]);
                        }
                    }
                }

            });
            

            

        }

        /// <summary>
        /// Updates the Settings later
        /// </summary>
        /// <param name="input"></param>
        public override void createOrUpdateAppSettingsItemAsync(icreateOrUpdateAppSettingsItemDto input)
        {
            blockQueue.Add(delegate()
            {
                dao.createOrUpdateAppSettingsItem(input);
            });
        }

        /// <summary>
        /// returns the Registry Settings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override ogetAppSettingsItemsDto getAppSettingsItems(igetAppSettingsItemsDto input)
        {
            ogetAppSettingsItemsDto res = new ogetAppSettingsItemsDto();
            if (input.syswfuser != 0 && input.bezeichnung != null && input.bezeichnung.Length > 0)
            {

                res.dtos = dao.deliverAppSettingsItems(input);

            }
            return res;
        }

        /// <summary>
        /// Returns one Registry Setting
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override RegVarDto getAppSettingsItem(igetAppSettingsItemsDto input)
        {
            RegVarDto item = null;
            RegVarDto[] list = dao.deliverAppSettingsItems(input);
            if (list != null && list.Count() > 0)
                item = list[0];

            return item;


        }

        /// <summary>
        /// Creates or updates the Registry Setting
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override RegVarDto createOrUpdateAppSettingsItem(icreateOrUpdateAppSettingsItemDto input)
        {
            RegVarDto item = null;
            RegVarDto[] list = dao.createOrUpdateAppSettingsItem(input);
            if (list != null && list.Count() > 0)
                item = list[0];

            return item;


        }

        /// <summary>
        /// Creates or updates multiple Settings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override RegVarDto[] createOrUpdateAppSettingsItems(icreateOrUpdateAppSettingsItemsDto input)
        {
            List<RegVarDto> rval = new List<RegVarDto>();

            if (input.regVars == null) return null;
            RegVarDto[] updateOnly = (from s in input.regVars
                                      where s.sysRegVar>0
                                      select s).ToArray();
            RegVarDto[] createOnly = (from s in input.regVars
                                      where s.sysRegVar <= 0
                                      select s).ToArray();

            rval.AddRange( ((AppSettingsDao)dao).updateAppSettingsItems(updateOnly));
            rval.AddRange(((AppSettingsDao)dao).createAppSettingsItems(input.sysWfuser, createOnly));


            return rval.ToArray();


        }
    }
}