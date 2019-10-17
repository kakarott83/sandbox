// OWNER BK, 27-07-2009
namespace Cic.OpenLease.Service
{
	[System.CLSCompliant(true)]
	public class ServiceHost : System.ServiceModel.ServiceHost
	{
		#region Constants
		private const string CnstBaseHostName = "BaseHostName";
		#endregion

		#region Constructors
		public ServiceHost(System.Type serviceType, params System.Uri[] baseAddresses)
			: base(serviceType, MyGetBaseAddresses(baseAddresses))
		{
		}
		#endregion

		#region Methods
		protected override void ApplyConfiguration()
		{
			base.ApplyConfiguration();
		}
		#endregion

		#region My methods
		private static System.Uri[] MyGetBaseAddresses(System.Uri[] baseAddresses)
		{
			System.Collections.Generic.List<System.Uri> addresses = new System.Collections.Generic.List<System.Uri>();

			MyAddBaseAddress(baseAddresses, addresses, CnstBaseHostName);

			return addresses.ToArray();
		}

		private static void MyAddBaseAddress(System.Uri[] baseAddresses, System.Collections.Generic.List<System.Uri> addresses, string key)
		{
			string Address = null;
			System.Uri BaseAddress = null;

			// Check base Addresse
			if ((baseAddresses != null) && (baseAddresses.GetLength(0) > 0))
			{
				// Get base address
				BaseAddress = baseAddresses[0];

				try
				{
					// read base addresses from AppSettings in config    
					Address = System.Configuration.ConfigurationManager.AppSettings[key];
				}
				catch
				{
					// Ignore exception
				}

				if (null != Address)
				{
					addresses.Add(new System.Uri(Address + BaseAddress.AbsolutePath));
				}
				else
				{
					addresses.Add(BaseAddress);
				}
			}
		}
		#endregion
	}
}
