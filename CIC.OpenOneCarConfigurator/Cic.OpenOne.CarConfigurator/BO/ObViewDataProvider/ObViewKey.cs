using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.ObViewDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Cic.P000001.Common;
    #endregion

    class ObViewKey : IKeyInfo
    {
        public Cic.P000001.Common.Setting setting { get; set; }

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

        public ObViewKey getPreviousLevelKey()
        {
            //parent - 2
            //key - 2
            ObViewKey rval = new ObViewKey(this);
            rval.setFields(rval.ParentKey, 2);
            rval.ParentKey = rval.ToString();
            return rval;
        }
        private ObViewKey(ObViewKey orgKey)
        {
            setting = orgKey.setting;
            ParentKey = orgKey.ParentKey;
            setFields(ParentKey, 0);
        }

        public ObViewKey(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            this.setting = setting;


            // Create an array with key's elements
            if (treeNode == null || String.IsNullOrEmpty(treeNode.Key))
            {

                ParentKey = null;
            }
            else
            {

                ParentKey = treeNode.Key;
            }

            VehicleType = 0;
            BrandCode = 0;
            GroupCode = 0;
            ModelCode = 0;
            NatCode = "";
            if(treeNode!=null)
                setFields(treeNode.Key, 0);


        }


        private void setFields(String key, int reduce)
        {
            String[] KeyArray = new String[0];
            if(key!=null && key.Length>0)
                KeyArray = key.Split(new Char[] { '>' });

            VehicleType = 0;
            BrandCode = 0;
            GroupCode = 0;
            ModelCode = 0;
            NatCode = "";
            for (int i = 0; i < KeyArray.Length-reduce; i++)
            {
                if(i==0)
                    VehicleType = int.Parse(KeyArray[i]);
                else if (i == 1)
                    BrandCode = int.Parse(KeyArray[i]);
                else if (i == 2)
                    GroupCode = int.Parse(KeyArray[i]);
                else if (i == 3)
                    ModelCode = int.Parse(KeyArray[i]);
                else if (i == 4)
                    NatCode = KeyArray[i];
            }
          
        }

      

        /*
        
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
       */
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


        public string getParentKey()
        {
            /*string Text = VehicleType.ToString();

            if (BrandCode != 0 && GroupCode != 0)
            {
                Text += ">" + BrandCode;

                if (GroupCode != 0 && ModelCode != 0)
                {
                    Text += ">" + GroupCode;
                    if (ModelCode != 0 && NatCode.Length > 0)
                        Text += ">" + ModelCode;
                }
            }

            return Text;*/
            return ParentKey;
        }

        public string getKey()
        {
            return ToString();
        }
    }
}
