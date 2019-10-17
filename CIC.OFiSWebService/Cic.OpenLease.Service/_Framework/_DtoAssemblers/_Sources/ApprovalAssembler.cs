namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenLease.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    using Cic.OpenOne.Common.Model.DdOl;
    #endregion

    public static class ApprovalAssembler
    {
        #region Methods
        public static ApprovalDto ConvertToDto(APPROVAL domain)
        {
            return MyMap(domain);
        }

        public static APPROVAL ConvertToDomain(ApprovalDto dto)
        {
            return MyMap(dto);
        }

        public static void Create(ApprovalDto dto, DdOlExtended context)
        {
            // Map the dto
            APPROVAL NewApproval = MyMap(dto);

            // Add the entity
            context.APPROVAL.Add(NewApproval);

            // Save the changes
            context.SaveChanges();
        }
        #endregion

        #region My methods
        private static APPROVAL MyMap(ApprovalDto dto)
        {
            // Create a result
            APPROVAL ResultDomain = new APPROVAL();

            // Set the properties
            ResultDomain.AUFLAGENDATUM = dto.AuflagenDatum;
            ResultDomain.AUFLAGENKOMP = dto.AuflagenKomp;
            ResultDomain.AUFLAGENREM = dto.AuflagenRem;
            ResultDomain.GENEHMIGUNG = dto.Genehmigung;
            ResultDomain.GENEHMREM = dto.GenehmRem;
            ResultDomain.KALKULATION = dto.Kalkulation;
            ResultDomain.SICHERHEITEN = dto.SicherHeiten;
            ResultDomain.VOTUM = dto.Votum;
            ResultDomain.VOTUMDATUM = dto.VotumDatum;
            ResultDomain.VOTUMKOMP = dto.VotumKomp;
            ResultDomain.VOTUMREM = dto.VotumRem;

            // Resturn the result
            return ResultDomain;
        }

        private static ApprovalDto MyMap(APPROVAL domain)
        {
            // Create a result
            ApprovalDto ResultDto = new ApprovalDto();

            // Set the properties
            ResultDto.AuflagenDatum = domain.AUFLAGENDATUM;
            ResultDto.AuflagenKomp = domain.AUFLAGENKOMP;
            ResultDto.AuflagenRem = domain.AUFLAGENREM;
            ResultDto.Genehmigung = domain.GENEHMIGUNG;
            ResultDto.GenehmRem = domain.GENEHMREM;
            ResultDto.Kalkulation = domain.KALKULATION;
            ResultDto.SicherHeiten = domain.SICHERHEITEN;
            ResultDto.Votum = domain.VOTUM;
            ResultDto.VotumDatum = domain.VOTUMDATUM;
            ResultDto.VotumKomp = domain.VOTUMKOMP;
            ResultDto.VotumRem = domain.VOTUMREM;

            // Resturn the result
            return ResultDto;
        }
        #endregion
    }
}