<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadForCr.aspx.cs" Inherits="Main_UploadForCr" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="gen_UploadFile" Src="~/Controls/gen_UploadFile.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:Literal ID="ltrForCompatibility" runat="server"></asp:Literal>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Add New Entry</title>
    <link href="../css/style-forms.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        function HideDvUpload() {
            $('#dvUpload').slideUp({ duration: 700, complete: function () { $('#btnSubmit').click(); } });
            return false;
        }

        function ShowHideDvUpload() {
            $('#dvUpload0').slideUp({ duration: 700 });
            $('#dvUpload1').slideUp({ duration: 700 });
            $('#dvUpload2').slideUp({ duration: 700 });

            $('#dvRequired0').css("display", "none");
            $('#dvRequired1').css("display", "none");
            $('#dvRequired2').css("display", "none");

            if ($('#rdCreativeUploadType0').is(':checked')) {
                $('#dvUpload0').slideDown({ duration: 700, complete: function () { $('#dvRequired0').css("display", "inline") } });
            }
            else if ($('#rdCreativeUploadType1').is(':checked')) {
                $('#dvUpload1').slideDown({ duration: 700, complete: function () { $('#dvRequired1').css("display", "inline") } });
            }
            else if ($('#rdCreativeUploadType2').is(':checked')) {
                $('#dvUpload2').slideDown({ duration: 700, complete: function () { $('#dvRequired2').css("display", "inline") } });
            }
            return false;
        }

        function FancyClose() {
            window.parent.CloseFancyBox();
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="radScrptMgr" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadProgressManager ID="radProgMgr" runat="server" />
        <div id="dvUpload">
            <h2>
                <asp:Label ID="lblTitle" runat="server"></asp:Label></h2>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="82%">
                        <asp:Label ID="lblDesc" runat="server"></asp:Label><br />
                    </td>
                    <td width="18%">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <asp:RadioButton ID="rdCreativeUploadType0" runat="server" Text="Creative Showcase in PDF only"
                            GroupName="grpCreativeUploadType" onclick="ShowHideDvUpload();" />
                    </td>
                </tr>
            </table>
            <div id="dvUpload0" style="display: none; margin-left: 30px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="82%">
                            <asp:Label ID="lblTitle1" runat="server" Style="display: block; padding-top: 7px;
                                padding-bottom: 7px;"></asp:Label>
                        </td>
                        <td width="18%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <uc:gen_UploadFile ID="ucGen_UploadFileForAll1" runat="server" isRequired="true"
                    replaceFile="true" createDirectory="true" />
                <div id="dvRequired0" style="display: none">
                    <div style="position: absolute; margin-top: -20px;">
                        <span style="font-size: 12px;">* file is required</span>
                    </div>
                </div>
                <uc:gen_UploadFile ID="ucGen_UploadFileForAll1Trans" runat="server" isRequired="false"
                    replaceFile="true" createDirectory="true" />
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <asp:RadioButton ID="rdCreativeUploadType1" runat="server" Text="4 min Creative Video only"
                            GroupName="grpCreativeUploadType" onclick="ShowHideDvUpload();" />
                    </td>
                </tr>
            </table>
            <div id="dvUpload1" style="display: none; margin-left: 30px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="82%">
                            <asp:Label ID="lblTitle2" runat="server" Style="display: block; padding-top: 7px;
                                padding-bottom: 7px;"></asp:Label>
                        </td>
                        <td width="18%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <uc:gen_UploadFile ID="ucGen_UploadFileForAll2" runat="server" isRequired="true"
                    replaceFile="true" createDirectory="true" />
                <div id="dvRequired1" style="display: none">
                    <div style="position: absolute; margin-top: -20px;">
                        <span style="font-size: 12px;">* file is required</span>
                    </div>
                </div>
                <uc:gen_UploadFile ID="ucGen_UploadFileForAll2Trans" runat="server" isRequired="false"
                    replaceFile="true" createDirectory="true" />
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <asp:RadioButton ID="rdCreativeUploadType2" runat="server" Text="4 min Creative Video AND PDF with additional still images from Creative Video"
                            GroupName="grpCreativeUploadType" onclick="ShowHideDvUpload();" />
                    </td>
                </tr>
            </table>
            <div id="dvUpload2" style="display: none; margin-left: 30px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="82%">
                            <asp:Label ID="lblTitle3" runat="server" Style="display: block; padding-top: 7px;
                                padding-bottom: 7px;"></asp:Label>
                        </td>
                        <td width="18%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <uc:gen_UploadFile ID="ucGen_UploadFileForAll3" runat="server" isRequired="true"
                    replaceFile="true" createDirectory="true" />
                <uc:gen_UploadFile ID="ucGen_UploadFileForAll4" runat="server" isRequired="true"
                    replaceFile="true" createDirectory="true" />
                <div id="dvRequired2" style="display: none">
                    <div style="position: absolute; margin-top: -20px;">
                        <span style="font-size: 12px;">
                            <asp:Label ID="lblRequired" runat="server"></asp:Label></span>
                    </div>
                </div>
                <uc:gen_UploadFile ID="ucGen_UploadFileForAll3Trans" runat="server" isRequired="false"
                    replaceFile="true" createDirectory="true" />
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr><td>&nbsp;</td></tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblDoneMsg" runat="server" ForeColor="Green"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div class="errorDiv">
                            <asp:Label ID="lblError" runat="server"></asp:Label></div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSubmitDummy" runat="server" Text="Upload" OnClientClick="return HideDvUpload();" />
                        <asp:Button ID="btnCloseDummy" runat="server" Text="OK" Visible="false" OnClientClick="return FancyClose();" />
                        <div style="display: none">
                            <asp:Button ID="btnSubmit" runat="server" Text="Upload" OnClick="btnSubmit_Click" /></div>
                    </td>                   
                </tr>
            </table>
            <br />
        </div>
        <script type="text/javascript">
            if ($('#rdCreativeUploadType0').is(':checked'))
                $('#dvUpload0').slideDown({ duration: 0 });
            else if ($('#rdCreativeUploadType1').is(':checked'))
                $('#dvUpload1').slideDown({ duration: 0 });
            else if ($('#rdCreativeUploadType2').is(':checked'))
                $('#dvUpload2').slideDown({ duration: 0 });
        </script>
        <telerik:RadProgressArea ID="radProgArea" runat="server" HeaderText="File Upload Progress">
        </telerik:RadProgressArea>
    </div>
    </form>
</body>
</html>
