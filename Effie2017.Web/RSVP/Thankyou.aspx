﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Thankyou.aspx.cs" Inherits="RSVP_Thankyou" %>

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
                <p class="centerHeader">
                    Thank you for responding to this invitation. <br /><br />
                    If you wish to change your response, please
                    contact us at <a href="mailto:apaceffie@ifektiv.com">apaceffie@ifektiv.com</a>.
                </p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
