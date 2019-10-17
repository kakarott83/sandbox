
namespace Cic.OpenOne.Util.Reflection
{
	[System.CLSCompliant(true)]
	public abstract class AdapterLoaderHelper<T> 
		where T : IAdapter
	{
		#region Private variables
		private System.Reflection.Assembly _ExecutingAssembly;
		private string _AppSettingsAdapterAssemblyFileNameKey;
		#endregion

		#region Constructors
		// TESTEDBY AdapterLoaderHelperTestFixture.ConstructWithoutExecutingAssembly
		// TESTEDBY AdapterLoaderHelperTestFixture.ConstructWithoutAppSettingsAdapterAssemblyFileNameKey
		// TESTEDBY AdapterLoaderHelperTestFixture.ConstructWithEmptyAppSettingsAdapterAssemblyFileNameKey
		// TESTEDBY AdapterLoaderHelperTestFixture.ConstructWithSpaceAppSettingsAdapterAssemblyFileNameKey
		// TESTEDBY AdapterLoaderHelperTestFixture.ConstructWithValidValues
		protected AdapterLoaderHelper(System.Reflection.Assembly executingAssembly, string appSettingsAdapterAssemblyFileNameKey)
		{
			// Check executing assembly
			if (executingAssembly == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("executingAssembly");
			}

			// Check key
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(appSettingsAdapterAssemblyFileNameKey))
			{
				// Throw exception
				throw new System.ArgumentNullException("appSettingsAdapterAssemblyFileNameKey");
			}

			// Set values
			_ExecutingAssembly = executingAssembly;
			_AppSettingsAdapterAssemblyFileNameKey = appSettingsAdapterAssemblyFileNameKey.Trim();
		}
		#endregion

		#region Methods
		// TESTEDBY AdapterLoaderHelperTestFixture.CheckLoadAdapter
		public T LoadAdapter()
		{
			try
			{
				// Internal
				return MyLoadAdapter(_AppSettingsAdapterAssemblyFileNameKey);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public T LoadAdapter(string appSettingsAdapterAssemblyFileNameSuffix)
		{
			// Check key
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(appSettingsAdapterAssemblyFileNameSuffix))
			{
				// Throw exception
				throw new System.ArgumentNullException("appSettingsAdapterAssemblyFileNameSuffix");
			}

			try
			{
				// Internal
				return MyLoadAdapter(_AppSettingsAdapterAssemblyFileNameKey + appSettingsAdapterAssemblyFileNameSuffix.Trim());
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region Properties
		public string AppSettingsAdapterAssemblyFileNameKey
		{
			get
			{
				// Return
				return _AppSettingsAdapterAssemblyFileNameKey;
			}
		}
		#endregion

		#region My methods
		private T MyLoadAdapter(string adapterAssemblyFileNameKey)
		{
            throw new System.Exception("Not yet implemented");
            /*
			Cic.Basic.Configuration.ExeConfiguration ExeConfiguration;
			T Adapter;
			string AssemblyExePath;
			string AdapterAssemblyFileName;
            string ThisAssemblyExePath;

			try
			{
				// Get exe path from assembly
				AssemblyExePath = Cic.Basic.Reflection.Assembly.AssemblyHelper.GetExePath(_ExecutingAssembly);
			}
			catch
			{
				// Throw new exception
				// TODO BK 0 BK, Localize text, Add exception class
				throw new System.ApplicationException("Unknown error.");
			}

			try
			{
				// New exe configuration
				ExeConfiguration = new Cic.Basic.Configuration.ExeConfiguration(AssemblyExePath);
			}
			catch (System.Exception ex)
			{
                throw ex;
			}

			try
			{
				// Get adapter assembly file name
				AdapterAssemblyFileName = ExeConfiguration.GetAppSettingsValue(adapterAssemblyFileNameKey);
			}
			
			catch (System.Exception ex)
			{
                throw ex;
			}

			// Check value
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(AdapterAssemblyFileName))
			{
				// Throw new exception
				throw new System.Exception("invalid assembly file name");
			}

            // TODO MK 0 BK, Whats with ..\
            // NOTE MK, Todo only because BK wanted it here. I wouldn't bother ;-)
            // Check file name
            if (AdapterAssemblyFileName.StartsWith(@".\", System.StringComparison.Ordinal))
            {
                try
                {
                    // Get exe path from assembly
                    ThisAssemblyExePath = AssemblyHelper.GetExePath(_ExecutingAssembly, false);
                }
                catch
                {
                    // Throw new exception
                    // TODO BK 0 BK, Localize text, Add exception class
                    throw new System.ApplicationException("Unknown error.");
                }

                // Check assembly exe path
                if (!ThisAssemblyExePath.EndsWith(@"\", System.StringComparison.Ordinal))
                {
                    // Add backslash
                    ThisAssemblyExePath = (ThisAssemblyExePath + @"\");
                }

                // Set new adapter assembly file name
                AdapterAssemblyFileName = AdapterAssemblyFileName.Replace(@".\", ThisAssemblyExePath);
            }

			try
			{
				// Create instance
				Adapter = (T)AssemblyHelper.CreateInstanceFromSpecifiedInterface(AdapterAssemblyFileName, typeof(T).ToString(), true);
			}
			catch (System.IO.FileNotFoundException ex)
			{
				// Throw
                //throw ex;
                throw ex;
				//// Throw new exception
				//// TODO BK 0 BK, Localize text, Add exception class
				//throw new System.ApplicationException("Adapter assembly file could not be found.", ex);
			}
			catch (System.IO.FileLoadException ex)
			{
				// Throw
				throw ex;
				//// Throw new exception
				//// TODO BK 0 BK, Localize text, Add exception class
				//throw new System.ApplicationException("Adapter assembly file could not be loaded.", ex);
			}
			catch (System.BadImageFormatException ex)
			{
				// Throw
				throw ex;
				//// Throw new exception
				//// TODO BK 0 BK, Localize text, Add exception class
				//throw new System.ApplicationException("Adapter assembly file has an invalid format.", ex);
			}
			catch (System.Exception ex)
			{
				// Throw new exception
				throw new System.Exception("adapter load failed",ex);
			}

			return Adapter;*/
		}
		#endregion
	}
}