using System.Threading;
using System.Collections.Generic;
using System;

namespace SelinaTestRunner
{
    class MultiTestRunner
    {
        private ClientManager clientManager;
        private TestManager testManager;
        private RunSpaceManager runSpaceManager;
        private System.Timers.Timer timer;
        private Dictionary<string, TestRunner> runningTests;
        private List<Thread> runningThreads;

        public MultiTestRunner()
        {
            clientManager = new ClientManager();
            runSpaceManager = new RunSpaceManager();
            testManager = new TestManager();
            runningTests = new Dictionary<string, TestRunner>();
            runningThreads = new List<Thread>();

            timer = new System.Timers.Timer();
            //timer.Interval = 100000000;
            timer.Interval = 10000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerEvent);
        }

        public void RunTests(){
            timer.Enabled = true;
            TimerEvent(null, null);
        }

        public void TimerEvent(object sender, System.Timers.ElapsedEventArgs args) {
            //var freeClients = clientManager.GetFreeClients();
            var freeClients = clientManager.GetFreeClientsHubMock();
            if (freeClients.Count > 0) {
                for (int index = 0; index < freeClients.Count; index++ ){
                //for (int index = 0; index < 1; index++){                
                    var client = freeClients[index];
                    client.status = ClientStatus.Busy;

                    var runSpace = runSpaceManager.GetFreeRunSpace();
                    runSpace.status = RunSpaceStatus.Busy;

                    var testToExecute = testManager.GetNextTestToExecute();

                    if (testToExecute == null)
                    {
                        StaticUtilities.Log("Test run completed");
                        timer.Enabled = false;
                        return;
                    }

                    var testRunner = new TestRunner(runSpace, testToExecute, client);
                    runningTests.Add(testToExecute, testRunner);
                    Thread newThread = null;
                    try
                    {
                        newThread = new Thread(new ThreadStart(testRunner.RunTest));
                        newThread.Start();
                    }
                    catch (Exception ex)
                    {
                        StaticUtilities.Log(ex);
                    }

                    if (newThread != null)
                    {
                        runningThreads.Add(newThread);
                    }
                }
            }
        }

       
    }
}
