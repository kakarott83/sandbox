using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;


namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// RISK Auskunft Interface
    /// </summary>
    public interface IRISKEWBS1DBDao
    {
        /// <summary>
        /// get data from db
        /// </summary>
        /// <param name="listS1InDto"></param>
        /// <returns></returns>
        long GetDataFromDB(ref List<RISKEWBS1DataDto> listS1InDto);

        /// <summary>
        /// check data from s1
        /// </summary>
        /// <param name="listS1InDto"></param>
        /// <returns></returns>
        long ValidateAnswerData(List<S1GetResponseDto> listS1InDto);

        /// <summary>
        /// save data from s1 in output tables 
        /// </summary>
        /// <param name="listS1InDto"></param>
        /// <returns></returns>
        long SaveDataFromS1(List<S1GetResponseDto> listS1InDto);

        /// <summary>
        /// add a new entity for db from s1 response stream
        /// </summary>
        /// <param name="context"></param>
        /// <param name="DEOutExec"></param>
        /// <param name="s1Dto"></param>
        S1GetResponseDBDto addStreamedS1Response(long SYSDEOUTEXEC, S1GetResponseDto s1Dto);
    }
}
