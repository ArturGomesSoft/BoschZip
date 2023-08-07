using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Text;



namespace BoschZip
{
    public class ZipWorker
    {
        private ZipSettings _settings;
        public ZipWorker(ZipSettings settings)
        {
            _settings = settings ?? new ZipSettings();
        }

        public ResultObj CheckResources()
        {
            ResultObj retVal = new ResultObj(ResultTypeEnum.Success);
            StringBuilder sb = new StringBuilder();
            DirectoryInfo dirInfo = new DirectoryInfo(_settings.InitialFolder);

            if (!dirInfo.Exists)
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine("Initial folder does not exist.");
            }
            if (!_settings.OverrideOutput)
            {
                FileInfo fileInfo = new FileInfo(_settings.OutputFile);
                if (fileInfo.Exists)
                {
                    retVal.ResultType = ResultTypeEnum.Failure;
                    retVal.Message.AppendLine("Destination file already exists.");
                }
            }
            return retVal;
        }

        private bool GetFilesToInclude(string directory, ref List<string> filesToZip)
        {
            if (filesToZip == null)
            {
                filesToZip = new List<string>();
            }

            DirectoryInfo dir = new DirectoryInfo(directory);
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string fileExt = file.Extension.Trim('.');
                    if (_settings.ExcludingExtensions != null && _settings.ExcludingExtensions.Any(x => x.Equals(fileExt, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }
                    if (_settings.ExcludingFiles != null && _settings.ExcludingFiles.Any(x => x.Equals(file.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }
                    filesToZip.Add(file.FullName);
                }
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo di in dirs)
                {
                    if (_settings.ExcludingFolders != null && _settings.ExcludingFolders.Any(x => x.Equals(di.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }
                    GetFilesToInclude(di.FullName, ref filesToZip);
                }
            }
            if (filesToZip.Count > 0)
            {
                return true;
            }
            return false;
        }

        public ResultObj DoZip()
        {
            var retVal = this.CheckResources();
            if (retVal.IsFailure)
            {
                return retVal;
            }
            retVal.ResultType = ResultTypeEnum.Success;
            retVal.Message.Clear();

            try
            {
                if (File.Exists(_settings.OutputFile) && _settings.OverrideOutput)
                {
                    File.Delete(_settings.OutputFile);
                }

                List<string> files = new List<string>();

                if (GetFilesToInclude(_settings.InitialFolder, ref files))
                {
                    using (ZipArchive zipFile = ZipFile.Open(_settings.OutputFile, ZipArchiveMode.Create))
                    {
                        foreach (string file in files)
                        {
                            //string fileInZip = Path.GetRelativePath(_settings.InitialFolder, file);
                            
                            string fileInZip =  file.Substring(_settings.InitialFolder.Length);
                            //string fileInZip = Directory.GetParent(_settings.InitialFolder, file);

                            zipFile.CreateEntryFromFile(file, fileInZip);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine(ex.Message);
            }
            return retVal;
        }

        public ResultObj DoSend()
        {
            ResultObj retVal = new ResultObj();

            // Check for the file to send
            if (!File.Exists(_settings.OutputFile))
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine("Output (zip) file not found.");
                return retVal;
            }

            ISender sender = null;
            // Send accordingly
            if (_settings.OutputType == OutputTypes.SMTP)
            {
                SMTPSettings sendSettings = new SMTPSettings()
                {
                    SenderName = "Bosch Zipper Sender",
                    SenderAddress = "bosch_zipper@my.server.com",
                    Password = "sender_password",
                    SmtpHost = "smtp.gmail.com",
                    SmtpPort = 587,
                    SmtpDeliverymethod = SmtpDeliveryMethod.Network,
                    SmtpUserDefaultCredentials = false,
                    SmtpEnableSsl = true,
                };

                sender = new SMTPSender(sendSettings, _settings.OutputOption);
            }
            else if (_settings.OutputType == OutputTypes.FileShare)
            {
                FileShareSettings sendSettings = new FileShareSettings()
                {
                    Domain = "128.0.0.1",
                    UserName = "UserName",
                    Password = "password",
                    UseCredentials = true,
                };
                sender = new FileShareSender(sendSettings, _settings.OutputOption);
            }
            else if (_settings.OutputType == OutputTypes.LocalFile)
            {
                LocalFileSettings sendSettings = new LocalFileSettings()
                {
                    MakeLocalExtraCopy = _settings.OutputOption == string.Empty ? false : true
                };
                sender = new LocalFileSender(sendSettings, _settings.OutputOption);
            }

            // Set the sender do do its stuff
            if (sender != null)
            {
                retVal = sender.Send(_settings.OutputFile);
            }

            return retVal;
        }


    }
}
