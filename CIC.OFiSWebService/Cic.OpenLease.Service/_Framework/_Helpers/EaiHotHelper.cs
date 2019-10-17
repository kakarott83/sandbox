// OWNER MK, 010-06-2010

namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenLease.ServiceAccess.Merge.OlClient;
    using Cic.OpenOne.Common.Model.DdOw;
    using CIC.Database.OW.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion



    public class EaiHotHelper
    {
        #region Private Constans
        private const string CnstCode = "AIDA_SubmitOffer";
        private const string CnstTable = "ANGEBOT";
        #endregion

        #region Methods
        internal static long Angebot2Antrag(long sysAngebot, long sysWfUSer)
        {
            // Fill EAIHOT
            // Create the entities
            using(DdOwExtended OwExtendedEntities = new DdOwExtended())
            {
                // Create a new document
                EAIHOT EaiHot = new EAIHOT();

                // Get an appropriate eai art
                EaiHot.CODE = CnstCode;
                EaiHot.EAIART = MyGetAidaEaiArt(OwExtendedEntities);

                // Set the table
                EaiHot.OLTABLE = CnstTable;

                // Set the area id
                EaiHot.SYSOLTABLE = sysAngebot;

                // Set the status
                EaiHot.PROZESSSTATUS = (int)EaiHotStatusConstants.Pending;       

                // Set the event engine enabled
                EaiHot.EVE = 1;

                // Dont set the computer name
                // EaiHot.COMPUTERNAME = Environment.MachineName;

                // Set the time
                DateTime Now = DateTime.Now;
                EaiHot.SUBMITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(Now);
                EaiHot.SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Now);

                EaiHot.SYSWFUSER = sysWfUSer;
            
                // Add the document
                OwExtendedEntities.EAIHOT.Add(EaiHot);

                // Save the changes
                OwExtendedEntities.SaveChanges();

                // Return the document's id
                return EaiHot.SYSEAIHOT;
            }
        }

        /*
        public static Angebot2AntragStateDto CheckAngebot2Antrag(long sysEaiHot)
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

                    // Return the document
                    return MyGetEaiHot(CurrentEaiHot, Entities);
                }
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the document.", e);
            }
        }
        */
      /*  public static Angebot2AntragStateDto[] CheckAngebot2Antrags(long sysAngebot)
        {
            try
            {
                List<Angebot2AntragStateDto> Angebot2AntragStateDtoList = new List<Angebot2AntragStateDto>();
                // Create the entities
                using (DdOwExtended Entities = new DdOwExtended())
                {
                    // Query EAIHOT
                    var EaiHots = (from EaiHot in Entities.EAIHOT
                                         where EaiHot.SysOLTable == sysAngebot && EaiHot.OLTable == "ANGEBOT"
                                         select EaiHot).ToList();

                    // Return the document
                    foreach (EAIHOT EAIHOTLoop in EaiHots)
                    {
                        Angebot2AntragStateDtoList.Add(MyGetEaiHot(EAIHOTLoop, Entities));
                    }
                    return Angebot2AntragStateDtoList.ToArray();
                }
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the document.", e);
            }
        }*/
        #endregion

        #region My Methods
        private static EAIART MyGetAidaEaiArt(DdOwExtended OwExtendedEntities)
        {
            try
            {
                // Query EAIART
                var CurrentEaiArt = (from EaiArt in OwExtendedEntities.EAIART
                                     where EaiArt.CODE == CnstCode
                                     select EaiArt).FirstOrDefault();

                // Check if nothing was found
                if (CurrentEaiArt == null)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not find " + CnstCode + " in EAIART.");
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

       /* private static Angebot2AntragStateDto MyGetEaiHot(EAIHOT eaiHot, DdOwExtended OwExtendedEntities)
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
                Angebot2AntragStateDto Angebot2AntragStateDto = new Angebot2AntragStateDto();

                // Set the id
                Angebot2AntragStateDto.SysEaiHot = eaiHot.SysEAIHOT;

                // Set the status
                Angebot2AntragStateDto.Status = (EaiHotStatusConstants)eaiHot.Prozessstatus.GetValueOrDefault();

                // Check if status is ready
                if (Angebot2AntragStateDto.Status == EaiHotStatusConstants.Ready)
                {
                    // Query EAIERR
                    var Errors = from EaiError in OwExtendedEntities.EAIERR
                                 where EaiError.SysEAI == eaiHot.SysEAIHOT
                                 select EaiError;

                    // Create the errors list
                    List<EaiErrorDto> ErrorsList = new List<EaiErrorDto>();

                    // Loop through all errors
                    foreach (var LoopError in Errors)
                    {
                        // Get the DTO
                        ErrorsList.Add(MyGetEaiErrorDto(LoopError));
                    }

                    // Assign the errors list
                    Angebot2AntragStateDto.Errors = ErrorsList.ToArray();
                }

                // Return the result
                return Angebot2AntragStateDto;

            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the document.", e);
            }
        }*/



        private static EaiHFileDto MyGetEaiHFileDto(EAIHFILE file)
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
        private static EaiErrorDto MyGetEaiErrorDto(EAIART error)
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
        #endregion

       
    }
}