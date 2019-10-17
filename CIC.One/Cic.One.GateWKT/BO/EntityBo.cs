using System;
using System.Collections.Generic;
using System.Linq;
using Cic.One.Web.DAO;
using Cic.One.DTO;
using System.Globalization;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.One.GateWKT.BO
{

    /// <summary>
    /// Overrides default getdetail/createorupdate logic for WKT
    /// </summary>
    public class EntityBo : Cic.One.Web.BO.EntityBo
    {
        private const string CFG_SEC = "EAIHOT";
        private const string CFG_VAR_CLIENTART = "CLIENTART";
        private const string CFG_VAR_HOSTCOMPUTER = "HOSTCOMPUTER";
        private const string CFG_VAR_EVE = "EVE";
        private const string CFG = "WIT_SERVICE";


        public EntityBo(IEntityDao dao, IAppSettingsBo appBo, ICASBo casBo)
            : base(dao, appBo, casBo)
        {
        }
        /// <summary>
        /// updates/creates finanzierung
        /// </summary>
        /// <param name="finanzierung"></param>
        /// <returns></returns>
        override public FinanzierungDto createOrUpdateFinanzierung(FinanzierungDto finanzierung, int saveMode)
        {
            new WITBO().createOrUpdateFinanzierung(finanzierung,saveMode);
            return finanzierung;
        }
      
    }
}