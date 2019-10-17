using Cic.OpenOne.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public enum KontrollinhaberStatus
    {
        /// <summary>
        /// Firma eindeutig identifiziert und Kontrollinhaber vorhanden
        /// select CFADDRESSMATCH.ADDRESSMATCHRESULTTYPE from AUSKUNFT,CFOUTIDENTADDR,CFADDRESSMATCH where AUSKUNFT.SYSAUSKUNFT = CFOUTIDENTADDR.SYSAUSKUNFT and CFOUTIDENTADDR.SYSCFADDRESSMATCH = CFADDRESSMATCH.SYSCFADDRESSMATCH and AUSKUNFT.SYSAUSKUNFT =:sysauskunft
        /// MATCH==eindeutig identifiziert
        /// + weitere Query für Kontrollinhaber vorhanden ja/nein
        /// </summary>
        [StringValue("FOUND")]
        FOUND = 0,
        /// <summary>
        /// Firma nicht eindeutig identifiziert
        /// select CFADDRESSMATCH.ADDRESSMATCHRESULTTYPE from AUSKUNFT,CFOUTIDENTADDR,CFADDRESSMATCH where AUSKUNFT.SYSAUSKUNFT = CFOUTIDENTADDR.SYSAUSKUNFT and CFOUTIDENTADDR.SYSCFADDRESSMATCH = CFADDRESSMATCH.SYSCFADDRESSMATCH and AUSKUNFT.SYSAUSKUNFT =:sysauskunft
        /// -> CANDIDATES
        /// </summary>
        [StringValue("LIST")]
        LIST = 1,
        /// <summary>
        /// Firma nicht gefunden
        /// select CFADDRESSMATCH.ADDRESSMATCHRESULTTYPE from AUSKUNFT,CFOUTIDENTADDR,CFADDRESSMATCH where AUSKUNFT.SYSAUSKUNFT = CFOUTIDENTADDR.SYSAUSKUNFT and CFOUTIDENTADDR.SYSCFADDRESSMATCH = CFADDRESSMATCH.SYSCFADDRESSMATCH and AUSKUNFT.SYSAUSKUNFT =:sysauskunft
        /// NO_MATCH
        /// </summary>
        [StringValue("NO_COMP_FOUND_CRIF")]
        NO_COMP_FOUND_CRIF = 2,
        /// <summary>
        /// Fehler CRIF - auskunft.fehlercode !=0/null
        /// </summary>
        [StringValue("ERROR_CRIF")]
        ERROR_CRIF = 3,
        /// <summary>
        /// Firma gefunden aber CRIF liefert keine Kontrollinhaber
        /// select CFADDRESSMATCH.ADDRESSMATCHRESULTTYPE from AUSKUNFT,CFOUTIDENTADDR,CFADDRESSMATCH where AUSKUNFT.SYSAUSKUNFT = CFOUTIDENTADDR.SYSAUSKUNFT and CFOUTIDENTADDR.SYSCFADDRESSMATCH = CFADDRESSMATCH.SYSCFADDRESSMATCH and AUSKUNFT.SYSAUSKUNFT =:sysauskunft
        /// MATCH==eindeutig identifiziert
        /// + weitere Query für Kontrollinhaber vorhanden ja/nein
        /// </summary>
        [StringValue("NO_DATA_CRIF")]
        NO_DATA_CRIF = 4
    }
}
