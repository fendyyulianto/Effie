<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="User_Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div style="float:left">
        <h1><asp:Label runat="server" Text="Sign Up For An Account" ID="lbSignUpTitle" Visible="false"></asp:Label></h1>
        <h1><asp:Label runat="server" ID="lbEditTitle" Text="Edit Profile" Visible="false"></asp:Label></h1>
        <h1><asp:Label runat="server" ID="lbEditUserTitle" Text="Edit User" Visible="false"></asp:Label></h1>
    </div>





        <asp:PlaceHolder ID="phStatus" runat="server" Visible="false">
            <div style="width: 100%; text-align: right;">
                <table width="700px" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="width:60%">&nbsp;</td>
                        <td style="width:40%; text-align:right;">
                            <asp:CheckBox ID="chkDisable" runat="server" Text="Disable" />
                        </td>
                    </tr>                    
                    <tr>
                        <td style="width:60%">&nbsp;</td>
                        <td style="width:40%; text-align:right;">
                            <asp:CheckBox ID="chkVerified" runat="server" Text="Verified" />
                        </td>
                    </tr>
                    <tr><td style="width:70%">&nbsp;</td><td style="width:30%"><asp:Button ID="btnAdminSubmitStatus" runat="server" Text="Submit" OnClick="btnAdminSubmitStatus_Click" /></td></tr>



                </table>
            </div>
        </asp:PlaceHolder>

        <div style="clear:both"></div>

        <asp:PlaceHolder ID="phAdminRemarks" runat="server" Visible="false">
            <div style="margin-left:400px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr><td style="vertical-align:top;">Remarks:</td></tr>
                    <tr><td><asp:Label ID="lbAdminRemarks" runat="server" /></td></tr>
                    <asp:PlaceHolder ID="phAdminRemarksEdit" runat="server">
                        <tr><td><asp:TextBox ID="txtAdminRemarks" runat="server" TextMode="MultiLine" /></td></tr>
                        <tr><td><asp:Button ID="btnAdminSubmitRemarks" runat="server" Text="Submit Remarks" OnClick="btnAdminSubmitRemarks_Click" /></td></tr>
                    </asp:PlaceHolder>
                </table>
            </div>
        </asp:PlaceHolder>












        <div style="clear:both"></div>
        <br />
    <p>Complete the fields below to sign up for an account and start submitting entries.</p>
     <br />
      <div class="errorDiv"><asp:Label ID="lbError" runat="server"></asp:Label></div>
      <hr />
        <p>*required fields</p>
        <br />
         <div class="leftContainer">
         
          <h2>Login Details</h2>
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tbody><tr>
                <td style="padding-bottom:10px; width:271px">Email Address*:<br><span style="font-size:11px">(This will be your User ID)</span></td>
                <td style="padding-bottom:10px"><asp:TextBox ID="txtEmail" MaxLength="50" runat="server"></asp:TextBox></td>
              </tr>
               <asp:PlaceHolder runat="server" ID="phLoginDetails"  >
              <tr>
                <td style="padding-bottom:10px">Confirm Email Address*:</td>
                <td style="padding-bottom:10px"><asp:TextBox ID="txtConfirmEmail" MaxLength="50" runat="server"></asp:TextBox></td>
              </tr>
              <tr>
                <td style="padding-bottom:10px">Password*:<br><span style="font-size:11px">(must be 8-10 characters)</span></td>
                <td style="padding-bottom:10px"><asp:TextBox ID="txtPassword" MaxLength="10" runat="server" TextMode="Password"></asp:TextBox></td>
              </tr>
              <tr>
                <td>Confirm Password*:</td>
                <td><span style="padding-bottom:10px">
                   <asp:TextBox ID="txtConfirmPassword" MaxLength="10" runat="server" TextMode="Password"></asp:TextBox>
                </span></td>
              </tr>
              </asp:PlaceHolder>
            </tbody></table>

          <br />
         </div>
        <div style="clear:both"></div>
        <hr />
         <div class="leftContainer">
          <h2>Personal Information</h2>
      
             <table width="100%" border="0" cellspacing="0" cellpadding="0">
                 <tbody>
                     <tr>
                         <td style="padding-bottom: 10px; width: 271px">
                             Salutation*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:DropDownList ID="ddlSalutation" runat="server" Style="width: 234px">
                                 <asp:ListItem Value="" Text="Please select" />
                                 <asp:ListItem Value="Dr" Text="Dr." />
                                 <asp:ListItem Value="Mr" Text="Mr." />
                                 <asp:ListItem Value="Ms" Text="Ms." />
                                 <asp:ListItem Value="Mrs" Text="Mrs." />
                             </asp:DropDownList>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             First Name*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:TextBox ID="txtFirstName" MaxLength="100" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             Last Name*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:TextBox ID="txtLastName" MaxLength="100" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             Job Title*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:TextBox ID="txtJobTitle" MaxLength="100" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                    <%-- <tr>
                         <td style="padding-bottom: 10px">
                             Company*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <input name="textfield" type="text" id="textfield">
                         </td>
                     </tr>--%>
                     <tr>
                         <td>
                             Contact Number*:
                         </td>
                         <td>
                             <span class="txt62">
                                 <asp:TextBox ID="txtContactConutry" runat="server" MaxLength="5"></asp:TextBox></span>
                             <span class="txt62">
                                 <asp:TextBox ID="txtContactArea" runat="server" MaxLength="5" CssClass="txt62"></asp:TextBox></span>
                             <span class="txt90">
                                 <asp:TextBox ID="txtContact" runat="server" MaxLength="20" CssClass="txt90"></asp:TextBox></span>
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
                         <td>
                             Mobile Number:
                         </td>
                         <td>
                          <span class="txt62">
                             <asp:TextBox ID="txtMobileCountry" runat="server" MaxLength="5"></asp:TextBox></span>
                              <span class="txt62">
                             <asp:TextBox ID="txtMobileArea" runat="server"  MaxLength="5" CssClass="txt62"></asp:TextBox></span>
                              <span class="txt90">
                             <asp:TextBox ID="txtMobile" runat="server" MaxLength="20" CssClass="txt90"></asp:TextBox></span>
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
                     </tr>
                 </tbody>
             </table>
               <br />
           </div>
        <div style="clear:both"></div>
        <hr />
         <div class="leftContainer">
          <h2>Company Information</h2>
             <table width="100%" border="0" cellspacing="0" cellpadding="0">
                 <tbody>
                     <tr>
                         <td style="padding-bottom: 10px; width: 271px">
                             Company Name*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:TextBox ID="txtCompanyName" MaxLength="100" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             Address 1*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:TextBox ID="txtAddress1" MaxLength="100" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             Address 2:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:TextBox ID="txtAddress2" MaxLength="100" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             City*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:TextBox ID="txtCity" MaxLength="100" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             Postal Code*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:TextBox ID="txtPostalCode" MaxLength="50" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td style="padding-bottom: 10px">
                             Country*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:DropDownList ID="ddlCountry" Style="width: 234px" runat="server">
                             </asp:DropDownList>
                         </td>
                     </tr>
                     <tr id="LookedTableRowId" runat="server">
                         <td style="padding-bottom: 10px">
                             Locked:
                         </td>
                         <td style="padding-bottom: 10px">
                             <asp:CheckBox ID="cbLooked" Style="width: 234px" runat="server">
                             </asp:CheckBox>
                         </td>
                     </tr>
                 </tbody>
             </table>
         </div>
          <div style="clear:both"></div>




            <br />
            <br />
            <p>
                
                <div style="padding-left: 25px; margin-top: -17px; ">
                    Review the <a href="http://www.apaceffie.com/terms-of-use#&panel1-1">Terms of Use</a> and Privacy & <a href="http://www.apaceffie.com/privacy-policy#&panel1-1">Cookies Policy</a>.
                </div>
                <br/>
                <asp:CheckBox ID="chkTC" runat="server" />
                <div style="padding-left: 25px; margin-top: -17px; ">
                    By signing up for an account, I agree to receive updates and information from APAC Effie with regards to the Effie Awards Competition and their associated activities.
                </div>
        <br />
        <br />
         <p>
             <div style="text-align:center">
                 <table width="100%">
                     <tr>
                         <td style="text-align: right ">
                           <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                                onclick="btnSubmit_Click" />
                         </td>
                         <td></td>
                         <td style="text-align: left"> 
                             <asp:Button ID="btnBack" runat="server" Text="Back" onclick="btnBack_Click" 
                                 /></td>
                     </tr>
                 </table>
             </div>
        
       
</asp:Content>

