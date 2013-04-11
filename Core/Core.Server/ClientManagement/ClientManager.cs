using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Core.Server.ClientManagement
{
    public class ClientManager
    {
        private List<Client> m_ClientList;
        public int Count
        {
            get { return this.m_ClientList.Count; }
        }
        public List<Client> List 
        {
            get { return this.m_ClientList; }
        }
        public ClientManager()
        {
            this.m_ClientList = new List<Client>();
        }
        public void Add(Client client)
        {
            this.m_ClientList.Add(client);
        }
        public void Clear()
        {
            this.m_ClientList.Clear();
        }
        public Client GetClient(string name)
        {
            return this.m_ClientList.Where(c => c.User.Equals(name)).SingleOrDefault<Client>();
        }
        public void Remove(Client client)
        {
            this.m_ClientList.Remove(client);
        }
        public void RemoveAt(int index)
        {
            this.m_ClientList.RemoveAt(index);
        }

        // Properties


    }
}
