<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="EFMultiMarket.aspx.cs" Inherits="Main_EFMultiMarket" MaintainScrollPositionOnPostback="false"
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
                                 <asp:Label runat="server" ID="lblEntryID" style="font-weight:bold; font-size: 26px;"></asp:Label>
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
                    <span style="font-weight:bold">Brand Name:</span>
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
          
        
        <span style="font-weight:bold"> Markets and Time Period of your Case:</span>
        	<br><br>
        <table class="tabledata" rules="all" style="font-size: 14px;">
            <tr>
                <td style="padding-bottom: 10px; width: 350px;">
                    <span style="font-weight:bold"> Markets in which the case is submitted:</span>
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
                    <span style="font-weight:bold"> Total number of Markets in which the case ran or is currently running:</span>
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
          <span style="font-weight: bold">Why is this a best in class example of marketing effectiveness and worthy of an award in this <b style="color: #ad9841;">Multi Market Products & Services</b> category? <br /> <br />
        Effie has no predetermined definition of effectiveness.  It is your job to propose why this case is effective: why the metrics presented are important for your brand and business/organisation within the context of the submitted category.
		<br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> 	If you are entering this effort in multiple categories, your response to this question is <u>required</u> to be different for each category submission.</li>
        </ul>
        <br><br>
        <asp:TextBox ID="txtExecutiveSummary" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExecutiveSummary.ClientID %>">100</span></font>
            (Maximum: 100 words) </p>
        <br />
    
    <span id="txtExecutiveSummary_Edit">
              <input type="submit" style="display:none" value="Upload charts/graphs" onclick="return UploadCharts('txtExecutiveSummary'); return false;" />
        </span>
    <span id="txtExecutiveSummary_View"></span>
	
       
       <br />
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2 class="headerText">SECTION 1: CHALLENGE, CONTEXT & OBJECTIVES 23.3% OF TOTAL SCORE</h2>
                
            </font>
        </div>
                <p class="efDesc">
                    This section provides the judges with the background to your challenge & objectives.  In this section, judges evaluate
                     whether they have the necessary context about your industry category , markets, competitors, and brand to understand 
                    your entry and the degree of challenge represented by your objectives.  <u> Be thorough and provide context for judges unfamiliar 
                    with your industry and/or market to understand the scope of your effort.  </u>
                </p>
        <br />
        <span style="font-weight: bold"> 1.	Explain the criteria used to determine the markets you are entering your case into.	
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> 	Entrants are encouraged to enter markets which demonstrate a high degree of challenge and diversity.  Explain the criteria you used to define the top market(s) that you are entering this case on (i.e. Media spend within each country?  Largest market in each region?  Greatest challenge in each market?).  Be specific on why these countries were chosen to demonstrate the global success of your case.</li>
            <li>Explain the criteria you used to define the top market(s) that you are entering this case on (i.e. Media spend within each market? Largest market in each region? Greatest challenge in each market?).</li>
            <li>Be specific on why these markets were chosen to demonstrate the success of your case.</li>
        </ul><br />
        <asp:TextBox ID="txtExplainCriteria" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainCriteria.ClientID %>">100</span></font>
            (Maximum: 100 words)</p>
        <br />
    <span id="txtExplainCriteria_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainCriteria'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs) 
        </span>
    <span id="txtExplainCriteria_View"></span>
	<br /><br />
        <span style="font-weight: bold">1A.	Describe the background specific to the market that this case is entered on.	
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> Explain any relevant characteristics or trends unique to the market that sets it apart from  the rest. 
              (For example, is there any government regulations in place, is the market exceptionally huge or small, etc.). 
              Highlight any relevant points which judges should know when evaluating the case in the context of the selected countries.</li>
        </ul><br />
        <asp:TextBox ID="txtDescribeMarket" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtDescribeMarket.ClientID %>">250</span></font>
               (Maximum: 250 words) </p>

        <br />
    <span id="txtDescribeMarket_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtDescribeMarket'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs) 
        </span>
    <span id="txtDescribeMarket_View"></span>
	<br>
        <br /><span style="font-weight: bold">1B.	Before your effort began, what was the state of the brand’s business and the marketplace/category in which it competes?<br /><br />
        What was the strategic communications challenge that stemmed from this business situation? Provide context on the degree of difficulty of this challenge and detail how the business effort addressed them.
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> Provide context about your brand and business situation, including main competitor spend, position in market, category benchmarks, etc.  What were the barriers you were tasked to overcome?</li>
        </ul><br />
        <asp:TextBox ID="txtStrategicChallengeObjectivesA" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesA.ClientID %>">450</span></font>
               (Maximum: 450 words)</p>
        <br />
    <span id="txtStrategicChallengeObjectivesA_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesA'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)
        </span>
    <span id="txtStrategicChallengeObjectivesA_View"></span>
	 <br /><br />
        <span style="font-weight: bold">1C.	Define the audience you were trying to reach. Why is this audience important to your brand and the growth of your brand’s business?
		&nbsp;&nbsp;
        <br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> Describe your audience using demographics, culture, behaviors, etc.  Explain if your target was a current audience, a new audience, or both.</li>
            <li>Explain if your target was a current audience, a new audience, or both.</li>
        </ul><br />
        <asp:TextBox ID="txtStrategicChallengeObjectivesB" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesB.ClientID %>">175</span></font>
              (Maximum: 175 words) </p>

        <br />
    <span id="txtStrategicChallengeObjectivesB_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesB'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs) 
        </span>
    <span id="txtStrategicChallengeObjectivesB_View"></span>
	<br /><br />
         <span style="font-weight: bold">1D.	What were your measurable objectives? What were the Key Performance Indicators (KPIs) against your objectives?  
             State specific goals on a market-by-market basis. Provide specific numbers/percentages for each objective and prior year benchmarks whenever possible.<br><br>
             Effie is open to all types of objectives: Business, Behavioural, Perceptual/Attitudinal.
             It is the entrant’s responsibility to explain why their particular objectives are important to the business and challenging to achieve. 
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> Judges will expect to see context, around the goal(s) set – why are these goals challenging?</li>
          <li> If you do not have specific, numerical objectives, explain why.  Outline how you planned to measure your KPIs. </li>
        </ul><br />
        <asp:TextBox ID="txtStrategicChallengeObjectivesC" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtStrategicChallengeObjectivesC.ClientID %>">175</span></font>
              (Maximum: 175 words) </p>

        <br />
    <span id="txtStrategicChallengeObjectivesC_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesC'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs) 
        </span>
    <span id="txtStrategicChallengeObjectivesC_View"></span>
	<br>
        <br />
        <span style="font-weight: bold">Sourcing: Section 1
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>You must provide a source for all data and facts. Judges encourage third party data where available. Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul><br />
        <asp:TextBox ID="txtStrategicChallengeObjectivesD" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <br />
    <span id="txtStrategicChallengeObjectivesD_Edit">
            <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtStrategicChallengeObjectivesD'); return false;" /> 
        </span>
    <span id="txtStrategicChallengeObjectivesD_View"></span>
	
        <br />
        <br />
        <!--Section 2 start -->
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2 class="headerText">SECTION 2: INSIGHTS & STRATEGIC IDEA 23.3% OF TOTAL SCORE</h2>
            </font>
        </div>
            <p  class="efDesc">
                This section prompts you to explain your strategic process and thinking to the judges.  Your idea should be borne from these unique insights.
            </p>
        <br>
        <span style="font-weight: bold"> 
            2A. State the insight that led to your big idea, and explain how your observations that led you to your insight.	
            <br><br>
            Keep in mind, an insight is not merely a fact or observation based on research, 
            but a strategic one that is unique to your brand and audience leveraged to help meet your objectives. 
		    &nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>It might be helpful to tell judges how you define an insight. It may be a consumer insight, a channel insight, marketplace insight, etc.</li>
          <li>Explain how your audience’s behaviours and attitudes, your research and/or business situation, led to the unique insights that formed the strategic idea.</li>
          <li>Describe how your thinking led to your strategy, and how this strategy influenced the big idea.</li>
        </ul><br />
        <asp:TextBox ID="txtIdeasA" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasA.ClientID %>">300</span></font>
             (Maximum: 300 words) </p>

        <br />
    <span id="txtIdeasA_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasA'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs)
        </span>
    <span id="txtIdeasA_View"></span>
	 <br /><br />
        <span style="font-weight: bold">2B.	In <u>one sentence</u>, state your strategic big idea.
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>The big idea is not the execution or tagline. It is the core idea that drove your effort which led to the breakthrough results.</li>
        </ul><br />
        <asp:TextBox ID="txtIdeasB" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasB.ClientID %>">25</span></font>
               (Maximum: 25 words) </p>

        <br />
    <span id="txtIdeasB_Edit">
             <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasB'); return false;" />
        </span>
    <span id="txtIdeasB_View"></span>
	

        <span style="font-weight: bold">2C.	How did your big idea evolve across the markets? while maintaining continuity/integrity?</span>
		<br />
        <asp:TextBox ID="txtIdeasC" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtIdeasC.ClientID %>">150</span></font>
              (Maximum: 150 words) </p>
   
         <br />
    <span id="txtIdeasC_Edit">
             <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasC'); return false;" />
        </span>
    <span id="txtIdeasC_View"></span>
	

        <span style="font-weight: bold">Sourcing: Section 2
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>You must provide a source for all data and facts. Judges encourage third party data where available.  Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul><br />
        <asp:TextBox ID="txtIdeasD" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing." CssClass="TextCtm" ></asp:TextBox>

        <br />
    <span id="txtIdeasD_Edit">
             <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtIdeasD'); return false;" />
        </span>
    <span id="txtIdeasD_View"></span>
	
        <!--End section 2 -->

        <!--Section 3 start -->
        <br/>
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2 class="headerText">SECTION 3: BRINGING THE IDEA TO LIFE 23.3% OF TOTAL SCORE</h2>
            </font>
        </div>
            <p class="headerText">
                This section relates to how and where you brought your idea to life – including your creative, communications and media strategies and the work itself.  
                Judges will be providing their score for this section <u>based on the information you provide in Question 3, the Media Addendum, and the creative work as presented 
                in the creative material and creative images</u>.  Between the creative examples and your response to this question, the judges should have a clear understanding of
                 the creative work that your audience experienced and how the creative elements worked together to achieve your objectives.
            </p>
        <br/>
        <span style="font-weight: bold"> 3.	How did you bring the idea to life?  Explain your idea and your overall communications strategy across the countries.<br /><br />
            Elaborate on your communications strategy, including the rationale behind your key channel choices. Your explanation below must include which specific channels were considered integral to your media strategy and why.<br /><br />
            Detail the ways in which the local challenges impacted the way the idea was carried out within each market, and how it was made culturally relevant to each environment, and why certain strategies for different markets were chosen.
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>What are your channel choices and why are they and your media strategy right for your specific audience and idea? Explain the media behaviours of your audience.</li>
          <li><u>How</u> did your communications elements work together?  Did they change over time?  If so, how?</li>
        </ul><br />
        <asp:TextBox ID="txtBringingIdea" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtBringingIdea.ClientID %>">600</span></font>
             (Maximum: 600 words) </p>

        <br />
    <span id="txtBringingIdea_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtBringingIdea'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs) 
        </span>
    <span id="txtBringingIdea_View"></span>
	<br /><br />
        <span style="font-weight: bold">Sourcing: Section 3
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>You must provide a source for all data and facts. Judges encourage third party data where available.  Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul><br />
        <asp:TextBox ID="txtSection3" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing." CssClass="TextCtm" ></asp:TextBox>
        <br />
         <br />
    <span id="txtSection3_Edit">
             <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtSection3'); return false;" />
        </span>
    <span id="txtSection3_View"></span>
	

        <!--End section 3 -->

        <!--Section 4 start --><br />
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2 class="headerText">SECTION 4: RESULTS 30% OF TOTAL SCORE</h2>
            </font>
        </div>
            <p  class="efDesc">
                This section relates to your results. Be sure to provide context (category/prior year) and explain the significance of your results as it relates to your brand’s business. 
                <u> Tie results back to the objectives</u> outlined in Section 1. Entrants are encouraged to use charts/graphs to display data whenever possible. As with the rest of the entry form, provide <u>dates and sourcing for all data provided.</u>
            </p>
        <br>
        <span style="font-weight: bold"> 4A.	How do you know it worked? <br />	
            Explain, with <u>category</u> and <u>prior year</u> context, why these results are significant for the brand’s business.  Results must relate to your specific audience, 
            objectives, and KPIs that was described earlier.  
            Provide a clear time frame for all data shown.  You must provide proof of results on a market-by-market basis, <u>results cannot be aggregated.</u>
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> Tie together the story of how your work drove the results. </li>
          <li> Entrants are strongly encouraged to <span style="font-weight:bold;">re-state their objectives from section 1 along with their corresponding results.</span></li>
          <li> Prove the results are significant using category, competitive, prior year, and brand context, and give the judges an understanding of what’s next.</li>
          <li> <span style="font-weight:bold;">Charts and graphs are strongly encouraged.</span></li>
        </ul><br />
            <p  class="efDesc">
               Note: Only results that fall between 1 Jul 2017 - 30 Sep 2018 will be evaluated by judges. 
                Dates for all results presented here should be included to prevent confusion. 
                You may include data before your start date for context purposes, 
                but do not include results after 30 Sep 2018. 
            </p>
            
            <br>
        <asp:TextBox ID="txtExplainWorkedA" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainWorkedA.ClientID %>">600</span></font>
             (Maximum: 600 words) </p>

        <br />
    <span id="txtExplainWorkedA_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainWorkedA'); return false;" />&nbsp;&nbsp;&nbsp;(up to 5 charts/graphs) 
        </span>
    <span id="txtExplainWorkedA_View"></span>
	<br /><br />
        <span style="font-weight: bold"> 4B.	Marketing communications rarely work in isolation.  Outside of your effort, what else in the marketplace could have affected the results of this case – positive or negative? Explain on a market-by-market basis.<br /><br />	
            Select factors from the chart and explain the influence of these factors in the space provided.  We recognise that attribution can be difficult; however, we’re inviting you to provide the broader picture here in making the case for your effectiveness.
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> 	 	Judges are industry executives and expect entrants to address any other factors here.  <span style="font-weight:bold;">Judges discourage entrants from</span>  responding “No Other Factors”.</li>
          <li> 	 	This is your opportunity to address what else was going on to convince judges of the impact of your case by addressing these factors. You are encouraged to use this space to address the significance or insignificance of other factors on the results achieved by your effort.</li>
          <li>  	The chart provided is a sampling of common marketplace activities, but your response is not limited to these factors.</li>
        </ul><br />
        <div id="TableRPTExplainOtherMarket"  runat="server" class="TableFix">
        <table class="tabledata" rules="all" border="1" >
            <tr>
                <td style=" font-weight: bold; ">Marketing Components</td>

                <td style="text-align: center; font-weight: bold; ">
                    Country 1<br/>
                    <asp:Label ID="lblCountryEx1" runat="server" style=" width: 230px; "> </asp:Label>
                    <%--<asp:DropDownList ID="DdlCountryEx1" runat="server" style=" width: 230px; "> </asp:DropDownList>--%>
                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Country 2<br/>
                    <asp:Label ID="lblCountryEx2" runat="server" style=" width: 230px; "> </asp:Label>
                    <%--<asp:DropDownList ID="DdlCountryEx2" runat="server" style=" width: 230px; "> </asp:DropDownList>--%>
                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Country 3<br/>
                    <asp:Label ID="lblCountryEx3" runat="server" style=" width: 230px; "> </asp:Label>
                    <%--<asp:DropDownList ID="DdlCountryEx3" runat="server" style=" width: 230px; "> </asp:DropDownList>--%>
                </td>
            </tr>
            <asp:Repeater runat="server" ID="RPTExplainOtherMarket" OnItemDataBound="RPTExplainOtherMarket_ItemDataBound" >
                <ItemTemplate>
                    <tr>
                        <td >
                            <asp:HiddenField ID="hdItemId" runat="server" />
                            <asp:Label ID="Title" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: center; "><asp:CheckBox id="cbCountry1" runat="server" /></td>
                        <td style="text-align: center; "><asp:CheckBox id="cbCountry2" runat="server" /></td>
                        <td style="text-align: center; "><asp:CheckBox id="cbCountry3" runat="server" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
             <tr>
                <td>
                    Others
                    <asp:TextBox id="txtOtherExplainOtherMarket1" runat="server" />
                </td>
                <td style="text-align: center; "><asp:CheckBox id="cbOtherExplainOtherMarket2" runat="server" /></td>
                <td style="text-align: center; "><asp:CheckBox id="cbOtherExplainOtherMarket3" runat="server" /></td>
                <td style="text-align: center; "><asp:CheckBox id="cbOtherExplainOtherMarket4" runat="server" /></td>
            </tr>
        </table>
        </div>
        <br>

        <asp:TextBox ID="txtExplainWorkedB" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainWorkedB.ClientID %>">250</span></font>
             (Maximum: 250 words) </p>
   
        <br />
    <span id="txtExplainWorkedB_Edit">
             <input type="submit" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainWorkedB'); return false;" />&nbsp;&nbsp;&nbsp;(up to 3 charts/graphs) 
        </span>
    <span id="txtExplainWorkedB_View"></span>
	<br><br>
        <%--<span style="font-weight: bold">4C.	List the countries where your case ran within the Qualifying Period.</span><br/><br/>--%>

        <asp:TextBox ID="txtExplainWorkedC" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtExplainWorkedC.ClientID %>">20</span></font>
             (Maximum: 20 words) </p>

        
    <span id="txtExplainWorkedC_Edit">
             <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainWorkedC'); return false;" />
        </span>
    <span id="txtExplainWorkedC_View"></span>
	
        <span style="font-weight: bold">Sourcing: Section 4
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>You must provide a source for all data and facts. Judges encourage third party data where available.  Sources must include the source of information, type of research, date range covered, etc.  Do not include agency names in the source of research.</li>
        </ul><br />
        <asp:TextBox ID="txtExplainWorkedD" runat="server" TextMode="MultiLine" placeholder="Provide Sourcing." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px"></p>
        <br />
        <br />
    <span id="txtExplainWorkedD_Edit">
             <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtExplainWorkedD'); return false;" />
        </span>
    <span id="txtExplainWorkedD_View"></span>
	
        <!--End section 4 -->
        <br>
        <div class="HeaderTitle">
            <font size="2" face="Verdana">
                <h2 class="headerText">MEDIA ADDENDUM – MULTI MARKET CATEGORY ONLY</h2>
            </font>
        </div>
                <p class="headerText">
                   The Media Addendum is reviewed as part of Section 3: Bringing the Idea to Life, along with your response to Question 3 and your creative work, as presented in the Creative Materials and Images of Creative.  These elements together account for 23.3% of your score.  
                </p>
        <br>
        
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                 <h2>PAID MEDIA EXPENDITURES</h2>
            </font>
        </div>
        <span style="font-weight: bold">
            Select paid media expenditures (purchased and donated), not including agency fees or production costs, for the effort described in this entry.
            Given the ‘spirit’ of this question, use your judgment on what constitutes fees, production and the broad span that covers media – from donated space to activation costs.  <br /><br />
            You are required to provide context around this budget range.  For example, if your budget has changed significantly, how this range compares to your competitors, etc.</span><br/>
            
        <br/>
        
        <table class="tabledata" rules="all" border=0" >
            <tr>
                <td style=" font-weight: bold; padding-bottom: 10px; width: 200px;" colspan="4">
                    All Amounts in USD - Indicate the size of your media budget in the chart below <u>using the following ranges</u>:
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

        <br/><br/>
        
        <div id="divPME"  runat="server" class="TableFix">
        <table class="tabledata" rules="all" border="1" >
            <tr>
                <td style=" font-weight: bold; "><b>Paid Media Expenditures</b></td>
                <td style="text-align: center; font-weight: bold; ">
                    Country 1<br/>
                    
                    <asp:Label ID="lblCountryPaid1" runat="server" style=" width: 230px; "> </asp:Label>
                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Country 2<br/>
                    <asp:Label ID="lblCountryPaid2" runat="server" style=" width: 230px; "> </asp:Label>
                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Country 3<br/>
                    <asp:Label ID="lblCountryPaid3" runat="server" style=" width: 230px; "> </asp:Label>
                </td>
            </tr>
            <asp:Repeater runat="server" ID="rptCountryPaidPart1" OnItemDataBound="rptrptCountryPaidPart1_ItemDataBound" Visible="false">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdItemId" runat="server" />
                            <asp:Label ID="Title" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox id="txtcountry1" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox id="txtcountry2" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox id="txtcountry3" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            
            <tr>
                <td><p style="font-weight:bold">Total Budget Range for this case for the current year</p> (Choose from the ranges above)</td>
                <td>
                    <asp:DropDownList ID="ddlMediaBudget1" runat="server" CssClass="rdbBlock" style=" width: 100%; " >
                    </asp:DropDownList>
                </td>
                <td>
                        <asp:DropDownList ID="ddlMediaBudget2" runat="server" CssClass="rdbBlock" style=" width: 100%; ">
                    </asp:DropDownList>
                </td>
                <td><asp:DropDownList ID="ddlMediaBudget3" runat="server" CssClass="rdbBlock" style=" width: 100%; ">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><p style="font-weight:bold">Average annual budget for this case in the prior year</p> (enter NA if not applicable)</td>
                <td>
                    <asp:TextBox ID="txtAnualBadge1" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtAnualBadge2" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtAnualBadge3" runat="server"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td><p style="font-weight:bold">Indicate the approximate % of the case's total media budget spent in each market.</p>  
                E.g. If your total media budget was X for the case over 3 markets this should = 100%.  What % out of 100% was spent in each of the markets you selected?</td>
                <td>
                    <asp:TextBox ID="txtApproximateBudget1" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtApproximateBudget2" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtApproximateBudget3" runat="server"></asp:TextBox>
                </td>
            </tr>
            <asp:Repeater runat="server" ID="rptCountryPaidPart2" OnItemDataBound="rptrptCountryPaidPart2_ItemDataBound" >
                <ItemTemplate>
                    <tr>
                        <td >
                            <asp:HiddenField ID="hdItemId" runat="server" />
                            <asp:Label ID="Title" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbnCountry1" runat="server" CssClass="rdbBlock" >
                                <asp:ListItem Value="Less" Text="Less" />
                                <asp:ListItem Value="same" Text="About the same" />
                                <asp:ListItem Value="More" Text="More" />
                                <asp:ListItem Value="NA (Elaboration Required)" Text="NA (Elaboration Required)" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                             <asp:RadioButtonList ID="rbnCountry2" runat="server" CssClass="rdbBlock">
                                <asp:ListItem Value="Less" Text="Less" />
                                <asp:ListItem Value="same" Text="About the same" />
                                <asp:ListItem Value="More" Text="More" />
                                <asp:ListItem Value="NA (Elaboration Required)" Text="NA (Elaboration Required)" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbnCountry3" runat="server" CssClass="rdbBlock">
                                <asp:ListItem Value="Less" Text="Less" />
                                <asp:ListItem Value="same" Text="About the same" />
                                <asp:ListItem Value="More" Text="More" />
                                <asp:ListItem Value="NA (Elaboration Required)" Text="NA (Elaboration Required)" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        </div>
        <br>

        <span style="font-weight: bold"> Budget Elaboration:  You are required to provide judges with the context to understand your budget.<br />	
            <br><br>For example, if your budget has changed significantly, how does this range compares to your competitors'; or, if your paid media expenditures were low and the production/other costs were high, you should elaborate here. 
            <br/><br/>If you have selected Not Applicable (NA) to either of the previous two questions, provide further explanation here.
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li>What was your distribution strategy? What was the balance of paid, earned, owned, and shared media?</li>
          <li>Did you outperform your media buy?</li>
        </ul><br />
        <asp:TextBox ID="txtBudgetElaboration" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>
        <br />
        <p style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words<%= txtBudgetElaboration.ClientID %>">100</span></font>
             (Maximum: 100 words)</p>
        <br />
    <span id="txtBudgetElaboration_Edit">
             <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtBudgetElaboration'); return false;" />
        </span>
    <span id="txtBudgetElaboration_View"></span>
	
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                 <h2>OWNED MEDIA</h2>
            </font>
        </div>
        <span style="font-weight: bold"> 	
            Elaborate on owned media (digital or physical company-owned real estate), that acted as communication channels for case content. 
		&nbsp;&nbsp;
        <br/><br/>Effie Tips: </span><br/><br/>

        <ul class="IconList">
          <li> Owned media examples may include a corporate website, social media platforms, packaging, a branded store, fleet of buses, etc.</li>
          <li> If owned media platforms were selected in the Communications Touchpoints chart, judges will expect your explanation of those platform(s) in your response. Similarly, any owned media described here must relate back to the touchpoints selected in the chart below.</li>
        </ul><br />
        <asp:TextBox ID="txtOwnedMedia" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>

        <br />
    <span id="txtOwnedMedia_Edit">
             
	    <input type="submit" style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtOwnedMedia'); return false;" />
        </span>
    <span id="txtOwnedMedia_View"></span>
        <br/><br/>
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                 <h2>SPONSORSHIPS</h2>
            </font>
        </div>
        <span style="font-weight: bold">
            Provide details regarding your sponsorships, if applicable. If not, please mark “NA”.
		&nbsp;&nbsp;</span><br/><br/>
        <asp:TextBox ID="txtSponsorship" runat="server" TextMode="MultiLine" placeholder="Provide Answer." CssClass="TextCtm" ></asp:TextBox>

        <br />
    <span id="txtSponsorship_Edit">
             <input type="submit"  style="display:none;" value="Upload charts/graphs" onclick="return UploadCharts('txtSponsorship'); return false;" />
        </span>
    <span id="txtSponsorship_View"></span>
	
        <br/><br/>
        <div class="HeaderTitleGray">
            <font size="2" face="Verdana">
                 <h2>COMMUNICATIONS TOUCHPOINTS</h2>
            </font>
        </div>
        <span style="font-weight: bold">
           Select all touchpoints used in the effort based on the options provided in the chart.  
            Within your response to Question 3, explain which touchpoints were integral in reaching your audience and why. 
		</span>
            <br>
            <br>
            <p  class=""><em>
                Notes: 
                <br>On the Creative Materials, you must show at least one complete example of each communication touchpoint that was integral to the effort’s success. For example, 
                if you mark 30 boxes below and 10 were key to driving the results and indicated as integral in Question 3, those 10 must be featured in the Creative Materials.</em>
            </p>

         <span style="font-weight: bold"><br/><br/>Effie Tips: <br/><br/>

        <ul class="IconList">
          <li> On the Creative Materials, you must show at least one complete example of each communication touchpoint that was <u>integral</u> to the effort’s success.  For example, if you mark 20 boxes below and 10 were key to driving the results and indicated as integral in Question 3, those 10 must be featured in the Creative Materials.</li>
        </ul><br />
        <div id="divrptCTP"  runat="server" class="TableFix">
        <table class="tabledata" rules="all" border="1" >
            <tr>
                <td></td>
                <td style="text-align: center; font-weight: bold; ">
                    Country 1<br />
                    <asp:Label ID="lbl11ACountry1" runat="server" style=" width: 230px; "> </asp:Label>
                </td>
                <td style="text-align: center; font-weight: bold; ">
                    Country 2<br /> 
                    <asp:Label ID="lbl11ACountry2" runat="server" style=" width: 230px; "> </asp:Label>
                </td>
                <td style="text-align: center; font-weight: bold; ">
                     Country 3<br />
                    <asp:Label ID="lbl11ACountry3" runat="server" style=" width: 230px; "> </asp:Label>
                </td>
            </tr>
            <asp:Repeater runat="server" ID="rptCTP" OnItemDataBound="rptCTP_ItemDataBound" >
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdItemId" runat="server" />
                            <asp:HiddenField ID="hdAttrType" runat="server" />
                            <asp:Label ID="Title" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: center; "><asp:CheckBox id="cbCountry1" runat="server" /></td>
                        <td style="text-align: center; "><asp:CheckBox id="cbCountry2" runat="server" /></td>
                        <td style="text-align: center; "><asp:CheckBox id="cbCountry3" runat="server" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td>
                    <asp:CheckBox style="margin-top: 7px;" ID="cbOtherCTP1" runat="server" />
                    Other:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtOtherCTP2"  runat="server" MaxLength="100" />
                </td>
            </tr>
        </table>
    </div>
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
