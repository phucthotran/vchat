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

        private readonly ICommand _AcceptCommand;
        private readonly ICommand _IgnoreCommand;

        private readonly ObservableCollection<RequestViewModel> _Requests;
        private readonly Users _Friend;
        private bool _IsSelected;
        private bool _IsIgnored = true; //Default
        private bool _IsAccepted;

        #endregion

        #region PROPERTY

        public ICommand AcceptCommand
        {
            get { return _AcceptCommand; }
        }

        public ICommand IgnoreCommand
        {
            get { return _IgnoreCommand; }
        }

        public Users Friend
        {
            get { return _Friend; }
        }

        public String FriendName
        {
            get { return (String.Format("{0} {1}", _Friend.FirstName, _Friend.LastName)); }
        }

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool IsIgnored
        {
            get { return _IsIgnored; }
            set
            {
                if (value != _IsIgnored)
                {
                    _IsIgnored = value;
                    this.OnPropertyChanged("IsIgnored");
                }
            }
        }

        public bool IsAccepted
        {
            get { return _IsAccepted; }
            set
            {
                if (value != _IsAccepted)
                {
                    _IsAccepted = value;
                    this.OnPropertyChanged("IsAccepted");
                }
            }
        }

        public ObservableCollection<RequestViewModel> Requests
        {
            get { return _Requests; }            
        }

        #endregion

        private RequestViewModel(Users Friend)
        {
            _Friend = Friend;
        }

        public RequestViewModel(List<Users> Friends)
        {
            _Requests = new ObservableCollection<RequestViewModel>(
                    (from Friend in Friends
                    select new RequestViewModel(Friend))
                    .ToList()
                );
            
            _AcceptCommand = new AcceptTask(this);
            _IgnoreCommand = new IgnoreTask(this);
        }

        #region MAIN METHOD

        public void AppendRequest(Users Friend)
        {
            if (Friend == null)
                return;

            _Requests.Add(new RequestViewModel(Friend));
        }

        public void RemoveRequest(Users Friend)
        {
            if (Friend == null)
                return;

            RequestViewModel MatchRequest = _Requests.FirstOrDefault(r => r.Friend.Equals(Friend));

            if (MatchRequest != null)
                _Requests.Remove(MatchRequest);
        }

        #endregion

        #region COMMAND PERFORM

        private void AcceptFriend()
        {
            List<RequestViewModel> SelectedRequest = _Requests.Where(r => r.IsSelected).ToList();

            foreach (RequestViewModel request in SelectedRequest)
                OnAcceptRequest(request.Friend);
        }

        private void IgnoreFriend()
        {
            List<RequestViewModel> SelectedRequest = _Requests.Where(r => r.IsSelected).ToList();

            foreach (RequestViewModel request in SelectedRequest)
                OnIgnoreRequest(request.Friend);
        }

        #endregion

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }        
    }
}
