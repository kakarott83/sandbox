using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// WFTable Codes
    /// </summary>
    public enum WfTableCode
    {
        /// <summary>
        /// LSADD
        /// </summary>
        LSADD,
        /// <summary>
        /// PERSON
        /// </summary>
        PERSON,
        /// <summary>
        /// STAAT
        /// </summary>
        STAAT,
        /// <summary>
        /// LAND
        /// </summary>
        LAND,
        /// <summary>
        /// VT
        /// </summary>
        VT,
        /// <summary>
        /// SYSTEM
        /// </summary>
        SYSTEM,
        /// <summary>
        /// MASS
        /// </summary>
        MASS,
        /// <summary>
        /// WAEHRUNG
        /// </summary>
        WAEHRUNG,
        /// <summary>
        /// KONZERN
        /// </summary>
        KONZERN,
        /// <summary>
        /// BRANCHE
        /// </summary>
        BRANCHE,
        /// <summary>
        /// VWAHL
        /// </summary>
        VWAHL,
        /// <summary>
        /// BENUTZER
        /// </summary>
        BENUTZER,
        /// <summary>
        /// FI
        /// </summary>
        FI,
        /// <summary>
        /// FIBU
        /// </summary>
        FIBU,
        /// <summary>
        /// FT
        /// </summary>
        FT,
        /// <summary>
        /// KALK
        /// </summary>
        KALK,
        /// <summary>
        /// KK
        /// </summary>
        KK,
        /// <summary>
        /// MASSL
        /// </summary>
        MASSL,
        /// <summary>
        /// MASSLPOS
        /// </summary>
        MASSLPOS,
        /// <summary>
        /// OB
        /// </summary>
        OB,
        /// <summary>
        /// PARTNER
        /// </summary>
        PARTNER,
        /// <summary>
        /// PEOPTION
        /// </summary>
        PEOPTION,
        /// <summary>
        /// RAP
        /// </summary>
        RAP,
        /// <summary>
        /// RN
        /// </summary>
        RN,
        /// <summary>
        /// VTOBHD
        /// </summary>
        VTOBHD,
        /// <summary>
        /// VTOBSL
        /// </summary>
        VTOBSL,
        /// <summary>
        /// VTOBVM
        /// </summary>
        VTOBVM,
        /// <summary>
        /// VTOPTION
        /// </summary>
        VTOPTION,
        /// <summary>
        /// MWST
        /// </summary>
        MWST,
        /// <summary>
        /// BA
        /// </summary>
        BA,
        /// <summary>
        /// KGRUPPE
        /// </summary>
        KGRUPPE,
        /// <summary>
        /// ZVB
        /// </summary>
        ZVB,
        /// <summary>
        /// VTTYP
        /// </summary>
        VTTYP,
        /// <summary>
        /// VART
        /// </summary>
        VART,
        /// <summary>
        /// VARTTAB
        /// </summary>
        VARTTAB,
        /// <summary>
        /// SLTYP
        /// </summary>
        SLTYP,
        /// <summary>
        /// FTTEXT
        /// </summary>
        FTTEXT,
        /// <summary>
        /// RNTYP
        /// </summary>
        RNTYP,
        /// <summary>
        /// NK
        /// </summary>
        NK,
        /// <summary>
        /// NKNUM
        /// </summary>
        NKNUM,
        /// <summary>
        /// VTMAHN
        /// </summary>
        VTMAHN,
        /// <summary>
        /// VTFZ
        /// </summary>
        VTFZ,
        /// <summary>
        /// MASSTYP
        /// </summary>
        MASSTYP,
        /// <summary>
        /// SKONTO
        /// </summary>
        SKONTO,
        /// <summary>
        /// NKONTO
        /// </summary>
        NKONTO,
        /// <summary>
        /// BLZ
        /// </summary>
        BLZ,
        /// <summary>
        /// BERATADD
        /// </summary>
        BERATADD,
        /// <summary>
        /// RVT
        /// </summary>
        RVT,
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        RVTPOS,
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        KONTO,
        /// <summary>
        /// 
        /// </summary>
        BLZFIL,
        /// <summary>
        /// 
        /// </summary>
        WFZUST,
        /// <summary>
        /// 
        /// </summary>
        KOSTART,
        /// <summary>
        /// 
        /// </summary>
        RNPOS,
        /// <summary>
        /// 
        /// </summary>
        KONTOKL,
        /// <summary>
        /// 
        /// </summary>
        BBAB,
        /// <summary>
        /// 
        /// </summary>
        PEMAHN,
        /// <summary>
        /// 
        /// </summary>
        SKONTOM,
        /// <summary>
        /// 
        /// </summary>
        WFJLVL2,
        /// <summary>
        /// 
        /// </summary>
        ARTIKEL,
        /// <summary>
        /// 
        /// </summary>
        ADRESSE,
        /// <summary>
        /// 
        /// </summary>
        ESR,
        /// <summary>
        /// 
        /// </summary>
        LS,
        /// <summary>
        /// 
        /// </summary>
        OBINI,
        /// <summary>
        /// 
        /// </summary>
        MARK,
        /// <summary>
        /// 
        /// </summary>
        GSTADD,
        /// <summary>
        /// 
        /// </summary>
        KOSTSTEL,
        /// <summary>
        /// 
        /// </summary>
        KOSTTRAE,
        /// <summary>
        /// 
        /// </summary>
        ABTEIL,
        /// <summary>
        /// 
        /// </summary>
        ADMADD,
        /// <summary>
        /// 
        /// </summary>
        ADMPLZ,
        /// <summary>
        /// 
        /// </summary>
        VPADD,
        /// <summary>
        /// 
        /// </summary>
        VPFILADD,
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        KKTYPZT,
        /// <summary>
        /// 
        /// </summary>
        KKSLZINS,
        /// <summary>
        /// 
        /// </summary>
        KKSLZPOS,
        /// <summary>
        /// 
        /// </summary>
        KKSL,
        /// <summary>
        /// 
        /// </summary>
        KKSLPOS,
        /// <summary>
        /// 
        /// </summary>
        KNETYP,
        /// <summary>
        /// 
        /// </summary>
        MWSTDATE,
        /// <summary>
        /// 
        /// </summary>
        BRANCHEA,
        /// <summary>
        /// 
        /// </summary>
        SAP,
        /// <summary>
        /// 
        /// </summary>
        ZINSTAB,
        /// <summary>
        /// 
        /// </summary>
        RW,
        /// <summary>
        /// 
        /// </summary>
        KALKCHK,
        /// <summary>
        /// 
        /// </summary>
        KALKPOOL,
        /// <summary>
        /// 
        /// </summary>
        OBTYP,
        /// <summary>
        /// 
        /// </summary>
        ZFORMEL,
        /// <summary>
        /// 
        /// </summary>
        RWTAB,
        /// <summary>
        /// 
        /// </summary>
        RWKM,
        /// <summary>
        /// 
        /// </summary>
        KALKTYP,
        /// <summary>
        /// 
        /// </summary>
        KALKTYPPOS,
        /// <summary>
        /// 
        /// </summary>
        UGT,
        /// <summary>
        /// 
        /// </summary>
        DBT,
        /// <summary>
        /// 
        /// </summary>
        MGT,
        /// <summary>
        /// 
        /// </summary>
        BUDGET,
        /// <summary>
        /// 
        /// </summary>
        BBGV,
        /// <summary>
        /// 
        /// </summary>
        BBTYP,
        /// <summary>
        /// 
        /// </summary>
        FIBUAWT,
        /// <summary>
        /// 
        /// </summary>
        FIBUNM,
        /// <summary>
        /// 
        /// </summary>
        FIBULVL1,
        /// <summary>
        /// 
        /// </summary>
        FIBULVL2,
        /// <summary>
        /// 
        /// </summary>
        FIBULVL3,
        /// <summary>
        /// 
        /// </summary>
        FIBULVL4,
        /// <summary>
        /// 
        /// </summary>
        FIBULVL5,
        /// <summary>
        /// 
        /// </summary>
        KURS,
        /// <summary>
        /// 
        /// </summary>
        KURSTAB,
        /// <summary>
        /// 
        /// </summary>
        NOTIZ,
        /// <summary>
        /// 
        /// </summary>
        KSUMLAGE,
        /// <summary>
        /// 
        /// </summary>
        RNZAHL,
        /// <summary>
        /// 
        /// </summary>
        HOBBY,
        /// <summary>
        /// 
        /// </summary>
        VTVWRT,
        /// <summary>
        /// 
        /// </summary>
        ANTRAG,
        /// <summary>
        /// 
        /// </summary>
        ANTOB,
        /// <summary>
        /// 
        /// </summary>
        ANTOBSL,
        /// <summary>
        /// 
        /// </summary>
        BERICHTE,
        /// <summary>
        /// 
        /// </summary>
        PERIOD,
        /// <summary>
        /// 
        /// </summary>
        ANGEBOT,
        /// <summary>
        /// 
        /// </summary>
        OBOPTION,
        /// <summary>
        /// 
        /// </summary>
        ANTOPTION,
        /// <summary>
        /// 
        /// </summary>
        AUFTRAG,
        /// <summary>
        /// 
        /// </summary>
        AUFTRPOS,
        /// <summary>
        /// 
        /// </summary>
        ANTOBOPTION,
        /// <summary>
        /// 
        /// </summary>
        SICHTYP,
        /// <summary>
        /// 
        /// </summary>
        ANGOPTION,
        /// <summary>
        /// 
        /// </summary>
        RNMAHN,
        /// <summary>
        /// 
        /// </summary>
        ANTKALK,
        /// <summary>
        /// 
        /// </summary>
        ANTOBSICH,
        /// <summary>
        /// 
        /// </summary>
        ANTOBHD,
        /// <summary>
        /// 
        /// </summary>
        ANTOBINI,
        /// <summary>
        /// 
        /// </summary>
        PUSER,
        /// <summary>
        /// 
        /// </summary>
        FIPERIOD,
        /// <summary>
        /// 
        /// </summary>
        FIJOUR,
        /// <summary>
        /// 
        /// </summary>
        KONF,
        /// <summary>
        /// 
        /// </summary>
        ANTPROV,
        /// <summary>
        /// 
        /// </summary>
        PROV,
        /// <summary>
        /// 
        /// </summary>
        REKLA,
        /// <summary>
        /// 
        /// </summary>
        MARKTAB,
        /// <summary>
        /// 
        /// </summary>
        AKTION,
        /// <summary>
        /// 
        /// </summary>
        SCHADEN,
        /// <summary>
        /// 
        /// </summary>
        MYCALC,
        /// <summary>
        /// 
        /// </summary>
        MASSTYPOS,
        /// <summary>
        /// 
        /// </summary>
        FIBUJOBS,
        /// <summary>
        /// 
        /// </summary>
        REKALK,
        /// <summary>
        /// 
        /// </summary>
        FIBUNM1,
        /// <summary>
        /// 
        /// </summary>
        MYCALCFS,
        /// <summary>
        /// 
        /// </summary>
        FAHRER,
        /// <summary>
        /// 
        /// </summary>
        TKARTE,
        /// <summary>
        /// 
        /// </summary>
        FZNGR,
        /// <summary>
        /// 
        /// </summary>
        ABRREGEL,
        /// <summary>
        /// 
        /// </summary>
        SREQUEST,
        /// <summary>
        /// 
        /// </summary>
        RVTRS,
        /// <summary>
        /// 
        /// </summary>
        ANTOBSLPOS,
        /// <summary>
        /// 
        /// </summary>
        FSTYP,
        /// <summary>
        /// 
        /// </summary>
        FZART,
        /// <summary>
        /// 
        /// </summary>
        TPCODE,
        /// <summary>
        /// 
        /// </summary>
        TPCNM,
        /// <summary>
        /// 
        /// </summary>
        VSTYP,
        /// <summary>
        /// 
        /// </summary>
        KMSTAND,
        /// <summary>
        /// 
        /// </summary>
        RVTLIST,
        /// <summary>
        /// 
        /// </summary>
        UKZ,
        /// <summary>
        /// 
        /// </summary>
        WFSYS,
        /// <summary>
        /// 
        /// </summary>
        VSADD,
        /// <summary>
        /// 
        /// </summary>
        DEBITOR,
        /// <summary>
        /// 
        /// </summary>
        ANTOBVM,
        /// <summary>
        /// 
        /// </summary>
        WAEHRTAB,
        /// <summary>
        /// 
        /// </summary>
        KREDITOR,
        /// <summary>
        /// 
        /// </summary>
        BELEGART,
        /// <summary>
        /// 
        /// </summary>
        ZINSLZ,
        /// <summary>
        /// 
        /// </summary>
        MIS,
        /// <summary>
        /// 
        /// </summary>
        PENKONTO,
        /// <summary>
        /// 
        /// </summary>
        NKK,
        /// <summary>
        /// 
        /// </summary>
        VTKUEND,
        /// <summary>
        /// 
        /// </summary>
        SPGLREK,
        /// <summary>
        /// 
        /// </summary>
        KD,
        /// <summary>
        /// 
        /// </summary>
        KKKSZINS,
        /// <summary>
        /// 
        /// </summary>
        KKKHZINS,
        /// <summary>
        /// 
        /// </summary>
        KAS,
        /// <summary>
        /// 
        /// </summary>
        ASDA,
        /// <summary>
        /// 
        /// </summary>
        VTOBSICH,
        /// <summary>
        /// 
        /// </summary>
        WFJLVL1,
        /// <summary>
        /// 
        /// </summary>
        VTRUEK,
        /// <summary>
        /// 
        /// </summary>
        BB,
        /// <summary>
        /// 
        /// </summary>
        BBPOS,
        /// <summary>
        /// 
        /// </summary>
        VTOBSLPOS,
        /// <summary>
        /// 
        /// </summary>
        PEOBLIGO,
        /// <summary>
        /// 
        /// </summary>
        BONITAET,
        /// <summary>
        /// 
        /// </summary>
        KSTPLAN,
        /// <summary>
        /// 
        /// </summary>
        WFTODO,
        /// <summary>
        /// 
        /// </summary>
        KONKURS,
        /// <summary>
        /// 
        /// </summary>
        FITEXT,
        /// <summary>
        /// 
        /// </summary>
        ANGOBSLPOS,
        /// <summary>
        /// 
        /// </summary>
        ANGOBSL,
        /// <summary>
        /// 
        /// </summary>
        VTABL,
        /// <summary>
        /// 
        /// </summary>
        RNZINS,
        /// <summary>
        /// 
        /// </summary>
        TKMAKER,
        /// <summary>
        /// 
        /// </summary>
        KASSE,
        /// <summary>
        /// 
        /// </summary>
        TODO,
        /// <summary>
        /// 
        /// </summary>
        WFJLVL3,
        /// <summary>
        /// 
        /// </summary>
        CICFLOG,
        /// <summary>
        /// 
        /// </summary>
        ZEK,
        /// <summary>
        /// 
        /// </summary>
        NEWZEK,
        /// <summary>
        /// 
        /// </summary>
        CMBULK,
        /// <summary>
        /// 
        /// </summary>
        IT,
        /// <summary>
        /// 
        /// </summary>
        NKKABSCHL,
        /// <summary>
        /// 
        /// </summary>
        KREMO,
        /// <summary>
        /// 
        /// </summary>
        WFLB,
        /// <summary>
        /// 
        /// </summary>
        WFTZVAR,
        /// <summary>
        /// 
        /// </summary>
        ANGOBSICH,
        /// <summary>
        /// 
        /// </summary>
        BONIPOS,
        /// <summary>
        /// 
        /// </summary>
        MASSOPTION,
        /// <summary>
        /// 
        /// </summary>
        ANGOB,
        /// <summary>
        /// 
        /// </summary>
        RNNOTES,
        /// <summary>
        /// 
        /// </summary>
        SUBVTYP,
        /// <summary>
        /// 
        /// </summary>
        SUBV,
        /// <summary>
        /// 
        /// </summary>
        OBVWRT,
        /// <summary>
        /// 
        /// </summary>
        ANGKALK,
        /// <summary>
        /// 
        /// </summary>
        ANGVAR,
        /// <summary>
        /// 
        /// </summary>
        ANGKV,
        /// <summary>
        /// 
        /// </summary>
        KNE,
        /// <summary>
        /// 
        /// </summary>
        PERSONIT,
        /// <summary>
        /// 
        /// </summary>
        PEROLE,
        /// <summary>
        /// 
        /// </summary>
        ROLETYPE
    }
}
