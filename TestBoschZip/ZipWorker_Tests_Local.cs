using BoschZip;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System;
using System.IO;

namespace TestBoschZip
{
    [TestFixture]
    public class ZipWorker_Tests_Local
    {
        private ZipWorker worker = null;
        private ZipSettings settings;

        [SetUp]
        public void Setup()
        {
            settings = ZipSettings.FromArgs(ResourceClasses.GetArgs_LocalFile());

            string folder = settings.InitialFolder;
            // Create local structure
            ResourceClasses.CreateLocalFileStructure(folder);

            // Try to create a worker
            worker = new ZipWorker(settings);
        }

        [Test]
        public void ZipWorker_Initialized()
        {
            Assert.IsNotNull(worker, "Worker not initialized.");
        }

        [Test]
        public void ZipWorker_CanZip()
        {
            ResultObj res = worker.DoZip();
            Assert.IsTrue(res.IsSuccess, "Worker should be able to zip the test files.");
        }

        [Test]
        public void ZipWorker_CanZip_CreateFile()
        {
            ResultObj res = worker.DoZip();
            Assert.IsTrue(File.Exists(settings.OutputFile), "The output file was not created");
        }

        [Test]
        public void ZipWorker_CanSend()
        {
            ResultObj res = worker.DoZip();
            res = worker.DoSend();
            Assert.IsTrue(res.IsSuccess, "Should be able to zip and send the output file.");
        }


    }
}