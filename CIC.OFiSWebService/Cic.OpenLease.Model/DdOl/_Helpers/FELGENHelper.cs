// OWNER WB, 12-05-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System.Linq;
    #endregion
    
    [System.CLSCompliant(true)]
    public static class FELGENHelper
    {
        #region Methods
        public static System.Collections.Generic.List<FELGEN> GetFelgen(OlEntities context)
        {
            var Query = from felgen in context.FELGEN
                        select felgen;
            
            return Query.ToList<FELGEN>();
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

        public static System.Collections.Generic.List<FELGEN> GetFelgenFromCode(OlExtendedEntities context, string code)
        {
            int Diameter = GetDiameterFromCode(code);
            System.Collections.Generic.List<FELGEN> FELGENList = null;

            try
            {
                var QueryFelgtyp = from felgtyp in context.FELGTYP
                                   where felgtyp.DURCHMESSER == Diameter
                                   select felgtyp.SYSFELGTYP;

                var Query = from felgen in context.FELGEN
                            where QueryFelgtyp.Contains(felgen.SYSFELGTYP.Value)
                            select felgen;
                FELGENList = Query.ToList();
            }
            catch
            {
                throw;
            }

            return FELGENList;
        }

        public static decimal GetWidth(long? sysFelgtyp)
        {
            decimal Width;

            using (OlExtendedEntities Context = new OlExtendedEntities())
            {
                try
                {
                    var Query = from felgtyp in Context.FELGTYP
                                       where felgtyp.SYSFELGTYP == sysFelgtyp
                                       select felgtyp.BREITE;
                    Width = Query.FirstOrDefault().GetValueOrDefault();
                }
                catch
                {
                    throw;
                }

            }

            return Width;
        }

        public static decimal GetDiameter(long? sysFelgtyp)
        {
            decimal Width;

            using (OlExtendedEntities Context = new OlExtendedEntities())
            {
                try
                {
                    var Query = from felgtyp in Context.FELGTYP
                                where felgtyp.SYSFELGTYP == sysFelgtyp
                                select felgtyp.DURCHMESSER;
                    Width = Query.FirstOrDefault().GetValueOrDefault();
                }
                catch
                {
                    throw;
                }

            }

            return Width;
        }
        #endregion


    }
}
