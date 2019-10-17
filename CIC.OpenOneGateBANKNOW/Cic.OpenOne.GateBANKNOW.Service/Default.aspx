<%@ Page Language="C#" %>
<%@ Import Namespace="Cic.OpenOne.GateBANKNOW.Service" %>
<%@ Import Namespace="Cic.OpenOne.Common.Util.Serialization" %>
<%@ Import Namespace="System.Xml.Serialization" %>
<%@ Import Namespace="CIC.Monitoring.Model" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<%
    var show = Request.QueryString.Get("show");
    bool showBool = false;
    bool.TryParse(show, out showBool);

    var xml = Request.QueryString.Get("xml");
    bool xmlBool = false;
    bool.TryParse(xml, out xmlBool);
    
    var json = Request.QueryString.Get("json");
    bool jsonBool = false;
    bool.TryParse(json, out jsonBool);
    if (xmlBool)
    {
        Response.ClearHeaders();
        Response.AddHeader("content-type", "text/xml");
        XmlSerializer xsSubmit = new XmlSerializer(typeof(CheckResult));
        var subReq = MonitoringCheck.MonitoringService.GetState(showBool);
        var result = "";

        using (var sww = new StringWriter())
        {
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, subReq);
                result = sww.ToString(); // Your XML
            }
        }
        Response.Write(result);
    }
    else if (jsonBool)
    {
        Response.ClearHeaders();
        Response.AddHeader("content-type", "application/json");
        Response.Write(JsonConvert.SerializeObject(MonitoringCheck.MonitoringService.GetState(showBool)));
    }
    else
    {
        Response.Write(MonitoringCheck.MonitoringService.GetStateAsHtmlPage(showBool));
    }
%>