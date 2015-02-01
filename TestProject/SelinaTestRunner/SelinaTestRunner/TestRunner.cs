using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace SelinaTestRunner
{
    class TestRunner
    {
        private Process process;
        private string TestRunDirectory;
        private string testName;
        private Client client;
        private RunSpace runSpace;
        private TestRunnerStatus status;

        public TestRunner(RunSpace runSpace, string testName, Client assignedClient) {
            this.runSpace = runSpace;
            TestRunDirectory = runSpace.TestRunDirectory;
            this.testName = testName;
            this.client = assignedClient;
        }

        public void RunTest()
        {
            StaticUtilities.Log("Running test " + testName + " on machine " + client.Address);
            status = TestRunnerStatus.Running;
            setUpConfig(client.Address);
            RunTestProcess(testName);            
        }
                
        private void setUpConfig(string clientAddress)
        {
            var config = new Config()
            {
                clientUrl = clientAddress + "/wd/hub"
            };
            var configString = JsonConvert.SerializeObject(config);

            try
            {
                using (StreamWriter sWriter = new StreamWriter(TestRunDirectory + "\\config.json"))
                {
                    sWriter.WriteLine(configString);
                    sWriter.Flush();
                }
            }
            catch (Exception ex) {
                StaticUtilities.Log(ex);
            }
        }

        private void RunTestProcess(string testName) {
            try
            {
                var processStartInfo = new ProcessStartInfo();

                processStartInfo.FileName = "mstest";
                //processStartInfo.FileName = "vstest.console.exe";
                processStartInfo.Arguments = "/testcontainer:CampaignUI.dll " +
                    "/testsettings:PPE.testsettings " +
                    "/test:" + testName;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.WorkingDirectory = TestRunDirectory;

                process = Process.Start(processStartInfo);

                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(processExited);
                process.WaitForExit();
            }
            catch (Exception ex) {
                StaticUtilities.Log(ex);
            }
        }

        private void processExited(object sender, System.EventArgs e)
        {
            StaticUtilities.Log("TestName  : " + testName + " ran on machine with ip : " + client.Address + " exited with code " + process.ExitCode); 

            runSpace.SaveTestResults();

            runSpace.status = RunSpaceStatus.Free;
            client.status = ClientStatus.Free;
            status = TestRunnerStatus.Exited;
        }              
    }

    public class Config {
        public string clientUrl { get; set; }
    }

    public enum TestRunnerStatus { 
        Running = 0,
        Exited = 1
    }
}
