using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class ProduktInfoDto : EntityDto
    {
        /// <summary>
        /// sysprproduct
        /// </summary>
        public long sysprproduct { get; set; }
        /// <summary>
        /// Produktname
        /// </summary>
        public string productName { get; set; }

        /// <summary>
        /// verfügbare Versicherungen
        /// </summary>
        public List<AvailableServiceDto> verfVersicherungen { get; set; }
        /// <summary>
        /// Versicherungen
        /// </summary>
        public List<AngAntVsDto> versicherungen { get; set; }

        /// <summary>
        /// lz
        /// </summary>
        public long laufzeit { get; set; }
        /// <summary>
        /// ahk
        /// </summary>
        public double barkaufpreis { get; set; }
        /// <summary>
        /// sz
        /// </summary>
        public double anzahlungEintausch { get; set; }
        /// <summary>
        /// rw
        /// </summary>
        public double restrate { get; set; }
        /// <summary>
        /// zinsrap
        /// </summary>
        public double effJahreszins { get; set; }
        /// <summary>
        /// ll
        /// </summary>
        public long kmProJahr { get; set; }
        /// <summary>
        /// rate
        /// </summary>
        public double rate { get; set; }
        /// <summary>
        /// rate
        /// </summary>
        public double rateBrutto { get; set; }
        /// <summary>
        /// rw
        /// </summary>
        public double indikativerRestwert { get; set; }
        /// <summary>
        /// rw
        /// </summary>
        public double indikativerRestwertBrutto { get; set; }
        /// <summary>
        /// grund
        /// </summary>
        public string grundlage { get; set; }

        /// <summary>
        /// bgextern
        /// </summary>
        public double kreditlimit { get; set; }
        /// <summary>
        /// auszahlung
        /// </summary>
        public double auszahlungBank { get; set; }
        /// <summary>
        /// initladung
        /// </summary>
        public double auszahlungKarte { get; set; }
        /// <summary>
        /// zinsrap
        /// </summary>
        public double effZinssatz { get; set; }
        /// <summary>
        /// card.emboss
        /// </summary>
        public string nameAufKarte { get; set; }

        /// <summary>
        /// bgextern
        /// </summary>
        public double kreditbetrag { get; set; }

        /// <summary>
        /// bgintern
        /// </summary>
        public double bgintern { get; set; }

        /// <summary>
        /// auszahlungsbetrag
        /// </summary>
        public double auszahlungsbetrag1 { get; set; }

        public int auszahlungsart { get; set; }
        /// <summary>
        /// angebot.kartennummer
        /// </summary>
        public string kartennummer { get; set; }
        /// <summary>
        /// syswaehrung
        /// </summary>
        public string waehrung { get; set; }

        /// <summary>
        /// rw
        /// </summary>
        public double restwert { get; set; }
        /// <summary>
        /// depot
        /// </summary>
        public double kaution { get; set; }
        /// <summary>
        /// zins
        /// </summary>
        public double nominalJahreszins { get; set; }

        public long syskdtyp { get; set; }
        public long sysobart { get; set; }
        /// <summary>
        /// sysobtyp
        /// </summary>
        public long sysobtyp { get; set; }

        public override long getEntityId()
        {
            return sysprproduct;
        }


        /// <summary>
        /// instsband
        /// </summary>
        public IntsbandDto intsband { get; set; }

        /// <summary>
        /// Vertriebsweg
        /// </summary>
        public string vertriebsweg { get; set; }
        public string vartcode { get; set; }
    }
}
