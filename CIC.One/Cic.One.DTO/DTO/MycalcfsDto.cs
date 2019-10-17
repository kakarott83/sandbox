using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Servicekalkulation WKT
    /// </summary>
    public class MycalcfsDto : EntityDto
    {
        public long sysmycalcfs { get; set; }
        public DateTime? valuta { get; set; }
        public double servicefee { get; set; }
        public double bailprice { get; set; }
        public double handoverprice { get; set; }
        public long sysvstyp { get; set; }
        public double sumvk { get; set; }
        public double sumhp { get; set; }
        public double passinsurance { get; set; }
        public double parkdamage { get; set; }
        public double securities { get; set; }
        public int taxfreeflag { get; set; }
        public long sysstaat { get; set; }
        public int originoption { get; set; }
        public int fuelflag { get; set; }
        public double fuelprice { get; set; }
        public int tiresflag { get; set; }
        public String stirescode { get; set; }
        public String stirestext { get; set; }
        public int stirescount { get; set; }
        public double stiresprice { get; set; }
        public String wtirescode { get; set; }
        public String wtirestext { get; set; }
        public int wtirescount { get; set; }
        public double wtiresprice { get; set; }
        public double ttiresprice { get; set; }
        public double tiresstorage { get; set; }
        public int tiresstorageflag { get; set; }
        public int bservicecount { get; set; }
        public int bserviceflag { get; set; }
        public double bserviceprice { get; set; }
        public double maintenance { get; set; }
        public int maintenanceflag { get; set; }
        public double maintenancemkm { get; set; }
        public double maintenancelkm { get; set; }
        public int repcarcount { get; set; }
        public double repcarprice { get; set; }
        public int repcarflag { get; set; }
        public double delivery { get; set; }
        public int deliveryflag { get; set; }
        public int carwashcount { get; set; }
        public int carwashflag { get; set; }
        public double carwashprice { get; set; }
        public int mgmntfeeflag { get; set; }
        public int vignettenflag { get; set; }
        public double vignetten { get; set; }
        public int vatflag { get; set; }
        public long fuelsysfstyp { get; set; }
        public int fuelzinsflag { get; set; }
        public long tiressysfstyp { get; set; }
        public int tireszinsflag { get; set; }
        public double tireszins { get; set; }
        public long tiresstosysfstyp { get; set; }
        public int tiresstozinsflag { get; set; }
        public double tiresstozins { get; set; }
        public long bservsysfstyp { get; set; }
        public int bservflag { get; set; }
        public double bservzins { get; set; }
        public long maintsysfstyp { get; set; }
        public int maintzinsflag { get; set; }
        public double maintzins { get; set; }
        public long repsysfstyp { get; set; }
        public int repzinsflag { get; set; }
        public double repzins { get; set; }
        public long delivsysfstyp { get; set; }
        public int delivzinsflag { get; set; }
        public double delivzins { get; set; }
        public long carwsysfstyp { get; set; }
        public int carwzinsflag { get; set; }
        public double carwzins { get; set; }
        public long vigsysfstyp { get; set; }
        public int vigzinsflag { get; set; }
        public double vigzins { get; set; }
        public long inssysfstyp { get; set; }
        public int inszinsflag { get; set; }
        public double inszins { get; set; }
        public long sysfzngr { get; set; }
        public double taxprice { get; set; }
        public double fahrerbet { get; set; }
        public double fahrerbetp { get; set; }
        public double avgfuelcons { get; set; }
        public String stirescodev { get; set; }
        public int stiresmodv { get; set; }
        public String stirestextv { get; set; }
        public int stirescountv { get; set; }
        public double stirespricev { get; set; }
        public String stirescodeh { get; set; }
        public int stiresmodh { get; set; }
        public String stirestexth { get; set; }
        public int stirescounth { get; set; }
        public double stirespriceh { get; set; }
        public int wholeweelflag { get; set; }
        public String wtirescodev { get; set; }
        public int wtiresmodv { get; set; }
        public String wtirestextv { get; set; }
        public int wtirescountv { get; set; }
        public double wtirespricev { get; set; }
        public String wtirescodeh { get; set; }
        public int wtiresmodh { get; set; }
        public String wtirestexth { get; set; }
        public int wtirescounth { get; set; }
        public double wtirespriceh { get; set; }
        public String rimscodev { get; set; }
        public int rimscountv { get; set; }
        public double rimspricev { get; set; }
        public String rimscodeh { get; set; }
        public int rimscounth { get; set; }
        public double rimspriceh { get; set; }
        public int tiresaddflag { get; set; }
        public double tiresaddition { get; set; }
        public double anabmeldung { get; set; }
        public int anabmldflag { get; set; }
        public double mgmgeb { get; set; }
        public int mgmgebflag { get; set; }
        public int fuelpauschflag { get; set; }
        public double extrasprice { get; set; }
        public int extrasflag { get; set; }
        public int hpflag { get; set; }
        public int vkflag { get; set; }
        public double rechtschutz { get; set; }
        public int rechtschutzflag { get; set; }
        public double motorvs { get; set; }
        public int motorvsflag { get; set; }
        public double insassen { get; set; }
        public int insassenflag { get; set; }
        public int tiresfixflag { get; set; }
        public int maintfixflag { get; set; }
        public double mehrkm { get; set; }
        public double mehrkmp { get; set; }
        public double minderkm { get; set; }
        public double minderkmp { get; set; }


        /// <summary>
        /// Wartung/Reparatur Standard
        /// </summary>
        public double maintenancedef {get;set;}
        /// <summary>
        /// Reifenrate Standard
        /// </summary>
        public double stirespricedef { get; set; }

        /// <summary>
        /// Reifen Wechselintervall
        /// </summary>
        public long tireschangeinterval { get; set; }

        /// <summary>
        /// Schadensmanagement Gebühr
        /// </summary>
        public double schadenmgmtfee { get; set; }
        /// <summary>
        /// Schadensmanagement Flag
        /// </summary>
        public int schadenmgmtfeeflag { get; set; }


        public override long getEntityId()
        {
            return sysmycalcfs;
        }
    }
}
