using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Main_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    //    string emailBody = "";
    //    Effie2017.App.EntryList List = Effie2017.App.EntryList.GetEntryList();

    //    List<Effie2017.App.Entry> ProcesList = new List<Effie2017.App.Entry>();
    //    Guid curentId = Guid.Empty;

    //    foreach (Effie2017.App.Entry entry in List)
    //    {
    //        if (curentId != entry.RegistrationId)
    //        {
    //            if (ProcesList.Count != 0)
    //            {
    //                //process
    //                #region Group DQ
    //                //ProcesList Order by DQ
    //                //ProcesList

    //                List<Effie2017.App.Entry> ProcesList2 = new List<Effie2017.App.Entry>(); ProcesList2.OrderBy()
    //                string curentDQ = string.Empty;

    //                foreach (Effie2017.App.Entry entry2 in ProcesList)
    //                {
    //                    if (curentDQ != entry2.FlagReason)
    //                    {
    //                        if (ProcesList2.Count != 0)
    //                        {
    //                            //process
    //                            #region Group Title
    //                            //ProcesList Order by Title
    //                            //ProcesList

    //                            List<Effie2017.App.Entry> ProcesList3 = new List<Effie2017.App.Entry>();
    //                            string curentTitle = string.Empty;

    //                            foreach (Effie2017.App.Entry entry3 in ProcesList2)
    //                            {
    //                                if (curentTitle != entry3.Title)
    //                                {
    //                                    if (ProcesList3.Count != 0)
    //                                    {
    //                                        //process
    //                                        string Head = "";
    //                                        foreach (Effie2017.App.Entry entry4 in ProcesList3)
    //                                        {
    //                                            Head += entry4.Serial + ", ";
    //                                        }
    //                                        Head += ProcesList3[0].Title;

    //                                        string DQList = "";
    //                                        foreach (Effie2017.App.Entry entry4 in ProcesList3[0].FlagReason)
    //                                        {
    //                                            DQList += entry4.Serial + ", ";
    //                                        }
    //                                        DQList += ProcesList3[0].Title;

    //                                        emailBody += "";


    //                                        ProcesList3.Clear();
    //                                    }
    //                                    else
    //                                    {
    //                                        ProcesList3.Add(entry);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    ProcesList3.Add(entry);
    //                                }

    //                                curentTitle = entry3.Title;
    //                            }
    //                            #region

    //                            ProcesList2.Clear();
    //                        }
    //                        else
    //                        {
    //                            ProcesList2.Add(entry);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        ProcesList2.Add(entry);
    //                    }

    //                    curentDQ = entry2.FlagReason;
    //                }
    //                #region

    //                ProcesList.Clear();
    //            }
    //            else
    //            {
    //                ProcesList.Add(entry);
    //            }
    //        }
    //        else
    //        {
    //            ProcesList.Add(entry);
    //        }

    //        curentId = entry.RegistrationId;
    //    }
    }
}