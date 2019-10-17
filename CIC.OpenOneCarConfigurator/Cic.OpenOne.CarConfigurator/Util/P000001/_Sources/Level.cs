// OWNER: BK, 15-04-2008
namespace Cic.P000001.Common
{
    
    [System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
	public class Level : Cic.P000001.Common.ILevel, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private int _Number;
		private string _Designation;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY LevelTestFixture.CheckProperties
		public Level()
		{
			// Set state
			_Parameterless = false;
		}

        public Level(int level)
        {
            this._Number = level;
            if (level == 0)
                _Designation = "Select vehicle type";
            else if (level ==1)
                _Designation = "Select brand";
            else if (level == 2)
                _Designation = "Select model group";
            else if (level == 3)
                _Designation = "Select model";
            else if (level == 4)
                _Designation = "Select type";
          
        }
		// TESTEDBY LevelTestFixture.ConstructWithInvalidNumber
		// TESTEDBY LevelTestFixture.CheckProperties
		public Level(int number, string designation)
		{
			// Check number
			if (number < 0)
			{
				// Throw exception
				throw  new System.ArgumentException("number");
			}

			// Set values
			_Number = number;
            if (!string.IsNullOrEmpty(designation))
            {
                _Designation = designation.Trim();
            }
		}

        // TODO BK 0 BK, Not tested
        internal Level(Cic.P000001.Common.Level level)
        {
            // Check object
            if (level == null)
            {
                // Throw exception
                throw new System.ArgumentException("level");
            }

            // Set values
            _Number = level.Number;
            _Designation = level.Designation;
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.Level")]
		// TESTEDBY LevelTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct
					// Create new instance
					new Cic.P000001.Common.Level(_Number, _Designation);
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

		#region ILevelBase properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public int Number
		{
			// TESTEDBY LevelTestFixture.CheckProperties
			get
			{
				// Return
				return _Number;
			}
			// NOTE BK, For serialization
			// TESTEDBY LevelTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Number = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Designation
		{
			// TESTEDBY LevelTestFixture.CheckProperties
			get
			{
				// Return
				return _Designation;
			}
			// NOTE BK, For serialization
			// TESTEDBY LevelTestFixture.CheckProperties
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
		#endregion
	}
}
