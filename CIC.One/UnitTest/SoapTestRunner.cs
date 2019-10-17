using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;
using System.IO;
using Cic.OpenOne.Common.Util;
using System.Linq;
using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Serialization;

namespace UnitTest
{
    /// <summary>
    /// Principle:
    /// a running webservice records all valid requests via soap-logging and stores them away, in a folder, named webservicename_methodname.xml
    /// this unit-tests parses all this xmls of the folders, firing to the server
    /// if the response fails (exception or the response contains a filled error-message-object the unit-test raises an assert)
    /// 
    /// </summary>
    [TestClass]
    public class SoapTestRunner
    {
     
        /// <summary>
        /// The Adress the tests are run against
        /// </summary>
        public static String TARGET_SERVER = "http://se-dnexec/OpenLeaseGateONE/";

        /// <summary>
        /// search for the <To></To> and replace the target-server-name
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static String replaceTo(String request, String newHost)
        {
            return request.Replace("DESTINATION_SERVER", newHost);
           
        }

        /// <summary>
        /// Finds a tag in a string, including or excluding the tag itself
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        /// <param name="contentOnly"></param>
        /// <returns></returns>
        private String findTag(String text, String tag, bool contentOnly)
        {
            if(text==null) return null;
            int s1 = text.IndexOf("<"+tag);
            if(s1<0) return null;
            int s2 = text.IndexOf(">",s1+1);
            int s3 = text.IndexOf("</"+tag,s2+1);
            int s4 = text.IndexOf(">",s3+1);
            if(contentOnly)
            {
                return text.Substring(s2+1,s3-(s2+1));
            }
            return text.Substring(s1,s4-s1+1);

        }
        /// <summary>
        /// Validate for an response-error
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private String checkError(String response)
        {
            String rt = response.ToUpper();
            UTF8Encoding enc = new UTF8Encoding();

            if(rt.IndexOf("<TYPE>WARN</TYPE>")>-1||rt.IndexOf("<TYPE>ERROR</TYPE>")>-1||rt.IndexOf("<TYPE>FATAL</TYPE>")>-1)
            {

                String message = findTag(response, "message", false);
                if (message != null)
                {
                    try
                    {

                        Message m = XMLDeserializer.objectFromXml<Message>(enc.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8"" xmlns=""http://schemas.datacontract.org/2004/07/Cic.OpenOne.Common.DTO""?>\n"+message), "UTF-8");


                        return m.ToString();
                    }catch(Exception e)
                    {
                        return message;
                    }
                }
                return "Unreadable Message of Type error occured";
            }
            if (rt.IndexOf("EXCEPTION")>0)
            {
                String message = findTag(response, "Message", true);
                if (message != null) return message;
                return "Exception occured";
            }
            if (rt.IndexOf("FAULT") > 0 && rt.IndexOf("REASON") > 0)
            {
                String message = findTag(response, "s:Text", true);
                if (message != null) return message;
                return "Exception occured";
            }
            if (rt.IndexOf("ORA-") > 0)
            {
                return "Database error occured "+response.Substring(response.IndexOf("ORA-"),100);
            }
          

            return null;
        }
        [TestMethod]
        public void TestSoaps()
        {
            System.Diagnostics.Trace.WriteLine("Path: " + FileUtils.getCurrentPath());
            int failCount = 0;
            int successCount = 0;
            String server = TARGET_SERVER;
            
            String path = FileUtils.getCurrentPath() + "\\..\\..\\resources";
            if(!Directory.Exists(path))
                path = FileUtils.getCurrentPath() + "\\..\\resources";
            if (!Directory.Exists(path))
                path = FileUtils.getCurrentPath() + "\\..\\..\\UnitTest\\resources";
            String[] allFiles = Directory.GetFiles(path, "*.xml");

            List<String> filesSorted = (from a in allFiles
             select a).OrderBy(a=>a,new NaturalSortComparer<string>()).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (String file in filesSorted)
            {
                addMessage(sb,"");
                addMessage(sb,"Testing " + file + "...");
                int idx1 = file.IndexOf("_") + 1;
                String svc = file.Substring(idx1, file.IndexOf("_", idx1+1)  - idx1);

                try
                {
                    byte[] soapBytes = FileUtils.loadData(file);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(server + svc + ".svc");
                    UTF8Encoding enc = new UTF8Encoding();

                    byte[] bytes = enc.GetBytes(replaceTo(enc.GetString(soapBytes), server));
                    request.Method = "POST";
                    request.ContentLength = bytes.Length;
                    request.ContentType = "application/soap+xml; charset=utf-8";
                    Stream stream = request.GetRequestStream();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();

                    Stream datastream = null;
                    try
                    {
                        HttpWebResponse hresponse = (HttpWebResponse)request.GetResponse();
                        datastream = hresponse.GetResponseStream();
                    }
                    catch (WebException ex)
                    {

                        string exMessage = ex.Message;

                        if (ex.Response != null)
                        {
                            datastream = ex.Response.GetResponseStream();
                        }
                    }
                    
                    StreamReader reader = new StreamReader(datastream);
                    String response = reader.ReadToEnd();
                    String message = checkError(response);
                    if (message != null)
                    {
                        failCount++;
                        addMessage(sb, " Failure: " + message);
                        FileUtils.saveFile(file + ".err", enc.GetBytes(response));
                        addMessage(sb, "  -->See File " + file + ".err");
                        
                        
                    }
                    else successCount++;


                }
                
                catch (Exception e)
                {
                    addMessage(sb, "Failure during communication using " + file + ": " + e.Message);
                }
            }

            if(failCount>0)
                Assert.Fail(failCount+" service calls failed, "+successCount+" services succeeded! \n"+sb.ToString());

            
        }
        private void addMessage(StringBuilder sb , String msg)
        {
            System.Diagnostics.Trace.WriteLine(msg);
            sb.AppendLine(msg);
        }
        /*
         <message xmlns="http://schemas.datacontract.org/2004/07/Cic.OpenOne.Common.DTO">
          <code>0</code>
          <detail />
          <duration>127</duration>
          <message />
          <stacktrace i:nil="true" />
          <type>None</type>
        </message>
         * */
    }
}
