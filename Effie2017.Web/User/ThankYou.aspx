<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true" CodeFile="ThankYou.aspx.cs" Inherits="User_ThankYou" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h1>
    <asp:Label runat="server" Text="Sign Up For An Account" ID="lbSignUpTitle" Visible="false"></asp:Label></h1><br />
      <p>Thank you signing up.</p><br />
      <p>You will receive a welcome email sent to <asp:HyperLink ID="hlEmail" runat="server"></asp:HyperLink> shortly. 
          Please click on the authentication link to activate your account. 
          Once it has been activated, you will be able to login to submit your entries.</p><br />
      <p>If you do not receive the welcome email within 15 minutes upon signing up, 
          please check your spam folder or contact us at <a href="mailto:support.apaceffie@ifektiv.com">support.apaceffie@ifektiv.com</a>.</p>
      <br />
      <br />
      <br />
      <br />
    <div style="text-align:center"> <asp:Button ID="btnSubmit" runat="server" Text="Back to HomePage" onclick="btnSubmit_Click"/></div>
</asp:Content>

