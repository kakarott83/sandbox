using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Describes all Subvention source value types
    /// </summary>
    public enum SubventionSourceField
    {
        /// <summary>
        /// Differenz des Leasingzins
        /// </summary>
        [Description("KALK_SUBV_ZINS")]
        DIFFERENZLEASINGZINS = 0
    }

    /// <summary>
    /// defines the share methods
    /// </summary>
    public enum SubventionShareMethod
    {
        /// <summary>
        /// Prozent
        /// </summary>
        PERCENT = 0,
        /// <summary>
        /// Wert
        /// </summary>
        VALUE = 1
    }

    /// <summary>
    /// defines the calculation methods
    /// </summary>
    public enum SubventionCalcMethod
    {
        /// <summary>
        /// Prozent
        /// </summary>
        PERCENT = 0,
        /// <summary>
        /// Anteilig
        /// </summary>
        PERCENTAGE = 1,
        /// <summary>
        /// Wert
        /// </summary>
        VALUE = 2
    }

    /// <summary>
    /// Abstract Class: Subvention Business Object
    /// </summary>
    public abstract class AbstractSubventionBo : ISubventionBo
    {
        /// <summary>
        /// Provision Data Access Objects
        /// </summary>
        protected ISubventionDao dao;
        /// <summary>
        /// Prismaparameter BO
        /// </summary>
        protected IPrismaParameterBo prismaParameterBo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dao">Prisma Data Access Object</param>
        /// <param name="prismaParameterBo"></param>
        public AbstractSubventionBo(ISubventionDao dao, IPrismaParameterBo prismaParameterBo)
        {
            this.dao = dao;
            this.prismaParameterBo = prismaParameterBo;
        }

        /// <summary>
        /// calculates the subvention
        /// </summary>
        /// <param name="subvention"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        abstract public List<AngAntSubvDto> calcSubvention(iSubventionDto subvention, prKontextDto context);

        /// <summary>
        /// delivers all Subventions for the given triggertype, triggerfield and product
        /// 
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysprfld"></param>
        /// <param name="trgtype">(explicit=1/implicit=2)</param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        protected List<PRSUBV> getSubventions(long sysprproduct, long sysprfld, SubventionType trgtype, DateTime perDate)
        {
            List<PrSubvTriggerDto> triggers1 = dao.getSubventionTriggers(perDate);
            List<PrSubvTriggerDto> triggers = triggers1.Where(t => t.sysprproduct == sysprproduct && t.trgtype == (int)trgtype && t.sysprfldtrg == sysprfld).ToList();
            List<PRSUBV> rval = new List<PRSUBV>();
            if (triggers == null) return rval;
            List<PRSUBV> subventions = dao.getSubventions();

            foreach (PrSubvTriggerDto trg in triggers)
            {
                PRSUBV subv = subventions.Where(t => t.SYSPRSUBV == trg.sysprsubv).FirstOrDefault();
                if (subv != null)
                    rval.Add(subv);
            }
            return rval;
        }

        /// <summary>
        /// delivers all implicit Subventions for the given triggerfield and product
        /// 
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysprfld"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        protected List<PRSUBV> getImplicitSubventions(long sysprproduct, long sysprfld, DateTime perDate)
        {
            return getSubventions(sysprproduct, sysprfld, SubventionType.IMPLICIT, perDate);
        }

        /// <summary>
        /// delivers all explicit Subventions for the given triggerfield and product
        /// 
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysprfld"></param>
        /// <param name="area"></param>
        /// <param name="areaId"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        protected List<PRSUBV> getExplicitSubventions(ExplicitSubventionArea area, long areaId, long sysprproduct, long sysprfld, DateTime perDate)
        {
            return getExplicitSubventions(area, areaId, getSubventions(sysprproduct, sysprfld, SubventionType.EXPLICIT, perDate));
        }

        /// <summary>
        /// returns the explicit subventions for the area and areaid
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaId"></param>
        /// <param name="subventions">list of Subventions from getSubventions with trgtype=1</param>
        /// <returns></returns>
        protected List<PRSUBV> getExplicitSubventions(ExplicitSubventionArea area, long areaId, List<PRSUBV> subventions)
        {
            List<long> ids = dao.getExplicitSubventionIds(area, areaId);

            List<PRSUBV> rval = new List<PRSUBV>();
            if (ids == null || subventions == null) return rval;

            foreach (PRSUBV subv in subventions)
            {
                if (ids.Contains(subv.SYSPRSUBV))
                    rval.Add(subv);
            }
            return rval;
        }

        /// <summary>
        /// Finds the corresponding Prisma Field /Area Id for the given Subvention and Context
        /// </summary>
        /// <param name="field"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected ParamDto getAreaId(SubventionSourceField field, prKontextDto context)
        {
            // Get the field info
            FieldInfo Field = typeof(SubventionSourceField).GetField(field.ToString());

            // Get the attributes
            DescriptionAttribute meta = (DescriptionAttribute)Attribute.GetCustomAttribute(Field, typeof(DescriptionAttribute));
            ParamDto pDto = prismaParameterBo.getParameter(context, meta.Description);
            return pDto;
        }

        /// <summary>
        /// Calculates the subvention for the given value and subvention-entity
        /// Only percent-Methods take the current item value into account
        /// 
        /// example: value = 100 monthly, laufzeit=12, subvention.laufzeit=6, subvention.rate=50% -> 300 Subvention value
        /// example: value = 100 monthly, laufzeit=12, subvention.laufzeit=null, subvention.rate=50% -> 600 Subvention value
        /// </summary>
        /// <param name="value">the value to calculate the subvention for</param>
        /// <param name="subvention">the configuration entity object for the subvention</param>
        /// <param name="laufzeit">duration</param>
        /// <returns></returns>
        private double calcSubvention(double value, PRSUBV subvention, long laufzeit)
        {
            bool subventionLz = subvention.TERM.HasValue && subvention.TERM.Value > 0;
            long lz = subventionLz ? (long)subvention.TERM : (long)laufzeit;

            switch (subvention.CALCMETHOD)
            {
                case ((int)SubventionCalcMethod.PERCENT):
                    return value * (double)subvention.SUBVRATE * lz;

                case ((int)SubventionCalcMethod.PERCENTAGE):
                    return value * (double)subvention.SUBVRATE / 100 * lz;

                case ((int)SubventionCalcMethod.VALUE):
                    return (double)subvention.SUBVVAL * lz;

            }
            throw new ArgumentException("Subvention " + subvention.SYSPRSUBV + " enthält keine Berechnungsmethode");
        }

        /// <summary>
        /// Distributes subventionValue to the Subvention-Donor
        /// If the remaining subventionValue would be below zero, the return value will be
        /// reduced accordingly and subventionValue will be zero
        /// </summary>
        /// <param name="subventionValue">current remaining subvention value</param>
        /// <param name="pos">Subvention Donor Information object</param>
        /// <param name="sub">Subvention Information object</param>
        /// <param name="subventionSum">Subvention sum as base for percentage distribution</param>
        /// <returns>the value the donor has to pay</returns>
        private double distributeSubvention(double subventionSum, ref double subventionValue, PRSUBVPOS pos, PRSUBV sub)
        {
            double rval = 0;
            if (sub.SHAREMETHOD == (int)SubventionShareMethod.VALUE)
            {
                rval = (double)pos.PARTVAL;
            }
            else if (sub.SHAREMETHOD == (int)SubventionShareMethod.PERCENT)
            {
                rval = (double)pos.PARTRATE / 100 * subventionSum;

            }
            if (subventionValue - rval < 0)
            {
                rval = subventionValue;
            }
            subventionValue -= rval;

            return rval;
        }

        /// <summary>
        /// Subvention berechnen
        /// </summary>
        /// <param name="sParams">Parameter</param>
        /// <param name="subventionList">subventionen</param>
        /// <param name="calcMode">Berechnungsmodus</param>
        /// <returns>Subventionsliste</returns>
        protected List<AngAntSubvDto> calculateSubvention(iSubventionDto sParams, List<PRSUBV> subventionList, SubventionCalcMode calcMode)
        {
            List<AngAntSubvDto> rval = new List<AngAntSubvDto>();

            // List<SubventionPosDto> subpos = new List<SubventionPosDto>();
            double subventionValue = sParams.defaultValue;
            bool limit = true;
            IRounding round = RoundingFactory.createRounding();

            foreach (PRSUBV subvention in subventionList)
            {
                double zuSubvention = calcSubvention(subventionValue, subvention, sParams.laufzeit);
                if (limit)
                {
                    if (zuSubvention > subventionValue)
                    {
                        zuSubvention = subventionValue;
                    }
                }
                if (calcMode == SubventionCalcMode.IMPLICIT)//implicit uses the whole value at once, no distribution!
                    zuSubvention = subventionValue;
                subventionValue -= zuSubvention;//pro subvention kann max. remaining ausgeschöpft werden, der rest kann auf weitere subventionen verteilt werden
                //subventionValue ist jetzt der Rest der noch subventioniert werden kann
                //zuSubvention enthält den gerade subventionierten Betrag der nun zugewiesen wird

                List<PRSUBVPOS> poslist = dao.getSubventionPositions(subvention.SYSPRSUBV, sParams.defaultValue);

                double sum = 0;
                double valForSubvention = zuSubvention;

                foreach (PRSUBVPOS pos in poslist)
                {
                    double facAbs = distributeSubvention(valForSubvention, ref zuSubvention, pos, subvention);

                    //create and save the SUBVENTION to database
                    AngAntSubvDto s = new AngAntSubvDto();
                    sum += facAbs;
                    s.beginn = sParams.perDate;
                    s.betragBrutto = round.RoundCHF(facAbs);
                    s.betrag = round.RoundCHF(round.getNetValue(facAbs, sParams.mwst));
                    s.betragust = s.betragBrutto - s.betrag;
                    s.betragdef = round.RoundCHF(sParams.defaultValue);
                    s.syssubvtyp = subvention.SYSSUBVTYP.HasValue ? subvention.SYSSUBVTYP.Value : 0;
                    s.sysprsubv = subvention.SYSPRSUBV;

                    s.lz = subvention.TERM != null && subvention.TERM > 0 ? (int)subvention.TERM : sParams.laufzeit;
                    /*
                     SubventionPosDto sinfo = new SubventionPosDto();
                     sinfo.SYSPRSUBVPOS = pos.SYSPRSUBVPOS;
                     sinfo.VALUE = s.BETRAGBRUTTO;
                     sinfo.VALUE_NETTO = s.BETRAG;
                     sinfo.VALUE_UST = s.BETRAGUST;
                     */
                    //wenn keine person hinterlegt, auf händler 
                    long? sysp = pos.SYSPERSON.HasValue && pos.SYSPERSON > 0 ? (long)pos.SYSPERSON : sParams.sysperson;
                    /*if (sysp.HasValue && sysp > 0)
                    {
                        sinfo.SYSPERSON = (long)sysp;
                        PERSON pers = PersonHelper.SelectBySysPERSON(_context, (long)sysp);
                        sinfo.VORNAME = pers.VORNAME;
                        if (sinfo.VORNAME == null || sinfo.VORNAME.Trim().Length == 0)
                            sinfo.VORNAME = "";
                        sinfo.NACHNAME = pers.NAME;
                        if (sinfo.NACHNAME == null || sinfo.NACHNAME.Trim().Length == 0)
                            sinfo.NACHNAME = "";
                    }
                    subpos.Add(sinfo);*/
                    s.syssubvg = (long)sysp;
                    rval.Add(s);
                    if (zuSubvention <= 0) break;
                }

                //info.VALUE = sum;
                // info.SUBVENTIONSGEBER = subpos.ToArray();

                if (zuSubvention > 0)
                    subventionValue += zuSubvention; //wurde von aktueller Stützung nicht verwendet, für weitere wieder verfügbar machen

                if (subventionValue <= 0) break;
            }
            sParams.defaultValue = round.RoundCHF(subventionValue);
            return rval;
        }
    }
}