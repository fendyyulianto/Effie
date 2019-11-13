using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Amazon.ElasticTranscoder;
using Amazon.ElasticTranscoder.Model;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Effie2017.App;
using System.Globalization;

namespace VideoSync
{
    class Program
    {
        public static Log logClass;

        static void Main(string[] args)
        {
            Log logMST = new Log();
            logMST.Begin();
            logMST.WriteLog("**********************************************");
            logMST.WriteLog("-------Service Start-------");
            logMST.WriteLog("DateTime: " + DateTime.Now.ToString());
            logMST.End();
           
            // Upload all videos to S3 Bucket and Transcode
            GetAllFiles();

            // Generate Report containing Entry Videos URL from Amazon S3
            //GenerateReport();

            // Read Entry Serial No from text file and upload it to Amazon S3
            //ReadEntryFromFile();

            // Generate Report for Entries from the text file
            //GenerateReportFromFile();

            // Get Missing Entries Serial No from Amazon S3
            //GetMissingEntries();

            //GetMissingFiles();

            // Update Entries to Round 2 from the Text File
            //UpdateEntryFromFiletoRound2();

            Log logMST2 = new Log();
            logMST2.Begin();
            logMST2.WriteLog("-------Service Stop-------");
            logMST2.WriteLog("DateTime: " + DateTime.Now.ToString());
            logMST2.WriteLog("**********************************************");
            logMST2.End(); 
        }

        public static void TranscodeLargeFiles()
        {
            #region Log "Result Check - START"
            logClass = new Log();
            logClass.Begin();
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Started Reading Videos...");
            logClass.WriteLog("----------------------------------------------");
            #endregion

            
            var sortedFiles = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles()
                                                  .OrderBy(f => f.LastWriteTime)
                                                  .ToList();

            int counter = 0;
            foreach (FileInfo file in sortedFiles)
            {
                logClass.WriteLog("Reading Video : " + file.Name);

                if (file.Length >= Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumCutOffSize"]))
                {
                    if (FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name))
                    {
                        DeleteFileInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);
                    }

                    logClass.WriteLog("Transcoding Video : " + file.Name);

                    ConvertHDtoLD_Video(file.Name, System.Configuration.ConfigurationSettings.AppSettings["AWSPipeLineID"], System.Configuration.ConfigurationSettings.AppSettings["AWSPresetID"]);

                    counter++;

                    logClass.WriteLog("No of Videos processed : " + counter);
                }
            }

            #region Log "Result Check - END"
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Finished Reading Videos...");
            logClass.WriteLog("----------------------------------------------");
            logClass.End();
            #endregion

        }

        public static void CopySmallFiles()
        {
            #region Log "Result Check - START"
            logClass = new Log();
            logClass.Begin();
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Started Reading Videos...");
            logClass.WriteLog("----------------------------------------------");
            #endregion

            int maximumFilesToRead = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumFiles"].ToString());
            var sortedFiles = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles()
                                                  .OrderBy(f => f.LastWriteTime).Take(maximumFilesToRead)
                                                  .ToList();

            int counter = 0;
            foreach (FileInfo file in sortedFiles)
            {
                logClass.WriteLog("Reading Video : " + file.Name);

                if (file.Length < Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumCutOffSize"]))
                {
                    if (FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name))
                    {
                        DeleteFileInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);
                    }

                    logClass.WriteLog("Copying Video : " + file.Name);

                    CopyFilesFromBucketToBucket(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name, System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);

                    counter++;

                    logClass.WriteLog("No of Videos processed : " + counter);
                }               
            }

