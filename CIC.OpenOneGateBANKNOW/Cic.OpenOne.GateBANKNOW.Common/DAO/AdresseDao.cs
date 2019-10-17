using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// AdresseDao erstellt und holt neue und vorhandene AdresseDtos
    /// </summary>
    public class AdresseDao : IAdresseDao
    {


        const String GETPLZBYPLZ = "select plz,bezirk,ort,l.countryname land,l.sysland from PLZ,land l where l.sysland=plz.sysland and PLZ = :postleitzahl";
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// createAdresse erstellt einen neuen Datensatz
        /// </summary>
        /// <param name="adresseInput">AdresseDto mit einer SYSADRESSE = 0</param>
        /// <returns>AdresseDto mit einer SYSADRESSE > 0</returns>
        public AdresseDto createAdresse(AdresseDto adresseInput)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                // ITADRESSE
                //IT it = context.IT.Where(par => par.SYSIT == adresseInput.sysperson).FirstOrDefault();
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                ITADRESSE adresseOutput = context.ITADRESSE.Include("IT").Where(par => par.IT.SYSIT == adresseInput.sysperson && par.RANG==2).FirstOrDefault();
                _log.Debug("createAdresse1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                if (adresseOutput == null)
                {
                    adresseOutput = new ITADRESSE();
                    context.ITADRESSE.Add(adresseOutput);
                }
               

                Mapper.Map<AdresseDto, ITADRESSE>(adresseInput, adresseOutput);
                adresseOutput.RANG = 2;
                adresseOutput.SYSIT=adresseInput.sysperson;
                context.SaveChanges();
                _log.Debug("createAdresse2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));

                return getAdresse(adresseOutput.SYSITADRESSE);
            }
        }

        /// <summary>
        /// updateAdresse holt einen vorhandenen Datensatz
        /// </summary>
        /// <param name="adresseInput">AdresseDto mit einer SYSADRESSE > 0</param>
        /// <returns>AdresseDto mit der gleichen SYSADRESSE</returns>
        public AdresseDto updateAdresse(AdresseDto adresseInput)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                ITADRESSE adresseOutput = (from adresse in context.ITADRESSE
                                         where adresse.SYSITADRESSE == adresseInput.sysadresse
                                         select adresse).FirstOrDefault();
                _log.Debug("updateAdresse1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                if (adresseOutput != null)
                {
                    Mapper.Map<AdresseDto, ITADRESSE>(adresseInput, adresseOutput);
                    adresseOutput.RANG = 2;
                    adresseOutput.SYSIT=adresseInput.sysperson;
                    context.SaveChanges();
                    _log.Debug("updateAdresse2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                }
                return getAdresse(adresseOutput.SYSITADRESSE);
            }
        }

        /// <summary>
        /// removes the address for the person
        /// </summary>
        /// <param name="sysperson"></param>
        public void deleteAdresse(long sysperson)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                // ITADRESSE
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                ITADRESSE adresseOutput = context.ITADRESSE.Include("IT").Where(par => par.IT.SYSIT == sysperson && par.RANG == 2).FirstOrDefault();
                _log.Debug("deleteAdresse1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                if (adresseOutput != null)
                {
                    context.DeleteObject(adresseOutput);
                }
                context.SaveChanges();
                _log.Debug("deleteAdresse2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            }
        }

        /// <summary>
        /// getAdresse holt einen vorhandenen Datensatz
        /// </summary>
        /// <param name="sysid">SYSADRESSE</param>
        /// <returns>AdresseDto mit der selben SYSADRESSE</returns>
        public AdresseDto getAdresse(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                AdresseDto rval = null;
                ITADRESSE adresseOutput = (from adresse in context.ITADRESSE
                                  where adresse.SYSITADRESSE == sysid
                                  select adresse).FirstOrDefault();
                if (adresseOutput != null)
                {
                    
                    rval = Mapper.Map<ITADRESSE, AdresseDto>(adresseOutput);
                    rval.sysadresse = adresseOutput.SYSITADRESSE;
                    
                }
                return rval;
            }            
        }

        /// <summary>
        /// Holt den Datensatz zu Postleitzahl
        /// </summary>
        /// <param name="plz">Postleitzahl</param>
        /// <returns>PlzDtoArray</returns>
        public PlzDto[] getPlz(string plz)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "postleitzahl", Value = plz });

                List<PlzDto>  rval = context.ExecuteStoreQuery<PlzDto>(GETPLZBYPLZ, parameters.ToArray()).ToList();
                return rval.ToArray();
            }
        }
    }
}
