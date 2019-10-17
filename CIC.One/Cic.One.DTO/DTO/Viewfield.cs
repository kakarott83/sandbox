using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Cic.One.DTO
{
    /// <summary>
    /// Describes a DB-To GUI mapped field
    /// </summary>
    public class Viewfield
    {
        /// <summary>
        /// Temporarily used for taking over BPE Vars
        /// </summary>
        [IgnoreDataMember]
        public EvalParamDto ep { get; set; }

        public Viewfield() { }
    

        public Viewfield clone()
        {
            Viewfield rval = new Viewfield();
            rval.id = id;
            rval.attr = attr.clone();
            if(value!=null)
                rval.value = value.clone();
            return rval;
        }

        public List<XproEntityDto> values { get;set;}

        /// <summary>
        /// Unique ID
        /// </summary>
        [XmlAttribute]
        public String id { get; set; }

        /// <summary>
        /// View Field Attributes
        /// </summary>
        public ViewFieldAttributes attr { get; set; }        
        
        /// <summary>
        /// Value for this generic field
        /// </summary>
        public ViewValue value { get; set; }

        public Object getValue()
        {
            try
            {
                switch (attr.type.ToLower())
                {

                    case "string":
                        return value.s;
                      
                    case "int":
                        return value.i;
                      
                    case "long":
                        return value.l;
                       
                    case "double":
                        return value.d;
                       
                    case "datetime":
                        return value.t;
                       

                }
            }
            catch (Exception e)
            {
                ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                _log.Error("Conversion of " + attr.type + " failed for column " + attr.field, e);
            }
            return null;
        }

        /// <summary>
        /// creates a cloned instance of ViewField, only containing the value and id
        /// </summary>
        /// <param name="config"></param>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Viewfield createFieldFromDataReader(Viewfield config,DbDataReader reader, int index)
        {
            Viewfield rval = new Viewfield();
            rval.id = config.id;
            rval.value = getFromDataReader(config.attr.type, reader, index);
            return rval;
        }

        /// <summary>
        /// Fills the value Structure with the data from the reader at its given index
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        private static ViewValue getFromDataReader(String type, DbDataReader reader, int index)
        {
            try
            {
                return getFromValue(type, reader.GetValue(index));
            }
            catch (Exception e)
            {
                ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                _log.Error("Conversion of " + type + " failed for column " + index, e);
                return new ViewValue();
            }
        }

        /// <summary>
        /// Returns a Value-Object for the given value,using the current attributes type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private static ViewValue getFromValue(String type,  Object val)
        {
            ViewValue value = new ViewValue();
            if (val is System.DBNull) return value;
            if (val == null) return value;
           
                switch (type.ToLower())
                {

                    case "string":
                        value.s = Convert.ToString(val);
                        break;
                    case "int":
                        value.i = Convert.ToInt32(val);
                        break;
                    case "long":
                        value.l = Convert.ToInt64(val);
                        break;
                    case "double":
                        value.d = Convert.ToDouble(val);
                        break;
                    case "datetime":
                        value.t = (DateTime)val;
                        break;
                }
           
            return value;
        }

        /// <summary>
        /// Fills the value Structure with the data from the reader at its given index
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        public void fillFromDataReader(DbDataReader reader, int index)
        {
            value = getFromDataReader(attr.type, reader, index);
        }

        /// <summary>
        /// Fills the value from an object, using the attributes type
        /// </summary>
        /// <param name="obj"></param>
        public void fillFromObject(Object obj)
        {
            value = getFromValue(attr.type, obj);
        }

        public void setStringValue(String v)
        {
            value = new ViewValue();
            value.s = v;
        }

        /// <summary>
        /// Fills the value Structure with the corresponding field from GViewDto and sets the Dto's field to null
        /// </summary>
        /// <param name="gviewDto"></param>
        public void fillFromGViewDto(GviewDto gviewDto)
        {
            PropertyInfo info = typeof(GviewDto).GetProperty(id);
            value = new ViewValue();

            switch (attr.type.ToLower())
            {

                case "string":
                    value.s = (String)info.GetValue(gviewDto, null);
                    break;
                case "int":
                    value.i = (int)info.GetValue(gviewDto, null);
                    break;
                case "long":
                    value.l = (long)info.GetValue(gviewDto, null);
                    break;
                case "double":
                    value.d = (double)info.GetValue(gviewDto, null);
                    break;
                case "datetime":
                    value.t = (DateTime)info.GetValue(gviewDto, null);
                    break;

            }
            info.SetValue(gviewDto, null, null);
        }
    }
}
