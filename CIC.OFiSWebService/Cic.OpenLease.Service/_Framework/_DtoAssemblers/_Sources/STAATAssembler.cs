// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class STAATAssembler : IDtoAssembler<STAATDto, STAAT>
    {
        #region IDtoAssembler<STAATDto,STAAT> Members (Methods)
        public bool IsValid(STAATDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public STAAT Create(STAATDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public STAAT Update(STAATDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public STAATDto ConvertToDto(STAAT domain)
        {
            STAATDto STAATDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            STAATDto = new STAATDto();
            MyMap(domain, STAATDto);

            return STAATDto;
        }

        public STAAT ConvertToDomain(STAATDto dto)
        {
            STAAT STAAT;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            STAAT = new STAAT();
            MyMap(dto, STAAT);

            return STAAT;
        }
        #endregion 

        #region IDtoAssembler<STAATDto,STAAT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(STAATDto fromSTAATDto, STAAT toSTAAT)
        {
            // Mapping
            // Ids            
            toSTAAT.SYSSTAAT = fromSTAATDto.SYSSTAAT;            

            // Properties
            toSTAAT.STAAT1 = fromSTAATDto.STAAT1;
        }

        private void MyMap(STAAT fromSTAAT, STAATDto toSTAATDto)
        {
            // Mapping
            // Ids
            toSTAATDto.SYSSTAAT = fromSTAAT.SYSSTAAT;

            // Property
            toSTAATDto.STAAT1 = fromSTAAT.STAAT1;            
        }
        #endregion
    }
}