using Cic.OpenOne.Common.Util.Security;

namespace Cic.OpenOne.Common.Model.DdOw
{
    /// <summary>
    /// Puser Hilfsmethoden
    /// </summary>
    public class PUserUtil
    {

        #region Private constants

        // 
        private const string CnstBlowfishKeyMA = "C.I.C.-S0ftwareGmbH1987Muenchen0";
        private const string CnstBlowfishKeyB2B = "C.I.C.-S0ftwareGmbH1987B2B2011_0";

        public const string CnstPreSharedKeyTypeMA = "MA";
        public const string CnstPreSharedKeyTypeB2B = "B2B";
        public const string CnstPreSharedKeyTypeTXT = "TXT";
        

       
        #endregion



        /// <summary>
        /// EncryptPassword
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password)
        {
            Blowfish Blowfish;

            try
            {
                Blowfish = new Blowfish(MyGetBlowFishKey());
                // Encode
                return Blowfish.Encode(password);
            }
            catch
            {
                // Ignore exception
                return password;
            }
        }

        // TEST BK 0 BK, Not tested
        /// <summary>
        /// DecryptPassword
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecryptPassword(string password)
        {
            Blowfish Blowfish;

            try
            {
                Blowfish = new Blowfish(MyGetBlowFishKey());
                // Encode
                return Blowfish.Decode(password);
            }
            catch
            {
                // Ignore exception
                return password;
            }
        }

        private static string MyGetBlowFishKey()
        {
            string blowFishKey = CnstBlowfishKeyMA;
            if (Cic.OpenOne.Common.Properties.Config.Default.PreSharedKeyType.Equals(CnstPreSharedKeyTypeB2B))
            {
                blowFishKey = CnstBlowfishKeyB2B;
            }
            return blowFishKey;
        }


    }
}