using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text.RegularExpressions;
using System.IO;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenLease.ServiceAccess.Merge.DTO;

namespace Cic.OpenLease.Service.Merge
{
    public class assistantServiceBo
    {
        public RequestAndResponseDto[] getLogFile(string logFile, igetLogFileDto input)
        {
            List<RequestAndResponseDto> rval = new List<RequestAndResponseDto>();
            RequestAndResponseDto rvalRequestAndResponse = new RequestAndResponseDto();
            bool isFirstLine = false;
            bool isNewRequest = false;
            string messageID = "";
            string method = "";
            DateTime datetime = DateTime.Now;
            string newRequest = "";
            string[] logFileLinesTemp = logFile.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < logFileLinesTemp.Length; i++)
            {
                logFileLinesTemp[i] = logFileLinesTemp[i].Trim('\r');
            }
            List<string> templist = new List<string>();
            foreach (string line in logFileLinesTemp)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    templist.Add(line);
                }
            }
            string[] logFileLines = templist.ToArray();
            for (int i = 0; i < logFileLines.Length; i++)
            {
                if (!isNewRequest)
                {
                    if (logFileLines[i].Length >= 19)
                    {
                        string date = logFileLines[i].Substring(0, 19);

                        try
                        {
                            datetime = DateTime.Parse(date);
                            isFirstLine = true;
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                bool finish = false;
                if (isNewRequest)
                {
                    Match regexMatch = Regex.Match(logFileLines[i], "envelope", RegexOptions.IgnoreCase);
                    if (!regexMatch.Success)
                    {
                        newRequest += logFileLines[i] + "\n";
                        Match regexMatchID = Regex.Match(logFileLines[i], "MessageID", RegexOptions.IgnoreCase);
                        Match regexMatchRelates = Regex.Match(logFileLines[i], "RelatesTo", RegexOptions.IgnoreCase);
                        if (regexMatchID.Success || regexMatchRelates.Success)
                        {
                            int idstart = logFileLines[i].IndexOf('>');
                            int idend = logFileLines[i].IndexOf("</");
                            messageID = logFileLines[i].Substring(idstart + 1, idend - idstart - 1);
                        }
                        Match regexMatchAction = Regex.Match(logFileLines[i], "Action", RegexOptions.IgnoreCase);
                        if (regexMatchAction.Success)
                        {
                            string sub = logFileLines[i].Substring(0, logFileLines[i].LastIndexOf('/') - 1);
                            method = sub.Substring(sub.LastIndexOf('/') + 1);
                            Match regexMatchResponse = Regex.Match(method, "Response", RegexOptions.IgnoreCase);
                            if (regexMatchResponse.Success)
                            {
                                method = method.Substring(0, method.LastIndexOf('R'));
                            }
                        }
                    }
                    else
                    {
                        newRequest += logFileLines[i];
                        finish = true;
                    }
                }
                if (isFirstLine)
                {
                    if (logFileLines[i].Contains("DEBUG Cic.Common.Web.Services.Protocols.MessageInspector [(null)]-"))
                    {
                        int begin = logFileLines[i].IndexOf('<');
                        if (begin > -1)
                        {
                            isNewRequest = true;
                            isFirstLine = false;
                            newRequest = logFileLines[i].Substring(begin) + "\n";
                        }
                    }
                }
                if (finish)
                {
                    if (String.IsNullOrEmpty(messageID))
                    {
                        rvalRequestAndResponse = new RequestAndResponseDto();
                        rvalRequestAndResponse.method = method;
                        RequestDto rvalrequest = checkOnParams(input, datetime, newRequest);
                        if (rvalrequest != null)
                        {
                            if (rvalrequest.isRequest)
                            {
                                rvalRequestAndResponse.request = rvalrequest;
                            }
                            else
                            {
                                rvalRequestAndResponse.response = rvalrequest;
                            }
                            rval.Add(rvalRequestAndResponse);
                        }
                        
                    }
                    else
                    {
                        RequestAndResponseDto request = (from rar in rval
                                                        where rar.messageID == messageID
                                                        select rar).FirstOrDefault();
                        if (request == null)
                        {
                            rvalRequestAndResponse = new RequestAndResponseDto();
                            rvalRequestAndResponse.messageID = messageID;
                            rvalRequestAndResponse.method = method;
                            RequestDto rvalrequest = checkOnParams(input, datetime, newRequest);
                            if (rvalrequest != null)
                            {
                                if (rvalrequest.isRequest)
                                {
                                    rvalRequestAndResponse.request = rvalrequest;
                                }
                                else
                                {
                                    rvalRequestAndResponse.response = rvalrequest;
                                }
                                rval.Add(rvalRequestAndResponse);
                            }
                            messageID = "";
                            method = "";
                        }
                        else
                        {
                            if (request.request != null)
                            {
                                request.response = createRequestDto(newRequest, datetime);
                            }
                            else
                            {
                                request.request = createRequestDto(newRequest, datetime);
                            }
                            messageID = "";
                            method = "";
                        }
                    }
                    isNewRequest = false;
                    isFirstLine = false;                                                                                                                                                                                                                                     
                }
               
            }

            return rval.ToArray();
        }

        public int[] getLogFilePids(string path)
        {
            List<int> rval = new List<int>();
            string filename = path.Substring(path.LastIndexOf('\\') + 1);
            path = path.Substring(0, path.LastIndexOf('\\') + 1);
            bool checkOnPID = filename.Contains("-[");
            if (checkOnPID)
            {
                string filenameWithoutPid = filename.Substring(0,filename.IndexOf('[')+1);
                string[] filePaths = Directory.GetFiles(path, "*.log");
                for (int i = 0; i < filePaths.Length; i++)
                {
                    string file = Path.GetFileName(filePaths[i]);
                    if(file.Contains(filenameWithoutPid))
                    {
                        int pid;
                        int start = file.IndexOf('[') +1;
                        int end = file.IndexOf(']');
                        string pids = file.Substring(start,end - start);
                        pid = Convert.ToInt32(pids);
                        if (pid > 0)
                        {
                            rval.Add(pid);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Das Logging besitzt keine PIDs");
            }
            return rval.ToArray();
        }

        private RequestDto createRequestDto(string request, DateTime timestamp)
        {
            RequestDto rval = new RequestDto();
            rval.wholeXMLRequest = request;
            rval.datumsstempel = timestamp;
            rval.isRequest = true;
            if (rval.wholeXMLRequest.Contains("Response"))
            {
                rval.isRequest = false;
            }
            string[] requestArray = rval.wholeXMLRequest.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string header = "";
            bool isheader = false;
            string body = "";
            bool isbody = false;
            for (int i = 0; i < requestArray.Length; i++)
            {
                if (!isheader && !isbody)
                {
                    Match regexMatchHeader = Regex.Match(requestArray[i], "s:header", RegexOptions.IgnoreCase);
                    if (regexMatchHeader.Success)
                    {
                        header += requestArray[i] + "\n";
                        isheader = true;
                    }
                    Match regexMatchBody = Regex.Match(requestArray[i], "s:body", RegexOptions.IgnoreCase);
                    if (regexMatchBody.Success)
                    {
                        body += requestArray[i] + "\n";
                        isbody = true;
                    }
                }
                else if(isheader && !isbody)
                {
                    header += requestArray[i] + "\n";
                    Match regexMatchHeaderEnd = Regex.Match(requestArray[i], "s:header", RegexOptions.IgnoreCase);
                    if (regexMatchHeaderEnd.Success)
                    {
                        Match regexMatchHeaderDefault = Regex.Match(requestArray[i], "DefaultMessageHeader", RegexOptions.IgnoreCase);
                        if (!regexMatchHeaderDefault.Success)
                        {
                            Match regexMatchHeaderEnd2 = Regex.Match(requestArray[i], "</");
                            if (regexMatchHeaderEnd2.Success)
                            {
                                rval.header = header;
                                isheader = false;
                            }
                        }
                    }
                }
                else if (!isheader && isbody)
                {
                    body += requestArray[i] + "\n";
                    Match regexMatchBodyEnd = Regex.Match(requestArray[i], "s:body", RegexOptions.IgnoreCase);
                    if (regexMatchBodyEnd.Success)
                    {
                        Match regexMatchBodyEnd2 = Regex.Match(requestArray[i], "</s:body>",RegexOptions.IgnoreCase);
                        if (regexMatchBodyEnd2.Success)
                        {
                            rval.body = body;
                            isheader = false;
                        }
                    }
                }
            }
            return rval;
        }

        private RequestDto checkOnParams(igetLogFileDto input, DateTime timestamp, string newRequest)
        {
            DateTime empty = new DateTime();
            RequestDto rvalrequest = new RequestDto();
            if (!String.IsNullOrEmpty(input.service))
            {
                Match regexMatchService = Regex.Match(newRequest, input.service, RegexOptions.IgnoreCase);
                bool service = regexMatchService.Success;
                if(!service)
                {
                    return null;
                }
            }
            if (!String.IsNullOrEmpty(input.method))
            {
                Match regexMatchService = Regex.Match(newRequest, input.method, RegexOptions.IgnoreCase);
                bool method = regexMatchService.Success;
                if (!method)
                {
                    return null;
                }
            }
            if (input.vonDatum == empty && input.bisDatum == empty)
            {
                return createRequestDto(newRequest, timestamp);
            }
            else if (input.vonDatum <= timestamp && input.bisDatum >= timestamp)
            {
                return createRequestDto(newRequest, timestamp);
            }
            else
            {
                return null;
            }
        }
    }
}