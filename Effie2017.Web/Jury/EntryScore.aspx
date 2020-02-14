<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPageJury.master" AutoEventWireup="true"
    CodeFile="EntryScore.aspx.cs" Inherits="Jury_EntryScore" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .rdb td
        {
            width: 300px;
        }
        .cbl td
        {
            width: 10%;
            height: 30px;
        }
        .txt62 input
        {
            width: 62px;
        }
        .txt90 input
        {
            width: 90px;
        }
        .txtlong input
        {
            width: 100%;
        }
        .rdbBlock
        {
            margin-left: 30px;
            width: 100%;
        }
        .rdbBlock input
        {
            margin-left: -20px;
        }
        .rdbBlock td
        {
            padding-bottom: 10px;
        }
        .down11
        {
            margin-bottom: -11px;
        }
    </style>
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2>
        Scoresheet</h2>
    <p>
        &nbsp;</p>
    <%--      <p>
      Please review the Entry Form and Creative Material(s) to score the entry. To view the materials, click on the icons. <span style="text-decoration:underline" >You are advised to review the Entry Form first before the Creative Materials.</span> You may save your Scoresheet at any time before finally submitting the results.  
      </p>
    --%>
    <p>
        Click on the icon for the Entry Form, Creative Materials and Translations for Creative Materials, if applicable.
    </p>
    <br />
    <p style="font-weight: bold;">
        As a reminder, please review the written Entry Form <span style="text-decoration:underline">BEFORE</span> reviewing the Creative Materials.</p>
    <br />
    <p>
        Please enter your initial scores, and save them as drafts. When you ready to confirm the scores, please go back to your Jury Dashboard (Entries Overview), click <span style="font-weight: bold;">Select All</span>, followed by <span style="font-weight: bold;">Submit</span>.</p>
    <br />
    <hr />
    <p>
        *required fields</p>
    <br />
    <div class="leftContainer">
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
                        Scoring Status:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbScoreCompletion" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <div class="leftContainer">
        <script>
            $('#form1').on('keyup keypress', function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });
        </script>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 200px">
                        Entry Id:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbEntryId" runat="server" />
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Title:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbTitle" runat="server" />
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Brand:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbBrand" runat="server" />
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Category:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbCategory" runat="server" />
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Materials:
                    </td>
                    <td style="padding-bottom: 10px">
                        <span style="font-weight: bold">Entry Form</span>
                        <asp:HyperLink ID="lnkEntryFormPDF" runat="server" ImageUrl="../images/btn-download-pdf.png"
                            ToolTip="Download Pdf" Target="_blank" Visible="false" />&nbsp;<asp:HyperLink ID="lnkEntryFormDOC"
                                runat="server" Text="Word" Target="_blank" Visible="false" />
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                </tr>
                <!-- tr>
                <td style="padding-bottom:10px"></td>
                <td style="padding-bottom:10px">Authorization Form <asp:HyperLink ID="lnkAutPDF" runat="server" Text="Pdf" Target="_blank" Visible="false" /></td>
                <td style="padding-bottom:10px"></td>
                <td style="padding-bottom:10px"></td>
              </tr -->
                <!-- tr>
                <td style="padding-bottom:10px"></td>
                <td style="padding-bottom:10px">Case Image <asp:HyperLink ID="lnkCaseImage" runat="server" Text="Jpg" Target="_blank" Visible="false" /></td>
                <td style="padding-bottom:10px"></td>
                <td style="padding-bottom:10px"></td>
              </tr -->
                <asp:PlaceHolder ID="phCreativeMaterials" runat="server">
                    <tr>
                        <td style="padding-bottom: 10px">
                        </td>
                        <td style="padding-bottom: 10px">
                            <span style="font-weight: bold">Creative Material(s)</span>
                            <asp:HyperLink ID="lnkCreativePDF" runat="server" ImageUrl="../images/btn-download-pdf.png"
                                ToolTip="Download Pdf" Target="_blank" Visible="false" />
                            &nbsp;
                            <asp:HyperLink ID="lnkCreativeVid" runat="server" ImageUrl="../images/btn-download-mov.png"
                                ToolTip="Download Video" Target="_blank" Visible="false" />
                            <asp:HyperLink ID="lnkCreativePDFTranslate" runat="server" ImageUrl="../images/btn-download-pdf.png"
                                ToolTip="Download Translation" Target="_blank" Visible="false" />
                            <%--<asp:ImageButton ID="btnCreativeVid" runat="server" ImageUrl="../images/btn-download-mov.png" ToolTip="Download Video" Target="_blank" Visible="false" onclick="btnCreativeVid_Click" CssClass="down11" /></td>--%>
                            <td style="padding-bottom: 10px">
                            </td>
                            <td style="padding-bottom: 10px">
                            </td>
                    </tr>
                </asp:PlaceHolder>
        </table>
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <p style="font-weight: bold;">
        Please recuse yourself from this entry if it is from the agency or company where
        you work, you have worked directly on the case or the case represents direct competitive
        work to a brand you work directly on.
    </p>
    <br />
    <div class="leftContainer">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 300px">
                        Recuse:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlRecuse" runat="server" Style="width: 234px; font-size: 15px;"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlRecuse_SelectedIndexChanged">
                            <asp:ListItem Value="No" Text="No" />
                            <asp:ListItem Value="Yes" Text="Yes" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 300px">
                        If Yes, state reason:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtRecuseRemarks" runat="server" TextMode="MultiLine" Font-Size="15px" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:PlaceHolder ID="phFlag" runat="server">
            <br />
            <p>
                Use this flag for disqualification, or if you feel the case does not meet the category definition.  Score as per normal and the Judging Committee will revew the case.</p>
            <br />
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td style="padding-bottom: 10px; width: 300px">
                            Flag:
                        </td>
                        <td style="padding-bottom: 10px">
                            <asp:DropDownList ID="ddlFlag" runat="server" Style="width: 234px; font-size: 15px;">
                                <asp:ListItem Value="" Text="Please select" />
                                <asp:ListItem Value="DQ" Text="Disqualification" />
                                <asp:ListItem Value="Wrong category" Text="Wrong category" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-bottom: 10px; width: 300px">
                            Flag Reason:
                        </td>
                        <td style="padding-bottom: 10px">
                            <span class="txtlong">
                                <asp:TextBox ID="txtFlagOthers" runat="server" MaxLength="100" Font-Size="15px" /></span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:PlaceHolder>
    </div>
    <div style="clear: both;">
    </div>
    <hr />
    <div class="leftContainer">
        For each entry, please score whole numbers <span
            style="text-decoration: underline;font-weight: bold;">10-100</span> for each criteria, 100 being the
            maximum score you can provide.
        <br />
        <br />
        <table width="100%" cellspacing="0" cellpadding="0" class="tableScore">
            <tbody>
                <tr>
                    <td width="30%" align="center">
                       Judging Criteria
                    </td>
                    <td align="center">
                        Scores
                    </td>
                    <td align="center">
                        Weightage
                    </td>
                    <td align="center" >
                        Total<br /> 
                        Composite <br /> 
                        Scores
                    </td>
                </tr>
                <tr>
                    <td>
                        Strategic Challenge & Objectives*:
                    </td>
                    <td align="center">
                        <span class="txt62">
                            <asp:TextBox ID="txtSC" MaxLength="3" runat="server" Text="0"  autocomplete="off" onkeypress="return isNumberKey(event)"></asp:TextBox> /100</span> 
                    </td>
                    <td align="center">
                        <asp:Label ID="lbWeightSC" runat="server"  />
                    </td>
                    <td align="center">
                        
                            <asp:Label ID="lbCScoreSC" runat="server" Text="0"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        Idea*:
                    </td>
                    <td align="center">
                        <span class="txt62">
                            <asp:TextBox ID="txtID" MaxLength="3" runat="server" Text="0"  autocomplete="off" onkeypress="return isNumberKey(event)"></asp:TextBox> /100</span> 
                    </td>
                    <td align="center">
                        <asp:Label ID="lbWeightID" runat="server"  />
                    </td>
                    <td align="center">                        
                            <asp:Label ID="lbCScoreID" runat="server" Text="0"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        Bringing Idea to Life*:
                    </td>
                    <td align="center">
                        <span class="txt62">
                            <asp:TextBox ID="txtIL" MaxLength="3" runat="server" Text="0" autocomplete="off" onkeypress="return isNumberKey(event)"></asp:TextBox> /100</span> 
                    </td>
                    <td align="center">
                        <asp:Label ID="lbWeightIL" runat="server"  />
                    </td>
                    <td align="center">                        
                             <asp:Label ID="lbCScoreIL" runat="server" Text="0"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        Results*:
                    </td>
                    <td align="center">
                        <span class="txt62">
                            <asp:TextBox ID="txtRE" MaxLength="3" runat="server" Text="0" autocomplete="off" onkeypress="return isNumberKey(event)"></asp:TextBox> /100</span> 

                            

                    </td>
                    <td align="center">
                        <asp:Label ID="lbWeightRE" runat="server"  />
                    </td>
                    <td align="center">                        
                            <asp:Label ID="lbCScoreRE" runat="server" Text="0"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;<br />
                        &nbsp;<br />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td align="center">
                        
                            <asp:Label ID="lbCScoreTotal" runat="server" Font-Bold="true" Text="0"  />
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
        <br />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr id="trRecommendation" runat="server">
                    <td style="padding-bottom: 10px; width: 300px">
                        <asp:Label ID="lbRecommendation" runat="server" Text="Advancement?*" />
                        <br />
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlRecommendation" runat="server" Style="width: 234px; font-size: 15px;">
                            <asp:ListItem Value="" Text="Please select" />
                            <asp:ListItem Value="Yes" Text="Yes" />
                            <asp:ListItem Value="No" Text="No" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="AdvancementFlag" runat="server">
                    <td style="padding-bottom: 10px; width: 300px">
                        <asp:Label ID="Label1" runat="server" Text="Recommendations for finalist and winners?*" />
                        <br />
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlNomination" runat="server" Style="width: 234px; font-size: 15px;">
                            <asp:ListItem Value="" Text="Please select" />
                            <asp:ListItem Value="GrandEffie" Text="Grand Effie" />
                            <asp:ListItem Value="Gold" Text="Gold" />
                            <asp:ListItem Value="Silver" Text="Silver" />
                            <asp:ListItem Value="Bronze" Text="Bronze" />
                            <asp:ListItem Value="Finalist" Text="Finalist" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    </div>
    <div style="clear: both">
    </div>
    <p style="font-weight:bold;">If you flag for advancement into Round 2, please ensure that your scores reflect your intent.</p><br />
    <hr />    
    <div class="leftContainer">
    <p>In addition to your scores, please provide feedback on each case to further explain your scoring. It also offers an opportunity to provide anonymous feedback to the entrant. Please comment on the strongest and weakest elements of the case. </p>
    <br />
    <br />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 300px">
                        What was the strongest element of the case and why?:<br />
                    </td>
                    <td style="padding-bottom: 10px; width: 630px">
                        <asp:TextBox ID="txtFeedbackStrong" runat="server" TextMode="MultiLine" Font-Size="16px" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 300px">
                        What was the weakest element of the case and why?:<br />
                    </td>
                    <td style="padding-bottom: 10px; width: 630px">
                        <asp:TextBox ID="txtFeedbackWeak" runat="server" TextMode="MultiLine" Font-Size="16px" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 300px">
                        Do you have any advice or additional comments for the entrants?:<br />
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtAdditionalComments" runat="server" TextMode="MultiLine" Font-Size="16px" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="clear: both;">
    </div>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <div style="clear: both;">
    </div>
    <div style="text-align: center">
        <table width="100%">
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btnBack" runat="server" Text="Back to Entries Overview" OnClick="btnBack_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  Visible="false"/>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSaveDraft" runat="server" Text="Save As Draft" OnClick="btnSaveDraft_Click" />
                </td>
                <%-- <td>
                </td>
                <td style="text-align: left; width: 40%">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Results" OnClick="btnSubmit_Click" Visible="false"
                        OnClientClick="return confirm('Confirm to submit scores?');" />
                </td>--%>
            </tr>
        </table>
    </div>
    <script>
        function OnChangeddlRecom(AdvancementFlagid, ddlRecommendationId, round) {
            ddlRecommendationIdVal = $("#" + ddlRecommendationId + "").val();
            $("#" + AdvancementFlagid + "").hide();
            if (ddlRecommendationIdVal == "Yes" && round == "2")
            {
                $("#" + AdvancementFlagid + "").show();
            }
        }
    </script>
</asp:Content>
