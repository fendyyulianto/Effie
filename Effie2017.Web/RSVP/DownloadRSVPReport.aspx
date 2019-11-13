<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownloadRSVPReport.aspx.cs"
    Inherits="RSVP_DownloadRSVPReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>APAC Effie <%=GeneralFunction.EffieEventYear() %></title>
    <link href="../css/style-forms.css" rel="stylesheet" type="text/css" />
    <script>
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>

    <style>        
        body
        {
            font-family:Verdana, Geneva, sans-serif !important;
        }        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="wrapper">
            <div class="masthead">
                <div class="centerLogo">
                    <a href="#"></a>
                </div>
            </div>
            <div class="content">
                <table width="100%">
                    <tr>
                        <td align="center">
                            <h2>
                                <label>
                                    Enter the Security Token to Download the RSVP Report</label></h2>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:TextBox runat="server" ID="txtSecurityCode" Width="280px" autocomplete="off"
                                TextMode="Password" Style="padding: 10px" Font-Size="20px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label runat="server" ID="lblError" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button Text="Download" runat="server" ID="btnDownload" CssClass="submit" 
                                onclick="btnDownload_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
