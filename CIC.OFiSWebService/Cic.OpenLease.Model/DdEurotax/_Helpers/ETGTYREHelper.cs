

namespace Cic.OpenLease.Model.DdEurotax
{
    #region Using
    using System.Linq;
    #endregion
    public static class ETGTYREHelper
    {
        public static string GetCode(ETGTYRES etgtyres)
        {
            string Width = etgtyres.WIDTH;
            string Crosssec = etgtyres.CROSSSEC;
            string Design = etgtyres.DESIGN;
            string Diameter = etgtyres.DIAMETER;

            if (Width.Contains("."))
            {
                Width = Width.Substring(0, Width.LastIndexOf("."));
            }

            if (Crosssec.Contains("."))
            {
                Crosssec = Crosssec.Substring(0, Crosssec.LastIndexOf("."));
            }

            if (Diameter.Contains("."))
            {
                Diameter = Diameter.Substring(0, Diameter.LastIndexOf("."));
            }

            string Code = Width + "/" + Crosssec + " " + Design + " " + Diameter;
            return Code;
        }



        public static string GetCodeWithR(string width, string crosssec, string diameter)
        {

            if (width.Contains("."))
            {
                width = width.Substring(0, width.LastIndexOf("."));
            }

            if (crosssec.Contains("."))
            {
                crosssec = crosssec.Substring(0, crosssec.LastIndexOf("."));
            }

            if (diameter.Contains("."))
            {
                diameter = diameter.Substring(0, diameter.LastIndexOf("."));
            }

            string Code = width + "/" + crosssec + " " + "R" + " " + diameter;
            return Code;
        }

    }
}