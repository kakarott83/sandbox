using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System.Reflection;
using System.Web;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST;
using Cic.OpenOne.Common.Util.Config;
using System.Security.Cryptography;
using AutoMapper;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST
{
    ///enumerates different WBM Web Method Names from the loginservice response
    public enum FoodasMethod
    {
        WMBeauftragungSpeichern = 0,
        WMBeauftragungStornieren=1,
        WMGetBewegung_Auftrag_Dokument=2,
        WMGetDokument=3,
        WMGetEVT_Event=4,
        WMGrunddatenImportSpeichern=5
    }
    /// <summary>
    /// BO handling the PS-Team Foodas Interface
    /// </summary>
    public class FoodasBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private String user, password, appkeywhole,appkeyretail, loginkey;
        private bool loggedIn = false;
        private String lastURL = "", lastError = "";//holds error log info in case of error
        private static String EPNAME = "PSFoodasWSSoap";
        private List<OutputDataWebServices> services;

        public FoodasBo()
        {
            user = AppConfig.getValueFromDb("SETUP", "PSTEAM", "USER");
            password = AppConfig.getValueFromDb("SETUP", "PSTEAM", "PASSWORD");
            appkeywhole = AppConfig.getValueFromDb("SETUP", "PSTEAM", "APPKEYWHOLE");
            appkeyretail = AppConfig.getValueFromDb("SETUP", "PSTEAM", "APPKEYRETAIL");
            

            password = CreateMD5(password);
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// returns the url for a web method
        /// e.g.
        /// https://ws001.ps-team.de/PSTeam.PSFoodas.WS_HCE_KIA_ASF_Test/PSTeam.PSFoodas.WS.asmx
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private String getUrl(FoodasMethod m)
        {
            String rval = (from t in services
                                       where t.WBM_WebMethodName.Equals(m.ToString())
                                       select t.ProgrammPfad).FirstOrDefault();
            lastURL = rval;
            return rval;
        }
        
        /// <summary>
        /// Mit dieser Methode wird der Dokumentenversand beauftragt (also OL beauftragt PS-Team den Brief an den Händler zu schicken)
        /// </summary>
        public FoodasBeauftragungOutDto beauftragungSpeichern(FoodasBeauftragungInDto iDto, bool retail, bool tempendg)
        {
            FoodasBeauftragungOutDto rval = new FoodasBeauftragungOutDto();
            login(retail);
            FoodasServiceReference.PSFoodasWSSoapClient sv = new FoodasServiceReference.PSFoodasWSSoapClient(EPNAME,getUrl(FoodasMethod.WMBeauftragungSpeichern));
            
            InputData i = new InputData();
            List<object> args = new List<object>();
            addArgument(args, "IgnoreErrors", "true","-1");//Steuert, ob die Methode beim Auftreten eines Validierungsfehlers abgebrochen werden soll, oder der Vorgang fortgeführt wird
            addArgument(args, "ValidateOnly", "false", "-1");//Gibt an, ob nur validiert werden soll, ohne eine Datenänderung durchzuführen
            addArgument(args, "TempEndg", tempendg ? "true" : "false", "-1");//Muss angegeben werden, wenn für die betroffenen Dokumente eine Statusumsetzung von temp. Versand auf endgültige Entnahme durchgeführt werden soll
            int recordCounter = 0;

            addDataRecord(args, "VW_Dokument", "Select", "-1", recordCounter);
            recordCounter = 1;
            addDataRecord(args, "Bewegung_Auftrag_Dokument", "Insert", "0", recordCounter);

            recordCounter = 0;
            addDataRecordField(args, "KundenRef", iDto.KundenRef, recordCounter);
            addDataRecordField(args, "FahrzeugIdent", iDto.FahrzeugIdent, recordCounter);

            recordCounter = 1;
            addDataRecordField(args, "Anschreiben_typ", iDto.Anschreiben_typ, recordCounter);
            addDataRecordField(args, "Anschreiben_Anrede", iDto.Anschreiben_Anrede, recordCounter);
            addDataRecordField(args, "Versandart", iDto.Versandart, recordCounter);
            addDataRecordField(args, "Empfaenger_Ansprechpartner", iDto.Empfaenger_Ansprechpartner, recordCounter);
            addDataRecordField(args, "Empfaenger_Name", iDto.Empfaenger_Name, recordCounter);
            addDataRecordField(args, "Empfaenger_Name2", iDto.Empfaenger_Name2, recordCounter);
            addDataRecordField(args, "Empfaenger_Strasse", iDto.Empfaenger_Strasse, recordCounter);
            addDataRecordField(args, "Empfaenger_Plz", iDto.Empfaenger_Plz, recordCounter);
            addDataRecordField(args, "Empfaenger_Ort", iDto.Empfaenger_Ort, recordCounter);
            addDataRecordField(args, "Empfaenger_Land", iDto.Empfaenger_Land, recordCounter);
            addDataRecordField(args, "Bemerkung_1", iDto.Bemerkung_1, recordCounter);
            addDataRecordField(args, "Bemerkung_2", iDto.Bemerkung_2, recordCounter);
            addDataRecordField(args, "Bemerkung_3", iDto.Bemerkung_3, recordCounter);
            addDataRecordField(args, "Bemerkung_4", iDto.Bemerkung_4, recordCounter);
            addDataRecordField(args, "Bemerkung_5", iDto.Bemerkung_5, recordCounter);
            addDataRecordField(args, "Bemerkung_6", iDto.Bemerkung_6, recordCounter);
            addDataRecordField(args, "Anschreiben_Bemerkung", iDto.Anschreiben_Bemerkung, recordCounter);

            i.Items = args.ToArray();

            String input = XMLSerializer.SerializeUTF8(i);
            String output = sv.WMBeauftragungSpeichern(loginkey, input);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(output);
            OutputData data = XMLDeserializer.objectFromXml<OutputData>(bytes, "UTF-8");
            String auftragsnummer = "";
            if(checkError(rval,"beauftragungSpeichern", data, "ID", ref auftragsnummer,input,output))
            {
                logout();
                return rval;
            }
            _log.Debug("beauftragungSpeichern got job#"+auftragsnummer);


            logout();
            rval.auftragsnummer = auftragsnummer;

            return rval;
        }

        /// <summary>
        /// Die Methode wird für die Übermittlung der AVIS-Liste verwendet. Mit dieser Methode werden die Daten für die gelieferten Kfz-Briefe (im Voraus) an PS-Team übermittelt.
        /// </summary>
        public List<FoodasGrunddatenOutDto> grunddatenImportSpeichernMass(List<FoodasGrunddatenInDto> inputdataList, bool retail)
        {
            
            login(retail);
            FoodasServiceReference.PSFoodasWSSoapClient sv = new FoodasServiceReference.PSFoodasWSSoapClient(EPNAME, getUrl(FoodasMethod.WMGrunddatenImportSpeichern));
            InputData i = new InputData();
            List<object> args = new List<object>();
            addArgument(args, "IgnoreErrors", "true");//Steuert, ob die Methode beim Auftreten eines Validierungsfehlers abgebrochen werden soll, oder der Vorgang fortgeführt wird
            addArgument(args, "ValidateOnly", "false");//Gibt an, ob nur validiert werden soll, ohne eine Datenänderung durchzuführen

            int recordCounter = 0;

            foreach (FoodasGrunddatenInDto inputdata in inputdataList)
            {
                addDataRecord(args, "GrundDatenImport", "Insert", "-1", recordCounter);
                recordCounter++;
            }
            recordCounter = 0;
            foreach (FoodasGrunddatenInDto inputdata in inputdataList)
            {
                addDataRecordField(args, "AKt_Briefnummer", inputdata.AKt_Briefnummer, recordCounter);
                addDataRecordField(args, "AKt_Kennzeichen", inputdata.AKt_Kennzeichen, recordCounter);
                addDataRecordField(args, "Zulassungs_datum", inputdata.Zulassungs_datum, recordCounter);
                addDataRecordField(args, "ErstZulassung", inputdata.ErstZulassung, recordCounter);
                addDataRecordField(args, "Abmeldungs_datum", inputdata.Abmeldungs_datum, recordCounter);
                addDataRecordField(args, "FahrzeugIdent", inputdata.FahrzeugIdent, recordCounter);
                addDataRecordField(args, "KundenRef", inputdata.KundenRef, recordCounter);
                addDataRecordField(args, "Haendler_id", inputdata.Haendler_id, recordCounter);
                addDataRecordField(args, "Hersteller_Text", inputdata.Hersteller_Text, recordCounter);
                addDataRecordField(args, "Typ_Text", inputdata.Typ_Text, recordCounter);
                addDataRecordField(args, "Zusatzfeld_1", inputdata.Zusatzfeld_1, recordCounter);
                addDataRecordField(args, "Zusatzfeld_2", inputdata.Zusatzfeld_2, recordCounter);
                recordCounter++;
            }

            i.Items = args.ToArray();

            String input = XMLSerializer.SerializeUTF8(i);
            String output = sv.WMGrunddatenImportSpeichern(loginkey, input);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(output);
            OutputData data = XMLDeserializer.objectFromXml<OutputData>(bytes, "UTF-8");


           
            List<OutputDataReturnValues> rvalues = (from d in data.Items
                                 where d is OutputDataReturnValues
                                 && ((OutputDataReturnValues)d).Name.Equals("ID")
                                 select ((OutputDataReturnValues)d)).ToList();
            
            List<OutputDataErrors> errors = (from d in data.Items
                                        where d is OutputDataErrors
                                        select ((OutputDataErrors)d)).ToList();

            List<FoodasGrunddatenOutDto> rvalList = new List<FoodasGrunddatenOutDto>();
            for(int t=0;t<recordCounter;t++)
            {
                FoodasGrunddatenOutDto rval = new FoodasGrunddatenOutDto();
                OutputDataReturnValues val = (from d in rvalues
                                                  where d.RecordID.Equals(""+t)
                                                  select d).FirstOrDefault();
                if(val!=null)
                {
                    rval.RecordID = val.RecordID;
                    rval.vorgangsnummer = val.Value;
                }
                else
                {
                    OutputDataErrors message = (from d in errors
                                                  where d.RecordID.Equals(""+t)
                                                  select d).FirstOrDefault();
                    if (message != null)
                    {
                         _log.Error("Grunddatenimport failed with Error " + message.Code + "/" + message.Message + " in Record " + message.RecordID);
                        rval.hasError = true;
                        rval.errorCode = message.Code;
                        rval.errorMessage = message.Message;
                        rval.errorRecordID = message.RecordID;
                    }
                }
                rvalList.Add(rval);

            }

            logout();
            return rvalList;
        }

        /// <summary>
        /// Die Methode wird für die Übermittlung der AVIS-Liste verwendet. Mit dieser Methode werden die Daten für die gelieferten Kfz-Briefe (im Voraus) an PS-Team übermittelt.
        /// </summary>
        public FoodasGrunddatenOutDto grunddatenImportSpeichern(FoodasGrunddatenInDto inputdata, bool retail)
        {
            FoodasGrunddatenOutDto rval = new FoodasGrunddatenOutDto();
            login(retail);
            FoodasServiceReference.PSFoodasWSSoapClient sv = new FoodasServiceReference.PSFoodasWSSoapClient(EPNAME, getUrl(FoodasMethod.WMGrunddatenImportSpeichern));
            InputData i = new InputData();
            List<object> args = new List<object>();
            addArgument(args, "IgnoreErrors", "true");//Steuert, ob die Methode beim Auftreten eines Validierungsfehlers abgebrochen werden soll, oder der Vorgang fortgeführt wird
            addArgument(args, "ValidateOnly", "false");//Gibt an, ob nur validiert werden soll, ohne eine Datenänderung durchzuführen
            
            int recordCounter = 0;


            addDataRecord(args, "GrundDatenImport", "Insert", "-1", recordCounter);

            addDataRecordField(args, "AKt_Briefnummer", inputdata.AKt_Briefnummer, recordCounter);
            addDataRecordField(args, "AKt_Kennzeichen", inputdata.AKt_Kennzeichen, recordCounter);
            addDataRecordField(args, "Zulassungs_datum", inputdata.Zulassungs_datum, recordCounter);
            addDataRecordField(args, "ErstZulassung", inputdata.ErstZulassung, recordCounter);
            addDataRecordField(args, "Abmeldungs_datum", inputdata.Abmeldungs_datum, recordCounter);
            addDataRecordField(args, "FahrzeugIdent", inputdata.FahrzeugIdent, recordCounter);
            addDataRecordField(args, "KundenRef", inputdata.KundenRef, recordCounter);
            addDataRecordField(args, "Haendler_id", inputdata.Haendler_id, recordCounter);
            addDataRecordField(args, "Hersteller_Text", inputdata.Hersteller_Text, recordCounter);
            addDataRecordField(args, "Typ_Text", inputdata.Typ_Text, recordCounter);
            addDataRecordField(args, "Zusatzfeld_1", inputdata.Zusatzfeld_1, recordCounter);
            addDataRecordField(args, "Zusatzfeld_2", inputdata.Zusatzfeld_2, recordCounter);


            i.Items = args.ToArray();

            String input = XMLSerializer.SerializeUTF8(i);
            String output = sv.WMGrunddatenImportSpeichern(loginkey, input);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(output);
            OutputData data = XMLDeserializer.objectFromXml<OutputData>(bytes, "UTF-8");
            String vorgangsnummer = "";
            if (checkError(rval,"grunddatenImportSpeichern", data, "ID", ref vorgangsnummer, input,output))
            {
                logout();
                return rval;
            }
            _log.Debug("grunddatenImportSpeichern got #" + vorgangsnummer);
            rval.vorgangsnummer = vorgangsnummer;

            logout();
            return rval;

        }

        /// <summary>
        /// Processes PS-TEAM EVENT-Data based on a file-content <OutputData>....</OutputData>
        /// </summary>
        /// <param name="outputData"></param>
        /// <returns></returns>
        public FoodasEventOutDto processEvents(byte[] bytes)
        {
            
            OutputData data = XMLDeserializer.objectFromXml<OutputData>(bytes, "UTF-8");
            String vorgangsnummer = "";
            FoodasEventOutDto rval = new FoodasEventOutDto();
            if (checkError(rval, "WMGetEVT_Event", data, null, ref vorgangsnummer,"",""))
            {
                return rval;
            }
            if (data.Items != null)
            {
                
                
                List<OutputDataData> datas = (from t in data.Items
                                              where t is OutputDataData
                                              select (OutputDataData)t).ToList<OutputDataData>();
                
                rval.events = Mapper.Map<List<OutputDataData>, List<FoodasEventOutDataDto>>(datas);
            }
            return rval;
        }

        /// <summary>
        /// Mit dieser Methode können alle neue Events seit dem letzten Aufruf abgefragt werden. Zu den Events zählen die Dokumenteingänge, Versendungen, Mahnungen, Scannen des Dokumentes, etc. 
        /// </summary>
        public FoodasEventOutDto getEvents(bool retail)
        {
           
            login(retail);
            FoodasServiceReference.PSFoodasWSSoapClient sv = new FoodasServiceReference.PSFoodasWSSoapClient(EPNAME, getUrl(FoodasMethod.WMGetEVT_Event));
            InputData i = new InputData();
            List<object> args = new List<object>();
            addArgument(args, "GetEVT_GetGemeldet", "false");//Mit dem Argument werden nur die bereits gemeldete Datensätze abgefragt. Dabie ist eine Beschränkung auf EVT_Datum zwingend erforderlich
            addArgument(args, "GetEVT", "true");//Gibt an, ob nur validiert werden soll, ohne eine Datenänderung durchzuführen
            addArgument(args, "NoCompression", "true", "-1");


            addField(args, "EVT_ID");
            //Ablauf: 
            //-> AVIS (Registrierung kfz-brief bei psteam) == Grunddatenimport
            //-> Versand an PS-Team über OL
            //-> EVENT 2==Ersteingang (bei PS-Team)
            //-> lagerort=PS-TEAM

            //-> OL: KFZ-Brief soll an Händler (Button wird gedrückt)
            //-> beauftragung-speichern
            //-> PS-Team versendet an händler
            //-> EVENT-Eingang 5, dann 8
            //-> lagerort=Händler

            //später.....
            //-> Händler versendet zurück an PS-TEAM
            //-> EVENT-Eingang= 10

            /*1 = Grunddatenimport
            2 = Ersteingang
            3 = Versandsperre
            4 = Entsperre
            5 = Auftragseingang
            6 = Auftragsfreigabe
            7 = Storno
            8 = Entnahme
            9 = Mahnung
            10 = Wiedereingang
            11 = Statusumsetzung
            12 = Dokument löschen
            13 = In Aufbietung
            14 = Aufbietung abgeschlossen
            16 = Fahrzeugdatenänderung
            17 = Ablage des PDF-Dokumentes*/
            addField(args, "EVT_EVA_ID");
            addField(args, "EVT_Anlage_Datum");
            addField(args, "EVT_BAD_ID");
            addField(args, "FahrzeugIdent");
            addField(args, "Pruefziffer");
            addField(args, "AKt_Kennzeichen");
            addField(args, "AKt_Briefnummer");
            addField(args, "KundenRef");
            addField(args, "Vertragsnummer");
            addField(args, "ErstZulassung");
            addField(args, "Abmeldungs_datum");
            addField(args, "Zulassungs_datum");
            addField(args, "Status");
            addField(args, "Haendler_id");
            addField(args, "Hersteller_Text");
            addField(args, "Typ_Text");
            addField(args, "Sperrung_Typ");
            addField(args, "Dokument_ID");
            addField(args, "Dokument_typ");
            addField(args, "Psbarcode");
            addField(args, "Akt_Bewegung_typ_id");
            addField(args, "Fahrzeug_ID");
            addField(args, "Bewegung_Auftrag_ID");
            addField(args, "Auftragsstatus");
            addField(args, "Empfaenger_Versandcode");
            addField(args, "Empfaenger_Name");
            addField(args, "Empfaenger_Name2");
            addField(args, "Anschreiben_Anrede");
            addField(args, "Empfaenger_Ansprechpartner");
            addField(args, "Empfaenger_Strasse");
            addField(args, "Empfaenger_Plz");
            addField(args, "Empfaenger_Ort");
            addField(args, "Empfaenger_Land");
            addField(args, "Bewegung_typ_ID");
            addField(args, "Versandart");
            addField(args, "Anschreiben_typ");
            addField(args, "Mahnstufe");
            addField(args, "Mahnstufe_Level");
            addField(args, "Einschreibennummer");
            addField(args, "KEP_Frachtnummer");


            i.Items = args.ToArray();

            String input = XMLSerializer.SerializeUTF8(i);
            String output = sv.WMGetEVT_Event(loginkey, input);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(output);
            OutputData data = XMLDeserializer.objectFromXml<OutputData>(bytes, "UTF-8");
            String vorgangsnummer = "";
            FoodasEventOutDto rval = new FoodasEventOutDto();
            if (checkError(rval,"WMGetEVT_Event", data, null, ref vorgangsnummer,input,output))
            {
                logout();
                return rval;
            }
            if(data.Items!=null)
            { 
                
                
                List < OutputDataData> datas = (from t in data.Items
                         where t is OutputDataData
                         select (OutputDataData)t).ToList<OutputDataData>();
                
                rval.events = Mapper.Map<List<OutputDataData>, List<FoodasEventOutDataDto>>(datas);
            }
            else
            {
                _log.Warn("No Events received for getEvents(" + retail + ")");
            }
            logout();
            return rval;
        }

        /// <summary>
        /// Mit dieser Methode können Dokumentinformationen inkl. PDF-Datei von PS-Team abgefragt werden.
        /// ACHTUNG: bei der PDF-Abfrage ist eine Beschränkung auf ein Fahrzeug oder ein Dokument erforderlich
        /// </summary>
        public FoodasGetDokumentOutDto getDokument(String fzid, bool retail)
        {
            FoodasGetDokumentOutDto rval = new FoodasGetDokumentOutDto();
            login(retail);
            FoodasServiceReference.PSFoodasWSSoapClient sv = new FoodasServiceReference.PSFoodasWSSoapClient(EPNAME, getUrl(FoodasMethod.WMGetDokument));
            InputData i = new InputData();
            List<object> args = new List<object>();
            addArgument(args, "PDFTransfer", "true","-1");//Angabe, ob PDF übertragen werden soll. Falls ja, wird der PDF-Datei in einem Feld "PDFStream" zurückgeliefert.
            addArgument(args, "NoCompression", "true","-1");

            addField(args, "Id");
            addField(args, "Fahrzeug_id");
            addField(args, "AKt_Briefnummer");
            addField(args, "AKt_Kennzeichen");
            addField(args, "FahrzeugIdent");
            addField(args, "KundenRef");
            addField(args, "Vertragsnummer");
            addField(args, "Haendler_id");
            addField(args, "Dokument_typ");
            addField(args, "Akt_Bewegung_typ_id");
            addField(args, "Psbarcode");
            addWhereToken(args, "Dokument_typ", "FieldGtValue", "15");
            addWhereToken(args, "Dokument_typ", "FieldLtValue", "20");
            addWhereToken(args, "FahrzeugIdent", "FieldEqValue", fzid);
            i.Items = args.ToArray();

            String input = XMLSerializer.SerializeUTF8(i);
            _log.Debug("Get Dokument with " + input);
            String output = sv.WMGetDokument(loginkey, input);
            
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(output);
            OutputData data = XMLDeserializer.objectFromXml<OutputData>(bytes, "UTF-8");
            String dummy="";

            if (checkError(rval, "getDokument", data, null,ref dummy,input,output))
            {
                logout();
                return rval;
            }
            if (data.Items != null && data.Items.Count() > 0)
            {

                
                
                List<OutputDataData> datas = (from t in data.Items
                                              where t is OutputDataData
                                              select (OutputDataData)t).ToList<OutputDataData>();
                OutputDataData myData = datas.OrderByDescending(dt=>dt.Dokument_typ).FirstOrDefault();
               
                rval = Mapper.Map<OutputDataData, FoodasGetDokumentOutDto>(myData, rval);
                rval.data = Convert.FromBase64String(myData.PDFStream); //Encoding.UTF8.GetBytes(myData.PDFStream);

            }
            else
            {
                _log.Warn("No Data received for getDokument(" + fzid + "," + retail + ")");
            }
            logout();

            return rval;
        }

        /// <summary>
        /// adds a record field
        /// </summary>
        /// <param name="args"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        private void addDataRecordField(List<object> args, String field, String value, int recordCounter)
        {
            InputDataDataRecordFields arg = new InputDataDataRecordFields();
            arg.Field = field;
            arg.Value = value;
            arg.RecordID = recordCounter.ToString();
            args.Add(arg);
        }

        /// <summary>
        /// adds a record with the action
        /// </summary>
        /// <param name="args"></param>
        /// <param name="table"></param>
        /// <param name="action"></param>
        private void addDataRecord(List<object> args, String table, String action, String parent, int recordCounter)
        {
            InputDataDataRecords arg = new InputDataDataRecords();
            arg.TableName = table;
            arg.DBAction = action;
            arg.RecordID = recordCounter.ToString();
            arg.ParentRecordID = parent;
            args.Add(arg);
        }

        /// <summary>
        /// add a field
        /// </summary>
        /// <param name="args"></param>
        /// <param name="field"></param>
        private void addField(List<object> args, String field)
        {
            InputDataFields f = new InputDataFields();
            f.Field = field;
            args.Add(f);
        }

       /// <summary>
       /// add a where-token for filtering the result
       /// </summary>
       /// <param name="args"></param>
        /// <param name="field">e.g. FahrzeugIdent</param>
       /// <param name="type">eg. FieldEqValue</param>
       /// <param name="value"></param>
        private void addWhereToken(List<object> args, String field, String type, String value)
        {
            InputDataWhereTokens f = new InputDataWhereTokens();
            f.Field = field;
            f.Type = type;
            f.Value1 = value;
            args.Add(f);
        }

        /// <summary>
        /// adds an argument field
        /// </summary>
        /// <param name="args"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void addArgument(List<object> args, String name, String value)
        {
            InputDataArguments arg = new InputDataArguments();
            arg.Name = name;
            arg.Value = value;
            args.Add(arg);
        }
        /// <summary>
        /// adds an argument field
        /// </summary>
        /// <param name="args"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void addArgument(List<object> args, String name, String value, String recId)
        {
            InputDataArguments arg = new InputDataArguments();
            arg.Name = name;
            arg.Value = value;
            arg.RecordID = recId;
            args.Add(arg);
        }

       
        /// <summary>
        /// Login to Foodas System, obtaining a loginkey for further method calls
        /// </summary>
        private void login(bool appKeyRetail)
        {
            if (loggedIn)
            {
                return;
            }
            FoodasLoginServiceReference.WSLoginSoapClient login = new FoodasLoginServiceReference.WSLoginSoapClient();
            String loginresponse = login.WMLogin(user, password, appKeyRetail?appkeyretail:appkeywhole);
            _log.Debug("got loginvalue: " + loginresponse);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(loginresponse);
            OutputData data = XMLDeserializer.objectFromXml<OutputData>(bytes, "UTF-8");
            loginkey = (from d in data.Items
                        where d is OutputDataReturnValues
                        && ((OutputDataReturnValues)d).Name.Equals("LoginKey")
                        select ((OutputDataReturnValues)d).Value).FirstOrDefault();

            _log.Debug("WSLogin to FOODAS delivered Loginkey: " + loginkey);

            String message = (from d in data.Items
                              where d is OutputDataErrors
                              select ((OutputDataErrors)d).Message).FirstOrDefault();
            if (message != null)
            {
                _log.Error("WSLogin to FOODAS delivered Error: " + message);
            }

            if (loginkey != null)
            {
                this.loggedIn = true;
            }

            services = (from d in data.Items
                                              where d is OutputDataWebServices
                                              select (OutputDataWebServices)d).ToList();


          
        }
        /// <summary>
        /// Validates for an error, returns true when error occured
        /// </summary>
        /// <param name="methodname"></param>
        /// <param name="data"></param>
        /// <param name="rvalname"></param>
        /// <param name="rval"></param>
        /// <returns></returns>
        private bool checkError(FoodasOutDto retval, String methodname, OutputData data, String rvalname, ref String rval, String debugInput, String debugOutput)
        {
           
            retval.hasError = false;
            if(rvalname!=null)
            {
                try
                {
                    String rvalue = (from d in data.Items
                                     where d is OutputDataReturnValues
                                     && ((OutputDataReturnValues)d).Name.Equals(rvalname)
                                     select ((OutputDataReturnValues)d).Value).FirstOrDefault();
                    rval = rvalue;
                }catch(Exception)
                {
                    return false;
                }
            }

            if (data.Items == null)
            {
                return false;
            }

            OutputDataErrors message = (from d in data.Items
                              where d is OutputDataErrors
                              select ((OutputDataErrors)d)).FirstOrDefault();
            
            if(message!=null)
            {
                lastError = methodname + " failed with Error " + message.Code + "/" + message.Message + " in Record " + message.RecordID;
                _log.Error(methodname+" failed with Error " + message.Code+"/"+message.Message+" in Record "+message.RecordID);
                _log.Error("Input was: " + debugInput);
                lastError += " Input=" + debugInput;
                _log.Error("Output was: " + debugOutput);
                lastError += " Output=" + debugOutput;
                retval.hasError = true;
                retval.errorCode = message.Code;
                retval.errorMessage = message.Message;
                retval.errorRecordID = message.RecordID;
                return true;
            }
            return false;
        }

        public void logError(long syseaihot)
        {
            LogUtil.addLogDump("EAIHOT", syseaihot, "PSTeam-Error", lastError, lastURL);
        }

        /// <summary>
        /// Logout from Foodas System
        /// </summary>
        private void logout()
        {
            FoodasLoginServiceReference.WSLoginSoapClient login = new FoodasLoginServiceReference.WSLoginSoapClient();
            String logoutresponse = login.WMLogout(loginkey);
            _log.Debug("got logoutvalue: " + logoutresponse);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(logoutresponse);
            OutputData data = XMLDeserializer.objectFromXml<OutputData>(bytes, "UTF-8");

            String success = (from d in data.Items
                        where d is OutputDataReturnValues
                        && ((OutputDataReturnValues)d).Name.Equals("Retval")
                        select ((OutputDataReturnValues)d).Value).FirstOrDefault();

            String message = (from d in data.Items
                              where d is OutputDataErrors
                              select ((OutputDataErrors)d).Message).FirstOrDefault();
            if (message != null)
            {
                _log.Error("WSLogin to FOODAS delivered Error: " + message);
            }

            if ("1".Equals(success))
            {
                _log.Info("PST Logout successful");
            }
            else
            {
                _log.Error("Logout from WS FOODAS failed: " + success);
            }
            loggedIn = false;
            loginkey = null;
        }
    }
}
