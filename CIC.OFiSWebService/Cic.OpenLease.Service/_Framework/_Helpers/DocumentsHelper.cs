namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess;
    using Cic.OpenLease.ServiceAccess.Merge.OlClient;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Model.DdOw;
    using CIC.Database.OIQUEUE.EF6.Model;
    using CIC.Database.OW.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public static class DocumentsHelper
    {
        #region Private constants
        private const string CnstDocumentsCfgCode = "DOKUMENTE";
        private const string CnstAidaPrintCode = "AIDA_PRINT";
        #endregion

        #region Methods
        public static List<DocumentShortDto> GetDocumentsList(AreaConstants area)
        {
            try
            {
                // Get the config entries
                List<CFGVAR> ConfigEntries = ConfigHelper.GetConfigEntries(CnstDocumentsCfgCode, area.ToString().ToUpper());
                ConfigEntries = (from c in ConfigEntries
                                 orderby c.WERT
                                 select c).ToList();

                // Create a documents list
                List<DocumentShortDto> Documents = new List<DocumentShortDto>();

                using (DdOlExtended context = new DdOlExtended())
                {
                    // Create an assembler
                    DocumentShortAssembler Assembler = new DocumentShortAssembler(context);

                    // Iterate through all congif entries
                    foreach (var LoopConfigEntry in ConfigEntries)
                    {
                        // Convert the entry and add it to the list
                        Documents.Add(Assembler.ConvertToDto(LoopConfigEntry));
                    }
                }

                // Return the list
                return Documents;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the documents list.", e);
            }
        }

        public static long AddToPrintQueue(DocumentShortDto document, long sysAreaId, long sysWfUser, string languageCode, EaiParameter EaiParameter)
        {
            try
            {
                // Verify the area id
                if (!MyVerifyAreaId(document.Area, sysAreaId))
                {
                    // Throw an exception
                    throw new ApplicationException("SysAreaId=" + sysAreaId + " not found in area " + document.Area + ".");
                }
                String area = document.Area.ToString().ToUpper();
                if(document.Area==AreaConstants.Vertragsuebersicht)
                    area="VT";
                if (document.Area == AreaConstants.Ruecknahmeprotokoll)
                    area = "VT";

                // Create the entities
                using (DdOwExtended Entities = new DdOwExtended())
                {
                    // Create a new document
                    CIC.Database.OW.EF6.Model.EAIHOT EaiHot = new CIC.Database.OW.EF6.Model.EAIHOT();

                    // Get an appropriate eai art
                    EaiHot.CODE = CnstAidaPrintCode;
                    EaiHot.EAIART = MyGetAidaPrintEaiArt(Entities);

                    // Set the table
                    EaiHot.OLTABLE = area;

                    // Set the area id
                    EaiHot.SYSOLTABLE = sysAreaId;

                    // Set the status
                    EaiHot.PROZESSSTATUS = (int)DocumentStatusConstants.Pending;

                    // Set the language
                    EaiHot.GUILANGUAGE = languageCode;
                    EaiHot.COMLANGUAGE = languageCode;

                    // Set the event engine enabled
                    EaiHot.EVE = 1;

                    // Set the computer name
                    // EaiHot.COMPUTERNAME = Environment.MachineName;

                    // Set the time
                    DateTime Now = DateTime.Now;
                    EaiHot.SUBMITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(Now);
                    EaiHot.SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Now);

                    // Set the document name, Angebotsnummer, Kundenname, Fahrzeug, Finanzierungsprodukt and Rate
                    EaiHot.INPUTPARAMETER1 = EaiParameter.INPUTPARAMETER1;
                    EaiHot.INPUTPARAMETER4 = EaiParameter.INPUTPARAMETER4;
                    EaiHot.INPUTPARAMETER5 = EaiParameter.INPUTPARAMETER5;
                    EaiHot.OUTPUTPARAMETER3 = EaiParameter.OUTPUTPARAMETER3;
                    EaiHot.OUTPUTPARAMETER4 = EaiParameter.OUTPUTPARAMETER4;
                    EaiHot.OUTPUTPARAMETER5 = EaiParameter.OUTPUTPARAMETER5;
                    
                    // Add the document
                    Entities.EAIHOT.Add(EaiHot);

                    // Save the changes
                    Entities.SaveChanges();

                    // Return the document's id
                    return EaiHot.SYSEAIHOT;
                }
            }
            catch(Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not add the document to the print queue.", e);
            }
        }

        public static DocumentDto GetDocument(long sysEaiHot, long syspuser)
        {
            try
            {
                // Create the entities
                using (DdOwExtended Entities = new DdOwExtended())
                {
                    // Query EAIHOT
                    var CurrentEaiHot = (from EaiHot in Entities.EAIHOT
                                         where EaiHot.SYSEAIHOT == sysEaiHot
                                         select EaiHot).FirstOrDefault();

                    AreaConstants area = AreaConstants.Alle;
                    
                    if (AreaConstants.Angebot.ToString().ToUpper().Equals(CurrentEaiHot.OLTABLE))
                    {
                        long permission = Entities.ExecuteStoreQuery<long>("select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + syspuser + ", 'ANGEBOT', sysdate)) where SYSID=" + CurrentEaiHot.SYSOLTABLE, null).FirstOrDefault();
                        if (permission == 0)
                            throw new ApplicationException("User has no Permission");

                        area = AreaConstants.Angebot;
                    }
                    else if (AreaConstants.Antrag.ToString().ToUpper().Equals(CurrentEaiHot.OLTABLE))

                        area = AreaConstants.Antrag;
                    else if (AreaConstants.It.ToString().ToUpper().Equals(CurrentEaiHot.OLTABLE))
                        area = AreaConstants.It;
                    else if (AreaConstants.VT.ToString().ToUpper().Equals(CurrentEaiHot.OLTABLE))
                        area = AreaConstants.VT;

                    // Return the document
                    DocumentDto rval = MyGetDocument(CurrentEaiHot, Entities, area);

                    // Set FileFlag to show that document has been read
                    if (CurrentEaiHot.FILEFLAGOUT == null)
                        CurrentEaiHot.FILEFLAGOUT = 1;
                    else CurrentEaiHot.FILEFLAGOUT++;
                    Entities.SaveChanges();

                    return rval;
                }
            }
            catch(Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the document.", e);
            }
        }

        public static DocumentDto[] GetDocuments(AreaConstants area, long sysAreaId)
        {
            try
            {
                // Verify the area id
                if (!MyVerifyAreaId(area, sysAreaId))
                {
                    // Throw an exception
                    throw new ApplicationException("SysAreaId=" + sysAreaId + " not found in area " + area + ".");
                }

                // Create the entities
                using (DdOwExtended Entities = new DdOwExtended())
                {
                    // Get the area string
                    string AreaStr = area.ToString().ToUpper();

                    // Query EAIHOT
                    var EaiEntries = from EaiHot in Entities.EAIHOT
                                     where EaiHot.OLTABLE == AreaStr
                                     && EaiHot.SYSOLTABLE == sysAreaId
                                     && EaiHot.PROZESSSTATUS == (int)DocumentStatusConstants.Ready
                                     orderby EaiHot.SYSOLTABLE descending
                                     select EaiHot;

                    // Create a documents list
                    List<DocumentDto> DocumentsList = new List<DocumentDto>();

                    // Loop through all documents
                    foreach (var LoopEai in EaiEntries)
                    {
                        // Add the document to the list
                        DocumentsList.Add(MyGetDocument(LoopEai, Entities, area));
                    }

                    // Return the document
                    return DocumentsList.ToArray();
                }
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the documents.", e);
            }
        }
        #endregion

        #region My methods
        private static DocumentDto MyGetDocument(CIC.Database.OW.EF6.Model.EAIHOT eaiHot, DdOwExtended entities, AreaConstants area)
        {
            try
            {

                // Check if nothing was found
                if (eaiHot == null)
                {
                    // Throw an exception
                    throw new ArgumentException("Argument eaiHot cannot be null.");
                }


                // Create the result
                DocumentDto ResultDocument = new Cic.OpenLease.ServiceAccess.Merge.OlClient.DocumentDto();

                // Set the id
                ResultDocument.SysEaiHot = eaiHot.SYSEAIHOT;
                ResultDocument.Description = "";

                // Set the status
                ResultDocument.Status = (DocumentStatusConstants)eaiHot.PROZESSSTATUS.GetValueOrDefault();


                // Check if status is ready
                if (ResultDocument.Status == DocumentStatusConstants.Ready)
                {
                    // Query EAIHFILE
                    var Files = from EaiFile in entities.EAIHFILE
                                where EaiFile.EAIHOT.SYSEAIHOT == eaiHot.SYSEAIHOT
                                select EaiFile;

                    // Create the files list
                    List<EaiHFileDto> FilesList = new List<EaiHFileDto>();


                    if (area == AreaConstants.Angebot || area == AreaConstants.AngebotEinreichen)
                    {
                        // Get the config entries
                        List<CFGVAR> ConfigEntries = ConfigHelper.GetConfigEntries(CnstDocumentsCfgCode, area.ToString().ToUpper());

                        // Iterate through all congif entries
                        foreach (var LoopConfigEntry in ConfigEntries)
                        {

                            if (LoopConfigEntry.CODE.Equals(eaiHot.INPUTPARAMETER1))
                            {
                                ResultDocument.Description = LoopConfigEntry.BEZEICHNUNG;
                                break;
                            }
                        }
                    }

                    // Loop through all files
                    foreach (var LoopFile in Files)
                    {
                        // Get the DTO
                        FilesList.Add(MyGetEaiHFileDto(LoopFile));
                    }

                    // Assign the files list
                    ResultDocument.Files = FilesList.ToArray();

                    // Query EAIERR
                    var Errors = from EaiError in entities.EAIERR
                                 where EaiError.SYSEAI == eaiHot.SYSEAIHOT
                                 select EaiError;

                    // Create the errors list
                    List<EaiErrorDto> ErrorsList = new List<EaiErrorDto>();
/*
                    // Loop through all errors
                    foreach (var LoopError in Errors)
                    {
                        // Get the DTO
                        ErrorsList.Add(MyGetEaiErrorDto(LoopError));
                    }*/

                    // Assign the errors list
                    ResultDocument.Errors = ErrorsList.ToArray();
                }

                // Return the result
                return ResultDocument;

            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the document.", e);
            }
        }

        private static CIC.Database.OW.EF6.Model.EAIART MyGetAidaPrintEaiArt(DdOwExtended entities)
        {
            try
            {
                // Query EAIART
                var CurrentEaiArt = (from EaiArt in entities.EAIART
                                     where EaiArt.CODE == CnstAidaPrintCode
                                     select EaiArt).FirstOrDefault();

                // Check if nothing was found
                if (CurrentEaiArt == null)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not find " + CnstAidaPrintCode + " in EAIART.");
                }

                // Return the id
                return CurrentEaiArt;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get Aida print eai art.", e);
            }
        }

        private static EaiHFileDto MyGetEaiHFileDto(CIC.Database.OW.EF6.Model.EAIHFILE file)
        {
            // Check if file is null
            if (file == null)
            {
                // Throw an exception
                throw new ArgumentException("Argument file cannot be null.");
            }

            // Create the result
            EaiHFileDto ResultDto = new EaiHFileDto();

            // Set the properties
            ResultDto.FileName = file.TARGETFILENAME;
            ResultDto.Code = file.CODE;
            ResultDto.FileContents = file.EAIHFILE1;
            // Return the result
            return ResultDto;
        }
        /*
        private static EaiErrorDto MyGetEaiErrorDto(CIC.Database.OW.EF6.Model.EAIERR error)
        {
            // Check if error is null
            if (error == null)
            {
                // Throw an exception
                throw new ArgumentException("Argument error cannot be null.");
            }

            // Create the result
            EaiErrorDto ResultDto = new EaiErrorDto();

            // Set the properties
            ResultDto.ErrorNumber = error.ERRORNUMBER.GetValueOrDefault();
            ResultDto.ErrorAction = error.ERRORACTION;
            ResultDto.ErrorText = error.ERRORTEXT;
            ResultDto.ErrorTime = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateAndTimeToDateTime(error.ERRORDATE.GetValueOrDefault(), (int)error.ERRORTIME.GetValueOrDefault());

            // Return the result
            return ResultDto;
        }*/

        private static bool MyVerifyAreaId(AreaConstants area, long sysAreaId)
        {
            // Create the entities
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Assume the id does not exist
                    bool IdExists = false;

                    switch (area)
                    {
                        case AreaConstants.Alle:
                        case AreaConstants.AngebotEinreichen:
                        case AreaConstants.Angebot:
                            var CurrentAngebot = (from Angebot in Entities.ANGEBOT
                                                  where Angebot.SYSID == sysAreaId
                                                  select Angebot).FirstOrDefault();
                            IdExists = CurrentAngebot != null;
                            break;

                        case AreaConstants.Antrag:
                            var CurrentAntrag = (from Antrag in Entities.ANTRAG
                                                 where Antrag.SYSID == sysAreaId
                                                 select Antrag).FirstOrDefault();
                            IdExists = CurrentAntrag != null;
                            break;

                        case AreaConstants.VT:
                        case AreaConstants.Ruecknahmeprotokoll:
                        case AreaConstants.Vertragsuebersicht:
                            var Currentvt = (from vertrag in Entities.VT
                                                 where vertrag.SYSID == sysAreaId
                                                 select vertrag).FirstOrDefault();
                            IdExists = Currentvt != null;
                            break;


                        case AreaConstants.It:
                            var CurrentIt = (from It in Entities.ANTRAG
                                             where It.SYSID == sysAreaId
                                             select It).FirstOrDefault();
                            IdExists = CurrentIt != null;
                            break;
                    }

                    // Return the verification result
                    return IdExists;
                }
                catch (Exception exception)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not verify area id.", exception);
                }
            }
        }
        #endregion
    }
}