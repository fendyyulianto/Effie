<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true" 
    CodeFile="EFMultiMarketPDF.aspx.cs" Inherits="Main_EFMultiMarketPDF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <script type="text/javascript">
        setTimeout(function () { location.reload(); }, 3000);
    </script>
     <h1>
     <asp:Label runat="server" Text="Sign Up For An Account" ID="lbSignUpTitle" Visible="false"></asp:Label></h1><br />
      <div style="text-align:center">
           <span style="font-weight:bold">Please wait while we convert your form to PDF.</span>
      </div>
      <br />
      <br />
      <br />
      <br />
</asp:Content>

