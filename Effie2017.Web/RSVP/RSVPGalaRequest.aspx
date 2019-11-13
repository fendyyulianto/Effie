<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RSVPGalaRequest.aspx.cs"
    Inherits="RSVP_RSVPGalaRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>APAC Effie <%=GeneralFunction.EffieEventYear() %></title>
    <link href="../css/style-forms.css" rel="stylesheet" type="text/css" />
    <style>
        body
        {
            font-family: Verdana, Geneva, sans-serif !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="wrapper">
            <div class="content">
                <table width="597" style="margin-left: 20%;">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                   <tr>
                        <td>
                            <p>
                                Dear
                                <asp:Label runat="server" ID="lblName"></asp:Label>,</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="justify">
                            <p>
                                With great pleasure, we invite you to <strong>join us at the <%=GeneralFunction.EffieEventYear() %> APAC Effie Awards Gala</strong> as we celebrate and honour the most effective work at the <strong>Four Seasons Hotel Singapore</strong> on <strong>April 25th</strong>.<br /><br />

                                Please refer to the invite below for details and remember to RSVP by April 16th.<br /><br />

                                See you there!
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <img src="../images/gala.jpg?v=2" />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">                          
                            <asp:RadioButtonList ID="rblstGalaDinner" runat="server" CssClass="rblRSVP" RepeatLayout="UnorderedList"
                                Style="margin-left: 24%;">
                                <asp:ListItem Text="Yes, I will attend" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No, I can’t make it" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <p>
                                Do you have any meal restrictions?</p>
                            <p>
                                &nbsp;</p>
                            <asp:TextBox runat="server" ID="txtDietery" TextMode="MultiLine" MaxLength="2000"
                                Style="resize: none;">
                            </asp:TextBox>
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
                            <asp:Button runat="server" ID="btnSubmit" Text="SUBMIT" CssClass="btnRSVP" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
