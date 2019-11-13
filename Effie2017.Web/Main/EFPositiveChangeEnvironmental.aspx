﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="EFPositiveChangeEnvironmental.aspx.cs" Inherits="Main_EFPositiveChangeEnvironmental" MaintainScrollPositionOnPostback="false"
    EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/lib/jquery.mousewheel-3.0.6.pack.js"></script>
<script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/jquery.fancybox.js?v=2.1.5"></script>
<link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/jquery.fancybox.css?v=2.1.5" media="screen" />
<link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-buttons.css?v=1.0.5" />
<script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-buttons.js?v=1.0.5"></script>
<link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-thumbs.css?v=1.0.7" />
<script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-thumbs.js?v=1.0.7"></script>
<script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-media.js?v=1.0.6"></script>

    <link href="../css/style-entryform.css" rel="stylesheet" type="text/css" />
    <script>
        
        function triggerSend(params) {
            var msg = "";
            if (params == "Preview")
            {
                console.log("Masuk " + params);
                window.open('<%= PDFURL %>', '_blank'); 
                 
                __doPostBack('btnCheck', params);
                
                return true;
                msg = 'To Preview the entry, the form will be automatically save as draft. proceed ?';
            }
            else if (params == "SaveImage")
            {
                msg = 'All images above should be saved ?';
            }

            if (window.confirm(msg) == false) {
                return false;
            } else {
                __doPostBack('btnCheck', params);
            }
        }

        function UploadCharts(arg)
        {
            __doPostBack('btnCheck', arg);
        }

        function PopupAlert(arg) {
            __doPostBack('btnCheck', arg);
        }

        function ConfirmCancel()
        {
            return confirm('Confirm to cancel? Any changes will not be saved.');
        }

        function ConfirmDelete() {
            return confirm('Confirm to Delete?');
        }


        function MsgAlert(msg) {
            return confirm(msg);
        }

    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager11" runat="server">
    </telerik:RadScriptManager>

    
    <div class="errorDiv"><asp:Label ID="lblverification1" runat="server"></asp:Label></div>

    <div style="float: left">
        <h1>
            <asp:Literal ID="ltMainTitle" runat="server" Text="Entry Form" />
        </h1>
    </div>
    
    <br/> <br/> <br/> <br/> 
    <asp:Literal ID="ltInstructions" runat="server"><span style="font-weight: bold"><p>Complete the entry details below. You may save your entry as draft at any point and return to complete before the entry deadlines.</p></span></asp:Literal>

    <br/><br/><span style="font-weight: bold">Entry Form Instructions & Reminders<br /><br /></span>
        <span style="color: #a08647;font-weight: bold">ELIGIBILITY</span><br />
        Your case must have run in the <span style="font-weight:bold">APAC region</span> at some point between <span style="font-weight:bold">July 1, 2017, to September 30, 2018</span>.<br/><br/>

    <ul class="IconList">
        <li>All results must be isolated to the APAC region during this time period. </li>
        <li>Including results beyond the end of the eligibility period is grounds for disqualification. It is fine (and encouraged) to include prior year data for context, but it must be stated clearly.</li>
        <li>Work may have started running before July 1, 2017 and it may continue running after September 30, 2018, <span style="font-weight:bold">but the results MUST be within the limits of the qualifying period.</span></li>
    </ul>

    <br/><span style="color: #a08647; font-weight: bold">FORMATTTING REQUIREMENTS</span><br />

    <ul class="IconList">
        <li><span style="font-weight:bold">Word Limits:</span> Because each entrant has a different story to tell, question word limits are kept broad; however, entrants are <span style="font-weight:bold">NOT required or encouraged to utilise all space provided.</span> Judges encourage brevity.</li>
        <li><span style="font-weight:bold">Charts and graphs</span> to display data are strongly encouraged throughout the form. Save each chart/graph individually as a .jpg image or .png image. Ensure charts & graphs are sized so they are legible when viewed in the entry portal (700 pixels wide recommended). </li>
        <li>DO NOT include images of your creative work or competitor logos in your written entry, and MUST NOT leave any questions unanswered.</li>
        <li>DO NOT include external links in your submission – the judges only review content provided in your written entry and creative examples.</li>
    </ul>

    <div class="errorDiv" style="text-align: left;"><asp:Label ID="lbConfirmation" runat="server"></asp:Label></div>
    <br />
    <hr />
    <div class="errorDiv"><asp:Label ID="lbError" runat="server"></asp:Label></div>
    <div>
        <h2 style="color: #a08647;"> Entry Information</h2>
        <table class="TableCtm" border="0" cellspacing="0" cellpadding="0" style="width:100%">
            <tr>
                <td style="width:340px">
                    <span style="font-weight:bold">Entry Title:</span>
                </td>
                <td style="width:340px">
                    <asp:Label runat="server" ID="txtCampaign" ></asp:Label>
                </td>
                <td rowspan="5" style="vertical-align: top;">
                    <table class="tblEntryId">
                        <tr>
                            <td style=" padding-bottom: 5px; ">
                                <span style="font-weight:bold;">Entry ID:</span><br>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <asp:Label runat="server" ID="lblEntryID" style="font-weight:bold; font-size: 28px;"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="font-weight:bold"> Product / Service Classification:</span>
                </td>
                <td>
                    <asp:Label runat="server" ID="txtClassification" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="font-weight:bold">Brand Name: </span>
                    <br />
                </td>
                <td>
                    <asp:Label runat="server" ID="txtBrand" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td >
                    <span style="font-weight:bold"> You are entering into:</span>
                    <br />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblCategoryMarket" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="font-weight:bold"> Entry Category: </span>
                    <br />
                </td>
                <td>
                    <asp:Label runat="server" ID="txtEntryCategory" ></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <br>
          
        
        <span style="font-weight:bold"> Country and Time Period of your Case:</span>
        	<br><br>
        <table class="tabledata" rules="all" style="font-size: 14px;">
            <tr>
                <td style="padding-bottom: 10px; width: 350px;">
                    <span style="font-weight:bold"> Market in which the case is submitted:</span>
                </td>
                <td style="padding-bottom: 10px; width: 400px;"  colspan="3">
                    <asp:Label runat="server" ID="TxtCountry1" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">
                    <span style="font-weight:bold"> Start Date: </span>
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:Label runat="server" ID="txtStartdate1" ></asp:Label>
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <span style="font-weight:bold"> End Date </span>
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:Label runat="server" ID="txtEnddate1" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;" colspan="3">
                    <span style="font-weight:bold"> Total number of countries in which the case ran or is currently running:</span>
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:DropDownList ID="ddlCountryNumber" runat="server" CssClass="rdbBlock" style="width:100%">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div runat="server" id="AdditionalB" visible="false">
            
        <br><br>
            Additional Information<br><br>
        &nbsp;&nbsp;&nbsp;&nbsp;Additional countries in which the case ran or is currently running :
         <asp:Literal ID="litListCountries" runat="server"></asp:Literal><br/><br/>

        &nbsp;&nbsp;&nbsp;&nbsp;Total Countries : <asp:Label ID="lblTotalCountries" runat="server"></asp:Label><br>
        </div>
         
       
    </div>
    <br/>
        <br/>
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>EXECUTIVE SUMMARY</h2>
            </font>
        </div>

        <span style="font-weight: bold">Why is this a best in class example of marketing effectiveness and worthy of an award in this <m style="color:#b1985c"> Positive Change - Environmental</m> category? 
        <br><br>Effie has no predetermined definition of effectiveness.  It is your job to propose why this case is effective: why the metrics presented are important for your brand and business/organisation within the context of the submitted category.

        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> 	If you are entering this effort in multiple categories, your response to this question is <u>required</u> to be different for each category submission.</li>
        </ul>

        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtExecutiveSummary" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExecutiveSummary.ClientID %>">100</span></font>
            (max 100)
        </p>
    
        <input style="display:none" type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExecutiveSummary'); return false;" /><br>
        <br/>
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>SECTION 1: CHALLENGE, CONTEXT & OBJECTIVES 23.3% OF TOTAL SCORE</h2>
            </font>
        </div>

                <p class="efDesc">
                    This section provides the judges with the background to your challenge & objectives.  In this section, 
                    judges evaluate whether they have the necessary context about your industry category, competitors, and brand to 
                    understand your entry and the degree of challenge represented by your objectives. <u>Be thorough and provide context for 
                    judges unfamiliar with your industry and/or market to understand the scope of your effort.</u>
                </p>
        <br/>
        <span style="font-weight: bold">1A. Describe the background specific to the market that this case is entered on.
		<br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> 	Explain any relevant characteristics or trends unique to the market that sets it apart from the rest. 
              (For example, is there any government regulations in place, is the market exceptionally huge or small, etc.). 
              Highlight any relevant points which judges should know when evaluating the case in the context of the specific market.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtStrategicChallengeObjectivesA" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesA.ClientID %>">125</span></font>
            (max 125)
        </p>
    <span id="txtStrategicChallengeObjectivesA_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesA'); return false;" />
             
        &nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)
        </span>
        <span id="txtStrategicChallengeObjectivesA_View"></span>
        <br/>
       
        <br/>
        <span style="font-weight: bold">1B. Before your effort began, what was the state of the brand’s business and the marketplace/category in which it competes?
            <br/><br/>What was the strategic communications challenge that stemmed from this business situation? Provide context on the degree of difficulty of this challenge and detail how the business effort addressed them.

		<br/> <br/> Effie Tips: </span> <br/> <br/>
        <ul class="IconList">
          <li> 	Provide context about your brand and business situation, including main competitor spend, position in market, category benchmarks, etc.  What were the barriers you were tasked to overcome? </li>
        </ul>
        <br/> <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtStrategicChallengeObjectivesB" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesB.ClientID %>">275</span></font>
            (max 275)
        </p>
    <span id="txtStrategicChallengeObjectivesB_Edit">
    <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesB'); return false;" />
	&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)
	</span>
    <span id="txtStrategicChallengeObjectivesB_View"></span>
     <br>
        <br/>
        <span style="font-weight: bold">1C. Define the audience you were trying to reach. Why is this audience important to your brand and the growth of your brand, organisation or cause?
	    <br/><br/>Effie Tips: </span><br/> <br/>
        <ul class="IconList">
            <li>Describe your audience using demographics, culture, behaviours, etc.</li>
            <li>Explain if your target was a current audience, a new audience, or both.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtStrategicChallengeObjectivesC" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesC.ClientID %>">175</span></font>
            (max 175)
        </p>
    <span id="txtStrategicChallengeObjectivesC_Edit"><input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesC'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtStrategicChallengeObjectivesC_View"></span>
     <br>
        <br/>
        <br/>
        <span style="font-weight: bold">
            1D. What were your measurable objectives? What were the Key Performance Indicators (KPIs) against your objectives? Provide specific numbers/percentages for each objective and prior year benchmarks wherever possible.<br /><br />
            Effie is open to all types of objectives: Business, Behavioural, Perceptual/Attitudinal. It is the entrant’s responsibility to explain why their particular objectives are important to the business and challenging to achieve.<br /><br />
            Efforts entering the Positive Change: Environmental category <u>MUST</u>  have changing audience behaviour towards more environmentally sustainable choices as one of the main strategic objectives. The following elements will be considered:<br /><br />
            Awareness – Making the audience aware of a sustainable product, service, or action. 
            <br>Trial – Trying a sustainable product, service, or action for the first time. 
            <br>Substitution – Switching to a more sustainable product, service, or action. 
            <br>Change in Use – Using a product or service more sustainably than before.

	    <br/><br/>Effie Tips: </span><br/> <br/>
        <ul class="IconList">
              <li>Judges will expect to see context, around the goal(s) set.</li>
              <li>If you do not have specific, numerical objectives, explain why. Outline how you planned to measure your KPIs.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtStrategicChallengeObjectivesD" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesD.ClientID %>">175</span></font>
            (max 175)
        </p>
    <span id="txtStrategicChallengeObjectivesD_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesD'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtStrategicChallengeObjectivesD_View"></span>
     <br>
        <br/>
        <br/>
        <span style="font-weight: bold">1E.  How do the environmental sustainability goals relate back to the overall brand or organisation’s strategy and objectives?
	    <br/><br/>Effie Tips: </span><br/> <br/>
        <ul class="IconList">
              <li> 	What was the problem the effort was meant to address?  What were the specific challenges/opportunities faced by the organization and/or brand?      </li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtStrategicChallengeObjectivesE" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesE.ClientID %>">150</span></font>
            (max 150)
        </p>
    <span id="txtStrategicChallengeObjectivesE_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesE'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/grap
        <span id="txtStrategicChallengeObjectivesE_View"></span>
        hs)</span> <br>
        <br/>
        <span style="font-weight: bold">Sourcing: Section 1	
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li> 	You must provide a source for all data and facts. Judges encourage third party data where available. Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtStrategicChallengeObjectivesF" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            <%--Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesF.ClientID %>">90</span></font>
            (max 90)--%>
        </p>
        <br />
        <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesF'); return false;" />
        <br/>

        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>SECTION 2: INSIGHTS & STRATEGIC IDEA 23.3% OF TOTAL SCORE</h2>
            </font>
        </div>

        <br/>
                <p class="efDesc">
                    This section prompts you to explain your strategic process and thinking to the judges.  
                    Your idea should be borne from these unique insights.
                </p>
        <br/>
        <span style="font-weight: bold">2A. State the insight that led to your big idea, and explain how your observations that led you to your insight.<br /><br />
