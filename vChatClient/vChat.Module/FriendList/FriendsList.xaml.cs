using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using vChat.Model.Entities;
using vChat.Model;
using System.ComponentModel;
using Core.Client;
using System.Threading;
using vChat.Module.AddFriend;
using MahApps.Metro.Controls;
using System.Windows.Threading;

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Interaction logic for FriendsList.xaml
    /// </summary>
    public partial class FriendsList : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void MouseEventHandler(object sender, FriendArgs e);
        public event MouseEventHandler OnFriendDoubleClick;

        #region CLASS MEMBER

        private AddFriend.AddFriend addFriendModule;
        private MetroWindow addFriendWin;
        private RemoveGroup.RemoveGroup removeGroupModule;
        private MetroWindow removeGroupWin;
        private ContactViewer.ContactViewer contactViewerModule;
        private MetroWindow contactViewerWin;
        private DispatcherTimer updateRequest;
        private DispatcherTimer updateUnresponseRequest;
        private GroupTreeViewModel groupTree;
        private RequestViewModel requestVM;
        private RequestViewModel unrespRequestVM;
        private GroupFriendList groupFriend;
        private String requestNewGroupName;
        private String moveNewGroupName;
        private int userId;

        #endregion

        #region PROPERTY

        /// <summary>
        /// Cây dữ liệu (Nhóm và bạn bè)
        /// </summary>
        public GroupTreeViewModel GroupTree
        {
            get { return groupTree; }
            set
            {
                if (value != groupTree)
                {
                    groupTree = value;
                    this.OnPropertyChanged("GroupTree");
                }
            }
        }

        /// <summary>
        /// Danh sách yêu cầu kết bạn
        /// </summary>
        public RequestViewModel RequestVM
        {
            get { return requestVM; }
            set
            {
                if (value != requestVM)
                {
                    requestVM = value;
                    this.OnPropertyChanged("RequestVM");
                }
            }
        }

        /// <summary>
        /// Danh sách yêu cầu kết bạn đang đợi phản hồi
        /// </summary>
        public RequestViewModel UnresponseRequesVM
        {
            get { return unrespRequestVM; }
            set
            {
                if (value != unrespRequestVM)
                {
                    unrespRequestVM = value;
                    this.OnPropertyChanged("UnresponseRequesVM");
                }
            }
        }

        /// <summary>
        /// Danh sách nhóm và bạn bè trong nhóm
        /// </summary>
        public GroupFriendList GroupFriend
        {
            get { return groupFriend; }
            set
            {
                if (value != groupFriend)
                {
                    groupFriend = value;
                    this.OnPropertyChanged("GroupFriend");
                }
            }
        }

        /// <summary>
        /// Tên nhóm mới (Dùng cho chấp nhận yêu cầu kết bạn)
        /// </summary>
        public String RequestNewGroupName
        {
            get { return requestNewGroupName; }
            set
            {
                if (value != requestNewGroupName)
                {
                    requestNewGroupName = value;
                    this.OnPropertyChanged("RequestNewGroupName");
                }
            }
        }

        /// <summary>
        /// Tên nhóm mới (Dùng cho di chuyển bạn bè)
        /// </summary>
        public String MoveNewGroupName
        {
            get { return moveNewGroupName; }
            set
            {
                if (value != moveNewGroupName)
                {
                    moveNewGroupName = value;
                    this.OnPropertyChanged("MoveNewGroupName");
                }
            }
        }

        #endregion

        public FriendsList()
        {
            InitializeComponent();
        }        

        #region MAIN METHOD

        /// <summary>
        /// Cài đặt thông tin user
        /// </summary>
        /// <param name="UserID">ID của user</param>
        public void Init(int UserID)
        {
            userId = UserID;

            groupFriend = FriendList(userId); //Lấy danh sách bạn bè trên CSDL
            List<Users> Requests = FriendRequests(userId); //Lấy các yêu cầu kết bạn trên CSDL
            List<Users> UnresponseRequests = UnresponseFriendRequests(userId); //Lấy các yêu cầu kết bạn đang đợi phản hồi trên CSDL

            //CÁC CÀI ĐẶT CHO CÁC MODULE
            changeAvatarModule.ChangeAvatarFor(userId);

            addFriendModule = new AddFriend.AddFriend();
            addFriendModule.SetUser(userId);
            addFriendModule.SetGroups(GroupFriend.FriendGroups);
            addFriendModule.OnAddFriendSuccess += new AddFriend.AddFriend.AddingHandler(AddFriendModule_OnAddFriendSuccess);
            addFriendModule.OnAddFriendError += new AddFriend.AddFriend.AddingHandler(AddFriendModule_OnAddFriendError);

            removeGroupModule = new Module.RemoveGroup.RemoveGroup();
            removeGroupModule.SetUser(userId);
            removeGroupModule.SetGroups(GroupFriend.FriendGroups);
            removeGroupModule.IntegratedWith(this);

            contactViewerModule = new ContactViewer.ContactViewer();

            groupTree = new GroupTreeViewModel(GroupFriend.FriendGroups);
            groupTree.OnMoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnMoveContact);
            groupTree.OnRemoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnRemoveContact);

            requestVM = new RequestViewModel(Requests);
            requestVM.OnAcceptRequest += new RequestViewModel.RequestHandler(RequestVM_OnAcceptRequest);
            requestVM.OnIgnoreRequest += new RequestViewModel.RequestHandler(RequestVM_OnIgnoreRequest);

            unrespRequestVM = new RequestViewModel(UnresponseRequests);            

            //Gửi yêu cầu kiểm tra trạng thái online/offline của bạn bè
            foreach (FriendGroup group in GroupFriend.FriendGroups)
                foreach (Users friend in group.Friends)
                    this.Get<Client>().SendCommand(Core.Data.CommandType.CheckOnline, "SERVER", friend.Username);

            this.DataContext = this;

            //Khởi tạo đối tượng dùng để cập nhập liên tục các yêu cầu kết bạn mới và những yêu cầu kết bạn đang chờ phản hồi đã được chấp nhận
            
            updateRequest = new DispatcherTimer();
            updateRequest.Interval = TimeSpan.FromMilliseconds(1000);
            updateRequest.Tick += new EventHandler(UpdateRequest_Tick);

            updateUnresponseRequest = new DispatcherTimer();
            updateUnresponseRequest.Interval = TimeSpan.FromMilliseconds(1000);
            updateUnresponseRequest.Tick += new EventHandler(UpdateUnresponseRequest_Tick);

            updateRequest.Start();
            updateUnresponseRequest.Start();
        }

        /// <summary>
        /// Thay đổi trạng thái online/offline của bạn bè (Thực thi bởi MainWindowListener)
        /// </summary>
        /// <param name="Username">Tên tài khoản</param>
        /// <param name="IsOnline">Trạng thái</param>
        public void SetFriendStatus(String Username, bool IsOnline)
        {
            GroupTree.SetFriendStatus(Username, IsOnline);
        }

        /// <summary>
        /// Thực hiện thao tác xóa nhóm (Được gọi bởi module RemoveGroup
        /// </summary>
        /// <param name="RemoveContact">Trạng thái xóa bạn bè hay không</param>
        /// <param name="GroupToRemove">Đối tượng chứa thông tin nhóm sẽ xóa</param>
        /// <param name="GroupMoveTo">Đối tượng chứa thông tin nhóm sẽ chuyển bạn bè tới (Mặc định Null)</param>
        public void DoRemoveGroup(bool RemoveContact, FriendGroup GroupToRemove, FriendGroup GroupMoveTo = null)
        {
            //Trường hợp nhóm bị xóa có bạn bè và người dùng muốn xóa bạn bè trong nhóm thì...
            if (GroupToRemove.Friends.Count > 0 && RemoveContact)
            {
                RemoveGroup(GroupToRemove.GroupID, RemoveContact); //Xóa nhóm và tất cả bạn bè trong nhóm
                GroupTree.RemoveGroup(GroupToRemove); //Xóa nhóm trong cây dữ liệu
                treeFriend.UpdateLayout();
                removeGroupWin.Close();

                return;
            }
            else if (GroupToRemove.Friends.Count == 0) //Trường hợp nhóm bị xóa không có bạn bè thì..
            {
                RemoveGroup(GroupToRemove.GroupID, false); //Xóa nhóm (Ko thực hiện thao tác xóa bạn bè)
                GroupTree.RemoveGroup(GroupToRemove); //Xóa nhóm trong cây dữ liệu
                treeFriend.UpdateLayout();
                removeGroupWin.Close();

                return;
            }

            //Trường hợp di chuyển bạn bè tới nhóm mới trước khi xóa
            //Duyệt qua tất cả bạn bè có trong nhóm cần xóa
            foreach (Users child in GroupToRemove.Friends)
            {
                MoveContact(userId, child.UserID, GroupMoveTo.GroupID); //Chuyển bạn bè sang nhóm mới (Cập nhập trên CSDL)
                GroupTree.MoveFriend(child, GroupToRemove, GroupMoveTo); //Chuyển bạn bè sang nhóm mới trong cây dữ liệu
            }

            GroupTree.RemoveGroup(GroupToRemove); //Xóa nhóm được chỉ định

            treeFriend.UpdateLayout();
            removeGroupWin.Close();
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region EVENT

        #region ADD FRIEND ZONE

        private void btnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            Helper.CreateWindow(ref addFriendWin, "Thêm Bạn", addFriendModule).ShowDialog();
        }

        private void AddFriendModule_OnAddFriendSuccess(object sender, AddFriendArgs e)
        {
            addFriendWin.Close();
            MessageBox.Show(String.Format("Đã thêm '{0}' vào nhóm {1} thành công. Vui lòng đợi '{0}' hồi đáp yêu cầu kết bạn của bạn", e.FriendName, e.GroupName));
        }

        private void AddFriendModule_OnAddFriendError(object sender, AddFriendArgs e)
        {
            MessageBox.Show(String.Format("Yêu cầu kết bạn với '{0}' không thành công. Vui lòng thử lại sau", e.FriendName));
        }

        #endregion

        #region REQUEST ZONE

        private void UpdateRequest_Tick(object sender, EventArgs e)
        {            
            int totalRequest = RequestVM.Requests.Count;

            requestTaskZone.Visibility = totalRequest == 0 ? Visibility.Collapsed : Visibility.Visible;

            List<Users> Requests = FriendRequests(userId);

            if (Requests.Count >= totalRequest) //Trường hợp nếu có request mới trên CSDL hoặc số request không thay đổi so với trước đó
                Requests = Requests.Skip(totalRequest).ToList(); //Bỏ qua tổng số request đã lấy trước đó
            else if (Requests.Count < totalRequest) //Ngược lại số request giảm (request đã được user chấp nhận hoặc từ chối
            {
                if (Requests.Count == 0) //Số request đã hết -> xóa tất cả các request đã hiển thị trên giao diện
                    RequestVM.ClearRequest();

                List<int> posToDelete = new List<int>();

                foreach (Users Friend in Requests) //Lấy ra vị trí (index) của những request đã được user chấp nhận
                {
                    for (int i = 0; i < totalRequest; i++)
                    {
                        if (!RequestVM.Requests[i].Friend.Equals(Friend))
                            if (!posToDelete.Contains(i))
                                posToDelete.Add(i);
                    }
                }

                for (int i = 0; i < posToDelete.Count; i++) //Xóa những request đã được user chấp nhận khỏi danh sách request
                {
                    Users MatchUser = RequestVM.Requests[i].Friend;
                    RequestVM.RemoveRequest(MatchUser);
                }
            }

            if (Requests.Count == 0) //Trường hợp không có request sẽ không bổ sung vào danh sách request
                return;

            foreach (Users Friend in Requests) //Thêm request mới vào danh sách
                RequestVM.AppendRequest(Friend);

            friendRequestZone.UpdateLayout();
        }

        private void UpdateUnresponseRequest_Tick(object sender, EventArgs e)
        {
            int totalRequest = UnresponseRequesVM.Requests.Count;

            List<Users> Requests = UnresponseFriendRequests(userId);

            if (Requests.Count >= totalRequest)
                Requests = Requests.Skip(totalRequest).ToList();
            else if(Requests.Count < totalRequest)
            {
                if (Requests.Count == 0)
                    UnresponseRequesVM.ClearRequest();

                List<int> posToDelete = new List<int>();

                foreach (Users Friend in Requests)
                {
                    for (int i = 0; i < totalRequest; i++)
                    {
                        if (!UnresponseRequesVM.Requests[i].Friend.Equals(Friend))
                            if (!posToDelete.Contains(i))
                                posToDelete.Add(i);
                    }
                }

                for (int i = 0; i < posToDelete.Count; i++)
                {
                    Users MatchUser = UnresponseRequesVM.Requests[i].Friend;
                    UnresponseRequesVM.RemoveRequest(MatchUser);
                }
            }

            if (Requests.Count == 0)                
                return;

            foreach (Users Friend in Requests)
                UnresponseRequesVM.AppendRequest(Friend);

            unresponseRequestZone.UpdateLayout();
        }

        private void lbUnresponseRequest_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ItemsControl) //Lấy ra item đã double click vào trước đó
            {
                ItemsControl clickedItem = (ItemsControl)sender;

                if (clickedItem.Items.CurrentItem != null && clickedItem.Items.CurrentItem is RequestViewModel) //Kiểm tra và lấy ra đối tượng RequestViewModel chứa thông tin request
                {
                    Users FriendArg = ((RequestViewModel)clickedItem.Items.CurrentItem).Friend;

                    OnFriendDoubleClick(this, new FriendArgs(
                        FriendArg.UserID,
                        FriendArg.Username,
                        FriendArg.FirstName,
                        FriendArg.LastName
                    ));
                }
            }
        }

        private void lbRequest_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ItemsControl) //Lấy ra item đã double click vào trước đó
            {
                ItemsControl clickedItem = (ItemsControl)sender;

                if (clickedItem.Items.CurrentItem != null && clickedItem.Items.CurrentItem is RequestViewModel) //Kiểm tra và lấy ra đối tượng RequestViewModel chứa thông tin request
                {
                    Users FriendArg = ((RequestViewModel)clickedItem.Items.CurrentItem).Friend;

                    OnFriendDoubleClick(this, new FriendArgs(
                        FriendArg.UserID,
                        FriendArg.Username,
                        FriendArg.FirstName,
                        FriendArg.LastName
                    ));
                }
            }
        }

        private void RequestVM_OnAcceptRequest(Users Friend)
        {
            FriendGroup AvailableGroup = cbRequestGroup.SelectedItem as FriendGroup; //Lấy nhóm có sẵn đã được chọn trên Combobox

            int NewGroupID = 0;

            if (RequestNewGroupName != null)
            {
                if (!AddNewGroup(userId, RequestNewGroupName, ref NewGroupID))
                    MessageBox.Show("Thêm nhóm mới không thành công");
            }
            else if(AvailableGroup == null && RequestNewGroupName == null)
            {
                MessageBox.Show("Không tồn tại nhóm có sẵn cũng như thông tin nhóm mới không hợp lệ nên không thể 'chấp nhận' yêu cầu kết bạn");
                return;
            }

            FriendGroup newGroup = GetGroup(NewGroupID);

            //Chấp nhận yêu cầu kết bạn và thêm bạn bè vào nhóm mới nếu thông tin không bỏ trống, ngược lại sẽ thêm vào nhóm sẵn có
            AcceptRequest(userId, Friend.UserID, NewGroupID != 0 ? newGroup.GroupID : AvailableGroup.GroupID);

            //Bổ sung bạn bè cây dữ liệu
            GroupTree.AppendFriend(Friend, NewGroupID != 0 ? newGroup : AvailableGroup);

            RequestVM.RemoveRequest(Friend); //Xóa yêu cầu sau khi chấp nhận

            friendRequestZone.UpdateLayout();
            treeFriend.UpdateLayout();
        }

        private void RequestVM_OnIgnoreRequest(Users Friend)
        {
            IgnoreRequest(userId, Friend.UserID); //Từ chối yêu cầu kết bạn và cập nhập thông tin lên CSDL
            RequestVM.RemoveRequest(Friend); //Xóa yêu cầu sau khi từ chối

            friendRequestZone.UpdateLayout();
        }

        private void chkRequestTaskDone_Checked(object sender, RoutedEventArgs e)
        {
            btnRequestTaskDone.IsEnabled = true;
        }

        private void chkRequestTaskDone_Unchecked(object sender, RoutedEventArgs e)
        {
            btnRequestTaskDone.IsEnabled = false;
        }

        private void btnRequestTaskDone_Click(object sender, RoutedEventArgs e)
        {
            String Tag = null;

            if (cbRequestTask.SelectedItem != null)
                Tag = ((ComboBoxItem)cbRequestTask.SelectedItem).Tag.ToString();

            switch (Tag)
            {
                case "Accept":
                    requestVM.AcceptCommand.Execute(null);
                    break;
                case "Ignore":
                    requestVM.IgnoreCommand.Execute(null);
                    break;
            }

            chkRequestTaskDone.IsChecked = false;
        }

        #endregion

        #region GROUP TREE ZONE

        private void TreeFriend_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Object SelectedObj = (Object)treeFriend.SelectedItem; //Lấy ra item đang được chọn trên TreeView
            
            //Duyệt từng node root trên TreeView
            foreach (Object parentObj in treeFriend.Items)
            {                
                TreeViewItem Group = treeFriend.ItemContainerGenerator.ContainerFromItem(parentObj) as TreeViewItem; //Lấy ra node cha (chứa thông tin nhóm)
                TreeViewItem MatchItem = treeFriend.ItemContainerGenerator.ContainerFromItem(SelectedObj) as TreeViewItem; //Lấy ra node chứa thông tin của SelectedObject

                if (MatchItem != null) //Nếu node cha chứa thông tin trùng khớp với SelectedObj thì...
                {
                    MatchItem.ContextMenu = treeFriend.Resources["GroupContext"] as ContextMenu; //Gán menu ngữ cảnh cho node cha đó
                    break;
                }
                
                //Trường hợp node con chứa thông tin của SelectedObject
                TreeViewItem Friend = Group.ItemContainerGenerator.ContainerFromItem(SelectedObj) as TreeViewItem; //Lấy ra node con chứa thông tin của SelectedObject

                if (Friend != null) //Nếu thông tin hợp lệ
                    Friend.ContextMenu = treeFriend.Resources["FriendContext"] as ContextMenu; //Gán menu ngữ cảnh cho node con đó
            }
        }

        private void TreeFriend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem doubleClickItem = e.Source as TreeViewItem; //Lấy ra đối tượng đã double click trên TreeFriend nếu đối tượng đó là một TreeViewItem

            if (doubleClickItem == null)
                return;

            object Item = doubleClickItem.Header; //Lấy đối tượng thực chứa bên trong TreeViewItem

            if (Item is FriendViewModel)
            {
                FriendViewModel SelectedFriend = treeFriend.SelectedItem as FriendViewModel;
                Users FriendArg = SelectedFriend.Friend; //Lấy thông tin của bạn bè mà user đã double click vào

                OnFriendDoubleClick(this, new FriendArgs(
                        FriendArg.UserID,
                        FriendArg.Username,
                        FriendArg.FirstName,
                        FriendArg.LastName
                    ));
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                GroupTree.SearchCommand.Execute(null);
        }

        private void mnuAddFriend_Click(object sender, RoutedEventArgs e)
        {
            GroupViewModel SelectedGroup = treeFriend.SelectedItem as GroupViewModel;

            addFriendModule.SetDefaultGroup(SelectedGroup.Group);

            Helper.CreateWindow(ref addFriendWin, "Thêm Bạn", addFriendModule).ShowDialog();
        }

        private void mnuRemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupViewModel SelectedGroup = treeFriend.SelectedItem as GroupViewModel;

            removeGroupModule.SetGroupToRemove(SelectedGroup.Group);

            Helper.CreateWindow(ref removeGroupWin, "Xóa Nhóm", removeGroupModule).ShowDialog();
        }

        private void mnuFriendDetail_Click(object sender, RoutedEventArgs e)
        {
            FriendViewModel SelectedFriend = treeFriend.SelectedItem as FriendViewModel;
            contactViewerModule.ViewFor(SelectedFriend.Friend);

            Helper.CreateWindow(ref contactViewerWin, "Thông Tin Của " + SelectedFriend.FriendName, contactViewerModule).ShowDialog();
        }

        private void mnuFriendRemove_Click(object sender, RoutedEventArgs e)
        {
            FriendViewModel SelectedFriend = treeFriend.SelectedItem as FriendViewModel;
            SelectedFriend.IsChecked = true; //Đánh dấu chọn bạn bè để dùng cho thao tác xóa

            GroupTree.RemoveCommand.Execute(null);
        }

        private void GroupTree_OnMoveContact(Users Friend, FriendGroup OldGroup)
        {
            FriendGroup NewGroup = cbNewGroup.SelectedItem as FriendGroup; //Lấy ra nhóm mới sẽ chuyển bạn bè tới

            int NewGroupID = 0;

            if (NewGroup == null || MoveNewGroupName != null) //Kiểm tra trường hợp nhóm chuyển tới (nhóm có sẵn) không có hoặc nhóm chuyển tới (nhóm mới) chưa được khai báo
                if (MoveNewGroupName != null) //Nhóm mới đã khai báo thì thực hiện tạo nhóm mới trên CSDL
                    if (!AddNewGroup(userId, MoveNewGroupName, ref NewGroupID))
                        MessageBox.Show("Thêm nhóm mới không thành công");

            //Thực hiện di chuyển bạn bè từ nhóm cũ tới nhóm mới. Nếu nhóm mới đã được khai báo thì di chuyển tới nhóm mới, ngược lại nếu sẽ chuyển tới nhóm có sẵn (đã chọn trước đó)
            MoveContact(userId, Friend.UserID, NewGroupID != 0 ? GetGroup(NewGroupID).GroupID : NewGroup.GroupID);

            //Di chuyển bạn bè sang nhóm mới trên cây dữ liệu. Thực hiện tương tự công việc của MoveContact
            GroupTree.MoveFriend(Friend, OldGroup, NewGroupID != 0 ? GetGroup(NewGroupID) : NewGroup);

            treeFriend.UpdateLayout();
        }

        private void GroupTree_OnRemoveContact(Users Friend, FriendGroup OldGroup)
        {
            RemoveContact(userId, Friend.UserID);
            GroupTree.RemoveFriend(Friend, OldGroup);

            treeFriend.UpdateLayout();
        }

        #endregion

        #region EDIT TASK ZONE

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {            
            btnSelectAll.Visibility = System.Windows.Visibility.Visible;
            btnDeselectAll.Visibility = System.Windows.Visibility.Visible;
            editTaskZone.Visibility = System.Windows.Visibility.Visible;
        }

        private void chkDone_Checked(object sender, RoutedEventArgs e)
        {
            btnDone.IsEnabled = true;
        }

        private void chkDone_Unchecked(object sender, RoutedEventArgs e)
        {
            btnDone.IsEnabled = false;
        }

        private void cbTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String Tag = null;

            if (cbTask.SelectedItem != null)
                Tag = ((ComboBoxItem)cbTask.SelectedItem).Tag.ToString();

            switch (Tag)
            {
                case "MoveContact":
                    tblNewGroup.Visibility = System.Windows.Visibility.Visible;
                    cbNewGroup.Visibility = System.Windows.Visibility.Visible;
                    tblAddNewGroup.Visibility = System.Windows.Visibility.Visible;
                    tbAddNewGroup.Visibility = System.Windows.Visibility.Visible;
                    break;

                case "RemoveContact":
                    tblNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    cbNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    tblAddNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    tbAddNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            String Tag = null;

            if (cbTask.SelectedItem != null)
                Tag = ((ComboBoxItem)cbTask.SelectedItem).Tag.ToString();

            switch (Tag)
            {
                case "MoveContact":
                    GroupTree.MoveCommand.Execute(null);
                    break;

                case "RemoveContact":
                    GroupTree.RemoveCommand.Execute(null);
                    break;
            }

            chkDone.IsChecked = false;
            btnDone.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GroupTree.CancelEditCommand.Execute(null);
            btnSelectAll.Visibility = System.Windows.Visibility.Collapsed;
            btnDeselectAll.Visibility = System.Windows.Visibility.Collapsed;
            editTaskZone.Visibility = System.Windows.Visibility.Collapsed;            
        }

        #endregion                      

        #endregion
    }    
}
