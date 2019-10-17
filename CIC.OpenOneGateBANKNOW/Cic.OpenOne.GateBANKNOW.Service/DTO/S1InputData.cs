using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using System.ServiceModel;
using System.IO;


namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Transferobjekt für  getAuskunftS1 Methode
    /// </summary>
    [MessageContract]
    public class S1InputData
    {
        /// <summary>
        /// SysID from AUSKUNFT
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public long SysAuskunft { get; set; }


        /// <summary>
        /// Error Code
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public long ErrorCode { get; set; }

        /// <summary>
        /// Error Text
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public string ErrorText { get; set; }


        /// <summary>
        /// Result from S1
        /// </summary>
        [MessageBodyMember(Order = 1)]
        public Stream result { get; set; }

        /// <summary>
        /// Close Stream
        /// </summary>
        public void Dispose()
        {
            if (result != null)
            {
                result.Close();
                result = null;
            }
        }
    }
}