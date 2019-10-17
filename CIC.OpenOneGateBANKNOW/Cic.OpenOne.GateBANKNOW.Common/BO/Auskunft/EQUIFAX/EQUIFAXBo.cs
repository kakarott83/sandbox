using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EQUIFAX;
using Dapper;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.EQUIFAX
{
    public class EQUIFAXBo : AbstractAuskunftBo<EQUIFAXInDto,AuskunftDto>
    {
        private IEQUIFAXDao dao;
        private IAuskunftDao adao;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public EQUIFAXBo(IEQUIFAXDao dao, IAuskunftDao ad)
        {
            this.dao = dao;
            this.adao = ad;
        }

      
 
        /// <summary>
        /// Performes the Auskunft with the prefilled input-data on the prefilled Auskunft-entry
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            EQUIFAXDao ed = new EQUIFAXDao();

            using (PrismaExtended ctx = new PrismaExtended())
            {
                try
                {
                    AuskunftCFGDto cfg = adao.getConfiguration(AuskunfttypDao.EQUIFAXRisk);
                    EQUIFAXInDto input = ctx.ExecuteStoreQuery<EQUIFAXInDto>("select * from EQUIFAXRISKIN where sysauskunft=" + sysAuskunft).FirstOrDefault();
                    if (input == null || input.idCode == null)
                        throw new Exception("EQUIFAXRISKIN was not filled for sysauskunft=" + sysAuskunft);
                    EQUIFAXOutDto output = dao.requestRiskData(cfg, input);

                    EQUIFAXRISKOUT toDB = AutoMapper.Mapper.Map<RISK, EQUIFAXRISKOUT>(output.data);
                    toDB = AutoMapper.Mapper.Map<ARAAttribute, EQUIFAXRISKOUT>(output.data.araAttributes, toDB);
                    toDB.sysauskunft = sysAuskunft;
                    toDB.transactionId = output.transactionId;
                    if (toDB.transactionId != null && toDB.transactionId.Length > 5)
                        toDB.transactionId = toDB.transactionId.Substring(toDB.transactionId.Length - 5);
                    toDB.transactionState = output.transactionState;
                    toDB.timestamp = output.timestamp;
                    //save data to db

                    ctx.insertData(@"INSERT INTO CIC.EQUIFAXRISKOUT(SYSAUSKUNFT,IDENTIFIER,IDCODE,RETURNCODE,PRESENT,RATING,TOTALNUMOP ,NUMCONSUMERCREDITOP ,NUMMORTGAGEOP ,NUMPERSONALLOANOP ,NUMCREDITCARDOP ,NUMTELCOOP ,TOTALNUMOTHERUP ,TOTALUPB ,UPBALOWNENTITY ,UPBALOENTITIES ,UPBALCONSUMERCREDIT ,UPBALMORTGAGE ,UPBALPERSONALLOAN ,UPBALCREDITCARD ,UPBALTELCO ,UPBALOPRODUCTS ,WORSTUPBAL ,WORSTSITUATIONCODE ,NUMDAYSWORSTSITUATION ,NUMCREDITORS ,DELINCUENCYDAYS ,TRANSACTIONSTATE ,TRANSACTIONID,TIMESTAMP) 
                            VALUES (:SYSAUSKUNFT,:IDENTIFIER,:IDCODE,:RETURNCODE,:PRESENT,:RATING,:TOTALNUMBEROFOPERATIONS,:numberOfConsumerCreditOps,:NUMBEROFMORTGAGEOPERATIONS,:NUMBEROFPERSONALLOANOPERATIONS,:NUMBEROFCREDITCARDOPERATIONS,:NUMBEROFTELCOOPERATIONS,:TOTALNUMBEROFOTHERUNPAID,:TOTALUNPAIDBALANCE,:UNPAIDBALANCEOWNENTITY,:UNPAIDBALANCEOFOTHERENTITIES,:UNPAIDBALANCEOFCONSUMERCREDIT,:UNPAIDBALANCEOFMORTGAGE,:UNPAIDBALANCEOFPERSONALLOAN,:UNPAIDBALANCEOFCREDITCARD,:UNPAIDBALANCEOFTELCO,:UNPAIDBALANCEOFOTHERPRODUCTS,:WORSTUNPAIDBALANCE,:WORSTSITUATIONCODE,:NUMBEROFDAYSOFWORSTSITUATION,:NUMBEROFCREDITORS,:DELINCUENCYDAYS,:TRANSACTIONSTATE,:TRANSACTIONID,:TIMESTAMP)", toDB);
         //           returning SYSEQUIFAXRISKOUT into :Id", toDB);


                    UpdateAuskunft(ctx,sysAuskunft, (long)AuskunftErrorCode.NoError);
                }
                catch(Exception ex)
                {
                    _log.Error("EQUIFAX Auskunft failed ", ex);
                    UpdateAuskunft(ctx, sysAuskunft, (long)AuskunftErrorCode.ErrorCIC);
                }
                DbConnection con = (ctx.Database.Connection);
                return con.Query<AuskunftDto>("select * from AUSKUNFT where sysauskunft=:sysauskunft", new { sysauskunft= sysAuskunft }).FirstOrDefault();
            }
            
        }
        private void UpdateAuskunft(PrismaExtended ctx,long sysAuskunft, long code)
        {
            DateTime datetime = DateTime.Now;
            long uhrzeit = datetime.Hour * 3600 * 100 + datetime.Minute * 60 * 100 + datetime.Second * 100 + 1;
             String status = "Aufruf erfolgreich";
            if(code<0)
                status = "Aufruf nicht erfolgreich";


            DbConnection con = (ctx.Database.Connection);
            con.Execute("update AUSKUNFT set ANFRAGEDATUM=sysdate, FEHLERCODE=:FEHLERCODE,ANFRAGEUHRZEIT=:ANFRAGEUHRZEIT,STATUS=:STATUS where sysauskunft=:sysauskunft", new { FEHLERCODE=code.ToString(), STATUS =status,sysauskunft = sysAuskunft, ANFRAGEUHRZEIT = uhrzeit });

        }

        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            EQUIFAXInDto input = new EQUIFAXInDto();
            //needed here: fill input from area/sysId
            return doAuskunft(input);
        }

        public override AuskunftDto doAuskunft(EQUIFAXInDto inDto)
        {
         
            //save input in db
            using (PrismaExtended ctx = new PrismaExtended())
            {
                DbConnection con = (ctx.Database.Connection);
              
                DateTime datetime = DateTime.Now;
                long uhrzeit = datetime.Hour * 3600 * 100 + datetime.Minute * 60 * 100 + datetime.Second * 100 + 1;
                long sysauskunfttyp = ctx.ExecuteStoreQuery<long>("select sysauskunfttyp from auskunfttyp where bezeichnung='" + AuskunfttypDao.EQUIFAXRisk + "'").FirstOrDefault();
                inDto.sysauskunft = ctx.insertData(@"INSERT INTO CIC.AUSKUNFT(SYSAUSKUNFTTYP,ANFRAGEDATUM,ANFRAGEUHRZEIT) VALUES (:SYSAUSKUNFTTYP,sysdate,:ANFRAGEUHRZEIT) returning sysauskunft into :Id", new { SYSAUSKUNFTTYP= sysauskunfttyp, ANFRAGEUHRZEIT=uhrzeit });
                
                int sysin = ctx.insertData(@"INSERT INTO CIC.EQUIFAXRISKIN(SYSAUSKUNFT, IDTYPE,IDCODE,POSTALCODE,DATEOFBIRTH) VALUES (:SYSAUSKUNFT, :IDTYPE,:IDCODE,:POSTALCODE,:DATEOFBIRTH) returning sysequifaxriskin into :Id", inDto);
                
            }
            //perform Auskunft
            return doAuskunft(inDto.sysauskunft);
        }
    }
}
