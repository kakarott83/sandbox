using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO;


namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Data Access Object for Notification Gateway
    /// </summary>
    public interface INotificationGatewayDbDao
    {

        /// <summary>
        /// Auslesen von Versanddaten einer Benachrichtigung
        /// </summary>
        /// <param name="NotificationID">Primary ID der Veranddaten auf EAIHOT</param>
        /// <param name="NotificationSettings">Settings for the Notification Gateway</param>
        /// <returns>Daten</returns>
        EaiNotificationDataDto getNotificationDaten(int NotificationID, Dictionary <NotifySettings,String> NotificationSettings);

        /// <summary>
        /// Speichern des Rückgabewertes des Aktuellen Versands via DB-Eintrag
        /// </summary>
        /// <param name="NotificationID"></param>
        /// <param name="ReturnCode"></param>
        void setNotifcationReturnValues(int NotificationID, int ReturnCode);
    }
}
