using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Util;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// KontoDao erstellt und holt neue und vorhandene KontoDtos
    /// </summary>
    public class KontoDao : IKontoDao
    {
        /// <summary>
        /// createKonto erstellt ein neuen Datensatz für ein Konto
        /// </summary>
        /// <param name="kontoInput">KontoDto mit SYSKONTO = 0</param>
        /// <returns>KontDto mit SYSKONTO > 0</returns>
        public KontoDto createKonto(KontoDto kontoInput)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ITKONTO kontoOutput = new ITKONTO();
                context.ITKONTO.Add(kontoOutput);
                //DODO XXX REFACTOR: change to EntityKey for faster assignment
                kontoOutput.IT = context.IT.Where(par => par.SYSIT == kontoInput.sysperson).FirstOrDefault();
                kontoOutput.IBAN = kontoInput.iban;
                kontoOutput.KONTONR = kontoInput.kontonr;
                kontoOutput.RANG = kontoInput.rang;
                kontoOutput.SYSBLZ = kontoInput.sysblz;
                context.SaveChanges();

                ITKONTOREF ktoRef = new ITKONTOREF();
                context.ITKONTOREF.Add(ktoRef);
                ktoRef.ITKONTO = kontoOutput;
                ktoRef.IT = kontoOutput.IT;
                ktoRef.ACTIVEFLAG = 1;
                if (kontoInput.sysantrag > 0)
                    ktoRef.SYSANTRAG= kontoInput.sysantrag;
                ktoRef.SYSBKONTOTP= 6;
                ktoRef.SYSKONTOTP=2;

                context.SaveChanges();

                return getKonto(kontoOutput.SYSITKONTO);
            }
        }

        /// <summary>
        /// updateKonto holt einen vorhandenen Datensatz
        /// </summary>
        /// <param name="kontoInput">KontoDto mit SYSKONTO > 0</param>
        /// <returns>KontoDto mit der selben SYSKONTO</returns>
        public KontoDto updateKonto(KontoDto kontoInput)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ITKONTO kontoOutput = (from itkonto in context.ITKONTO
                                       where itkonto.SYSITKONTO == kontoInput.syskonto
                                       select itkonto).FirstOrDefault();

                if (kontoOutput != null)//schon vorhanden
                {
                    if (kontoOutput.IT == null)
                        context.Entry(kontoOutput).Reference(f => f.IT).Load();
                    
                    if (kontoOutput.IT != null)//konto schon mit einem Kunden verknüpft
                    {
                        ITKONTOREF ktoRef = context.ITKONTOREF.Where(par => par.ITKONTO.SYSITKONTO == kontoOutput.SYSITKONTO).FirstOrDefault();
                        if (kontoOutput.IT.SYSIT != kontoInput.sysperson)
                        {
                            //DODO XXX REFACTOR: change to EntityKey for faster assignment
                            kontoOutput.IT = context.IT.Where(par => par.SYSIT == kontoInput.sysperson).FirstOrDefault();
                            ktoRef.ITKONTO = kontoOutput;
                            ktoRef.IT = kontoOutput.IT;
                            ktoRef.ACTIVEFLAG = 1;
                        }

                        
                        if (kontoInput.sysantrag > 0)
                            ktoRef.SYSANTRAG = kontoInput.sysantrag;
                        ktoRef.SYSBKONTOTP = 6;
                        ktoRef.SYSKONTOTP = 2;
                    }
                    else //konto noch nicht mit IT verknüpft
                    {
                        ITKONTOREF ktoRef = context.ITKONTOREF.Where(par => par.ITKONTO.SYSITKONTO == kontoOutput.SYSITKONTO).FirstOrDefault();
                        if (ktoRef != null)
                        {
                            //DODO XXX REFACTOR: change to EntityKey for faster assignment
                            kontoOutput.IT = context.IT.Where(par => par.SYSIT == kontoInput.sysperson).FirstOrDefault();
                            ktoRef.ITKONTO = kontoOutput;
                            ktoRef.IT = kontoOutput.IT;
                        }
                        else
                        {
                            ktoRef = new ITKONTOREF();
                            context.ITKONTOREF.Add(ktoRef);
                            kontoOutput.IT = context.IT.Where(par => par.SYSIT == kontoInput.sysperson).FirstOrDefault();
                            ktoRef.ITKONTO = kontoOutput;
                            ktoRef.IT = kontoOutput.IT;
                            ktoRef.ACTIVEFLAG = 1;
                        }
                        

                        if (kontoInput.sysantrag > 0)
                            ktoRef.SYSANTRAG = kontoInput.sysantrag;
                        ktoRef.SYSBKONTOTP = 6;
                        ktoRef.SYSKONTOTP = 2;
                    }
                    kontoOutput.IBAN = kontoInput.iban;
                    kontoOutput.KONTONR = kontoInput.kontonr;
                    kontoOutput.RANG = kontoInput.rang;
                    kontoOutput.SYSBLZ = kontoInput.sysblz;
                    kontoOutput.SYSITKONTO = kontoInput.syskonto;

                    context.SaveChanges();
                }
                return getKonto(kontoOutput.SYSITKONTO);
            }
        }

        /// <summary>
        /// getKonto holt einen vorhandenen Datensatz
        /// </summary>
        /// <param name="sysid">SYSKONTO</param>
        /// <returns>KontoDto mir selber SYSKONTO</returns>
        public KontoDto getKonto(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ITKONTO kontoOutput = (from konto in context.ITKONTO
                                       where konto.SYSITKONTO == sysid
                                       select konto).FirstOrDefault();
                
                KontoDto rval = Mapper.Map<ITKONTO, KontoDto>(kontoOutput);
                if (rval != null && kontoOutput != null)
                {
                    // Ticket#2012090410000131: falsche Blz zurückgegeben
                    rval.blz = (from blz in context.BLZ where blz.SYSBLZ == kontoOutput.SYSBLZ select blz.BLZ1).FirstOrDefault();
                    rval.syskonto = kontoOutput.SYSITKONTO;
                }
                return rval;
            }
        }


        /// <summary>
        /// finds all Blz by the given key and type of key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<BlzDto> findBlz(string data, BlzType type)
        {
            

            List<BlzDto> rval = new List<BlzDto>();
            using (DdOlExtended context = new DdOlExtended())
            {
                if (type == BlzType.BIC)
                {
                    List<BLZ> banken = (from blz in context.BLZ
                                        where blz.BIC.Contains(data)
                                        select blz).ToList();
                    rval.AddRange(Mapper.Map<List<BLZ>, List<BlzDto>>(banken));
                }
                else if (type == BlzType.BICENDSWITH)
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "val", Value = "%" + data });
                    rval.AddRange(context.ExecuteStoreQuery<BlzDto>("select * from blz where bic like :val", parameters.ToArray()).ToList());
                    
                }
                else if (type == BlzType.BLZ)
                {
                    List<BLZ> banken = (from blz in context.BLZ
                                        where blz.BLZ1.Contains(data)
                                        select blz).ToList();                    
                    
                   
                    rval.AddRange(Mapper.Map<List<BLZ>, List<BlzDto>>(banken));
                }
                else if (type == BlzType.BLZENDSWITH)
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "val", Value = "%"+data });
                    rval.AddRange(context.ExecuteStoreQuery<BlzDto>("select * from blz where blz like :val", parameters.ToArray()).ToList());
                    
                }
                else if (type == BlzType.IBAN)
                {
                    IBANValidator v = new IBANValidator();
             
                    Cic.OpenOne.Common.DTO.IBANValidationError err = v.checkIBAN(data);
                    if (err.error == OpenOne.Common.DTO.IBANValidationErrorType.NoError)
                    {


                        BlzDto blzdto = new  BlzDto();

                        string blzstring = long.Parse(v.getBLZ(data)).ToString();
                        
                        List<BLZ> banken = (from blz in context.BLZ
                                            where blz.BLZ1 ==  blzstring
                                            select blz).ToList();
                        rval.AddRange(Mapper.Map<List<BLZ>, List<BlzDto>>(banken));

                    }
                }
            }
            return rval;
        }

    }
}
