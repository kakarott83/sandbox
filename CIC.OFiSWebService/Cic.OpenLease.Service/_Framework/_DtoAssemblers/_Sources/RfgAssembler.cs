namespace Cic.OpenLease.Service
{
    #region Using
    #endregion

    // [System.CLSCompliant(true)]
    /* public class RfgAssembler : IDtoAssembler<RfgDto, RFG>
     {
         #region IDtoAssembler<LANDDto,LAND> Members (Methods)
         public bool IsValid(RfgDto dto)
         {
             // NOTE JJ, Not necessary
             throw new NotImplementedException();
         }

         public RFG Create(RfgDto dto)
         {
             // NOTE JJ, Not necessary
             throw new NotImplementedException();
         }

         public RFG Update(RfgDto dto)
         {
             // NOTE JJ, Not necessary
             throw new NotImplementedException();
         }

         public RfgDto ConvertToDto(RFG domain)
         {
             RfgDto RfgDto;

             if (domain == null)
             {
                 throw new ArgumentException("domain");
             }

             RfgDto = new RfgDto();
             MyMap(domain, RfgDto);

             return RfgDto;
         }

         public RFG ConvertToDomain(RfgDto dto)
         {
             RFG RFG;

             if (dto == null)
             {
                 throw new ArgumentException("dto");
             }

             RFG = new RFG();
             MyMap(dto, RFG);

             return RFG;
         }
         #endregion

         #region IDtoAssembler<RfgDto,RFG> Members (Properties)
         public System.Collections.Generic.Dictionary<string, string> Errors
         {
             // NOTE JJ, Not necessary
             get { throw new NotImplementedException(); }
         }
         #endregion

         #region My methods
         private void MyMap(RfgDto fromRfgDto, RFG toRFG)
         {
             toRFG.AUSFUEHREN = fromRfgDto.AUSFUEHREN;
             toRFG.BEARBEITEN = fromRfgDto.BEARBEITEN;
             toRFG.BEIAUFRUF = fromRfgDto.BEIAUFRUF;
             toRFG.EINFUEGEN = fromRfgDto.EINFUEGEN;
             toRFG.LOESCHEN = fromRfgDto.LOESCHEN;
             toRFG.SEHEN = fromRfgDto.SEHEN;
         }

         private void MyMap(RFG fromRFG, RfgDto toRfgDto)
         {
             toRfgDto.AUSFUEHREN = fromRFG.AUSFUEHREN;
             toRfgDto.BEARBEITEN = fromRFG.BEARBEITEN;
             toRfgDto.BEIAUFRUF = fromRFG.BEIAUFRUF;
             toRfgDto.EINFUEGEN = fromRFG.EINFUEGEN;
             toRfgDto.LOESCHEN = fromRFG.LOESCHEN;
             toRfgDto.SEHEN = fromRFG.SEHEN;
         }
         #endregion
     }*/
}