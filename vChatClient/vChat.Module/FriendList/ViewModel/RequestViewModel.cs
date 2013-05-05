using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using vChat.Model.Entities;
using System.Windows.Input;

namespace vChat.Module.FriendList
{
    public partial class RequestViewModel : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void RequestHandler(Users Friend);
        public event RequestHandler OnAcceptRequest;
        public event RequestHandler OnIgnoreRequest;

        #region CLASS MEMBER

        private readonly ICommand acceptCommand;
        private readonly ICommand ignoreCommand;

        private readonly ObservableCollection<RequestViewModel> requests;
        private readonly Users friend;
        private bool isSelected;
        private bool isIgnored = true; //Default
        private bool isAccepted;

        #endregion

        #region PROPERTY

        public ICommand AcceptCommand
        {
            get { return acceptCommand; }
        }

        public ICommand IgnoreCommand
        {
            get { return ignoreCommand; }
        }

        public Users Friend
        {
            get { return friend; }
        }

        public String FriendName
        {
            get { return (String.Format("{0} {1}", friend.FirstName, friend.LastName)); }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool IsIgnored
        {
            get { return isIgnored; }
            set
            {
                if (value != isIgnored)
                {
                    isIgnored = value;
                    this.OnPropertyChanged("IsIgnored");
                }
            }
        }

        public bool IsAccepted
        {
            get { return isAccepted; }
            set
            {
                if (value != isAccepted)
                {
                    isAccepted = value;
                    this.OnPropertyChanged("IsAccepted");
                }
            }
        }

        public ObservableCollection<RequestViewModel> Requests
        {
            get { return requests; }            
        }

        #endregion

        private RequestViewModel(Users Friend)
        {
            friend = Friend;
        }

        public RequestViewModel(List<Users> Friends)
        {
            requests = new ObservableCollection<RequestViewModel>(
                    (from Friend in Friends
                    select new RequestViewModel(Friend))
                    .ToList()
                );
            
            acceptCommand = new AcceptTask(this);
            ignoreCommand = new IgnoreTask(this);
        }

        #region MAIN METHOD

        public void AppendRequest(Users Friend)
        {
            if (Friend == null)
                return;

            requests.Add(new RequestViewModel(Friend));
        }

        public void RemoveRequest(Users Friend)
        {
            if (Friend == null)
                return;

            RequestViewModel MatchRequest = requests.FirstOrDefault(r => r.Friend.Equals(Friend));

            if (MatchRequest != null)
                requests.Remove(MatchRequest);
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region COMMAND PERFORM

        private void AcceptFriend()
        {
            List<RequestViewModel> SelectedRequest = requests.Where(r => r.IsSelected).ToList();

            foreach (RequestViewModel request in SelectedRequest)
                OnAcceptRequest(request.Friend);
        }

        private void IgnoreFriend()
        {
            List<RequestViewModel> SelectedRequest = requests.Where(r => r.IsSelected).ToList();

            foreach (RequestViewModel request in SelectedRequest)
                OnIgnoreRequest(request.Friend);
        }

        #endregion
                
    }
}
