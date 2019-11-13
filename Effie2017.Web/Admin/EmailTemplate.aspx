
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailTemplate.aspx.cs"
    Inherits="Admin_EmailTemplate" MasterPageFile="~/Admin/MasterPageAdmin.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- InstanceBeginEditable name="content" -->
    <h2>
        <div style="float: left">
            <h1>
                <asp:Label runat="server" ID="lbTitle" Text=""></asp:Label></h1>
        </div>
    </h2>
    <div style="clear: both">
    </div>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <hr />
    <p>
        *required fields</p>
    <br />
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr runat="server" id="templateRow" visible="false">
                    <td width="20%" valign="top">
                        Choose Default Template*:
                    </td>
                    <td style="padding-bottom: 10px" valign="top">
                        <asp:DropDownList runat="server" ID="ddlTemplateList" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:HiddenField runat="server" ID="hdfRounds" />
                        <asp:HiddenField runat="server" ID="hdfEmailCategory" />
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td valign="top" width="20%">
                        Template Name*:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtTemplateName" MaxLength="100" runat="server" Width="300px"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px" valign="top">
                        Subject*:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtTemplateSubject" MaxLength="200" runat="server" Width="450px"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px" valign="top">
                        Email Type*:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlEmailType" runat="server">
                        </asp:DropDownList>      
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                        <td style="padding-bottom: 10px; vertical-align: top;" valign="top">Body*:
                        </td>
                        <td style="padding-bottom: 10px"></td>
                        <td valign="top"></td>
                </tr>
					<tr>
                        <td style="padding-bottom: 10px; vertical-align: top;" valign="top" colspan="3">
                            <telerik:RadEditor ID="rEditorBody" runat="server" Width="100%" Height="400px" ToolsFile="~/css/ToolsFile.xml"
		                        EditModes="Design" AutoResizeHeight="false" ContentAreaMode="Iframe">
		                        <CssFiles>
		                            <telerik:EditorCssFile Value="~/css/main.css" />
		                        </CssFiles>
		                    </telerik:RadEditor>
                        </td>
                    </tr>	
				<tr>
                     <td style="padding-bottom: 10px; vertical-align: top;" valign="top">Placeholder:
                     </td>
                     <td style="padding-bottom: 10px">
                         <table class="tabledata" runat="server" id="Table1">
                             <tr>
                                 <th>Placeholder
                                 </th>
                                 <th>Description
                                 </th>
                             </tr>
                             <tr>
                                 <td>#FIRSTNAME#
                                 </td>
                                 <td>First Name
                                 </td>
                             </tr>
                             <tr>
                                 <td>#LASTNAME#
                                 </td>
                                 <td>Last Name
                                 </td>
                             </tr>
                             <tr>
                                 <td>#ENTRYID#
                                 </td>
                                 <td>Entry ID
                                 </td>
                             </tr>
                             <tr>
                                 <td>#INVOICE#
                                 </td>
                                 <td>Invoice
                                 </td>
                             </tr>
                             <tr>
                                 <td>#FLAGREASON#
                                 </td>
                                 <td>Flag Reason(s)
                                 </td>
                             </tr>
                             <tr>
                                 <td>#ENTRYDATA#
                                 </td>
                                 <td>Entry Data(s)
                                 </td>
                             </tr>
                         </table>
                     </td>
                     <td valign="top"></td>
                </tr>	
                <tr>
                    <td style="padding-bottom: 10px" valign="top">
                        Is Active:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:CheckBox runat="server" ID="chkActive" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr style="display:none;">
                    <td style="padding-bottom: 10px" valign="top">
                        Is Invitation Email:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:CheckBox runat="server" ID="chkInvitation" />
                    </td>
                    <td>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    </div>
    <div style="text-align: center">
        <table width="100%">
            <tr>
                <td style="text-align: right">
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                </td>
                <td>
                </td>
                <td style="text-align: left">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
