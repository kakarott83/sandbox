<%@ Page Language="C#" %>
<%@ Import Namespace="Cic.OpenOne.GateBANKNOW.Service" %>

<%
    var show = Request.QueryString.Get("show");
    bool showBool = false;
    bool.TryParse(show, out showBool);

    Response.Write(MonitoringCheck.MonitoringService.GetStateAsHtmlPage(showBool)); 
%>
