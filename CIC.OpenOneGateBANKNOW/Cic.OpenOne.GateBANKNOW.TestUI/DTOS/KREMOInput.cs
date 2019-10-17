using System.ComponentModel;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DTOS
{
    /// <summary>
    /// Kremo Input Data Transfer Object
    /// </summary>
    public class KREMOInput : INotifyPropertyChanged
    {
        private double _GebDatum;
        private double _Anredecode;
        private double _Kalkcode;
        private double _Glz;
        private double _Kreditsumme;
        private double _Rw;
        private double _Zins;
        private double _Zinsnomflag;
        private double _Einkbrutto;
        private double _Einknetto;
        private double _Nebeneinkbrutto;
        private double _Nebeneinknetto;
        private double _Famstandcode;
        private double _Grundcode;
        private double _Kantoncode;
        private double _Anzkind1;
        private double _Anzkind2;
        private double _Anzkind3;
        private double _Miete;
        private double _Plz;
        private double _Unterhalt;
        private double _Qstflag;
        private double _Anzkind4;
        private double _GebDatum2;
        private double _Anredecode2;
        private double _Einkbrutto2;
        private double _Einknetto2;
        private double _Nebeneinkbrutto2;
        private double _Nebeneinknetto2;
        private double _Famstandcode2;
        private double _Kantoncode2;
        private double _Plz2;
        private double _Unterhalt2;
        private double _Qstflag2;

        /// <summary>
        /// Fininstcode
        /// </summary>
        public double _Fininstcode;

        /// <summary>
        /// Standard constructor
        /// </summary>
        public KREMOInput()
        {
        }

        /// <summary>
        /// Format: YYYYMMDD
        /// </summary>
        public double GebDatum
        {
            get { return _GebDatum; }
            set
            {
                _GebDatum = value;
                Changed("GebDatum");
            }
        }

        ///<summary>
        /// 0 = Herr / 1 = Frau
        /// </summary>
        public double Anredecode
        {
            get { return _Anredecode; }
            set
            {
                _Anredecode = value;
                Changed("Anredecode");
            }
        }

        /// <summary>
        /// 0=Kredit, 1=Leasing, 2=Teilzahler
        /// </summary>
        public double Kalkcode
        {
            get { return _Kalkcode; }
            set
            {
                _Kalkcode = value;
                Changed("Kalkcode");
            }
        }

        /// <summary>
        /// Gesamtlaufzeit neuer Kredit/Leasing
        /// </summary>
        public double Glz
        {
            get { return _Glz; }
            set
            {
                _Glz = value;
                Changed("Glz");
            }
        }

        /// <summary>
        /// Betrag neuer Kredit/Leasing
        /// </summary>
        public double Kreditsumme
        {
            get { return _Kreditsumme; }
            set
            {
                _Kreditsumme = value;
                Changed("Kreditsumme");
            }
        }

        /// <summary>
        /// Restwert neues Leasing
        /// </summary>
        public double Rw
        {
            get { return _Rw; }
            set
            {
                _Rw = value;
                Changed("Rw");
            }
        }

        /// <summary>
        /// Zinssatz neuer Kredit/Leasing in Prozent
        /// </summary>
        public double Zins
        {
            get { return _Zins; }
            set
            {
                _Zins = value;
                Changed("Zins");
            }
        }

        /// <summary>
        /// 0=nominal, 1=effektiv
        /// </summary>
        public double Zinsnomflag
        {
            get { return _Zinsnomflag; }
            set
            {
                _Zinsnomflag = value;
                Changed("Zinsnomflag");
            }
        }

        /// <summary>
        /// Bruttoeinkommen
        /// </summary>
        public double Einkbrutto
        {
            get { return _Einkbrutto; }
            set
            {
                _Einkbrutto = value;
                Changed("Einkbrutto");
            }
        }

        /// <summary>
        /// Bruttoeinkommen
        /// </summary>
        public double Einknetto
        {
            get { return _Einknetto; }
            set
            {
                _Einknetto = value;
                Changed("Einknetto");
            }
        }

        /// <summary>
        /// Nebeneinkommen brutto
        /// </summary>
        public double Nebeneinkbrutto
        {
            get { return _Nebeneinkbrutto; }
            set
            {
                _Nebeneinkbrutto = value;
                Changed("Nebeneinkbrutto");
            }
        }

        /// <summary>
        /// Nebeneinkommen netto
        /// </summary>
        public double Nebeneinknetto
        {
            get { return _Nebeneinknetto; }
            set
            {
                _Nebeneinknetto = value;
                Changed("Nebeneinknetto");
            }
        }

        /// <summary>
        /// 0=ledig, 1=verheiratet, 2=geschieden, 3=gerichtlich getrennt, 4=verwitwet, 5=eingetragene Partnerschaft, 
        /// 6=gerichtlich aufgelöste Partnerschaft (wird wie geschieden behandelt)
        /// </summary>
        public double Famstandcode
        {
            get { return _Famstandcode; }
            set
            {
                _Famstandcode = value;
                Changed("Famstandcode");
            }
        }

        /// <summary>
        /// 0=alleinstehende, 1=alleinstehende in Haushaltgemeinschaft mit Erwachsenem, 
        /// 2=Ehepaar in dauernder Haushaltgemeinschaft, 3=alleinerziehende mit Unterstützungspflichten ohne Haushaltgemeinschaft,
        /// 4=alleinerziehend mit Haushaltgemeinschaft
        /// </summary>
        public double Grundcode
        {
            get { return _Grundcode; }
            set
            {
                _Grundcode = value;
                Changed("Grundcode");
            }
        }

        /// <summary>
        /// 0=AG, 1=AR, 2=AI, 3=BS, 4=BL, 5=BE, 6=FR, 7=GE, 8=GL, 9=GR, 10=JU, 11=LU, 12=NE, 13=NW, 14=OW, 15=SH, 16=SZ, 17=SO, 18=SG,
        /// 19=TG, 20=TI, 21=UR, 22=VS, 23=VD, 24=ZG, 25=ZH, 26=FL
        /// </summary>
        public double Kantoncode
        {
            get { return _Kantoncode; }
            set
            {
                _Kantoncode = value;
                Changed("Kantoncode");
            }
        }

        /// <summary>
        /// Anzahl Kinder bis 6 Jahre
        /// </summary>
        public double Anzkind1
        {
            get { return _Anzkind1; }
            set
            {
                _Anzkind1 = value;
                Changed("Anzkind1");
            }
        }

        /// <summary>
        /// Anzahl Kinder 11 bis 12 Jahre
        /// </summary>
        public double Anzkind2
        {
            get { return _Anzkind2; }
            set
            {
                _Anzkind2 = value;
                Changed("Anzkind2");
            }
        }

        /// <summary>
        /// Anzahl Kinder über 12 Jahre
        /// </summary>
        public double Anzkind3
        {
            get { return _Anzkind3; }
            set
            {
                _Anzkind3 = value;
                Changed("Anzkind3");
            }
        }

        /// <summary>
        /// Miete
        /// </summary>
        public double Miete
        {
            get { return _Miete; }
            set
            {
                _Miete = value;
                Changed("Miete");
            }
        }

        /// <summary>
        /// PLZ
        /// </summary>
        public double Plz
        {
            get { return _Plz; }
            set
            {
                _Plz = value;
                Changed("Plz");
            }
        }

        ///<summary>
        /// Unterstützungs-/Unterhaltsbeiträge
        /// </summary>
        public double Unterhalt
        {
            get { return _Unterhalt; }
            set
            {
                _Unterhalt = value;
                Changed("Unterhalt");
            }
        }

        /// <summary>
        /// Quellensteuer-ID: 0=nein, 1=ja
        /// </summary>
        public double Qstflag
        {
            get { return _Qstflag; }
            set
            {
                _Qstflag = value;
                Changed("Qstflag");
            }
        }

        /// <summary>
        /// Anzahl Kinder 7 bis 10 Jahre Der Betrag der gemäss Anzahl Kinder in die Haushaltsrechnung einfliesst, wird in die
        /// bestehenden KLKKDN.KIND1 (Kanton umgestellt) bzw. KLKKDN.KIND2 (Kanton noch nicht umgestellt) Felder zurückgegeben.
        /// </summary>
        public double Anzkind4
        {
            get { return _Anzkind4; }
            set
            {
                _Anzkind4 = value;
                Changed("Anzkind4");
            }
        }

        /// <summary>
        /// Format: YYYYMMDD
        /// </summary>
        public double GebDatum2
        {
            get { return _GebDatum2; }
            set
            {
                _GebDatum2 = value;
                Changed("GebDatum2");
            }
        }

        /// <summary>
        /// 0 = Herr / 1 = Frau
        /// </summary>
        public double Anredecode2
        {
            get { return _Anredecode2; }
            set
            {
                _Anredecode2 = value;
                Changed("Anredecode2");
            }
        }

        /// <summary>
        /// Einkommensbrutto 2
        /// </summary>
        public double Einkbrutto2
        {
            get { return _Einkbrutto2; }
            set
            {
                _Einkbrutto2 = value;
                Changed("Einkbrutto2");
            }
        }

        /// <summary>
        /// Einkommensnetto 2
        /// </summary>
        public double Einknetto2
        {
            get { return _Einknetto2; }
            set
            {
                _Einknetto2 = value;
                Changed("Einknetto2");
            }
        }

        /// <summary>
        /// Nebeneinkommen Brutto 2
        /// </summary>
        public double Nebeneinkbrutto2
        {
            get { return _Nebeneinkbrutto2; }
            set
            {
                _Nebeneinkbrutto2 = value;
                Changed("Nebeneinkbrutto2");
            }
        }

        /// <summary>
        /// Nebeneinkommen Netto 2
        /// </summary>
        public double Nebeneinknetto2
        {
            get { return _Nebeneinknetto2; }
            set
            {
                _Nebeneinknetto2 = value;
                Changed("Nebeneinknetto2");
            }
        }

        /// <summary>
        /// 0=ledig, 1=verheiratet, 2=geschieden, 3=gerichtlich getrennt, 4=verwitwet, 5=eingetragene Partnerschaft, 
        /// 6=gerichtlich aufgelöste Partnerschaft (wird wie geschieden behandelt)
        /// </summary>
        public double Famstandcode2
        {
            get { return _Famstandcode2; }
            set
            {
                _Famstandcode2 = value;
                Changed("Famstandcode2");
            }
        }

        /// <summary>
        /// Kanton code
        /// </summary>
        public double Kantoncode2
        {
            get { return _Kantoncode2; }
            set
            {
                _Kantoncode2 = value;
                Changed("Kantoncode2");
            }
        }

        /// <summary>
        /// PLZ 2
        /// </summary>
        public double Plz2
        {
            get { return _Plz2; }
            set
            {
                _Plz2 = value;
                Changed("Plz2");
            }
        }

        /// <summary>
        /// Unterhalt 2
        /// </summary>
        public double Unterhalt2
        {
            get { return _Unterhalt2; }
            set
            {
                _Unterhalt2 = value;
                Changed("Unterhalt2");
            }
        }

        /// <summary>
        /// Ost-Flag 2
        /// </summary>
        public double Qstflag2
        {
            get { return _Qstflag2; }
            set
            {
                _Qstflag2 = value;
                Changed("Qstflag2");
            }
        }

        /// <summary>
        /// Mandant
        /// </summary>
        public double Fininstcode
        {
            get { return _Fininstcode; }
            set
            {
                _Fininstcode = value;
                Changed("Fininstcode");
            }
        }

        /// <summary>
        /// Property auf Änderung Prüfen
        /// </summary>
        /// <param name="propertyName">Property</param>
        private void Changed(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Event Deklaration
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}