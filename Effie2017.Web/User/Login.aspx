<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPageLogin.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="User_Login" %>

<%@ Register TagPrefix="uc" TagName="usr_Login" Src="~/Controls/usr_Login.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <%--<div>
        <div style=" margin-left: 169px; margin-right: 169px; line-height:21px ">
            <span style="font-weight:bold;">LAST CHANCE TO ENTER <%=GeneralFunction.EffieEventYear() %> APAC EFFIE AWARDS</span> <br /><br />
            <span>Due to overwhelming requests, we are offering a 24 hours extension to the 4th and final deadline to Jan 8, 2019 SGT 9pm.</span>  </span> 
            <br /><br /><span>Material Submission: 14 Jan 2019.</span>
        </div>
        <br/><br/>
    </div>--%>

    <div>
        <div style=" margin-left: 169px; margin-right: 169px; line-height:21px ">
            <%--<span style="font-weight:bold;">Final Material Submission Deadline: 14 January 2019</span>--%> <%--<br /><br />--%>
        </div>
    </div>
    

    <%--<div>
        <div style=" margin-left: 169px; margin-right: 169px; line-height:21px ">
            <span style="font-weight:bold;">The Entry Portal for the <%=GeneralFunction.EffieEventYear() %> APAC Effie Awards will be opened on the 5 November 2018.</span> <br /><br />
            <span>In preparation for your submissions, please download the Entry Kit and Entry Form Templates at <a href="http://www.apaceffie.com/"> www.apaceffie.com</a>.</span>  </span> 
            <br /><br /><span>If you have any questions, please contact <a href="mailto:support.apaceffie@ifektiv.com">support.apaceffie@ifektiv.com</a> or call us at +65 6338 7739.</span>
        </div>
        <br/><br/>
    </div>--%>

    <%--<div>
        <div style=" margin-left: 169px; margin-right: 169px; line-height:21px ">
            <span style="font-weight:bold;">Entry Application for the <%=GeneralFunction.EffieEventYear() %> APAC Effie Awards is now closed. </span> <br /><br />
            <div style="text-align: justify;">As we need to move on to the next stage of judging preparations, you may no longer create new accounts or add new entries. Entrants may continue to login to complete their Entry Materials Submissions bearing in mind the respective submission deadlines.</div>  </span> 
            <br /><span>Thank you for your support!</span>
        </div>
        <br><br>
    </div>--%>
    <%--style="display:none;"--%>
    <div class="signin" >
        
      	<h2>Sign In</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <asp:PlaceHolder ID="phLogin" runat="server">
                <tr>
                    <td width="58%" style="padding-bottom:10px;vertical-align:top">
                        <uc:usr_Login ID="usr_Login" runat="server" loginSuccessRedirection="../Main/Dashboard.aspx"
                            forgetPasswordVisible="true" forgetPasswordRedirection="../User/ForgotPassword.aspx" />
                    </td>
                    <td width="5%" style="padding-bottom:10px; border-left:1px solid #ccc">&nbsp;</td>
                    <td width="37%" style="padding-bottom:10px;vertical-align:top">
                        <br />
                        <asp:Panel ID="panelRegister" runat="server">
                            Not already a Registered User?
                            <br /><br />
                            Sign up an Account here:<br /><br />
                            <asp:Button ID="btnSignup" runat="server" Text="Sign up for an Account" OnClick="btnSignup_OnClick" />
                        </asp:Panel>
                    </td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phCutoff" runat="server" Visible="false">
                <tr style="height: 150px; text-align:center">
                    <td colspan="3">You are unable to login as entry season has ended.</td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td colspan="3" align="center">
                    <br />Best viewed in Chrome, Firefox, IE 9+ and Safari 6+.
                    
                </td>
            </tr>
        </table>
        </div>

</asp:Content>

