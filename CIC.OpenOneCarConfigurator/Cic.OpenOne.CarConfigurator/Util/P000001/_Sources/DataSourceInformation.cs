// OWNER: BK, 15-04-2008
using Cic.OpenOne.Util.Reflection;
using Cic.OpenOne.Util;
namespace Cic.P000001.Common
{
	#region Using directives
	
	#endregion
	
	[System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
	public sealed class DataSourceInformation : Cic.P000001.Common.IDataSourceInformation, IReconstructable, System.IComparable
	{
		#region Private variables
		private bool _Parameterless=true;
		private System.Guid _Identifier;
		private string _Version;
		private string _VersionDescription;
		private string _Designation;
		private string _Description;
		private string _Copyright;
		private string[] _AvailableLanguages;
		private string[] _AvailableCurrencies;
		private bool _StandardEquipmentDetailsSupported;
		private bool _TechnicalDataDetailsSupported;
		private bool _ExteriorPicturesSupported;
		private bool _InteriorPicturesSupported;
		private Cic.P000001.Common.PriceInclusionConstants _PriceInclusionConstant;
		private string _ValueAddedTaxInfo;
		private string _SpecialCarTaxInfo;
		private string _ImportDutyInfo;
		private string _ShippingCostInfo;
		private string _PriceInfo;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY DataSourceInformationTestFixture.CheckProperties
		public DataSourceInformation()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY DataSourceInformationTestFixture.ConstructWithoutVersion
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithEmptyVersion
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithSpaceVersion
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithoutDesignation
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithEmptyDesignation
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithSpaceDesignation
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithNullEntryAvailableLanguages
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithInvalidEntryAvailableLanguages
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithNullEntryAvailableCurrencies
		// TESTEDBY DataSourceInformationTestFixture.ConstructWithInvalidEntryAvailableCurrencies
		// TODO BK 0 BK, Add test: DataSourceInformationTestFixture.ConstructWithInvalidPriceInclusionConstant
		// TESTEDBY DataSourceInformationTestFixture.CheckProperties
		public DataSourceInformation(System.Guid identifier, string version, string versionDescription, string designation, string description, string copyright, string[] availableLanguages, string[] availableCurrencies, bool standardEquipmentDetailsSupported, bool technicalDataDetailsSupported, bool exteriorPicturesSupported, bool interiorPicturesSupported, Cic.P000001.Common.PriceInclusionConstants priceInclusionConstant, string valueAddedTaxInfo, string specialCarTaxInfo, string importDutyInfo, string shippingCostInfo, string priceInfo)
		{
			long Index;

			// Check version
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(version))
			{
				// Throw exception
				throw  new System.ArgumentException("version");
			}

			// Check designation
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(designation))
			{
				// Throw exception
				throw  new System.ArgumentException("designation");
			}

			// Check available languages
			if ((availableLanguages != null) && (availableLanguages.GetLength(0) > 0))
			{
				// Reset index
				Index = -1;
				// Loop through array
				foreach (string LoopAvailableLanguage in availableLanguages)
				{
					// Increase index
					Index += 1;
					// Check name
					if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(LoopAvailableLanguage))
					{
						// Throw exception
						throw  new System.ArgumentException("availableLanguages[" + Index.ToString() + "]");
					}

					// Validate specific culture name
					if (!CultureHelper.ValidateRFC4646CultureName(LoopAvailableLanguage))
					{
						// Throw exception
                        throw new System.ArgumentException("availableLanguages[" + Index.ToString() + "]");
					}
				}
			}

