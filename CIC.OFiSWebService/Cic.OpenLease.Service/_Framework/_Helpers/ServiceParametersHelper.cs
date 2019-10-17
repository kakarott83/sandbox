// OWNER MK, 04-02-2009
using System;
namespace Cic.OpenLease.Service.Helpers
{
    //[System.CLSCompliant(true)]
    internal class ServiceParametersHelper
    {
        #region Constants
        private const int CnstMaxTop = 300;
        private const int CnstMinTop = 1;
        #endregion

        #region Methods
        internal static void CheckSelectParameter(string where, object[] whereParams, string order, int skip, int top)
        {
            CheckCountParameter(where, whereParams);

            CheckTopParameter(top);

            CheckSkipParameter(skip);
        }

        internal static void CheckTopParameter(int top)
        {
            // Check max
            if (top > CnstMaxTop)
            {
                throw new Exception("Min = " + CnstMinTop + ", max = " + CnstMaxTop+ "top");
            }

            // Check min
            if (top < CnstMinTop)
            {
                throw new Exception("Min = " + CnstMinTop + ", max = " + CnstMaxTop+ "top");
            }

            // Check skip
            if (top < 0)
            {
                // Throw exception
                throw new Exception("top");
            }
        }

        internal static void CheckSkipParameter(int skip)
        {
            // Check skip
            if (skip < 0)
            {
                // Throw exception
                throw new Exception("skip");
            }
        }
        
        internal static void CheckCountParameter(string where, object[] whereParams)
        {
            // Currently no check
        }

        internal static void CheckSelectByIdParameter(long id)
        {
            // Currently no checks
        }

        #endregion
    }
}
