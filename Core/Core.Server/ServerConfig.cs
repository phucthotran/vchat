using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;

namespace Core.Server
{
    public class ServerConfig
    {
        private string _IPStr;

        public EndPoint IP { get; set; }
        public int Port { get; set; }
        public int Limit { get; set; }

        public ServerConfig(bool autoInit)
        {
            if (autoInit)
            {
                bool success = CheckingAppConfig("IP", "Port", "Limit");
                if (success)
                {
                    this._IPStr = ConfigurationManager.AppSettings["IP"];
                    this.Port = ConfigurationManager.AppSettings["Port"].ToInt();
                    this.IP = new IPEndPoint(IPAddress.Parse(this._IPStr), this.Port);
                    this.Limit = ConfigurationManager.AppSettings["Limit"].ToInt();
                }
                else
                    throw new FailedAppConfigException("App.config has changed. The system need to be restore the origin config file.");
            }
        }

        public bool CheckingAppConfig(params string[] keys)
        {
            foreach (string key in keys)
            {
                if (ConfigurationManager.AppSettings[key] == null)
                    return false;
            }
            return true;
        }
    }

    [Serializable]
    public class FailedAppConfigException : Exception
    {
        public FailedAppConfigException() : base() { }
        public FailedAppConfigException(string message) : base(message) { }
    }
}
