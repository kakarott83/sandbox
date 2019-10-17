
using System;
namespace Cic.P000001.Common
{
    

    public interface IKeyInfo
    {
        String getParentKey();
        String getKey();
    }

	[System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
	public sealed class TreeNode : Cic.P000001.Common.ITreeNode, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
        
		private bool _Parameterless=true;
		private string _Key;
		private string _ParentKey;
		private Cic.P000001.Common.Level _Level;
		private string _DisplayName;
		private Cic.P000001.Common.Parameter[] _Parameters;
		private bool _IsType;
		private bool _HasDetails;
		private bool _HasPictures;
		private double _Price;
		private double _NewPrice;
        private string _CODE;
        private string _FILTERDATA;
        private ObViewDto _data { get; set; }
		#endregion

		#region Constructors
		
		public TreeNode()
		{
			// Set state
			_Parameterless = false;
		}

        public TreeNode(ObViewDto data, int level, IKeyInfo key)
        {
            _HasPictures = false;
            _HasDetails = false;
            _Level = new Level(level);
            _NewPrice = data.neupreisbrutto;
            _Price = data.neupreisbrutto;
            _IsType = (level == 4) || data.level==4;
            _ParentKey = key.getParentKey();
            if (level == 0)
                _Key = data.id;
            else if (data.path != null)
            {
                _Key = data.path;
            }
            else
            {
                _Key = key.getKey() + ">" + data.id;
            }
            _DisplayName = data.bezeichnung;
            this._data = data;

           
        }
	
		public TreeNode(string key, string parentKey, Cic.P000001.Common.Level level, string displayName, Cic.P000001.Common.Parameter[] parameters, bool isType, bool hasDetails, bool hasPictures, double price, double newPrice)
		{
			// Check key
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(key))
			{
				// Throw exception
				throw  new System.ArgumentException("key");
			}

			// Check level
			if (level == null)
			{
				// Throw exception
				throw new System.ArgumentException("level");
			}

			// Check display name
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(displayName))
			{
				// Throw exception
				throw  new System.ArgumentException("displayName");
			}

			// Check level number
			if (level.Number > 0)
			{
				// Check key
				if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(parentKey))
				{
					// Throw exception
					throw  new System.ArgumentException("parentKey");
				}
			}
			else
			{
				// Check parent key
				if (parentKey != null)
				{
					// Throw exception
					throw new System.ArgumentException("parentKey");
				}
			}

			// Set values
			_Key = key;
			_ParentKey = parentKey;
			_Level = level;
			_DisplayName = displayName.Trim();
			_Parameters = parameters;
			_IsType = isType;
			_HasDetails = hasDetails;
			_HasPictures = hasPictures;
			_Price = price;
			_NewPrice = newPrice;
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
		// NOTE BK, Essential for reconstruction
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.TreeNode")]
		// TESTEDBY TreeNodeTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct level
					if (_Level != null)
					{
						// Reconstruct
						_Level.Reconstruct();
					}
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

					// Create new instance
					new Cic.P000001.Common.TreeNode(_Key, _ParentKey, _Level, _DisplayName, _Parameters, _IsType, _HasDetails, _HasPictures, _Price, _NewPrice);
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

		#region ITreeNodeBase properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Key
		{
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _Key;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
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
		public string ParentKey
		{
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _ParentKey;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ParentKey = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string DisplayName
		{
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _DisplayName;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
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
		public bool IsType
		{
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _IsType;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_IsType = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool HasDetails
		{
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _HasDetails;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
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
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _HasPictures;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
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
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _Price;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
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
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _NewPrice;
			}
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			set
			{
				// NOTE BK, This setting needs no parameterless check 
				// Check value
				if (value < 0.0)
				{
					// TODO BK 0 BK, Not testes
					// Throw exception
					throw  new System.ArgumentException("value");
				}
				// Set value
				_NewPrice = value;
			}
		}
		#endregion

		#region ITreeNode properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Level Level
		{
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _Level;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Level = value;
				}
			}
		}
       
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public ObViewDto data
        {
         
            get
            {

                return _data;
            }
           
            set
            {
                // Check state
                if (!_Parameterless)
                {
                    // Set value
                    _data = value;
                }
            }
        }
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Parameter[] Parameters
		{
			// TESTEDBY TreeNodeTestFixture.CheckProperties
			get
			{
				// Return
				return _Parameters;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeTestFixture.CheckProperties
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
        public string CODE
        {
         
            get
            {
                
                return _CODE;
            }
             set
            {
                
                _CODE = value;
            }
        }
        /// <summary>
        /// Contains user invisible data needed for filtering
        /// </summary>
        public string FILTERDATA
        {

            get
            {

                return _FILTERDATA;
            }
            set
            {

                _FILTERDATA = value;
            }
        }
		#endregion
	}
}
