

namespace Cic.OpenLease.Model.DdEurotax
{
    #region Using
    using System.Linq;
    #endregion
    public static class ETGTYPEHelper
    {
        #region Methods

        public static System.Collections.Generic.List<ETGTYRES> GetFrontTires(EurotaxEntities context, string natCode)
        {
            var QueryTireFront = from etgjwheel in context.ETGJWHEEL
                                 select etgjwheel;

            if (!string.IsNullOrEmpty(natCode))
            {
                QueryTireFront = QueryTireFront.Where(Tire => Tire.NATCODE == natCode);
            }

            var QueryTireFrontId = from Tire in QueryTireFront
                                   select Tire.TYRTYREFCD;

            var QueryTireGet = from etgtyres in context.ETGTYRES
                               where QueryTireFrontId.Contains(etgtyres.ID)
                               select etgtyres;

            return QueryTireGet.ToList<ETGTYRES>();

        }

        public static System.Collections.Generic.List<ETGTYRES> GetRearTires(EurotaxEntities context, string natCode)
        {
            var QueryTireFront = from etgjwheel in context.ETGJWHEEL
                                 select etgjwheel;

            if (!string.IsNullOrEmpty(natCode))
            {
                QueryTireFront = QueryTireFront.Where(Tire => Tire.NATCODE == natCode);
            }

            var QueryTireFrontId = from Tire in QueryTireFront
                                   select Tire.TYRTYRERCD;

            var QueryTireGet = from etgtyres in context.ETGTYRES
                               where QueryTireFrontId.Contains(etgtyres.ID)
                               select etgtyres;

            return QueryTireGet.ToList<ETGTYRES>();

        }

        public static System.Collections.Generic.List<ETGRIMS> GetFrontRims(EurotaxEntities context, string natCode)
        {
            var QueryTireFront = from etgjwheel in context.ETGJWHEEL
                                 select etgjwheel;

            if (!string.IsNullOrEmpty(natCode))
            {
                QueryTireFront = QueryTireFront.Where(Rim => Rim.NATCODE == natCode);
            }

            var QueryTireFrontId = from Rim in QueryTireFront
                                   select Rim.RIMRIMFCD;

            var QueryTireGet = from etgrims in context.ETGRIMS
                               where QueryTireFrontId.Contains(etgrims.ID)
                               select etgrims;

            return QueryTireGet.ToList<ETGRIMS>();

        }

        public static System.Collections.Generic.List<ETGRIMS> GetRearRims(EurotaxEntities context, string natCode)
        {
            var QueryTireFront = from etgjwheel in context.ETGJWHEEL
                                 select etgjwheel;

            if (!string.IsNullOrEmpty(natCode))
            {
                QueryTireFront = QueryTireFront.Where(Rim => Rim.NATCODE == natCode);
            }

            var QueryTireFrontId = from Rim in QueryTireFront
                                   select Rim.RIMRIMRCD;

            var QueryTireGet = from etgrims in context.ETGRIMS
                               where QueryTireFrontId.Contains(etgrims.ID)
                               select etgrims;
            
            return QueryTireGet.ToList<ETGRIMS>();

        }

        
        public static System.Collections.Generic.List<ETGRIMS> GetFrontRims(EurotaxEntities context, string natCode, string code)
        {
            int Diameter = GetDiameterFromCode(code);

            string DiameterToQuery = Diameter.ToString() + ".000";
            var QueryTireFront = from etgjwheel in context.ETGJWHEEL
                                 select etgjwheel;

            if (!string.IsNullOrEmpty(natCode))
            {
                QueryTireFront = QueryTireFront.Where(Rim => Rim.NATCODE == natCode);
            }

            var QueryTireFrontId = from Rim in QueryTireFront
                                   select Rim.RIMRIMFCD;

            var QueryTireGet = from etgrims in context.ETGRIMS
                               where (QueryTireFrontId.Contains(etgrims.ID)) && (etgrims.DIAMETER == DiameterToQuery)
                               select etgrims;

            return QueryTireGet.ToList<ETGRIMS>();

        }

        public static System.Collections.Generic.List<ETGRIMS> GetRearRims(EurotaxEntities context, string natCode, string code)
        {
            int Diameter = GetDiameterFromCode(code);
            string DiameterToQuery = Diameter.ToString() + ".000";

            var QueryTireFront = from etgjwheel in context.ETGJWHEEL
                                 select etgjwheel;

            if(!string.IsNullOrEmpty(natCode))
            {
                QueryTireFront = QueryTireFront.Where(Rim => Rim.NATCODE == natCode);
            }

            var QueryTireFrontId = from Rim in QueryTireFront
                                   select Rim.RIMRIMRCD;

            var QueryTireGet = from etgrims in context.ETGRIMS
                               where (QueryTireFrontId.Contains(etgrims.ID)) && (etgrims.DIAMETER == DiameterToQuery)
                               select etgrims;

            return QueryTireGet.ToList<ETGRIMS>();

        }

        public static int GetDiameterFromCode(string code)
        {
            int Diameter = 0;
            string ParsedCode = code.ToUpper().Trim();
            int StartIndex = ParsedCode.IndexOf("R") + 2;
            int Length = ParsedCode.Length - (ParsedCode.IndexOf("R") + 2);

            if (Length < 0)
            {
                Length = Length * -1;
            }
            try
            {
                if (StartIndex + Length <= ParsedCode.Length)
                {
                    ParsedCode = ParsedCode.Substring(StartIndex, Length);
                    int.TryParse(ParsedCode, out Diameter);
                }
            }
            catch
            {
                throw;
            }

            return Diameter;
        }

        

        #endregion
    }
}
