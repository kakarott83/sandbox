using CIC.Database.OL.EF4.Model;
using System;

namespace Cic.OpenOne.Common.Model.DdOl
{
    /// <summary>
    /// PEUNIArea-Enum
    /// </summary>
    public enum PEUNIArea
    {
        /// <summary>
        /// PERSON
        /// </summary>
        PERSON,

        /// <summary>
        /// IT
        /// </summary>
        IT,

        /// <summary>
        /// VT
        /// </summary>
        VT,

        /// <summary>
        /// ANTRAG
        /// </summary>
        ANTRAG,

        /// <summary>
        /// ANGEBOT
        /// </summary>
        ANGEBOT,

        /// <summary>
        /// MYCALC
        /// </summary>
        MYCALC
    }

    public partial class PEUNIHelper : global::System.Data.Objects.DataClasses.EntityObject
    {

      
        /// <summary>
        /// PEUNI
        /// </summary>
        public PEUNIHelper()
        {
        }

      

        #region Methods
        /// <summary>
        /// ConnectNodes
        /// </summary>
        /// <param name="context"></param>
        /// <param name="areas"></param>
        /// <param name="sysId"></param>
        /// <param name="sysPerole"></param>
        public static void ConnectNodes(OLContext context, PEUNIArea areas, long sysId, long sysPerole)
        {
            DateTime DateTimeNow = DateTime.Today;
            PEUNI PeUni = new PEUNI();

            // Set
            PeUni.AREA = areas.ToString();
            PeUni.SYSID = sysId;
            PeUni.UNIDATE = DateTimeNow;
            PeUni.SYSPEROLE= sysPerole;

            // Add
            context.AddToPEUNI(PeUni);
        }

        /// <summary>
        /// getEntityKey
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static System.Data.EntityKey getEntityKey(OLContext context,System.Type type, long value)
        {
            System.Data.Metadata.Edm.EntityType etype = MyGetEntityType(context, type);
            return new System.Data.EntityKey(context.DefaultContainerName + "." + etype.Name, etype.KeyMembers[0].Name, value);
        }

        private static System.Data.Metadata.Edm.EntityType MyGetEntityType(System.Data.Objects.ObjectContext context, System.Type type)
        {
            return context.MetadataWorkspace.GetType(type.Name, type.Namespace, System.Data.Metadata.Edm.DataSpace.CSpace) as System.Data.Metadata.Edm.EntityType;
        }
        #endregion

    }
}