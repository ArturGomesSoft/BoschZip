using SimpleImpersonation;
using System;
using System.IO;
using System.Security.Principal;

namespace BoschZip
{
    public class FileShareSender : ISender
    {
        FileShareSettings settings;
        string destinationPath;

        public FileShareSender(FileShareSettings settings, string destinationPath)
        {
            this.settings = settings;
            this.destinationPath = destinationPath;
        }

        public ResultObj Send(string fileToSend)
        {
            ResultObj retVal = new ResultObj();

            if (string.IsNullOrEmpty(fileToSend) || !File.Exists(fileToSend))
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine("Invalid file to be sent.");
                return retVal;
            }

            if (!this.destinationPath.StartsWith(@"\\"))
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine("The provided path does not look like a network share path.");
                return retVal;
            }

            try
            {
                string destFile = Path.Combine(this.destinationPath, Path.GetFileName(fileToSend));

                if (settings.UseCredentials)
                {
                    //TODO: Finish the logic to connect the network drive.
                    //      Probably the best approach in dotNet Core is to use 'SimpleImpersonation' NuGet package

                    UserCredentials credentials = new UserCredentials(settings.Domain, settings.UserName, settings.Password);
                    using (var token = credentials.LogonUser(LogonType.NewCredentials))
                    {
                        WindowsIdentity.RunImpersonated(token, () =>
                        {
                            File.Copy(fileToSend, destFile, true);
                        });
                    };
                }
                else
                {
                    // For the following approach to success, the network share must be available
                    // and the current windows user must have enough rights
                    File.Copy(fileToSend, destFile, true);
                }
                retVal.ResultType = ResultTypeEnum.Success;

            }
            catch (Exception ex)
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine(ex.Message);
            }
            return retVal;

        }

    }
}
