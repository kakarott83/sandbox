<%@ Page Language="C#" %>
<%@ Import Namespace="Cic.OpenLease" %>

<%
    var show = Request.QueryString.Get("show");
    bool showBool = false;
    bool.TryParse(show, out showBool);
    Response.Write(Global.MonitoringService.GetStateAsHtmlPage(showBool)); 
%>