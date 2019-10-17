using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;  
using System.Text;
using System.Globalization;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    [ImmutableObject(true)]


    public sealed class OrderAttribute : Attribute
    {
        private readonly int order;
        private readonly bool active;
        public int Order { get { return order; } }
        public bool Active { get { return active; } }
        public OrderAttribute(int order, bool active) { this.order = order; this.active = active; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstarctRISKEWBS1DataDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            CultureInfo cul = new CultureInfo("de-DE");

            foreach (var prop in GetSortedProperties())
            {
                if (((OrderAttribute)prop.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Active == true)
                {
                    object val = prop.GetValue(this, null);
                    if (val != null)
                    {
                        if (prop.PropertyType == typeof(DateTime))
                            result.Append(((DateTime)val).ToString("o", cul));
                        else if (prop.PropertyType == typeof(DateTime?))
                            result.Append(((DateTime)val).ToString("o", cul));
                        else if (prop.PropertyType == typeof(decimal?))
                            result.Append(((decimal)val).ToString(cul));
                        else
                            result.Append(val.ToString());
                    }
                    result.Append("\t");
                }
            }
            result.Append("\v");
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IOrderedEnumerable<PropertyInfo> GetSortedProperties()
        {
            return this.GetType().GetProperties().OrderBy(p => ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class S1DataPack : List<AbstarctRISKEWBS1DataDto>
    {
        public override string ToString()
        {
            string outStr = String.Empty;

            foreach (AbstarctRISKEWBS1DataDto dto in this)
            {
                outStr = outStr + dto.ToString() + "\v";
            }
            return outStr;
        }

        public static object GetValue(string inStr, Type inType)
        {
            CultureInfo cul = new CultureInfo("de-DE");

            if (inStr == String.Empty || inStr.Trim() == String.Empty )
                return null;

            try
            {
                if (inType == typeof(string))
                    return inStr;
                else if (inType == typeof(decimal))
                    return decimal.Parse(inStr, cul);
                else if (inType == typeof(DateTime))
                    return DateTime.Parse(inStr, cul);
                else if (inType == typeof(DateTime?))
                    return DateTime.Parse(inStr, cul);
            }
            catch { }

            return null;
        }

        private static Dictionary<PropertyInfo, bool> orderAttCache = new Dictionary<PropertyInfo, bool>();

        public static bool isOrderAttributeActive(PropertyInfo pInfo)
        {
            if (!orderAttCache.ContainsKey(pInfo))
            {
                orderAttCache[pInfo] = (((OrderAttribute)pInfo.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Active == true);
            }
            return orderAttCache[pInfo];
        }

        private static Dictionary<Type, PropertyInfo[]> propCache = new Dictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] GetSortedProperties<T>()
        {
            Type t = typeof(T);
            if (!propCache.ContainsKey(t))
                propCache[t] = t.GetProperties().OrderBy(p => ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order).ToArray();
            return propCache[t];
        }
        /*public static IOrderedEnumerable<PropertyInfo> GetSortedProperties<T>()
        {
            return typeof(T).GetProperties().OrderBy(p => ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order);
        }

        public IOrderedEnumerable<PropertyInfo> GetSortedProperties()
        {
            return this.GetType().GetProperties().OrderBy(p => ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order);
        }*/
    }

    /// <summary>
    /// Ausgehende Daten zu SimpleService, befüllt über Query
    /// </summary>
    public class RISKEWBS1DataDto : AbstarctRISKEWBS1DataDto
    {
        [Order(1, true)]
        public string InquiryCode { get; set; }
        [Order(2, true)]
        public string OrganizationCode { get; set; }
        [Order(3, true)]
        public string DecisionProcessCode { get; set; }
        [Order(4, true)]
        public decimal? ProcessVersion { get; set; }
        [Order(5, true)]
        public decimal? CallNumber { get; set; }
        [Order(6, true)]
        public decimal? RandomNumber { get; set; }
        [Order(7, true)]
        public DateTime? InquiryDate { get; set; }
        [Order(8, true)]
        public string InquiryTime { get; set; }
        [Order(9, true)]
        public string I_C_VARCHAR_F01 { get; set; }
        [Order(10, true)]
        public string I_C_VARCHAR_F02 { get; set; }
        [Order(11, true)]
        public string I_C_VARCHAR_F03 { get; set; }
        [Order(12, true)]
        public string I_C_VARCHAR_F04 { get; set; }
        [Order(13, true)]
        public string I_C_VARCHAR_F05 { get; set; }
        [Order(14, true)]
        public string I_C_VARCHAR_F06 { get; set; }
        [Order(15, true)]
        public string I_C_VARCHAR_F07 { get; set; }
        [Order(16, true)]
        public string I_C_VARCHAR_F08 { get; set; }
        [Order(17, true)]
        public string I_C_VARCHAR_F09 { get; set; }
        [Order(18, true)]
        public string I_C_VARCHAR_F10 { get; set; }
        [Order(19, true)]
        public string I_C_VARCHAR_F11 { get; set; }
        [Order(20, true)]
        public string I_C_VARCHAR_F12 { get; set; }
        [Order(21, true)]
        public string I_C_VARCHAR_F13 { get; set; }
        [Order(22, true)]
        public string I_C_VARCHAR_F14 { get; set; }
        [Order(23, true)]
        public string I_C_VARCHAR_F15 { get; set; }
        [Order(24, true)]
        public DateTime? I_C_DATE_F16 { get; set; }
        [Order(25, true)]
        public DateTime? I_C_DATE_F17 { get; set; }
        [Order(26, true)]
        public DateTime? I_C_DATE_F18 { get; set; }
        [Order(27, true)]
        public DateTime? I_C_DATE_F19 { get; set; }
        [Order(28, true)]
        public DateTime? I_C_DATE_F20 { get; set; }
        [Order(29, true)]
        public DateTime? I_C_DATE_F21 { get; set; }
        [Order(30, true)]
        public DateTime? I_C_DATE_F22 { get; set; }
        [Order(31, true)]
        public DateTime? I_C_DATE_F23 { get; set; }
        [Order(32, true)]
        public DateTime? I_C_DATE_F24 { get; set; }
        [Order(33, true)]
        public DateTime? I_C_DATE_F25 { get; set; }
        [Order(34, true)]
        public DateTime? I_C_DATE_F26 { get; set; }
        [Order(35, true)]
        public DateTime? I_C_DATE_F27 { get; set; }
        [Order(36, true)]
        public DateTime? I_C_DATE_F28 { get; set; }
        [Order(37, true)]
        public DateTime? I_C_DATE_F29 { get; set; }
        [Order(38, true)]
        public DateTime? I_C_DATE_F30 { get; set; }
        [Order(39, true)]
        public DateTime? I_C_DATE_F31 { get; set; }
        [Order(40, true)]
        public DateTime? I_C_DATE_F32 { get; set; }
        [Order(41, true)]
        public DateTime? I_C_DATE_F33 { get; set; }
        [Order(42, true)]
        public DateTime? I_C_DATE_F34 { get; set; }
        [Order(43, true)]
        public DateTime? I_C_DATE_F35 { get; set; }
        [Order(44, true)]
        public decimal? I_C_NUMBER_152_F36 { get; set; }
        [Order(45, true)]
        public decimal? I_C_NUMBER_152_F37 { get; set; }
        [Order(46, true)]
        public decimal? I_C_NUMBER_152_F38 { get; set; }
        [Order(47, true)]
        public decimal? I_C_NUMBER_152_F39 { get; set; }
        [Order(48, true)]
        public decimal? I_C_NUMBER_152_F40 { get; set; }
        [Order(49, true)]
        public decimal? I_C_NUMBER_152_F41 { get; set; }
        [Order(50, true)]
        public decimal? I_C_NUMBER_152_F42 { get; set; }
        [Order(51, true)]
        public decimal? I_C_NUMBER_152_F43 { get; set; }
        [Order(52, true)]
        public decimal? I_C_NUMBER_152_F44 { get; set; }
        [Order(53, true)]
        public decimal? I_C_NUMBER_152_F45 { get; set; }
        [Order(54, true)]
        public decimal? I_C_NUMBER_152_F46 { get; set; }
        [Order(55, true)]
        public decimal? I_C_NUMBER_152_F47 { get; set; }
        [Order(56, true)]
        public decimal? I_C_NUMBER_152_F48 { get; set; }
        [Order(57, true)]
        public decimal? I_C_NUMBER_152_F49 { get; set; }
        [Order(58, true)]
        public decimal? I_C_NUMBER_152_F50 { get; set; }
        [Order(59, true)]
        public decimal? I_C_NUMBER_152_F51 { get; set; }
        [Order(60, true)]
        public decimal? I_C_NUMBER_152_F52 { get; set; }
        [Order(61, true)]
        public decimal? I_C_NUMBER_152_F53 { get; set; }
        [Order(62, true)]
        public decimal? I_C_NUMBER_152_F54 { get; set; }
        [Order(63, true)]
        public decimal? I_C_NUMBER_152_F55 { get; set; }
        [Order(64, true)]
        public decimal? I_C_NUMBER_152_F56 { get; set; }
        [Order(65, true)]
        public decimal? I_C_NUMBER_152_F57 { get; set; }
        [Order(66, true)]
        public decimal? I_C_NUMBER_152_F58 { get; set; }
        [Order(67, true)]
        public decimal? I_C_NUMBER_152_F59 { get; set; }
        [Order(68, true)]
        public decimal? I_C_NUMBER_152_F60 { get; set; }
        [Order(69, true)]
        public decimal? I_C_NUMBER_120_F61 { get; set; }
        [Order(70, true)]
        public decimal? I_C_NUMBER_120_F62 { get; set; }
        [Order(71, true)]
        public decimal? I_C_NUMBER_120_F63 { get; set; }
        [Order(72, true)]
        public decimal? I_C_NUMBER_120_F64 { get; set; }
        [Order(73, true)]
        public decimal? I_C_NUMBER_120_F65 { get; set; }
        [Order(74, true)]
        public decimal? I_C_NUMBER_120_F66 { get; set; }
        [Order(75, true)]
        public decimal? I_C_NUMBER_120_F67 { get; set; }
        [Order(76, true)]
        public decimal? I_C_NUMBER_120_F68 { get; set; }
        [Order(77, true)]
        public decimal? I_C_NUMBER_120_F69 { get; set; }
        [Order(78, true)]
        public decimal? I_C_NUMBER_120_F70 { get; set; }
        [Order(79, true)]
        public decimal? I_C_NUMBER_030_F71 { get; set; }
        [Order(80, true)]
        public decimal? I_C_NUMBER_030_F72 { get; set; }
        [Order(81, true)]
        public decimal? I_C_NUMBER_030_F73 { get; set; }
        [Order(82, true)]
        public decimal? I_C_NUMBER_030_F74 { get; set; }
        [Order(83, true)]
        public decimal? I_C_NUMBER_030_F75 { get; set; }
        [Order(84, true)]
        public decimal? I_C_NUMBER_030_F76 { get; set; }
        [Order(85, true)]
        public decimal? I_C_NUMBER_030_F77 { get; set; }
        [Order(86, true)]
        public decimal? I_C_NUMBER_030_F78 { get; set; }
        [Order(87, true)]
        public decimal? I_C_NUMBER_030_F79 { get; set; }
        [Order(88, true)]
        public decimal? I_C_NUMBER_030_F80 { get; set; }



        //CR 453
        [Order(89, true)] public DateTime I_C_DATE_F81 { get; set; }
        [Order(90, true)] public DateTime I_C_DATE_F82 { get; set; }
        [Order(91, true)] public DateTime I_C_DATE_F83 { get; set; }
        [Order(92, true)] public DateTime I_C_DATE_F84 { get; set; }
        [Order(93, true)] public DateTime I_C_DATE_F85 { get; set; }
        [Order(94, true)] public DateTime I_C_DATE_F86 { get; set; }
        [Order(95, true)] public DateTime I_C_DATE_F87 { get; set; }
        [Order(96, true)] public DateTime I_C_DATE_F88 { get; set; }
        [Order(97, true)] public DateTime I_C_DATE_F89 { get; set; }
        [Order(98, true)] public DateTime I_C_DATE_F90 { get; set; }
        [Order(99, true)] public decimal? I_C_NUMBER_120_F91 { get; set; }
        [Order(100, true)] public decimal? I_C_NUMBER_120_F92 { get; set; }
        [Order(101, true)] public decimal? I_C_NUMBER_120_F93 { get; set; }
        [Order(102, true)] public decimal? I_C_NUMBER_120_F94 { get; set; }
        [Order(103, true)] public decimal? I_C_NUMBER_120_F95 { get; set; }
        [Order(104, true)] public decimal? I_C_NUMBER_120_F96 { get; set; }
        [Order(105, true)] public decimal? I_C_NUMBER_120_F97 { get; set; }
        [Order(106, true)] public decimal? I_C_NUMBER_120_F98 { get; set; }
        [Order(107, true)] public decimal? I_C_NUMBER_120_F99 { get; set; }
        [Order(108, true)] public decimal? I_C_NUMBER_120_F100 { get; set; }
        [Order(109, true)] public decimal? I_C_NUMBER_120_F101 { get; set; }
        [Order(110, true)] public decimal? I_C_NUMBER_120_F102 { get; set; }
        [Order(111, true)] public decimal? I_C_NUMBER_120_F103 { get; set; }
        [Order(112, true)] public decimal? I_C_NUMBER_120_F104 { get; set; }
        [Order(113, true)] public decimal? I_C_NUMBER_120_F105 { get; set; }
        [Order(114, true)] public decimal? I_C_NUMBER_120_F106 { get; set; }
        [Order(115, true)] public decimal? I_C_NUMBER_120_F107 { get; set; }
        [Order(116, true)] public decimal? I_C_NUMBER_120_F108 { get; set; }
        [Order(117, true)] public decimal? I_C_NUMBER_120_F109 { get; set; }
        [Order(118, true)] public decimal? I_C_NUMBER_120_F110 { get; set; }
        [Order(119, true)] public decimal? I_C_NUMBER_030_F111 { get; set; }
        [Order(120, true)] public decimal? I_C_NUMBER_030_F112 { get; set; }
        [Order(121, true)] public decimal? I_C_NUMBER_030_F113 { get; set; }
        [Order(122, true)] public decimal? I_C_NUMBER_030_F114 { get; set; }
        [Order(123, true)] public decimal? I_C_NUMBER_030_F115 { get; set; }
        [Order(124, true)] public decimal? I_C_NUMBER_030_F116 { get; set; }
        [Order(125, true)] public decimal? I_C_NUMBER_030_F117 { get; set; }
        [Order(126, true)] public decimal? I_C_NUMBER_030_F118 { get; set; }
        [Order(127, true)] public decimal? I_C_NUMBER_030_F119 { get; set; }
        [Order(128, true)] public decimal? I_C_NUMBER_030_F120 { get; set; }

        public RISKEWBS1DataDto()
        {
        }
        /*public RISKEWBS1DataDto(RISKEWBS1DBDto S1DBDto)
        {
            InquiryCode      = S1DBDto.InquiryCode;
            OrganizationCode = S1DBDto.OrganizationCode; // "BNOW"
            DecisionProcessCode = "RATINGJOB";
            ProcessVersion = Convert.ToDecimal(S1DBDto.ProcessVersion);
            CallNumber = 1;
            RandomNumber = 1;
            if (S1DBDto.InquiryDate.HasValue)
            {
                InquiryDate = (DateTime)S1DBDto.InquiryDate;
            }
            InquiryTime = S1DBDto.InquiryTime;

            I_C_VARCHAR_F01 = S1DBDto.Str01;
            I_C_VARCHAR_F02 = S1DBDto.Str02;
            I_C_VARCHAR_F03 = S1DBDto.Str03;
            I_C_VARCHAR_F04 = S1DBDto.Str04;
            I_C_VARCHAR_F05 = S1DBDto.Str05;
            I_C_VARCHAR_F06 = S1DBDto.Str06;
            I_C_VARCHAR_F07 = S1DBDto.Str07;
            I_C_VARCHAR_F08 = S1DBDto.Str08;
            I_C_VARCHAR_F09 = S1DBDto.Str09;
            I_C_VARCHAR_F10 = S1DBDto.Str10;
            I_C_VARCHAR_F11 = S1DBDto.Str11;
            I_C_VARCHAR_F12 = S1DBDto.Str12;
            I_C_VARCHAR_F13 = S1DBDto.Str13;
            I_C_VARCHAR_F14 = S1DBDto.Str14;
            I_C_VARCHAR_F15 = S1DBDto.Str15;

            I_C_DATE_F16 = S1DBDto.Dat01;
            I_C_DATE_F17 = S1DBDto.Dat02;
            I_C_DATE_F18 = S1DBDto.Dat03;
            I_C_DATE_F19 = S1DBDto.Dat04;
            I_C_DATE_F20 = S1DBDto.Dat05;
            I_C_DATE_F21 = S1DBDto.Dat06;
            I_C_DATE_F22 = S1DBDto.Dat07;
            I_C_DATE_F23 = S1DBDto.Dat08;
            I_C_DATE_F24 = S1DBDto.Dat09;
            I_C_DATE_F25 = S1DBDto.Dat10;
            I_C_DATE_F26 = S1DBDto.Dat11;
            I_C_DATE_F27 = S1DBDto.Dat12;
            I_C_DATE_F28 = S1DBDto.Dat13;
            I_C_DATE_F29 = S1DBDto.Dat14;
            I_C_DATE_F30 = S1DBDto.Dat15;
            I_C_DATE_F31 = S1DBDto.Dat16;
            I_C_DATE_F32 = S1DBDto.Dat17;
            I_C_DATE_F33 = S1DBDto.Dat18;
            I_C_DATE_F34 = S1DBDto.Dat19;
            I_C_DATE_F35 = S1DBDto.Dat20;

            I_C_NUMBER_152_F36 = S1DBDto.Pdec1501;
            I_C_NUMBER_152_F37 = S1DBDto.Pdec1502;
            I_C_NUMBER_152_F38 = S1DBDto.Pdec1503;
            I_C_NUMBER_152_F39 = S1DBDto.Pdec1504;
            I_C_NUMBER_152_F40 = S1DBDto.Pdec1505;
            I_C_NUMBER_152_F41 = S1DBDto.Pdec1506;
            I_C_NUMBER_152_F42 = S1DBDto.Pdec1507;
            I_C_NUMBER_152_F43 = S1DBDto.Pdec1508;
            I_C_NUMBER_152_F44 = S1DBDto.Pdec1509;
            I_C_NUMBER_152_F45 = S1DBDto.Pdec1510;
            I_C_NUMBER_152_F46 = S1DBDto.Pdec1511;
            I_C_NUMBER_152_F47 = S1DBDto.Pdec1512;
            I_C_NUMBER_152_F48 = S1DBDto.Pdec1513;
            I_C_NUMBER_152_F49 = S1DBDto.Pdec1514;
            I_C_NUMBER_152_F50 = S1DBDto.Pdec1515;
            I_C_NUMBER_152_F51 = S1DBDto.Pdec1516;
            I_C_NUMBER_152_F52 = S1DBDto.Pdec1517;
            I_C_NUMBER_152_F53 = S1DBDto.Pdec1518;
            I_C_NUMBER_152_F54 = S1DBDto.Pdec1519;
            I_C_NUMBER_152_F55 = S1DBDto.Pdec1520;
            I_C_NUMBER_152_F56 = S1DBDto.Pdec1521;
            I_C_NUMBER_152_F57 = S1DBDto.Pdec1522;
            I_C_NUMBER_152_F58 = S1DBDto.Pdec1523;
            I_C_NUMBER_152_F59 = S1DBDto.Pdec1524;
            I_C_NUMBER_152_F60 = S1DBDto.Pdec1525;

            I_C_NUMBER_120_F61 = S1DBDto.Int01;
            I_C_NUMBER_120_F62 = S1DBDto.Int02;
            I_C_NUMBER_120_F63 = S1DBDto.Int03;
            I_C_NUMBER_120_F64 = S1DBDto.Int04;
            I_C_NUMBER_120_F65 = S1DBDto.Int05;
            I_C_NUMBER_120_F66 = S1DBDto.Int06;
            I_C_NUMBER_120_F67 = S1DBDto.Int07;
            I_C_NUMBER_120_F68 = S1DBDto.Int08;
            I_C_NUMBER_120_F69 = S1DBDto.Int09;
            I_C_NUMBER_120_F70 = S1DBDto.Int10;

            I_C_NUMBER_030_F71 = S1DBDto.Flag01;
            I_C_NUMBER_030_F72 = S1DBDto.Flag02;
            I_C_NUMBER_030_F73 = S1DBDto.Flag03;
            I_C_NUMBER_030_F74 = S1DBDto.Flag04;
            I_C_NUMBER_030_F75 = S1DBDto.Flag05;
            I_C_NUMBER_030_F76 = S1DBDto.Flag06;
            I_C_NUMBER_030_F77 = S1DBDto.Flag07;
            I_C_NUMBER_030_F78 = S1DBDto.Flag08;
            I_C_NUMBER_030_F79 = S1DBDto.Flag09;
            I_C_NUMBER_030_F80 = S1DBDto.Flag10;
        }*/

    }

    /// <summary>
    /// Definiert die Rückantwortstruktur von S1 (von SimpleService)
    /// 
    /// </summary>
    public class S1GetResponseDto : AbstarctRISKEWBS1DataDto
    {
        [Order(1, true)]
        public string InquiryCode { get; set; }
        [Order(2, true)]
        public string OrganizationCode { get; set; }
        [Order(3, true)]
        public string DecisionProcessCode { get; set; }
        [Order(4, true)]
        public decimal ProcessVersion { get; set; }
        [Order(5, true)]
        public decimal LayoutVersion { get; set; }
        [Order(6, true)]
        public decimal RandomNumber { get; set; }
        [Order(7, true)]
        public DateTime InquiryDate { get; set; }
        [Order(8, true)]
        public string InquiryTime { get; set; }
        [Order(9, true)]
        public decimal O_ErrorCode { get; set; }
        [Order(10, true)]
        public string O_ErrorDescription { get; set; }
        [Order(11, true)]
        public decimal O_Reserve_Number_1 { get; set; }
        [Order(12, true)]
        public decimal O_Reserve_Number_2 { get; set; }
        [Order(13, true)]
        public decimal O_Reserve_Number_3 { get; set; }
        [Order(14, true)]
        public DateTime O_Reserve_Date_1 { get; set; }
        [Order(15, true)]
        public DateTime O_Reserve_Date_2 { get; set; }
        [Order(16, true)]
        public DateTime O_Reserve_Date_3 { get; set; }
        [Order(17, true)]
        public string O_Reserve_Text_1 { get; set; }
        [Order(18, true)]
        public string O_Reserve_Text_2 { get; set; }
        [Order(19, true)]
        public string O_Reserve_Text_3 { get; set; }
        [Order(20, true)]
        public decimal O_sysVT { get; set; }
        [Order(21, true)]
        public decimal O_syskd { get; set; }
        [Order(22, true)]
        public decimal O_DEC_Risikoklasse_VT { get; set; }
        [Order(23, true)]
        public decimal O_DEC_Kapital_WB { get; set; }
        [Order(24, true)]
        public decimal O_DEC_Zins_WB { get; set; }
        [Order(25, true)]
        public decimal O_DEC_Kapital_WB_ACP { get; set; }
        [Order(26, true)]
        public decimal O_DEC_Zins_WB_ACP { get; set; }
        [Order(27, true)]
        public decimal O_DEC_Parameter_ID { get; set; }
        [Order(28, true)]
        public decimal O_DEC_PDt1 { get; set; }
        [Order(29, true)]
        public decimal O_DEC_PDt1_frac { get; set; }
        [Order(30, true)]
        public DateTime S1Error_InquiryDate { get; set; }
        [Order(31, true)]
        public string S1Error_Code { get; set; }
        [Order(32, true)]
        public string S1Error_Description { get; set; }
        [Order(33, true)]
        public string S1Error_EngineVersion { get; set; }
        [Order(34, true)]
        public string S1Error_EngineStackTrace { get; set; }
        [Order(35, true)]
        public string S1Error_JavaStackTrace { get; set; }

        //2019 CR 453
        [Order(36, true)] public decimal O_DEC_LGD { get; set; }
        [Order(37, true)] public decimal O_DEC_EAD { get; set; }
        [Order(38, true)] public decimal O_DEC_PD_LT { get; set; }
        [Order(39, true)] public decimal O_DEC_Risikoklasse_LT { get; set; }
        [Order(40, true)] public decimal O_DEC_Kapital_WB_LT { get; set; }
        [Order(41, true)] public decimal O_DEC_Zins_WB_LT { get; set; }
        [Order(42, true)] public string O_DEC_Scorebezeichnung { get; set; }
        [Order(43, true)] public string O_DEC_Scoreversion { get; set; }
        [Order(44, true)] public decimal O_DEC_Scorewert { get; set; }
        [Order(45, true)] public decimal O_SC_ID_1 { get; set; }
        [Order(46, true)] public string O_SC_Scoretyp_1 { get; set; }
        [Order(47, true)] public string O_SC_Bezeichnung_1 { get; set; }
        [Order(48, true)] public decimal O_SC_Resultatwert_1 { get; set; }
        [Order(49, true)] public string O_SC_Eingabewert_1 { get; set; }
        [Order(50, true)] public decimal O_SC_ID_2 { get; set; }
        [Order(51, true)] public string O_SC_Scoretyp_2 { get; set; }
        [Order(52, true)] public string O_SC_Bezeichnung_2 { get; set; }
        [Order(53, true)] public decimal O_SC_Resultatwert_2 { get; set; }
        [Order(54, true)] public string O_SC_Eingabewert_2 { get; set; }
        [Order(55, true)] public decimal O_SC_ID_3 { get; set; }
        [Order(56, true)] public string O_SC_Scoretyp_3 { get; set; }
        [Order(57, true)] public string O_SC_Bezeichnung_3 { get; set; }
        [Order(58, true)] public decimal O_SC_Resultatwert_3 { get; set; }
        [Order(59, true)] public string O_SC_Eingabewert_3 { get; set; }
        [Order(60, true)] public decimal O_SC_ID_4 { get; set; }
        [Order(61, true)] public string O_SC_Scoretyp_4 { get; set; }
        [Order(62, true)] public string O_SC_Bezeichnung_4 { get; set; }
        [Order(63, true)] public decimal O_SC_Resultatwert_4 { get; set; }
        [Order(64, true)] public string O_SC_Eingabewert_4 { get; set; }
        [Order(65, true)] public decimal O_SC_ID_5 { get; set; }
        [Order(66, true)] public string O_SC_Scoretyp_5 { get; set; }
        [Order(67, true)] public string O_SC_Bezeichnung_5 { get; set; }
        [Order(68, true)] public decimal O_SC_Resultatwert_5 { get; set; }
        [Order(69, true)] public string O_SC_Eingabewert_5 { get; set; }
        [Order(70, true)] public decimal O_SC_ID_6 { get; set; }
        [Order(71, true)] public string O_SC_Scoretyp_6 { get; set; }
        [Order(72, true)] public string O_SC_Bezeichnung_6 { get; set; }
        [Order(73, true)] public decimal O_SC_Resultatwert_6 { get; set; }
        [Order(74, true)] public string O_SC_Eingabewert_6 { get; set; }
        [Order(75, true)] public decimal O_SC_ID_7 { get; set; }
        [Order(76, true)] public string O_SC_Scoretyp_7 { get; set; }
        [Order(77, true)] public string O_SC_Bezeichnung_7 { get; set; }
        [Order(78, true)] public decimal O_SC_Resultatwert_7 { get; set; }
        [Order(79, true)] public string O_SC_Eingabewert_7 { get; set; }
        [Order(80, true)] public decimal O_SC_ID_8 { get; set; }
        [Order(81, true)] public string O_SC_Scoretyp_8 { get; set; }
        [Order(82, true)] public string O_SC_Bezeichnung_8 { get; set; }
        [Order(83, true)] public decimal O_SC_Resultatwert_8 { get; set; }
        [Order(84, true)] public string O_SC_Eingabewert_8 { get; set; }
        [Order(85, true)] public decimal O_SC_ID_9 { get; set; }
        [Order(86, true)] public string O_SC_Scoretyp_9 { get; set; }
        [Order(87, true)] public string O_SC_Bezeichnung_9 { get; set; }
        [Order(88, true)] public decimal O_SC_Resultatwert_9 { get; set; }
        [Order(89, true)] public string O_SC_Eingabewert_9 { get; set; }
        [Order(90, true)] public decimal O_SC_ID_10 { get; set; }
        [Order(91, true)] public string O_SC_Scoretyp_10 { get; set; }
        [Order(92, true)] public string O_SC_Bezeichnung_10 { get; set; }
        [Order(93, true)] public decimal O_SC_Resultatwert_10 { get; set; }
        [Order(94, true)] public string O_SC_Eingabewert_10 { get; set; }
        [Order(95, true)] public decimal O_SC_ID_11 { get; set; }
        [Order(96, true)] public string O_SC_Scoretyp_11 { get; set; }
        [Order(97, true)] public string O_SC_Bezeichnung_11 { get; set; }
        [Order(98, true)] public decimal O_SC_Resultatwert_11 { get; set; }
        [Order(99, true)] public string O_SC_Eingabewert_11 { get; set; }
        [Order(100, true)] public decimal O_SC_ID_12 { get; set; }
        [Order(101, true)] public string O_SC_Scoretyp_12 { get; set; }
        [Order(102, true)] public string O_SC_Bezeichnung_12 { get; set; }
        [Order(103, true)] public decimal O_SC_Resultatwert_12 { get; set; }
        [Order(104, true)] public string O_SC_Eingabewert_12 { get; set; }
        [Order(105, true)] public decimal O_SC_ID_13 { get; set; }
        [Order(106, true)] public string O_SC_Scoretyp_13 { get; set; }
        [Order(107, true)] public string O_SC_Bezeichnung_13 { get; set; }
        [Order(108, true)] public decimal O_SC_Resultatwert_13 { get; set; }
        [Order(109, true)] public string O_SC_Eingabewert_13 { get; set; }
        [Order(110, true)] public decimal O_SC_ID_14 { get; set; }
        [Order(111, true)] public string O_SC_Scoretyp_14 { get; set; }
        [Order(112, true)] public string O_SC_Bezeichnung_14 { get; set; }
        [Order(113, true)] public decimal O_SC_Resultatwert_14 { get; set; }
        [Order(114, true)] public string O_SC_Eingabewert_14 { get; set; }
        [Order(115, true)] public decimal O_SC_ID_15 { get; set; }
        [Order(116, true)] public string O_SC_Scoretyp_15 { get; set; }
        [Order(117, true)] public string O_SC_Bezeichnung_15 { get; set; }
        [Order(118, true)] public decimal O_SC_Resultatwert_15 { get; set; }
        [Order(119, true)] public string O_SC_Eingabewert_15 { get; set; }
        [Order(120, true)] public decimal O_SC_ID_16 { get; set; }
        [Order(121, true)] public string O_SC_Scoretyp_16 { get; set; }
        [Order(122, true)] public string O_SC_Bezeichnung_16 { get; set; }
        [Order(123, true)] public decimal O_SC_Resultatwert_16 { get; set; }
        [Order(124, true)] public string O_SC_Eingabewert_16 { get; set; }
        [Order(125, true)] public decimal O_SC_ID_17 { get; set; }
        [Order(126, true)] public string O_SC_Scoretyp_17 { get; set; }
        [Order(127, true)] public string O_SC_Bezeichnung_17 { get; set; }
        [Order(128, true)] public decimal O_SC_Resultatwert_17 { get; set; }
        [Order(129, true)] public string O_SC_Eingabewert_17 { get; set; }
        [Order(130, true)] public decimal O_SC_ID_18 { get; set; }
        [Order(131, true)] public string O_SC_Scoretyp_18 { get; set; }
        [Order(132, true)] public string O_SC_Bezeichnung_18 { get; set; }
        [Order(133, true)] public decimal O_SC_Resultatwert_18 { get; set; }
        [Order(134, true)] public string O_SC_Eingabewert_18 { get; set; }
        [Order(135, true)] public decimal O_SC_ID_19 { get; set; }
        [Order(136, true)] public string O_SC_Scoretyp_19 { get; set; }
        [Order(137, true)] public string O_SC_Bezeichnung_19 { get; set; }
        [Order(138, true)] public decimal O_SC_Resultatwert_19 { get; set; }
        [Order(139, true)] public string O_SC_Eingabewert_19 { get; set; }
        [Order(140, true)] public decimal O_SC_ID_20 { get; set; }
        [Order(141, true)] public string O_SC_Scoretyp_20 { get; set; }
        [Order(142, true)] public string O_SC_Bezeichnung_20 { get; set; }
        [Order(143, true)] public decimal O_SC_Resultatwert_20 { get; set; }
        [Order(144, true)] public string O_SC_Eingabewert_20 { get; set; }
        [Order(145, true)] public decimal O_DEC_EAD_RATIO { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class OutputDtoPack : S1DataPack
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outStr"></param>
        public void FromString(string outStr)
        {
            this.Clear();


            int propCount = typeof(S1GetResponseDto).GetProperties().Where(p => ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Active == true).ToArray().Length;

            string sss;

            sss = outStr;

            string[] outDtoRecStrArr = outStr.Split('\v');
            foreach (string outDtoRecStr in outDtoRecStrArr)
            {
                //string[] outDtoStrArr = outDtoRecStr.Split('\t');
                string[] outDtoStrArr = outDtoRecStr.Split('\t');

                if (outDtoStrArr.Length < propCount)
                    continue;

                S1GetResponseDto outDto = new S1GetResponseDto();
                int idx = 0;

                foreach (var prop in GetSortedProperties<S1GetResponseDto>())
                {
                    if (((OrderAttribute)prop.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Active == true)
                    {
                        prop.SetValue(outDto, GetValue(outDtoStrArr[idx], prop.PropertyType), null);
                        idx++;
                    }
                }

                this.Add(outDto);
            }
        }

        public static S1GetResponseDto FromStream(string outDtoRecStr)
        {
           
                string[] outDtoStrArr = outDtoRecStr.Split('\t');

                //_log.Debug("Read from S1: " + outDtoRecStr + " length: " + outDtoStrArr.Length);

                S1GetResponseDto outDto = new S1GetResponseDto();
                int idx = 0;

                foreach (var prop in GetSortedProperties<S1GetResponseDto>())
                {
                    try
                    {
                        if (isOrderAttributeActive(prop)) //if (((OrderAttribute)prop.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Active == true)
                        {
                            prop.SetValue(outDto, GetValue(outDtoStrArr[idx], prop.PropertyType), null);
                            idx++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Error setting Value in Response from " + idx,ex);
                    }
                }

                return outDto;
            
        }
    }
   

}
