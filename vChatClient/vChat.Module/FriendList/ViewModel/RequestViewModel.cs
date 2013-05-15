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
    /// <summary>
    /// Thông tin dành cho Binding các yêu cầu kết bạn trên ItemsControl
    /// </summary>
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
        private bool isChecked;
        private bool isIgnored = true; //Default
        private bool isAccepted;

        #endregion

        #region PROPERTY

        /// <summary>
        /// Command dùng cho thao tác chấp nhận yêu cầu kết bạn
        /// </summary>
        public ICommand AcceptCommand
        {
            get { return acceptCommand; }
        }

        /// <summary>
        /// Command dùng cho thao tác từ chối yêu cầu kết bạn
        /// </summary>
        public ICommand IgnoreCommand
        {
            get { return ignoreCommand; }
        }

        /// <summary>
        /// Thông tin người yêu cầu
        /// </summary>
        public Users Friend
        {
            get { return friend; }
        }

        /// <summary>
        /// Tên người yều
        /// </summary>
        public String FriendName
        {
            get { return (String.Format("{0} {1}", friend.FirstName, friend.LastName)); }
        }

        /// <summary>
        /// Trạng thái được đánh dấu chọn hay không
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;
                    this.OnPropertyChanged("IsChecked");
                }
            }
        }

        /// <summary>
        /// Trạng thái từ chối hay không
        /// </summary>
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

        /// <summary>
        /// Trạng thái chấp nhận hay không
        /// </summary>
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

        /// <summary>
        /// Danh sách các yêu cầu
        /// </summary>
        public ObservableCollection<RequestViewModel> Requests
        {
            get { return requests; }            
        }

        #endregion

        /// <summary>
        /// Khởi tạo yêu cầu mới với thông tin chỉ định
        /// </summary>
        /// <param name="Friend">Đối tượng chứa thông tin bạn bè</param>
        private RequestViewModel(Users Friend)
        {
            friend = Friend;
        }

        /// <summary>
        /// Khởi tạo danh sách các yêu cầu
        /// </summary>
        /// <param name="Friends">Đối tượng chứa danh sách các yêu cầu</param>
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

        /// <summary>
        /// Thêm yêu cầu
        /// </summary>
        /// <param name="Friend"></param>
        public void AppendRequest(Users Friend)
        {
            if (Friend == null)
                return;

            requests.Add(new RequestViewModel(Friend));
        }

        /// <summary>
        /// Xóa yêu cầu
        /// </summary>
        /// <param name="Friend">Đối tượng chứa yêu cầu</param>
        public void RemoveRequest(Users Friend)
        {
            if (Friend == null)
                return;

            RequestViewModel MatchRequest = requests.FirstOrDefault(r => r.Friend.Equals(Friend)); //Tìm đối tượng RequestViewModel chứa yêu cầu được chỉ định

            if (MatchRequest != null)
                requests.Remove(MatchRequest);
        }

        /// <summary>
        /// Xóa tất cả yêu cầu
        /// </summary>
        public void ClearRequest()
        {
            requests.Clear();
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
            List<RequestViewModel> SelectedRequest = requests.Where(r => r.IsChecked).ToList(); //Lấy ra danh sách những yêu cầu đã được đánh dấu chọn

            //Thực hiện chấp nhận lần lượt các yêu cầu
            foreach (RequestViewModel request in SelectedRequest)
                OnAcceptRequest(request.Friend);
        }

        private void IgnoreFriend()
        {
            List<RequestViewModel> SelectedRequest = requests.Where(r => r.IsChecked).ToList(); //Lấy ra danh sách những yêu cầu đã được đánh dấu chọn

            //Thực hiện từ chối lần lượt các yêu cầu
            foreach (RequestViewModel request in SelectedRequest)
                OnIgnoreRequest(request.Friend);
        }

        #endregion
                
    }
}
