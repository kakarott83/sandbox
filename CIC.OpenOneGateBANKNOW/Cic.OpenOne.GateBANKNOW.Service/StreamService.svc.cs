using System;
using System.Reflection;
using System.ServiceModel;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Cic.OpenOne.Common.Util.IO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    /// <summary>
    /// Data Receive Dto
    /// </summary>
    [MessageContract]
    public class RemoteDataInfo : IDisposable
    {
        /// <summary>
        /// File Name
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        /// <summary>
        /// Length
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public long Length;

        /// <summary>
        /// Stream
        /// </summary>
        [MessageBodyMember(Order = 1)]
        public Stream data;

        /// <summary>
        /// Close the Stream
        /// </summary>
        public void Dispose()
        {
            if (data != null)
            {
                data.Close();
                data = null;
            }
        }
    }

   

    /// <summary>
    /// Service for streaming data in both directions
    /// 
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW", InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class StreamService : IStreamService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Example-Service to receive Data / upload via stream
        /// </summary>
        /// <param name="data"></param>
        public void setData(RemoteDataInfo data)
        {
            Stream input = data.data;
            

            DelimitedStreamReader.lineRead processor = delegate(DelimitedStreamReader context, String line)
            {
                Console.WriteLine(line);

            };
            DelimitedStreamReader dsr = new DelimitedStreamReader(input, 0x0B, 16800, UTF8Encoding.Default, processor, null);
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            dsr.start();
            double duration = DateTime.Now.TimeOfDay.TotalMilliseconds - start;

          
            _log.Debug("Loaded " + dsr.bytesTotal + " in " + duration + "ms for id " + data.FileName);
        }

        /// <summary>
        /// Example Service to deliver requested data as stream
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Stream getData(int id)
        {
            

            return new AsyncLoadedDataStream(delegate(BlockingCollection<byte> queue)
            {
                byte[] mybytes = FileUtils.loadData(@"C:\temp\s1test.txt"); //ASCIIEncoding.Default.GetBytes("blafaselblafaselblafaselblafaselblafaselblafaselblafaselblafaselENDE");
                foreach (byte b in mybytes) queue.Add(b);
            });
        }

        public void setAuskunftS1(S1InputData input)
        {
            if (input == null || input.SysAuskunft == 0)
            {
                if (input.result != null)
                {
                    try
                    {
                        _log.Debug("Initializing setAuskunftS1...");
                        DelimitedStreamReader.lineRead processor = delegate (DelimitedStreamReader ctx, String line) { };
                        DelimitedStreamReader dsr = new DelimitedStreamReader(input.result, 0x0B, 16800, UTF8Encoding.Default, processor, new byte[] { 0x01, 0x01, 0x01 });
                        dsr.start();
                    }
                    catch (Exception)
                    {
                        _log.Debug("Done with 1");
                    }
                    _log.Debug("Done with 0");
                }
                return;
            }

            oMessagingDto rval = new oMessagingDto();
            CredentialContext cctx = new CredentialContext();
            List<S1GetResponseDto> listInBO = new List<S1GetResponseDto>();

            try
            {

                _log.Debug("Starting to receive Data from S1...");
                cctx.validateService();

                if (input.SysAuskunft == 0)
                {
                    throw new ArgumentException("getAuskunftS1: SysAuskunftssatz is empty or 0");
                }

                RISKEWBS1DBDao s1DBDao = new RISKEWBS1DBDao(input.SysAuskunft);

                if (input.ErrorCode > 0)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorS1BatchProgram, "");

                    ArgumentException e = new ArgumentException(input.ErrorText);
                    _log.Error("getAuskunftS1: error from Simple Services", e);
                    throw new ArgumentException("getAuskunftS1: Fehler S1/Batchprogram: " + input.ErrorText);
                }


                if (input.result == null)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorS1BatchProgram, "");

                    ArgumentException e = new ArgumentException("Data from Siple Services is empy");
                    _log.Error("getAuskunftS1", e);
                    throw new ArgumentException("Received data is empty");
                }



                // parsing imputString (received from s1) into list of the S1GetResponseDto
                S1Bo s1Bo = new S1Bo(s1DBDao);

                // check the data received from s1
                if (s1DBDao.ValidateAnswerData(null) == 1)
                {
                    s1DBDao.SaveAuskunftStatus(AuskunftStatus.ErrorSimpleService, "");
                    ArgumentException e = new ArgumentException(s1DBDao.ErrorText);
                    _log.Error("getAuskunftS1: Data was not sent successfully to S1 for this response", e);
                    throw new ArgumentException(s1DBDao.ErrorText);
                }
                s1Bo.processStream(input.result, input.SysAuskunft);



            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                _log.Error("Failed to receive Data from S1 Stream", e);
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                _log.Error("Failed to receive Data from S1 Stream", e);
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                _log.Error("Failed to receive Data from S1 Stream", e);

            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                _log.Error("Failed to receive Data from S1 Stream", e);

            }
        }


        public oMessagingDto connectionTest()
        {
            oMessagingDto rval = new oMessagingDto();
            CredentialContext cctx = new CredentialContext();

            try
            {
                _log.Debug("StreamService.connectionTest called");
                cctx.validateService();
                _log.Debug("StreamService.connectionTest validateService successful");
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");

            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");

            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");

            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");

            }

            return rval;
        }
    }

  
}