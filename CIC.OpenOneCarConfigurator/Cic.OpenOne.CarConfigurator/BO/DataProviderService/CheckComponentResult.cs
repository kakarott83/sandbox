// OWNER: BK, 18-04-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// Check Component Result
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public sealed class CheckComponentResult : Cic.OpenOne.CarConfigurator.BO.DataProviderService.ICheckComponentResult
	{
		#region Private variables
		private Cic.P000001.Common.DataProvider.CheckComponentResultConstants _CheckComponentResultConstant;
		private Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression[] _ConflictCheckComponentExpressions;
		private Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression[] _RequirementCheckComponentExpressions;
		private Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression[] _NewPriceCheckComponentExpressions;
		private Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression[] _DiscountCheckComponentExpressions;
		private string _Message;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public CheckComponentResult()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="checkComponentResult"></param>
        /// <todo>BK 0 BK, Not tested</todo>
		internal CheckComponentResult(Cic.P000001.Common.DataProvider.CheckComponentResult checkComponentResult)
		{
			System.Collections.Generic.List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression> ConflictCheckComponentExpressionList;
			System.Collections.Generic.List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression> RequirementCheckComponentExpressionList;
			System.Collections.Generic.List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression> NewPriceCheckComponentExpressionList;
			System.Collections.Generic.List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression> DiscountCheckComponentExpressionList;
			int Index;

			// Check object
			if (checkComponentResult == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("checkComponentResult");
			}

			try
			{
				// Check expressions
				if ((checkComponentResult.CheckComponentExpressions != null) && (checkComponentResult.CheckComponentExpressions.GetLength(0) > 0))
				{
					// New lists
					ConflictCheckComponentExpressionList = new System.Collections.Generic.List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression>();
					RequirementCheckComponentExpressionList = new System.Collections.Generic.List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression>();
					NewPriceCheckComponentExpressionList = new System.Collections.Generic.List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression>();
					DiscountCheckComponentExpressionList = new System.Collections.Generic.List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression>();

					// Loop through check component expressions
					foreach (Cic.P000001.Common.DataProvider.CheckComponentExpression LoopCheckComponentExpression in checkComponentResult.CheckComponentExpressions)
					{
						// Check type
						switch (LoopCheckComponentExpression.CheckComponentExpressionConstant)
						{
							// Conflict
							case Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants.Conflict:
								// Add to list
								ConflictCheckComponentExpressionList.Add(Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertConflictCheckComponentExpression((Cic.P000001.Common.DataProvider.ConflictCheckComponentExpression)LoopCheckComponentExpression));
								break;
							// Requirement
							case Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants.Requirement:
								// Add to list
                                RequirementCheckComponentExpressionList.Add(Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertRequirementCheckComponentExpression((Cic.P000001.Common.DataProvider.RequirementCheckComponentExpression)LoopCheckComponentExpression));
								break;
							// NewPrice
							case Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants.NewPrice:
								// Add to list
                                NewPriceCheckComponentExpressionList.Add(Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertNewPriceCheckComponentExpression((Cic.P000001.Common.DataProvider.NewPriceCheckComponentExpression)LoopCheckComponentExpression));
								break;
							// Discount
							case Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants.Discount:
								// Add to list
                                DiscountCheckComponentExpressionList.Add(Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertDiscountCheckComponentExpression((Cic.P000001.Common.DataProvider.DiscountCheckComponentExpression)LoopCheckComponentExpression));
								break;
						}
					}

					// Check conflict list
					if (ConflictCheckComponentExpressionList.Count > 0)
					{
						// Resize
						System.Array.Resize<Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression>(ref _ConflictCheckComponentExpressions, ConflictCheckComponentExpressionList.Count);
						// Reset index
						Index = -1;
						// Loop through list
						foreach(Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression LoopConflictCheckComponentExpression in ConflictCheckComponentExpressionList)
						{
							// Increase index
							Index++;
							// Add
							_ConflictCheckComponentExpressions[Index] = LoopConflictCheckComponentExpression;
						}
					}
					// Check requirement list
					if (RequirementCheckComponentExpressionList.Count > 0)
					{
						// Resize
						System.Array.Resize<Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression>(ref _RequirementCheckComponentExpressions, RequirementCheckComponentExpressionList.Count);
						// Reset index
						Index = -1;
						// Loop through list
						foreach (Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression LoopRequirementCheckComponentExpression in RequirementCheckComponentExpressionList)
						{
							// Increase index
							Index++;
							// Add
							_RequirementCheckComponentExpressions[Index] = LoopRequirementCheckComponentExpression;
						}
					}
					// Check new price list
					if (NewPriceCheckComponentExpressionList.Count > 0)
					{
						// Resize
						System.Array.Resize<Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression>(ref _NewPriceCheckComponentExpressions, NewPriceCheckComponentExpressionList.Count);
						// Reset index
						Index = -1;
						// Loop through list
						foreach (Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression LoopNewPriceCheckComponentExpression in NewPriceCheckComponentExpressionList)
						{
							// Increase index
							Index++;
							// Add
							_NewPriceCheckComponentExpressions[Index] = LoopNewPriceCheckComponentExpression;
						}
					}
					// Check discount list
					if (DiscountCheckComponentExpressionList.Count > 0)
					{
						// Resize
						System.Array.Resize<Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression>(ref _DiscountCheckComponentExpressions, DiscountCheckComponentExpressionList.Count);
						// Reset index
						Index = -1;
						// Loop through list
						foreach (Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression LoopDiscountCheckComponentExpression in DiscountCheckComponentExpressionList)
						{
							// Increase index
							Index++;
							// Add
							_DiscountCheckComponentExpressions[Index] = LoopDiscountCheckComponentExpression;
						}
					}
				}
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Set values
			_CheckComponentResultConstant = checkComponentResult.CheckComponentResultConstant;
			_Message = checkComponentResult.Message;
		}
		#endregion

        /// <summary>
        /// Check Component Result Constant
        /// </summary>
		#region ICheckComponentResultBase properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.DataProvider.CheckComponentResultConstants CheckComponentResultConstant
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _CheckComponentResultConstant;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_CheckComponentResultConstant = value;
			}
		}

        /// <summary>
        /// Message
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Message
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _Message;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_Message = value;
			}
		}
		#endregion

		#region ICheckComponentResult properties
        /// <summary>
        /// Conflict Check for Component Expression
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression[] ConflictCheckComponentExpressions
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _ConflictCheckComponentExpressions;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_ConflictCheckComponentExpressions = value;
			}
		}

        /// <summary>
        /// Requirement Check fo Component Expression
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression[] RequirementCheckComponentExpressions
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _RequirementCheckComponentExpressions;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_RequirementCheckComponentExpressions = value;
			}
		}

        /// <summary>
        /// New Price Check for Compnent Expression
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression[] NewPriceCheckComponentExpressions
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _NewPriceCheckComponentExpressions;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_NewPriceCheckComponentExpressions = value;
			}
		}

        /// <summary>
        /// Discount Check for Component Expressions
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression[] DiscountCheckComponentExpressions
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _DiscountCheckComponentExpressions;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_DiscountCheckComponentExpressions = value;
			}
		}
		#endregion
	}
}
