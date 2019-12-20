<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="EFSustainedSuccess.aspx.cs" Inherits="Main_EFSustainedSuccess" MaintainScrollPositionOnPostback="false"
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
            if (params == "Preview") {
                console.log("Masuk " + params);
                window.open('<%= PDFURL %>', '_blank');

                __doPostBack('btnCheck', params);

                return true;
                msg = 'To Preview the entry, the form will be automatically save as draft. proceed ?';
            }
            else if (params == "SaveImage") {
                msg = 'All images above should be saved ?';
            }

            if (window.confirm(msg) == false) {
                return false;
            } else {
                __doPostBack('btnCheck', params);
            }
        }

        function UploadCharts(arg) {
            __doPostBack('btnCheck', arg);
        }

        function PopupAlert(arg) {
            __doPostBack('btnCheck', arg);
        }

        function ConfirmCancel() {
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
        Your case must have run in the <span style="font-weight:bold">APAC region</span> at some point between <span style="font-weight:bold">July 1, 2018, to August 31, 2019</span>.<br/><br/>

    <ul class="IconList">
        <li>All results must be isolated to the APAC region during this time period.</li>
        <li><span style="font-weight:bold">At least 3 years of results is required.  </span>At minimum, your results must date back to <span style="font-weight:bold">31 August 2016</span>, and your entries must include results from the current competition year. </li>
        <li>Including results beyond the end of the eligibility period is grounds for disqualification. It is fine (and encouraged) to include prior year data for context, but it must be stated clearly.</li>
        <li>Work may have continued running after <span style="font-weight:bold">August 2019 but the results MUST be within the limits of the qualifying period.</span></li>
    </ul>

    <br/><span style="color: #a08647; font-weight: bold">FORMATTTING REQUIREMENTS</span><br />

    <ul class="IconList">
        <li><span style="font-weight:bold">Word Limits:</span> Because each entrant has a different story to tell, question word limits are kept broad; however, entrants are <span style="font-weight:bold">NOT required or encouraged to utilise all space provided</span>. Judges encourage brevity.</li>
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
        <span style="font-weight: bold">Why is this a best in class example of marketing effectiveness and worthy of an award in this <span style="color: #a08647;font-weight: bold;""> Sustained Success</span> category? <br>
        <br>Effie has no predetermined definition of effectiveness.  It is your job to propose why this case is effective: why the metrics presented are important for your brand and business/organisation within the context of the submitted category.

        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> 	If you are entering this effort in multiple categories, your response to this question is <u>required</u> to be different for each category submission.</li>
        </ul>

        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtExecutiveSummary" style="height: 140px;font-size: 14px !important;" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align:right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExecutiveSummary.ClientID %>">100</span></font>
            (max 100 words)
        </p>
    
        <input style="display:none" type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExecutiveSummary'); return false;" /><br>
        <br/>
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>SECTION 1: CHALLENGE, CONTEXT & OBJECTIVES 23.3% OF TOTAL SCORE</h2>
            </font>
        </div>

                <p class="efDesc">
                    This section provides the judges with the background to your challenge & objectives.  
                    In this section, judges evaluate whether they have the necessary context about your industry category, 
                    competitors, and brand to understand your entry and the degree of challenge represented by your objectives.  
                    <u>Be thorough and provide context for judges unfamiliar with your industry and/or market to understand the scope of your effort. </u>  
                </p>
        <br/>
        <span style="font-weight: bold">1A. Describe the background specific to the market that this case is entered on and how did it evolve over time.
		<br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>Explain any relevant characteristics or trends unique to the market/country that sets it apart from the rest. 
              (For example, is there any government regulations in place, is the market exceptionally huge or small, etc.). 
              Highlight any relevant points which judges should know when evaluating the case in the context of the specific market.</li>
        </ul>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtStrategicChallengeObjectivesA" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesA.ClientID %>">150 </span></font>
            (max 150 words)
        </p>
    <span id="txtStrategicChallengeObjectivesA_Edit">
         <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesA'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    
    <span id="txtStrategicChallengeObjectivesA_View"></span>
      <br>
        <br/>
        <span style="font-weight: bold">1B. Before your effort began, what was the state of the brand’s business and the marketplace/category in which it competes?
            <br/><br/>What was the strategic communications challenge that stemmed from this business situation? How did it change over time? Provide context on the degree of difficulty of this challenge and detail how the business effort addressed them.
		<br/> <br/> Effie Tips: </span> <br/> <br/>
        <ul class="IconList">
            <li>Keep in mind judges may not be familiar with your brand’s industry. This context is critical for judges to understand your degree of difficulty.</li>
          <li>Provide context about your brand and business situation, including main competitor spend, position in market, category benchmarks, etc. at the beginning of your case and over time. What were the barriers you were tasked to overcome? </li>
        </ul>
        <br/> <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtStrategicChallengeObjectivesB" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesB.ClientID %>">350</span></font>
            (max 350 words)
        </p>
    <span id="txtStrategicChallengeObjectivesB_Edit">
         <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesB'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtStrategicChallengeObjectivesB_View"></span>
      <br>
        <br/>
        <span style="font-weight: bold">1C. Define the audience you were trying to reach. Why is this audience important to your brand and the growth of your brand’s business? Did your audience change over time? If so, describe how and why.
	    <br/><br/>Effie Tips: </span><br/> <br/>
        <ul class="IconList">
            <li>Describe your audience using demographics, culture, behaviours, etc.  Explain if your target was a current audience, a new audience, or both.</li>
            <li>Explain if your target was a current audience, a new audience, or both.</li>
        </ul>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtStrategicChallengeObjectivesC" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesC.ClientID %>">175</span></font>
            (max 175 words)
        </p>
    <span id="txtStrategicChallengeObjectivesC_Edit">
         <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesC'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtStrategicChallengeObjectivesC_View"></span>
      <br>
        <br/>
        <br/>
        <span style="font-weight: bold">
            1D. What were your measurable objectives, and how did it change over time? What were the Key Performance Indicators (KPIs) against your objectives?  Provide specific numbers/percentages for each objective and prior year benchmarks wherever possible.
            <br /><br />
            Effie is open to all types of objectives: Business, Behavioural, Perceptual/Attitudinal. It is the entrant’s responsibility to explain why their particular objectives are important to the business and challenging to achieve. 
	    <br/><br/>Effie Tips: </span><br/> <br/>
        <ul class="IconList">
              <li> 	Judges will expect to see context, including category background, around the goal(s) set.</li>
              <li>  If you do not have specific, numerical objectives, explain why.  Outline how you planned to measure your KPIs.</li>
        </ul>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtStrategicChallengeObjectivesD" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesD.ClientID %>">175</span></font>
            (max 175 words)
        </p>
    <span id="txtStrategicChallengeObjectivesD_Edit">
         <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesD'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtStrategicChallengeObjectivesD_View"></span>
     <br>
        <br/>
        <span style="font-weight: bold">Sourcing: Section 1	
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li> 	You must provide a source for all data and facts. Judges encourage third party data where available. Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtStrategicChallengeObjectivesE" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing."></asp:TextBox> <br/>
        <br/>
        <p style="text-align: right; font-size: 12px; display:none;">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesE.ClientID %>">200</span></font>
            (max 200 words)
        </p>
         <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesE'); return false;" />
        <br/>

        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>SECTION 2: INSIGHTS & STRATEGIC IDEA 23.3% OF TOTAL SCORE</h2>
            </font>
        </div>
                <p class="efDesc">
                    This section prompts you to explain your strategic process and thinking to the judges.  Your idea should be borne from these unique insights.
                </p>
        <br/>
        <span style="font-weight: bold">
            2A. State the insight that led to your big idea, and explain how your observations led you to your insight.
            <br><br>Keep in mind, an insight is not merely a fact or observation based on research, but a strategic one that is unique to your brand and audience leveraged to help meet your objectives. 
		<br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>It might be helpful to tell judges how you define an insight. It may be a consumer insight, a channel insight, marketplace insight, etc.</li>
          <li>Include your audience’s behaviours and attitudes, your research and/or business situation, and how it led to the unique insights that brought success to the brand.</li>
          <li>Describe how your thinking led to your strategy, and how this strategy influenced the big idea.</li>
        </ul>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtIdeasA" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasA.ClientID %>">250</span></font>
            (max 250 words)
        </p>
    <span id="txtIdeasA_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasA'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtIdeasA_View"></span>
     <br>
        <br/>
        <span style="font-weight: bold">2B. In <u>one sentence</u>, state your strategic big idea. What was the core idea that drove your effort and led to the breakthrough results?
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li> What was at the very heart of the success of this case?  The big idea is <u>not the execution or tagline.</u></li>
        </ul>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtIdeasB" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasB.ClientID %>">25</span></font>
            (max One-Sentence: 25 words)
        </p>
        <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasB'); return false;" />
        <span style="font-weight: bold">Sourcing: Section 2
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>You must provide a source for all data and facts. Judges encourage third party data where available.  Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul>
        <br/>
        <asp:TextBox CssClass="TextCtm"  ID="txtIdeasC" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px; display:none;">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasC.ClientID %>">30</span></font>
            (max One-Sentence: 30 words)
        </p>
        <br/>
        <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasC'); return false;" />
        <br/>

        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>SECTION 3: BRINGING THE IDEA TO LIFE 23.3% OF TOTAL SCORE</h2>
            </font>
        </div>
                <p class="efDesc">
                     This section relates to how and where you brought your idea to life – including your creative, 
                    communications and media strategies and the work itself.  
                    Judges will be providing their score for this section <u>based on the information you provide in Question 3, 
                    the Media Addendum, and the creative work as presented in the creative material and creative images. </u> 
                    Between the creative examples and your response to this question, 
                    the judges should have a clear understanding of the creative work that your audience experienced and how the creative elements worked together to achieve your objectives.
                </p>
        <br/>
        <span style="font-weight: bold">3. How did you bring the idea to life, and how did you sustain it over time?  Explain your idea and your overall communications strategy as borne from the insights and strategic challenge described earlier.
        <br/><br/>Elaborate on your communications strategy, including the rationale behind your key channel choices. Your explanation below must include which specific channels were considered integral to your media strategy and why.

        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>What are your channel choices and why are they and your media strategy right for your specific audience and idea? Explain the media behaviours of your audience.</li>
          <li><u>How</u>  did your communications elements work together?  Did they change over time?  If so, how?</li>
        </ul>
        <br/>
         <asp:TextBox CssClass="TextCtm"  ID="txtAnything" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
    
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtAnything.ClientID %>">600 </span></font>
            (max 600 words)
        </p>
    <br/>
    <span id="txtAnything_Edit">
    <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtAnything'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)</span>
    <span id="txtAnything_View"></span>
      <br>
    <br/>
        <span style="font-weight: bold">Sourcing: Section 3
        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>You must provide a source for all data and facts. Judges encourage third party data where available.  Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtBringingIdea" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px; display:none;">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtBringingIdea.ClientID %>">90 </span></font>
            (max 90 words)
        </p>
        <br/>
        <input type="submit" style="display:none" value="Upload charts/graphs" onclick="return UploadCharts('txtBringingIdea'); return false;" />
        
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>SECTION 4: RESULTS 30% OF TOTAL SCORE</h2>
            </font>
        </div>
                <p class="efDesc">
                    This section relates to your results. Be sure to provide context (category/prior year) and explain the significance of your results as it relates to your brand’s business.  
                    <u>Tie results back to the objectives</u> outlined in Section 1.  
                    Entrants are encouraged to use charts/graphs to display data whenever possible.  
                    As with the rest of the entry form, provide <u>dates and sourcing for all data provided.</u>

                </p>
        <br/>
		<span style="font-weight: bold">4A. How do you know it worked?  
        <br/><br/>Explain, with <u>category</u> and <u>prior year</u> context,
            why these results are significant for the brand’s business.  
            Results must relate to your specific audience, objectives, and KPIs that was described earlier.  
            Provide a clear time frame for all data shown, so that judges can see your success over time.  


        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
              <li>Tie together the story of how your work drove the results. </li>
              <li>Entrants are strongly encouraged to <span style="font-weight:bold;">re-state their objectives from section 1 along with their corresponding results.</span></li>
              <li>Prove the results are significant using category, competitive, prior year, and brand context, and give the judges an understanding of what’s next.</li>
              <li><span style="font-weight:bold;">Charts and graphs are strongly encouraged.</span></li>
        </ul>

        <br>

        <p class="efDesc">
            Note:
            <ul class="IconList">
                  <li>Dates for all results presented here should be included to prevent confusion. At minimum, results must date back to 31 August 2016, and must include the current competition year’s results (1 July 2017 to 31 August 2019). Do not include results after 31 August 2019. </li>
                  <li>If presenting more than 3 years in this case, make sure to provide results here for the full spectrum of years that you are presenting in this case and in your creative material.</li>
            </ul>
        </p>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtExplainWorkedA" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
       
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainWorkedA.ClientID %>">475</span></font>
            (max 475 words)
        </p>
    <span id="txtExplainWorkedA_Edit">
        <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainWorkedA'); return false;" />&nbsp;&nbsp;&nbsp;(up to 5 charts/graphs)</span>
    <span id="txtExplainWorkedA_View"></span>
      <br>
        <br/>
        <span style="font-weight: bold">4B. Marketing communications rarely work in isolation.  Outside of your effort, what else in the marketplace could have affected the results of this case – positive or negative over the time period?
        <br/><br/>Select factors from the chart and explain the influence of these factors over time in the space provided.  We recognise that attribution can be difficult; however, we’re inviting you to provide the broader picture here in making the case for your effectiveness.


        <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>This is your opportunity to convince the judges and address the significance or insignificance of other factors on the results achieved by your effort. udges discourage entrants from responding "No Other Factors".</li>
          <li>The chart provided is a sampling of common marketplace activities, but your response is not limited to these factors.</li>
          <li>Address the full sustained success time period (three or more years).</li>
        </ul>
        <br/>
		<div id="divrptEOM"  runat="server" class="TableFix">
        <table class="tabledata" rules="all" border="1" >
            <tr>
                <td>Marketing Components</td>
                <td>Time Period</td>
            </tr>
            <asp:Repeater runat="server" ID="RPTExplainOtherMarket" OnItemDataBound="RPTExplainOtherMarket_ItemDataBound" >
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdItemId" runat="server" />
                            <asp:Label ID="MarketingComponents" runat="server"></asp:Label>
                        </td>
                        <td><asp:TextBox id="txtTimePeriod" runat="server" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td>
                    Other
                    <asp:TextBox id="txtOtherExplainOtherMarket1" runat="server" />
                </td>
                <td><asp:TextBox id="txtOtherExplainOtherMarket2" runat="server" /></td>
            </tr>
        </table>
            </div>
        <br>
        <asp:TextBox  CssClass="TextCtm" ID="txtExplainWorkedB" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <p style="text-align: right; font-size: 12px;">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainWorkedB.ClientID %>">200</span></font>
            (max 200 words)
        </p>
        <br>
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

        <asp:TextBox CssClass="TextCtm"  ID="txtListAndExplainOtherMarketingText" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px; display:none;">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtListAndExplainOtherMarketingText.ClientID %>">200</span></font>
            (max 200 words)
        </p>
        <br><br>
        <input type="submit" value="Upload charts/graphs" style="display:none;" onclick="return UploadCharts('txtListAndExplainOtherMarketingText'); return false;" />
     
        
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>MEDIA ADDENDUM – SUSTAINED SUCCESS CATEGORY ONLY</h2>
            </font>
        </div>

                <p class="efDesc">
                   The Media Addendum is reviewed as part of Section 3: Bringing the Idea to Life, along with your response to Question 3 and your creative work, as presented in the Creative Materials and Images of Creative.  These elements together account for 23.3% of your score.  
                </p>
        <br/>
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                <h2>
                    PAID MEDIA EXPENDITURES
                </h2>
            </font>
        </div>
        <span style="font-weight:bold">Select paid media expenditures (purchased and donated), not including agency fees or production costs, for the effort described in this entry over time.
        Given the ‘spirit’ of this question, use your judgment on what constitutes fees, production and the broad span that covers media – from donated space to activation costs. 
        You must provide the budget for A) the initial year the case started (initial year is either the year your case started or at least 3 years ago), B) one interim year, and C) the current year (July 1, 2018, to August 31, 2019).
        <br/><br/>Indicate the <u> percent change</u> for your budget for each year represented compared to the prior year. (e.g. 2% increase, same, etc.) If not known or not applicable, indicate this.
        </span><br/><br/>
        <span style="font-weight:bold;"> All amounts in USD.</span> 
        <br/><br/>
        <table class="tabledata" rules="all" border=0" >
            <tr>
                <td style="padding-bottom: 10px; width: 200px;" colspan="4">
                    Indicate the size of your media budget in the chart below using the following ranges:
                </td>
            </tr>
            <tr>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                   Under $100K
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                    $100K - under $250K
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                    $250K - under $500K
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                    $500K - under $1M
                </td>
            </tr>
            <tr>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                   $1M - under $5M
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                    $5M -under $10M
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                    $10M -under $20M
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                    $20M and over
                </td>
            </tr>
            <tr>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                   Not Applicable (NA)
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                   
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                    
                </td>
                <td style="font-style: italic; padding-bottom: 10px; width: 200px;">
                   
                </td>
            </tr>
        </table>
        <br/>
        <table class="tabledata" rules="all" border="1" >
            <tr>
                <td style="text-align: center; font-weight: bold; "></td>
                <td style="text-align: center; font-weight: bold; ">
                    Initial Year<br/>
                    <asp:TextBox ID="DdlInitialYearPaid" runat="server" style=" width: 230px; " placeholder="Insert Year"> </asp:TextBox>

                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Interim Year<br/>
                    <asp:TextBox ID="DdlInterimYearPaid" runat="server" style=" width: 230px; " placeholder="Insert Year"> </asp:TextBox>

                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Current Competition Year<br/>
                    <asp:TextBox ID="DdlCurrentYearPaid" runat="server" style=" width: 230px; " placeholder="Insert Year"> </asp:TextBox>

                </td>
            </tr>
            <tr>
                <td>
                    
                    <span style="font-weight:bold;">Paid Media Expenditures</span> <br/>
                     <i>Example: $2-5 Million</i>

                </td>
                <td>
                    <asp:DropDownList ID="ddlInitialYearPME" runat="server" CssClass="rdbBlock" style="width:100%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlInterimYearPME" runat="server" CssClass="rdbBlock" style="width:100%">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCurrentYearPME" runat="server" CssClass="rdbBlock" style="width:100%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="font-weight:bold;">Percent Change from Previous Sample Year </span>  <br/>
                    <i> Example: Approx. 5% increase</i>
                </td>
                <td>
                    <asp:TextBox ID="txtInitialYearPCP" runat="server" CssClass="rdbBlock">
                    </asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtInterimYearPCP" runat="server" CssClass="rdbBlock">
                    </asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtCurrentYearPCP" runat="server" CssClass="rdbBlock">
                    </asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td><span style="font-weight: bold">Compared to other competitors in this category, this budget is: </span></td>
                <td style=" background: #e4e4e4; ">
                    <%--<asp:RadioButtonList ID="rblComparedInitial" runat="server" CssClass="rdbBlock">
                        <asp:ListItem Value="Less" Text="Less" />
                        <asp:ListItem Value="same" Text="About the same" />
                        <asp:ListItem Value="More" Text="More" />
                        <asp:ListItem Value="Not" Text="NA (Elaboration Required)" />
                    </asp:RadioButtonList>--%>
                </td>
                <td style=" background: #e4e4e4; ">
                   <%-- <asp:RadioButtonList ID="rblComparedInterim" runat="server" CssClass="rdbBlock">
                        <asp:ListItem Value="Less" Text="Less" />
                        <asp:ListItem Value="same" Text="About the same" />
                        <asp:ListItem Value="More" Text="More" />
                        <asp:ListItem Value="Not" Text="NA (Elaboration Required)" />
                    </asp:RadioButtonList>--%>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblComparedCurrent" runat="server" CssClass="rdbBlock">
                        <asp:ListItem Value="Less" Text="Less" />
                        <asp:ListItem Value="same" Text="About the same" />
                        <asp:ListItem Value="More" Text="More" />
                        <asp:ListItem Value="Not" Text="NA (Elaboration Required)" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td><span style="font-weight: bold">Compared to overall spend on the brand in the prior year, <br/>the brand’s overall budget this year is: </span></td>
                 <td style=" background: #e4e4e4; ">
                    <%--<asp:RadioButtonList ID="rblComparedoverallInitial" runat="server" CssClass="rdbBlock">
                        <asp:ListItem Value="Less" Text="Less" />
                        <asp:ListItem Value="same" Text="About the same" />
                        <asp:ListItem Value="More" Text="More" />
                        <asp:ListItem Value="Not" Text="NA (Elaboration Required)" />
                    </asp:RadioButtonList>--%>
                </td>
                <td style=" background: #e4e4e4; ">
                   <%-- <asp:RadioButtonList ID="rblComparedoverallInterim" runat="server" CssClass="rdbBlock">
                        <asp:ListItem Value="Less" Text="Less" />
                        <asp:ListItem Value="same" Text="About the same" />
                        <asp:ListItem Value="More" Text="More" />
                        <asp:ListItem Value="Not" Text="NA (Elaboration Required)" />
                    </asp:RadioButtonList>--%>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblComparedoverallCurrent" runat="server" CssClass="rdbBlock">
                        <asp:ListItem Value="Less" Text="Less" />
                        <asp:ListItem Value="same" Text="About the same" />
                        <asp:ListItem Value="More" Text="More" />
                        <asp:ListItem Value="Not" Text="NA (Elaboration Required)" />
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <br/>
        <span style="font-weight: bold">
           Budget Elaboration: You are required to provide judges with the context to understand your budget over time.
           <br/><br/>For example, if your budget has changed significantly, how does this range compares to your competitors'; or, if your paid media expenditures were low and the production/other costs were high, you should elaborate here. 
            <br/><br/>If you selected Not Applicable (NA) to either of the previous two questions, provide further explanation here.

         <br/><br/>Effie Tips: </span><br/><br/>
        <ul class="IconList">
          <li>  How did your budget change over time?</li>
          <li>  What was your distribution strategy? What was the balance of paid, earned, owned, and shared media?</li>
          <li> 	Did you outperform your media buy?</li>
         
        </ul>
        <br/>
        <asp:TextBox  CssClass="TextCtm" ID="txtExplainListOtherMarketing" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainListOtherMarketing.ClientID %>">100</span></font>
            (max 100 words)
        </p>
        <input style="display:none" type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainListOtherMarketing'); return false;" /><br>
        
        <br/>
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

        <asp:TextBox  CssClass="TextCtm" ID="txtOwnedMedia" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align:right; font-size: 12px; display:none;">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtOwnedMedia.ClientID %>">90</span></font>
            (max 90 words)
        </p>
    <br>
       <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtOwnedMedia'); return false;" />
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

        <asp:TextBox  CssClass="TextCtm" ID="txtSponsorship" runat="server" TextMode="MultiLine" placeholder="Provide Answer."></asp:TextBox>
        <br/>
        <p style="text-align: right; font-size: 12px; display:none;">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtSponsorship.ClientID %>">90</span></font>
            (max 90 words)
        </p>
        <br/><input type="submit"  style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtSponsorship'); return false;" />
        <br/><br/>
        
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                 <h2>COMMUNICATIONS TOUCHPOINTS</h2>
            </font>
        </div>
        <br/>
        <span style="font-weight: bold"><%--Select all touchpoints used in the effort based on the options provided in the chart.  Within your response to Question 3, explain which touchpoints were integral in reaching your audience and why.--%>
		Select all touchpoints used in the effort based on the options provided in the chart and the <u>% of the total budget</u> that was used for each communications touch point, which should equal 100% for each year. 
 		<br /><br />
        You must provide information for A) the initial year your case started (initial year is either the year case started or at least 3 years ago, B) 1 interim year, and C), the current year (1 July 2018 to 31 August 2019).
            <br /><br />
        Within your response to Question 3, explain which touchpoints from the below list were integral to reaching your audience and why.  

        <br/><br/>Notes: </span><br/><br/>
        <ul class="IconList">
          <li>On the Creative Materials, you must show at least one complete example of each communication touchpoint that was <span style="text-decoration:underline;">  integral</span> to the effort’s success.  For example, if you mark 30 boxes below and 10 were key to driving the results and indicated as integral in Question 3, those 10 must be featured in the Creative Materials.</li>
          <%--<li>Answers below should indicate % of total budget used for each communications touchpoint, which should equal 100% for each year.</li>--%>
        </ul>
        <br/>
        
		<div id="tabledataCTP"  runat="server" class="TableFix">
        <table class="tabledata" rules="all" border="1" >
            <tr>
                <td style="text-align: center; font-weight: bold; ">Consumer Touch Points</td>
                <td style="text-align: center; font-weight: bold; ">
                    Initial Year<br/>
                    <asp:TextBox ID="DdlInitialYear" runat="server" style=" width: 230px; "  placeholder="Insert Year"> </asp:TextBox>

                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Interim Year<br/>
                    <asp:TextBox ID="DdlInterimYear" runat="server" style=" width: 230px; " placeholder="Insert Year"> </asp:TextBox>

                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Current Competition Year<br/>
                    <asp:TextBox ID="DdlCurrentYear" runat="server" style=" width: 230px; " placeholder="Insert Year"> </asp:TextBox>

                </td>

            </tr>
            <asp:Repeater runat="server" ID="rptCTP" OnItemDataBound="rptCTP_ItemDataBound" >
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdItemId" runat="server" />
                            <asp:Literal ID="Title" runat="server"></asp:Literal>
                        </td>
                        <td style="text-align: center; ">
                            <asp:TextBox id="txtInitial" runat="server" />

                        </td>
                        <td style="text-align: center; ">
                            <asp:TextBox id="txtInterim" runat="server" />

                        </td>
                        <td style="text-align: center; ">
                            <asp:TextBox id="txtCurrent" runat="server" />

                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td>
                    Other
                    <asp:TextBox id="txtOtherCTP1" runat="server" />
                </td>
                <td style="text-align: center; "><asp:TextBox id="cbOtherCTP2" runat="server" /></td>
                <td style="text-align: center; "><asp:TextBox id="cbOtherCTP3" runat="server" /></td>
                <td style="text-align: center; "><asp:TextBox id="cbOtherCTP4" runat="server" /></td>
            </tr>
        </table>
        </div>
        <br><br>
         <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2>ADDITIONAL INSTRUCTIONS FOR CREATIVE MATERIALS <l style="color:black">SUSTAINED SUCCESS</l> CATEGORY ONLY</h2>
            </font>
        </div>
        <br/>
        <ul class="IconList">
            <li>Feature work that ran in the <l style="color: #a08647;font-weight: bold;">initial year </l>(initial year is either: A) year case started or B) at least 3 years ago), <l style="color: #a08647;font-weight: bold;">at least one interim year</l> , and the  <span style="color: #a08647;font-weight: bold;">current competition year</span> (1 July 2018 to 31 August 2019).</li>
            <li>Each example of the creative work must be <l style="color: #a08647;font-weight: bold;">labeled with the year it ran</l> .  Entries that do not label the year will be disqualified. Also label the year on images of creative uploaded for judging</li>
            <li>The Sustained Success Creative Video may be a <l style="color: #a08647;font-weight: bold;">maximum of 4 minutes</l>.  (All other categories – maximum 3 minutes.)</li>
            <li>On the creative materials, you must show at least one complete example of each touchpoint you mark on the Media Addendum Communications Touchpoints chart that was <l style="color: #a08647;font-weight: bold;">integral</l> to the effort’s success.  </li>
            <li>Additional examples of creative work are encouraged on the creative video vs. re-telling the story outlined in the written case, as judges read the written case before watching the video and frequently comment they would like to see more examples of the work. Entrants should show the “how-when-where” you connected with your audience over time.</li>
            <li>Review and follow the complete Creative Materials Instructions in Section 3.3 of the Entry Kit</li>
        </ul>
    <br/><br/>
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
