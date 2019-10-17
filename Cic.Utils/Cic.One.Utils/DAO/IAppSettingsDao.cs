using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.Common.DAO
{
    public interface IAppSettingsDao
    {

        /// <summary>
        /// SettingsItems zurückgeben
        /// </summary>
        /// <returns>array ofAppRegisterDtos</returns>
        RegVarDto[] deliverAppSettingsItems(igetAppSettingsItemsDto input);

        /// <summary>
        /// SettingsItems speichern
        /// </summary>
        /// <returns>array ofAppRegisterDtos</returns>
        RegVarDto[] createOrUpdateAppSettingsItem(icreateOrUpdateAppSettingsItemDto input);

        /// <summary>
        /// SettingsItems speichern
        /// </summary>
        /// <returns>array ofAppRegisterDtos</returns>
        RegVarDto[] createOrUpdateAppSettingsItems(icreateOrUpdateAppSettingsItemsDto input);
    
      
    
    }
}