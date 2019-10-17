// OWNER: BK, 15-04-2008
using System.Collections.Generic;
namespace Cic.P000001.Common
{
	[System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
	public sealed class Setting : Cic.P000001.Common.ISetting, Cic.OpenOne.Util.Reflection.IReconstructable
	{
        /// <summary>
        /// 100 = BMW
        /// 200 = BankNOW
        /// 300 = Hyundai - HEK Enabled in obviewDao
        /// </summary>
        public int customerCode { get; set; }//used to determine the mode of operation, used for bmw
        public long sysperole { get; set; }
		#region Private variables
        
		private bool _Parameterless=true;
		private string _SelectedLanguage;
		private string _SelectedCurrency;
        private List<string> showonly = null;
        private List<string> hide = null;
        [System.Runtime.Serialization.DataMember(IsRequired = false)]
        public TypedSearchParam[] SearchParams
        {
            get;
            set;
        }
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY SettingTestFixture.CheckProperties
		public Setting()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY SettingTestFixture.ConstructWithInvalidSelectedLanguage
		// TESTEDBY SettingTestFixture.ConstructWithInvalidSelectedCurrency
		// TESTEDBY SettingTestFixture.CheckProperties
        public Setting(Setting setting):this(setting.SelectedLanguage, setting.SelectedCurrency)
        {
            if (setting.showonly!=null)
            {
                showonly = new List<string>();
                showonly = setting.showonly;
            }
            if (setting.hide != null)
            {
                hide = new List<string>();
                hide = setting.hide;
            }
            if(setting.SearchParams!=null)
            {
                this.SearchParams = new TypedSearchParam[setting.SearchParams.Length];
                int i = 0;
                foreach (TypedSearchParam tp in setting.SearchParams)
                {
                    this.SearchParams[i++] = new TypedSearchParam(tp);
                }
            }
        }
		public Setting(string selectedLanguage, string selectedCurrency)
		{
			// Check selected language
			if (selectedLanguage != null)
			{
				// Validate specific culture name
				if (!Cic.OpenOne.Util.CultureHelper.ValidateRFC4646CultureName(selectedLanguage))
				{
					// Throw exception
                    throw new System.ArgumentException("selectedLanguage");
				}
			}

			// Check selected currency
			if (selectedCurrency != null)
			{
				// Validate currency name
				if (!Cic.OpenOne.Util.CultureHelper.ValidateISO4217CurrencyName(selectedCurrency))
				{
					// Throw exception
					throw new System.ArgumentException("selectedCurrency");
				}
			}

			// Set values
			_SelectedLanguage = selectedLanguage;
			_SelectedCurrency = selectedCurrency;
		}
		#endregion


        

        #region IInputData methods
        public void CheckProperties()
        {
            try
            {
                // TODO MK 0 MK, check
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.Setting")]
		// TESTEDBY SettingTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Create new instance
					new Cic.P000001.Common.Setting(_SelectedLanguage, _SelectedCurrency);
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

		#region ISettingBase properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string SelectedLanguage
		{
			// TESTEDBY SettingTestFixture.CheckProperties
			get
			{
				// Return
				return _SelectedLanguage;
			}
			// NOTE BK, For serialization
			// TESTEDBY SettingTestFixture.CheckProperties
			set
			{
				// Check state
                if (!_Parameterless)
				{
					// Set value
					_SelectedLanguage = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string SelectedCurrency
		{
			// TESTEDBY SettingTestFixture.CheckProperties
			get
			{
				// Return
				return _SelectedCurrency;
			}
			// NOTE BK, For serialization
			// TESTEDBY SettingTestFixture.CheckProperties
			set
			{
				// Check state
                if (!_Parameterless)
				{
					// Set value
					_SelectedCurrency = value;
				}
			}
		}

        public void addShowOnly(string str)
        {
            if (showonly == null) showonly = new List<string>();
            showonly.Add(str);
        }
        public void addHide(string str)
        {
            if (hide == null) hide = new List<string>();
            hide.Add(str);
        }
        public bool hasFilter()
        {
            return !(hide == null && showonly == null);
        }
        /// <summary>
        /// returns true if filterString has not to be shown (==filtered)
        /// </summary>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public bool filter(string filterString)
        {
            if (filterString == null) return false;

            filterString = filterString.ToLower();
            if (showonly != null)
            {
               
                foreach (string s in showonly)
                {
                    if (filterString.IndexOf(s) > -1)
                        return false;
                    
                }
                    return true;
               
            }
            else if (hide != null)
            {
                foreach (string s in hide)
                {
                    if (filterString.IndexOf(s) > -1)
                        return true;//filter the item
                   
                }
            }
            return false;
        }
		#endregion
	}
}
