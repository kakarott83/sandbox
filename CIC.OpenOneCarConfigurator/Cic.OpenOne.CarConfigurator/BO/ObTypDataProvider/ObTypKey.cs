using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.ObTypDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
  
    #endregion

    internal class ObTypKey
    {
        #region Private constants
        private const string CnstKeySeparator = ">";
        #endregion

        #region Properties
        public long VehicleTypeId
        {
            get;
            private set;
        }

        public long BrandId
        {
            get;
            private set;
        }

        public long ModelGroupId
        {
            get;
            private set;
        }

        public long ModelId
        {
            get;
            private set;
        }

        public long TypeId
        {
            get;
            private set;
        }

        public ObTypKey Parent
        {
            get
            {
                return MyGetParent(this);
            }
        }

        public long LastValidId
        {
            get
            {
                return MyGetLastValidId();
            }
        }
        #endregion

        #region Constructors
        private ObTypKey()
        {
        }

        public ObTypKey(string key, long nextId)
        {
            // Construct the key
            string Key = string.IsNullOrEmpty(key) ? nextId.ToString() : key + CnstKeySeparator + nextId;

            // Parse the key
            MyParse(this, Key);
        }

        public ObTypKey(ObTypKey parent, long nextId)
        {
            // Construct the key
            string Key = parent == null ? nextId.ToString() : parent + CnstKeySeparator + nextId;

            // Parse the key
            MyParse(this, Key);
        }

        public ObTypKey(long vehicleTypeId, long brandId, long modelGroupId, long modelId, long typeId)
        {
            // Map the properties
            this.VehicleTypeId = vehicleTypeId;
            this.BrandId = brandId;
            this.ModelGroupId = modelGroupId;
            this.ModelId = modelId;
            this.TypeId = typeId;
        }

        public ObTypKey(string key)
        {
            // Parse the key
            MyParse(this, key);
        }
        #endregion

        #region Methods
        public static ObTypKey Parse(string key)
        {
            // Create ObTypKey
            ObTypKey Result = new ObTypKey();

            // Parse
            MyParse(Result, key);

            // Return ObTypKey
            return Result;
        }

        public override string ToString()
        {
            // Set the prefix
            string Result = string.Empty;

            // Check if vehicle type exists
            if (this.VehicleTypeId > 0)
            {
                // Append the vehicle type
                Result += this.VehicleTypeId;

                // Check if brand exists
                if (this.BrandId > 0)
                {
                    // Append the brand
                    Result += CnstKeySeparator + this.BrandId;

                    // Check if model group exists
                    if (this.ModelGroupId > 0)
                    {
                        // Append the model group
                        Result += CnstKeySeparator + this.ModelGroupId;

                        // Check if model exists
                        if (this.ModelId > 0)
                        {
                            // Append the model
                            Result += CnstKeySeparator + this.ModelId;

                            // Check if type exists
                            if (this.TypeId > 0)
                            {
                                // Append the type
                                Result += CnstKeySeparator + this.TypeId;
                            }
                        }
                    }
                }
            }

            // Return the string
            return Result;
        }
        #endregion

        #region My methods
        private static void MyParse(ObTypKey obTypKey, string key)
        {
            if (obTypKey == null)
            {
                throw new ArgumentNullException("obTypKey", "ObTypKey is null.");
            }

            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key is null or empty.", "key");
            }

            // Split the key
            string[] KeyElements = key.Split(new string[] { CnstKeySeparator }, StringSplitOptions.None);

            // Set the default values
            long VehicleTypeId = 0;
            long BrandId = 0;
            long ModelGroupId = 0;
            long ModelId = 0;
            long TypeId = 0;

            // Check if the key contains any elements
            if (KeyElements.Length > 0)
            {
                // Try to parse the vehicle type
                if (long.TryParse(KeyElements[0], out VehicleTypeId) && KeyElements.Length > 1)
                {
                    // Try to parse the brand
                    if (long.TryParse(KeyElements[1], out BrandId) && KeyElements.Length > 2)
                    {
                        // Try to parse the model group
                        if (long.TryParse(KeyElements[2], out ModelGroupId) && KeyElements.Length > 3)
                        {
                            // Try to parse the model
                            if (long.TryParse(KeyElements[3], out ModelId) && KeyElements.Length > 4)
                            {
                                // Try to parse the type
                                long.TryParse(KeyElements[4], out TypeId);
                            }
                        }
                    }
                }
            }

            // Set the properties
            obTypKey.VehicleTypeId = VehicleTypeId;
            obTypKey.BrandId = BrandId;
            obTypKey.ModelGroupId = ModelGroupId;
            obTypKey.ModelId = ModelId;
            obTypKey.TypeId = TypeId;
        }

        private ObTypKey MyGetParent(ObTypKey obTypKey)
        {
            // Check if the brand is set
            if (obTypKey.BrandId <= 0)
            {
                // The key has no parent - return null
                return null;
            }

            // Set the default values
            long VehicleTypeId = obTypKey.VehicleTypeId;
            long BrandId = 0;
            long ModelGroupId = 0;
            long ModelId = 0;

            // Check if model group is set
            if (obTypKey.ModelGroupId > 0)
            {
                // Set the brand
                BrandId = obTypKey.BrandId;

                // Check if model is set
                if (obTypKey.ModelId > 0)
                {
                    // Set the model group
                    ModelGroupId = obTypKey.ModelGroupId;

                    // Check if type is set
                    if (obTypKey.TypeId > 0)
                    {
                        // Set the model
                        ModelId = obTypKey.ModelId;
                    }
                }
            }

            // Create and return a new key
            return new ObTypKey(VehicleTypeId, BrandId, ModelGroupId, ModelId, 0);
        }

        private long MyGetLastValidId()
        {
            long LastValidId = 0;

            if (this.VehicleTypeId > 0)
            {
                LastValidId = this.VehicleTypeId;

                if (this.BrandId > 0)
                {
                    LastValidId = this.BrandId;

                    if (this.ModelGroupId > 0)
                    {
                        LastValidId = this.ModelGroupId;

                        if (this.ModelId > 0)
                        {
                            LastValidId = this.ModelId;

                            if (this.TypeId > 0)
                            {
                                LastValidId = this.TypeId;
                            }
                        }
                    }
                }
            }

            return LastValidId;
        }
        #endregion
    }
}
