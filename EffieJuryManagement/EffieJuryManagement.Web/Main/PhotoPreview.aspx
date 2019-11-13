<%@ Page Language="C#" %>

<script runat="server" language="c#">
    Guid juryGuid = Guid.Empty;
    string juryGuidString = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        juryGuidString = Request.QueryString["juryId"];
        string token = Request.QueryString["t"];

        if (juryGuidString != null)
        {
            if (IptechLib.Validation.ValidateGuid(IptechLib.Crypto.StringDecryption(juryGuidString)))
            {
                EffieJuryManagementApp.Jury jury = null;

                try
                {
                    Guid.TryParse(IptechLib.Crypto.StringDecryption(juryGuidString), out juryGuid);

                    jury = EffieJuryManagementApp.Jury.GetJury(juryGuid);
                }
                catch { }

                if (!IsPostBack)
                {
                    PopulateForm(jury);
                }
            }
            else
                return;
        }
    }

    private void PopulateForm(EffieJuryManagementApp.Jury jury)
    {
        if (jury != null)
        {
            byte[] imgBytes = null;

            try
            {
                imgBytes = System.IO.File.ReadAllBytes(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id.ToString() + ".jpg");
            }
            catch { }

            if (imgBytes != null)
                imgPhoto.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(imgBytes);
        }
    }
</script>
<html>
<head>
    <style>
        body
        {
            margin: 0;
        }
        
        .outer-container
        {
            position: absolute;
            display: table;
            width: 100%; /* This could be ANY width */
            height: 100%; /* This could be ANY height */
            background: #ccc;
        }
        
        .inner-container
        {
            display: table-cell;
            vertical-align: middle;
            text-align: center;
        }
        
        .centered-content
        {
            display: inline-block;
            text-align: left;
            background: #fff;
            padding: 20px;
            border: 1px solid #000;
        }
    </style>
</head>
<body>
    <div class="outer-container">
        <div class="inner-container">
            <div class="centered-content">
                <asp:Image runat="server" ID="imgPhoto" Style="height: auto; width: auto; max-height: 255px;
                    max-width: 357px;" />
            </div>
        </div>
    </div>
</body>
</html>
