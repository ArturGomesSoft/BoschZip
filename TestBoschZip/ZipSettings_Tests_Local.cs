using BoschZip;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBoschZip
{
    [TestFixture]
    public class ZipSettings_Tests_Local
    {
        private ZipSettings settings;

        [SetUp]
        public void Setup()
        {
            settings = ZipSettings.FromArgs(ResourceClasses.GetArgs_LocalFile());
        }

        [Test]
        public void ZipSettings_CanLoad()
        {
            Assert.IsNotNull(settings, "Settings should have been read successfully.");
        }

        [Test]
        public void ZipSettings_OutputType_LocalFile()
        {
            Assert.IsTrue(settings.OutputType == OutputTypes.LocalFile, "Output type should be 'local file'.");
        }

        [Test]
        public void ZipSettings_LocalFilename()
        {
            string file = @"c:\temp\BoschZipFile.zip";
            Assert.IsTrue(settings.OutputFile.Equals(file, StringComparison.OrdinalIgnoreCase), "Output file not correctly");
        }

        [Test]
        public void ZipSettings_ExcludeFile_FolderInside()
        {
            string testString = "folderinside";
            Assert.IsTrue(settings.ExcludingFolders
                .Any(x => x.Equals(testString, StringComparison.OrdinalIgnoreCase)),
                "'FolderInside' folder should be in the exclusion list");
        }

        [Test]
        public void ZipSettings_ExcludeFile_Documents()
        {
            string testString = "documents";
            Assert.IsFalse(settings.ExcludingFolders
                .Any(x => x.Equals(testString, StringComparison.OrdinalIgnoreCase)),
                "'Documents' folder should not be in the exclusion list");
        }

        [Test]
        public void ZipSettings_ExcludeFile_File1()
        {
            string testString = "BoschTestFile.txt";
            Assert.IsTrue(settings.ExcludingFiles
                .Any(x => x.Equals(testString, StringComparison.OrdinalIgnoreCase)),
                "'BoschTestFile.txt' filename should be in the exclusion list");
        }

        [Test]
        public void ZipSettings_ExcludeFile_File2()
        {
            string testString = "file2.txt";
            Assert.IsFalse(settings.ExcludingFiles
                .Any(x => x.Equals(testString, StringComparison.OrdinalIgnoreCase)),
                "'File2.txt' filename should not be in the exclusion list");
        }

        [Test]
        public void ZipSettings_Override_Output()
        {
            Assert.IsTrue(settings.OverrideOutput, "The OverrideOutput property should be set.");
        }

        [TestCase("xml")]
        [TestCase("doc")]
        [TestCase("dll")]
        public void ZipSettings_Exclude_Extensions(string value)
        {
            Assert.IsTrue(settings.ExcludingExtensions
                .Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase)),
                $"'{value}' extension should be in the exclusion list");
        }

        [TestCase("txt")]
        [TestCase("cs")]
        [TestCase("zip")]
        public void ZipSettings_Exclude_Extensions_Not(string value)
        {
            Assert.IsFalse(settings.ExcludingExtensions
                .Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase)),
                $"'{value}' extension should not be in the exclusion list");
        }

    }
    
}
