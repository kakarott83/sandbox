using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using System.ComponentModel;
using System.Reflection;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.Common.BO.Prisma
{

    /// <summary>
    /// Describes all Subvention source value types
    /// </summary>
    public enum ProvisionSourceField
    {
        /// <summary>
        /// Provisions Basis versicherung
        /// </summary>
        [Description("PROV_BASE_VERSICHERUNG")]
        PROV_BASE_VERSICHERUNG = 0,
        /// <summary>
        /// Provisions Basiszinsen Ablose intern
        /// </summary>
        [Description("PROV_BASE_ZINS_ABLINTERN")]
        PROV_BASE_ZINS_ABLINTERN = 1,
        /// <summary>
        /// Provisions Basiszinsen Ablose extern
        /// </summary>
        [Description("PROV_BASE_ZINS_ABLEXTERN")]
        PROV_BASE_ZINS_ABLEXTERN = 2,
        /// <summary>
        /// Provision Basis Umsatz Neugeld
        /// </summary>
        [Description("PROV_BASE_UMSATZ_NEUGELD")]
        PROV_BASE_UMSATZ_NEUGELD = 3,
        /// <summary>
        /// Provision Mark Stueck Neugeld
        /// </summary>
        [Description("PROV_MARK_STUECK_NEUGELD")]
        PROV_MARK_STUECK_NEUGELD = 4,
        /// <summary>
        /// Provisions Basiszins Neugeld
        /// </summary>
        [Description("PROV_BASE_ZINS_NEUGELD")]
        PROV_BASE_ZINS_NEUGELD = 5,
        /// <summary>
        /// Provision Mark Stueck Neugeld
        /// </summary>
        [Description("PROV_MARK_STUECK_VERSICHERUNG")]
        PROV_MARK_STUECK_VERSICHERUNG = 6,
        /// <summary>
        /// Provision Basisumsatz Abloese intern
        /// </summary>
        [Description("PROV_BASE_UMSATZ_ABLINTERN")]
        PROV_BASE_UMSATZ_ABLINTERN = 7,
        /// <summary>
        /// Provision Basisumsatz Abloese extern
        /// </summary>
        [Description("PROV_BASE_UMSATZ_ABLEXTERN")]
        PROV_BASE_UMSATZ_ABLEXTERN = 8,
        /// <summary>
        /// Provisionsbasis Dispo
        /// </summary>
        [Description("PROV_BASE_DISPO")]
        PROV_BASE_DISPO = 9,
        /// <summary>
        /// Provisionsbasis Dispo
        /// </summary>
        [Description("PROV_MARK_STUECK_VERSICHERUNG_RIP")]
        PROV_MARK_STUECK_VERSICHERUNG_RIP = 10,
        /// <summary>
        /// Provisionsbasis Dispo
        /// </summary>
        [Description("PROV_BASE_VERSICHERUNG_RIP")]
        PROV_BASE_VERSICHERUNG_RIP = 11,
        /// <summary>
        /// Folgeauszahlungsprovisionen
        /// </summary>
        [Description("PROV_BASE_FASZL")]
        PROV_BASE_FASZL = 12,
        /// <summary>
        /// Umsatzprovision Gesamt
        /// </summary>
        [Description("PROV_BASE_UMSATZ_GESAMT")]
        PROV_BASE_UMSATZ_GESAMT = 13,

        /// <summary>
        /// Incentive – für die Anzahl der Versicherungen (ANTVS-Sätze) auf dem selektierten Antrag 
        /// </summary>
        [Description("VT_PPI_STK")]
        VT_PPI_STK = 14,
        /// <summary>
        /// Incentive – für den Finanzierungsbetrag - BGEXTERN des selektierten Antrags 
        /// </summary>
        [Description("VT_UMSATZ")]
        VT_UMSATZ = 15,
        /// <summary>
        /// Incentive – KICKBACK für den kumulierten Basis aller bisher abgerechneten Anträge plus den Finanzierungsbetrag - BGEXTERN des selektierten Antrags 
        /// </summary>
        [Description("VT_GESAMTUMSATZ")]
        VT_GESAMTUMSATZ = 16
    }

    /// <summary>
    /// Ablosetypen
    /// </summary>
    public enum Abloesetyp
    {
        /// <summary>
        /// Extern
        /// </summary>
        [StringValue("EXTERN")]
        EXTERN,
        /// <summary>
        /// Intern
        /// </summary>
        [StringValue("INTERN")]
        INTERN
    }

    /// <summary>
    /// Abstract Class: Provision Business Object
    /// </summary>
    public abstract class AbstractProvisionBo : IProvisionBo
    {

        /// <summary>
        /// Provision Data Access Objects
        /// </summary>
        protected IProvisionDao dao;

        /// <summary>
        /// Objekt DAO
        /// </summary>
        protected IObTypDao obDao;

        /// <summary>
        /// Parameter Bo
        /// </summary>
        protected IPrismaParameterBo prismaParameterBo;

        /// <summary>
        /// VG data access
        /// </summary>
        protected IVGDao vgDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dao">Prisma Data Access Object</param>
        /// <param name="obDao">Objekttyp DAO</param>
        /// <param name="prismaParameterBo">Prisma Parameter BO</param>
        /// <param name="vgDao">Wertegruppen DAO</param>
        public AbstractProvisionBo(IProvisionDao dao, IObTypDao obDao, IPrismaParameterBo prismaParameterBo, IVGDao vgDao)
        {
            this.dao = dao;
            this.obDao = obDao;
            this.prismaParameterBo = prismaParameterBo;
            this.vgDao = vgDao;
        }

        /// <summary>
        /// Finds the corresponding Prisma Field /Area Id for the given Provision and Context
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public ParamDto getPrfld(ProvisionSourceField sourceField, prKontextDto context)
        {
            // Get the field info
            FieldInfo field = typeof(ProvisionSourceField).GetField(sourceField.ToString());

            // Get the attributes
            DescriptionAttribute meta = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            ParamDto pDto = prismaParameterBo.getParameter(context, meta.Description);
            return pDto;
        }

        /// <summary>
        /// Finds the corresponding Prisma Field Id for the given Provision and Context
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public long getPrfldId(ProvisionSourceField sourceField, prKontextDto context)
        {
            // Get the field info
            FieldInfo field = typeof(ProvisionSourceField).GetField(sourceField.ToString());

            // Get the attributes
            DescriptionAttribute meta = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            return prismaParameterBo.getFieldID(context, meta.Description);
        }


        /// <summary>
        /// Provisionsdaten berechnen
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract List<AngAntProvDto> calculateProvision(provKontextDto ctx, iProvisionDto param);

        /// <summary>
        /// Calculates the Provisions for incentives
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <param name="tracing">traces all steps</param>
        /// <returns></returns>
        abstract public List<AngAntProvDto> calculateIncentiveProvision(provKontextDto ctx, iProvisionDto param, List<ProvKalkDto> tracing);

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public abstract List<PRPROVTYPE> getProvisionTypes();

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public abstract List<PRPROVTYPE> getProvisionTypes(long sysprfld);

        /// <summary>
        /// Returns a list of all prisma fields that have a provision configured
        /// </summary>
        /// <returns></returns>
        public abstract List<long> getPrFlds();

        /// <summary>
        /// checks if the abloese is of the given type
        /// </summary>
        /// <param name="sysabltyp"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public abstract bool isAbloesetyp(long sysabltyp, Abloesetyp typ);

        /// <summary>
        /// checks if prhgroup must be 0
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprhgroud"></param>
        /// <returns></returns>
        public abstract bool isPrhGroup(long sysperole, long sysprhgroud);


        /// <summary>
        /// get the Eigenabloeseinformation
        /// </summary>
        /// <param name="sysvorvt"></param>
        /// <returns></returns>
        public abstract EigenAblInfo getEigenabloeseInfo(long sysvorvt);

        /// <summary>
        /// Returns the current provision plan for Kickback for the given role and prhgroup
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public abstract PRPROVSET getProvisionsPlan(long sysperole, DateTime perDate);

                /// <summary>
        /// Returns the current provision plan (prprovset) id for the given provision context
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="sysprprovset"></param>
        /// <returns></returns>
        public abstract long getProvisionPlan(provKontextDto ctx, long sysprprovset);
    }
}
