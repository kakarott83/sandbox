// OWNER JJ, 02-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using System.Collections.Generic;
    #endregion

    #region Methods
    [System.CLSCompliant(true)]
    public static class DtoAssemblerHelper
    {
        public static string DeliverErrorMessage(System.Collections.Generic.Dictionary<string, string> errors)
        {
            System.Text.StringBuilder StringBuilder;
            string ErrorMessage = null;

            if (errors != null && errors.Count > 0)
            {
                StringBuilder = new System.Text.StringBuilder();

                foreach(KeyValuePair<string, string> LoopError in errors)
                {
                    StringBuilder.Append("Key: ");
                    StringBuilder.Append(LoopError.Key);
                    StringBuilder.Append(". ");
                    StringBuilder.Append("Description: ");
                    StringBuilder.Append(LoopError.Value);                    
                    StringBuilder.AppendLine();
                }

                ErrorMessage = StringBuilder.ToString();
            }

            return ErrorMessage;
        }
    }
    #endregion
}