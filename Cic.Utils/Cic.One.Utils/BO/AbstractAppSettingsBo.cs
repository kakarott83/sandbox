using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    public abstract class AbstractAppSettingsBo : IAppSettingsBo
    {
        protected IAppSettingsDao dao;

        public AbstractAppSettingsBo(IAppSettingsDao dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// inserts a new entry into the recent list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sysWfUser"></param>
        public abstract void insertRecent(EntityDto item, long sysWfUser);

        /// <summary>
        /// Get List of Settings - Elements für User
        /// </summary>
        /// <param name="igetXproItems"></param>
         /// <returns>List of AppRegisterDto</returns>
        public abstract ogetAppSettingsItemsDto getAppSettingsItems(igetAppSettingsItemsDto input);

        /// <summary>
        /// Gets XproEntity for xprocode and entityid
        /// </summary>
        /// <param name="igetXproItem"></param>
        /// <returns>AppRegisterDto</returns>
        public abstract RegVarDto getAppSettingsItem(igetAppSettingsItemsDto input);

        /// <summary>
        /// inserts app setting asynchronously
        /// </summary>
        /// <param name="input"></param>
        public abstract void createOrUpdateAppSettingsItemAsync(icreateOrUpdateAppSettingsItemDto input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract RegVarDto createOrUpdateAppSettingsItem(icreateOrUpdateAppSettingsItemDto input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract RegVarDto[] createOrUpdateAppSettingsItems(icreateOrUpdateAppSettingsItemsDto input);
    }


 }
