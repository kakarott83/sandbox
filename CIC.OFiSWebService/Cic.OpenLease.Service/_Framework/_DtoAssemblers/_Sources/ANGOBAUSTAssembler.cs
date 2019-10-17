// OWNER WB, 04.05.2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class ANGOBAUSTAssembler : IDtoAssembler<ANGOBAUSDto, ANGOBAUST>
    {
        #region Private variables
        private System.Collections.Generic.Dictionary<string, string> _Errors;
        private long? _SysPUSER;
        #endregion

        #region Constructors
        public ANGOBAUSTAssembler(long? sysPUSER)
        {
            _SysPUSER = sysPUSER;
            _Errors = new System.Collections.Generic.Dictionary<string, string>();
        }
        #endregion

        #region IDtoAssembler<ANGOBAUSTDto, ANGOBAUST> Members (Methods)
        public bool IsValid(ANGOBAUSDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            // Otymistic
            bool IsValid = true;

            // DdOw
            using (DdOlExtended Context = new DdOlExtended())
            {
                // Check if _SysPERSONInPEROLE is not null
                if (IsValid && !_SysPUSER.HasValue)
                {
                    _Errors.Add("SYSIT", "Not exists in sight field. SysPERSONInPEROLE is null.");
                    IsValid = false;
                }
            }

            return IsValid;
        }

        public ANGOBAUST Create(ANGOBAUSDto dto)
        {
            ANGOBAUST NewANGOBAUST;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            NewANGOBAUST = new ANGOBAUST();

            // Map
            MyMap(dto, NewANGOBAUST);


            using (DdOlExtended Context = new DdOlExtended())
            {
                //TRANSACTIONS
               // using (System.Transactions.TransactionScope TransactionScope = new System.Transactions.TransactionScope())
                {

                    Context.ANGOBAUST.Add(NewANGOBAUST);

                    // Save changes
                    Context.SaveChanges();

                    // Set transaction complete
                    //TransactionScope.Complete();
                }
            }

            return NewANGOBAUST;
        }

        public ANGOBAUST Update(ANGOBAUSDto dto)
        {
            ANGOBAUST OriginalANGOBAUST;
            ANGOBAUST ModifiedANGOBAUST;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            using (DdOlExtended Context = new DdOlExtended())
            {
                var QueryANGOBAUST = from angobaust in Context.ANGOBAUST
                                     where angobaust.SNR == dto.SNR
                                     select angobaust;
                OriginalANGOBAUST = QueryANGOBAUST.FirstOrDefault<ANGOBAUST>();
                
                // Map
                MyMap(dto, OriginalANGOBAUST);

                // Update (with Save changes)
                //ModifiedANGOBAUST = Context.Update < ANGOBAUST>(OriginalANGOBAUST, null);
                Context.SaveChanges();
                ModifiedANGOBAUST = OriginalANGOBAUST;
            }

            return ModifiedANGOBAUST;

        }


        public ANGOBAUSDto ConvertToDto(ANGOBAUST domain)
        {
            ANGOBAUSDto ANGOBAUSDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            ANGOBAUSDto = new ANGOBAUSDto();
            MyMap(domain, ANGOBAUSDto);

            return ANGOBAUSDto;
        }

        public ANGOBAUST ConvertToDomain(ANGOBAUSDto dto)
        {
            ANGOBAUST ANGOBAUST;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            ANGOBAUST = new ANGOBAUST();
            MyMap(dto, ANGOBAUST);

            return ANGOBAUST;
        }
        #endregion

        #region IDtoAssembler<ITDto,IT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get
            {
                return _Errors;
            }
        }
        #endregion

        #region My methods
        private void MyMap(ANGOBAUSDto fromANGOBAUSDto, ANGOBAUST toANGOBAUST)
        {
            // Mapping
            toANGOBAUST.BESCHREIBUNG = fromANGOBAUSDto.BESCHREIBUNG;
            toANGOBAUST.BETRAG = fromANGOBAUSDto.BETRAG;
            toANGOBAUST.BETRAG2 = fromANGOBAUSDto.BETRAG2;
            toANGOBAUST.FLAGPACKET = fromANGOBAUSDto.FLAGPACKET;
            toANGOBAUST.FLAGRWREL = fromANGOBAUSDto.FLAGRWREL;
            toANGOBAUST.SNR = fromANGOBAUSDto.SNR;
            toANGOBAUST.FREITEXT = fromANGOBAUSDto.FREITEXT;
        }

        private void MyMap(ANGOBAUST fromANGOBAUST, ANGOBAUSDto toANGOBAUSTDto)
        {
            // Mapping
            toANGOBAUSTDto.BESCHREIBUNG = fromANGOBAUST.BESCHREIBUNG;
            toANGOBAUSTDto.BETRAG = fromANGOBAUST.BETRAG;
            toANGOBAUSTDto.BETRAG2 = fromANGOBAUST.BETRAG2;
            toANGOBAUSTDto.FLAGPACKET = fromANGOBAUST.FLAGPACKET;
            toANGOBAUSTDto.FLAGRWREL = fromANGOBAUST.FLAGRWREL;
            toANGOBAUSTDto.SNR = fromANGOBAUST.SNR;
            toANGOBAUSTDto.FREITEXT = fromANGOBAUST.FREITEXT;
        }

        

        #endregion
    }
}