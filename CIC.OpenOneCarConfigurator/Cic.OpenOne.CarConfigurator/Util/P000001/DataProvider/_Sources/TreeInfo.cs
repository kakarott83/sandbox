// OWNER: BK, 15-04-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public sealed class TreeInfo : Cic.P000001.Common.DataProvider.ITreeInfo
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
		// TESTEDBY TreeInfoTestFixture.ConstructWithToSmallCountOfLevels
		// TESTEDBY TreeInfoTestFixture.ConstructWithToSmallCountOfNodes
		// TESTEDBY TreeInfoTestFixture.ConstructWithIsUniqueIsTrueAndWithoutLevels
		// TESTEDBY TreeInfoTestFixture.ConstructWithInvalidCountOfNodes
		// TESTEDBY TreeInfoTestFixture.ConstructWithPreviousLevelAndWithInvalidCurrentLevel
		// TESTEDBY TreeInfoTestFixture.ConstructWithPreviousLevelAndWithInvalidNextLevel
		// TESTEDBY TreeInfoTestFixture.ConstructWithoutPreviousLevelAndWithInvalidNextLevel
		// TESTEDBY TreeInfoTestFixture.CheckProperties
		public TreeInfo(bool isUnique, int countOfLevels, int countOfNodes, Cic.P000001.Common.Level previousLevel, Cic.P000001.Common.Level currentLevel, Cic.P000001.Common.Level nextLevel, Cic.P000001.Common.Level[] levels)
		{
			// Check count of levels
			if (countOfLevels < -1)
			{
				// Throw exception
				throw  new System.ArgumentException("countOfLevels");
			}

			// Check count of nodes
			if (countOfNodes < -1)
			{
				// Throw exception
				throw  new System.ArgumentException("countOfNodes");
			}

			// Check isunique and levels
			if ((!isUnique) && (levels != null))
			{
				// Throw exception
                throw new System.ArgumentException("levels");
			}

			// Check count of levels and nodes
			if ((countOfLevels == 0) && (countOfNodes > -1) && (countOfNodes != 0))
			{
				// Throw exception
				throw  new System.ArgumentException("countOfNodes");
			}

			// Check previous level
			if (previousLevel != null)
			{
				// Check current level number
				if ((currentLevel != null) && ((previousLevel.Number + 1) != currentLevel.Number))
				{
					// Throw exception
					throw  new System.ArgumentException("currentLevel.Number");
				}
				// Check next level number
				if ((nextLevel != null) && ((previousLevel.Number + 2) != nextLevel.Number))
				{
					// Throw exception
					throw  new System.ArgumentException("nextLevel.Number");
				}
			}
			else
			{
				// Check next level number
				if ((currentLevel != null) && (nextLevel != null) && ((currentLevel.Number + 1) != nextLevel.Number))
				{
					// Throw exception
					throw  new System.ArgumentException("nextLevel.Number");
				}
			}

			// Set values
			_IsUnique = isUnique;
			_CountOfLevels = countOfLevels;
			_CountOfNodes = countOfNodes;
			_PreviousLevel = previousLevel;
			_CurrentLevel = currentLevel;
			_NextLevel = nextLevel;
			_Levels = levels;
		}
		#endregion

		#region ITreeInfoBase properties
		public bool IsUnique
		{
			// TESTEDBY TreeInfoTestFixture.CheckProperties
			get
			{
				// Return
				return _IsUnique;
			}
		}

		public int CountOfLevels
		{
			// TESTEDBY TreeInfoTestFixture.CheckProperties
			get
			{
				// Return
				return _CountOfLevels;
			}
		}

		public int CountOfNodes
		{
			// TESTEDBY TreeInfoTestFixture.CheckProperties
			get
			{
				// Return
				return _CountOfNodes;
			}
		}
		#endregion

		#region ITreeInfo properties
		public Cic.P000001.Common.Level PreviousLevel
		{
			// TESTEDBY TreeInfoTestFixture.CheckProperties
			get
			{
				// Return
				return _PreviousLevel;
			}
		}

		public Cic.P000001.Common.Level CurrentLevel
		{
			// TESTEDBY TreeInfoTestFixture.CheckProperties
			get
			{
				// Return
				return _CurrentLevel;
			}
		}

		public Cic.P000001.Common.Level NextLevel
		{
			// TESTEDBY TreeInfoTestFixture.CheckProperties
			get
			{
				// Return
				return _NextLevel;
			}
		}

		public Cic.P000001.Common.Level[] Levels
		{
			// TESTEDBY TreeInfoTestFixture.CheckProperties
			get
			{
				// Return
				return _Levels;
			}
		}
		#endregion
	}
}
