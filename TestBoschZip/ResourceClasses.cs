using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestBoschZip
{
    public static class ResourceClasses
    {
        public static string[] GetArgs_LocalFile()
        {
            return new string[]
            {
                @"-if:c:\temp\BoschZipTests",
                @"-of:c:\temp\BoschZipFile.zip",
                @"-xe:xml,doc,dll",
                @"-xd:folderinside",
                @"-xf:BoschTestFile.txt",
                @"-rf"
            };
        }

        public static string[] GetArgs_SMTP()
        {
            return new string[]
            {
                @"-if:c:\temp\BoschZipTests",
                @"-of:c:\temp\BoschZipFile.zip",
                @"-xe:xml,doc,dll",
                @"-xd:folderinside",
                @"-xf:BoschTestFile.txt",
                @"-rf",
                @"-ot:3",
                @"-op:my_email@my.server.com"
            };
        }

        public static void CreateLocalFileStructure(string folder)
        {
            try
            {
                CreateFoldersAndFiles(folder);
                for (int x = 1; x <= 3; x++)
                {
                    CreateFoldersAndFiles(Path.Combine(folder, $"SubFolder{x}"));
                    if(x == 2)
                    {
                        for (int y = 1; y <= 2; y++)
                        {
                            CreateFoldersAndFiles(Path.Combine(Path.Combine(folder, "SubFolder2"), $"SubSubFolder{y}"));
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CreateFoldersAndFiles(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Create two files;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("TEST-TEST-TEST-TEST");
                sb.AppendLine(" ");
                sb.AppendLine("This is test file for BoschZip.");
                sb.AppendLine(" ");
                sb.AppendLine("Please, do not consider this file, nor its extension.");
                sb.AppendLine(" ");
                sb.AppendLine("TEST-TEST-TEST-TEST");
                sb.AppendLine();

                string file = $"BoschTestFile.txt";

                if (!File.Exists(file))
                {
                    File.WriteAllText(Path.Combine(folder, file), sb.ToString());
                }

                file = "BoschTestFile.doc";
                if (!File.Exists(file))
                {
                    File.WriteAllText(Path.Combine(folder, file), sb.ToString());
                }

                file = "BoschTestFile.dll";
                if (!File.Exists(file))
                {
                    File.WriteAllText(Path.Combine(folder, file), sb.ToString());
                }

                file = "BoschTestFile.xml";
                if (!File.Exists(file))
                {
                    File.WriteAllText(Path.Combine(folder, file), sb.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void CreateLocalOutputTestFile(string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Create two files;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("TEST-TEST-TEST-TEST");
                sb.AppendLine(" ");
                sb.AppendLine("This is supposed to be a output file for testing BoschZip.");
                sb.AppendLine(" ");
                sb.AppendLine("Please, do not consider this file, nor its extension.");
                sb.AppendLine(" ");
                sb.AppendLine("TEST-TEST-TEST-TEST");
                sb.AppendLine();

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.WriteAllText(filePath, sb.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
