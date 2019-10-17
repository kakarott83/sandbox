// OWNER: BK, 06-06-2008
namespace Cic.P000001.Common.ConfigurationManager
{
	#region Using directives
	
	#endregion

	[System.Serializable]
	[System.CLSCompliant(true)]
	public sealed class ConfigurationComponent : Cic.P000001.Common.ConfigurationManager.IConfigurationComponent, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private Cic.P000001.Common.Component _Component;
		private Cic.P000001.Common.ComponentDetail[] _ComponentDetails;
		private Cic.P000001.Common.Picture[] _Pictures;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
		public ConfigurationComponent()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY ConfigurationComponentTestFixture.ConstructWithoutComponent
		// TESTEDBY ConfigurationComponentTestFixture.ConstructWithInvalidComponentDetails
		// TESTEDBY ConfigurationComponentTestFixture.ConstructWithInvalidPictures
		// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
		public ConfigurationComponent(Cic.P000001.Common.Component component, Cic.P000001.Common.ComponentDetail[] componentDetails, Cic.P000001.Common.Picture[] pictures)
		{
			int Index;

			// Check component
			if (component == null)
			{
				// Throw exception
				throw new System.ArgumentException("component");
			}

			// Check component details
			if ((componentDetails != null) && (componentDetails.GetLength(0) > 0))
			{
				// Reset index
				Index = -1;
				// Loop through component details
				foreach (Cic.P000001.Common.ComponentDetail LoopComponentDetail in componentDetails)
				{
					// Increase index
					Index += 1;
					// Check component detail
					if (LoopComponentDetail == null)
					{
						// Throw exception
						throw new System.ArgumentException("componentDetails[" + Index.ToString() + "]");
					}
				}
			}

			// Check pictures
			if ((pictures != null) && (pictures.GetLength(0) > 0))
			{
				// Reset index
				Index = -1;
				// Loop through pictures
				foreach (Cic.P000001.Common.Picture LoopPicture in pictures)
				{
					// Increase index
					Index += 1;
					// Check picture
					if (LoopPicture == null)
					{
						// Throw exception
						throw new System.ArgumentException("pictures[" + Index.ToString() + "]");
					}
				}
			}

			// Set values
			_Component = component;
			_ComponentDetails = componentDetails;
			_Pictures = pictures;
		}
		#endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.ConfigurationManager.ConfigurationComponent")]
		// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct component
					if (_Component != null)
					{
						// Reconstruct
						_Component.Reconstruct();
					}
					// Reconstruct component details
					if ((_ComponentDetails != null) && (_ComponentDetails.GetLength(0) > 0))
					{
						// Loop through component details
						foreach (Cic.P000001.Common.ComponentDetail LoopComponentDetail in _ComponentDetails)
						{
							// Check object
							if (LoopComponentDetail != null)
							{
								// Reconstruct
								LoopComponentDetail.Reconstruct();
							}
						}
					}
					// Reconstruct pictures
					if ((_Pictures != null) && (_Pictures.GetLength(0) > 0))
					{
						// Loop through component details
						foreach (Cic.P000001.Common.Picture LoopPicture in _Pictures)
						{
							// Check object
							if (LoopPicture != null)
							{
								// Reconstruct
								LoopPicture.Reconstruct();
							}
						}
					}

					// Create new instance
					new Cic.P000001.Common.ConfigurationManager.ConfigurationComponent(_Component, _ComponentDetails, _Pictures);
				}
				catch
				{
					// Throw caught exception
					throw;
				}

				// Reset state
				_Parameterless = false;
			}
		}
		#endregion

		#region IConfigurationComponent properties
		public Cic.P000001.Common.Component Component
		{
			// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
			get { return _Component; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Component = value;
				}
			}
		}

		public Cic.P000001.Common.ComponentDetail[] ComponentDetails
		{
			// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
			get { return _ComponentDetails; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ComponentDetails = value;
				}
			}
		}

		public Cic.P000001.Common.Picture[] Pictures
		{
			// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
			get { return _Pictures; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Pictures = value;
				}
			}
		}
		#endregion
	}
}
