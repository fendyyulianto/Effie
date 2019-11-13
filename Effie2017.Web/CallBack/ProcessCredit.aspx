<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProcessCredit.aspx.cs" Inherits="Order_ProcessCredit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <p>
        Thank you for your submission.</p>
    <p>
        An email confirmation will be sent to your registered email address.</p>
    <br />
    <br />
    <br />
    <p>
        <asp:Button ID="btnAddEntry" runat="server" OnClick="btnAddEntry_Click" Text="Add New Entry"
            Width="130px" />
        &nbsp;
        <asp:Button ID="btnDashboard" runat="server" OnClick="btnDashboard_Click" Text="Entries Overview"
            Width="130px" /></p>
</asp:Content>
