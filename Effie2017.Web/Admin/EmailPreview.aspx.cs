using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_EmailPreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string emailTemplateId = Request.QueryString["etmId"];

        if (emailTemplateId != null)
        {
            if (IptechLib.Validation.ValidateGuid(emailTemplateId))
            {
                EmailTemplate emailtemplate = EmailTemplate.GetEmailTemplate(new Guid(emailTemplateId));

                ltrlPreviewText.Text = emailtemplate.Body;
            }
        }

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    public void LoadForm()
    {

    }

    public void PopulateForm()
    {

    }

   
}