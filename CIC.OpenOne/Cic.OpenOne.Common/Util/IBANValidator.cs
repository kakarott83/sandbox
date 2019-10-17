using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Cic.OpenOne.Common.Util
{
    

    

    /// <summary>
    /// Internally used BLZInformation for BIC-Validation
    /// </summary>
    public class BLZInfo
    {
        public String NAME { get; set; }
        public String BLZ { get; set; }
        public String BIC { get; set; }
        public String IBAN { get; set; }
        public String KONTONR { get; set; }
        public String SIGNCITY { get; set; }
        public long SYSMANDAT { get; set; }
        public long SYSKONTO { get; set; }
        public long SYSKI { get; set; }
        public long SYSBLZ { get; set; }
        public int? EINZUG { get; set; }
    }

    /// <summary>
    /// IBANValidator Prüfmethode
    ///-------------------------
    ///public IBANValidationError checkIBANandBIC(String iban, String bic) ...
    /// a) IBAN validieren, die Methode bricht bei der ersten fehlgeschlagenen Prüfung ab
    ///    Prüfung gemäß UN CEFACT TBG5 (United Nations Centre for Trade Facilitation and Electronic Business, http://www.tbg5-finance.org/?ibandocs.shtml)
    ///    1) Prüfung auf ungültige Zeichen (nur A-Z, a-z und 0-9 sind erlaubt)
    ///       wenn Prüfung fehlschlägt wird error=IBANValidationErrorType.InvalidCharacter gesetzt, errorFields enthält alle 4er Blöcke welche ungültige Zeichen enthalten
    ///    2) Prüfung der ersten 4 Zeichen, erforderlich ist BuchstabeBuchstabeZifferZiffer
    ///       wenn Prüfung fehlschlägt wird error=IBANValidationErrorType.InvalidStart gesetzt, errorFields enthält 0
    ///    3) Validierung der Prüfsumme #1, Ausschluß der veralteten Werte 00, 01 oder 99
    ///       wenn Prüfung fehlschlägt wird error=IBANValidationErrorType.InvalidChecksum gesetzt, errorFields enthält 0
    ///    4) Validierung des Landescodes (erste zwei Zeichen)
    ///       wenn Prüfung fehlschlägt wird error=IBANValidationErrorType.InvalidCountry gesetzt, errorFields enthält 0
    ///    5) Validierung der Länge (Abhängig vom Landescode)
    ///       wenn Prüfung fehlschlägt wird error=IBANValidationErrorType.InvalidLength gesetzt, errorFields enthält 0-basierten Index des letzten Feldes
    ///    6) Validierung der Landesstruktur (landesabhängige Abfolge von Zeichen inkl. Groß/Kleinschreibung und Ziffern)
    ///       wenn Prüfung fehlschlägt wird error=IBANValidationErrorType.InvalidCountryFormat gesetzt, errorFields enthält 0-basierten Index des fehlerhaften Feldes
    ///    7) Validierung der Prüfsumme #2, validierung gemäß  ISO 7064 mod 97-10
    ///       wenn Prüfung fehlschlägt wird error=IBANValidationErrorType.InvalidChecksum gesetzt, errorFields enthält 0
    ///    8) IBAN Validierung erfolgreich   
    ///       error=IBANValidationErrorType.NoError, errorFields ist leer
    /// b) BLZ aus IBAN extrahieren, damit in DB in BLZ-Tabelle suchen und falls Treffer die BIC aus BLZ.BIC mit übergebener vergleichen
    ///    wenn der Vergleich fehlschlägt ist bicwarning=true, ansonsten wird in bankname der Name der Filiale zurückgegeben
    /// </summary>
    public class IBANValidator
    {

        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool BICDBVALIDATION = true;//check bic by structure and not by db-matching if set to false
        private static bool loadConfig = true;
        private static bool tested = false;
        #region Variables

        //country codes, fixed length for those countries, inner structure, IBAN example, IBAN requirement, SEPA, appliance of EU REGULATION 924/2009;260/2012 and EUR,     
                                                        //BLZ-LENGHT (8), BLZ-POS (9), BRANCH-LENGTH (10), BRANCH-CHECKSUM (11), KTO-IDX ohne Prüfziffer, KTO-LENGTH ohne Prüfziffer
        //the first three items of the array are mandatory!

        //Example for inner structure, country and checksum (first four chars) are always validated and not defined in the structure:
        //Bulgarien 	22 	BGpp bbbb ssss ddkk kkkk kk
        /*          case "A": IsMatchpattern += "0-9A-Za-z"; break;
                    case "B": IsMatchpattern += "0-9A-Z"; break;
                    case "C": IsMatchpattern += "A-Za-z"; break;
                    case "F": IsMatchpattern += "0-9"; break;
                    case "L": IsMatchpattern += "a-z"; break;
                    case "U": IsMatchpattern += "A-Z"; break;
                    case "W": IsMatchpattern += "0-9a-z"; break;
         * */
        //INDEX 10: branch length
        //INDEX 11: branch check digit length
        //---->U04F04F02A08
        String[][] ilbced = new String[][]
        {

            new String[]{"AD", "24", "F04F04A12",		"AD1200012030200359100100",		"n", "n", "n", "n","4","4","4","0"   ,"12","12"},
           new String[]{"AE", "23", "F03F16",		"AE070331234567890123456",		"n", "n", "n", "n","3","4","0","0","7","16"},
           new String[]{"AL", "28", "F08A16",		"AL47212110090000000235698741",		"n", "n", "n", "n","3","4","4","1","12","16"},
           new String[]{"AT", "20", "F05F11",		"AT611904300234573201",       "n", "y", "y", "y","5","4","0","0","9","11"},
           new String[]{"AZ", "28", "U04A20",		"AZ21NABZ00000000137010001944",		"n", "n", "n", "n","4","4","0","0","8","20"},
           new String[]{"BA", "20", "F03F03F08F02",	"BA391290079401028494",      "n", "n", "n", "n","3","4","3","0","10","8"},
           new String[]{"BE", "16", "F03F07F02",		"BE68539007547034",      "n", "y", "y", "y","3","4","0","0","7","7"},
           new String[]{"BG", "22", "U04F04F02A08",	"BG80BNBG96611020345678",		"n", "y", "y", "n","4","4","4","0","14","8"},
           new String[]{"BH", "22", "U04A14",		"BH67BMAG00001299123456",		"y", "n", "n", "n","4","4","0","0","8","14"},
           new String[]{"BR", "29", "F08F05F10U01A01",	"BR9700360305000010009795493P1",	"n", "n", "n", "n","8","4","5","0","17","12"},
           new String[]{"CH", "21", "F05A12",		"CH9300762011623852957",		"n", "y", "n", "n","5","4","0","0","9","12"},
           new String[]{"CR", "21", "F03F14",		"CR0515202001026284066",		"n", "n", "n", "n","3","4","0","0","7","14"},
           new String[]{"CY", "28", "F03F05A16",		"CY17002001280000001200527600",		"n", "y", "y", "y","3","4","5","0","12","16"},
           new String[]{"CZ", "24", "F04F06F10",		"CZ6508000000192000145399",		"n", "y", "y", "n","4","4","0","0","8","16"},
           new String[]{"DE", "22", "F08F10",		"DE89370400440532013000",		"n", "y", "y", "y","8","4","0","0","12","10"},
           new String[]{"DK", "18", "F04F09F01",		"DK5000400440116243",       "n", "y", "y", "n","4","4","0","0","8","9"},
           new String[]{"DO", "28", "U04F20",		"DO28BAGR00000001212453611324",		"n", "n", "n", "n","4","4","0","0","8","20"},
           new String[]{"EE", "20", "F02F02F11F01",	"EE382200221020145685",      "n", "y", "y", "y","2","4","0","0","6","13"},
           new String[]{"ES", "24", "F04F04F01F01F10",	"ES9121000418450200051332",		"n", "y", "y", "y","4","4","4","1","14","10"},
            new String[]{"FI", "18", "F06F07F01",		"FI2112345600000785",     "n", "y", "y", "y","6","4","0","0","10","7"},
           new String[]{"FO", "18", "F04F09F01",		"FO6264600001631634",      "n", "y", "n", "n","4","4","0","0","8","9"},
           new String[]{"FR", "27", "F05F05A11F02",	"FR1420041010050500013M02606",		"n", "y", "y", "y","5","4","5","0","14","11"},
           new String[]{"GF", "27", "F05F05A11F02",	"FR1420041010050500013M02606",		"n", "y", "y", "y","5","4","5","0","14","11"},
           new String[]{"PF", "27", "F05F05A11F02",	"FR1420041010050500013M02606",		"n", "y", "y", "y","5","4","5","0","14","11"},
           new String[]{"TF", "27", "F05F05A11F02",	"FR1420041010050500013M02606",		"n", "y", "y", "y","5","4","5","0","14","11"},
           new String[]{"GB", "22", "U04F06F08",		"GB29NWBK60161331926819",		"n", "y", "y", "n","4","4","6","0","14","8"},
           new String[]{"GE", "22", "U02F16",		"GE29NB0000000101904917",		"n", "n", "n", "n","2","4","0","0","6","16"},
           new String[]{"GI", "23", "U04A15",		"GI75NWBK000000007099453",		"n", "y", "y", "n","4","4","0","0","8","15"},
           new String[]{"GL", "18", "F04F09F01",		"GL8964710001000206",     "n", "y", "n", "n","4","4","0","0","8","9"},
           new String[]{"GR", "27", "F03F04A16",		"GR1601101250000000012300695",		"n", "y", "y", "y","3","4","4","0","11","17"},
           new String[]{"GT", "28", "A04A20",		"GT82TRAJ01020000001210029690",		"n", "n", "n", "n","4","4","0","0","8","20"},
           new String[]{"HR", "21", "F07F10",		"HR1210010051863000160",		"n", "n", "n", "n","7","4","0","0","11","10"},
           new String[]{"HU", "28", "F03F04F01F15F01",	"HU42117730161111101800000000",		"n", "y", "y", "n","3","4","4","1","12","15"},
           new String[]{"IE", "22", "U04F06F08",		"IE29AIBK93115212345678",		"n", "y", "y", "y","4","4","6","0","14","8"},
           new String[]{"IL", "23", "F03F03F13",		"IL620108000000099999999",		"n", "n", "n", "n","3","4","3","0","11","13"},
           new String[]{"IS", "26", "F04F02F06F10",	"IS140159260076545510730339",		"n", "y", "y", "n","4","4","2","0","10","6"},
           new String[]{"IT", "27", "U01F05F05A12",	"IT60X0542811101000000123456",		"n", "y", "y", "y","5","5","5","0","15","12"},
           new String[]{"KW", "30", "U04A22",		"KW81CBKU0000000000001234560101",	"y", "n", "n", "n","4","4","0","0","8","22"},
           new String[]{"KZ", "20", "F03A13",		"KZ86125KZT5004100100",      "n", "n", "n", "n","3","4","0","0","7","13"},
           new String[]{"LB", "28", "F04A20",		"LB62099900000001001901229114",		"n", "n", "n", "n","4","4","0","0","8","20"},
           new String[]{"LI", "21", "F05A12",		"LI21088100002324013AA",		"n", "y", "y", "n","5","4","0","0","9","12"},
           new String[]{"LT", "20", "F05F11",		"LT121000011101001000",      "n", "y", "y", "n","5","4","0","0","9","11"},
           new String[]{"LU", "20", "F03A13",		"LU280019400644750000",       "n", "y", "y", "y","3","4","0","0","7","13"},
           new String[]{"LV", "21", "U04A13",		"LV80BANK0000435195001",		"n", "y", "y", "n","4","4","0","0","8","13"},
           new String[]{"MC", "27", "F05F05A11F02",	"MC5811222000010123456789030",		"n", "y", "y", "n","5","4","5","0","14","11"},
           new String[]{"MD", "24", "U02F18",		"MD24AG000225100013104168",		"n", "n", "n", "n","2","4","0","0","6","18"},
           new String[]{"ME", "22", "F03F13F02",		"ME25505000012345678951",		"n", "n", "n", "n","3","4","0","0","7","13"},
           new String[]{"MK", "19", "F03A10F02",		"MK07250120000058984",      "n", "n", "n", "n","3","4","0","0","7","10"},
           new String[]{"MR", "27", "F05F05F11F02",	"MR1300020001010000123456753",		"n", "n", "n", "n","5","4","5","0","14","11"},
           new String[]{"MT", "31", "U04F05A18",		"MT84MALT011000012345MTLCAST001S",	"n", "y", "y", "y","4","4","5","0","13","18"},
           new String[]{"MU", "30", "U04F02F02F12F03U03",	"MU17BOMM0101101030300200000MUR",	"n", "n", "n", "n","6","4","2","0","12","15"},
           new String[]{"NL", "18", "U04F10",		"NL91ABNA0417164300",       "n", "y", "y", "y","4","4","0","0","8","10"},
           new String[]{"NO", "15", "F04F06F01",		"NO9386011117947",      "n", "y", "y", "n","4","4","0","0","8","6"},
           new String[]{"PK", "24", "U04A16",		"PK36SCBL0000001123456702",		"n", "n", "n", "n","4","4","0","0","10","14"},
           new String[]{"PL", "28", "F08F16",		"PL61109010140000071219812874",		"y", "y", "y", "n","3","4","0","0","12","16"},
           new String[]{"PS", "29", "U04A21",		"PS92PALS000000000400123456702",	"n", "n", "n", "n","4","4","0","0","17","12"},
           new String[]{"PT", "25", "F04F04F11F02",	"PT50000201231234567890154",		"n", "y", "y", "y","4","4","4","0","12","11"},
           new String[]{"RO", "24", "U04A16",		"RO49AAAA1B31007593840000",		"n", "y", "y", "n","4","4","0","0","8","16"},
           new String[]{"RS", "22", "F03F13F02",		"RS35260005601001611379",		"n", "n", "n", "n","3","4","0","0","7","13"},
           new String[]{"SA", "24", "F02A18",		"SA0380000000608010167519",		"y", "n", "n", "n","2","4","0","0","6","18"},
           new String[]{"SE", "24", "F03F16F01",		"SE4550000000058398257466",		"n", "y", "y", "n","3","4","0","0","7","16"},
           new String[]{"SI", "19", "F05F08F02",		"SI56263300012039086",      "n", "y", "y", "n","2","4","3","0","9","8"},
           new String[]{"SK", "24", "F04F06F10",		"SK3112000000198742637541",		"n", "y", "y", "y","4","4","6","0","14","10"},
           new String[]{"SM", "27", "U01F05F05A12",	"SM86U0322509800000000270100",		"n", "n", "n", "n","5","5","5","0","15","12"},
           new String[]{"TN", "24", "F02F03F13F02",	"TN5910006035183598478831",		"n", "n", "n", "n","2","4","3","0","9","13"},
           new String[]{"TR", "26", "F05A01A16",		"TR330006100519786457841326",		"y", "n", "n", "n","5","4","0","0","10","16"},
           new String[]{"VG", "24", "U04F16",		"VG96VPVG0000012345678901",		"n", "n", "n", "n","4","4","0","0","8","16"}

       };

        #endregion Variables

        /// <summary>
        /// Creates a new validator instance and performs a self-test
        /// </summary>
        public IBANValidator()
        {
            if (loadConfig)
            {
                String path = FileUtils.getCurrentPath() + "\\..\\ibanconfig.xml";
                try
                {
                    byte[] data = FileUtils.loadData(path);
                    ilbced = XMLDeserializer.objectFromXml<String[][]>(data, "UTF-8");
                }
                catch (Exception)
                {//file not found
                    try
                    {
                        byte[] writeData = XMLSerializer.objectToXml(ilbced, "UTF-8");
                        FileUtils.saveFile(path, writeData);
                        byte[] data = FileUtils.loadData(path);
                        ilbced = XMLDeserializer.objectFromXml<String[][]>(data, "UTF-8");
                    }
                    catch (Exception e2)
                    {
                        _Log.Error("External IBAN-Config xml not used", e2);
                    }
                }
                loadConfig = false;
            }


            selfTest();
        }

        /// <summary>
        /// Returns a descriptor for the GUI to prevalidate/layout the iban depending on country-code
        /// </summary>
        /// <returns></returns>
        public List<IBANInfoDto> getIBANInformation()
        {
            List<IBANInfoDto> rval = new List<IBANInfoDto>();
            foreach (String[] supIban in ilbced)
            {
                IBANInfoDto info = new IBANInfoDto();
                info.countryCode = supIban[0];
                info.length = int.Parse(supIban[1]);
                info.inputFields = (int)Math.Ceiling((double)info.length / 4);
                rval.Add(info);
            }
            return rval;
        }



        /// <summary>
        /// Self-Test the ibanvalidator with valid and invalid iban codes
        /// </summary>
        private void selfTest()
        {
            if (tested) return;

            check(checkIBAN("AD120001A030200B59100103"), true);
            check(checkIBAN("AD120001203020035910010"), true);//The length of IBAN is wrong. The IBAN of Andorra needs to be 24 characters long.
            check(checkIBAN("AD1200012030200359100103"), true);//The IBAN is incorrect.
            check(checkIBAN("AD 1200012030200359100103"), true);//The IBAN contains illegal characters.
            check(checkIBAN("AA1200012030200359100103"), true);//Can not check correct length of IBAN because AA is currently not respected
            check(checkIBAN("BEA8539007547034"), true);//The structure of IBAN is wrong.
            check(checkIBAN("AT99993232 22 329999"), true);
            check(checkIBANandBIC("AT402050607701179967", "SPKUAT22XXX"), true);
            check(checkIBANandBIC("AT402050607701179967", "SPKUAT22XX"), true);
            check(checkIBANandBIC("AT402050607701179967", "SPKUAT22X"), true);
            check(checkIBANandBIC("AT402050607701179967", "SPKUAT22"), true);
            foreach (String[] test in ilbced)
            {
                check(checkIBAN(test[3]), false);
            }
            tested = true;
        }

        /// <summary>
        /// Logging for internal iban checking method
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="expectedError"></param>
        private void check(IBANValidationError msg, bool expectedError)
        {
            if (expectedError && msg.error == IBANValidationErrorType.NoError)
                _Log.Debug("IBAN-Validator failure " + msg.error + ": " + msg.detail);
            else if (!expectedError && msg.error != IBANValidationErrorType.NoError)
                _Log.Debug("IBAN-Validator failure " + msg.error + ": " + msg.detail);
        }

        /// <summary>
        /// Zur Validierung der Prüfsumme wird zunächst eine Zahl erstellt. Diese setzt sich aus BBAN (in Deutschland z.B. 18 Stellen) + Länderkürzel kodiert (2 Stellen, siehe Punkt 2.) + Prüfsumme zusammen.
        /// Die beiden Buchstaben des Länderkürzels sowie weitere etwa in der Kontonummer enthaltene Buchstaben werden durch ihre Position im lateinischen Alphabet + 9 ersetzt (A = 10, B = 11, …, Z = 35).
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String ISO13616Prepare(String str)
        {
            String isostr = str.ToUpper();
            isostr = isostr.Substring(4) + isostr.Substring(0, 4);
            for (int x = 65; x <= 90; x++)
            {
                int replacewith = x - 64 + 9;
                string replace = ((char)x).ToString();
                isostr = isostr.Replace(replace, replacewith.ToString());
            }
            return isostr;
        }

        /// <summary>
        /// Nun wird der Rest berechnet, der sich beim Teilen der Zahl durch 97 ergibt (Modulo 97).
        /// Das Ergebnis muss 1 sein, ansonsten ist die IBAN falsch.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String ISO7064Mod97_10(String str)
        {
            int parts = (int)Math.Ceiling((double)str.Length / 7);
            String remainer = "";
            for (int i = 1; i <= parts; i++)
            {
                if (i == parts)
                    remainer = (int.Parse(remainer + str.Substring((i - 1) * 7)) % 97).ToString();
                else
                    remainer = (int.Parse(remainer + str.Substring((i - 1) * 7, 7)) % 97).ToString();

            }
            return remainer;
        }
        /// <summary>
        /// returns the kontonummer of a valid iban or null
        /// </summary>
        /// <param name="iban"></param>
        /// <returns></returns>
        public String getKontonummer(String iban)
        {
            if (iban == null) return null;
            if (iban.Length < 4) return null;
            //a) validate iban
            IBANValidationError rval = checkIBAN(iban);
            if (rval.error != IBANValidationErrorType.NoError) return null;//invalid iban
            
            String[] ilbc = (from cset in ilbced
                                 where cset[0].Equals(iban.Substring(0, 2).ToUpper())
                                 select cset).FirstOrDefault();
                //c) find bic by blz, compare to bic


            if (iban.Length.ToString() == ilbc[1])
            {
                int ktoidx = int.Parse(ilbc[12]);
                int ktolength = int.Parse(ilbc[13]);
                return iban.Substring(ktoidx, ktolength);
                  
            }
            return null;
        }
        /// <summary>
        /// returns the blz of a valid iban or null
        /// </summary>
        /// <param name="iban"></param>
        /// <returns></returns>
        public String getBLZ(String iban)
        {
            if (iban == null) return null;
            if (iban.Length < 4) return null;
            //a) validate iban
            IBANValidationError rval = checkIBAN(iban);
            if (rval.error != IBANValidationErrorType.NoError) return null;//invalid iban

            String[] ilbc = (from cset in ilbced
                             where cset[0].Equals(iban.Substring(0, 2).ToUpper())
                             select cset).FirstOrDefault();
            //c) find bic by blz, compare to bic


            if (iban.Length.ToString() == ilbc[1])
            {
                return iban.Substring(int.Parse(ilbc[9]), int.Parse(ilbc[8]));

            }
            return null;
        }

        /// <summary>
        /// Validates IBAN
        /// Extracts BLZ from IBAN
        /// Searches Bankname from BLZ
        /// Validates bic to blz
        /// </summary>
        /// <param name="iban"></param>
        /// <param name="bic"></param>
        /// <returns></returns>
        public IBANValidationError checkIBANandBIC(String iban, String bic)
        {



            //a) validate iban
            IBANValidationError rval = checkIBAN(iban);
            rval.bicwarning = false;

            //b) extract blz
            if (iban != null && iban.Length > 2)
            {
                String[] ilbc = (from cset in ilbced
                                 where cset[0].Equals(iban.Substring(0, 2).ToUpper())
                                 select cset).FirstOrDefault();
                //c) find bic by blz, compare to bic


                if (!BICDBVALIDATION)//check bic by structure and not by db-matching
                {
                    Regex bicStructure = new Regex(@"([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)");
                    if (bicStructure.IsMatch(bic))
                        rval.bicwarning = false;
                }
                else if (ilbc == null)//wrong iban first digits!
                {
                    rval.bicwarning = true;
                }
                else if (iban.Length.ToString() == ilbc[1])
                {
                    rval.blz = iban.Substring(int.Parse(ilbc[9]), int.Parse(ilbc[8]));
                    using (PrismaExtended context = new PrismaExtended())
                    {
                        try
                        {
                            /*	Zuerst muss die BLZ inkl. Filialinformation + Kontrollziffer gesucht werden
                                -	Wird nichts gefunden, wird die BLZ inkl. Filialinformation gesucht
                                -	Wird nichts gefunden, wird nur die BLZ gesucht
                            */
                            int branchlength = int.Parse(ilbc[10]);
                            int branchcheck = int.Parse(ilbc[11]);
                            int cblz = 0;

                            List<BLZInfo> blzList = new List<BLZInfo>();
                            if (branchlength > 0)
                            {
                                if (branchcheck > 0)
                                {
                                    cblz = int.Parse(iban.Substring(int.Parse(ilbc[9]), int.Parse(ilbc[8]) + branchlength + branchcheck));
                                    blzList.AddRange(context.ExecuteStoreQuery<BLZInfo>("select NAME, BLZ, BIC, SYSBLZ from cic.blz where cic.to_num(blz)=cic.to_num('" + cblz + "')", null).ToList());
                                }
                                cblz = int.Parse(iban.Substring(int.Parse(ilbc[9]), int.Parse(ilbc[8]) + branchlength));
                                blzList.AddRange(context.ExecuteStoreQuery<BLZInfo>("select NAME, BLZ, BIC, SYSBLZ from cic.blz where cic.to_num(blz)=cic.to_num('" + cblz + "')", null).ToList());
                            }
                            cblz = int.Parse(iban.Substring(int.Parse(ilbc[9]), int.Parse(ilbc[8])));
                            blzList.AddRange(context.ExecuteStoreQuery<BLZInfo>("select NAME, BLZ, BIC, SYSBLZ from cic.blz where cic.to_num(blz)=cic.to_num('" + cblz + "')", null).ToList());


                            if (blzList != null)
                            {
                                foreach (BLZInfo blz in blzList)
                                {
                                    // if (blz.BIC.Trim().Equals(bic.Trim()))
                                    {
                                        rval.bicwarning = false;
                                        rval.bankname = blz.NAME;
                                        rval.sysblz = blz.SYSBLZ;
                                        break;
                                    }
                                }
                                /*  if (rval.bicwarning)//test xxx filialcode is empty
                                  {
                                      String bic2 = bic.Trim() + "XXX";
                                      foreach (BLZInfo blz in blzList)
                                      {
                                          if (blz.BIC.Trim().Equals(bic2))
                                          {
                                              rval.bicwarning = false;
                                              rval.bankname = blz.NAME;
                                              rval.sysblz = blz.SYSBLZ;
                                              rval.newBIC = bic2;
                                              break;
                                          }
                                      }
                                  }*/
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }

                }
            }

            //return validation of a and c and bankname
            return rval;
        }

        /// <summary>
        /// Determine the correct 4-char block field by complete index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int getErrorField(int index)
        {
            return (int)Math.Floor((double)(index) / 4);
        }

        /// <summary>
        /// Validates an iban
        /// </summary>
        /// <param name="iban"></param>
        /// <returns></returns>
        public IBANValidationError checkIBAN(String iban)
        {
            int standard = -1;
            Regex illegal = new Regex(@"\W|_"); // contains chars other than (a-zA-Z0-9) 
            IBANValidationError rval = new IBANValidationError();
            if (iban == null || iban.Length < 8)
            {
                rval.error = IBANValidationErrorType.InvalidLength;
                rval.detail = "";
                rval.errorFields = new int[] { 0 };
                return rval;
            }
            if (illegal.IsMatch(iban))
            { // yes, alert and exit
                illegal = new Regex(@"((\W|_)+)");

                String aliban = "";
                int lindex = -1;
                List<int> errors = new List<int>();

                foreach (Match match in illegal.Matches(iban))
                {
                    aliban += iban.Substring(lindex + 1, match.Index - (lindex + 1)) + "|" + match.Value + "|";
                    errors.Add(getErrorField(match.Index));
                    lindex = match.Index;
                }
                aliban += iban.Substring(lindex + 1);
                aliban = aliban.Replace(@"\|", "%7C");

                rval.error = IBANValidationErrorType.InvalidCharacter;
                rval.detail = aliban;
                rval.errorFields = errors.ToArray();
                return rval;
            }

            illegal = new Regex(@"^\D\D\d\d.+"); // first chars are letter letter digit digit
            if (illegal.IsMatch(iban) == false)
            { // no, alert and exit


                rval.error = IBANValidationErrorType.InvalidStart;
                rval.detail = "|" + iban.Substring(0, 4) + "|" + iban.Substring(5);
                rval.errorFields = new int[] { 0 };
                return rval;
            }

            illegal = new Regex(@"^\D\D00.+|^\D\D01.+|^\D\D99.+"); // check digits are 00 or 01 or 99
            if (illegal.IsMatch(iban))
            { // yes, alert and exit


                rval.error = IBANValidationErrorType.InvalidChecksum;
                rval.detail = iban.Substring(0, 2) + "|" + iban.Substring(2, 2) + "|" + iban.Substring(5);
                rval.errorFields = new int[] { 0 };
                return rval;
            }
            else
            { // no, continue
                String[] ilbc = (from cset in ilbced
                                 where cset[0].Equals(iban.Substring(0, 2).ToUpper())
                                 select cset).FirstOrDefault();//Suche nach LAND, unabhängig von Case
                //lofi = ilbc.slice(0,ctcnt).in_array(iban.Substring(0,2).ToUpper()); // IsMatch if country respected
                if (ilbc == null)
                {
                    rval.error = IBANValidationErrorType.InvalidCountry;
                    rval.detail = iban.Substring(0, 2).ToUpper();
                    rval.errorFields = new int[] { 0 };
                    return rval;
                }
                illegal = new Regex(@".+");

                if ((iban.Length - int.Parse(ilbc[1]) != 0))
                { // fits length to country

                    rval.error = IBANValidationErrorType.InvalidLength;
                    rval.detail = ilbc[1];
                    rval.errorFields = new int[] { getErrorField(int.Parse(ilbc[1]) - 1) };
                    return rval;
                } // yes, continue

                illegal = buildIsMatch("B04" + ilbc[2], standard);
                // fetch sub structure of respected country

                // or take care of not respected country
                if (!illegal.IsMatch(iban))
                { // fits sub structure to country

                    rval.error = IBANValidationErrorType.InvalidCountryFormat;
                    rval.detail = getStructureAlert(ilbc[2], iban, rval);
                    return rval;
                }
                else
                { // yes, continue

                    String chksum = ISO7064Mod97_10(ISO13616Prepare(iban));
                    if ("1".Equals(chksum))
                    {
                        rval.error = IBANValidationErrorType.NoError;
                        rval.detail = iban;
                    }
                    else
                    {
                        rval.error = IBANValidationErrorType.InvalidChecksum;
                        rval.errorFields = new int[] { 0 };
                        rval.detail = chksum;
                    }
                    return rval;
                }
            }


        }

        /// <summary>
        /// creates the regular expression by the configured iban-structure
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        private Regex buildIsMatch(String structure, int kind)
        {
            String result = "";

            int i = 0;
            Regex pattern = new Regex(@"([ABCFLUW]\d\d)");

            foreach (Match match in pattern.Matches(structure))
            {

                if (((kind >= 0) && (i != kind)) || (kind == -2))
                {
                    result += isMatchpart(match.Value, "any");
                }
                else
                {
                    result += isMatchpart(match.Value, "standard");
                }
                i++;
            }
            return new Regex(result);
        }

        /// <summary>
        /// Build a regex for a certain part
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        private String isMatchpart(String pattern, String kind)
        {
            String IsMatchpattern = "(";
            if (kind == "any")
            {
                IsMatchpattern += ".";
            }
            else
            {
                IsMatchpattern += "[";
                if (kind == "reverse")
                {
                    IsMatchpattern += "^";
                }
                switch (pattern.Substring(0, 1))
                {
                    case "A": IsMatchpattern += "0-9A-Za-z"; break;
                    case "B": IsMatchpattern += "0-9A-Z"; break;
                    case "C": IsMatchpattern += "A-Za-z"; break;
                    case "F": IsMatchpattern += "0-9"; break;
                    case "L": IsMatchpattern += "a-z"; break;
                    case "U": IsMatchpattern += "A-Z"; break;
                    case "W": IsMatchpattern += "0-9a-z"; break;
                }
                IsMatchpattern += "]";
            }
            if (((int.Parse(pattern.Substring(1, 2))) > 1) && (kind != "reverse"))
            {
                IsMatchpattern += "{" + int.Parse(pattern.Substring(1, 2)) + "}";
            }
            IsMatchpattern += ")";
            return IsMatchpattern;
        }

        /// <summary>
        /// Returns the failing part of the iban, enclosed by | |
        /// </summary>
        /// <param name="structureStr"></param>
        /// <param name="iban"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private String getStructureAlert(String structureStr, String iban, IBANValidationError error)
        {
            int any = -2;
            String structure = "B04" + structureStr;
            String result = "";
            Regex failpattern;
            int i = 0;
            Regex pattern = new Regex(@"([ABCFLUW]\d\d)");
            List<int> errors = new List<int>();
            foreach (Match match in pattern.Matches(structure))
            {

                failpattern = buildIsMatch(structure, i);
                if (!failpattern.IsMatch(iban))
                {
                    failpattern = buildIsMatch(structure, any);


                    String ibanpart = failpattern.Match(iban).Groups[i + 1].Value;

                    Regex failpattern2 = new Regex(isMatchpart(match.Value, "reverse"));

                    int failure = 0;
                    int pos = 0;
                    MatchCollection mc = failpattern2.Matches(ibanpart);


                    while ((pos < ibanpart.Length) && (failure < mc.Count))
                    {
                        if (ibanpart.Substring(pos, 1).Equals(mc[failure].Value))
                        {
                            result += "|" + ibanpart.Substring(pos, 1) + "|";
                            errors.Add(getErrorField(iban.IndexOf(ibanpart) + pos));
                            ++failure;
                        }
                        else
                        {
                            result += ibanpart.Substring(pos, 1);
                        }
                        ++pos;
                    }
                    result += ibanpart.Substring(pos) + " ";

                }
                else
                {
                    String ibanparts = failpattern.Match(iban).Groups[i + 1].Value;
                    result += ibanparts + " ";
                }
                i++;
            }
            error.errorFields = errors.Distinct().ToArray();
            result = result.Replace("||", "");
            return result;
        }

    }
}
