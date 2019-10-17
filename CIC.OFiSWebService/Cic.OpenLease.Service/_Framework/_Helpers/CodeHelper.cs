// OWNER WB, 26-03-2010
namespace Cic.OpenLease.Service
{
    // [System.CLSCompliant(true)]
    internal static class CodeHelper
    {

        #region Methods

        //It returns guid's part depends on length value
        public static string GetCorrectValue(string value, int length)
        {
            if (length == 0)
            {
                length = 35;
            }

            string ReturnValue = value;
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(value))
            {
                ReturnValue = System.Guid.NewGuid().ToString().Substring(0, length);
            }

            return ReturnValue;
        }

        #endregion
    }
}