Keep in mind, an insight is not merely a fact or observation based on research, but a strategic one that is unique to your brand and audience leveraged to help meet your objectives. 
		<br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
            <li>It might be helpful to tell judges how you define an insight. It may be a consumer insight, a channel insight, marketplace insight, etc.</li>
            <li> Explain how your audience’s behaviours and attitudes, your research and/or business situation led to the unique insights that formed your strategic idea.</li>
            <li> Describe how your thinking led to your strategy, and how this strategy influenced the idea you were briefing for. </li>
            <li>Explain your theory for positive change here, if not already addressed in your entry.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtIdeasA" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasA.ClientID %>">225</span></font>
            (max 225)
        </p>
    <span id="txtIdeasA_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasA'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtIdeasA_View"></span>
     <br>
        <br/>
        <span style="font-weight: bold">2B. In <u>one sentence</u>, state your strategic big idea. What was the core idea that drove your effort and led to the breakthrough results?
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li> 	What was at the very heart of the success of this case?  The big idea is <u>not the execution or tagline.</u></li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtIdeasB" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasB.ClientID %>">25</span></font>
            (max 25)
        </p>
    <span id="txtIdeasB_Edit">
        <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasB'); return false;" /></span>
    <span id="txtIdeasB_View"></span>
        <span style="font-weight: bold">Sourcing: Section 2	
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li> 	You must provide a source for all data and facts. Judges encourage third party data where available.  Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtIdeasC" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing."></asp:TextBox>
        <br/><br/>
        <%--<p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasC.ClientID %>">90</span></font>
            (max 90)
        </p>--%>
    	<br />
        <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasC'); return false;" />

        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>SECTION 3: BRINGING THE IDEA TO LIFE 23.3% OF TOTAL SCORE</h2>
            </font>
        </div>

        <br/>
                <p class="efDesc">
                    This section relates to how and where you brought your idea to life – including your creative, 
                    communications and media strategies and the work itself.  
                    Judges will be providing their score for this section <u>based on the information you provide in Question 3, 
                    the Media Addendum, and the creative work as presented in the creative material and creative images.</u>  
                    Between the creative examples and your response to this question, 
                    the judges should have a clear understanding of the creative work that your audience experienced and 
                    how the creative elements worked together to achieve your objectives.
                </p>
        <br/>
        <span style="font-weight: bold">3.  How did you bring the idea to life?  
            <br/><br/>How did you bring the idea to life? Explain your idea and your overall communications strategy as borne from the insights and strategic challenge described earlier.
            <br/><br/>Elaborate on your communications strategy, including the rationale behind your key channel choices. Your explanation below must include which specific channels were considered integral to your media strategy and why.

        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>What are your channel choices and why are they and your media strategy right for your specific audience and idea? Explain the media behaviours of your audience.</li>
          <li><u>How</u> did your communications elements work together?  Did they change over time?  If so, how?</li>
        </ul>
        <br/>
         <asp:TextBox CssClass="TextCtm" ID="txtAnything" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtAnything.ClientID %>">475 </span></font>
            (max 475)
        </p><br />
    <span id="txtAnything_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtAnything'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtAnything_View"></span>
     <br><br />
        <span style="font-weight: bold">Sourcing: Section 3
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li> 	You must provide a source for all data and facts. Judges encourage third party data where available.  Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtBringingIdea" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
        </p><br />
    <span id="txtBringingIdea_Edit">
        <input type="submit" style="display:none" value="Upload charts/graphs" onclick="return UploadCharts('txtBringingIdea'); return false;" /></span>
    <span id="txtBringingIdea_View"></span>
        <br/>
        
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>SECTION 4: RESULTS 30% OF TOTAL SCORE</h2>
            </font>
        </div>
                <p class="efDesc">
                    This section relates to your results. Be sure to provide context (category/prior year) and explain the significance of your results as it relates to your brand’s business.  
                    <u>Tie results back to the objectives</u> outlined in Section 1.  <br /><br />
                    Positive Change: Environmental entrants must provide results regarding behavior change towards more environmentally sustainable choices.
                    Entrants are encouraged to use charts/graphs to display data whenever possible.  
                    As with the rest of the entry form, provide <u>dates and sourcing for all data provided.</u>

                </p>
        <br/>

        <span style="font-weight: bold">4A(i). How do you know it worked?  
        <br/><br/>Explain, with <u>category</u> and <u>prior year</u> context, why these results are significant for the brand’s business.  Results must relate to your specific audience, objectives, and KPIs.  Provide a clear time frame for all data shown.  
            <br /><br />Judges of the Positive Change: Environmental category will specifically evaluate the result of behaviour change towards more environmentally sustainable choices</span>
        <br/><br/><span style="font-weight: bold">Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>  Tie together the story of how your work drove the results. </li>
          <li> 	Entrants are strongly encouraged to <span style="font-weight:bold;">re-state their objectives from section 1 along with their corresponding results.</span></li>
          <li> 	Prove the results are significant using category, competitive, prior year, and brand context, and give the judges an understanding of what’s next.</li>
          <li> 	<span style="font-weight:bold;">Charts and graphs are strongly encouraged.</span></li>
        </ul>
        <br />
        <p class="efDesc">
            Note: Only results that fall between 1 Jul 2017 - 30 Sep 2018 will be evaluated by judges. 
            Dates for all results presented here should be included to prevent confusion. 
            You may include data before your start date for context purposes, 
            but do not include results after 30 Sep 2018. 
        </p>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtExplainWorkedA" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
       
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainWorkedA.ClientID %>">400</span></font>
            (max 400)
        </p>
    <span id="txtExplainWorkedA_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainWorkedA'); return false;" />&nbsp;&nbsp;&nbsp;(up to 5 charts/graphs)</span>
    <span id="txtExplainWorkedA_View"></span>
     <br>
        <br/>
        <span style="font-weight: bold">4A. (ii). How will the effort drive sustained change?</span>
        <br/><br/><span style="font-weight: bold">Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>Provide information on whether the effort is part of an ongoing program and/or how it acts as a catalyst for future change. </li>
          <li>Address the sustainability of the program or outcome.</li>
          <li>It is fine to refer to future plans, but no results after 30 Sep 2018 are permitted.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtExplainWorkedA2" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
       
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainWorkedA2.ClientID %>">100</span></font>
            (max 100)
        </p>
    <span id="txtExplainWorkedA2_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainWorkedA2'); return false;" />&nbsp;&nbsp;&nbsp;(up to 5 charts/graphs)</span> <br><br>
    <span id="txtExplainWorkedA2_View"></span>
        <span style="font-weight: bold">4B. Marketing communications rarely work in isolation.  Outside of your effort, what else in the marketplace could have affected the results of this case – positive or negative?<br /><br />
            Select factors from the chart and explain the influence of these factors in the space provided.  We recognise that attribution can be difficult; however, we’re inviting you to provide the broader picture here in making the case for your effectiveness.
        </span>

        <br/><br/><span style="font-weight: bold">Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>Judges are industry executives and expect entrants to address any other factors here.  <span style="font-weight:bold">Judges discourage entrants from</span> responding “No Other Factors”.</li>
          <li>This is your opportunity to address what else was going on to convince judges of the impact of your case by addressing these factors. You are encouraged to use this space to address the significance or insignificance of other factors on the results achieved by your effort.</li>
          <li>The chart provided is a sampling of common marketplace activities, but your response is not limited to these factors.</li>
        </ul>

        <br/>
        <div id="TableRPTExplainOtherMarket"  runat="server" class="TableFix">
        <table class="tabledata" rules="all" border="1">
            <asp:Repeater runat="server" ID="RPTExplainOtherMarket" OnItemDataBound="RPTExplainOtherMarket_ItemDataBound" >
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox CssClass="Cusinput" ID="cbItem" runat="server" />
                            <asp:HiddenField ID="hdItemId" runat="server" />
                            <asp:Label CssClass="Cuslabel" ID="Title" runat="server"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td>
                    <asp:CheckBox id="cbOtherOtherMarket" runat="server" />
                    <asp:Label runat="server" CssClass="Cuslabel" Id="LabelEom" Style="line-height: 25px;">Other</asp:Label>&nbsp;&nbsp;
                    <asp:TextBox id="txtOtherOtherMarket" runat="server" MaxLength="100" />
                </td>
            </tr>
        </table>
        </div>
        <br/>
    
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtExplainWorkedB" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
       
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainWorkedB.ClientID %>">150</span></font>
            (max 150)
        </p>
    <span id="txtExplainWorkedB_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainWorkedB'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtExplainWorkedB_View"></span>
     <br><br>
    
    

        <span style="font-weight: bold">Sourcing: Section 4
       <br/><br/>Effie Tips: </span><br/><br/>
       
        <ul class="IconList">
          <li> 	You must provide a source for all data and facts. Judges encourage third party data where available. Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul>
        <br/>
    

        <asp:TextBox  CssClass="TextCtm" ID="txtListAndExplainOtherMarketingText" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            
        </p>
        <br />
        <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtListAndExplainOtherMarketingText'); return false;" />
        <br/>

    
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>MEDIA ADDENDUM – POSITIVE CHANGE ENVIRONMENTAL ONLY</h2>
            </font>
        </div>

        
         <p class="efDesc">
            The Media Addendum is reviewed as part of Section 3: Bringing the Idea to Life, along with your response to Question 3 and your creative work, as presented in the Creative Materials and Images of Creative.  These elements together account for 23.3% of your score.
        </p>
        <br/><br/>
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                <h2>
                    PAID MEDIA EXPENDITURES
                </h2>
            </font>
        </div>
        <span style="font-weight:bold">Select paid media expenditures (purchased and donated), not including agency fees or production costs, for the effort described in this entry. 
        Given the ‘spirit’ of this question, use your judgment on what constitutes fees, production and the broad span that covers media – from donated space to activation costs. 
        <br/><br/>You are required to provide context around this budget range.  For example, if your budget has changed significantly, how this range compares to your competitors, etc.
        <br/><br/>All amounts in USD. </span>

        <br/><br/>
        <table class="tabledata" rules="all" border="1" style="font-size: 14px;">
            <tr>
                <td>Current Year 
                    <asp:TextBox ID="ddlCurrentYear" runat="server" CssClass="rdbBlock" style="width:100%" placeholder="Insert Year">
                    </asp:TextBox>
                </td>
                <td>Year Prior
                   <asp:TextBox ID="YearPrior" runat="server" CssClass="rdbBlock" style="width:100%" placeholder="Insert Year">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlPaidMediaExpendituresCurrent" runat="server" CssClass="rdbBlock" style="width:100%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPaidMediaExpendituresPrior" runat="server" CssClass="rdbBlock" style="width:100%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><span style="font-weight: bold">Compared to other competitors in this category, this budget is: </span></td>
                <td>
                    <asp:RadioButtonList ID="rblComparedOtherCompetitorsCheck" runat="server" CssClass="rdbBlock rdFix">
                        <asp:ListItem Value="Less" Text="Less" />
                        <asp:ListItem Value="same" Text="About the same" />
                        <asp:ListItem Value="More" Text="More" />
                        <asp:ListItem Value="Not" Text="Not Applicable (Elaboration Required)" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td><span style="font-weight: bold">Compared to overall spend on the brand in the prior year, <br/>the brand’s overall budget this year is: </span></td>
                <td>
                    <asp:RadioButtonList ID="rblComparedOverallSpendCheck" runat="server" CssClass="rdbBlock rdFix">
                        <asp:ListItem Value="Less" Text="Less" />
                        <asp:ListItem Value="same" Text="About the same" />
                        <asp:ListItem Value="More" Text="More" />
                        <asp:ListItem Value="Not" Text="Not Applicable (Elaboration Required)" />
                    </asp:RadioButtonList>
                </td>
            </tr>

        </table>
        <br/>
        <span style="font-weight: bold">
            Budget Elaboration:  You are required to provide judges with the context to understand your budget. For example, if your budget has changed significantly, how does this range compares to your competitors'; or, if your paid media expenditures were low and the production/other costs were high, you should elaborate here. 
            <br><br>
            If you have selected Not Applicable (NA) to either of the previous two questions, provide further explanation here.
         <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li> 	What was your distribution strategy? What was the balance of paid, earned, owned, and shared media?</li>
          <li> 	Did you outperform your media buy?</li>
          <li> 	This is an opportunity to provide further context surrounding your budget so judges have a clear understanding and do not question the information provided above.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm" ID="txtExplainListOtherMarketing" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainListOtherMarketing.ClientID %>">100</span></font>
            (max 100)
        </p>
        <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainListOtherMarketing'); return false;" />
        <br/><br/>
        
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                <h2>
                  OWNED MEDIA
                </h2>
            </font>
        </div>


        <br/>

        <span style="font-weight: bold">Elaborate on owned media (digital or physical company-owned real estate), that acted as communication channels for case content. 
        <br/><br/>Effie Tips: </span><br/><br/>
       
        <ul class="IconList">
          <li> 	Owned media examples may include a corporate website, social media platforms, packaging, a branded store, fleet of buses, etc. </li>
          <li> 	If owned media platforms were selected in the Communications Touchpoints chart, judges will expect your explanation of those platform(s) in your response. Similarly, any owned media described here must relate back to the touchpoints selected in the chart below.</li>
        </ul>
        <br/>

        <asp:TextBox CssClass="TextCtm" ID="txtOwnedMedia" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox><br/><br/>
        <%--<br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtOwnedMedia.ClientID %>">90</span></font>
            (max 90)
        </p>--%>
        <input type="submit"  style="display:none;"  value="Upload charts/graphs" onclick="return UploadCharts('txtOwnedMedia'); return false;" />
        <br/><br/>
        
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                <h2>
                SPONSORSHIPS
                </h2>
            </font>
        </div>


        <br/>
        <span style="font-weight: bold">Provide details regarding your sponsorships, if applicable. If not, please mark “NA”.</span>
        <br/><br/>

        <asp:TextBox CssClass="TextCtm" ID="txtSponsorship" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox><br/>
        <%--<br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtSponsorship.ClientID %>">90</span></font>
            (max 90)
        </p>--%>
        <br/><input type="submit"  style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtSponsorship'); return false;" />
        <br/><br/>
        
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                 <h2>COMMUNICATIONS TOUCHPOINTS </h2>
            </font>
        </div>
        <br/>
        <span style="font-weight: bold">Select all touchpoints used in the effort based on the options provided in the chart.  Within your response to Question 3, explain which touchpoints were integral in reaching your audience and why.
		
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>On the Creative Materials, you must show at least one complete example of each communication touchpoint that was <u>integral</u> to the effort’s success.  For example, if you mark 20 boxes below and 10 were key to driving the results and indicated as integral in Question 3, those 10 must be featured in the Creative Materials.</li>
        </ul>
        <br/>
        <div id="divrptCTP"  runat="server" class="TableFix">
        <table class="tabledata" rules="all" border="1">
            <asp:Repeater runat="server" ID="rptCTP" OnItemDataBound="rptCTP_ItemDataBound" >
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox CssClass="Cusinput" ID="cbItem" runat="server" />
                            <asp:HiddenField ID="hdItemId" runat="server" />
                            <asp:HiddenField ID="hdAttrType" runat="server" />
                            <asp:Label CssClass="Cuslabel" ID="Title" runat="server"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td>
                    <asp:CheckBox CssClass="Cusinput" style="margin-top: 7px;" ID="cbOtherCTP1" runat="server" />
                    <asp:Label runat="server" CssClass="Cuslabel" Id="LabelOtherCTP" Style="line-height: 25px;">Other (describe – limit 100 characters)</asp:Label>&nbsp;&nbsp;
                    <asp:TextBox CssClass="TextCtmOther" ID="txtOtherCTP2"  runat="server" MaxLength="100" />
                </td>
            </tr>
        </table>
        </div>
    <br/>
    <div class="errorDiv"><asp:Label ID="lblverification2" runat="server"></asp:Label></div>
    <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" />
    <asp:Button ID="btnDraft" runat="server" Text="Save as Draft" OnClick="btnDraft_Click" />
    <input type="submit" id="hlShowPDF" runat="server" value="Preview PDF" onclick="return triggerSend('Preview'); return false;" />
    <%--<input type="submit" id="btnCheck" value="Submit" onclick="return triggerSend('SaveEntryForm'); return false;" />--%>
    <%--<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />--%>
    <asp:Button ID="btnSubmit" runat="server" Text="Confirm Submit" OnClick="btnSave_Click" />
    <asp:Button ID="btnNext" runat="server" Text="Submit" OnClick="btnNext_Click" />
    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Back to Entry Overview" OnClientClick="return ConfirmCancel();" />
    

    <br/><br/>
    <div class="errorDiv"><asp:Label ID="lbError2" runat="server"></asp:Label></div>

    <asp:PlaceHolder ID="PopupUploadImage" runat="server" Visible="false" >
        <div class="ModalPopUpSmall">
            <h2> <asp:Label runat="server" ID="lblTitle"> Image for Charts/Graphs</asp:Label></h2>
            <hr />
            <br />

            <asp:Panel runat="server" ID="TableEditImage">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <asp:Repeater runat="server" ID="rptFileUpload" OnItemDataBound="rptFileUpload_ItemDataBound" 
                         OnItemCommand="rptFileUpload_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td >
                                    <asp:Label ID="Title" runat="server"> </asp:Label>
                                    <asp:HiddenField ID="ID" runat="server"> </asp:HiddenField>
                                </td>
                                <td style="line-height: 22px;" width="350px">
                                   <asp:FileUpload ID="File" runat="server" /> 
                                   <asp:HyperLink runat="server" ID="PrevImage" target="_blank"> Preview Image</asp:HyperLink>
                                    <br/>
                                     <a ID="btnEdit" runat="server" style="color:#b1985c;" href="javascript:void(0)" > Edit</a>
                                     <asp:LinkButton ID="btnDelete" style="color:#b1985c;" runat="server" Text="Delete" CommandName="DeleteImage" OnClientClick="return ConfirmDelete();" />
                                </td>
                                <td>
                                    
                                     <a ID="btnCancel" runat="server" href="javascript:void(0)" class="DisableControl"> Cancel</a>
                                </td>
                            </tr>
                            <tr>
                                <td width="200px">
                                    &nbsp;
                                </td>
                                <td style="font-size: 10px"  width="350px">
                                    <asp:Label ID="lblMax" runat="server">Image format to be in Jpeg/Png (max file size 1mb)</asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </asp:Panel>

        <asp:Panel runat="server" ID="TablePreviewImage">
            <div>
                <asp:Repeater runat="server" ID="rptPrevImage" OnItemDataBound="rptPrevImage_ItemDataBound">
                    <ItemTemplate>
                        <asp:HiddenField ID="ID" runat="server"> </asp:HiddenField>
                        <asp:Image runat="server"  ID="PrevImagerptPrevImage" style=" width: 200px; height: 180px; "/>
                        <asp:Literal ID="Space" runat="server"> </asp:Literal>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>


         <asp:Label id="lblDoneMsg" runat="server" class="errorDiv" Visible="false">Upload completed.<br></asp:Label>
         <br/>
        <asp:Button ID="btnSaveImage" runat="server" OnClick="btnSaveImage_Click" Text="Save" />
        <%--<input type="submit" id="btnSaveImage" value="Submit" onclick="return triggerSend('SaveImage'); return false;" />--%> &nbsp;&nbsp;
        <asp:Button ID="btnClosePopupimage" runat="server" OnClick="btnClosePopupimage_Click" Text="Close" />
        </div>
        <div class="overlay"></div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phAlert" runat="server" Visible="false" >
        <div class="fancybox-overlay fancybox-overlay-fixed" style="width: auto; height: auto; display: block;">
	        <div class="fancybox-wrap fancybox-desktop fancybox-type-iframe fancybox-opened FancyPopup" tabindex="-1">
		        <div class="fancybox-skin" style="padding: 15px;">
			        <div class="fancybox-outer">
			            <div class="fancybox-inner" style=" height: 50px; ">
			                <%=MsgAlert %>
			            </div>
			        </div>
                    <hr>
                    <div style="text-align:right">
                        <a href="javascript:void(0)"  onclick="return PopupAlert('Cancel_Alert'); return false;" style="color: #617bfb;text-decoration:none" >Cancel</a> &nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="javascript:void(0)"  onclick="return PopupAlert('<%=ActionAlert %>'); return false;" style="font-weight: bold; color: #617bfb;text-decoration:none" >OK</a> &nbsp;&nbsp;&nbsp;&nbsp;
		                <a title="Close" class="fancybox-item fancybox-close" href="javascript:;" style="display:none;"></a>
                    </div>
                
		        </div>
	        </div>
        </div>
    </asp:PlaceHolder>

    <asp:HiddenField ID="hdScrollPos" runat="server" />
    <script type="text/javascript">
        $(window).scroll(function () {
            $('#ContentPlaceHolder1_hdScrollPos').val($(window).scrollTop());
        });

        if ($('#ContentPlaceHolder1_hdScrollPos').val() != '')
            $(window).scrollTop($('#ContentPlaceHolder1_hdScrollPos').val());
    </script>
</asp:Content>
