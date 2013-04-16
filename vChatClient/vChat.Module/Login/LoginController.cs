using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Data;
using System.Windows;
using Core.Client;
using System.Windows.Controls;
using vChat.Service.UserService;
using vChat.Model;
using System.Collections;
using System.Configuration;
using vChat.Model.Entities;

namespace vChat.Module.Login
{
    public partial class Login : UserControl
    {        
        private bool DoLogin(string user, string pass, bool isRemember)
        {            
            MethodInvokeResult result = this.Get<UserServiceClient>().Login(user, pass);
            if (result.Status == MethodInvokeResult.RESULT.SUCCESS)
            {
                DoConnect(user);
                RememberAccount(isRemember, user, pass);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoLoginSuccess(string user)
        {
            Users tmpUser = this.Get<UserServiceClient>().FindName(user);

            Users userLogged = new Users { UserID = tmpUser.UserID, Username = tmpUser.Username };
            this.Get<Core.Client.Client>().ID = userLogged.UserID;
            OnLoginSuccess(userLogged);
        }

        private void DoConnect(string user)
        {
            Client client = this.Get<Client>();
            if (!client.Socket.Connected)
                client.Connect();
            client.SendCommand(null, CommandType.LogIn, "SERVER", user);
            client.Name = user;
        }

        private void RememberAccount(bool isRemember, string user, string pass)
        {
            Cookie cookie = Cookie.Instance;
            if (isRemember)
            {
                cookie.Set("user", user, false);
                cookie.Set("pass", pass, true);
                cookie.Set("expire", DateTime.UtcNow.AddDays(3).ToFileTimeUtc().ToString(), false);
            }
            else
            {
                cookie.Unset("user");
                cookie.Unset("pass");
                cookie.Unset("expire");
            }
            cookie.Save();
        }

        private string RememberedUser()
        {
            Cookie cookie = Cookie.Instance;
            if (cookie.Isset("expire"))
            {
                if ((long.Parse(cookie["expire"].ToString()) - DateTime.UtcNow.ToFileTimeUtc()) > 0)
                {
                    if (this.Get<UserServiceClient>().LoginHash(cookie["user"].ToString(), cookie["pass"].ToString()).Status == MethodInvokeResult.RESULT.SUCCESS)
                    {
                        return cookie["user"].ToString();
                    }
                }
            }
            return null;
        }
    }

    public class Cookie : Hashtable
    {
        public static Cookie Instance
        {
            get { return new Cookie(); }
        }
        private static Configuration _config;
        static Cookie()
        {
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configFile = System.IO.Path.Combine(appPath, "app.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            _config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }
        public void Set(string name, string value, bool isHash)
        {
            if (isHash)
                value = vChat.Lib.MD5Encrypt.Hash(value);
            _config.AppSettings.Settings.Add(name, value);
        }
        public void Unset(string name)
        {
            _config.AppSettings.Settings.Remove(name);
        }
        public void Unset(params string[] name)
        {
            foreach (string n in name)
            {
                Unset(n);
            }
        }
        public bool Isset(string name)
        {
            return (_config.AppSettings.Settings[name] != null);
        }
        public void Save()
        {
            _config.Save();
        }
        public override object this[object key]
        {
            get
            {
                return _config.AppSettings.Settings[key.ToString()].Value;
            }
        }
    }
}
