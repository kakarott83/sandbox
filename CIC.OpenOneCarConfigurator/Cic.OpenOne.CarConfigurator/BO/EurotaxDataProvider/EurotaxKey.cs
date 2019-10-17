using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.EurotaxDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    class EurotaxKey
    {

        #region Properties
        public int VehicleType
        {
            private set;
            get;
        }

        public int BrandCode
        {
            private set;
            get;
        }

        public int GroupCode
        {
            set;
            get;
        }

        public int ModelCode
        {
            private set;
            get;
        }

        public String NatCode
        {
            private set;
            get;
        }

        public String Market
        {
            private set;
            get;
        }

        public String Language
        {
            private set;
            get;
        }

        public String Currency
        {
            private set;
            get;
        }

        public String ParentKey
        {
            set;
            get;
        }

        public string KeyText
        {
            get
            {
                string Text = this.VehicleType.ToString();

                if (this.BrandCode != 0)
                {
                    Text += ">" + this.BrandCode;
                    if (this.GroupCode != 0)
                    {
                        Text += ">" + this.GroupCode;
                        if (this.ModelCode != 0)
                        {
                            Text += ">" + this.ModelCode;
                            if (this.NatCode != string.Empty)
                                Text += ">" + this.NatCode;
                        }
                    }
                }

                return Text;
            }
        }

        public string ParentKeyText
        {
            get
            {
                string Text = string.Empty;

                if (this.BrandCode != 0)
                {
                    Text += this.VehicleType;
                    if (this.GroupCode != 0)
                    {
                        Text += ">" + this.BrandCode;
                        if (this.ModelCode != 0)
                        {
                            Text += ">" + this.GroupCode;
                            if (this.NatCode != string.Empty)
                                Text += ">" + this.ModelCode;
                        }
                    }
                }

                return Text;
            }
        }
        #endregion

        #region Constructors
        public EurotaxKey(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            String[] KeyArray;

            // Create an array with key's elements
            if (treeNode == null || String.IsNullOrEmpty(treeNode.Key))
            {
                KeyArray = new String[0];
                ParentKey = null;
            }
            else
            {
                KeyArray = treeNode.Key.Split(new Char[] { '>' });
                ParentKey = treeNode.Key;
            }

            // Parse the elements
            if (KeyArray.Length > 0)
                VehicleType = int.Parse(KeyArray[0]);
            else
                VehicleType = 0;

            if (KeyArray.Length > 1)
                BrandCode = int.Parse(KeyArray[1]);
            else
                BrandCode = 0;

            if (KeyArray.Length > 2)
                GroupCode = int.Parse(KeyArray[2]);
            else
                GroupCode = 0;

            if (KeyArray.Length > 3)
                ModelCode = int.Parse(KeyArray[3]);
            else
                ModelCode = 0;

            if (KeyArray.Length > 4)
                NatCode = KeyArray[4];
            else
                NatCode = "";

            // Get globalization info
            Language = EurotaxGlobalization.GetEurotaxLanguageCode(setting.SelectedLanguage);
            Currency = EurotaxGlobalization.GetEurotaxCurrencyCode(setting.SelectedCurrency);
            Market = EurotaxGlobalization.GetEurotaxMarketCode(setting.SelectedLanguage).ToUpper();
           
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            string Text = VehicleType.ToString();

            if (BrandCode != 0)
            {
                Text += ">" + BrandCode;

                if (GroupCode != 0)
                {
                    Text += ">" + GroupCode;
                    if (ModelCode != 0)
                        Text += ">" + ModelCode;
                }
            }

            return Text;
        }
        #endregion
    }
}
