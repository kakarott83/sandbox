// OWNER MK, 19-03-2009
namespace Cic.OpenLease.Service.Merge.CalculationCore
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.CalculationCore;
    using System;
    using Cic.One.Utils.DTO;
    #endregion

    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.CalculationCore")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class CalculationCoreService : Cic.OpenLease.ServiceAccess.Merge.CalculationCore.ICalculationCore
    {
        #region Private const
        private enum SearchModeConstant : int
        {
            Floor = 1,
            Round
        }
        #endregion

        #region Private constants
        private const SearchModeConstant CnstDefaultSearchLZModeConstant = SearchModeConstant.Floor;
        #endregion

        #region Methods
        public decimal DeliverResidualValue(ResidualValueRequestDto residualValueRequestDto)
        {
            decimal Value = 0.0M;

            //// Validate
            //ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            //ServiceValidator.ValidateExecute();

            if (residualValueRequestDto == null)
            {
                throw new ArgumentException("residualValueRequestDto");
            }

            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                // Set SysVg from ValueGroup enumerator
                long SysVg = (int)residualValueRequestDto.ValueGroupType;

                // Ich MappingId set than set new SysVg acording to this value
                if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(residualValueRequestDto.MappingId))
                {
                    SysVg = Cic.OpenLease.Model.DdOl.OBTYPHelper.DeliverSysRwVg(Context, residualValueRequestDto.MappingId.Trim());
                }

                Value = Cic.OpenLease.Model.DdOl.VGHelper.DeliverValue(Context, SysVg, residualValueRequestDto.KmPerYear, residualValueRequestDto.Term);
            }

            return Value;
        }

        public CalculationDto Calculate(CalculationDto calculationDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateExecute();

            if (calculationDto == null)
            {
                throw new ArgumentException("creditCalculationDto");
            }

            calculationDto = MyCalculate(calculationDto);

            return calculationDto;
        }

        
        #endregion

        #region Methods
       
        
        #endregion
        
        #region MyMethods

        private static CalculationDto MyCalculate(CalculationDto calculationDto)
        {
            if (calculationDto.PPY == 0)
            {
                throw new ArgumentException("PPY");
            }

            
            // Nominal or effective interest
            if (calculationDto.HoldZINS)
            {
                if (calculationDto.ZINS == 0)
                {
                    throw new ArgumentException("ZINS");
                }
                calculationDto.ZINSEFF = KalkulationHelper.CalculateEffectiveInterest(calculationDto.ZINS, calculationDto.PPY);
            }
            else
            {
                calculationDto.ZINS = KalkulationHelper.CalculateNominalInterest(calculationDto.ZINSEFF, calculationDto.PPY);
            }

            // FirstPayment
           if (calculationDto.HoldSZ)
            {
                // Get percent
                calculationDto.SZP = KalkulationHelper.CalculatePercent(calculationDto.BG, calculationDto.SZ);
            }
            else
            {
                // Get value
                calculationDto.SZ = KalkulationHelper.CalculateValue(calculationDto.BG, calculationDto.SZP);
            }

            // ResidualValue
            if (calculationDto.HoldRW)
            {
                // Get percent
                calculationDto.RWP = KalkulationHelper.CalculatePercent(calculationDto.BG, calculationDto.RW);
            }
            else
            {
                // Get value
                calculationDto.RW = KalkulationHelper.CalculateValue(calculationDto.BG, calculationDto.RWP);
            }

            

            decimal Base = calculationDto.BG;
            decimal FirstPayment = calculationDto.SZ;
            decimal Term = calculationDto.LZ;
            decimal Rate = calculationDto.RATE;
            decimal ResidualValue = calculationDto.RW;
            decimal Interest = calculationDto.ZINS;

            // Go to calculation core
         
           // Cic.Basic.OpenLease.CalculationCore.CalculationCoreHelper.Calculate(ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref Interest, calculationDto.PPY, calculationDto.CalculationTarget, calculationDto.Mode);
            decimal InterestEff = 0;
            KalkulationHelper.CalculateRate(ref Base, ref FirstPayment, ref Term, ref Rate, ref ResidualValue, ref Interest, ref InterestEff, calculationDto.PPY, calculationDto.CalculationTarget, calculationDto.Mode);

            calculationDto.BG = Base;
            calculationDto.SZ = FirstPayment;
            calculationDto.RATE = Rate;
            calculationDto.RW = ResidualValue;
            calculationDto.ZINS = Interest;

            if (calculationDto.CalculationTarget == CalculationTargets.CalculateTerm)
            {
                // Find the LZ value in accordance with the conditions
                calculationDto.LZ = MyFindLZ(Term, calculationDto.TermMin, calculationDto.TermMax, calculationDto.TermStep, CnstDefaultSearchLZModeConstant);
            }
            else
            {
                calculationDto.LZ = MyFindLZ(Term);
            }

            if (calculationDto.CalculationTarget == CalculationTargets.CalculateNominalInterest)
            {
                // Effektivzins
                calculationDto.ZINSEFF = KalkulationHelper.CalculateEffectiveInterest(calculationDto.ZINS, calculationDto.PPY);
            }

            if (calculationDto.CalculationTarget == CalculationTargets.CalculateFirstPayment)
            {
                // FirstPayment
                calculationDto.SZP = KalkulationHelper.CalculatePercent(calculationDto.BG, calculationDto.SZ);
            }

            if (calculationDto.CalculationTarget == CalculationTargets.CalculateResidualValueOrRemainingDebt)
            {
                // ResidualValue
                calculationDto.RWP = KalkulationHelper.CalculatePercent(calculationDto.BG, calculationDto.RW);
            }

            // Calculation target is lz - recalulate with target sz
            if (calculationDto.CalculationTarget == CalculationTargets.CalculateTerm)
            {
                calculationDto.CalculationTarget = CalculationTargets.CalculateFirstPayment;
                return MyCalculate(calculationDto);
            }

            return calculationDto;
        }

        private static int MyFindLZ(decimal term, int? termMin, int? termMax, int? termStep, SearchModeConstant searchModeConstant)
        {
            System.Collections.Generic.List<int> TermList;
            int NewTerm;

            TermList = null;
            NewTerm = 0;

            try
            {
                // Create list
                if (termMin != null && termMin > 0 && termMax != null && termMax > termMin)
                {
                    TermList = new System.Collections.Generic.List<int>();

                    if (termStep != null && termStep > 0)
                    {                        
                        int NextStep = (int)termMin;

                        while (NextStep <= termMax)
                        {
                            TermList.Add((int)NextStep);
                            NextStep = NextStep + (int)termStep;
                        }
                    }
                }

                if (searchModeConstant == SearchModeConstant.Floor)
                {
                    // Find the floor value
                    if (TermList != null && TermList.Count >= 2)
                    {
                        if (term < TermList[0])
                        {
                            // Choose minimum
                            NewTerm = TermList[0];
                        }
                        else if (term >= TermList[TermList.Count - 1])
                        {
                            // Choose maximum
                            NewTerm = TermList[TermList.Count - 1];
                        }
                        else
                        {
                            for (int Index = 0; Index < (TermList.Count - 1); Index++)
                            {
                                // Find nearest values
                                if (term >= TermList[Index] && term < TermList[(Index + 1)])
                                {
                                    NewTerm = TermList[Index];
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (searchModeConstant == SearchModeConstant.Round)
                {
                    // Find the nearest value
                    if (TermList != null && TermList.Count >= 2)
                    {
                        if (term < TermList[0])
                        {
                            // Choose minimum
                            NewTerm = TermList[0];
                        }
                        else if (term > TermList[TermList.Count - 1])
                        {
                            // Choose maximum
                            NewTerm = TermList[TermList.Count - 1];
                        }
                        else
                        {
                            for (int Index = 0; Index < (TermList.Count - 1); Index++)
                            {
                                // Find the nearest values
                                if (term >= TermList[Index] && term <= TermList[(Index + 1)])
                                {
                                    // Make middle value
                                    decimal MiddleValue = ((TermList[Index] + TermList[(Index + 1)]) / 2.0M);

                                    if (term < MiddleValue)
                                    {
                                        NewTerm = TermList[Index];
                                        break;
                                    }
                                    else
                                    {
                                        NewTerm = TermList[(Index + 1)];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore exception
            }

            if (NewTerm == 0)
            {
                // The list was not created
                NewTerm = MyFindLZ(term);
            }

            return NewTerm;
        }

        private static int MyFindLZ(decimal term)
        {
            return (int)System.Math.Floor(term);
        }
        #endregion
    }
}
