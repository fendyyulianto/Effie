<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master" AutoEventWireup="true" CodeFile="JuryPanelList.aspx.cs" Inherits="Admin_JuryPanelList" MaintainScrollPositionOnPostback="true"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="pnPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>



    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="#EEEEEE" Transparency="20" BackgroundPosition="Top">
        <div style="position:relative; top:50%;"><img src="../images/progress.gif" /></div>
    </telerik:RadAjaxLoadingPanel>



<div class="leftContainer">
         
          <h2>Jury Panel - Round <asp:Literal ID="ltRound" runat="server" /></h2>
            <p>&nbsp;</p>

          <asp:Panel ID="pnPanel" runat="server">
            <asp:Repeater ID="rptPanel" runat="server" 
              onitemdatabound="rptPanel_ItemDataBound">
                <ItemTemplate>
                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tbody>
                      <tr>
                        <td style="padding-bottom:10px; width:300px; font-weight: bold;">Jury Panel No:</td>
                        <td style="padding-bottom:10px"><asp:Label ID="lbPanelNo" runat="server" Font-Bold="true"/></td>
                      </tr>
                      <tr>
                        <td style="padding-bottom:10px">No. of judges assigned:</td>
                        <td style="padding-bottom:10px"><asp:Label ID="lbJuryCount" runat="server"/></td>
                      </tr>
                    </tbody>
                   </table>
                   <br />
                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tbody>
                        <tr>
                            <td>
                               <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                  <tbody>
                                      <tr>
                                        <td style="padding-bottom:10px"></td>
                                        <td style="padding-bottom:10px">Categories</td>
                                        <td style="padding-bottom:10px">No. of entries</td>
                                      </tr>
                                      <tr>
                                        <td style="padding-bottom:10px">Category 1:</td>
                                        <td style="padding-bottom:10px"><asp:DropDownList ID="ddlCategory1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged" /></td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCount1" runat="server" Text="0" /></td>
                                      </tr>
                                      <tr>
                                        <td style="padding-bottom:10px">Category 2:</td>
                                        <td style="padding-bottom:10px"><asp:DropDownList ID="ddlCategory2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged"/></td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCount2" runat="server" Text="0" /></td>
                                      </tr>
                                      <tr>
                                        <td style="padding-bottom:10px">Category 3:</td>
                                        <td style="padding-bottom:10px"><asp:DropDownList ID="ddlCategory3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged"/></td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCount3" runat="server" Text="0" /></td>
                                      </tr>
                                      <tr>
                                        <td style="padding-bottom:10px">Category 4:</td>
                                        <td style="padding-bottom:10px"><asp:DropDownList ID="ddlCategory4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged"/></td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCount4" runat="server" Text="0" /></td>
                                      </tr>
                                      <tr>
                                        <td style="padding-bottom:10px">Category 5:</td>
                                        <td style="padding-bottom:10px"><asp:DropDownList ID="ddlCategory5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged"/></td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCount5" runat="server" Text="0" /></td>
                                      </tr>                                      
                                      <tr>
                                        <td style="padding-bottom:10px">Category 6:</td>
                                        <td style="padding-bottom:10px"><asp:DropDownList ID="ddlCategory6" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged"/></td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCount6" runat="server" Text="0" /></td>
                                      </tr>
                                       <tr>
                                        <td style="padding-bottom:10px">Category 7:</td>
                                        <td style="padding-bottom:10px"><asp:DropDownList ID="ddlCategory7" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged"/></td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCount7" runat="server" Text="0" /></td>
                                      </tr>
                                       <tr>
                                        <td style="padding-bottom:10px">Category 8:</td>
                                        <td style="padding-bottom:10px"><asp:DropDownList ID="ddlCategory8" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged"/></td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCount8" runat="server" Text="0" /></td>
                                      </tr>
                                      <tr>
                                        <td style="padding-bottom:10px">&nbsp;</td>
                                        <td style="padding-bottom:10px; text-align:right;">Total:</td>
                                        <td style="padding-bottom:10px; text-align:center;"><asp:Label ID="lbEntriesCountTotal" runat="server" Text="0" /></td>
                                      </tr>
                                  </tbody>
                               </table>
                            </td>
                            <td style="vertical-align:top; padding-left:20px;">
                                <div style="padding-bottom:10px;">Jury List</div>
                                 <telerik:RadGrid ID="radGridJury" runat="server"  Skin="Windows7" Width="300px"
                                    AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false" OnItemDataBound="radGridJury_ItemDataBound" OnItemCommand="radGridJury_ItemCommand">
                                    <PagerStyle AlwaysVisible="false" />
                                    <MasterTableView>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <%# Container.DataSetIndex+1 %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Jury Name" ItemStyle-Width="290px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnJuryName" runat="server" Text="" CommandName="ViewJury" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                 </telerik:RadGrid>
                            </td>
                        </tr>
                      </tbody>
                  </table>
                  <hr />
                </ItemTemplate>
          </asp:Repeater>
          </asp:Panel>


          <asp:Label ID="lbError" runat="server" ForeColor="Red" /><br />

          <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />&nbsp;
          <asp:Button ID="btnConfirm" runat="server" Text="Confirm" onclick="btnConfirm_Click" OnClientClick="return confirm('Confirm to submit jury panels?');"/>&nbsp;
          <br /><br />
         </div>

</asp:Content>

