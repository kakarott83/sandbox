using System;
using System.Collections.Generic;

using Microsoft.Exchange.WebServices.Data;
using Cic.One.Web.BO;
using Cic.One.Web.BO.Mail;
using Cic.One.DTO;

namespace Cic.One.Web.DAO.Mail
{
    public class SubscriptionWrapper : IDisposable
    {
        public const int Lifetime = 30;

        /// <summary>
        ///
        /// </summary>
        public ExchangeService Service { get; set; }

        /// <summary>
        /// Userdaten, welche für die Verbindung verwendet wurden
        /// </summary>
        public EWSUserDto UserDataDto { get; set; }

        /// <summary>
        /// FolderId von dem AKF/CRM-Ordner
        /// </summary>
        public FolderId CRMFolder { get; set; }

        /// <summary>
        /// Der aktuelle Status der Synchronisation
        /// </summary>
        public string SyncState { get; set; }

        /// <summary>
        /// Gibt an ob eine PullSubscription verwendet werden soll
        /// </summary>
        public bool IsPullsubscription { get; set; }

        /// <summary>
        /// Gibt an, ob der Wrapper initialisiert wurde
        /// </summary>
        public bool IsInitialized { get; set; }

        private StreamingSubscription streamedSubscription;
        private StreamingSubscriptionConnection streamedSubscriptionConnection;

        public event StreamingSubscriptionConnection.SubscriptionErrorDelegate OnDisconnect;

        public delegate void SubscriptionErrorDelegate(object sender, SubscriptionErrorEventArgs args);

        /// <summary>
        /// leerer Konstruktor
        /// </summary>
        public SubscriptionWrapper()
        {
        }

        /// <summary>
        /// Initialisiert den Wrapper
        /// </summary>
        /// <param name="UserDataDto">Userdaten</param>
        /// <param name="Service">Service-Verbindung</param>
        /// <param name="CRMFolder">CRM-Ordner</param>
        /// <param name="SyncState">SyncState</param>
        public void Initialize(EWSUserDto UserDataDto, ExchangeService Service, FolderId CRMFolder, string SyncState)
        {
            this.CRMFolder = CRMFolder;
            this.UserDataDto = UserDataDto;
            this.SyncState = SyncState;

            if (Service.RequestedServerVersion < ExchangeVersion.Exchange2010_SP1)
                IsPullsubscription = true;
            else
                this.Service = Service;

            TrySubscribe();
            IsInitialized = true;

            //CheckStatus();
        }

        /// <summary>
        /// Initialisiert den Wrapper
        /// </summary>
        /// <param name="UserDataDto">Userdaten</param>
        /// <param name="Service">Service-Verbindung</param>
        /// <param name="CRMFolder">CRM-Ordner</param>
        /// <param name="SyncState">SyncState</param>
        public SubscriptionWrapper(EWSUserDto UserDataDto, ExchangeService Service, FolderId CRMFolder, string SyncState)
        {
            Initialize(UserDataDto, Service, CRMFolder, SyncState);
        }

        /// <summary>
        /// Überprüft ob die Verbindung aktiv ist.
        /// Falls noch keine Verbindung da ist, wird synchronisiert und eine Verbindung erstellt.
        /// Falls die Verbindung geschlossen ist, wird synchronisiert und die Verbindung wieder gestartet.
        /// </summary>
        public void CheckStatus()
        {
            if (UserDataDto == null)
                return;

            if (IsPullsubscription)
            {
                SyncSaveItems();
            }
            else
            {
                if (streamedSubscriptionConnection == null)
                {
                    TrySubscribe();
                    SyncSaveItems();
                }
                else if (!streamedSubscriptionConnection.IsOpen)
                {
                    streamedSubscriptionConnection.Open();
                    SyncSaveItems();
                }
            }
        }

        /// <summary>
        /// Löscht alle Events, schließt die Verbindung und Disposed das Objekt
        /// </summary>
        public void Dispose()
        {
            if (streamedSubscriptionConnection != null)
            {
                streamedSubscriptionConnection.OnDisconnect -= streamedSubscriptionConnection_OnDisconnect;
                streamedSubscriptionConnection.OnNotificationEvent -= streamedSubscriptionConnection_OnNotificationEvent;
                streamedSubscriptionConnection.OnSubscriptionError -= streamedSubscriptionConnection_OnSubscriptionError;

                if (streamedSubscriptionConnection.IsOpen)
                    streamedSubscriptionConnection.Close();

                streamedSubscriptionConnection.Dispose();
            }
            if (streamedSubscription != null)
            {
                streamedSubscription.Unsubscribe();
            }

            streamedSubscriptionConnection = null;
            streamedSubscription = null;
            Service = null;
            UserDataDto = null;
            CRMFolder = null;
            SyncState = null;
        }

        /// <summary>
        /// Erstellt die StreamedSubscription
        /// </summary>
        /// <returns>true, falls erfolgreich</returns>
        private bool TrySubscribe()
        {
            if (!IsPullsubscription && streamedSubscriptionConnection == null)
            {
                streamedSubscription = Service.SubscribeToStreamingNotifications(new List<FolderId>() { CRMFolder }, EventType.Copied, EventType.Created, EventType.Deleted, EventType.Modified, EventType.Moved, EventType.NewMail, EventType.Status);

                streamedSubscriptionConnection = new StreamingSubscriptionConnection(Service, Math.Min(30, Math.Max(1, Lifetime)));
                streamedSubscriptionConnection.AddSubscription(streamedSubscription);
                streamedSubscriptionConnection.OnDisconnect += new StreamingSubscriptionConnection.SubscriptionErrorDelegate(streamedSubscriptionConnection_OnDisconnect);
                streamedSubscriptionConnection.OnNotificationEvent += new StreamingSubscriptionConnection.NotificationEventDelegate(streamedSubscriptionConnection_OnNotificationEvent);
                streamedSubscriptionConnection.OnSubscriptionError += new StreamingSubscriptionConnection.SubscriptionErrorDelegate(streamedSubscriptionConnection_OnSubscriptionError);

                streamedSubscriptionConnection.Open();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Wird aufgerufen, sobald ein Disconnect verursacht wurde.
        /// Wird nicht aufgerufen, falls die Verbindung per Programmcode geschlossen wird.
        /// Nun wird die Verbindung neu geöffnet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void streamedSubscriptionConnection_OnDisconnect(object sender, SubscriptionErrorEventArgs args)
        {
            if (OnDisconnect != null)
            {
                OnDisconnect(this, args);
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn ein Subscription Error entstanden ist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void streamedSubscriptionConnection_OnSubscriptionError(object sender, SubscriptionErrorEventArgs args)
        {
            Exception e = args.Exception;
            Console.WriteLine("\n-------------Error ---" + e.Message + "-------------");

            //TODO error
        }

        /// <summary>
        /// Wird aufgerufen, sobald Notifications vorhanden sind.
        /// Damit werden die Items synchronisiert und gespeichert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void streamedSubscriptionConnection_OnNotificationEvent(object sender, NotificationEventArgs args)
        {
            SyncSaveItems();
        }

        /// <summary>
        /// Synchronisiert Items ab einem bestimmten Status,
        /// speichert den neuen Status
        /// und speichert die Items in der Datenbank.
        /// </summary>
        public void SyncSaveItems()
        {
            lock (this)
            {
                IEntityMailBo bo = BOFactoryFactory.getInstance().getEntityMailBO(UserDataDto.SysWfuser);
                SyncState = bo.SyncSaveItems(UserDataDto, SyncState);
            }
        }
    }
}