// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using Cic.OpenLease.Model.DdOl;
    using Cic.OpenLease.ServiceAccess.DdOl;
    #endregion

    [System.CLSCompliant(true)]
    public class ANGOBSLAssembler : IDtoAssembler<MitAntragStellerDto, ANGOBSL>
    {
        #region IDtoAssembler<MitAntragStellerDto, ANGOBSL> Members (Methods)
        public bool IsValid(MitAntragStellerDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ANGOBSL Create(MitAntragStellerDto dto)
        {
            ANGOBSL NewANGOBSL;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            NewANGOBSL = new ANGOBSL();

            // Map
            MyMap(dto, NewANGOBSL);

            using (OlExtendedEntities Context = new OlExtendedEntities())
            {
                // Transaction
                using (System.Transactions.TransactionScope TransactionScope = new System.Transactions.TransactionScope())
                {
                    // Insert (with Save changes)
                    Context.ANGOBSLExtension.Insert(NewANGOBSL);

                    // Save changes
                    Context.SaveChanges();

                    // Set transaction complete
                    TransactionScope.Complete();
                }
            }

            return NewANGOBSL;
        }

        public ANGOBSL Update(MitAntragStellerDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public MitAntragStellerDto ConvertToDto(ANGOBSL domain)
        {
            MitAntragStellerDto MitAntragStellerDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            MitAntragStellerDto = new MitAntragStellerDto();
            MyMap(domain, MitAntragStellerDto);

            return MitAntragStellerDto;
        }

        public ANGOBSL ConvertToDomain(MitAntragStellerDto dto)
        {
            ANGOBSL ANGOBSL;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            ANGOBSL = new ANGOBSL();
            MyMap(dto, ANGOBSL);

            return ANGOBSL;
        }
        #endregion

        #region IDtoAssembler<MitAntragStellerDto,ANGOBSL> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(MitAntragStellerDto fromMitAntragStellerDto, ANGOBSL toANGOBSL)
        {
            toANGOBSL.SYSVT = fromMitAntragStellerDto.SYSVT;
            toANGOBSL.BEZEICHNUNG = fromMitAntragStellerDto.BEZEICHNUNG;
            toANGOBSL.SYSSLTYP = fromMitAntragStellerDto.SYSSICHTYP;
        }

        private void MyMap(ANGOBSL fromANGOBSL, MitAntragStellerDto toMitAntragStellerDto)
        {
            toMitAntragStellerDto.BEZEICHNUNG = fromANGOBSL.BEZEICHNUNG;
            toMitAntragStellerDto.SYSVT = fromANGOBSL.SYSVT;
            toMitAntragStellerDto.SYSSICHTYP = fromANGOBSL.SYSSLTYP;

        }
        #endregion
    }
}