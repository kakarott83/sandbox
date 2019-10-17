using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.IO;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using CIC.Database.IC.EF6.Model;
using Devart.Data.Oracle;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// 
    /// </summary>
    public class S1Bo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// myInputString
        /// </summary>
        private string myInputString = string.Empty;

        /// <summary>
        /// errorText
        /// </summary>
        private string errorText = string.Empty;

        private IRISKEWBS1DBDao dbDao;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        public S1Bo(string inputString)
        {
            myInputString = inputString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbDao"></param>
        public S1Bo(IRISKEWBS1DBDao dbDao)
        {
            this.dbDao = dbDao;
        }

        /// <summary>
        /// ErrorText
        /// </summary>
        public string ErrorText
        {
            get
            {
                return errorText;
            }
        }


        /// <summary>
        /// processes the complete s1 response stream, converts to dto and saves to db
        /// 
        /// </summary>
        /// <param name="input">Stream to process</param>
        /// <param name="sysAuskunft">id of auskunft</param>
        public void processStream(Stream input, long sysAuskunft)
        {
            long sysdeoutexec = 0;
            using (DdIcExtended context = new DdIcExtended())
            {
                // AUSKUNFT
                AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                // DEOUTEXEC
                DEOUTEXEC DEOutExec = new DEOUTEXEC();
                DEOutExec.AUSKUNFT = Auskunft;
                context.DEOUTEXEC.Add(DEOutExec);

                context.SaveChanges();
                sysdeoutexec = DEOutExec.SYSDEOUTEXEC;
            }
            int maxblockcount = myGetMaxInsertRecords(0);
            int blockcount = 0;
            AuskunftStatus rvalstatus = AuskunftStatus.OK;
            BlockingCollection<S1GetResponseDBDto> insertQueue = new BlockingCollection<S1GetResponseDBDto>(10000);

            Task consumer = Task.Factory.StartNew(() =>
            {
                while (!insertQueue.IsAddingCompleted)
                {
                    try
                    {
                        if(insertQueue.Count>0)
                            saveQueue(insertQueue,sysdeoutexec);
                    }
                    catch (Exception e)
                    {
                        _log.Debug("Problem in Processing S1 Inserts", e);
                        break;
                    }
                    // Wait a bit until queue has more data
                    Thread.Sleep(5000);
                }
                _log.Debug("S1 Insert Queue finished");
            });
            
            DelimitedStreamReader.lineRead processor = delegate (DelimitedStreamReader ctx, String line)
            {
                //read line by line
                S1GetResponseDto dto = OutputDtoPack.FromStream(line);

                S1GetResponseDBDto oneResult = dbDao.addStreamedS1Response(sysdeoutexec, dto);
                insertQueue.Add(oneResult);
                if (oneResult.status != AuskunftStatus.OK)
                {
                    //fachlicher übersteuert fehlerfrei und technischer übersteuert fachlich
                    if (rvalstatus != AuskunftStatus.ErrorTech)
                    {
                        rvalstatus = oneResult.status;
                    }
                }

                // Für Lesen und Schreiben im BLockmodus hinzugefügt
                blockcount++;
                if (maxblockcount != 0)
                {
                    if (blockcount % maxblockcount == 0)
                    {
                        _log.Debug("Added " + blockcount + " values");                       
                    }
                }
            };
            DelimitedStreamReader dsr = new DelimitedStreamReader(input, 0x0B, 16800, UTF8Encoding.Default, processor, new byte[] { 0x01, 0x01, 0x01 });
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            dsr.start();//will take until all lines have been processed in processor above!
            insertQueue.CompleteAdding();
            consumer.Wait();           

            _log.Debug("Saved all values");
            double duration = DateTime.Now.TimeOfDay.TotalMilliseconds - start;
            _log.Debug("Finished reading from Stream in " + duration + "ms with a total of " + dsr.bytesTotal + "bytes");

            using (DdIcExtended context = new DdIcExtended())
            {
                // AUSKUNFT
                AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();

                Auskunft.FEHLERCODE = ((int)rvalstatus).ToString();
                if (Auskunft.AUSKUNFTTYP == null)
                    context.Entry(Auskunft).Reference(f => f.AUSKUNFTTYP).Load();


                string bezeichnungTyp = "";
                if (Auskunft.AUSKUNFTTYP != null)
                {
                    bezeichnungTyp = Auskunft.AUSKUNFTTYP.BEZEICHNUNG;
                }

                if (rvalstatus >= 0)
                {
                    Auskunft.STATUS = bezeichnungTyp + " - Aufruf erfolgreich";
                }
                else
                {
                    Auskunft.STATUS = bezeichnungTyp + " - Aufruf nicht erfolgreich";
                }

                context.SaveChanges();

            }
        }

        private void saveQueue(BlockingCollection<S1GetResponseDBDto> insertQueue, long sysdeoutexec)
        {


            ParallelOptions opt = new ParallelOptions();
            opt.MaxDegreeOfParallelism = 3;
            String cstr = Configuration.DeliverOpenLeaseConnectionString();
            int amount = 0;
            Parallel.ForEach(insertQueue.GetConsumingPartitioner(), opt, (dto) =>
            {
                OracleConnection con = null;
                try
                {
                    con = new OracleConnection(cstr);
                    con.Open();
                    amount++;
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"INSERT /*+ append */ INTO CIC.DEWBRKL(SYSDEOUTEXEC, SYSKD, SYSVT,RKLVT,WBKAPITAL,WBKAPITALACP,WBZINS,WBZINSACP,PARAMETERID,PDT1,
PDT1FRAC,FEHLERCODE,FEHLERBESCHREIBUNG,PDEC901,PDEC902,PDEC903,DAT01,DAT02,DAT03,STR01,STR02,STR03,LGD,EAD,PD_LT,RKL_LT,WBKAPITAL_LT,WBZINS_LT,SCOREBEZEICHNUNG,SCOREVERSION,SCOREWERT,EADRATIO) 
VALUES (:SYSDEOUTEXEC,:SYSKD,:SYSVT,:RKLVT,:WBKAPITAL,:WBKAPITALACP,:WBZINS,:WBZINSACP,:PARAMETERID,:PDT1,:PDT1FRAC,:FEHLERCODE,:FEHLERBESCHREIBUNG,:PDEC901,:PDEC902,:PDEC903,:DAT01,:DAT02,:DAT03,:STR01,:STR02,:STR03,:LGD,:EAD,:PD_LT,:RKL_LT,:WBKAPITAL_LT,:WBZINS_LT,:SCOREBEZEICHNUNG,:SCOREVERSION,:SCOREWERT,:EADRATIO) returning sysdewbrkl into :Id";
                    cmd.Parameters.Add(new OracleParameter("SYSDEOUTEXEC", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("SYSKD", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("SYSVT", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("RKLVT", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("WBKAPITAL", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("WBKAPITALACP", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("WBZINS", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("WBZINSACP", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("PARAMETERID", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("PDT1", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("PDT1FRAC", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("FEHLERCODE", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("FEHLERBESCHREIBUNG", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("PDEC901", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("PDEC902", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("PDEC903", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("DAT01", OracleDbType.Date));
                    cmd.Parameters.Add(new OracleParameter("DAT02", OracleDbType.Date));
                    cmd.Parameters.Add(new OracleParameter("DAT03", OracleDbType.Date));
                    cmd.Parameters.Add(new OracleParameter("STR01", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("STR02", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("STR03", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("LGD", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("EAD", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("EADRATIO", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("PD_LT", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("RKL_LT", OracleDbType.Integer));
                    cmd.Parameters.Add(new OracleParameter("WBKAPITAL_LT", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("WBZINS_LT", OracleDbType.Number));
                    cmd.Parameters.Add(new OracleParameter("SCOREBEZEICHNUNG", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("SCOREVERSION", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("SCOREWERT", OracleDbType.Integer));
                    cmd.Parameters.Add(new OracleParameter("Id", OracleDbType.Long, System.Data.ParameterDirection.ReturnValue));


                    DbCommand envout = con.CreateCommand();
                    envout.CommandType = System.Data.CommandType.Text;
                    envout.CommandText = @"INSERT /*+ append */ INTO CIC.DEENVOUT(SYSDEWBRKL, INQUIRYCODE, ORGANIZATIONCODE, PROCESSVERSION, LAYOUTVERSION, INQUIRYDATE, INQUIRYTIME) VALUES(:SYSDEWBRKL,:INQUIRYCODE,:ORGANIZATIONCODE,:PROCESSVERSION,:LAYOUTVERSION,:INQUIRYDATE,:INQUIRYTIME)";
                    envout.Parameters.Add(new OracleParameter("SYSDEWBRKL", OracleDbType.Long));
                    envout.Parameters.Add(new OracleParameter("INQUIRYCODE", OracleDbType.VarChar));
                    envout.Parameters.Add(new OracleParameter("ORGANIZATIONCODE", OracleDbType.VarChar));
                    envout.Parameters.Add(new OracleParameter("PROCESSVERSION", OracleDbType.Integer));
                    envout.Parameters.Add(new OracleParameter("LAYOUTVERSION", OracleDbType.Integer));
                    envout.Parameters.Add(new OracleParameter("INQUIRYDATE", OracleDbType.Date));
                    envout.Parameters.Add(new OracleParameter("INQUIRYTIME", OracleDbType.VarChar));


                    DbCommand errout = con.CreateCommand();
                    errout.CommandType = System.Data.CommandType.Text;
                    errout.CommandText = @"INSERT /*+ append */ INTO CIC.DEERROROUT(SYSDEWBRKL, INQUIRYDATE, INQUIRYTIME,CODE,DESCRIPTION,ENGINEVERSION,ENGINESTACKTRACE,JAVASTACKTRACE) VALUES (:SYSDEWBRKL,:INQUIRYDATE,:INQUIRYTIME,:CODE,:DESCRIPTION,:ENGINEVERSION,:ENGINESTACKTRACE,:JAVASTACKTRACE)";
                    errout.Parameters.Add(new OracleParameter("SYSDEWBRKL", OracleDbType.Long));
                    errout.Parameters.Add(new OracleParameter("INQUIRYDATE", OracleDbType.Date));
                    errout.Parameters.Add(new OracleParameter("INQUIRYTIME", OracleDbType.Long));
                    errout.Parameters.Add(new OracleParameter("CODE", OracleDbType.Long));
                    errout.Parameters.Add(new OracleParameter("DESCRIPTION", OracleDbType.VarChar));
                    errout.Parameters.Add(new OracleParameter("ENGINEVERSION", OracleDbType.VarChar));
                    errout.Parameters.Add(new OracleParameter("ENGINESTACKTRACE", OracleDbType.VarChar));
                    errout.Parameters.Add(new OracleParameter("JAVASTACKTRACE", OracleDbType.VarChar));

                    DbCommand cmdscore = con.CreateCommand();
                    cmdscore.CommandType = System.Data.CommandType.Text;
                    cmdscore.CommandText = @"INSERT /*+ append */ INTO CIC.DEWBSCORE(SYSDEWBRKL,RANK, SCORETYP, BEZEICHNUNG,RESULTATWERT,EINGABEWERT) VALUES (:SYSDEWBRKL,:RANK, :SCORETYP, :BEZEICHNUNG,:RESULTATWERT,:EINGABEWERT)";
                    cmdscore.Parameters.Add(new OracleParameter("SYSDEWBRKL", OracleDbType.Long));
                    cmdscore.Parameters.Add(new OracleParameter("RANK", OracleDbType.Integer));
                    cmdscore.Parameters.Add(new OracleParameter("SCORETYP", OracleDbType.VarChar));
                    cmdscore.Parameters.Add(new OracleParameter("BEZEICHNUNG", OracleDbType.VarChar));
                    cmdscore.Parameters.Add(new OracleParameter("RESULTATWERT", OracleDbType.Number));
                    cmdscore.Parameters.Add(new OracleParameter("EINGABEWERT", OracleDbType.VarChar));



                    dto.insertToDb(cmd, envout, errout, cmdscore);
                    cmd.Dispose();
                    envout.Dispose();
                    errout.Dispose();
                    cmdscore.Dispose();
                    if(amount%10000==0)
                        _log.Debug("Saved " + amount + " to db");

                }
                catch (Exception e)
                {
                    if (dto.dewbrkl!=null)
                        _log.Error("Saving one QueueEntry failed for VT.SYSID="+ dto.dewbrkl.SYSVT, e);
                    else
                        _log.Error("Saving one QueueEntry failed ", e);
                }
                finally
                {
                    try
                    {
                        if (con != null)
                            con.Close();
                    }
                    catch (Exception) { }
                }
            });
            

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public void GetList(List<S1GetResponseDto> List)
        {
            OutputDtoPack _outPack = new OutputDtoPack();

            _outPack.FromString(myInputString);



            foreach (S1GetResponseDto _s1DBDto in _outPack)
            {
                List.Add(_s1DBDto);
            }
        }

        /// <summary>
        /// Holt den Max-Insert Records Value aus der CFG
        /// </summary>
        /// <returns>Max-Insert-Records</returns>
        private static int myGetMaxInsertRecords(int defaultValue)
        {
            int retValue = 0;
            String cfgParam = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry("RISKEWB_S1", "MAX_INSERT_RECORDS", defaultValue.ToString(), "DECISIONENGINE");
            Int32.TryParse(cfgParam, out retValue);

            return retValue;
        }


    }
}