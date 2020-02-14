<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPageJury.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Jury_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function CheckAll(id) {
            var masterTable = $find("<%= radGridEntry.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    var checkBox = masterTable.get_dataItems()[i].findElement("chkbox");
                    if (checkBox != null)
                        checkBox.checked = true;  // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    var checkBox = masterTable.get_dataItems()[i].findElement("chkbox");
                    if (checkBox != null)
                        checkBox.checked = false; // for unchecking the checkboxes
                }
            }
        }
    </script>
    
    <style>

        #popup {
            opacity: 1;
            width: 800px;
            height: 550px;
            padding: 25px;
            margin-left: 115px;
            top: 0;
            margin-top: 0px;
            position: fixed;
            border: 6px solid #bbbbbb;
            background-color: #fff;
            z-index: 100;
            overflow-y: scroll;
        }

        .overlay {
            background-color: #000;
            position: fixed;
            top: 0;
            right: 0;
            min-height: 935px;
            min-width: 1800px;
            opacity: 0.5;
            filter: alpha(opacity=50);
        }

        a.tooltip span {
            width: 200px;
        }

        .ModalPopUpSmall {
            opacity: 1;
            width: 800px;
            height: 290px;
            padding: 25px;
            margin-left: 115px;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            margin: auto;
            position: fixed;
            border: 6px solid #bbbbbb;
            background-color: #fff;
            z-index: 1002;
            overflow-y: scroll;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    
    <asp:PlaceHolder ID="phRecuseEntry" runat="server" Visible="false">
        <div class="ModalPopUpSmall">
            <h2>
                <asp:Label runat="server" ID="Label4">Recuse Entry</asp:Label></h2>
            <hr />
            <br />
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 300px">
                        Recuse:
                    </td>
                    <%--OnSelectedIndexChanged="ddlRecuse_SelectedIndexChanged" AutoPostBack="True" --%>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlRecuse" runat="server" Style="width: 234px; font-size: 15px;">
                            <asp:ListItem Value="No" Text="No" />
                            <asp:ListItem Value="Yes" Text="Yes" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 25%;">
                        If Yes, state reason:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtRecuseRemarks" runat="server" TextMode="MultiLine" Font-Size="15px" />
                        <asp:HiddenField ID="txtEntryId" runat="server" />
                    </td>
                </tr>
            </tbody>
            </table>
            <br />
            <br />
            <asp:Button runat="server" ID="Button5" Text="Submit" OnClick="btnSubmitRecuseEntry_Click" />
            &nbsp;
             <asp:Button runat="server" ID="Button4" Text="Close" OnClick="btnClose_Click" />
        </div>
        <div class="overlay">
        </div>
    </asp:PlaceHolder>


    <h2>
        Entries Overview</h2>
    <br />
    <p style="text-decoration: underline;">
        Important Reminders:</p>
    <p>
        &nbsp;</p>
    <p style="font-weight: bold;">
        To be considered for an APAC Effie Award, campaigns must offer proof of having met or exceeded clearly stated objectives. Judging procedures are primarily based on objective evidence of performance against goals.</p>
    <br />
    <p>
        On the menu bar above, you will find links to the Judging Guide, Category Definitions, Scoring Scale, Reasons for DQ and FAQ.</p>
    <br />
    <p>
        Please ensure that you go through the Judging Guide in it’s entirety before you commence judging, and take your time for each case.
    </p>
    <br />    
    <div>
        <span style="font-weight: bold;">RECUSAL POLICY:</span> Please review the list of entries that are assigned to you and RECUSE yourself if one or more of the following is true: <br/>
 
        <ul style="margin-left: 15px;">
            <li>If it is from the agency or company where you work.</li>
            <li>You have worked directly on the case, or </li>
            <li>The case represents direct competitive work to a brand you work directly on.</li>
        </ul>

        <br/>

        <span style="font-weight: bold;">To begin, click on the Score button to review all the materials and score the entry. </span> 

        <br/><br/>

        Your initial scores will be saved as drafts to allow for calibrations and adjustments as you move along. When you are ready to confirm the scores, 
        you may submit the cases individually or en-mass by clicking the <span style="font-weight: bold;">Select All</span> button on the left of the table, and clicking the <span style="font-weight: bold;">Submit Score(s) on this page.</span> 




      <%--  <span style="font-weight: bold; text-decoration: underline;">Click on the Score button to review all the materials and score the entry.</span>  Your initial scores will be saved as drafts to allow for calibrations and adjustments as you move along. When you are ready to confirm the scores, you may submit the cases individually or en-mass by clicking the <span style="font-weight: bold;">Select All</span> button on the left of the table.--%>

    </div>
    <br />
    <%--<p>
        If you need any assistance at any time, please do not hesitate to reach out to the APAC Effie team.<br /><br />
        Email: <a href="mailto:judging.apaceffie@ifektiv.com">judging.apaceffie@ifektiv.com</a><br />
        Office Phone: +65 6338 7739<br />
        Vanessa TAY: Mobile +65 9710 9606 / Jessica WONG: +65 9734 9101 
    </p><br />--%>
    
    <br />
    <div style="clear: both;">
    </div>
    <div style="float: left;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 200px">
                        Judge Id:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbJuryId" runat="server" Font-Bold="true" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Judge Name:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbJuryName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Company:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbCompany" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Judging Round:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbRound" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Jury Panel:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbPanel" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Scoring Completion:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbScoreCompletion" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="margin-left: 430px;">
        <table width="500px" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 50px; text-decoration: underline;">
                        Legend
                    </td>
                    <td style="padding-bottom: 10px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 50px">
                        SC
                    </td>
                    <td style="padding-bottom: 10px">
                        Strategic Challenge & Objectives (23.33%)
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        ID
                    </td>
                    <td style="padding-bottom: 10px">
                        Idea (23.33%)
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        IL
                    </td>
                    <td style="padding-bottom: 10px">
                        Bringing Idea to Life (23.33%)
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        RE
                    </td>
                    <td style="padding-bottom: 10px">
                        Results (30%)
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="clear: both;">
    </div>
    <div style="padding-right: 85%;">
        <asp:Button runat="server" ID="btnSubmitScore" Text="Submit Score(s)" OnClientClick="return confirm('Confirm to submit score for all entry(s)?');"
            OnClick="btnSubmitScore_Click" style="font-size: 15px;"/>
    </div>
    <div style="clear: both">
    </div>
    <br />
    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="Windows7" AutoGenerateColumns="false"
        AllowPaging="true" PageSize="50" AllowSorting="true" OnItemDataBound="radGridEntry_ItemDataBound"
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView TableLayout="Auto">
            <Columns>
                <telerik:GridTemplateColumn ItemStyle-Width="70px">
                    <HeaderTemplate>
                        <center>
                            Select All
                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" /></center>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <center>
                            <asp:CheckBox runat="server" ID="chkbox" /></center>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Id" Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdfId" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="10px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Entry Id" ItemStyle-Width="60px">
                    <ItemTemplate>
                        <asp:Label ID="lbSerial" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Entry Title" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lbCampaign" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Client" ItemStyle-Width="150px" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lbClient" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Brand" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lbBrand" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Category" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lbCategory" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Entrant" ItemStyle-Width="100px" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lnkBtnBuSubmittedBy" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="SC" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label ID="lbScoreSC" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="ID" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label ID="lbScoreID" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="IL" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label ID="lbScoreIL" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="RE" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label ID="lbScoreRE" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Comp. Score" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label ID="lbScoreComposite" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Scoring Status" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Label ID="lbScoreStatus" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jury Flag" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryFlag" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--                <telerik:GridTemplateColumn HeaderText="Jury Recuse" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryRecuse" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkScore" runat="server" Text="Score" CssClass="tblLinkBlack"
                            CommandName="Score"></asp:LinkButton>
                         <asp:LinkButton ID="lnkRecuse" runat="server" Text="Recuse" CssClass="tblLinkBlack"
                            CommandName="Recuse" OnClientClick="return confirm('Confirm to recuse?');"></asp:LinkButton>&nbsp;

                        <asp:LinkButton ID="LnkUnrecuse" runat="server" Text="Unrecuse" CssClass="tblLinkBlack" CommandName="Unrecuse"  Visible="false"></asp:LinkButton> <%--OnClientClick="return confirm('Confirm to unrecuse?');"--%>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
</asp:Content>
