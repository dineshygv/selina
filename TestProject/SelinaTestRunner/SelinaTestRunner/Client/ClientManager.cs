using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace SelinaTestRunner
{
    class ClientManager
    {
        private Dictionary<string, Client> clientList;
        private List<Client> grandHubClients;

        public ClientManager() {
            clientList = new Dictionary<string, Client>();
            grandHubClients = new List<Client>();
            for (int index = 0; index < 20; index++)
            {
                grandHubClients.Add(new Client("http://134.170.220.187:84/"));
            }
        }

        public List<Client> GetFreeClientsHubMock() { 
            List<Client> freeClients = new List<Client>();
            for (int index = 0; index < grandHubClients.Count; index++)
            {
                if (grandHubClients[index].status == ClientStatus.Free) {
                    freeClients.Add(grandHubClients[index]);
                }
            }
            return freeClients;
        }

        public List<Client> GetFreeClients()
        {
            var freeClients = new List<Client>();

            var connectClients = GetListOfClients();

            if (connectClients.Count > 0) {
                foreach (var clientAddress in connectClients) {
                    Client clientInstance;
                    if (clientList.ContainsKey(clientAddress))
                    {                        
                        clientList.TryGetValue(clientAddress, out clientInstance);
                        if (clientInstance.status == ClientStatus.Free)
                        {
                            freeClients.Add(clientInstance);
                        }
                    }
                    else {
                        clientInstance = new Client(clientAddress);
                        clientList.Add(clientAddress, clientInstance);
                        freeClients.Add(clientInstance);
                    }
                }
            }

            return freeClients;
        }

        private static List<string> GetListOfClients()
        {
            MatchCollection matches = null;
            try
            {
                WebClient webClient = new WebClient();
                string s = webClient.DownloadString("http://localhost:4444/grid/console");
                matches = Regex.Matches(s, "remoteHost:[^<]*");

            }
            catch(Exception ex){
                StaticUtilities.Log(ex);
            }

            List<string> hosts = new List<string>();

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string host = match.ToString().Substring(11);
                    hosts.Add(host);
                }
            }
            
            return hosts;
        }

    }
}
