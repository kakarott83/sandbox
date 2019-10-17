// OWNER MK, 30-09-2008
namespace Cic.OpenLease.ServiceAccess.Merge.Recorder
{
    [System.CLSCompliant(true)]
    public static class Factory
    {
        #region Methods
        public static Cic.OpenLease.ServiceAccess.Merge.Recorder.IRecorderService CreateRecorderService()
        {
            System.ServiceModel.Channels.Binding ServiceBinding;
            string ServiceUrl;
            System.ServiceModel.ChannelFactory<Cic.OpenLease.ServiceAccess.Merge.Recorder.IRecorderService> ChannelFactory;

            // Set the binding and url
            ServiceBinding = ServiceConfigurationHelper.GetBinding();
            ServiceUrl = ServiceConfigurationHelper.GetServicesUrl(null, @"Merge/RecorderService.svc");

            // Create channel facotry
            ChannelFactory = new System.ServiceModel.ChannelFactory<Cic.OpenLease.ServiceAccess.Merge.Recorder.IRecorderService>(ServiceBinding, ServiceUrl);

            // Open factory and creatche channel
            ChannelFactory.Open();
            return ChannelFactory.CreateChannel();
        }
        #endregion
    }
}
