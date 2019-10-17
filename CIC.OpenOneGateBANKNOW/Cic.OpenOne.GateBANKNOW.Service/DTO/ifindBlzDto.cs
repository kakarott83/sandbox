using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService"/> Methode
    /// </summary>
    public class ifindBlzDto
    {
        /// <summary>
        /// Daten zum Suchen von einer Bank
        /// </summary>
        public String searchValue
        {
            get;
            set;
        }

        /// <summary>
        /// Typ der Suche
        /// </summary>
        public BlzType searchType
        {
            get;
            set;
        }
    }
}
