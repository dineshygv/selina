using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelinaTestRunner
{
    class RunSpaceManager
    {
        private List<RunSpace> RunSpaces;

        public RunSpaceManager() {
            RunSpaces = new List<RunSpace>();
        }

        public RunSpace GetFreeRunSpace() {
            if (RunSpaces.Count > 0) {
                foreach (var runSpace in RunSpaces) {
                    if (runSpace.status == RunSpaceStatus.Free) {
                        runSpace.CleanRunSpace();
                        return runSpace;                        
                    }
                }
            }
            var newRunSpace = new RunSpace();
            RunSpaces.Add(newRunSpace);
            return newRunSpace;
        }
    }
}
