using System;
using System.Collections.Generic;
using System.Text;

namespace BoschZip
{
    public class ZipSettings
    {
        public ZipSettings()
        {
        }

        public static ZipSettings FromArgs(string[] args)
        {
            ZipSettings settings = new ZipSettings();
            ParseArguments(args, ref settings);
            return settings;
        }

        public string InitialFolder { get; set; } = string.Empty;

        public string OutputFile { get; set; } = string.Empty;

        public List<string> ExcludingExtensions { get; set; } = null;

        public List<string> ExcludingFolders { get; set; } = null;

        public List<string> ExcludingFiles { get; set; } = null;

        public OutputTypes OutputType { get; set; } = OutputTypes.LocalFile;

        public string OutputOption { get; set; } = string.Empty;

        public bool OverrideOutput { get; set; } = false;

        public bool ShowHelp { get; set; } = false;

        public ResultObj Error { get; set; } = null;

        public string GetHelpString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("Parameters syntax:");
            sb.AppendLine("---------------------");
            sb.AppendLine("BoschZip.exe -<par>:<value(s)> ");
            sb.AppendLine("Ex: ");
            sb.AppendLine("BoschZip.exe -if:C:\\temp\\ -of:C:\\temp\\MyZipfile.zip -xe:doc,zip -ot:3 -op:\"myEmail@server.com\" -rf ");
            sb.AppendLine("");
            sb.AppendLine("par    description");
            sb.AppendLine("---    -----------");
            sb.AppendLine("-if     initial folder");
            sb.AppendLine("-of     output file");
            sb.AppendLine("-xe     excluding extensions (comma separated)");
            sb.AppendLine("-xd     excluding folders (comma separated)");
            sb.AppendLine("-xf     excluding filenames (comma separated)");
            sb.AppendLine("-ot     output type ");
            sb.AppendLine("           1 - local file");
            sb.AppendLine("           2 - file share location ");
            sb.AppendLine("           3 - Email recipient (SMTP) ");
            sb.AppendLine("-op     output parameter for -ot 2 (share path) and 3 (email address) ");
            sb.AppendLine("-rf     Override output file, if present");
            sb.AppendLine("");
            sb.AppendLine("If any other thing, this help should be displayed.");
            sb.AppendLine("");
            if (this.Error != null && this.Error.IsFailure)
            {
                sb.AppendLine("Error message:");
                sb.AppendLine(" " + this.Error.Message);
            }

            return sb.ToString();
        }

        public Boolean ValidSettings()
        {
            if (this.InitialFolder != string.Empty
                && this.OutputFile != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ParseArguments(string[] args, ref ZipSettings settings)
        {
            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    string sepStr = "";
                    string valStr = "";
                    if (arg.StartsWith("-"))
                    {
                        if ((arg.Length >= 4) && (arg.Substring(3, 1) == ":"))
                        {
                            sepStr = arg.Substring(1, 2);
                            valStr = arg.Substring(4);
                        }
                        else if (arg.Length == 3)
                        {
                            sepStr = arg.Substring(1, 2);
                            if (sepStr.ToUpper() == "RF")
                            {
                                settings.OverrideOutput = true;
                                continue;
                            }
                        }
                        else
                        {
                            settings.ShowHelp = true;
                            break;
                        }
                    }
                    else
                    {
                        settings.ShowHelp = true;
                        break;
                    }
                    switch (sepStr.ToUpper())
                    {
                        case "IF":
                            settings.InitialFolder = valStr;
                            break;
                        case "OF":
                            settings.OutputFile = valStr;
                            break;
                        case "XE":
                            settings.ExcludingExtensions = GetStringList(valStr);
                            break;
                        case "XD":
                            settings.ExcludingFolders = GetStringList(valStr);
                            break;
                        case "XF":
                            settings.ExcludingFiles = GetStringList(valStr);
                            break;
                        case "OT":
                            settings.OutputType = GetOutputType(valStr);
                            if (settings.OutputType == OutputTypes.none)
                            {
                                settings.ShowHelp = true;
                                settings.Error = new ResultObj(ResultTypeEnum.Failure, $"Unknown output type: {sepStr}");
                            }
                            break;
                        case "OP":
                            settings.OutputOption = valStr;
                            break;
                        default:
                            settings.ShowHelp = true;
                            settings.Error = new ResultObj(ResultTypeEnum.Failure, $"Unknown parameter: {sepStr}");
                            break;
                    }
                }
            }
        }

        private static List<string> GetStringList(string strList)
        {
            var list = new List<string>();
            var ar = strList.Split(',');
            if (ar.Length > 0)
            {
                foreach (string arg in ar)
                {
                    list.Add(arg);
                }
            }
            return list;
        }

        private static OutputTypes GetOutputType(string strVal)
        {
            OutputTypes retVal = OutputTypes.none;

            if (int.TryParse(strVal, out int num))
            {
                if (Enum.IsDefined(typeof(OutputTypes), num))
                {
                    retVal = (OutputTypes)num;
                }
            }
            return retVal;
        }

    }

    public enum OutputTypes
    {
        none = 0,
        LocalFile = 1,
        FileShare = 2,
        SMTP = 3
    }

}
