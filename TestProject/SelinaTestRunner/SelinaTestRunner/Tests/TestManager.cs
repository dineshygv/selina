using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelinaTestRunner
{
    class TestManager
    {
        private List<string> testList;
        private int lastExecutedTestIndex; 

        public TestManager() {
            testList = GetListOfTests();
            lastExecutedTestIndex = -1;
        }

        public string GetNextTestToExecute() {
            lastExecutedTestIndex++;
            if (lastExecutedTestIndex == testList.Count) {
                return null;
            }
            return testList[lastExecutedTestIndex];
        }

        private List<string> GetListOfTests() {
            var testQuery = new TestQuery();
            var workingDirectory = StaticUtilities.GetWorkingDirectoryPath();
            var testList = testQuery.GetTestNames(workingDirectory + @"\TestAssembly\CampaignUI.dll", "CampaignUI.CampaignFeatures");
            return testList;
        }
    }
}
