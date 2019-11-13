<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RSVPRequest.aspx.cs" Inherits="RSVP_RSVPRequest" %>

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
            <div class="masthead">
                <div class="centerLogo">
                    <a href="#"></a>
                </div>
                <p class="centerHeader">
                    You are cordially invited to join us for the</p>
            </div>
            <div class="content">
                <asp:Panel runat="server" ID="pnlLocal" Visible="false">
                    <table width="100%">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                APAC EFFIE JURY COCKTAIL
                                <br />
                                22 February 2018 | 1800 - 2000<br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <img src="../images/RSVP/cocktail.jpg" /><br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                THE ORIENTAL OUTPOST @ China Square (FOLKS COLLECTIVE)<br />
                                25 Church Street, #01-04, Capital Square Three
                                <br />
                                Singapore 049482
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:RadioButtonList ID="rblstCocktailLocal" runat="server" CssClass="rblRSVP" RepeatLayout="UnorderedList"
                                    Style="margin-left: 34%;">
                                    <asp:ListItem Text="Yes, I will attend" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No, I can’t make it" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlOverseas" Visible="false">
                    <table width="100%">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                APAC EFFIE JURY COCKTAIL
                                <br />
                                22 February 2018 | 1800 - 2000<br />
                                <br />
                            </td>
                            <td align="center" valign="top">
                                WELCOME DINNER
                                <br />
                                22 February 2018 | 2000 - 2200<br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                <img src="../images/RSVP/cocktail.jpg" /><br />
                                <br />
                            </td>
                            <td align="center" valign="top">
                                <img src="../images/RSVP/galadinner.jpg" /><br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                Cocktail Venue:<br />
                                THE ORIENTAL OUTPOST @
                                <br />
                                China Square (FOLKS COLLECTIVE)<br />
                                25 Church Street, #01-04,
                                <br />
                                Capital Square Three
                                <br />
                                Singapore 049482
                            </td>
                            <td align="center" valign="top">
                                Welcome Dinner Venue:
                                <br />
                                Tong Le Private Dining
                                <br />
                                60 Collyer Quay, OUE Tower, Level 10, OUE
                                <br />
                                Tower, 049322
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                <asp:RadioButtonList ID="rblstCocktailOverseas" runat="server" CssClass="rblRSVP"
                                    Style="margin-left: 13%;" RepeatLayout="UnorderedList">
                                    <asp:ListItem Text="Yes, I will attend" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No, I can’t make it" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td align="center" valign="top">
                                <asp:RadioButtonList ID="rblstGalaOverseas" runat="server" CssClass="rblRSVP" RepeatLayout="UnorderedList"
                                    Style="margin-left: 13%;">
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
                            <td align="left" colspan="2">
                                <p>
                                    Do you have any meal restrictions?</p>
                                <p>
                                    &nbsp;</p>
                                <asp:TextBox runat="server" ID="txtDietery" TextMode="MultiLine" MaxLength="2000" Style="resize:none;">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table width="100%">                    
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
