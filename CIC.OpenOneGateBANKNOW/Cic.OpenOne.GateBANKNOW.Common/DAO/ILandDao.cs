namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public interface ILandDao
    {
        /// <summary>
        /// Returns the sysland from an iso country
        /// </summary>
        /// <param name="isoCountry"></param>
        /// <returns></returns>
        long GetSysLandFromIsoCountry(string isoCountry);
    }
}