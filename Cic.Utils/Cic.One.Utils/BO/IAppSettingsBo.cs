using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    public interface IAppSettingsBo
    {
        /// <summary>
        /// inserts a new entry into the recent list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sysWfUser"></param>
        void insertRecent(EntityDto item, long sysWfUser);

        /// <summary>
        /// Get List of Settings - Elements für User
        /// </summary>
        /// <param name="igetXproItems"></param>
        /// <returns>List of Xproentites</returns>
        ogetAppSettingsItemsDto getAppSettingsItems(igetAppSettingsItemsDto input);

        /// <summary>
        /// Gets XproEntity for xprocode and entityid
        /// </summary>
        /// <param name="igetXproItem"></param>
        /// <returns>XproEntity</returns>
        RegVarDto getAppSettingsItem(igetAppSettingsItemsDto input);

        /// <summary>
        /// inserts app setting asynchronously
        /// </summary>
        /// <param name="input"></param>
        void createOrUpdateAppSettingsItemAsync(icreateOrUpdateAppSettingsItemDto input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        RegVarDto createOrUpdateAppSettingsItem(icreateOrUpdateAppSettingsItemDto input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        RegVarDto[] createOrUpdateAppSettingsItems(icreateOrUpdateAppSettingsItemsDto input);
    }
}