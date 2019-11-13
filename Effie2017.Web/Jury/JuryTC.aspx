<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPageJuryLogin.master"
    AutoEventWireup="true" CodeFile="JuryTC.aspx.cs" Inherits="Jury_JuryTC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .chkBig input {
            width:20px;height:20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2>
        Welcome to APAC Effie Awards <%=GeneralFunction.EffieEventYear() %> Judging - Round One.
    </h2>
    <br />
    <p>
        The content you will be reviewing during the judging session is privileged information.
        Before you begin, we require all judges to observe and agree to APAC Effie Awards
        strict rules on confidentiality.</p>
    <br />
    <p style="font-weight: bold; text-align: center;">
        STATEMENT OF CONFIDENTIALITY</p>
    <br />
    <p>
        I agree to keep confidential any and all information I receive in my role as a judge
        for APAC Effie Awards. I agree to:
        <br />
        <br />
        <ul style="margin-left: 15px;">
            <li>Respect the APAC Effie Awards strict policy on confidentiality. </li>
            <li>Not download, circulate or pass on any materials, notes or other information that
                I judge or learn about during the judging process. </li>
            <li>Refrain from discussing the contents of any entry I review.</li>
            <li>Keep the content of the cases reviewed confidential to the judging session. Specific
                case materials or judging session activity should not be discussed or written about
                outside of the sessions or with media representatives, etc.</li>
        </ul>
    </p>
    <br />
    <br />
    <table><tr><td>
        <asp:CheckBox ID="chkOK" runat="server" CssClass="chkBig" /></td><td>I agree to abide by both the spirit
        and the letter of this statement.
    </td></tr></table>
    <br />
    <asp:Label ID="lbError" runat="server" ForeColor="Red" />
    <br />
    <asp:Button ID="btnSubmit" runat="server" Text="Proceed" OnClick="btnSubmit_Click" />
</asp:Content>
