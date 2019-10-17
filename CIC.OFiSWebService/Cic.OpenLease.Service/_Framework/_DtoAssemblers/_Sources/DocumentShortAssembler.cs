namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using System.Collections.Generic;
    using Cic.OpenLease.ServiceAccess.Merge.OlClient;
    using Cic.OpenLease.ServiceAccess;
    using CIC.Database.OIQUEUE.EF6.Model;
    using Cic.OpenOne.Common.Model.DdOl;
    #endregion

    public class DocumentShortAssembler : IDtoAssembler<DocumentShortDto, CFGVAR>
    {
        #region Properties
        public Dictionary<string, string> Errors
        {
            get
            {
                return null;
            }
        }
        private DdOlExtended context;
        #endregion
        public DocumentShortAssembler(DdOlExtended context)
        {
            this.context=context;
        }
        #region IDtoAssembler methods
        public bool IsValid(DocumentShortDto dto)
        {
            return true;
        }

        public CFGVAR Create(DocumentShortDto dto)
        {
            throw new NotSupportedException();
        }

        public CFGVAR Update(DocumentShortDto dto)
        {
            throw new NotSupportedException();
        }

        public DocumentShortDto ConvertToDto(CFGVAR domain)
        {
            return MyMap(domain,context);
        }

        public CFGVAR ConvertToDomain(DocumentShortDto dto)
        {
            return MyMap(dto);
        }
        #endregion

        #region My methods
        private CFGVAR MyMap(DocumentShortDto dto)
        {
            throw new NotSupportedException();
        }

        private DocumentShortDto MyMap(CFGVAR domain, DdOlExtended context)
        {
            // Create the result
            DocumentShortDto Result = new DocumentShortDto();

            if (domain.CFGSEC == null)
                context.Entry(domain).Reference(f => f.CFGSEC).Load();
           

            // Map the properties
            Result.Name = domain.CODE;
            if(domain.CFGSEC!=null)
                Result.Area = (AreaConstants)Enum.Parse(typeof(AreaConstants), domain.CFGSEC.CODE, true);
            Result.Description = domain.BEZEICHNUNG;
            if(Result.Description!=null)
                Result.Description = Result.Description.Replace(':', ' ');

            // Return the result
            return Result;
        }
        #endregion
    }
}