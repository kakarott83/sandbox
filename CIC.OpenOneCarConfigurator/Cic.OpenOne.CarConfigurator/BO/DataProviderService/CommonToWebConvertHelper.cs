// OWNER: BK, 10-06-2008


using Cic.OpenOne.CarConfigurator.Util.Expressions;
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
	
	internal static class DataProviderCommonToWebConvertHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Level ConvertLevel(Cic.P000001.Common.Level commonLevel)
		{
			Cic.P000001.Common.Level WebLevel = null;

			// Check common level
            if (commonLevel != null)
            {
                // New level
                WebLevel = commonLevel;

            }

			// Return
			return WebLevel;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Level[] ConvertLevels(Cic.P000001.Common.Level[] commonLevels)
		{
			Cic.P000001.Common.Level[] WebLevels = null;
			int Index;

			// Check common level
			if (commonLevels != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Level>(ref WebLevels, commonLevels.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through levels
				foreach (Cic.P000001.Common.Level LoopLevel in commonLevels)
				{
					// Increase index
					Index++;
					try
					{
						// Add new level
                        WebLevels[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertLevel(LoopLevel);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebLevels;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Parameter ConvertParameter(Cic.P000001.Common.Parameter commonParameter)
		{
			Cic.P000001.Common.Parameter WebParameter = null;

			// Check common parameter
			if (commonParameter != null)
			{
				try
				{
					// New parameter
					WebParameter = commonParameter;
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebParameter;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Parameter[] ConvertParameters(Cic.P000001.Common.Parameter[] commonParameters)
		{
			Cic.P000001.Common.Parameter[] WebParameters = null;
			int Index;

			// Check common parameters
			if (commonParameters != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Parameter>(ref WebParameters, commonParameters.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through parameters
				foreach (Cic.P000001.Common.Parameter LoopParameter in commonParameters)
				{
					// Increase index
					Index++;
					try
					{
						// Add new parameter
                        WebParameters[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertParameter(LoopParameter);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebParameters;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.TreeNode ConvertTreeNode(Cic.P000001.Common.TreeNode commonTreeNode)
		{
			Cic.P000001.Common.TreeNode WebTreeNode = null;

			// Check common tree node
			if (commonTreeNode != null)
			{
				try
				{
					// New tree node
					WebTreeNode = commonTreeNode;
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebTreeNode;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.TreeNode[] ConvertTreeNodes(Cic.P000001.Common.TreeNode[] commonTreeNodes)
		{
			Cic.P000001.Common.TreeNode[] WebTreeNodes = null;
			int Index;

			// Check common tree nodes
			if (commonTreeNodes != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.TreeNode>(ref WebTreeNodes, commonTreeNodes.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through tree nodes
				foreach (Cic.P000001.Common.TreeNode LoopTreeNode in commonTreeNodes)
				{
					// Increase index
					Index++;
					try
					{
						// Add new tree node
                        WebTreeNodes[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertTreeNode(LoopTreeNode);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebTreeNodes;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Picture ConvertPicture(Cic.P000001.Common.Picture commonPicture)
		{
			Cic.P000001.Common.Picture WebPicture = null;

			// Check common picture
			if (commonPicture != null)
			{
				try
				{
					// New picture
					WebPicture = commonPicture;
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebPicture;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Picture[] ConvertPictures(Cic.P000001.Common.Picture[] commonPictures)
		{
			Cic.P000001.Common.Picture[] WebPictures = null;
			int Index;

			// Check common pictures
			if (commonPictures != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Picture>(ref WebPictures, commonPictures.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through pictures
				foreach (Cic.P000001.Common.Picture LoopPicture in commonPictures)
				{
					// Increase index
					Index++;
					try
					{
						// Add new picture
                        WebPictures[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertPicture(LoopPicture);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebPictures;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Component ConvertComponent(Cic.P000001.Common.Component commonComponent)
		{
			Cic.P000001.Common.Component WebComponent = null;

			// Check common component
			if (commonComponent != null)
			{
				try
				{
					// New component
					WebComponent = commonComponent;
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebComponent;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Component[] ConvertComponents(Cic.P000001.Common.Component[] commonComponents)
		{
			Cic.P000001.Common.Component[] WebComponents = null;
			int Index;

			// Check common components
			if (commonComponents != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Component>(ref WebComponents, commonComponents.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through components
				foreach (Cic.P000001.Common.Component LoopComponent in commonComponents)
				{
					// Increase index
					Index++;
					try
					{
						// Add new component
                        WebComponents[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertComponent(LoopComponent);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebComponents;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentResult ConvertCheckComponentResult(Cic.P000001.Common.DataProvider.CheckComponentResult commonCheckComponentResult)
		{
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentResult WebCheckComponentResult = null;

			// Check common check component result
			if (commonCheckComponentResult != null)
			{
				try
				{
					// New check component result
					WebCheckComponentResult = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentResult(commonCheckComponentResult);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebCheckComponentResult;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.TreeNodeDetail ConvertTreeNodeDetail(Cic.P000001.Common.TreeNodeDetail commonTreeNodeDetail)
		{
			Cic.P000001.Common.TreeNodeDetail WebTreeNodeDetail = null;

			// Check common tree node detail
			if (commonTreeNodeDetail != null)
			{
				try
				{
					// New tree node detail
					WebTreeNodeDetail = commonTreeNodeDetail;
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebTreeNodeDetail;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.TreeNodeDetail[] ConvertTreeNodeDetails(Cic.P000001.Common.TreeNodeDetail[] commonTreeNodeDetails)
		{
			Cic.P000001.Common.TreeNodeDetail[] WebTreeNodeDetails = null;
			int Index;

			// Check common tree node details
			if (commonTreeNodeDetails != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.TreeNodeDetail>(ref WebTreeNodeDetails, commonTreeNodeDetails.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through tree node details
				foreach (Cic.P000001.Common.TreeNodeDetail LoopTreeNodeDetail in commonTreeNodeDetails)
				{
					// Increase index
					Index++;
					try
					{
						// Add new tree node detail
                        WebTreeNodeDetails[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertTreeNodeDetail(LoopTreeNodeDetail);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebTreeNodeDetails;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ComponentDetail ConvertComponentDetail(Cic.P000001.Common.ComponentDetail commonComponentDetail)
		{
			Cic.P000001.Common.ComponentDetail WebComponentDetail = null;

			// Check common component detail
			if (commonComponentDetail != null)
			{
				try
				{
					// New component detail
					WebComponentDetail = commonComponentDetail;
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebComponentDetail;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ComponentDetail[] ConvertComponentDetails(Cic.P000001.Common.ComponentDetail[] commonComponentDetails)
		{
			Cic.P000001.Common.ComponentDetail[] WebComponentDetails = null;
			int Index;

			// Check common component details
			if (commonComponentDetails != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.ComponentDetail>(ref WebComponentDetails, commonComponentDetails.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through component details
				foreach (Cic.P000001.Common.ComponentDetail LoopComponentDetail in commonComponentDetails)
				{
					// Increase index
					Index++;
					try
					{
						// Add new component detail
                        WebComponentDetails[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertComponentDetail(LoopComponentDetail);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebComponentDetails;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject[] ConvertExpression(Expression<string> expression)
		{
			System.Collections.Generic.Dictionary<string, Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject> ExpressionDictionary = null;
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject[] ExpressionDataTransferObjects = null;
			int Index;

			// Check expression
			if (expression != null)
			{
				// Create expression dictionary
				MyCreateExpressionDictionary(expression, ref ExpressionDictionary);
				// Check dictionary
				if ((ExpressionDictionary != null) && (ExpressionDictionary.Count > 0))
				{
					// Resize
					System.Array.Resize<Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject>(ref ExpressionDataTransferObjects, ExpressionDictionary.Count);
					// Reset index
					Index = -1;
					// Loop through expression data transfer objects
					foreach (Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject LoopExpressionDataTransferObject in ExpressionDictionary.Values)
					{
						// Increase index
						Index++;
						try
						{
							// Add expression data transfer object
							ExpressionDataTransferObjects[Index] = LoopExpressionDataTransferObject;
						}
						catch
						{
							// Throw caught exception
							throw;
						}
					}
				}
			}

			// Return
			return ExpressionDataTransferObjects;
		}
		#endregion

		#region My methods
		// TODO BK 0 BK, Not tested
		private static Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants MyGetExpressionTypeConstant(Expression<string> expression)
		{
			System.Type Type;
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants TypeConstant;

			// Check expression
			if (expression == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("expression");
			}

			// Get type
			Type = expression.GetType();

			// Component expression
			if (Type == typeof(ValueExpression<string>))
			{
				// Set type constant
				TypeConstant = Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants.Component;
			}
			// And expression
			else if (Type == typeof(AndExpression<string>))
			{
				// Set type constant
				TypeConstant = Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants.And;
			}
			// Or expression
			else if (Type == typeof(OrExpression<string>))
			{
				// Set type constant
				TypeConstant = Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants.Or;
			}
			// Else
			else
			{
				// Throw exception
				// TODO BK 0 BK, Localize text, add exception class
				throw new System.ApplicationException("Expression type is not supported");
			}

			// Return
			return TypeConstant;
		}

		// TODO BK 0 BK, Not tested
		private static string MyCreateExpressionDictionary(Expression<string> expression, ref System.Collections.Generic.Dictionary<string, Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject> expressionDictionary)
		{
			ValueExpression<string> ComponentExpression;
			AndExpression<string> AndExpression;
			OrExpression<string> OrExpression;
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject ExpressionDataTransferObject;
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants TypeConstant;
			string Key = null;
			string KeyLeft;
			string KeyRight;

			// Check expression
			if (expression != null)
			{
				// Get type constant
				TypeConstant = MyGetExpressionTypeConstant(expression);

				// Check type constant
				switch (TypeConstant)
				{
					// Component expression
					case Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants.Component:
						// Set object
						ComponentExpression = (ValueExpression<string>)expression;
						// Get key
						Key = MyGetExpressionDictionaryKey(expressionDictionary);
						// Check dictionary
						if (expressionDictionary == null)
						{
							// New dictionary
							expressionDictionary = new System.Collections.Generic.Dictionary<string, Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject>();
						}

						try
						{
							// New 
							ExpressionDataTransferObject = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject(Key, TypeConstant, ComponentExpression.Value, null, null);
							// Add
							expressionDictionary.Add(Key, ExpressionDataTransferObject);
						}
						catch
						{
							// Throw caught exception
							throw;
						}

						break;
					// And expression
					case Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants.And:
						// Set object
						AndExpression = (AndExpression<string>)expression;

						try
						{
							// Create expression dictionary
							KeyRight = MyCreateExpressionDictionary(AndExpression.RightExpression, ref expressionDictionary);
							// Create expression dictionary
							KeyLeft = MyCreateExpressionDictionary(AndExpression.LeftExpression, ref expressionDictionary);
						}
						catch
						{
							// Throw caught exception
							throw;
						}

						// Get key
						Key = MyGetExpressionDictionaryKey(expressionDictionary);

						try
						{
							// New 
							ExpressionDataTransferObject = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject(Key, TypeConstant, null, KeyLeft, KeyRight);
							// Add
							expressionDictionary.Add(Key, ExpressionDataTransferObject);
						}
						catch
						{
							// Throw caught exception
							throw;
						}
						break;
					// Or expression
					case Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferTypeConstants.Or:
						// Set object
						OrExpression = (OrExpression<string>)expression;

						try
						{
							// Create expression dictionary
							KeyRight = MyCreateExpressionDictionary(OrExpression.RightExpression, ref expressionDictionary);
							// Create expression dictionary
							KeyLeft = MyCreateExpressionDictionary(OrExpression.LeftExpression, ref expressionDictionary);
						}
						catch
						{
							// Throw caught exception
							throw;
						}

						// Get key
						Key = MyGetExpressionDictionaryKey(expressionDictionary);

						try
						{
							// New 
							ExpressionDataTransferObject = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject(Key, TypeConstant, null, KeyLeft, KeyRight);
							// Add
							expressionDictionary.Add(Key, ExpressionDataTransferObject);
						}
						catch
						{
							// Throw caught exception
							throw;
						}
						break;
				}
			}

			// Return
			return Key;
		}

		// TODO BK 0 BK, Not tested
		private static string MyGetExpressionDictionaryKey(System.Collections.Generic.Dictionary<string, Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject> expressionDictionary)
		{
			string Key;

			// Check dictionary
			if (expressionDictionary != null)
			{
				// Create key
				Key = System.Guid.NewGuid().ToString();
				// Check existence
				while (expressionDictionary.ContainsKey(Key))
				{
					// Create key
					Key = System.Guid.NewGuid().ToString();
				}
			}
			else
			{
				// Create key
				Key = System.Guid.NewGuid().ToString();
			}

			// Return
			return Key;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression ConvertConflictCheckComponentExpression(Cic.P000001.Common.DataProvider.ConflictCheckComponentExpression commonConflictCheckComponentExpression)
		{
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression WebConflictCheckComponentExpression = null;

			// Check common conflict check component expression
			if (commonConflictCheckComponentExpression != null)
			{
				try
				{
					// New conflict check component expression
					WebConflictCheckComponentExpression = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression(commonConflictCheckComponentExpression);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebConflictCheckComponentExpression;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression ConvertRequirementCheckComponentExpression(Cic.P000001.Common.DataProvider.RequirementCheckComponentExpression commonRequirementCheckComponentExpression)
		{
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression WebRequirementCheckComponentExpression = null;

			// Check common requirement check component expression
			if (commonRequirementCheckComponentExpression != null)
			{
				try
				{
					// New requirement check component expression
					WebRequirementCheckComponentExpression = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression(commonRequirementCheckComponentExpression);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebRequirementCheckComponentExpression;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression ConvertNewPriceCheckComponentExpression(Cic.P000001.Common.DataProvider.NewPriceCheckComponentExpression commonNewPriceCheckComponentExpression)
		{
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression WebNewPriceCheckComponentExpression = null;

			// Check common new price check component expression
			if (commonNewPriceCheckComponentExpression != null)
			{
				try
				{
					// New new price check component expression
					WebNewPriceCheckComponentExpression = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression(commonNewPriceCheckComponentExpression);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebNewPriceCheckComponentExpression;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression ConvertDiscountCheckComponentExpression(Cic.P000001.Common.DataProvider.DiscountCheckComponentExpression commonDiscountCheckComponentExpression)
		{
			Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression WebDiscountCheckComponentExpression = null;

			// Check common discount check component expression
			if (commonDiscountCheckComponentExpression != null)
			{
				try
				{
					// New discount check component expression
					WebDiscountCheckComponentExpression = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression(commonDiscountCheckComponentExpression);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return WebDiscountCheckComponentExpression;
		}
		#endregion
	}
}
