// OWNER JJ, 30-11-2009
using System.Collections.Generic;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class ITDto
    {
        #region Enums
        public enum Sex : int
        {
            Unknown = 0,
            Male = 1,
            Female = 2
        }
        #endregion

        #region Ids properties

        [System.Runtime.Serialization.DataMember]
        public long? SYSIT
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSPERSON
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSLAND
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long? SYSLAND2
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSSTAAT
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSLANDNAT
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSCTLANG
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSBRANCHE
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSLANDAG
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public long? SYSKDTYP
        {
            get;
            set;
        }
        #endregion

        #region Flag properties
        // True: private
        // False: company

        [System.Runtime.Serialization.DataMember]
        public bool PRIVATFLAG
        {
            get;
            set;
        }
        #endregion

        #region Properties
        // Salutation
        // Privat: Name, Anrede
        // Einzelunternehmen: Name, Anrede
        // Unternehmen: -

        [System.Runtime.Serialization.DataMember]
        public string ANREDE
        {
            get;
            set;
        }

        // Title        
        // Privat: Name, Titel
        // Einzelunternehmen: Name, Titel
        // Unternehmen: -

        [System.Runtime.Serialization.DataMember]
        public string TITEL
        {
            get;
            set;
        }

        // Suffix
        // Privat: Name, Suffix
        // Einzelunternehmen: Name, Suffix
        // Unternehmen: -

        [System.Runtime.Serialization.DataMember]
        public string SUFFIX
        {
            get;
            set;
        }

        // First name
        // Privat: Name, Vorname 
        // Einzelunternehmen: Name, Vorname 
        // Unternehmen: Name, Firmenname 1

        [System.Runtime.Serialization.DataMember]
        public string VORNAME
        {
            get;
            set;
        }

        // Last name
        // Privat: Name, Name  
        // Einzelunternehmen: Name, Name  
        // Unternehmen: Name, Firmenname 2

        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        // Addition
        // Privat: -
        // Einzelunternehmen: Firmendaten, Firmenname
        // Unternehmen: Name, Firmenname 3

        [System.Runtime.Serialization.DataMember]
        public string ZUSATZ
        {
            get;
            set;
        }

        // Sex
        // Privat: Name, Geschlecht  
        // Einzelunternehmen: Name, Geschlecht
        // Unternehmen: -

        [System.Runtime.Serialization.DataMember]
        public Sex GESCHLECHT
        {
            get;
            set;
        }

        /*
         *  private Sex _geschlecht;
        [System.Runtime.Serialization.DataMember]
        public Sex GESCHLECHT
        {
            get { return _geschlecht; }
            set {
                try
                {
                    _geschlecht = (Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex)EnumUtil.DeliverDefinedOrDefault(typeof(Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex), value, Cic.OpenLease.ServiceAccess.DdOl.ITDto.Sex.Unknown);
                }
                catch (Exception) { }
            }
        }
        */
        // Date of birth
        // Privat: Name, Geburtsdatum
        // Einzelunternehmen: Name, Geburtsdatum
        // Unternehmen: -

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? GEBDATUM
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string GEBORT
        {
            get;
            set;
        }
        // Legal form
        // Privat: -
        // Einzelunternehmen: Firmendaten, Rechtsform
        // Unternehmen: Firmendaten, Rechtsform

        [System.Runtime.Serialization.DataMember]
        public int? RECHTSFORM
        {
            get;
            set;
        }

        // Foundation
        // Privat: -
        // Einzelunternehmen: Firmendaten, Gründungsdatum
        // Unternehmen: Firmendaten, Gründungsdatum

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? GRUENDUNG
        {
            get;
            set;
        }

        // UID
        // Privat: -
        // Einzelunternehmen: Firmendaten, UID Nr.
        // Unternehmen: Firmendaten, UID Nr.

        [System.Runtime.Serialization.DataMember]
        public string UIDNUMMER
        {
            get;
            set;
        }

        // Traderegister number
        // Privat: -
        // Einzelunternehmen: Firmendaten, Firmenbuchnummer
        // Unternehmen: Firmendaten, Firmenbuchnummer

        [System.Runtime.Serialization.DataMember]
        public string HREGISTER
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string HREGISTERORT { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string AMTSGERICHT
        {
            get;
            set;
        }

        // Street
        // Privat: Adresse, Straße
        // Einzelunternehmen: Adresse, Straße
        // Unternehmen: Adresse, Straße

        [System.Runtime.Serialization.DataMember]
        public string STRASSE
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string STRASSE2
        {
            get;
            set;
        }

        // No.
        // Privat: Adresse, Hausnummer
        // Einzelunternehmen: Adresse, Hausnummer
        // Unternehmen: Adresse, Hausnummer

        [System.Runtime.Serialization.DataMember]
        public string HSNR
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string HSNR2
        {
            get;
            set;
        }

        // ZIP code
        // Privat: Adresse, PLZ
        // Einzelunternehmen: Adresse, PLZ
        // Unternehmen: Adresse, PLZ

        [System.Runtime.Serialization.DataMember]
        public string PLZ
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string PLZ2
        {
            get;
            set;
        }

        // Location
        // Privat: Adresse, Land
        // Einzelunternehmen: Adresse, Land
        // Unternehmen: Adresse, Land

        [System.Runtime.Serialization.DataMember]
        public string ORT
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string ORT2
        {
            get;
            set;
        }

        // Type of identification
        // Privat: Legitimationsdaten, Ausweisart 
        // Einzelunternehmen: Legitimationsdaten, Ausweisart 
        // Unternehmen: Legitimationsdaten, Ausweisart

        [System.Runtime.Serialization.DataMember]
        public int? AUSWEISART
        {
            get;
            set;
        }

        /// <summary>
        /// Ausländischer Ausweiscode
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string AUSLAUSWEISCODE { get; set; }

        // Identification number
        // Privat: Legitimationsdaten, Ausweisnummer  
        // Einzelunternehmen: Legitimationsdaten, Ausweisnummer 
        // Unternehmen: Legitimationsdaten, Ausweisart

        [System.Runtime.Serialization.DataMember]
        public string AUSWEISNR
        {
            get;
            set;
        }

        // A Identification issued by
        // Privat: Legitimationsdaten, Ausstellungsbehörde    
        // Einzelunternehmen: Legitimationsdaten, Ausstellungsbehörde   
        // Unternehmen: Legitimationsdaten, Ausstellungsbehörde 

        [System.Runtime.Serialization.DataMember]
        public string AUSWEISBEHOERDE
        {
            get;
            set;
        }

        // The Place of publication Identification
        // Privat: Legitimationsdaten, Ausstellungsort   
        // Einzelunternehmen: Legitimationsdaten, Ausstellungsort  
        // Unternehmen: Legitimationsdaten, Ausstellungsort 

        [System.Runtime.Serialization.DataMember]
        public string AUSWEISORT
        {
            get;
            set;
        }

        // The Date of publication Identification
        // Privat: Legitimationsdaten, Ausstellungsdatum    
        // Einzelunternehmen: Legitimationsdaten, Ausstellungsdatum   
        // Unternehmen: Legitimationsdaten, Ausstellungsdatum  

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? AUSWEISDATUM
        {
            get;
            set;
        }

        // Expiration date
        // Privat: Legitimationsdaten, gültig bis     
        // Einzelunternehmen: Legitimationsdaten, gültig bis    
        // Unternehmen: Legitimationsdaten, gültig bis  

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? AUSWEISABLAUF
        {
            get;
            set;
        }

        // Checked/Examined on
        // Privat: Legitimationsdaten, geprüft am  
        // Einzelunternehmen: Legitimationsdaten, geprüft am     
        // Unternehmen: Legitimationsdaten, geprüft am   

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? LEGITDATUM
        {
            get;
            set;
        }

        // Checked/Examined by
        // Privat: Legitimationsdaten, geprüft von 
        // Einzelunternehmen: Legitimationsdaten, geprüft von    
        // Unternehmen: Legitimationsdaten, geprüft von        

        [System.Runtime.Serialization.DataMember]
        public string LEGITABNEHMER
        {
            get;
            set;
        }

        // Social Security Number
        // Privat: Legitimationsdaten, Sozialversicherungsnummer 
        // Einzelunternehmen: Legitimationsdaten, Sozialversicherungsnummer 
        // Unternehmen: Legitimationsdaten, Sozialversicherungsnummer       

        [System.Runtime.Serialization.DataMember]
        public string SVNR
        {
            get;
            set;
        }

        // Telephone
        // Privat: Kontaktdaten, Telefon geschäftlich 
        // Einzelunternehmen: Kontaktdaten, Telefon geschäftlich 
        // Unternehmen: Kontaktdaten, Telefon 

        [System.Runtime.Serialization.DataMember]
        public string TELEFON
        {
            get;
            set;
        }

        // Telephone private
        // Privat: Kontaktdaten, Telefon Privat 
        // Einzelunternehmen: Kontaktdaten, Telefon Privat  
        // Unternehmen: -

        [System.Runtime.Serialization.DataMember]
        public string PTELEFON
        {
            get;
            set;
        }

        // Telephone mobile
        // Privat: Kontaktdaten, Mobiltelefon
        // Einzelunternehmen: Kontaktdaten, Mobile
        // Unternehmen: Kontaktdaten, Mobile 

        [System.Runtime.Serialization.DataMember]
        public string HANDY
        {
            get;
            set;
        }

        // Fax
        // Privat: Kontaktdaten, Fax
        // Einzelunternehmen: Kontaktdaten, Fax
        // Unternehmen: Kontaktdaten, Fax 

        [System.Runtime.Serialization.DataMember]
        public string FAX
        {
            get;
            set;
        }

        // E-mail
        // Privat: Kontaktdaten, E-mail
        // Einzelunternehmen: Kontaktdaten, E-mail
        // Unternehmen: Kontaktdaten, E-mail 

        [System.Runtime.Serialization.DataMember]
        public string EMAIL
        {
            get;
            set;
        }

        // The best reachable at 
        // Privat: Kontaktdaten, bevorzugter Kommunikationskanal
        // Einzelunternehmen: Kontaktdaten,bevorzugter Kommunikationskanal
        // Unternehmen: Kontaktdaten, bevorzugter Kommunikationskanal

        [System.Runtime.Serialization.DataMember]
        public int? ERREICHBTREL
        {
            get;
            set;
        }

        // Account holder
        // Privat: Bankdaten, Kontoinhaber
        // Einzelunternehmen: Bankdaten, Kontoinhaber
        // Unternehmen: Bankdaten, Kontoinhaber

        [System.Runtime.Serialization.DataMember]
        public string KONTOINHABER
        {
            get;
            set;
        }

        // BC
        // Privat: Bankdaten, BLZ
        // Einzelunternehmen: Bankdaten, BLZ
        // Unternehmen: Bankdaten, BLZ

        [System.Runtime.Serialization.DataMember]
        public string BLZ
        {
            get;
            set;
        }

        // Account number 
        // Privat: Bankdaten, Kontonummer
        // Einzelunternehmen: Bankdaten, Kontonummer
        // Unternehmen: Bankdaten, Kontonummer

        [System.Runtime.Serialization.DataMember]
        public string KONTONR
        {
            get;
            set;
        }

        // Bank name
        // Privat: Bankdaten, Bankname
        // Einzelunternehmen: Bankdaten, Bankname
        // Unternehmen: Bankdaten, Bankname

        [System.Runtime.Serialization.DataMember]
        public string BANKNAME
        {
            get;
            set;
        }

        // IBAN
        // Privat: Bankdaten, IBAN
        // Einzelunternehmen: Bankdaten, IBAN
        // Unternehmen: Bankdaten, IBAN

        [System.Runtime.Serialization.DataMember]
        public string IBAN
        {
            get;
            set;
        }

        // BIC (Swift)
        // Privat: Bankdaten, BIC
        // Einzelunternehmen: Bankdaten, BIC
        // Unternehmen: Bankdaten, BIC

        [System.Runtime.Serialization.DataMember]
        public string BIC
        {
            get;
            set;
        }

        // Contact person, Salutation
        // Privat: -
        // Einzelunternehmen: -
        // Unternehmen: Anprechpartner, Anrede

        [System.Runtime.Serialization.DataMember]
        public string ANREDEKONT
        {
            get;
            set;
        }

        // Contact person, Title
        // Privat: -
        // Einzelunternehmen: -
        // Unternehmen: Anprechpartner, Titel

        [System.Runtime.Serialization.DataMember]
        public string TITELKONT
        {
            get;
            set;
        }

        // Contact person, First name
        // Privat: -
        // Einzelunternehmen: -
        // Unternehmen: Anprechpartner, Vorname

        [System.Runtime.Serialization.DataMember]
        public string VORNAMEKONT
        {
            get;
            set;
        }

        // Contact person, Last Name
        // Privat: -
        // Einzelunternehmen: -
        // Unternehmen: Anprechpartner, Nachname

        [System.Runtime.Serialization.DataMember]
        public string NAMEKONT
        {
            get;
            set;
        }

        // Contact person, Telephone business
        // Privat: -
        // Einzelunternehmen: -
        // Unternehmen: Anprechpartner, Telefonnummer

        [System.Runtime.Serialization.DataMember]
        public string TELEFONKONT
        {
            get;
            set;
        }

        // Contact person, E-Mail
        // Privat: -
        // Einzelunternehmen: -
        // Unternehmen: Anprechpartner, Mail

        [System.Runtime.Serialization.DataMember]
        public string EMAILKONT
        {
            get;
            set;
        }

        // Marital status
        // Selbstauskunft, Haushalt, Familienstand

        [System.Runtime.Serialization.DataMember]
        public int? FAMILIENSTAND
        {
            get;
            set;
        }

        // Number of children
        // Selbstauskunft, Haushalt, unterhaltsberechtigte Personen

        [System.Runtime.Serialization.DataMember]
        public int? KINDERIMHAUS
        {
            get;
            set;
        }

        // Type of house
        // Selbstauskunft, Haushalt, Art der Wohnung

        [System.Runtime.Serialization.DataMember]
        public string WOHNUNGART
        {
            get;
            set;
        }

        // Military Service
        // Selbstauskunft, Haushalt, Wehrdienst

        [System.Runtime.Serialization.DataMember]
        public string WEHRDIENST
        {
            get;
            set;
        }

        // Main income netton
        // Selbstauskunft, Einkommen, monatliches Nettoeinkommen

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? EINKNETTO
        {
            get;
            set;
        }

        // Additional income netton
        // Selbstauskunft, Einkommen, sonstige nachweisbare Einkünfte

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? NEBENEINKNETTO
        {
            get;
            set;
        }

        // Extra income netton
        // Selbstauskunft, Einkommen, Zulagen/Diäten

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? ZEINKNETTO
        {
            get;
            set;
        }

        // Additional acquirement
        // Selbstauskunft, Einkommen, Sonst. Vermögen

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? SONSTVERM
        {
            get;
            set;
        }

        // Type of additional acquirement
        // Selbstauskunft, Einkommen, Art sonst. Vermögen

        [System.Runtime.Serialization.DataMember]
        public string ARTSONSTVERM
        {
            get;
            set;
        }

        // Rent
        // Selbstauskunft, Ausgaben, Miete

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? MIETE
        {
            get;
            set;
        }

        // Additional monthly costs
        // Selbstauskunft, Ausgaben, Betriebskosten

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? AUSLAGEN
        {
            get;
            set;
        }

        // Credit rate 1
        // Selbstauskunft, Ausgaben, Kreditraten Wohnraumschaffung

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? KREDRATE1
        {
            get;
            set;
        }

        // Alimony
        // Selbstauskunft, Ausgaben, Alimente

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? UNTERHALT
        {
            get;
            set;
        }

        // Attendant costs
        // Selbstauskunft, Ausgaben, sonstige Zahlungsverpflichtungen

        [Cic.OpenLease.ServiceAccess.CurrencyFormat]
        [System.Runtime.Serialization.DataMember]
        public decimal? MIETNEBEN
        {
            get;
            set;
        }

        // Occupation
        // Selbstauskunft, Berufliche Daten, Beruf

        [System.Runtime.Serialization.DataMember]
        public string BERUF
        {
            get;
            set;
        }

        // Employed since
        // Selbstauskunft, Berufliche Daten, beschäftigt seit

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BESCHSEITAG
        {
            get;
            set;
        }

        // Employed until
        // Selbstauskunft, Berufliche Daten, beschäftigt bis

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BESCHBISAG
        {
            get;
            set;
        }

        // Employer, Name
        // Selbstauskunft, Berufliche Daten, Name

        [System.Runtime.Serialization.DataMember]
        public string NAMEAG
        {
            get;
            set;
        }

        // Employer, Street
        // Selbstauskunft, Arbeitgeber, Straße/Hausnummer

        [System.Runtime.Serialization.DataMember]
        public string STRASSEAG
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string HSNRAG
        {
            get;
            set;
        }

        // Employer, ZIP code
        // Selbstauskunft, Arbeitgeber, PLZ

        [System.Runtime.Serialization.DataMember]
        public string PLZAG
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int? WOHNVERH
        {
            get;
            set;
        }

        // Employer, Location
        // Selbstauskunft, Arbeitgeber, Ort

        [System.Runtime.Serialization.DataMember]
        public string ORTAG
        {
            get;
            set;
        }

        // Employer 1, Name
        // Selbstauskunft, Vorheriger Arbeitgeber, Name

        [System.Runtime.Serialization.DataMember]
        public string NAMEAG1
        {
            get;
            set;
        }

        // Employer 1, Employed since
        // Selbstauskunft, Vorheriger Arbeitgeber, beschäftigt von

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BESCHSEITAG1
        {
            get;
            set;
        }

        // Employer 1, Employed until
        // Selbstauskunft, Vorheriger Arbeitgeber, beschäftigt bis

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BESCHBISAG1
        {
            get;
            set;
        }

        // Employer 2, Name
        // Selbstauskunft, 2.Vorheriger Arbeitgeber, Name

        [System.Runtime.Serialization.DataMember]
        public string NAMEAG2
        {
            get;
            set;
        }

        // Employer 2, Employed since
        // Selbstauskunft, 2.Vorheriger Arbeitgeber, beschäftigt von

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BESCHSEITAG2
        {
            get;
            set;
        }

        // Employer 2, Employed until
        // Selbstauskunft, 2.Vorheriger Arbeitgeber, beschäftigt bis

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BESCHBISAG2
        {
            get;
            set;
        }

        // Employer 3, Name
        // Selbstauskunft, 3.Vorheriger Arbeitgeber, Name

        [System.Runtime.Serialization.DataMember]
        public string NAMEAG3
        {
            get;
            set;
        }

        // Employer 3, Employed since
        // Selbstauskunft, 3.Vorheriger Arbeitgeber, beschäftigt von

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BESCHSEITAG3
        {
            get;
            set;
        }

        // Employer 3, Employed until
        // Selbstauskunft, 3.Vorheriger Arbeitgeber, beschäftigt bis

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BESCHBISAG3
        {
            get;
            set;
        }

        // Zusatzinformationen für ausländische Staatsbürger (bei Privatperson und Einzelunternehmer)

        //IT:meldedatum	gemeldet seit

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? MELDEDATUM
        {
            get;
            set;
        }

        //IT:ahbewilligung	Aufenthaltsbewilligung seit

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? AHBEWILLIGUNG
        {
            get;
            set;
        }


        //IT:ahbewilligungbis	Aufenthaltsbewilligung bis

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? AHBEWILLIGUNGBIS
        {
            get;
            set;
        }

        //IT:ahgueltig	Aufenthaltsbewilligung unbefristet

        [System.Runtime.Serialization.DataMember]
        public int? AHGUELTIG
        {
            get;
            set;
        }

        //IT:ahbewilldurch	Aufenthaltsbewilligung ausgestellt von

        [System.Runtime.Serialization.DataMember]
        public string AHBEWILLDURCH
        {
            get;
            set;
        }

        //IT:abbewilligung	Arbeitsbewilligung seit

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ABBEWILLIGUNG
        {
            get;
            set;
        }

        //IT:abbewilligungbis	Arbeitsbewilligung bis

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ABBEWILLIGUNGBIS
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public System.DateTime? WOHNSEIT
        {
            get;
            set;
        }

        //IT:abgueltig	Arbeitsbewilligung unbefristet

        [System.Runtime.Serialization.DataMember]
        public int? ABGUELTIG
        {
            get;
            set;
        }

        //IT:abbewilldurch	Arbeitsbewilligung ausgestellt von

        [System.Runtime.Serialization.DataMember]
        public string ABBEWILLDURCH
        {
            get;
            set;
        }

        //IT:url	Url Website

        [System.Runtime.Serialization.DataMember]
        public string URL
        {
            get;
            set;
        }

        //IT:WBEGUENST	wirtschaftl. Begünstigte

        [System.Runtime.Serialization.DataMember]
        public string WBEGUENST
        {
            get;
            set;
        }

        //IT:SUFFIXKONT  Suffix	

        [System.Runtime.Serialization.DataMember]
        public string SUFFIXKONT
        {
            get;
            set;
        }

        //IT:USTABZUG  Umsatzsteuerpflicht	

        [System.Runtime.Serialization.DataMember]
        public int? USTABZUG
        {
            get;
            set;
        }

        //IT:KUNDENGRUPPE  Kundengruppe	

        [System.Runtime.Serialization.DataMember]
        public string KUNDENGRUPPE
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public ITTypeConstants ITCONFIGSOURCE
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ITCONFIGID
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public int? AUSWEISGUELTIG
        {
            get;
            set;
        }

        //Vertretungsberechtigung (aus DDLKPPOS Code=VERTRETUNGSBERECHTIG)
        [System.Runtime.Serialization.DataMember]
        public string VERTRETUNGSBERECHTIGUNG
        {
            get;
            set;
        }
        #endregion        

        [System.Runtime.Serialization.DataMember]
        public string WERBECODE
        {
            get;
            set;
        }
        public int? SCHUFAFLAG
        {
            get;
            set;
        }
        #region Extended properties

        [System.Runtime.Serialization.DataMember]
        public string ExtTitle
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ExtCompleteName
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ExtZipCodeCity
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ExtBankAccountCompleteName
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ExtBankCompleteName
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public string ExtContactCompleteName
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int PAYART
        {
            get;
            set;
        }

        /// <summary>
        /// AIDA2 CRM Fields
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int CAMPCount { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int EOTCount { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int VTCount { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int ANGEBOTCount { get; set; }
        /// <summary>
        /// Is PERSON FLAG (Person for IT available)
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool ISPERSON { get { if (SYSPERSON.HasValue && SYSPERSON.Value > 0) return true; return false; } set { } }

        #endregion


        #region lookupcodefields

        [System.Runtime.Serialization.DataMember]
        public string RECHTSFORMCODE { get; set; }
        [System.Runtime.Serialization.DataMember]
        public string ANREDECODE { get; set; }
        [System.Runtime.Serialization.DataMember]
        public string TITELCODE { get; set; }
        #endregion

        [System.Runtime.Serialization.DataMember]
        public string PREVNAME { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string IDENTEG { get; set; }

        #region compliance data
        ///Bezeichnung aus COMPLIANCE - Bezeichnung des Amtes
        [System.Runtime.Serialization.DataMember]
        public string AMTBEZ { get; set; }

        ///BEGINN aus COMPLIANCE - Amt seit
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? AMTSEIT { get; set; }

        ///ENDE aus COMPLIANCE - Amt bis
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? AMTBIS { get; set; }

        ///SYSLAND aus COMPLIANCE - Land
        [System.Runtime.Serialization.DataMember]
        public long? AMTLAND { get; set; }

        ///FLAGAKTIV aus COMPLIANCE - exponierte Person
        [System.Runtime.Serialization.DataMember]
        public bool FLAGAKTIV { get; set; }
        #endregion

        /// <summary>
        /// Beziehungen, wobei SYSIT=SYSUNTER
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public List<ITKNEDto> KNELIST { get; set; }

        /// <summary>
        /// Aktuelle Beziehung für die dieser Satz verwendet wird, wird in ITKNE gespeichert
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public ITKNEDto KNE {get;set;}

        /*[System.Runtime.Serialization.DataMember]
        public string LEI { get; set; }*/

        [System.Runtime.Serialization.DataMember]
        public string HREGISTERPLZ { get; set; }

        [System.Runtime.Serialization.DataMember]
        public int? HREGISTERART { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string IDENTUST { get; set; }

        [System.Runtime.Serialization.DataMember]
        public decimal? WINTUMFANG { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string WINTART { get; set; }
    }
}
 