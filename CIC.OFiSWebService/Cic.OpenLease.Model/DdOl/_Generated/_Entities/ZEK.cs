// AUTOGENERATED, 18.11.2010 15:27:39
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Cic.OpenLease.Model.DdOl
{
    [System.CodeDom.Compiler.GeneratedCode("Cic.OpenLease.CodeGenerator", "1.0.0.0")]
    [System.CLSCompliant(true)]
    public partial class ZEK
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(ZEK));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSZEK
            {
                get { return "SYSZEK"; }
            }

            public static string TYP
            {
                get { return "TYP"; }
            }

            public static string RANG
            {
                get { return "RANG"; }
            }

            public static string ANFRAGEDATUM
            {
                get { return "ANFRAGEDATUM"; }
            }

            public static string ANFRAGEUSER
            {
                get { return "ANFRAGEUSER"; }
            }

            public static string ANTWORTDATUM
            {
                get { return "ANTWORTDATUM"; }
            }

            public static string BEZEICHNUNG
            {
                get { return "BEZEICHNUNG"; }
            }

            public static string ZEKSERVICEID
            {
                get { return "ZEKSERVICEID"; }
            }

            public static string ZEKKUNDENID
            {
                get { return "ZEKKUNDENID"; }
            }

            public static string ZEKGESUCHSID
            {
                get { return "ZEKGESUCHSID"; }
            }

            public static string ZEKVERTRAGSID
            {
                get { return "ZEKVERTRAGSID"; }
            }

            public static string VTBEGINN
            {
                get { return "VTBEGINN"; }
            }

            public static string VTENDE
            {
                get { return "VTENDE"; }
            }

            public static string KREDITBETRAG
            {
                get { return "KREDITBETRAG"; }
            }

            public static string RATE
            {
                get { return "RATE"; }
            }

            public static string LZ
            {
                get { return "LZ"; }
            }

            public static string SZ
            {
                get { return "SZ"; }
            }

            public static string SZDATUM
            {
                get { return "SZDATUM"; }
            }

            public static string RW
            {
                get { return "RW"; }
            }

            public static string RWDATUM
            {
                get { return "RWDATUM"; }
            }

            public static string KREDITARTCODE
            {
                get { return "KREDITARTCODE"; }
            }

            public static string BONITAETSCODE
            {
                get { return "BONITAETSCODE"; }
            }

            public static string ABLEHNUNGSCODE
            {
                get { return "ABLEHNUNGSCODE"; }
            }

            public static string RUECKMELDUNG
            {
                get { return "RUECKMELDUNG"; }
            }

            public static string SICHERSTELLUNGSCODE
            {
                get { return "SICHERSTELLUNGSCODE"; }
            }

            public static string KREDITBETRAGPROZ
            {
                get { return "KREDITBETRAGPROZ"; }
            }

            public static string ABLEHNUNG
            {
                get { return "ABLEHNUNG"; }
            }

            public static string AUFHEBUNG
            {
                get { return "AUFHEBUNG"; }
            }

            public static string GUELTIGBIS
            {
                get { return "GUELTIGBIS"; }
            }

            public static string KARTENMELDUNG
            {
                get { return "KARTENMELDUNG"; }
            }

            public static string KREDITGESUCH
            {
                get { return "KREDITGESUCH"; }
            }

            public static string NEGATIVEREIGNIS
            {
                get { return "NEGATIVEREIGNIS"; }
            }

            public static string REFERENZ
            {
                get { return "REFERENZ"; }
            }

            public static string SALDOSTICHTAG
            {
                get { return "SALDOSTICHTAG"; }
            }

            public static string BONITAET
            {
                get { return "BONITAET"; }
            }

            public static string EINGABECODE
            {
                get { return "EINGABECODE"; }
            }

            public static string ANFRAGEGRUND
            {
                get { return "ANFRAGEGRUND"; }
            }

            public static string STATUSCODE
            {
                get { return "STATUSCODE"; }
            }

            public static string EREIGNISCODE
            {
                get { return "EREIGNISCODE"; }
            }

            public static string HERKUNFT
            {
                get { return "HERKUNFT"; }
            }

            public static string IKOPFLICHTIG
            {
                get { return "IKOPFLICHTIG"; }
            }

            public static string ZIVILSTANDCODE
            {
                get { return "ZIVILSTANDCODE"; }
            }

            public static string AMTSINFOCODE
            {
                get { return "AMTSINFOCODE"; }
            }

            public static string RUECKMELDECODE
            {
                get { return "RUECKMELDECODE"; }
            }

            public static string KARTENTYPCODE
            {
                get { return "KARTENTYPCODE"; }
            }

            public static string KREDITLIMITE
            {
                get { return "KREDITLIMITE"; }
            }

            public static string LOESCHCODE
            {
                get { return "LOESCHCODE"; }
            }

            public static string SALDOABRECHNUNG
            {
                get { return "SALDOABRECHNUNG"; }
            }

            public static string SALDOKONTOAUSZUG
            {
                get { return "SALDOKONTOAUSZUG"; }
            }

            public static string SCHULDNERROLLE
            {
                get { return "SCHULDNERROLLE"; }
            }

            public static string HAUSVT
            {
                get { return "HAUSVT"; }
            }

            public static string BANKNR
            {
                get { return "BANKNR"; }
            }

            public static string NOTIZ
            {
                get { return "NOTIZ"; }
            }

            public static string POSEREIGNIS
            {
                get { return "POSEREIGNIS"; }
            }

            public static string QUELDAT
            {
                get { return "QUELDAT"; }
            }

            public static string EINFRISTDAT
            {
                get { return "EINFRISTDAT"; }
            }

            public static string AMTSKANTON
            {
                get { return "AMTSKANTON"; }
            }

            public static string AMTSPLZ
            {
                get { return "AMTSPLZ"; }
            }

            public static string KDNAME
            {
                get { return "KDNAME"; }
            }

            public static string KDVORNAME
            {
                get { return "KDVORNAME"; }
            }

            public static string KDGEBURTSDATUM
            {
                get { return "KDGEBURTSDATUM"; }
            }

            public static string KDPLZ
            {
                get { return "KDPLZ"; }
            }

            public static string KDORT
            {
                get { return "KDORT"; }
            }

            public static string KDSTRASSE
            {
                get { return "KDSTRASSE"; }
            }

            public static string KDHSNR
            {
                get { return "KDHSNR"; }
            }

            public static string KDLANDCODE
            {
                get { return "KDLANDCODE"; }
            }

            public static string KDWOHNTSEIT
            {
                get { return "KDWOHNTSEIT"; }
            }

            public static string KDNATIONALITAETCODE
            {
                get { return "KDNATIONALITAETCODE"; }
            }

            public static string KDRECHTSFORMCODE
            {
                get { return "KDRECHTSFORMCODE"; }
            }

            public static string KDBRANCHE
            {
                get { return "KDBRANCHE"; }
            }

            public static string MHNAME
            {
                get { return "MHNAME"; }
            }

            public static string MHVORNAME
            {
                get { return "MHVORNAME"; }
            }

            public static string MHGEBURTSDATUM
            {
                get { return "MHGEBURTSDATUM"; }
            }

            public static string MHPLZ
            {
                get { return "MHPLZ"; }
            }

            public static string MHORT
            {
                get { return "MHORT"; }
            }

            public static string MHSTRASSE
            {
                get { return "MHSTRASSE"; }
            }

            public static string MHHSNR
            {
                get { return "MHHSNR"; }
            }

            public static string MHLANDCODE
            {
                get { return "MHLANDCODE"; }
            }

            public static string MHWOHNTSEIT
            {
                get { return "MHWOHNTSEIT"; }
            }

            public static string MHNATIONALITAETCODE
            {
                get { return "MHNATIONALITAETCODE"; }
            }

            public static string MHRECHTSFORMCODE
            {
                get { return "MHRECHTSFORMCODE"; }
            }

            public static string MHBRANCHE
            {
                get { return "MHBRANCHE"; }
            }

            public static string GATEWAYFUNCTION
            {
                get { return "GATEWAYFUNCTION"; }
            }

            public static string GATEWAYPRODUCT
            {
                get { return "GATEWAYPRODUCT"; }
            }

            public static string ZEKMETHOD
            {
                get { return "ZEKMETHOD"; }
            }

            public static string CURRENCYISO4217
            {
                get { return "CURRENCYISO4217"; }
            }

            public static string PRIVATFLAG
            {
                get { return "PRIVATFLAG"; }
            }

            public static string ACTIONFLAG
            {
                get { return "ACTIONFLAG"; }
            }

            public static string ACTIONERROR
            {
                get { return "ACTIONERROR"; }
            }

            public static string WAITUNTIL
            {
                get { return "WAITUNTIL"; }
            }

            public static string KDGEBORT
            {
                get { return "KDGEBORT"; }
            }

            public static string MHGEBORT
            {
                get { return "MHGEBORT"; }
            }

            public static string KDSEX
            {
                get { return "KDSEX"; }
            }

            public static string MHSEX
            {
                get { return "MHSEX"; }
            }

            public static string KDADRTYPE
            {
                get { return "KDADRTYPE"; }
            }

            public static string MHADRTYPE
            {
                get { return "MHADRTYPE"; }
            }

            public static string KDCREDITNO
            {
                get { return "KDCREDITNO"; }
            }

            public static string MHCREDITNO
            {
                get { return "MHCREDITNO"; }
            }

            public static string SYSWFUSER
            {
                get { return "SYSWFUSER"; }
            }

            public static string SYSWFEXEC
            {
                get { return "SYSWFEXEC"; }
            }

            public static string ANFRAGEZEIT
            {
                get { return "ANFRAGEZEIT"; }
            }

            public static string SCHLUESSELWERT
            {
                get { return "SCHLUESSELWERT"; }
            }

            public static string PARANAME
            {
                get { return "PARANAME"; }
            }

            public static string DIENSTANBIETER
            {
                get { return "DIENSTANBIETER"; }
            }

            public static string DIENST
            {
                get { return "DIENST"; }
            }

            public static string MASSENMELDUNG
            {
                get { return "MASSENMELDUNG"; }
            }

            public static string IMMERSENDEN
            {
                get { return "IMMERSENDEN"; }
            }

            public static string WAITUNTILTIME
            {
                get { return "WAITUNTILTIME"; }
            }

            public static string ERSTEANFRAGEDATUM
            {
                get { return "ERSTEANFRAGEDATUM"; }
            }

            public static string ERSTEANFRAGEZEIT
            {
                get { return "ERSTEANFRAGEZEIT"; }
            }

            public static string LETZTEANFRAGEDATUM
            {
                get { return "LETZTEANFRAGEDATUM"; }
            }

            public static string LETZTEANFRAGEZEIT
            {
                get { return "LETZTEANFRAGEZEIT"; }
            }

            public static string ANTWORTZEIT
            {
                get { return "ANTWORTZEIT"; }
            }

            public static string MITGLIEDSCHAFT
            {
                get { return "MITGLIEDSCHAFT"; }
            }


			#endregion
        }

    }
}