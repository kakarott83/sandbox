using System.Collections.Generic;

using Cic.OpenOne.Common.Model.DdOd;

using Devart.Data.Oracle;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    public class InteressentBo
    {
        public long SysIt { get; set; }

        public long SysPerson { get; set; }

        public bool IsMa { get; set; }

        public bool IsComplementary { get; set; }

        public bool ExistsOnlyInIT
        {
            get { return SysPerson == 0 && SysIt > 0; }
        }

        public string SchufaId { get; set; }

        public string CrefoId { get; set; }

        public string FicoId { get; set; }

        public string KeyId { get; set; }

        public void UpdatePerson(DdOdExtended ctx)
        {
            if (SysPerson > 0)
            {
                var parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter { ParameterName = "syskd", Value = SysPerson });
                string updateIds = string.Empty;
                
                if (!string.IsNullOrWhiteSpace(SchufaId))
                {
                    parameters.Add(new OracleParameter { ParameterName = "schufaId", Value = SchufaId });
                    updateIds += ", SCHUFAID = :schufaId";
                }
                if (!string.IsNullOrWhiteSpace(CrefoId))
                {
                    parameters.Add(new OracleParameter { ParameterName = "crefoId", Value = CrefoId });
                    updateIds += ", CREFOID = :crefoId";
                }
                if (!string.IsNullOrWhiteSpace(FicoId))
                {
                    parameters.Add(new OracleParameter { ParameterName = "ficoId", Value = FicoId });
                    updateIds += ", FICOID = :ficoId";
                }
                ctx.ExecuteStoreCommand(string.Format("UPDATE PERSON SET AKTIVKZ=1, FLAGKD=1{0} where sysperson=:syskd", updateIds), parameters.ToArray());
            }
        }
    }
}