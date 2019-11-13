<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

        Exception exp = (HttpUnhandledException)Server.GetLastError();

        if (exp is HttpUnhandledException)
        {
            try {
                string emailSubject = System.Configuration.ConfigurationManager.AppSettings["emailSubjectError"].ToString();
                string emailErrorList = System.Configuration.ConfigurationSettings.AppSettings["emailToError"];
                string Previous = "";
                string Current = "";
                string emailMessage = "";
                try { Current = Request.Url.AbsoluteUri; } catch { }
                try { Previous = Request.UrlReferrer.AbsoluteUri; } catch { }
                try { emailMessage =
                    "<div style=' background: white; '>" +
                       "SESSION Effie.Security.Login.Administrator: " + HttpContext.Current.Session["Effie.Security.Login.Administrator"] + "<br>" +
                       "SESSION Effie.Security.Login.Registration: " + HttpContext.Current.Session["Effie.Security.Login.Registration"] + "<br>" +
                       "Current URL : " + Current + "<br>" +
                       "Previous URL : " + Previous + "<br>" +
                       "Datetime : " + DateTime.Now + "<br>" +
                   "<div>" +
                   "<br><br><br> ERROR : <br>" + ((HttpUnhandledException)exp).GetHtmlErrorMessage() + "<br><br><br><br>";
                }
                catch { }
                if (!string.IsNullOrEmpty(emailErrorList))
                {
                    foreach(string emailTo in emailErrorList.Split(','))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(emailTo))
                            {
                                Email.SendMail(emailTo, "support@iptech.com.sg", "Support IPTECH", "", "", emailSubject, emailMessage, true, null, null, false);
                            }
                        }
                        catch
                        {
                            //SEND TODO
                            //Email.SendMail(emailTo, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", "", emailSubject, emailMessage, true, null, null);
                        }
                    }
                }

            }
            catch { }

            //Response.Redirect(System.Configuration.ConfigurationSettings.AppSettings["WebUrl"]);
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
