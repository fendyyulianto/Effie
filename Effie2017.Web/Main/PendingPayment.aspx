<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true" CodeFile="PendingPayment.aspx.cs" Inherits="Main_PendingPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


   <p>Thank you for your submission.</p>
 
   <p>An email confirmation will be sent to your registered email address.</p>

   <br /><br /><br />
   <p><asp:Button ID="btnAddEntry" runat="server" onclick="btnAddEntry_Click" Text="Add New Entry" Width="130px" />&nbsp;
   <asp:Button ID="btnDashboard" runat="server" onclick="btnDashboard_Click" Text="Entries Overview" Width="130px" /></p>
</asp:Content>

