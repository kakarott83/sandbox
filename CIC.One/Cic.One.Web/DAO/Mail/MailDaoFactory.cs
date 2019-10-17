using Cic.OpenOne.Common.Util.Collection;
using System;
using System.Timers;
using System.Threading.Tasks;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.One.Web.BO;
using Cic.One.DTO;

namespace Cic.One.Web.DAO.Mail
{
    public class MailDaoFactory
    {

        private static long CACHE_TIMEOUT = 1000 * 60 * 30;//30 min
        private static volatile MailDaoFactory _self = null;
        private static readonly object InstanceLocker = new Object();
        private static CacheDictionary<long, SubscriptionWrapper> subscriptionConnections = CacheFactory<long, SubscriptionWrapper>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instanz der MailDao Factory erzeugen
        /// </summary>
        /// <returns></returns>
        public static MailDaoFactory getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new MailDaoFactory();
                }
            }
            return _self;
        }

        private Timer timer;

        private void StartSubscriptionTimer()
        {
            if (timer != null && timer.Enabled)
                return;

            if (timer == null)
            {
                
                int intervallSeconds = Convert.ToInt32(EWSDBDao.GetFromWebconfig("ExchangeSyncIntervallSeconds"));
                timer = new Timer(intervallSeconds * 1000);
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            }
            timer.Enabled = true;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (subscriptionConnections.Count == 0)
                {
                    timer.Enabled = false;
                }
                else
                {
                    foreach (SubscriptionWrapper wrapper in subscriptionConnections.Values)
                    {
                        if (wrapper.IsPullsubscription)
                            wrapper.CheckStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                 _log.Error("SyncMail - Timer Tick: ",ex);
            }
        }

        public IMailDao getMailDao(SubscriptionWrapper subscription, IMailDBDao mailDBDao = null)
        {
            return new EWSDao(subscription, getMailDBDao(subscription.UserDataDto.SysWfuser, mailDBDao), getTraceListenerDao());
        }

        public IMailDao getMailDao(EWSUserDto userDataDto, long syswfuser, IMailDBDao mailDBDao = null)
        {
            if (userDataDto == null)
                return new EWSDao(userDataDto, getMailDBDao(syswfuser, mailDBDao), getTraceListenerDao()); ;
            return new EWSDao(userDataDto, getMailDBDao(userDataDto.SysWfuser, mailDBDao), getTraceListenerDao());
        }

        public IMailDao getMailDao(long syswfuser)
        {
            //if (subscriptionConnections.ContainsKey(userid))
            //{
            //    return getMailDao(subscriptionConnections[userid]);
            //}

            var mailDBDao = getMailDBDao(syswfuser);
            var userDataDto = mailDBDao.GetUser(syswfuser);
            return getMailDao(userDataDto, syswfuser);
        }

        public ITraceListenerDao getTraceListenerDao()
        {
            return new DebugTraceListenerDao();
        }

        public IMailDBDao getMailDBDao(long syswfuser, IMailDBDao mailDBDao = null)
        {
            if (mailDBDao == null)
                return new EWSDBDao(DAOFactoryFactory.getInstance().getEntityDao(), BOFactoryFactory.getInstance().getAppSettingsBO()) { SysWfuser = syswfuser };
            else
                return mailDBDao;
        }

     
        public void CheckCreateSubscriptionAsync(long syswfuser)
        {
            Task.Factory.StartNew(()=>
                {
                    var mailDBDao = getMailDBDao(syswfuser);
                    var userDataDto = mailDBDao.GetUser(syswfuser);
                    return CheckCreateSubscription(syswfuser, userDataDto, mailDBDao);
                })
                //Error Logging
                .ContinueWith((a)=> _log.Error("CheckCreateSubscriptionAsync failed for Wfuser "+syswfuser+": ",a.Exception),TaskContinuationOptions.OnlyOnFaulted);
        }


        public SubscriptionWrapper CheckCreateSubscription(long syswfuser, EWSUserDto userDataDto, IMailDBDao mailDBDao = null)
        {
            if (userDataDto == null)
            {
                //Kein User -> Exchange ausgeschaltet
                return null;
            }

            SubscriptionWrapper foundConnection = null;
            StartSubscriptionTimer();

            if (!subscriptionConnections.ContainsKey(syswfuser))
            {
                lock (this)
                {
                    if (!subscriptionConnections.ContainsKey(syswfuser))
                    {
                        subscriptionConnections.Add(syswfuser, new SubscriptionWrapper());
                    }
                    foundConnection = subscriptionConnections[syswfuser];
                }
            }
            else
                foundConnection = subscriptionConnections[syswfuser];

            lock (foundConnection)
            {
                if (!foundConnection.IsInitialized)
                {
                    EWSDao ewsDao = getMailDao(userDataDto, userDataDto.SysWfuser, mailDBDao) as EWSDao;
                    mailDBDao = getMailDBDao(userDataDto.SysWfuser, mailDBDao);

                    foundConnection.Initialize(
                            userDataDto,
                            ewsDao.Service,
                            ewsDao.CRMFolder,
                            mailDBDao.GetSyncState(userDataDto)
                            );
                }
                foundConnection.CheckStatus();
            }

            return foundConnection;
        }
    }
}