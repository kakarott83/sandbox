// OWNER MK, 22-05-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using directives
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class VGHelper
    {
        public static decimal DeliverValue(OlExtendedEntities olExtendedEntities, long sysVG, decimal x, decimal y)
        {
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            // TODO MK 0 MK, Values in lines, columns and cells are not strongly typed, for now i presume they are double
            //// Get VG Type
            //var VgQuery = from vg in olExtendedEntities.VG
            //              where vg.SYSVG == sysVG
            //              select vg;
            //VG Vg = VgQuery.FirstOrDefault();

            //if (Vg != null)
            //{
            //    if (!Vg.VGTYPEReference.IsLoaded)
            //    {
            //        Vg.VGTYPEReference.Load();
            //    }


            //    if (Vg.VGTYPE != null)
            //    {
            //        var VgUnitXQuery = from vgUnit in olExtendedEntities.VGUNIT
            //                           where vgUnit.SYSVGUNIT == Vg.VGTYPE.SYSVGUNITX
            //                           select vgUnit;

            //        VGUNIT VgUnitX = VgUnitXQuery.FirstOrDefault();

            //        var VgUnitYQuery = from vgUnit in olExtendedEntities.VGUNIT
            //                           where vgUnit.SYSVGUNIT == Vg.VGTYPE.SYSVGUNITY
            //                           select vgUnit;

                    
            //        VGUNIT VgUnitY = VgUnitYQuery.FirstOrDefault();

            //        if (VgUnitX != null && VgUnitY != null)
            //        {
                        
            //        }
            //    }
            //}

            // Get valid group id
            var VgValidQuery = from vgValid in olExtendedEntities.VGVALID
                               where vgValid.VG.SYSVG == sysVG &&
                               vgValid.VALIDFROM < System.DateTime.Now &&
                               vgValid.VALIDUNTIL > System.DateTime.Now
                               orderby vgValid.VALIDFROM ascending
                               select vgValid.SYSVGVALID;
            long SysVgValid = VgValidQuery.FirstOrDefault();

            if (SysVgValid != 0)
            {
                // Get lines
                var VgLineQuery = from vgLine in olExtendedEntities.VGLINE
                                  where vgLine.VGVALID.SYSVGVALID == SysVgValid
                                  select vgLine;

                System.Collections.Generic.List<VGLINE> VgLineList = VgLineQuery.ToList<VGLINE>();
                System.Collections.Generic.Dictionary<long, decimal> VgLineDictionary = new System.Collections.Generic.Dictionary<long, decimal>();
                foreach (VGLINE VgLineLoop in VgLineList)
                {
                    decimal Value = 0.0M;
                    decimal.TryParse(VgLineLoop.SCALEVALUE, out Value);
                    VgLineDictionary.Add(VgLineLoop.SYSVGLINE, Value);
                }

                var VgLineQuery1 = from v in VgLineDictionary
                                   where v.Value <= x
                                   orderby v.Value descending
                                   select v;

                // Get line 1 SysVgLine/Value pair
                System.Collections.Generic.KeyValuePair<long, decimal> VgLine1 = VgLineQuery1.FirstOrDefault();

                var VgLineQuery2 = from v in VgLineDictionary
                                   where v.Value >= x
                                   orderby v.Value ascending
                                   select v;

                // Get line 2 SysVgLine/Value pair
                System.Collections.Generic.KeyValuePair<long, decimal> VgLine2 = VgLineQuery2.FirstOrDefault();

                // Get columns
                var VgClmnQuery = from vgClmn in olExtendedEntities.VGCLMN
                                  where vgClmn.VGVALID.SYSVGVALID == SysVgValid
                                  select vgClmn;

                System.Collections.Generic.List<VGCLMN> VgClmnList = VgClmnQuery.ToList<VGCLMN>();
                System.Collections.Generic.Dictionary<long, decimal> VgClmnDictionary = new System.Collections.Generic.Dictionary<long, decimal>();
                foreach (VGCLMN VgClmnLoop in VgClmnList)
                {
                    decimal Value = 0.0M;
                    decimal.TryParse(VgClmnLoop.SCALEVALUE, out Value);
                    VgClmnDictionary.Add(VgClmnLoop.SYSVGCLMN, Value);
                }

                var VgClmnQuery1 = from v in VgClmnDictionary
                                   where v.Value <= y
                                   orderby v.Value descending
                                   select v;

                // Get column 1 SysVgLine/Value pair
                System.Collections.Generic.KeyValuePair<long, decimal> VgClmn1 = VgClmnQuery1.FirstOrDefault();

                var VgClmnQuery2 = from v in VgClmnDictionary
                                   where v.Value >= y
                                   orderby v.Value ascending
                                   select v;

                // Get line 2 SysVgLine/Value pair
                System.Collections.Generic.KeyValuePair<long, decimal> VgClmn2 = VgClmnQuery2.FirstOrDefault();

                // Get the values
                var VgCell1Query = from vgCell in olExtendedEntities.VGCELL
                                   where
                                   vgCell.VGLINE.SYSVGLINE == VgLine1.Key
                                   && vgCell.VGCLMN.SYSVGCLMN == VgClmn1.Key
                                   select vgCell.VALUE;

                decimal VgLine1Clmn1Cell = VgCell1Query.FirstOrDefault().GetValueOrDefault(0);

                var VgCell2Query = from vgCell in olExtendedEntities.VGCELL
                                   where
                                   vgCell.VGLINE.SYSVGLINE == VgLine2.Key
                                   && vgCell.VGCLMN.SYSVGCLMN == VgClmn1.Key
                                   select vgCell.VALUE;

                decimal VgLine2Clmn1Cell = VgCell2Query.FirstOrDefault().GetValueOrDefault(0);

                var VgCell3Query = from vgCell in olExtendedEntities.VGCELL
                                   where
                                   vgCell.VGLINE.SYSVGLINE == VgLine1.Key
                                   && vgCell.VGCLMN.SYSVGCLMN == VgClmn2.Key
                                   select vgCell.VALUE;

                decimal VgLine1Clmn2Cell = VgCell3Query.FirstOrDefault().GetValueOrDefault(0);

                var VgCell4Query = from vgCell in olExtendedEntities.VGCELL
                                   where
                                   vgCell.VGLINE.SYSVGLINE == VgLine2.Key
                                   && vgCell.VGCLMN.SYSVGCLMN == VgClmn2.Key
                                   select vgCell.VALUE;

                decimal VgLine2Clmn2Cell = VgCell4Query.FirstOrDefault().GetValueOrDefault(0);

                //string s = ((System.Data.Objects.ObjectQuery)VgCell1Query).ToTraceString();

                // Interpolate the result
                return MyInterpolate(VgClmn1.Value, VgClmn2.Value, VgLine1Clmn1Cell, VgLine1Clmn2Cell, y,
                    VgLine1.Value, VgLine2.Value, VgLine2Clmn1Cell, VgLine2Clmn2Cell, x);
            }

            return 0.0M;
        }

        #region My methods
        private static decimal MyInterpolate(
            decimal x0, decimal x1, decimal fx0, decimal fx1, decimal x,
            decimal y0, decimal y1, decimal fy0, decimal fy1, decimal y)
        {
            if (x0 != x1)
            {
                if (y0 == y1)
                {
                    return MyInterpolate(x0, x1, fx0, fy0, x);
                }
                decimal i0 = MyInterpolate(x0, x1, fx0, fx1, x);
                decimal i1 = MyInterpolate(x0, x1, fy0, fy1, x);
                return MyInterpolate(y0, y1, i0, i1, y);
            }
            else if (y0 != y1)
            {
                return MyInterpolate(y0, y1, fx0, fy0, y);
            }
            else
            {
                return x0;
            }

        }

        private static decimal MyInterpolate(decimal x0, decimal x1, decimal fx0, decimal fx1, decimal x)
        {
            return fx0 + ((fx1 - fx0) / (x1 - x0)) * (x - x0);
        }
        #endregion
    }

}
