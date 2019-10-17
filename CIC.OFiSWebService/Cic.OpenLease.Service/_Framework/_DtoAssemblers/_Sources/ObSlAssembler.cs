namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    #endregion

    public class ObSlAssembler : IDtoAssembler<ObSlDto, OB>
    {
        #region Properties
        public Dictionary<string, string> Errors
        {
            get
            {
                return null;
            }
        }
        #endregion

        #region Methods
        public bool IsValid(ObSlDto dto)
        {
            return true;
        }

        public OB Create(ObSlDto dto)
        {
            throw new NotImplementedException();
        }

        public OB Update(ObSlDto dto)
        {
            throw new NotImplementedException();
        }

        public ObSlDto ConvertToDto(OB domain)
        {
            return MyMap(domain);
        }

        public OB ConvertToDomain(ObSlDto dto)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region My methods
        private ObSlDto MyMap(OB domain)
        {
            ObSlDto Result = new ObSlDto();
            Result.Rang = domain.RANG;
            Result.Bezeichnung = domain.BEZEICHNUNG;
            Result.Betrag = 0;

            return Result;
        }
        #endregion
    }
}