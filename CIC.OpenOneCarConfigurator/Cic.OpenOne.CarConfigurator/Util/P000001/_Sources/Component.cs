// OWNER: BK, 15-04-2008
using Cic.OpenOne.Util;
namespace Cic.P000001.Common
{
	[System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
	public sealed class Component : Cic.P000001.Common.IComponent, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
        
		private bool _Parameterless=true;
		private string _Key;
		private Cic.P000001.Common.ComponentTypeConstants _ComponentTypeConstant;
		private string _Category;
		private string _DisplayName;
		private Cic.P000001.Common.Parameter[] _Parameters;
		private bool _HasDetails;
		private bool _HasPictures;
		private double _Price;
		private double _NewPrice;
		private Cic.P000001.Common.Component[] _Components;
        private bool _Selectable;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY ComponentTestFixture.CheckProperties
		public Component()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY ComponentTestFixture.ConstructWithoutKey
		// TESTEDBY ComponentTestFixture.ConstructWithEmptyKey
		// TESTEDBY ComponentTestFixture.ConstructWithSpaceKey
		// TESTEDBY ComponentTestFixture.ConstructWithInvalidComponentTypeConstant
		// TESTEDBY ComponentTestFixture.ConstructWithoutCategory
		// TESTEDBY ComponentTestFixture.ConstructWithEmptyCategory
		// TESTEDBY ComponentTestFixture.ConstructWithSpaceCategory
		// TESTEDBY ComponentTestFixture.ConstructWithoutDisplayName
		// TESTEDBY ComponentTestFixture.ConstructWithEmptyDisplayName
		// TESTEDBY ComponentTestFixture.ConstructWithSpaceDisplayName
		// TESTEDBY ComponentTestFixture.ConstructCheckComponentsMustBeNullIfComponentTypeIsNotPackage
		// TESTEDBY ComponentTestFixture.CheckProperties
		public Component(string key, Cic.P000001.Common.ComponentTypeConstants componentTypeConstant, string category, string displayName, Cic.P000001.Common.Parameter[] parameters, bool hasDetails, bool hasPictures, double price, double newPrice, Cic.P000001.Common.Component[] components, bool selectable)
        {
            // Check key
            if (StringHelper.IsTrimedNullOrEmpty(key))
            {
                // Throw exception
                throw  new System.ArgumentException("key");
            }

            // Check component type constant
            if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.ComponentTypeConstants), componentTypeConstant))
            {
                // Throw exception
                throw  new System.ArgumentException("componentTypeConstant");
            }

            // Check category
            if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(category))
            {
                // Throw exception
                throw  new System.ArgumentException("category");
            }

            // Check display name
            if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(displayName))
            {
                // Throw exception
                throw  new System.ArgumentException("displayName");
            }

            // Not pack but with components?
            if ((componentTypeConstant != Cic.P000001.Common.ComponentTypeConstants.Package) && components != null)
            {
                // Throw exception
                throw new System.ArgumentException("components");
            }

            // Set values
            _Key = key.Trim();
            _ComponentTypeConstant = componentTypeConstant;
            _Category = category.Trim();
            _DisplayName = displayName;
            _Parameters = parameters;
            _HasDetails = hasDetails;
            _HasPictures = hasPictures;
            _Price = price;
			_NewPrice = newPrice;
            _Components = components;
            _Selectable = selectable;
        }
		#endregion

        #region IInputData methods
        public void CheckProperties()
        {
            try
            {
                // TODO MK 0 MK, Check
            }
            // TODO BK 0 BK, catch specific exceptions
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        #endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.Component")]
		// TESTEDBY ComponentTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct parameters
					if ((_Parameters != null) && (_Parameters.GetLength(0) > 0))
					{
						// Loop through parameters
						foreach (Cic.P000001.Common.Parameter LoopParameter in _Parameters)
						{
							// Check object
							if (LoopParameter != null)
							{
								// Reconstruct
								LoopParameter.Reconstruct();
							}
						}
					}
					// Reconstruct components
					if ((_Components != null) && (_Components.GetLength(0) > 0))
					{
						// Loop through components
						foreach (Cic.P000001.Common.Component LoopComponent in _Components)
						{
							// Check object
							if (LoopComponent != null)
							{
								// Reconstruct
								LoopComponent.Reconstruct();
							}
						}
					}

					// Create new instance
					new Cic.P000001.Common.Component(_Key, _ComponentTypeConstant, _Category, _DisplayName, _Parameters, _HasDetails, _HasPictures, _Price, _NewPrice, _Components, _Selectable);
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

		#region IComponentBase properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Key
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _Key;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Key = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.ComponentTypeConstants ComponentTypeConstant
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _ComponentTypeConstant;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ComponentTypeConstant = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Category
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _Category;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Category = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string DisplayName
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _DisplayName;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_DisplayName = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool HasDetails
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _HasDetails;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_HasDetails = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool HasPictures
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _HasPictures;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_HasPictures = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public double Price
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _Price;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Price = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public double NewPrice
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _NewPrice;
			}
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// NOTE BK, This setting needs no parameterless check 
				// Check value
				if (value < 0.0)
				{
					// Throw exception
					throw  new System.ArgumentException("value");
				}
				// Set value
				_NewPrice = value;
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public bool Selectable
        {
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
            {
                return _Selectable;
            }
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
            {
                if (!_Parameterless)
                {
                    _Selectable = value;
                }
            }
        }
		#endregion

		#region IComponent properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Parameter[] Parameters
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _Parameters;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Parameters = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Component[] Components
		{
			// TESTEDBY ComponentTestFixture.CheckProperties
			get
			{
				// Return
				return _Components;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Components = value;
				}
			}
		}
		#endregion
	}
}
