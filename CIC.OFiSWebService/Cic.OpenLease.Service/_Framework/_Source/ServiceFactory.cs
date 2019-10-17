// OWNER BK, 27-07-2009
namespace Cic.OpenLease.Service
{
	[System.CLSCompliant(true)]
	public class ServiceFactory : System.ServiceModel.Activation.ServiceHostFactory
	{
		#region Methods
		protected override System.ServiceModel.ServiceHost CreateServiceHost(System.Type serviceType, System.Uri[] baseAddresses)
		{
			Cic.OpenLease.Service.ServiceHost ServiceHost;

			ServiceHost = new Cic.OpenLease.Service.ServiceHost(serviceType, baseAddresses);

			return ServiceHost;
		}
		#endregion
	}
}


