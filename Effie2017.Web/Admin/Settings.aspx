<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Admin_Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  <div style="float:left">
        <h1>Setup</h1>
    </div>

        

        <div style="clear:both"></div>
        <br />

      <div class="errorDiv"><asp:Label ID="lbError" runat="server"></asp:Label></div>
<%--      <hr />
        <p>*required fields</p>
        <br />--%>

         <div class="leftContainer">
          <h2>Active Round</h2>
      
             <table width="100%" border="0" cellspacing="0" cellpadding="0">
                 <tbody>
                     <tr>
                         <td style="padding-bottom: 10px; width: 200px;">
                             Round:
                         </td>
                         <td style="padding-bottom: 10px">
                            <asp:RadioButton ID="rbRound1" runat="server" GroupName="round" 
                                 AutoPostBack="true" oncheckedchanged="rbRound1_CheckedChanged" />1 &nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="rbRound2" runat="server" GroupName="round" 
                                 AutoPostBack="true" oncheckedchanged="rbRound2_CheckedChanged" />2
                         </td>
                     </tr>
                 </tbody>
             </table>
               <br />
           </div>



           <div style="clear: both;" />
         <div class="leftContainer">
          <h2>Active Panels</h2>
      
             <table width="100%" border="0" cellspacing="0" cellpadding="0">
                 <tbody>
                     <tr>
                         <td style="padding-bottom: 10px; width: 200px;">
                             Round 1:
                         </td>
                         <td style="padding-bottom: 10px">
                            <asp:CheckBoxList ID="cblRound1" runat="server" RepeatColumns="6"  CssClass="cbl3" />
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px; width: 200px;">
                             Round 2:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:CheckBoxList ID="cblRound2" runat="server" RepeatColumns="6" CssClass="cbl3" />
                         </td>
                     </tr>

<%--                     <tr>
                         <td>
                             Fax Number:
                         </td>
                         <td>
                          <span class="txt62">
                             <asp:TextBox ID="txtFaxCountry" runat="server" MaxLength="5"></asp:TextBox></span>
                              <span class="txt62">
                             <asp:TextBox ID="txtFaxArea" runat="server" MaxLength="5" CssClass="txt62"></asp:TextBox></span>
                             <span class="txt90">
                             <asp:TextBox ID="txtFax" runat="server" MaxLength="20" CssClass="txt90"></asp:TextBox></span>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             &nbsp;
                         </td>
                         <td style="padding-bottom: 10px; font-size: 10px">
                             Country Code Area Code&nbsp;&nbsp;&nbsp;Number
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             Website:
                         </td>
                         <td style="padding-bottom: 10px; font-size: 10px">
                             <span style="padding-bottom: 10px">
                                 <asp:TextBox ID="txtWebsite" runat="server"></asp:TextBox>
                             </span>
                         </td>
                     </tr>--%>
                 </tbody>
             </table>
               <br />
           </div>


           <div style="clear: both;" />
        <br />
        <div class="errorDiv"><asp:Label ID="lbError2" runat="server"></asp:Label></div>
         <p>
             <div style="text-align:center">
                 <table width="100%">
                     <tr>
                         <td style="text-align: left ">
                           <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                                onclick="btnSubmit_Click" />
                         </td>
                         <td></td>
                         <td style="text-align: left"> </td>
                     </tr>
                 </table>
             </div>
        
       

</asp:Content>

