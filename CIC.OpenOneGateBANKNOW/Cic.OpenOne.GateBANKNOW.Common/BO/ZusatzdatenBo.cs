using System;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Zusatzdaten BO
    /// </summary>
    public class ZusatzdatenBo : AbstractZusatzdatenBo
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="zDao">Zusatzdaten DAO</param>
        public ZusatzdatenBo(IZusatzdatenDao zDao)
            : base(zDao)
        {
        }

        /// <summary>
        /// Zusatzdaten erzeugen oder ändern
        /// </summary>
        /// <param name="zusatzdaten">Zusatzdaten Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override ZusatzdatenDto createOrUpdateZusatzdaten(ZusatzdatenDto zusatzdaten, KundeDto kunde)
        {
            ZusatzdatenDto zusatzOutput = new ZusatzdatenDto();
            zusatzOutput.kdtyp = zusatzdaten.kdtyp;
            if ((zusatzdaten.kdtyp == AngAntBo.KDTYPID_PRIVAT) || (zusatzdaten.kdtyptyp==AngAntBo.KDTYPTYP_PRIVAT))
            {
                PkzDto[] pkzOut = new PkzDto[zusatzdaten.pkz.Length];
                for (int i = 0; i < zusatzdaten.pkz.Length; i++)
                {
                    if (zusatzdaten.pkz[i].syspkz == 0)
                    {
                        pkzOut[i] = createPkz(zusatzdaten.pkz[i], kunde);
                    }
                    else if (zusatzdaten.pkz[i].syspkz > 0)
                    {
                        pkzOut[i] = updatePkz(zusatzdaten.pkz[i], kunde);
                    }
                }
                zusatzOutput.pkz = pkzOut;
            }
            else if (zusatzOutput.kdtyp == AngAntBo.KDTYPID_FIRMA || zusatzdaten.kdtyptyp == AngAntBo.KDTYPTYP_FIRMA)
            {
                UkzDto[] ukzOut = new UkzDto[zusatzdaten.ukz.Length];
                for (int i = 0; i < zusatzdaten.ukz.Length; i++)
                {
                    if (zusatzdaten.ukz[i].sysukz == 0)
                    {
                        ukzOut[i] = createUkz(zusatzdaten.ukz[i], kunde);
                    }
                    else if (zusatzdaten.ukz[i].sysukz > 0)
                    {
                        ukzOut[i] = updateUkz(zusatzdaten.ukz[i], kunde);
                    }
                }
                zusatzOutput.ukz = ukzOut;
                zusatzOutput.kne = null;
                
                if (zusatzdaten.kne!=null&&zusatzdaten.kne.Count>0)
                {
                    zusatzOutput.kne = new System.Collections.Generic.List<KneDto>();
                    foreach (KneDto kne in zusatzdaten.kne)
                    {
                        KneDto updateKne = this.zusatzdatenDao.createOrUpdateKne(kne);
                        if (updateKne != null)
                            zusatzOutput.kne.Add(updateKne);
                    }
                    
                }
                
            }
            else
            {
                throw new Exception(string.Format("Kein gültiger Kundentyp angegeben. Kundentyp muss {0} (Pkz) oder {1} (Ukz) sein.", AngAntBo.KDTYPID_PRIVAT, AngAntBo.KDTYPID_FIRMA));
            }
            return zusatzOutput;
        }

        /// <summary>
        /// Zusatzdaten erzeugen oder ändern
        /// </summary>
        /// <param name="zusatzdaten">Zusatzdaten Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override ZusatzdatenDto createOrUpdateZusatzdatenPerson(ZusatzdatenDto zusatzdaten, KundeDto kunde)
        {
            ZusatzdatenDto zusatzOutput = new ZusatzdatenDto();
            zusatzOutput.kdtyp = zusatzdaten.kdtyp;
            if ((zusatzdaten.kdtyp == AngAntBo.KDTYPID_PRIVAT) || (zusatzdaten.kdtyptyp == AngAntBo.KDTYPTYP_PRIVAT))
            {
                PkzDto[] pkzOut = new PkzDto[zusatzdaten.pkz.Length];
                for (int i = 0; i < zusatzdaten.pkz.Length; i++)
                {
                    if (zusatzdaten.pkz[i].syspkz == 0)
                    {
                        pkzOut[i] = createPkzPerson(zusatzdaten.pkz[i], kunde);
                    }
                    else if (zusatzdaten.pkz[i].syspkz > 0)
                    {
                        pkzOut[i] = updatePkzPerson(zusatzdaten.pkz[i], kunde);
                    }
                }
                zusatzOutput.pkz = pkzOut;
            }
            else if (zusatzOutput.kdtyp == AngAntBo.KDTYPID_FIRMA || zusatzdaten.kdtyptyp == AngAntBo.KDTYPTYP_FIRMA)
            {
                UkzDto[] ukzOut = new UkzDto[zusatzdaten.ukz.Length];
                for (int i = 0; i < zusatzdaten.ukz.Length; i++)
                {
                    if (zusatzdaten.ukz[i].sysukz == 0)
                    {
                        ukzOut[i] = createUkzPerson(zusatzdaten.ukz[i], kunde);
                    }
                    else if (zusatzdaten.ukz[i].sysukz > 0)
                    {
                        ukzOut[i] = updateUkzPerson(zusatzdaten.ukz[i], kunde);
                    }
                }
                zusatzOutput.ukz = ukzOut;
            }
            else
            {
                throw new Exception("Kein gültiger Kundentyp angegeben. Kundentyp muss " + AngAntBo.KDTYPID_PRIVAT + " (Pkz) oder " + AngAntBo.KDTYPID_FIRMA + " (Ukz) sein.");
            }
            return zusatzOutput;
        }

        /// <summary>
        /// PKZ erzeugen
        /// </summary>
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override PkzDto createPkz(PkzDto pkzInput, KundeDto kunde)
        {
            return this.zusatzdatenDao.createPkz(pkzInput, kunde);
        }

        /// <summary>
        /// PKZ erzeugen
        /// </summary>
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override PkzDto createPkzPerson(PkzDto pkzInput, KundeDto kunde)
        {
            return this.zusatzdatenDao.createPkzPerson(pkzInput, kunde);
        }

        /// <summary>
        /// PKZ ändern 
        /// </summary>
        /// <param name="pkzInput">PKZ Eingabe</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override PkzDto updatePkz(PkzDto pkzInput, KundeDto kunde)
        {
            return this.zusatzdatenDao.updatePkz(pkzInput, kunde);
        }

        /// <summary>
        /// PKZ ändern 
        /// </summary>
        /// <param name="pkzInput">PKZ Eingabe</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override PkzDto updatePkzPerson(PkzDto pkzInput, KundeDto kunde)
        {
            return this.zusatzdatenDao.updatePkzPerson(pkzInput, kunde);
        }

        /// <summary>
        /// UKZ erzeugen
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override UkzDto createUkz(UkzDto ukzInput, KundeDto kunde)
        {
            return this.zusatzdatenDao.createUkz(ukzInput, kunde);
        }

        /// <summary>
        /// UKZ ändern
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override UkzDto updateUkz(UkzDto ukzInput, KundeDto kunde)
        {
            return this.zusatzdatenDao.updateUkz(ukzInput, kunde);
        }



        /// <summary>
        /// UKZ erzeugen
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override UkzDto createUkzPerson(UkzDto ukzInput, KundeDto kunde)
        {
            return this.zusatzdatenDao.createUkzPerson(ukzInput, kunde);
        }

        /// <summary>
        /// UKZ ändern
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public override UkzDto updateUkzPerson(UkzDto ukzInput, KundeDto kunde)
        {
            return this.zusatzdatenDao.updateUkzPerson(ukzInput, kunde);
        }

        /// <summary>
        /// Zusatzdaten via ID's holen
        /// </summary>
        /// <param name="sysid">Liste mit Primary Keys</param>
        /// <param name="kdtyp">Kundentyp</param>
        /// <returns></returns>
        public override ZusatzdatenDto getZusatzdaten(long[] sysid, int kdtyp)
        {
            ZusatzdatenDto zusatzOutput = new ZusatzdatenDto();
            zusatzOutput.kdtyp = kdtyp;
            if (kdtyp == AngAntBo.KDTYPID_PRIVAT || zusatzOutput.kdtyptyp == AngAntBo.KDTYPTYP_PRIVAT)
            {
                PkzDto[] pkzOut = new PkzDto[sysid.Length];
                for (int i = 0; i < sysid.Length; i++)
                {
                    pkzOut[i] = zusatzdatenDao.getPkz(sysid[i]);
                }
                zusatzOutput.pkz = pkzOut;
            }
            else if (kdtyp == AngAntBo.KDTYPID_FIRMA || zusatzOutput.kdtyptyp == AngAntBo.KDTYPTYP_FIRMA)
            {
                UkzDto[] ukzOut = new UkzDto[sysid.Length];
                for (int i = 0; i < sysid.Length; i++)
                {
                    ukzOut[i] = zusatzdatenDao.getUkz(sysid[i]);
                }
                zusatzOutput.ukz = ukzOut;
            }
            else
            {
                throw new Exception("Kein gültiger Kundentyp angegeben. Kundentyp muss 1 (Pkz) oder 3 (Ukz) sein.");
            }

            return zusatzOutput;
        }

       /// <summary>
        ///  PKZ/UKZ aus dem letzten Antrag im Zustand 'Vertrag aktiviert' 
       /// </summary>
       /// <param name="sysit"></param>
       /// <param name="kdtyp"></param>
       /// <returns></returns>
        public override ZusatzdatenDto getZusatzdatenAktiv(long sysit, int kdtyp)
        {
            ZusatzdatenDto zusatzOutput = new ZusatzdatenDto();
            
            zusatzOutput.kdtyp = kdtyp;


            if (kdtyp == AngAntBo.KDTYPID_PRIVAT || zusatzOutput.kdtyptyp == AngAntBo.KDTYPTYP_PRIVAT)
            {
                PkzDto pkz = zusatzdatenDao.getITPkzAktiv(sysit);
                if (pkz != null)
                {
                    PkzDto[] pkzOut = new PkzDto[1];
                    pkzOut[0] = pkz;
                    zusatzOutput.pkz = pkzOut;
                }
            }
            else if (zusatzOutput.kdtyp == AngAntBo.KDTYPID_FIRMA || zusatzOutput.kdtyptyp == AngAntBo.KDTYPTYP_FIRMA)
            {
                UkzDto ukz = zusatzdatenDao.getITUkzAktiv(sysit);
                if (ukz != null)
                {
                    UkzDto[] ukzOut = new UkzDto[1];
                    ukzOut[0] = ukz;
                    zusatzOutput.ukz = ukzOut;
                }
            }
            else
            {
                throw new Exception("Kein gültiger Kundentyp angegeben. Kundentyp muss 1 (Pkz) oder 3 (Ukz) sein.");
            }
           return zusatzOutput;
        }
    }
}