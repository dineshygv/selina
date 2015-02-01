using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SelinaTestRunner
{
    class RunSpace
    {
        public string TestRunDirectory;
        private string WorkingDirectory;
        private int RunInstance;
        public RunSpaceStatus status;
        private string TestResultsDirectoryPath;
        private string AccumulatedTestResultsDirectory;
        private string TestAssemblyPath;

        public RunSpace() {
            this.status = RunSpaceStatus.Free;
            RunInstance = new Random().Next();

            WorkingDirectory = StaticUtilities.GetWorkingDirectoryPath();
            TestRunDirectory = WorkingDirectory + @"TestRuns\TestRun" + RunInstance;
            TestResultsDirectoryPath = TestRunDirectory + @"\TestResults\";
            AccumulatedTestResultsDirectory = WorkingDirectory + @"\TestResults\";
            TestAssemblyPath = StaticUtilities.GetTestAssemblyPath();

            SetUpAssemblies();
        }

        private void SetUpAssemblies()
        {
            if (!Directory.Exists(TestRunDirectory))
            {
                try
                {
                    Directory.CreateDirectory(TestRunDirectory);
                }
                catch (Exception ex)
                {
                    StaticUtilities.Log(ex);
                }
            }
            StaticUtilities.CopyDirectory(TestAssemblyPath, TestRunDirectory);
        }

        public void CleanRunSpace() {      
            var configFilePath = TestRunDirectory + "\\config.json";
            if (File.Exists(configFilePath))
            {
                var configFile = new FileInfo(configFilePath);
                try
                {
                    configFile.Delete();
                }
                catch (Exception ex)
                {
                    StaticUtilities.Log(ex);
                }
            }
            else {
                StaticUtilities.Log("config.json is not found during clean up");
            }

            if (Directory.Exists(TestResultsDirectoryPath))
            {
                StaticUtilities.DeleteDirectory(TestResultsDirectoryPath);
            }
        }

        public void SaveTestResults() {
            string[] trxFilePaths = null;
            try
            {
                trxFilePaths = Directory.GetFiles(TestResultsDirectoryPath, "*.trx");
            }
            catch (Exception ex) {
                StaticUtilities.Log(ex);
            }

            if (trxFilePaths != null && trxFilePaths.Length > 0)
            {
                FileInfo testResultFile = null;
                try
                {
                    testResultFile = new FileInfo(trxFilePaths[0]);
                }
                catch (Exception ex)
                {
                    StaticUtilities.Log(ex);
                }

                if (!Directory.Exists(AccumulatedTestResultsDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(AccumulatedTestResultsDirectory);
                    }
                    catch(Exception ex)
                    {
                        StaticUtilities.Log(ex);
                    }
                }

                var newTestResultPath = AccumulatedTestResultsDirectory + Guid.NewGuid().ToString() + ".trx";

                if (newTestResultPath != null && !File.Exists(newTestResultPath))
                {
                    try
                    {
                        testResultFile.CopyTo(newTestResultPath);
                    }
                    catch (Exception ex)
                    {
                        StaticUtilities.Log(ex);
                    }
                }
                else {
                    StaticUtilities.Log("test results file not found, expected path : " + newTestResultPath);
                }                
            }
        }
        
    }

    public enum RunSpaceStatus { 
        Free = 0,
        Busy = 1
    }
}
