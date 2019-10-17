using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Notification Gateway Data Acces Object
    /// </summary>
    public class NotificationGatewayDbDao : INotificationGatewayDbDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Auslesen von Versanddaten einer Benachrichtigung
        /// </summary>
        /// <param name="NotificationID">Primary ID der Veranddaten auf EAIHOT</param>
        /// <param name="NotificationSettings">Settings for the Notification Gateway</param>
        /// <returns>Daten</returns>
        public EaiNotificationDataDto getNotificationDaten(int NotificationID, Dictionary<NotifySettings, String> NotificationSettings)
        {
            EaiNotificationDataDto RetVal = new EaiNotificationDataDto();
            RetVal.Attachments = new List<EaiNotificationAttachmentsDto>();
            RetVal.Recipients = new List<string>();
            RetVal.BlindCarbonCopyRecipients = new List<string>();
            RetVal.CarbonCopyRecipients = new List<string>();
            RetVal.XTags = new Dictionary<string, string>();
            _log.Info("Starting Serverdata readout.");
            try
            {
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended olCtx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {
                    _log.Debug("Created OlEntities context");
                    RetVal.SYSEAIHOT = NotificationID;
                    EaihotDto eaiHot = olCtx.ExecuteStoreQuery<EaihotDto>("select * from eaihot where syseaihot=" + NotificationID, null).FirstOrDefault();
                    if(eaiHot==null)
                    {
                        _log.Warn("No EAIHOT found for syseaihot="+NotificationID);
                        return RetVal;
                    }
                    List<EaihfileDto> eaiHFiles = null;
                    try
                    {
                        eaiHFiles = olCtx.ExecuteStoreQuery<EaihfileDto>("select * from eaihfile where syseaihot=" + eaiHot.SYSEAIHOT, null).ToList(); 
                    }catch(Exception e)
                    {
                        _log.Warn("No Files for EAIHOT="+eaiHot.SYSEAIHOT+" found: "+e.Message);
                        eaiHFiles = new List<EaihfileDto>();
                    }

                   

                   
                    switch (eaiHot.CODE)
                    {
                        case "SMS":
                            SetSMSData(eaiHot, eaiHFiles, RetVal);
                            return RetVal;
                        case "MAIL":
                            List<EAIQIN> eaiQin = null;
                            try { 
                                eaiQin = (from inp in olCtx.EAIQIN where inp.EAIHOT.SYSEAIHOT == eaiHot.SYSEAIHOT select inp).ToList();
                            }
                            catch (Exception e)
                            {
                                _log.Warn("No Inputparameters for EAIHOT=" + eaiHot.SYSEAIHOT + " found: " + e.Message);
                                eaiQin = new List<Cic.OpenOne.Common.Model.DdOw.EAIQIN>();
                            }
                            SetMailData(eaiHot, eaiHFiles,eaiQin, RetVal);
                            return RetVal;
                        case "FAX":
                            SetFaxData(eaiHot, eaiHFiles, RetVal, NotificationSettings[NotifySettings.FaxAdresse]);
                            return RetVal;
                        default:
                            throw (new Exception("Kein Notification Type in der gegebenen EAIHOTID!"));
                    }
                }
            }
            catch (Exception Exp)
            {
                _log.Error("Error at reading Server Data! Exception: " + Exp);
                throw (new Exception("Fehler beim Auslesen der Notification Daten von der Datenbank!", Exp));
            }
        }



        /// <summary>
        /// Prepares Data for SMS-Sending
        /// </summary>
        /// <param name="eaiHot">EAIHOT Dataset</param>
        /// <param name="eaiHFiles">EAIHFILES AttachmentDatasets</param>
        /// <param name="RetVal">Notification Dataset as returnvalue</param>
        private void SetSMSData(EaihotDto eaiHot, List<EaihfileDto> eaiHFiles, EaiNotificationDataDto RetVal)
        {
            RetVal.Type = BO.NotificationType.SMS;
            RetVal.Text = eaiHot.EVALEXPRESSION;
            RetVal.Sender = eaiHot.INPUTPARAMETER4;
            string RecipientList = eaiHot.INPUTPARAMETER1;
            if (RecipientList != null && RecipientList.Length > 0)
            {
                foreach (string Recipient in RecipientList.Split(';'))
                {
                    RetVal.Recipients.Add(Recipient);
                }
            }

            RetVal.Attachments = new List<EaiNotificationAttachmentsDto>();
            foreach (EaihfileDto file in eaiHFiles)
            {
                EaiNotificationAttachmentsDto NewEntry = new EaiNotificationAttachmentsDto();
                NewEntry.Filename = file.TARGETFILENAME;
                NewEntry.MIME = file.TARGETPATHSPEC;
                NewEntry.Data = file.EAIHFILE;
                RetVal.Attachments.Add(NewEntry);
            }
        }

        /// <summary>
        /// Prepares Data for E-Mail-Sending
        /// </summary>
        /// <param name="eaiHot">EAIHOT Dataset</param>
        /// <param name="eaiHFiles">EAIHFILES AttachmentDatasets</param>
        /// <param name="eaiQin">EAIQIN AttachmentDatasets</param>
        /// <param name="RetVal">Notification Dataset as returnvalue</param>
        private void SetMailData(EaihotDto eaiHot, List<EaihfileDto> eaiHFiles, List<EAIQIN> eaiQin, EaiNotificationDataDto RetVal)
        {
            RetVal.Type = BO.NotificationType.EMail;
            RetVal.Subject = eaiHot.INPUTPARAMETER5;
            RetVal.Text = eaiHot.EVALEXPRESSION;

            Dictionary<string, string> variables = eaiQin.ToDictionary(a => a.F02, a => a.F03);
            RetVal.Variables = variables;

            string CCList = eaiHot.INPUTPARAMETER2;
            if (CCList != null && CCList.Length > 0)
            {
                foreach (string Recipient in CCList.Split(';'))
                {
                    RetVal.CarbonCopyRecipients.Add(Recipient);
                }
            }

            string BCCList = eaiHot.INPUTPARAMETER3;
            if (BCCList != null && BCCList.Length > 0)
            {
                foreach (string Recipient in BCCList.Split(';'))
                {
                    RetVal.BlindCarbonCopyRecipients.Add(Recipient);
                }
            }
            RetVal.Sender = eaiHot.INPUTPARAMETER4;
            string RecipientList = eaiHot.INPUTPARAMETER1;
            if (RecipientList != null && RecipientList.Length > 0)
            {
                foreach (string Recipient in RecipientList.Split(';'))
                {
                    RetVal.Recipients.Add(Recipient);
                }
            }
            RetVal.Attachments = new List<EaiNotificationAttachmentsDto>();
            foreach (EaihfileDto file in eaiHFiles)
            {
                EaiNotificationAttachmentsDto NewEntry = new EaiNotificationAttachmentsDto();
                NewEntry.Filename = file.TARGETFILENAME;
                NewEntry.MIME = file.TARGETPATHSPEC;
                NewEntry.Data = file.EAIHFILE;
                RetVal.Attachments.Add(NewEntry);
            }
        }


        /// <summary>
        /// Prepares Data for FAX-Sending
        /// </summary>
        /// <param name="eaiHot">EAIHOT Dataset</param>
        /// <param name="eaiHFiles">EAIHFILES AttachmentDatasets</param>
        /// <param name="RetVal">Notification Dataset as returnvalue</param>
        /// <param name="SenderDomain">Sender Domain</param>
        private void SetFaxData(EaihotDto eaiHot, List<EaihfileDto> eaiHFiles, EaiNotificationDataDto RetVal, string SenderDomain)
        {
            RetVal.Type = BO.NotificationType.Fax;
            if (SenderDomain == null || SenderDomain.Length == 0)
                throw new Exception("Keine Domäne angegeben!");
            if (eaiHot.INPUTPARAMETER4.Contains('|'))
            {
                String[] AbsenderSplit = eaiHot.INPUTPARAMETER4.Split('|');
                RetVal.Sender = AbsenderSplit[1];
                RetVal.XTags.Add("X-FaxTSI", AbsenderSplit[0]);
            }
            else
            {
                if (eaiHot.INPUTPARAMETER4.Contains('@'))
                {
                    RetVal.Sender = eaiHot.INPUTPARAMETER4;
                }
                else
                {
                    RetVal.XTags.Add("X-FaxTSI", eaiHot.INPUTPARAMETER4);
                    if (SenderDomain[0] == '@')
                        RetVal.Sender = eaiHot.INPUTPARAMETER4 + SenderDomain;
                    else
                        RetVal.Sender = eaiHot.INPUTPARAMETER4 + "@" + SenderDomain;
                }
            }
            if (eaiHot.INPUTPARAMETER5 != null)
            {
                RetVal.XTags.Add("X-FaxTAGLINE", eaiHot.INPUTPARAMETER5);
            }
            else
            {
                RetVal.XTags.Add("X-FaxTAGLINE", RetVal.XTags["X-FaxTSI"]);
            }
            // Optional: Fax sending at a preset time
            if (eaiHot.INPUTPARAMETER3 != null && eaiHot.INPUTPARAMETER3.Length != 0)
            {
                RetVal.XTags.Add("X-FaxTime", eaiHot.INPUTPARAMETER3);
            }
            // Optional: Fax sending with Notification
            if (eaiHot.INPUTPARAMETER2 != null && eaiHot.INPUTPARAMETER2.Length != 0)
            {
                RetVal.XTags.Add("X-FaxNotify", eaiHot.INPUTPARAMETER2);
            }
            RetVal.XTags.Add("X-FaxIDENTIFY", eaiHot.INPUTPARAMETER1);
            if (eaiHot.INPUTPARAMETER1 != null && eaiHot.INPUTPARAMETER1.Length > 0)
            {
                string[] Identifications = eaiHot.INPUTPARAMETER1.Split(',');
                RetVal.Recipients.Add(Identifications[0]);
            }
            RetVal.Subject = "Fax-Versand:" + eaiHot.INPUTPARAMETER1;
            RetVal.Attachments = new List<EaiNotificationAttachmentsDto>();
            foreach (EaihfileDto file in eaiHFiles)
            {
                EaiNotificationAttachmentsDto NewEntry = new EaiNotificationAttachmentsDto();
                NewEntry.Filename = file.TARGETFILENAME;
                NewEntry.MIME = file.TARGETPATHSPEC;
                NewEntry.Data = file.EAIHFILE;
                RetVal.Attachments.Add(NewEntry);
            }
        }

        /// <summary>
        /// Setzen des Rückgabewerts für Erfolg in der EAIHOT
        /// </summary>
        /// <param name="NotificationID">Primary key</param>
        /// <param name="ReturnCode">Rückgabewert</param>
        public void setNotifcationReturnValues(int NotificationID, int ReturnCode)
        {
            _log.Info("Starting Serverdata readout.");
            try
            {
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended olCtx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {
                    _log.Debug("Created OlEntities context");
                    Cic.OpenOne.Common.Model.DdOw.EAIHOT eaiHot = (from noti in olCtx.EAIHOT where noti.SYSEAIHOT == NotificationID select noti).FirstOrDefault();
                    eaiHot.PROZESSSTATUS = ReturnCode;
                    eaiHot.COMPUTERNAME = System.Environment.MachineName;

                    olCtx.SaveChanges();
                }
            }
            catch (Exception Exp)
            {
                _log.Error("Error at storing Returnvalues in DB Data! Exception: " + Exp);
                throw (new Exception("Fehler beim Speichern der Rückmeldung auf der Datenbank!", Exp));
            }
        }
    }
}