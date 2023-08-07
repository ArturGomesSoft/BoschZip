using BoschZip;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestBoschZip
{
    [TestFixture]
    public class ZipSender_Tests_Local
    {
        private LocalFileSender sender = null;
        private LocalFileSettings settings;
        string destinationFolder = @"c:\temp\BoschZipTempFolder";
        string fileToSend = @"c:\temp\BoschZipSendingFile.zip";

        [SetUp]
        public void Setup()
        {
            settings = new LocalFileSettings()
            {
                MakeLocalExtraCopy = true,
            };
            // Create local test zip file
            ResourceClasses.CreateLocalOutputTestFile(fileToSend);

            // Create destination folder
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Path to extra copy
            sender = new LocalFileSender(settings, destinationFolder);
        }

        [Test]
        public void ZipSender_Initialized()
        {
            Assert.IsNotNull(sender, "Sender not initialized.");
        }

        [Test]
        public void ZipSender_CanSend()
        {
            ResultObj res = sender.Send(fileToSend);
            Assert.IsTrue(res.IsSuccess, "Sender was unable to send the output file.");
        }


    }
}
