using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Reflection;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Base class of all Dto's to be returned
    /// allows the WS-Client to fetch the id with always the same method
    /// the concrete Dto has to implement the getEntityId-Method and return the corresponding unique id
    /// </summary>
    public abstract class EntityDto
    {
        /// <summary>
        /// primary key value
        /// </summary>
        public long entityId
        {
            get { return getEntityId(); }
            set { }
        }

        /// <summary>
        /// description of the field, filled by reflection 
        /// </summary>
        public String entityBezeichnung
        {
            get { return getEntityBezeichnung(); }
            set { }
        }

        /// <summary>
        /// Indicator (EXPRANGE) output
        /// </summary>
        public String indicatorContent {get;set;}


        /// <summary>
        /// must be implemented from the extending class to determine its primary key field value
        /// </summary>
        /// <returns></returns>
        public abstract long getEntityId();

		/// <summary>
		///  Gelesen-FLAG
		/// </summary>
		public int? preadFlag 
		{
			 get; set; 
		}

		///// <summary>
		///// Gelesen-FLAG	(rh 20170515)
		///// </summary>
		///// <returns>Gelesen-FLAG</returns>
		//public bool getReadFlag ()
		//{
		//	return preadFlag > 0;
		//}

        /// <summary>
        /// Upon serialization this will be called and fill the transmitted entityBezeichnung
        /// it will contain a reflection-based return-value of the bezeichnung/description-method of this object, if available
        /// </summary>
        /// <returns></returns>
        virtual public String getEntityBezeichnung()
        {
            Type t = this.GetType();
            PropertyInfo[] props = t.GetProperties();
            //first look for properties starting with a prefix
            foreach (PropertyInfo p in props)
            {
                if (p.Name.ToLower().Contains("entitybezeichnung")) continue;
                if (!p.PropertyType.Equals(typeof(String))) continue;
                if (p.Name.ToLower().StartsWith("bezeichnung"))
                    return (String)p.GetValue(this, null);
                if (p.Name.ToLower().StartsWith("description"))
                    return (String)p.GetValue(this, null);

            }
            //second look for properties containing a string 
            foreach (PropertyInfo p in props)
            {
                if (p.Name.ToLower().Contains("entitybezeichnung")) continue;
                if (!p.PropertyType.Equals(typeof(String))) continue;
                if (p.Name.ToLower().Contains("bezeichnung"))
                    return (String)p.GetValue(this, null);
            }
            return "";
        }

        public virtual string getArea()
        {
            String className = this.GetType().Name;
            className = className.Substring(className.LastIndexOf('.') + 1);
            className = className.Substring(0, className.Length - 3);
            return className;
        }

        //public PairDto[] extvalues { get; set; }
    }
}