            #region Log "Result Check - END"
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Finished Reading Videos...");
            logClass.WriteLog("----------------------------------------------");
            logClass.End();
            #endregion
 
        }

        public static void GetAllFiles()
        {
            //Last Sync System
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Effie2017.App.Gen_GeneralUseValueList generalUseValueList = Effie2017.App.Gen_GeneralUseValueList.GetGen_GeneralUseValueList("AWSLastSync");
            DateTime lastSyncDateTime = Convert.ToDateTime(generalUseValueList[0].Value);
            DateTime nowSyncDateTime = DateTime.Now;
            //END Last Sync System

            #region Log "Result Check - START"
            logClass = new Log();
            logClass.Begin();
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Started Reading Videos...");
            logClass.WriteLog("----------------------------------------------");
            #endregion

            List<FileInfo> sortedFiles = new List<FileInfo>();
            
            DateTime latestProgramRunDate = GetLatestProgramDate();
            int maximumFilesToRead = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumFiles"].ToString());

            //var entryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.Completed)
            //                .Where(m => m.DateSubmitted > latestProgramRunDate && m.DateSubmitted > Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["Extended_2_CutOff"])).Take(maximumFilesToRead)
            //                .OrderBy(m => m.DateSubmitted).ToList();
            var entryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "")
                            .Take(maximumFilesToRead)
                            .OrderBy(m => m.DateSubmitted).ToList();

            //Check List
            int c = 0;
            foreach(Entry en in entryList)
            {
                c++;

                logClass.WriteLog(c.ToString() + ": " + en.Serial);
            }
            //End Check List

            var videFileList = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles().ToList();

            c = 0;
            foreach (FileInfo file in videFileList)
            {
                c++;
                logClass.WriteLog(c.ToString() + ": " + file.Name + "=>" + file.Name.Split('_')[0]);

                string entryId = file.Name.Split('_')[0];                

                Entry entry = null;

                try
                {
                    entry = entryList.Where(m => m.Serial.Equals(entryId)).Single();
                }
                catch{}
                
                if (entry != null && entry.MaterialsSubmitted != "" && DateTime.Parse(entry.MaterialsSubmitted, cultureInfo) >= lastSyncDateTime)
                {
                    file.LastAccessTime = entry.DateSubmitted;
                    
                    sortedFiles.Add(file);
                }
            }

            sortedFiles = sortedFiles.OrderBy(m => m.LastAccessTime).ToList();
            
            //sortedFiles = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles().Where(m => m.LastWriteTime > latestProgramRunDate)
            //                                      .OrderBy(f => f.LastWriteTime).Take(maximumFilesToRead)
            //                                      .ToList();


            int counter = 0;
            foreach (FileInfo file in sortedFiles)
            {
                string fileSize = (file.Length / 1048576).ToString("N");

                logClass.WriteLog("Uploading Video : " + file.Name + " FileSize :" + fileSize + " MB");

                UploadFileToAmazonS3(file.OpenRead(), System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name);

                if (FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name))
                {
                    DeleteFileInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);
                }

                if (file.Length < Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumCutOffSize"]))
                {
                    logClass.WriteLog("Copying Video : " + file.Name);                  
                    CopyFilesFromBucketToBucket(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name, System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);                    
                }
                else if (file.Length >= Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumCutOffSize"]))
                {
                    logClass.WriteLog("Transcoding Video : " + file.Name);
                    ConvertHDtoLD_Video(file.Name, System.Configuration.ConfigurationSettings.AppSettings["AWSPipeLineID"], System.Configuration.ConfigurationSettings.AppSettings["AWSPresetID"]);
                }

                logClass.WriteLog("Updating Setting File");

               
                using (FileStream stream = new FileStream(System.Configuration.ConfigurationSettings.AppSettings["SettingsFilePath"].ToString(), FileMode.Create))
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(file.LastAccessTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
                }

                

                counter++;

                logClass.WriteLog("No of Videos processed : " + counter);
            }
            
            //Update Last Sync System
            Effie2017.App.Gen_GeneralUseValue generalUseValue = Effie2017.App.Gen_GeneralUseValue.GetGen_GeneralUseValue(generalUseValueList[0].Id);
            generalUseValue.Value = nowSyncDateTime.ToString();
            generalUseValue.DateModifiedString = DateTime.Now.ToString();
            generalUseValue.Save();
            //END Update Last Sync System

            #region Log "Result Check - END"
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Finished Reading Videos...");
            logClass.WriteLog("----------------------------------------------");
            logClass.End();
            #endregion
        }

        public static void GetMissingFiles()
        {
            #region Log "Result Check - START"
            logClass = new Log();
            logClass.Begin();
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Started Reading Videos...");
            logClass.WriteLog("----------------------------------------------");
            #endregion

            List<FileInfo> sortedFiles = new List<FileInfo>();
           
            var entryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.Completed)
                             .OrderBy(m => m.DateSubmitted).ToList();

            var videFileList = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles().ToList();
            
            foreach (FileInfo file in videFileList)
            {
                string entryId = file.Name.Split('_')[0];

                Entry entry = null;

                try
                {
                    entry = entryList.Where(m => m.Serial.Equals(entryId)).Single();
                }
                catch { }

                if (entry != null)
                {
                    if (!FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name))
                    {
                        sortedFiles.Add(file);                        
                    }
                }
            }

            sortedFiles = sortedFiles.OrderBy(m => m.LastAccessTime).ToList();

           
            int counter = 0;
            foreach (FileInfo file in sortedFiles)
            {
                string fileSize = (file.Length / 1048576).ToString("N");

                logClass.WriteLog("Uploading Video : " + file.Name + " FileSize :" + fileSize + " MB");

                UploadFileToAmazonS3(file.OpenRead(), System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name);

                if (FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name))
                {
                    DeleteFileInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);
                }

                if (file.Length < Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumCutOffSize"]))
                {
                    logClass.WriteLog("Copying Video : " + file.Name);
                    CopyFilesFromBucketToBucket(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name, System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);
                }
                else if (file.Length >= Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumCutOffSize"]))
                {
                    logClass.WriteLog("Transcoding Video : " + file.Name);
                    ConvertHDtoLD_Video(file.Name, System.Configuration.ConfigurationSettings.AppSettings["AWSPipeLineID"], System.Configuration.ConfigurationSettings.AppSettings["AWSPresetID"]);
                }

                logClass.WriteLog("Updating Setting File");
                
                counter++;

                logClass.WriteLog("No of Videos processed : " + counter);
            }


            #region Log "Result Check - END"
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Finished Reading Videos...");
            logClass.WriteLog("----------------------------------------------");
            logClass.End();
            #endregion
        }

        public static DateTime GetLatestProgramDate()
        {
            DateTime dt = DateTime.MinValue;
            CultureInfo cultureInfo = new CultureInfo("en-US");

            using (StreamReader sw = new StreamReader(System.Configuration.ConfigurationSettings.AppSettings["SettingsFilePath"].ToString()))
            {
                string datetimeSetting = sw.ReadToEnd().Trim().ToString();
                dt = DateTime.Parse(datetimeSetting, cultureInfo);
                return dt;
            }
        }

        public static void UploadFileToAmazonS3(FileStream fu, string bucketName, string fileName)
        {
            if (fu != null)
            {
                if (FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], fileName))
                {
                    DeleteFileInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], fileName);
                }

                Amazon.S3.AmazonS3Client s3Client = new Amazon.S3.AmazonS3Client();

                PutObjectRequest request = new PutObjectRequest();
                request.InputStream = fu;
                request.BucketName = bucketName;
                request.CannedACL = S3CannedACL.PublicRead;
                request.Key = fileName;
                s3Client.PutObject(request);                
            }
            else
                throw new System.Exception("File Empty.");
        }

        public static void ConvertHDtoLD_Video(string fileName, string pipeLineID, string presetID)
        {
            AmazonElasticTranscoderClient etsClient = new AmazonElasticTranscoderClient();
            var response = etsClient.CreateJob(new CreateJobRequest()
            {
                PipelineId = pipeLineID, //pipeline.Id,
                Input = new JobInput()
                {
                    AspectRatio = "auto",
                    Container = "mp4",
                    FrameRate = "auto",
                    Interlaced = "auto",
                    Resolution = "auto",                    
                    Key = fileName
                },
                Output = new CreateJobOutput()
                {
                    ThumbnailPattern = "",
                    Rotate = "0",
                    PresetId = presetID,
                    Key = fileName
                }
            });
        }

        public static bool FileExistsInAmazonS3(string bucketName, string fileName)
        {
            using (AmazonS3Client client = new AmazonS3Client())
            {
                S3FileInfo s3FileInfo = new S3FileInfo(client, bucketName, fileName);
                if (s3FileInfo.Exists)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void DeleteFileInAmazonS3(string bucketName, string fileName)
        {
            using (AmazonS3Client client = new AmazonS3Client())
            {
                S3FileInfo s3FileInfo = new S3FileInfo(client, bucketName, fileName);
                if (s3FileInfo.Exists)
                {
                    s3FileInfo.Delete();
                }

            }
        }

        public static void CopyFilesFromBucketToBucket(string sourceBucket,string sourceFileName,string destinationBucket,string destinationFileName)
        {
            using (AmazonS3Client client = new AmazonS3Client())
            {
                CopyObjectRequest request = new CopyObjectRequest()
                {
                    SourceBucket = sourceBucket,
                    SourceKey = sourceFileName,
                    DestinationBucket = destinationBucket,
                    DestinationKey = destinationFileName
                };
                CopyObjectResponse response = client.CopyObject(request);
            }
        }

        protected static void GenerateReport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No");
            dt.Columns.Add("File Name");
            dt.Columns.Add("Original Size (MB)");
            dt.Columns.Add("Date");
            dt.Columns.Add("URL");


            List<FileInfo> sortedFiles = new List<FileInfo>();

            DateTime latestProgramRunDate = GetLatestProgramDate();

            //var entryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.Completed)
            //                .Where(m => m.DateSubmitted > Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["Extended_2_CutOff"]))
            //                .OrderBy(m => m.DateSubmitted).ToList();

            var entryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.Completed)                            
                            .OrderBy(m => m.DateSubmitted).ToList();


            var videFileList = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles().ToList();

            foreach (FileInfo file in videFileList)
            {
                string entryId = file.Name.Split('_')[0];

                Entry entry = null;

                try
                {
                    entry = entryList.Where(m => m.Serial.Equals(entryId)).Single();
                }
                catch { }

                if (entry != null)
                {
                    file.LastAccessTime = entry.DateSubmitted;

                    sortedFiles.Add(file);
                }
            }

            sortedFiles = sortedFiles.OrderBy(m => m.LastAccessTime).ToList();

            int counter = 1;
            foreach (FileInfo file in sortedFiles)
            {              
                if (FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name))
                {
                    DataRow dr = dt.NewRow();

                    dr["No"] = counter.ToString();
                    dr["File Name"] = file.Name;
                    dr["Original Size (MB)"] = file.Length / 1048576;
                    dr["Date"] = file.LastAccessTime;
                    dr["URL"] = System.Configuration.ConfigurationSettings.AppSettings["AWSS3WebURL"] + System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"] + "/" + file.Name;

                    dt.Rows.Add(dr);

                    counter++;
                }
            }

            CreateCSVFile(dt, System.Configuration.ConfigurationSettings.AppSettings["LogFileFolder"] + DateTime.Now.Ticks.ToString() + ".csv");
            
        }

        protected static void GenerateReportFromFile()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No");
            dt.Columns.Add("File Name");
            dt.Columns.Add("Original Size (MB)");
            dt.Columns.Add("Date");
            dt.Columns.Add("URL");


            List<FileInfo> sortedFiles = new List<FileInfo>();

            var videoFileList = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles().ToList();
            var entriesToRead = File.ReadAllLines(System.Configuration.ConfigurationSettings.AppSettings["EntryFilePath"].ToString());
            
            foreach (var entry in entriesToRead)
            {
                if (!String.IsNullOrEmpty(entry))
                {
                    foreach (FileInfo file in videoFileList)
                    {
                        string entryId = file.Name.Split('_')[0];

                        if (entry.Trim().ToLower().Equals(entryId.Trim().ToLower()))
                        {
                            sortedFiles.Add(file);
                        }
                    }
                }
            }            

            sortedFiles = sortedFiles.OrderBy(m => m.LastAccessTime).ToList();

            int counter = 1;
            foreach (FileInfo file in sortedFiles)
            {
                if (FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name))
                {
                    DataRow dr = dt.NewRow();

                    dr["No"] = counter.ToString();
                    dr["File Name"] = file.Name;
                    dr["Original Size (MB)"] = file.Length / 1048576;
                    dr["Date"] = file.LastAccessTime;
                    dr["URL"] = System.Configuration.ConfigurationSettings.AppSettings["AWSS3WebURL"] + System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"] + "/" + file.Name;

                    dt.Rows.Add(dr);

                    counter++;
                }
            }

            CreateCSVFile(dt, System.Configuration.ConfigurationSettings.AppSettings["LogFileFolder"] + DateTime.Now.Ticks.ToString() + ".csv");

        }

        protected static void GetMissingEntries()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No");
            dt.Columns.Add("Entry Id");
            dt.Columns.Add("File Name");
            dt.Columns.Add("Original Size (MB)");
            dt.Columns.Add("Date");

            List<FileInfo> sortedFiles = new List<FileInfo>();

            var entryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.Completed)                            
                            .OrderBy(m => m.DateSubmitted).ToList();

            var videFileList = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles().ToList();

            int counter = 1;
            foreach (FileInfo file in videFileList)
            {
                string entryId = file.Name.Split('_')[0];

                Entry entry = null;

                try
                {
                    entry = entryList.Where(m => m.Serial.Equals(entryId)).Single();
                }
                catch { }

                if (entry != null)
                {
                    if (!FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name))
                    {
                        DataRow dr = dt.NewRow();

                        dr["No"] = counter.ToString();
                        dr["Entry Id"] = entryId;
                        dr["File Name"] = file.Name;
                        dr["Original Size (MB)"] = file.Length / 1048576;
                        dr["Date"] = file.LastAccessTime;

                        dt.Rows.Add(dr);

                        counter++;
                    }
                }
            }
           
            CreateCSVFile(dt, System.Configuration.ConfigurationSettings.AppSettings["LogFileFolder"] +  "Missing_Entry_List.csv");
        }

        public static void CreateCSVFile(DataTable dt, string strFilePath)
        {
            #region Export Grid to CSV
            // Create the CSV file to which grid data will be exported.
            StreamWriter sw = new StreamWriter(strFilePath, false);
            // First we will write the headers.
            //DataTable dt = m_dsProducts.Tables[0];
            int iColCount = dt.Columns.Count;

            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            // Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();

            #endregion

        }

        protected static void ReadEntryFromFile()
        {
            #region Log "Result Check - START"
            logClass = new Log();
            logClass.Begin();
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Started Reading Entries from Files...");
            logClass.WriteLog("----------------------------------------------");
            #endregion

            var videoFileList = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["VideosFileFolder"].ToString()).GetFiles().ToList();
            var entriesToRead = File.ReadAllLines(System.Configuration.ConfigurationSettings.AppSettings["EntryFilePath"].ToString());

            int counter = 0;

            foreach (var entry in entriesToRead)
            {
                if (!String.IsNullOrEmpty(entry))
                {
                    foreach (FileInfo file in videoFileList)
                    {
                        string entryId = file.Name.Split('_')[0];

                        if (entry.Trim().ToLower().Equals(entryId.Trim().ToLower()))
                        {
                            string fileSize = (file.Length / 1048576).ToString("N");

                            logClass.WriteLog("Uploading Video : " + file.Name + " FileSize :" + fileSize + " MB");

                            UploadFileToAmazonS3(file.OpenRead(), System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name);

                            if (FileExistsInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name))
                            {
                                DeleteFileInAmazonS3(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);
                            }

                            if (file.Length < Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumCutOffSize"]))
                            {
                                logClass.WriteLog("Copying Video : " + file.Name);
                                CopyFilesFromBucketToBucket(System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Original"], file.Name, System.Configuration.ConfigurationSettings.AppSettings["AWSBucket_Small"], file.Name);
                            }
                            else if (file.Length >= Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaximumCutOffSize"]))
                            {
                                logClass.WriteLog("Transcoding Video : " + file.Name);
                                ConvertHDtoLD_Video(file.Name, System.Configuration.ConfigurationSettings.AppSettings["AWSPipeLineID"], System.Configuration.ConfigurationSettings.AppSettings["AWSPresetID"]);
                            }
                        }
                    }

                    counter++;
                    logClass.WriteLog("No of Entries processed : " + counter);
                }                
            }

            #region Log "Result Check - END"
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Finished Reading Entries from Files...");
            logClass.WriteLog("----------------------------------------------");
            logClass.End();
            #endregion
        }

        protected static void UpdateEntryFromFiletoRound2()
        {
            #region Log "Result Check - START"
            logClass = new Log();
            logClass.Begin();
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Started Reading Entries from Files...");
            logClass.WriteLog("----------------------------------------------");
            #endregion
            
            var entriesToRead = File.ReadAllLines(System.Configuration.ConfigurationSettings.AppSettings["EntryFilePath"].ToString());
            
            int counter = 0;

            foreach (var entry in entriesToRead)
            {
                if (!String.IsNullOrEmpty(entry))
                {
                    logClass.WriteLog("Processing : " + entry);

                    List<Entry> entryList = EntryList.GetEntryList(Guid.Empty,Guid.Empty,entry).ToList();

                    if (entryList.Count == 1)
                    {
                        entryList[0].IsRound2 = true;

                        entryList[0].Save();
                        logClass.WriteLog("Processed : " + entry);
                    }
                    
                    counter++;                    
                }
            }

            logClass.WriteLog("No of Entries processed : " + counter);

            #region Log "Result Check - END"
            logClass.WriteLog("----------------------------------------------");
            logClass.WriteLog("...Finished Reading Entries from Files...");
            logClass.WriteLog("----------------------------------------------");
            logClass.End();
            #endregion
        }
    }

    public static class StatusEntry
    {
        public const string Draft = "DFT";
        public const string Ready = "RDY";
        public const string PaymentPending = "PPN";
        public const string UploadPending = "UPN";
        public const string UploadCompleted = "UPC";
        public const string Completed = "OK";
    }
}
