<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashReport.aspx.cs" Inherits="Ats.Reports.DashReport" %>
<%@Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" /> 
    <title></title>
    <style type="text/css">
        #form1 {
            height: 488px;
            width: 1263px;
        }
    </style>
</head>
<body style="height: 198px">
    <form id="form1" runat="server">     
      <asp:ScriptManager runat="server"></asp:ScriptManager>        
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" Height="447px" style="margin-left: 0px" Width="1244px">
        </rsweb:ReportViewer>            
    </form>
</body>
</html>
