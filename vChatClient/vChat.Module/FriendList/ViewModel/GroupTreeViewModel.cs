using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using vChat.Model.Entities;
using System.Windows;
using System.Windows.Media;

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Cây phân cấp dữ liệu (Nhóm và bạn bè) dùng cho Binding trên TreeView
    /// </summary>
    public partial class GroupTreeViewModel
    {
        public delegate void FriendHandler(Users Friend, FriendGroup OldGroup);
        public event FriendHandler OnMoveContact;
        public event FriendHandler OnRemoveContact;

        #region CLASS MEMBER

        private readonly ObservableCollection<GroupViewModel> groups;       
        private readonly ICommand searchCommand;
        private String searchText = String.Empty;
        private List<FriendViewModel> matchFriends;
        private Brush matchColor = Brushes.Red;

        private readonly ICommand editCommand;
        private readonly ICommand cancelEditCommand;
        private readonly ICommand selectCommand;
        private readonly ICommand deselectCommand;
        private readonly ICommand moveCommand;
        private readonly ICommand removeCommand;

        #endregion

        #region PROPERTY

        /// <summary>
        /// Danh sách nhom
        /// </summary>
        public ObservableCollection<GroupViewModel> Groups
        {
            get { return groups; }
        }

        /// <summary>
        /// Command dùng cho tìm kiếm bạn bè
        /// </summary>
        public ICommand SearchCommand
        {
            get { return searchCommand; }
        }

        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public String SearchText
        {
            get { return searchText; }
            set
            {
                if (value == searchText)
                    return;

                searchText = value;
                
                //Reset lại kết quá tìm kiếm
                if(matchFriends != null)
                    foreach (FriendViewModel Friend in matchFriends)
                        Friend.MatchColor = System.Windows.Media.Brushes.Black;

                matchFriends = null;
            }
        }

        /// <summary>
        /// Command dùng cho chỉnh sửa bạn bè
        /// </summary>
        public ICommand EditCommand
        {
            get { return editCommand; }
        }

        /// <summary>
        /// Command dùng cho hủy thao tác chỉnh sửa bạn bè
        /// </summary>
        public ICommand CancelEditCommand
        {
            get { return cancelEditCommand; }
        }

        /// <summary>
        /// Command dùng cho thao tác chọn tất cả bạn bè
        /// </summary>
        public ICommand SelectCommand
        {
            get { return selectCommand; }
        }

        /// <summary>
        /// Command dùng cho thao tác bỏ chọn tất cả bạn bè
        /// </summary>
        public ICommand DeselectCommand
        {
            get { return deselectCommand; }
        }

        /// <summary>
        /// Command dùng cho thao tác di chuyển bạn bè
        /// </summary>
        public ICommand MoveCommand
        {
            get { return moveCommand; }
        }

        /// <summary>
        /// Command dùng cho thao tác xóa bạn bè
        /// </summary>
        public ICommand RemoveCommand
        {
            get { return removeCommand; }
        }

        #endregion

        /// <summary>
        /// Khởi thông tin cho cây dữ liệu dùng cho Binding
        /// </summary>
        /// <param name="Groups">Đối tượng chứa danh sách nhóm</param>
        public GroupTreeViewModel(ObservableCollection<FriendGroup> Groups)
        {
            groups = new ObservableCollection<GroupViewModel>();

            foreach (FriendGroup child in Groups)
                groups.Add(new GroupViewModel(child));

            searchCommand = new SearchTask(this);
            editCommand = new EditTask(this);
            selectCommand = new SelectTask(this);
            deselectCommand = new DeselectTask(this);
            moveCommand = new MoveTask(this);
            removeCommand = new RemoveTask(this);
            cancelEditCommand = new CancelEditTask(this);
        }

        #region MAIN METHOD

        /// <summary>
        /// Thêm nhóm
        /// </summary>
        /// <param name="Group">Đối tượng chứa thông tin nhóm</param>
        public void AppendGroup(FriendGroup Group)
        {
            if (Group != null)
            {
                //Kiểm tra trường hợp nhóm đã tồn tại trong cây dữ liệu hay chưa
                GroupViewModel AvailableGroup = groups.FirstOrDefault(g => g.Group.Equals(Group));

                if(AvailableGroup == null) //Nếu chưa tồn tại thì mới bổ sung mới
                    groups.Add(new GroupViewModel(Group));
            }
        }

        /// <summary>
        /// Xóa nhóm
        /// </summary>
        /// <param name="Group">Đối tượng chứa thông tin nhóm</param>
        public void RemoveGroup(FriendGroup Group)
        {
            if (Group == null)
                return;

            GroupViewModel MatchGroup = groups.FirstOrDefault(g => g.Group.Equals(Group));

            if(MatchGroup != null)
                groups.Remove(MatchGroup);
        }

        /// <summary>
        /// Thêm bạn bè vào một nhóm chỉ định. Nếu nhóm không tồn tại thực hiện bổ sung nhóm mới
        /// </summary>
        /// <param name="Friend">Đối tượng chứa thông tin bạn bè</param>
        /// <param name="Group">Đối tượng chứa thông tin nhóm</param>
        public void AppendFriend(Users Friend, FriendGroup Group)
        {
            if (Friend == null || Group == null)
                return;

            GroupViewModel ParentGroup = groups.FirstOrDefault(g => g.Group.Equals(Group)); //Tìm đối tượng GroupViewModel chứa thông tin nhóm được chỉ định

            if (ParentGroup == null) //Nếu không tìm thấy thì thực hiện bổ sung nhóm mới
            {
                AppendGroup(Group);
                ParentGroup = groups.FirstOrDefault(g => g.Group.Equals(Group));
            }

            FriendViewModel AvailableFriend = null;

            //Duyệt tất cả các nhóm và kiểm xem đã tồn tại thông tin bạn bè muốn bổ sung hay không
            foreach (GroupViewModel group in groups)
            {
                foreach (FriendViewModel friend in group.Children)
                {
                    AvailableFriend = friend;

                    if (friend.Equals(Friend))
                        break;
                }
            }
            
            ParentGroup.Children.Add(new FriendViewModel(Friend, ParentGroup));
        }

        /// <summary>
        /// Xóa bạn bè khỏi nhóm chỉ định
        /// </summary>
        /// <param name="Friend"></param>
        /// <param name="Group"></param>
        public void RemoveFriend(Users Friend, FriendGroup Group)
        {
            if (Friend == null || Group == null)
                return;

            GroupViewModel ParentGroup = groups.FirstOrDefault(g => g.Group.Equals(Group)); //Tìm đối tượng GroupViewModel chứa thông tin nhóm được chỉ định
            FriendViewModel MatchFriend = ParentGroup.Children.FirstOrDefault(f => f.Friend.Equals(Friend)); //Tìm đối tượng FriendViewModel chứa thông tin bạn bè được chỉ định

            if (ParentGroup != null && MatchFriend != null) //Sau khi tìm thấy thực hiện xóa nhóm
                ParentGroup.Children.Remove(MatchFriend);
        }

        /// <summary>
        /// Di chuyển bạn bè từ nhóm cũ sang nhóm mới
        /// </summary>
        /// <param name="Friend">Đối tượng chứa thông tin bạn bè</param>
        /// <param name="OldGroup">Đối tượng chứa thông tin nhóm cũ</param>
        /// <param name="NewGroup">Đối tượng chứa thông tin nhóm mới</param>
        public void MoveFriend(Users Friend, FriendGroup OldGroup, FriendGroup NewGroup)
        {
            if (Friend == null || OldGroup == null || NewGroup == null)
                return;

            GroupViewModel NewParentGroup = groups.FirstOrDefault(g => g.Group.Equals(NewGroup)); //Tìm đối tượng GroupViewModel chứa thông tin nhóm mới được chỉ định
            GroupViewModel ParentGroup = groups.FirstOrDefault(g => g.Group.Equals(OldGroup)); //Tìm đối tượng GroupViewModel chứa thông tin nhóm cũ được chỉ định
            FriendViewModel MatchFriend = ParentGroup.Children.FirstOrDefault(f => f.Friend.Equals(Friend)); //Tìm đối tượng FriendViewModel chứa thông tin bạn bè được chỉ định

            FriendViewModel MatchFriendStateFull = new FriendViewModel(Friend, NewParentGroup); //Khởi tạo đối tượng FriendViewModel để chứa thông tin bạn bè và nhóm mới
            MatchFriendStateFull.ToogleCheckbox = MatchFriend.ToogleCheckbox; //Gán lại trạng thái đánh dấu chọn trước đó
            
            ParentGroup.Children.Remove(MatchFriend); //Xóa bạn bè trong nhóm cũ
            NewParentGroup.Children.Add(MatchFriendStateFull); //Bổ sung bạn bè vào nhóm mới
        }

        /// <summary>
        /// Cài đặt trạng thái online/offlien của bạn bè
        /// </summary>
        /// <param name="Username">Tên tài khoản</param>
        /// <param name="IsOnline">Trạng thái</param>
        public void SetFriendStatus(String Username, bool IsOnline)
        {
            //Duyệt tất các nhóm trong cây dữ liệu
            foreach (GroupViewModel Parent in groups)
            {
                //Tìm đối tượng FriendViewModel chứa thông tin bạn bè dựa trên Username
                FriendViewModel MatchUser = Parent.Children.FirstOrDefault(f => f.Friend.Username.Equals(Username));

                if (MatchUser != null)
                {
                    MatchUser.IsOnline = IsOnline;
                    break;
                }
            }
        }

        #endregion

        #region COMMAND PREFORM

        private void PerformSearch()
        {
            if (String.IsNullOrWhiteSpace(searchText))
                return;

            matchFriends = new List<FriendViewModel>();

            //Duyệt tất cả các nhóm và lấy ra danh sách bạn bè khớp với từ khóa tìm kiếm
            foreach (GroupViewModel Parent in groups)
            {
                matchFriends.AddRange(Parent.Children
                                      .Where(friend => friend.NameContainsText(searchText))
                                      .ToList());
            }

            //Thay đổi trạng thái của nhóm sang trạng thái mở (Expanded) và đánh dấu màu cho những bạn bè khớp với từ khóa tìm kiém
            foreach (FriendViewModel Friend in matchFriends)
            {
                Friend.Parent.IsExpanded = true;
                Friend.MatchColor = matchColor;
            }
        }        

        private void PerformEdit()
        {
            //Duyệt tất cả các nhóm, bạn bè trong nhóm và mở ẩn các checkbox
            foreach(GroupViewModel Parent in groups)
            {
                Parent.ToogleCheckbox = Visibility.Visible;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.ToogleCheckbox = Visibility.Visible;
            }
        }

        private void PerformCancelEdit()
        {
            //Duyệt tất cả các nhóm, bạn bè trong nhóm và ẩn các checkbox đã hiện trước đó
            foreach (GroupViewModel Parent in groups)
            {
                Parent.ToogleCheckbox = Visibility.Collapsed;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.ToogleCheckbox = Visibility.Collapsed;
            }
        }

        private void PerformSelect()
        {
            //Duyệt tất cả các nhóm, bạn bè trong nhóm và thay đổi trạng thái đánh dấu chọn của các Checkbox sang trạng thái "được chọn"
            foreach (GroupViewModel Parent in groups)
            {
                Parent.IsChecked = true;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.IsChecked = true;
            }
        }

        private void PerformDeselect()
        {
            //Duyệt tất cả các nhóm, bạn bè trong nhóm và thay đổi trạng thái đánh dấu chọn của các Checkbox sang trạng thái "không được chọn"
            foreach (GroupViewModel Parent in groups)
            {
                Parent.IsChecked = false;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.IsChecked = false;
            }
        }

        private void PerformMove()
        {
            List<FriendViewModel> MatchFriends = new List<FriendViewModel>();

            //Duyệt tất cả các nhóm và lấy ra danh sách bạn bè được đánh dấu chọn
            foreach (GroupViewModel Parent in groups)
            {
                MatchFriends.AddRange(Parent.Children
                                      .Where(friend => friend.IsChecked == true)
                                      .ToList());
            }

            //Di chuyển từng bạn bè trong danh sách tìm được sang nhóm mới
            foreach (FriendViewModel child in MatchFriends)
                OnMoveContact(child.Friend, child.Parent.Group);
        }

        private void PerformRemove()
        {
            List<FriendViewModel> MatchFriends = new List<FriendViewModel>();

            //Duyệt tất cả các nhóm và lấy ra danh sách bạn bè được đánh dấu chọn
            foreach (GroupViewModel Parent in groups)
            {
                MatchFriends.AddRange(Parent.Children
                                      .Where(friend => friend.IsChecked == true)
                                      .ToList());
            }

            //Xóa từng bạn bè (trong một nhóm chỉ định) trong danh sách tìm được
            foreach (FriendViewModel child in MatchFriends)
                OnRemoveContact(child.Friend, child.Parent.Group);
        }

        #endregion
    }
}
