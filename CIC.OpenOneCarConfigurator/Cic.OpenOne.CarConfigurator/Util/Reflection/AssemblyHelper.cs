
namespace Cic.OpenOne.Util.Reflection
{
	// PUBLISHED BK
	[System.CLSCompliant(true)]
	public static class AssemblyHelper
	{
		#region Private constants
		private const string CnstConfigFileSubNameReplace = ".user";
		private const string CnstExeConfigurationFileSuffix = "config";
		#endregion

		#region Methods
		// TEST BK 0 BK, Not tested
		public static string GetExePath(System.Reflection.Assembly assembly)
		{
			try
			{
				// Return
				return MyGetExePath(assembly, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static string GetExePath(System.Reflection.Assembly assembly, bool addScopeName)
		{
			try
			{
				// Return
				return MyGetExePath(assembly, addScopeName);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static string GetExeConfigurationFileName(System.Reflection.Assembly assembly)
		{
			try
			{
				// Return
				return (MyGetExePath(assembly, true) + "." + CnstExeConfigurationFileSuffix);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static System.IO.FileInfo GetExeConfigurationFileInfo(System.Reflection.Assembly assembly)
		{
			try
			{
				// Return
				return MyGetConfigurationFileInfo(assembly, null);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// TEST BK 0 BK, Not tested
		public static System.IO.FileInfo GetUserConfigurationFileInfo(System.Reflection.Assembly assembly, string replaceValueForUserConfig)
		{
			// Check replace value
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(replaceValueForUserConfig))
			{
				// Throw exception
				throw new System.ArgumentNullException("replaceValueForUserConfig");
			}

			try
			{
				// Return
				return MyGetConfigurationFileInfo(assembly, replaceValueForUserConfig);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// Note BK, The call of Assembly.LoadFrom is important, do not remove it unless you have another solution. Everthing is checked before loading.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
		// TEST BK 0 BK, Not tested
		public static object CreateInstanceFromSpecifiedInterface(string assemblyFile, string interfaceName, bool ignoreCaseOfInterfaceName)
		{
			System.Reflection.Assembly Assembly;
			System.Type[] TypeArray = null;
			System.Type TypeInterface = null;
			bool InterfaceFound = false;
			object Instance = null;

			// Check assembly file
			if (StringHelper.IsTrimedNullOrEmpty(assemblyFile))
			{
				// Throw new exception
				throw new System.ArgumentNullException("assemblyFile");
			}

			// Check interface name
			if (StringHelper.IsTrimedNullOrEmpty(interfaceName))
			{
				// Throw new exception
				throw new System.ArgumentNullException("interfaceName");
			}

			// Check existance
			if (!System.IO.File.Exists(assemblyFile))
			{
				// Throw exception
				// TODO BK 0 BK, Add exception class. !!! AdapterLoaderHelper !!!
				throw new System.IO.FileNotFoundException(new System.IO.FileNotFoundException().Message, assemblyFile);
			}

			try
			{
				// Load assembly from file
				Assembly = System.Reflection.Assembly.LoadFrom(assemblyFile);
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			try
			{
				// Get type array
				TypeArray = Assembly.GetTypes();
			}
			catch
			{
				// Ignore exception
			}

			// Check type array
			if ((TypeArray != null) && (TypeArray.GetLength(0) > 0))
			{
				// Loop through array
				foreach (System.Type LoopType in TypeArray)
				{
					// Check type, only public and not abstract
					if (LoopType.IsPublic && !LoopType.IsAbstract)
					{
						try
						{
							// Check wether the type derived from a specific interface
							TypeInterface = LoopType.GetInterface(interfaceName, ignoreCaseOfInterfaceName);
						}
						catch
						{
							// Ignore exception
						}

						// Check type interface
						if (TypeInterface != null)
						{
							// Set state
							InterfaceFound = true;
							// Create instance
							Instance = System.Activator.CreateInstance(LoopType);
							// Exit
							break;
						}
					}
				}
			}

			// Check instance
			if (Instance == null)
			{
				// Check state
				if (!InterfaceFound)
				{
					// TODO BK 0 BK, Localize text add exception class
					throw new System.ApplicationException("No type with specific interface found.");
				}
				else
				{
					// TODO BK 0 BK, Localize text add exception class
					throw new System.ApplicationException("Unable to create instance from type with specific interface.");
				}
			}

			// Return interface
			return Instance;
		}
		#endregion

		#region My methods
		// TEST BK 0 BK, Not tested
		private static string MyGetExePath(System.Reflection.Assembly assembly, bool addScopeName)
		{
			string CodeBase;
			string ScopeName;

			// Check assembly
			if (assembly == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("assembly");
			}

			// Get code base
			CodeBase = assembly.CodeBase;
			CodeBase = CodeBase.Replace("file:///", "");
			CodeBase = CodeBase.Replace("/", @"\");

			// Check state
			if (addScopeName)
			{
				// set scope name
				ScopeName = (@"\" + assembly.ManifestModule.ScopeName);
			}
			else
			{
				// set scope name
				ScopeName = string.Empty;
			}

			try
			{
				// Return
				return (System.IO.Path.GetDirectoryName(CodeBase) + ScopeName);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		private static System.IO.FileInfo MyGetConfigurationFileInfo(System.Reflection.Assembly assembly, string replaceValueForUserConfig)
		{
			string ExePath;

			try
			{
				// Get exe path from assembly
				ExePath = MyGetExePath(assembly, true);
				// Check replace value
				if (!Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(replaceValueForUserConfig))
				{
					// Replace
					ExePath = ExePath.Replace(replaceValueForUserConfig, CnstConfigFileSubNameReplace);
				}
				// Return
				return new System.IO.FileInfo(ExePath + "." + CnstExeConfigurationFileSuffix);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion
	}
}
