// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class ANGOBSICHAssembler : IDtoAssembler<MitantragstellerDto, ANGOBSICH>
    {
        #region IDtoAssembler<MitAntragStellerDto, ANGOBSL> Members (Methods)
        public bool IsValid(MitantragstellerDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ANGOBSICH Create(MitantragstellerDto dto)
        {
            ANGOBSICH NewANGOBSICH;
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            using (DdOlExtended Context = new DdOlExtended())
            {
                NewANGOBSICH = new ANGOBSICH();

                // Ticket#2012121910000071 — ANGEBOT Mitantragssteller ist weg
                // Die Verknüpfung zwischen angobsich und angebot lautet in AIDA: angobsich.sysvt == angebot.sysid.
                NewANGOBSICH.SYSVT = dto.SYSVT;
                NewANGOBSICH.SYSANGEBOT = dto.SYSVT;

                NewANGOBSICH.SICHTYP = SICHTYPHelper.GetSichTyp(Context, dto.SICHTYPRANG);
                
                //PERFORMANCE-FIX:
                NewANGOBSICH.SYSIT=dto.SYSIT;

                // Map
                MyMap(dto, NewANGOBSICH);

                //TRANSACTIONS
                //using (System.Transactions.TransactionScope TransactionScope = new System.Transactions.TransactionScope())
                {
                    // Insert (with Save changes)
                    Context.ANGOBSICH.Add(NewANGOBSICH);

                    // Save changes
                    Context.SaveChanges();

                    // Set transaction complete
                    //TransactionScope.Complete();
                }
            }
            return NewANGOBSICH;
        }

        public ANGOBSICH Update(MitantragstellerDto dto)
        {
            ANGOBSICH OriginalANGOBSICH;
            ANGOBSICH ModifiedANGOBSICH;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            using (DdOlExtended Context = new DdOlExtended())
            {
                OriginalANGOBSICH = ANGOBSICHHelper.GetAngobsischById(Context, dto.SysId);
                
                //PERFORMANCE-FIX:
                OriginalANGOBSICH.SYSIT=dto.SYSIT;

                // Ticket#2012121910000071 — ANGEBOT Mitantragssteller ist weg
                // Die Verknüpfung zwischen angobsich und angebot lautet in AIDA: angobsich.sysvt == angebot.sysid.
                OriginalANGOBSICH.SYSVT = dto.SYSVT;
                OriginalANGOBSICH.SYSANGEBOT = dto.SYSVT;
                
                OriginalANGOBSICH.SICHTYP = SICHTYPHelper.GetSichTyp(Context, dto.SICHTYPRANG);

                // Map
                MyMap(dto, OriginalANGOBSICH);

                // Update (with Save changes)
                //ModifiedANGOBSICH = Context.Update<ANGOBSICH>(OriginalANGOBSICH, null);
                Context.SaveChanges();
                ModifiedANGOBSICH = OriginalANGOBSICH;
            }

            return ModifiedANGOBSICH;
        }

        public MitantragstellerDto ConvertToDto(ANGOBSICH domain)
        {
            MitantragstellerDto MitAntragStellerDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            MitAntragStellerDto = new MitantragstellerDto();
            MyMap(domain, MitAntragStellerDto);

            return MitAntragStellerDto;
        }

        public ANGOBSICH ConvertToDomain(MitantragstellerDto dto)
        {
            ANGOBSICH ANGOBSICH;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            ANGOBSICH = new ANGOBSICH();
            MyMap(dto, ANGOBSICH);

            return ANGOBSICH;
        }
        #endregion

        #region IDtoAssembler<MitAntragStellerDto,ANGOBSICH> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(MitantragstellerDto fromMitAntragStellerDto, ANGOBSICH toANGOBSICH)
        {
            toANGOBSICH.BEZEICHNUNG = fromMitAntragStellerDto.BEZEICHNUNG;
            toANGOBSICH.BEGINN = fromMitAntragStellerDto.BEGIN;
            toANGOBSICH.ENDE = fromMitAntragStellerDto.END;
            toANGOBSICH.AKTIVFLAG = fromMitAntragStellerDto.AKTIVZ;
            toANGOBSICH.RANG = fromMitAntragStellerDto.RANG;
            if (fromMitAntragStellerDto.OPTION1 != null && fromMitAntragStellerDto.OPTION1.Length>0)
                toANGOBSICH.OPTION1 = fromMitAntragStellerDto.OPTION1;
        }

        private void MyMap(ANGOBSICH fromANGOBSICH, MitantragstellerDto toMitAntragStellerDto)
        {
            toMitAntragStellerDto.SysId = fromANGOBSICH.SYSID;
            toMitAntragStellerDto.BEZEICHNUNG = fromANGOBSICH.BEZEICHNUNG;

            // Ticket#2012121910000071 — ANGEBOT Mitantragssteller ist weg
            // Die Verknüpfung zwischen angobsich und angebot lautet in AIDA: angobsich.sysvt == angebot.sysid.
            toMitAntragStellerDto.SYSVT = fromANGOBSICH.SYSVT.HasValue ? (long)fromANGOBSICH.SYSVT : 0;

            if (fromANGOBSICH.SICHTYP != null)
            {
                toMitAntragStellerDto.SYSSICHTYP = fromANGOBSICH.SICHTYP.SYSSICHTYP;
                toMitAntragStellerDto.SICHTYPRANG = fromANGOBSICH.SICHTYP.RANG.Value;
            }

            toMitAntragStellerDto.BEGIN = fromANGOBSICH.BEGINN;
            toMitAntragStellerDto.END = fromANGOBSICH.ENDE;
            toMitAntragStellerDto.OPTION1 = fromANGOBSICH.OPTION1;

            if (fromANGOBSICH.IT != null)
            {
                toMitAntragStellerDto.SYSIT = fromANGOBSICH.IT.SYSIT;
            }
            toMitAntragStellerDto.AKTIVZ = fromANGOBSICH.AKTIVFLAG;

            toMitAntragStellerDto.RANG = fromANGOBSICH.RANG.HasValue ? (int)fromANGOBSICH.RANG : 0;

        }
        #endregion
    }
}
