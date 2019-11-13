<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportRSVPList.aspx.cs" Inherits="RSVP_ImportRSVPList" MasterPageFile="~/Common/MasterPage.master" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="gen_UploadFile" Src="~/Controls/gen_UploadFile.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <h2>
        Import RSVP List
    </h2>
    <hr />
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>    
    <div id="dvUpload">
        <h2>
            <asp:Label ID="lblTitle" runat="server"></asp:Label></h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="82%">
                    <asp:Label ID="lblDesc" runat="server"></asp:Label>
                </td>
                <td width="18%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <uc:gen_UploadFile ID="ucGen_UploadFileForImport" runat="server" isRequired="true"
            replaceFile="true" createDirectory="false" />
        <br />
        <br />       
        <asp:Button ID="btnSubmit" runat="server" Text="Upload" OnClick="btnSubmit_Click" />
    </div>
</asp:Content>
