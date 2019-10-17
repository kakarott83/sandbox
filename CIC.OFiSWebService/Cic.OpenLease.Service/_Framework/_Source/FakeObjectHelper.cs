// OWNER JJ, 06-10-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public static class FakeObjectHelper
    {
        #region Private constants
        // Angebot
        //private const  ANGEBOT.AngebotStatuses CnstAngebotState =  ANGEBOT.AngebotStatuses.New;
        private const long CnstAngebotSYSLS = 1;
        private const int CnstAngebotLOCKED = 0;
        private const int CnstAngebotAKTIVKZ = 1;
        private const long CnstAngebotDefaultSYSBRAND = 1;
        // PrProduct
        private const long CnstPrProductHardcodedCreditNumber = 3;      // CREDIT-now
        private const long CnstPrProductHardcodedLeasingNumber = 4;     // LEASE-now
        // Obtyp
        private const long CnstObtypHardcodedCreditTypeNumber = 11;     // 1 - Barauszahlung
        private const long CnstObtypHardcodedLeasingTypeNumber = 12;    // 2 - Personenwagen
        #endregion

        #region Methods
     /*   public static void SetDefaultData(Cic.OpenLease.Model.DdOl.ANGEBOT angebot, string externeId)
        {
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(angebot.ANGEBOT1))
            {
                angebot.ANGEBOT1 = Cic.Basic.OpenLease.NumberRangeHelper.GenerateNextOfferObjectNumberRangeValue();
            }
            
            angebot.ERFASSUNG = System.DateTime.Now;
            angebot.ZUSTAND = Cic.OpenLease.Model.DdOl.StateHelper.DeliverAngebotState(CnstAngebotState);
            angebot.ZUSTANDAM = System.DateTime.Now;
            angebot.SYSLS = CnstAngebotSYSLS;
            angebot.LOCKED = CnstAngebotLOCKED;
            angebot.AKTIVKZ = CnstAngebotAKTIVKZ;
            OlExtendedEntities.
            angebot.SYSBRAND = MyDeliverSysBrand(externeId);
        }*/

        public static  OBTYP DeliverObTyp(DdOlExtended context, string schwacke,  PRPRODUCT prproduct)
        {
            long? FixSYSOBTYP = null;

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(schwacke))
            {
                // get OBTYP from schwacke nr
                var Query = from obTypMap in context.OBTYPMAP
                            where obTypMap.CODE == schwacke
                            select obTypMap.OBTYP;

                return Query.FirstOrDefault();
            }
            else
            {
                if (prproduct != null)
                {
                    if (prproduct.SYSPRPRODUCT == CnstPrProductHardcodedCreditNumber)
                    {
                        // Credit
                        FixSYSOBTYP = CnstObtypHardcodedCreditTypeNumber;
                    }
                    else if (prproduct.SYSPRPRODUCT == CnstPrProductHardcodedLeasingNumber)
                    {
                        // Leasing
                        FixSYSOBTYP = CnstObtypHardcodedLeasingTypeNumber;
                    }

                    if (FixSYSOBTYP != null)
                    {
                        // get hardcoded obtyp
                        var Query = from obTyp in context.OBTYP
                                    where obTyp.SYSOBTYP == FixSYSOBTYP
                                    select obTyp;

                        return Query.FirstOrDefault();
                    }
                }

                return null;
            }
        }
        #endregion

        #region My methods
        private static long? MyDeliverSysBrand(string externeId)
        {
            long? SysBrand = CnstAngebotDefaultSYSBRAND;

            if (externeId.ToLower() == "netvision4")
            {
                SysBrand = 4; // SYSBRAND 4, BMW
            }

            return SysBrand;
        }
        #endregion
    }
}
