using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.CRIF;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public class CRIFBo
    {
        public static readonly  String AUSKUNFTTYP = "CrifKontrollinhaber";
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// CRIF Kontrollinhaber Verarbeitung
        /// </summary>
        /// <param name="sysantrag">Antrags-Id</param>
        /// <param name="adressid">Bei Trefferliste Auswahl der CRRSID für den Kunden</param>
        /// <param name="sysperole">Perole</param>
        /// <returns></returns>
        public ogetControlPersonBusinessDto getControlPersonBusiness(long sysantrag, String adressid, long sysperole)
        {
            ogetControlPersonBusinessDto rval = new ogetControlPersonBusinessDto();
            using (PrismaExtended pe = new PrismaExtended())
            {
                DbConnection con = (pe.Database.Connection);
                long sysit = pe.ExecuteStoreQuery<long>("select sysit from antrag where sysid=" + sysantrag).FirstOrDefault();
                CrifKontrollinhaberBo ckb = (CrifKontrollinhaberBo)AuskunftBoFactory.CreateCrifKontrollinhaberBO(AuskunfttypDao.CrifKontrollinhaber);

                long sysauskunft = 0;
                bool CRIFOK = false;
            

                //0 falls adressid übergeben wird, in den IT des antrags in CRRSID schreiben (Trefferliste-Zuweisung) 
                if (adressid != null) {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "aid", Value = adressid });
                    pe.ExecuteStoreCommand("update it set crrsid=:aid where sysit=:sysit", parameters.ToArray());
                }
                else if(!CRIFOK)//remove to avoid adress-id matching inside InhaberBO
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    pe.ExecuteStoreCommand("update it set crrsid=null where sysit=:sysit", parameters.ToArray());
                }

                //1 Auskunft auslösen
                
                try
                {
                    if (!CRIFOK||sysauskunft==0)
                    {
                        AuskunftDto adto = ckb.doAuskunft("ANTRAG", sysantrag);
                        sysauskunft = adto.sysAuskunft;
                    }

                    rval.status = ckb.getExtendedAuskunftStatus(sysauskunft);


                    //2 in IT des antrags itoption.FLAG01 Feststellungspflicht übernehmen - CFCOMPANY.LEGALFORMTEXT und Lookupliste matchen (DDLKPPOS CRIF_LEGALFORM tooltip)
                    long optcount = pe.ExecuteStoreQuery<long>("select count(*) from itoption where sysit=" + sysit).FirstOrDefault();
                    int bpflicht = ckb.getFeststellungspflicht(sysauskunft) ? 1 : 0;

                 
                    
                    String custAdrId = ckb.getCustomerAdressId(sysauskunft);
                    if (custAdrId != null && !CRIFOK)//falls Customer eindeutig ermittelt wurde, gleich in IT zurückschreiben
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "aid", Value = custAdrId });
                        pe.ExecuteStoreCommand("update it set crrsid=:aid where sysit=:sysit", parameters.ToArray());
                    }

                    //3 falls nicht eindeutiges Ergebnis Trefferliste füllen 
                    if (rval.status == KontrollinhaberStatus.LIST)
                        rval.adresslist = ckb.getTrefferliste(sysauskunft);

                    //4 falls eindeutig und Feststellungspflicht die ggf. vorhandenen Kontrollinhaber in IT übernehmen und für den Antrag die KNE anlegen
                    //KNE mit RELATETYPCODE WB, KNETYP OHNE
                    //falls für den it bereits kne bestehen hier wieder zurückliefern, wenn aus CRIF keine neuen Kontrollinhaber geliefert werden, ansonsten alte löschen
                    List<AdresseDto> itsOrg = ckb.getKontrollinhaber(sysauskunft);
                    List<AdresseDto> its = new List<AdresseDto>();
                    String lastcpt = null;

                    //alle verwenden, welche as controlPersonType null sind und alle des jeweils ersten vorhandenen controlPersonType!=null
                    foreach(AdresseDto adr in itsOrg)
                    {
                        if(adr.controlPersonType==null||adr.controlPersonType.Length==0)
                        {
                            adr.controlPersonType = "HIGHEST_DECISION_MAKER";//Sofern von der CRIF ein Kontrollinhaber ohne Funktionstyp geliefert werden, wird per Default immer als Art der „HIGHEST_DECISION_MAKER“ verwendet
                            its.Add(adr); continue;
                        }
                        if (lastcpt != null && !adr.controlPersonType.Equals(lastcpt))
                            break;
                        its.Add(adr); 
                        lastcpt = adr.controlPersonType;
                    }
                    if (rval.status == KontrollinhaberStatus.FOUND && !CRIFOK)
                        rval.kne = manageKNE(sysantrag, sysit, sysperole, its);
                    else                   
                        rval.kne = con.Query<KneDto>("select * from itkne where sysunter=:sysit and area='ANTRAG' and sysarea=:sysantrag", new { sysit = sysit, sysantrag = sysantrag }).ToList();

                    
                    //save Feststellungspflicht
                    if (optcount == 0)
                    {
                        pe.ExecuteStoreCommand("insert into itoption(sysitoption,sysit,flag01) values(" + sysit + "," + sysit + "," + bpflicht + ")");
                    }
                    else
                    {
                        pe.ExecuteStoreCommand("update itoption set flag01=" + bpflicht + " where sysit=" + sysit);
                    }
                    pe.SaveChanges();
                    rval.feststellungsPflicht = bpflicht;

                }
                catch(Exception e)
                {
                    _log.Error("Error fetching Kontrollinhaber from CRIF: " + e.Message, e);
                    rval.status = KontrollinhaberStatus.ERROR_CRIF;
                    
                }
            
                

            }
        

            
            return rval;

        }

        /// <summary>
        /// Legt die in CRIF gefundenen Adressen als Interessenten an und verknüpft sie als ITKNE-Satz mit dem Antrag
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="sysit"></param>
        /// <param name="sysperole"></param>
        /// <param name="its"></param>
        /// <returns></returns>
        private List<DTO.KneDto> manageKNE(long sysantrag, long sysit,long sysperole, List<AdresseDto> its)
        {
            List<DTO.KneDto> rval = null;
            purgeKNE(sysit, sysantrag);
            if (its != null && its.Count > 0)
            {
                KundeDao kdao = new KundeDao();
                ZusatzdatenDao zddao = new ZusatzdatenDao();
                rval = new List<DTO.KneDto>();

                foreach (AdresseDto adr in its)
                {
                    KundeDto newIt = kdao.createKunde(AutoMapper.Mapper.Map<AdresseDto, KundeDto>(adr), sysperole);
                    KneDto kne = new KneDto();
                    kne.sysarea = sysantrag;
                    kne.sysober = newIt.sysit;
                    kne.sysunter = sysit;
                    kne.coderelatekind = adr.controlPersonType;
                    rval.Add(zddao.createOrUpdateKne(kne));
                }
            }
            return rval;
        }
       
        /// <summary>
        /// update auskunft for KNE set statusnum=1 für area/id von Antrag
        /// remove all kne's for the customer
        /// </summary>
        /// <param name="sysantrag"></param>
        public void resetAuskunfStatus(long sysantrag)
        {
            IAuskunftDao dao = new AuskunftDao();
            //KNE
            dao.invalidateAuskunft(sysantrag, "ANTRAG", AUSKUNFTTYP);
            using (PrismaExtended pe = new PrismaExtended())
            {
                long sysit = pe.ExecuteStoreQuery<long>("select sysit from antrag where sysid=" + sysantrag).FirstOrDefault();

                purgeKNE(sysit, sysantrag);
            }
        }

        /// <summary>
        /// Deletes all KNE's
        /// </summary>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        private void purgeKNE(long sysit, long sysantrag)
        {
            using (PrismaExtended pe = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                pe.ExecuteStoreCommand("delete from itkne where sysunter=:sysit and area='ANTRAG' and sysarea=:sysantrag", parameters.ToArray());
            }
        }
    }
}
