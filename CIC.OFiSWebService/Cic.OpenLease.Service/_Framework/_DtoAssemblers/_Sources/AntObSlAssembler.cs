namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public class AntObSlAssembler : IDtoAssembler<AntObSlDto, ANTOBSL>
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
        public AntObSlAssembler(DdOlExtended context)
        {
            this.context = context;
        }
        #region Methods
        public bool IsValid(AntObSlDto dto)
        {
            return true;
        }

        public ANTOBSL Create(AntObSlDto dto)
        {
            throw new NotImplementedException();
        }

        public ANTOBSL Update(AntObSlDto dto)
        {
            throw new NotImplementedException();
        }

        public AntObSlDto ConvertToDto(ANTOBSL domain)
        {
            return MyMap(domain,context);
        }

        public ANTOBSL ConvertToDomain(AntObSlDto dto)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region My methods
        private AntObSlDto MyMap(ANTOBSL domain, DdOlExtended context)
        {
            AntObSlDto Result = new AntObSlDto();
            Result.Rang = domain.RANG;
            Result.Bezeichnung = domain.BEZEICHNUNG;
            if (!context.Entry(domain).Collection(f => f.ANTOBSLPOSList).IsLoaded)
                context.Entry(domain).Collection(f => f.ANTOBSLPOSList).Load();
           

            ANTOBSLPOS AntObSlPos = domain.ANTOBSLPOSList.FirstOrDefault();

            if (AntObSlPos == null)
            {
                Result.Betrag = 0;
            }
            else
            {
                Result.Betrag = AntObSlPos.BETRAG.GetValueOrDefault();
            }

            return Result;
        }
        #endregion
    }
}