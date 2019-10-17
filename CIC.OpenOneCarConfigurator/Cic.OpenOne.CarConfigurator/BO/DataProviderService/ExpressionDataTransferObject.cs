// OWNER: BK, 10-07-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// Expression Data Transfer Object
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public sealed class ExpressionDataTransferObject
	{
		#region Private variables
		private string _Key;
		private Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants _TypeConstant;
		private string _Value;
		private string _ChildKey1;
		private string _ChildKey2;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public ExpressionDataTransferObject()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="typeConstant"></param>
        /// <param name="value"></param>
        /// <param name="childKey1"></param>
        /// <param name="childKey2"></param>
        /// <todo>BK 0 BK, Not tested</todo>
		internal ExpressionDataTransferObject(string key, Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants typeConstant, string value, string childKey1, string childKey2)
		{
			// Check key
			if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(key))
			{
				// Throw exception
				throw new System.ArgumentNullException("key");
			}
			// Check type constant
			if (!System.Enum.IsDefined(typeof(Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants), typeConstant))
			{
				// Throw exception
				throw new System.ArgumentOutOfRangeException("typeConstant");
			}
			// Check type constant
			if (typeConstant == Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants.Component)
			{
				// Check value
				if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(value))
				{
					// Throw exception
					throw new System.ArgumentNullException("value");
				}
				// Check child key1
				if (childKey1 != null)
				{
					// Throw exception
					throw new System.ArgumentException("must be null: childKey1");
				}
				// Check child key2
				if (childKey2 != null)
				{
					// Throw exception
					throw new System.ArgumentException("must be null: childKey2");
				}
			}
			else
			{
				// Check value
				if (value != null)
				{
					// Throw exception
					throw new System.ArgumentException("must be null: value");
				}
				// Check child key1
				if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(childKey1))
				{
					// Throw exception
					throw new System.ArgumentNullException("childKey1");
				}
				// Check child key2
				if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(childKey2))
				{
					// Throw exception
					throw new System.ArgumentNullException("childKey2");
				}
			}

			// Set values
			_Key = key;
			_TypeConstant = typeConstant;
			_Value = value;
			_ChildKey1 = childKey1;
			_ChildKey2 = childKey2;
		}
		#endregion

		#region Properties
        /// <summary>
        /// Key
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Key
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _Key;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_Key = value;
			}
		}

        /// <summary>
        /// Type Constant
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants TypeConstant
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _TypeConstant;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_TypeConstant = value;
			}
		}

        /// <summary>
        /// Value
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Value
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _Value;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_Value = value;
			}
		}

        /// <summary>
        /// Child Key 1
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string ChildKey1
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _ChildKey1;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_ChildKey1 = value;
			}
		}

        /// <summary>
        /// Child Key 2
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string ChildKey2
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _ChildKey2;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_ChildKey2 = value;
			}
		}
		#endregion
	}
}
