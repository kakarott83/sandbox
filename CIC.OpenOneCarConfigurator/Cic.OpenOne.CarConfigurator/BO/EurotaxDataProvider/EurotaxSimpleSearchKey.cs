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

    class EurotaxSimpleSearchKey
    {

        #region Properties
        public int NatCode
        {
            private set;
            get;
        }

        public int ModCd
        {
            private set;
            get;
        }

        public int ModLevOne
        {
            private set;
            get;
        }

        public int MakCd
        {
            private set;
            get;
        }

        public int VehType
        {
            private set;
            get;
        }

        public String Language
        {
            private set;
            get;
        }

        public String Market
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
            private set;
            get;
        }
        #endregion

        #region Constructors
        public EurotaxSimpleSearchKey(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
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
            if (KeyArray.Length > 1)
            {
                ModCd = int.Parse(KeyArray[0]);
                NatCode = int.Parse(KeyArray[1]);
            }
            else
            {
                ModCd = 0;
                NatCode = 0;
            }
            if (KeyArray.Length > 3)
            {
                ModLevOne = int.Parse(KeyArray[1]);
                MakCd = int.Parse(KeyArray[0]);
            }
            else
            {
                ModLevOne = 0;
                MakCd = 0;
            }
            if (KeyArray.Length > 4)
                VehType = int.Parse(KeyArray[2]);
            else
                VehType = 0;

            

            // Get globalization info
            Language = EurotaxGlobalization.GetEurotaxLanguageCode(setting.SelectedLanguage);
            Currency = EurotaxGlobalization.GetEurotaxCurrencyCode(setting.SelectedCurrency);
            Market = EurotaxGlobalization.GetEurotaxMarketCode(setting.SelectedLanguage);

        }
        #endregion

        #region Methods
        //public override string ToString()
        //{
        //    String Text = VehicleType.ToString();

        //    if (BrandCode != 0)
        //    {
        //        Text += ">" + BrandCode;

        //        if (GroupCode != 0)
        //        {
        //            Text += ">" + GroupCode;
        //            if (ModelCode != 0)
        //                Text += ">" + ModelCode;
        //        }
        //    }

        //    return Text;
        //}
        #endregion
    }
}
