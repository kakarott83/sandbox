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
    #endregion

    public class BoniTaetAssembler : IDtoAssembler<BonitaetDto, BONITAET>
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

        #region IDtoAssembler methods
        public bool IsValid(BonitaetDto dto)
        {
            return true;
        }

        public BONITAET Create(BonitaetDto dto)
        {
            throw new NotSupportedException();
        }

        public BONITAET Update(BonitaetDto dto)
        {
            throw new NotSupportedException();
        }

        public BonitaetDto ConvertToDto(BONITAET domain)
        {
            return MyMap(domain);
        }

        public BONITAET ConvertToDomain(BonitaetDto dto)
        {
            return MyMap(dto);
        }

        public void Create(BonitaetDto dto, OlExtendedEntities context)
        {
            // Create BONITAET
            BONITAET NewBoniTaet = MyMap(dto);

            // Add the entity to the context
            context.BONITAET.Add(NewBoniTaet);

            // Save the changes
            context.SaveChanges();
        }
        #endregion

        #region My methods
        private BonitaetDto MyMap(BONITAET domain)
        {
            // Create the result
            BonitaetDto Result = new BonitaetDto();

            // Map the properties
            Result.BankScore = domain.BANKSCORE;
            Result.Bemerkung = domain.BEMERKUNG;
            Result.BilanzScore = domain.BILANZSCORE;
            Result.BwaScore = domain.BWASCORE;
            Result.Erstelltam = domain.ERSTELLTAM;
            Result.GesamtScore = domain.GESAMTSCORE;
            Result.HandelsScore = domain.HANDELSSCORE;
            Result.KreditScore = domain.KREDITSCORE;
            Result.ObjektScore = domain.OBJEKTSCORE;
            Result.Rang = domain.RANG;
            Result.RatingScore = domain.RATINGSCORE;
            Result.SbeArbeiter = domain.SBEARBEITER;
            Result.SchufaScore = domain.SCHUFASCORE;
            Result.ZahlScore = domain.ZAHLSCORE;

            // Ceate BoniPos list
            List<BoniPosDto> BoniPosList = new List<BoniPosDto>();

            BoniPosAssembler Assembler = new BoniPosAssembler();

            // Iterate through all BoniPos
            foreach (var LoopBoniPos in domain.BONIPOSList)
            {
                // Add mapped BoniPos to the list
                BoniPosList.Add(Assembler.ConvertToDto(LoopBoniPos));
            }

            // Assign the BoniPos list
            Result.BoniPos = BoniPosList.ToArray();

            // Return the result
            return Result;
        }

        private BONITAET MyMap(BonitaetDto dto)
        {
            // Create the result
            BONITAET Result = new BONITAET();

            // Map the properties
            Result.BANKSCORE = dto.BankScore;
            Result.BEMERKUNG = dto.Bemerkung;
            Result.BILANZSCORE = dto.BilanzScore;
            Result.BWASCORE = dto.BwaScore;
            Result.ERSTELLTAM = dto.Erstelltam;
            Result.GESAMTSCORE = dto.GesamtScore;
            Result.HANDELSSCORE = dto.HandelsScore;
            Result.KREDITSCORE = dto.KreditScore;
            Result.OBJEKTSCORE = dto.ObjektScore;
            Result.RANG = dto.Rang;
            Result.RATINGSCORE = dto.RatingScore;
            Result.SBEARBEITER = dto.SbeArbeiter;
            Result.SCHUFASCORE = dto.SchufaScore;
            Result.ZAHLSCORE = dto.ZahlScore;

            BoniPosAssembler Assembler = new BoniPosAssembler();

            // Loop through all BoniPos
            foreach (var LoopBoniPos in dto.BoniPos)
            {
                // Add the mapped BoniPos
                Result.BONIPOSList.Add(Assembler.ConvertToDomain(LoopBoniPos));
            }

            // Return the result
            return Result;
        }
        #endregion
    }
}