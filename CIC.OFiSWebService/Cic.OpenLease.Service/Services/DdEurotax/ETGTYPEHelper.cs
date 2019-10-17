

namespace Cic.OpenLease.Service.DdEurotax
{
    #region Using
    using System.Linq;
    
    using Cic.OpenOne.Common.Model.DdEurotax;
    using CIC.Database.ET.EF6.Model;
    #endregion
    public static class ETGTYPEHelper
    {
        #region Methods

        public static System.Collections.Generic.List<ETGTYRES> GetFrontTires(DdEurotaxExtended context, string natCode)
        {
            string query = @"select * from (select TYRESFrontADD.*
            from etgtype TYP
		              INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode
                  INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
			            INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
                  INNER JOIN ETGTYRES TYRESFrontADD ON TYRESFrontADD.ID = WheelADD.TYRTyreFCd AND TYRESFRONTADD.VehType = WheelADD.VehType
            where length(crosssec)>1 and typ.NATCODE ='" + natCode + @"' 
 union all select etgtyres.* from etgtyres,etgjwheel where length(crosssec)>1 and etgjwheel.natcode='" + natCode + @"' and etgjwheel.tyrtyrefcd=etgtyres.id)
            order by diameter,CROSSSEC,width";

            System.Collections.Generic.List<ETGTYRES> rval = context.ExecuteStoreQuery<ETGTYRES>(query, null).ToList<ETGTYRES>();
            return rval;


        }

        public static System.Collections.Generic.List<ETGTYRES> GetRearTires(DdEurotaxExtended context, string natCode)
        {
            string query = @"select * from (select TYRESBackADD.*
                from etgtype TYP
		                  INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode
                      INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
			                INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
                       INNER JOIN ETGTYRES TYRESBackADD ON TYRESBackADD.ID = WheelADD.TYRTyreRCd AND TYRESBackADD.VehType = WheelADD.VehType
                where length(crosssec)>1 and typ.NATCODE ='" + natCode + @"' 
            union all select etgtyres.* from etgtyres,etgjwheel where length(crosssec)>1 and etgjwheel.natcode='" + natCode + @"' and etgjwheel.tyrtyrercd=etgtyres.id)
            order by diameter,CROSSSEC,width";
            System.Collections.Generic.List<ETGTYRES> rval = context.ExecuteStoreQuery<ETGTYRES>(query, null).ToList<ETGTYRES>();
            return rval;

        }

        public static System.Collections.Generic.List<ETGRIMS> GetFrontRims(DdEurotaxExtended context, string natCode)
        {
            string query = @"select * from (select RIMSFrontADD.*
from etgtype TYP
		  INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode
      INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
			INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
        INNER JOIN ETGRIMS RIMSFrontADD ON RIMSFrontADD.ID = WheelADD.RIMRimFCd AND RIMSFRONTADD.VehType = WheelADD.VehType
where typ.NATCODE ='" + natCode + @"' and length(diameter)>1
 union all select etgrims.* from etgrims,etgjwheel where etgjwheel.natcode='" + natCode + @"' and etgjwheel.rimrimfcd=etgrims.id)
   
  order by diameter,width";


            System.Collections.Generic.List<ETGRIMS> rval = context.ExecuteStoreQuery<ETGRIMS>(query, null).ToList<ETGRIMS>();
            return rval;



        }

        public static System.Collections.Generic.List<ETGRIMS> GetRearRims(DdEurotaxExtended context, string natCode)
        {
            string query = @"select * from (select RIMSBackADD.*
from etgtype TYP
		  INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode
      INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
			INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
       INNER JOIN ETGRIMS RIMSBackADD ON RIMSBackADD.ID = WheelADD.RIMRimRCd AND RIMSBACKADD.VehType = WheelADD.VehType
where typ.NATCODE ='" + natCode + @"' and length(diameter)>1 
union all select etgrims.* from etgrims,etgjwheel where etgjwheel.natcode='" + natCode + @"' and etgjwheel.rimrimrcd=etgrims.id)
   
  order by diameter,width";
            System.Collections.Generic.List<ETGRIMS> rval = context.ExecuteStoreQuery<ETGRIMS>(query, null).ToList<ETGRIMS>();
            return rval;



        }


        public static System.Collections.Generic.List<ETGRIMS> GetFrontRims(DdEurotaxExtended context, string natCode, string code)
        {
            int Diameter = GetDiameterFromCode(code);

            string DiameterToQuery = Diameter.ToString() + ".000";

            string query = @"select * from (select RIMSFrontADD.*
from etgtype TYP
		  INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode
      INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
			INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
        INNER JOIN ETGRIMS RIMSFrontADD ON RIMSFrontADD.ID = WheelADD.RIMRimFCd AND RIMSFRONTADD.VehType = WheelADD.VehType
where typ.NATCODE ='" + natCode + @"'   
union all select etgrims.* from etgrims,etgjwheel where etgjwheel.natcode='" + natCode + @"' and etgjwheel.rimrimfcd=etgrims.id)
   where diameter='" + DiameterToQuery + @"'
  order by diameter,width";


            System.Collections.Generic.List<ETGRIMS> rval = context.ExecuteStoreQuery<ETGRIMS>(query, null).ToList<ETGRIMS>();
            return rval;



        }

        public static System.Collections.Generic.List<ETGRIMS> GetRearRims(DdEurotaxExtended context, string natCode, string code)
        {
            int Diameter = GetDiameterFromCode(code);
            string DiameterToQuery = Diameter.ToString() + ".000";
            string query = @"select * from (select RIMSBackADD.* from etgtype TYP INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode 
 INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
 INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
 INNER JOIN ETGRIMS RIMSBackADD ON RIMSBackADD.ID = WheelADD.RIMRimRCd AND RIMSBACKADD.VehType = WheelADD.VehType
where typ.NATCODE ='" + natCode + @"'  
union all select etgrims.* from etgrims,etgjwheel where etgjwheel.natcode='" + natCode + @"' and etgjwheel.rimrimrcd=etgrims.id)
   where diameter='" + DiameterToQuery + @"'
  order by diameter,width";

            System.Collections.Generic.List<ETGRIMS> rval = context.ExecuteStoreQuery<ETGRIMS>(query, null).ToList<ETGRIMS>();
            return rval;

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
