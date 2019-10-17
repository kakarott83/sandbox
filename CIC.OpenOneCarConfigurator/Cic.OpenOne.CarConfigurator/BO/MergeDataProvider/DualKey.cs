using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.MergeDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    
    #endregion

    internal class DualKey
    {
        #region Properties
        public string PrimaryKey
        {
            get;
            private set;
        }

        public string SecondaryKey
        {
            get;
            private set;
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this.PrimaryKey) && string.IsNullOrEmpty(this.SecondaryKey);
            }
        }
        #endregion

        #region Constructors
        public DualKey(string compositeKey)
        {
            MyParse(compositeKey, this);
        }

        public DualKey(string primaryKey, string secondaryKey)
        {
            this.PrimaryKey = primaryKey;
            this.SecondaryKey = secondaryKey;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return "[" + this.PrimaryKey + "][" + this.SecondaryKey + "]";
        }
        #endregion

        #region My methods
        private void MyParse(string compositeKey, DualKey destinationKey)
        {
            // Check if composite key is not empty
            if (string.IsNullOrEmpty(compositeKey))
            {
                this.PrimaryKey = null;
                this.SecondaryKey = null;
                return;
            }

            // Split the composite key into several keys wrapped with brackets
            MatchCollection Keys = Regex.Matches(compositeKey, "\\[([^\\]]*)\\]");
            
            // Check if there are exactly two keys
            if (Keys.Count != 2)
            {
                // Throw an exception
                throw new Exception("The composite key is invalid.");
            }

            this.PrimaryKey = Keys[0].Value.Replace("[", string.Empty).Replace("]", string.Empty);
            this.SecondaryKey = Keys[1].Value.Replace("[", string.Empty).Replace("]", string.Empty);
        }
        #endregion
    }
}