			// Check available currencies
			if ((availableCurrencies != null) && (availableCurrencies.GetLength(0) > 0))
			{
				// Reset index
				Index = -1;
				// Loop through array
				foreach (string LoopAvailableCurrency in availableCurrencies)
				{
					// Increase index
					Index += 1;
					// Check name
					if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(LoopAvailableCurrency))
					{
						// Throw exception
						throw  new System.ArgumentException("availableCurrencies[" + Index.ToString() + "]");
					}

					// Validate currency name
					if (!CultureHelper.ValidateISO4217CurrencyName(LoopAvailableCurrency))
					{
						// Throw exception
                        throw new System.ArgumentException("availableCurrencies[" + Index.ToString() + "]");
					}
				}
			}

			// Check price inclusion constant
			// TODO BK 0 BK, Check bitsum
			//if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.PriceInclusionConstants), priceInclusionConstant))
			//{
			//    // Throw exception
			//    throw  new System.ArgumentException("priceInclusionConstant");
			//}

			// Set values
			_Identifier = identifier;
			_Version = version.Trim();
			_VersionDescription = versionDescription;
			_Designation = designation.Trim();
			_Description = description;
			_Copyright = copyright;
			_AvailableLanguages = availableLanguages;
			_AvailableCurrencies = availableCurrencies;
			_StandardEquipmentDetailsSupported = standardEquipmentDetailsSupported;
			_TechnicalDataDetailsSupported = technicalDataDetailsSupported;
			_ExteriorPicturesSupported = exteriorPicturesSupported;
			_InteriorPicturesSupported = interiorPicturesSupported;
			_PriceInclusionConstant = priceInclusionConstant;
			_ValueAddedTaxInfo = valueAddedTaxInfo;
			_SpecialCarTaxInfo = specialCarTaxInfo;
			_ImportDutyInfo = importDutyInfo;
			_ShippingCostInfo = shippingCostInfo;
			_PriceInfo = priceInfo;
		}
		#endregion

		#region IComparable methods
		// TESTEDBY DataSourceInformationTestFixture.CheckCompareToWithoutValue
		// TESTEDBY DataSourceInformationTestFixture.CheckCompareToWithInvalidObjectType
		// TESTEDBY DataSourceInformationTestFixture.CheckCompareToWithInvalidIdentifier
		// TESTEDBY DataSourceInformationTestFixture.CheckCompareTo
		public int CompareTo(object obj)
		{
			Cic.P000001.Common.DataSourceInformation DataSourceInformation = null;

			// Check value
			if (obj == null)
			{
				// Throw exception
				throw new System.ArgumentException("obj");
			}

			// Convert to data source information
			DataSourceInformation = (Cic.P000001.Common.DataSourceInformation)obj;

			// Check identifier
			if (DataSourceInformation.Identifier != _Identifier)
			{
				// Return
				// NOTE BK, Other returns
				return string.Compare(_Identifier.ToString(), DataSourceInformation.Identifier.ToString(), System.StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				// Return
				// NOTE BK, Other returns
				return string.Compare(_Version, DataSourceInformation.Version, System.StringComparison.OrdinalIgnoreCase);
			}
		}

		// TODO BK 0 BK, Not tested
		public override int GetHashCode()
		{
			// Return
			// TODO BK 1 BK, Check hash code generating
			return (_Identifier.GetHashCode() + _Version.GetHashCode());
		}

		// TODO BK 0 BK, Not tested
		public bool Equals(Cic.P000001.Common.DataSourceInformation other)
		{
			bool Equal = false;

			// Check object
			if (other != null)
			{
				// Check properties
				Equal = (this.CompareTo(other) == 0);
			}

			// Return
			return Equal;
		}

		// TODO BK 0 BK, Not tested
		public override bool Equals(object obj)
		{
			bool Equal = false;

			// Check object type
			if ((obj != null ) && (obj.GetType() == typeof(Cic.P000001.Common.DataSourceInformation)))
			{
				// Set value
				Equal = this.Equals((Cic.P000001.Common.DataSourceInformation)obj);
			}

			// Return
			return Equal;
		}
		#endregion

		#region IComparable operators
		// TODO BK 0 BK, Not tested
		public static bool operator ==(Cic.P000001.Common.DataSourceInformation dataSourceInformation1, Cic.P000001.Common.DataSourceInformation dataSourceInformation2)
		{
			bool Result = false;

			try
			{
				// Get result
				Result = dataSourceInformation1.Equals(dataSourceInformation2);
			}
			catch
			{
				// Ignore exeption
			}

			return Result;
		}

		// TODO BK 0 BK, Not tested
		public static bool operator !=(Cic.P000001.Common.DataSourceInformation dataSourceInformation1, Cic.P000001.Common.DataSourceInformation dataSourceInformation2)
		{
			return !(dataSourceInformation1 == dataSourceInformation2);
		}

		// TODO BK 0 BK, Not tested
		public static bool operator <(Cic.P000001.Common.DataSourceInformation dataSourceInformation1, Cic.P000001.Common.DataSourceInformation dataSourceInformation2)
		{
			return ((dataSourceInformation1 != null) && (dataSourceInformation2 != null) && (dataSourceInformation1.CompareTo(dataSourceInformation2) < 0));
		}

		// TODO BK 0 BK, Not tested
		public static bool operator >(Cic.P000001.Common.DataSourceInformation dataSourceInformation1, Cic.P000001.Common.DataSourceInformation dataSourceInformation2)
		{
			return ((dataSourceInformation1 != null) && (dataSourceInformation2 != null) && (dataSourceInformation1.CompareTo(dataSourceInformation2) > 0));
		}
		#endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.DataSourceInformation")]
		// TESTEDBY DataSourceInformationTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Create new instance
					new Cic.P000001.Common.DataSourceInformation(_Identifier, _Version, _VersionDescription, _Designation, _Description, _Copyright, _AvailableLanguages, _AvailableCurrencies, _StandardEquipmentDetailsSupported, _TechnicalDataDetailsSupported, _ExteriorPicturesSupported, _InteriorPicturesSupported, _PriceInclusionConstant, _ValueAddedTaxInfo, _SpecialCarTaxInfo, _ImportDutyInfo, _ShippingCostInfo, _PriceInfo);
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

		#region Methods
		// TESTEDBY DataSourceInformationTestFixture.CheckVersionEquality
		public bool CheckVersionEquality(object value)
		{
			bool Result = false;

			try
			{
				// Compare
				Result = (this.CompareTo(value) == 0);
			}
			catch
			{
				// Ignore exception
				// Reset value
				Result = false;
			}

			// Return
			return Result;
		}
		#endregion

		#region IDataSourceInformationBase properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public System.Guid Identifier
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _Identifier;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Identifier = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Designation
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _Designation;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Designation = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Description
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _Description;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Description = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Version
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _Version;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Version = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string VersionDescription
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _VersionDescription;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_VersionDescription = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Copyright
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _Copyright;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Copyright = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string[] AvailableLanguages
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _AvailableLanguages;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_AvailableLanguages = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string[] AvailableCurrencies
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _AvailableCurrencies;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_AvailableCurrencies = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool StandardEquipmentDetailsSupported
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _StandardEquipmentDetailsSupported;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_StandardEquipmentDetailsSupported = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool TechnicalDataDetailsSupported
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _TechnicalDataDetailsSupported;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_TechnicalDataDetailsSupported = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool ExteriorPicturesSupported
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _ExteriorPicturesSupported;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ExteriorPicturesSupported = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool InteriorPicturesSupported
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _InteriorPicturesSupported;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_InteriorPicturesSupported = value;
				}
			}
		}
[System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.PriceInclusionConstants PriceInclusionConstant
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _PriceInclusionConstant;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_PriceInclusionConstant = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string ValueAddedTaxInfo
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _ValueAddedTaxInfo;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ValueAddedTaxInfo = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string SpecialCarTaxInfo
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _SpecialCarTaxInfo;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_SpecialCarTaxInfo = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string ImportDutyInfo
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _ImportDutyInfo;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ImportDutyInfo = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string ShippingCostInfo
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _ShippingCostInfo;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ShippingCostInfo = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string PriceInfo
		{
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			get
			{
				// Return
				return _PriceInfo;
			}
			// NOTE BK, For serialization
			// TESTEDBY DataSourceInformationTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_PriceInfo = value;
				}
			}
		}
		#endregion
	}
}
