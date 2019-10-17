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

    public class BoniPosAssembler : IDtoAssembler<BoniPosDto, BONIPOS>
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
        public bool IsValid(BoniPosDto dto)
        {
            return true;
        }

        public BONIPOS Create(BoniPosDto dto)
        {
            throw new NotSupportedException();
        }

        public BONIPOS Update(BoniPosDto dto)
        {
            throw new NotSupportedException();
        }

        public BoniPosDto ConvertToDto(BONIPOS domain)
        {
            // Map
            return MyMap(domain);
        }

        public BONIPOS ConvertToDomain(BoniPosDto dto)
        {
            // Map
            return MyMap(dto);
        }

        public void Create(BoniPosDto dto, OlExtendedEntities context)
        {
            // Create BONIPOS
            BONIPOS NewBoniPos = MyMap(dto);

            // Add the entity
            context.BONIPOS.Add(NewBoniPos);

            // Save the changes
            context.SaveChanges();
        }
        #endregion

        #region My methods
        private BoniPosDto MyMap(BONIPOS domain)
        {
            // Create the result
            BoniPosDto Result = new BoniPosDto();

            // Map the properties
            Result.AngefAm = domain.ANGEFAM;
            Result.AngefVon = domain.ANGEFVON;
            Result.Beschreibung = domain.BESCHREIBUNG;
            Result.Bezeichnung = domain.BEZEICHNUNG;
            Result.Depot = domain.DEPOT;
            Result.Erham = domain.ERHAM;
            Result.Form = domain.FORM;
            Result.Gewichtung = domain.GEWICHTUNG;
            Result.Lieferant = domain.LIEFERANT;
            Result.Rang = domain.RANG;
            Result.ScoreExt = domain.SCOREEXT;
            Result.ScoreInt = domain.SCOREINT;
            Result.ScoreRel = domain.SCOREREL;
            Result.SysWaehrung = domain.SYSWAEHRUNG;

            // Return the result
            return Result;
        }

        private BONIPOS MyMap(BoniPosDto dto)
        {
            // Create the result
            BONIPOS Result = new BONIPOS();

            // Map the properties
            Result.ANGEFAM = dto.AngefAm;
            Result.ANGEFVON = dto.AngefVon;
            Result.BESCHREIBUNG = dto.Beschreibung;
            Result.BEZEICHNUNG = dto.Bezeichnung;
            Result.DEPOT = dto.Depot;
            Result.ERHAM = dto.Erham;
            Result.FORM = dto.Form;
            Result.GEWICHTUNG = dto.Gewichtung;
            Result.LIEFERANT = dto.Lieferant;
            Result.RANG = dto.Rang;
            Result.SCOREEXT = dto.ScoreExt;
            Result.SCOREINT = dto.ScoreInt;
            Result.SCOREREL = dto.ScoreRel;
            Result.SYSWAEHRUNG = dto.SysWaehrung;

            // Return the result
            return Result;
        }
        #endregion
    }
}