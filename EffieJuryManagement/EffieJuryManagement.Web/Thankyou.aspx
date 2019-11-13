<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Thankyou.aspx.cs" Inherits="Thankyou"
    MasterPageFile="~/Common/MasterPageLogin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="justify">
                <div style="height: 50px">
                </div>
                <div>
                    <asp:Panel runat="server" ID="pnlSuccess" Visible="false">
                        <p>
                            Thank you for accepting the invitation. It is our pleasure to have you on board
                            the APAC Effie jury.</p>
                        <p>
                            &nbsp;</p>
                        <p>
                            We will reach out with more details soon. In the meantime, feel free to reach out
                            to us at <a href="mailto:judging.apaceffie@tenasia.com.sg">judging.apaceffie@tenasia.com.sg</a>
                            if you have any questions.</p>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlReject" Visible="false">
                        <p>
                            Thank you for your indication. We look forward to your involvement in APAC Effie
                            in future.</p>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlLock" Visible="false">
                        <p>
                            You have responded to this invitation. If you wish to change your response, kindly
                            reach out to us at <a href="mailto:judging.apaceffie@tenasia.com.sg">judging.apaceffie@tenasia.com.sg</a>.</p>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlDeadline" Visible="false">
                        <p>
                            Deadline for accepting Invitation had been over. In the meantime, feel free to reach out
                            to us at <a href="mailto:judging.apaceffie@tenasia.com.sg">judging.apaceffie@tenasia.com.sg</a>
                            if you have any questions.</p>
                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
