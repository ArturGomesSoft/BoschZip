using System;
using System.IO;

namespace BoschZip
{
    public class LocalFileSender : ISender
    {
        LocalFileSettings settings;
        string destinationFolder = "";

        public LocalFileSender(LocalFileSettings settings, string destinationFolder)
        {
            this.settings = settings;
            this.destinationFolder = destinationFolder;
        }

        public ResultObj Send(string fileToSend)
        {
            ResultObj retVal = new();

            if (string.IsNullOrEmpty(fileToSend) || !File.Exists(fileToSend))
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine("Invalid file to be sent.");
                return retVal;
            }

            if (settings.MakeLocalExtraCopy && !Directory.Exists(destinationFolder))
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine("The provided path does not exist.");
                return retVal;
            }

            try
            {
                string destFile = Path.Combine(this.destinationFolder, Path.GetFileName(fileToSend));

                if (settings.MakeLocalExtraCopy)
                {
                    // For the following approach to success, the current windows
                    // user must have enough rights on the specified folder
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
