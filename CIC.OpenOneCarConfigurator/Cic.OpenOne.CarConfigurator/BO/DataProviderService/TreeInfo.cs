// OWNER: BK, 16-04-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    
    /// <summary>
    /// Tree Info
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public sealed class TreeInfo : Cic.OpenOne.CarConfigurator.BO.DataProviderService.ITreeInfo
	{
		#region Private variables
		private bool _IsUnique;
		private int _CountOfLevels;
		private int _CountOfNodes;
		private Cic.P000001.Common.Level _PreviousLevel;
		private Cic.P000001.Common.Level _CurrentLevel;
		private Cic.P000001.Common.Level _NextLevel;
		private Cic.P000001.Common.Level[] _Levels;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
		public TreeInfo()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="treeInfo"></param>
		internal TreeInfo(Cic.P000001.Common.DataProvider.TreeInfo treeInfo)
		{
			// Check object
			if (treeInfo == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("treeInfo");
			}

			try
			{
				// Set values
                _PreviousLevel = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertLevel(treeInfo.PreviousLevel);
                _CurrentLevel = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertLevel(treeInfo.CurrentLevel);
                _NextLevel = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertLevel(treeInfo.NextLevel);
                _Levels = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertLevels(treeInfo.Levels);
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Set values
			_IsUnique = treeInfo.IsUnique;
			_CountOfLevels = treeInfo.CountOfLevels;
			_CountOfNodes = treeInfo.CountOfNodes;
		}
		#endregion

		#region ITreeInfoBase properties
        /// <summary>
        /// Is Unique
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool IsUnique
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _IsUnique;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_IsUnique = value;
			}
		}

        /// <summary>
        /// Count of Levels
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public int CountOfLevels
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _CountOfLevels;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_CountOfLevels = value;
			}
		}

        /// <summary>
        /// Count of Nodes
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public int CountOfNodes
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _CountOfNodes;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_CountOfNodes = value;
			}
		}
		#endregion

		#region ITreeInfo properties
        /// <summary>
        /// Previous Level
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Level PreviousLevel
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _PreviousLevel;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_PreviousLevel = value;
			}
		}

        /// <summary>
        /// Current Level
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Level CurrentLevel
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _CurrentLevel;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_CurrentLevel = value;
			}
		}

        /// <summary>
        /// Next Level
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Level NextLevel
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _NextLevel;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_NextLevel = value;
			}
		}

        /// <summary>
        /// Levels
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Level[] Levels
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _Levels;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_Levels = value;
			}
		}
		#endregion
	}
}
