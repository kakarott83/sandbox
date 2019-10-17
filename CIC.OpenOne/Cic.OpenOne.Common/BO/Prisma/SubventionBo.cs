using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Common;

using System.Reflection;

using System.Data.EntityClient;

using System.Data.Objects;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO.Prisma;
using System.ComponentModel;

namespace Cic.OpenOne.Common.BO.Prisma
{
   
    /// <summary>
    /// Bo for Subventions
    /// </summary>
    [System.CLSCompliant(true)]
    public class SubventionBo : AbstractSubventionBo
    {
        #region Private variables
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dao">DAO</param>
        /// <param name="prismaBo"></param>
        public SubventionBo(ISubventionDao dao, IPrismaParameterBo prismaBo)
            : base(dao, prismaBo)
        {


        }

        /*
           je nach Subvention:
            *alle zu prüfenden SubventionSourceFields
            *zu jedem SubventionSourceField die jeweilige id, falls explizit
            * zu jedem wird benötigt: 
            *          defaultwert ohne subvention
            *              -> daraus berechenbar subventionierter betrag (wird dann für die endbetragsberechnung z.b. in der versicherungsberechnung abgezogen)
            *         endbetrag oder nachlass des verkäufers
           */
        /// <summary>
        /// calculates the subvention
        /// </summary>
        /// <param name="subvention"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        override public List<AngAntSubvDto> calcSubvention(iSubventionDto subvention, prKontextDto context)
        {
            List<AngAntSubvDto> rval = new List<AngAntSubvDto>();

            ParamDto param = getAreaId(subvention.subventionSource, context);
            if (param == null)
            {
                _log.Warn("No suitable PrParam for given ProductKontext to calculate Subvention " + subvention.subventionSource);
                return rval;
            }

           
            if (subvention.calcMode == SubventionCalcMode.EXPLICIT || subvention.calcMode==SubventionCalcMode.BOTH)
            {
                if (!subvention.areaId.HasValue)
                {
                    _log.Error("Cant calculate explicit Subvention for " + subvention.subventionSource + " - no area id given");
                }
                else
                {
                    List<PRSUBV> explSubs = this.getExplicitSubventions(subvention.sourceArea, subvention.areaId.Value, context.sysprproduct, param.sysprfld, context.perDate);
                    rval.AddRange(calculateSubvention(subvention, explSubs, SubventionCalcMode.EXPLICIT));
                }
            }
            if (subvention.calcMode == SubventionCalcMode.IMPLICIT || subvention.calcMode == SubventionCalcMode.BOTH)
            {
                List<PRSUBV> implSubs = this.getImplicitSubventions(context.sysprproduct, param.sysprfld, context.perDate);
                rval.AddRange(calculateSubvention(subvention, implSubs, SubventionCalcMode.IMPLICIT));
            }
            return rval;
           
          
        }


    }


}

