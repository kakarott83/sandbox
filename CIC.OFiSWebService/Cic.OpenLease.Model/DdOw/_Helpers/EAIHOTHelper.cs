// OWNER MK, 06-10-2009
namespace Cic.OpenLease.Model.DdOw
{
    [System.CLSCompliant(true)]
    public static class EAIHOTHelper
    {
        #region Methods
        public static EAIHOT InstanciateEaiHot(string code, string olTable, long? sysOlTable, string comLanguage, long sysPuser, long sysWfUser)
        {
            EAIHOT EaiHot = new EAIHOT();
            EaiHot.CODE = code;
            EaiHot.OLTABLE = olTable;
            EaiHot.SYSOLTABLE = sysOlTable;
            EaiHot.COMLANGUAGE = comLanguage;
            EaiHot.GUILANGUAGE = comLanguage;
            EaiHot.SYSPORTAL = sysPuser;
            EaiHot.SYSWFUSER = sysWfUser;
            EaiHot.EVALEXPRESSION = @"_f('" + code + "','','','','','')";

            return EaiHot;
        }
        #endregion
    }
}
