using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.ObjectModel;

namespace Core.Server.ClientManagement
{
    public class ClientCollection : Collection<Client>
    {
        public ClientCollection()
        {
        }

        public Client GetClient(string name)
        {
            try
            {
                return this.First(c => c.Name.Equals(name));
            }
            catch
            {
                return null;
            }
        }
    }
}
