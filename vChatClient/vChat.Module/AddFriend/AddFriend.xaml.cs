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
using System.ComponentModel;
using System.Collections.ObjectModel;
using vChat.Model;

namespace vChat.Module.AddFriend
{
    /// <summary>
    /// Interaction logic for AddFriend.xaml
    /// </summary>
    public partial class AddFriend : UserControl, INotifyPropertyChanged
    {
        public delegate void AddingHandler(object sender, AddFriendArgs e);
        public event AddingHandler OnAddFriendSuccess;
        public event AddingHandler OnAddFriendError;

        public event PropertyChangedEventHandler PropertyChanged;

        private FriendList.FriendsList _IntegratedModule;
        private int _UserID;
        private ObservableCollection<FriendGroup> _Groups;
        private String _FriendName;
        private String _NewGroupName;

        public ObservableCollection<FriendGroup> Groups
        {
            get { return _Groups; }
        }

        public String FriendName
        {
            get { return _FriendName; }
            set
            {
                if (value != _FriendName)
                {
                    _FriendName = value;
                    this.OnPropertyChanged("FriendName");
                }
            }
        }

        public String NewGroupName
        {
            get { return _NewGroupName; }
            set
            {
                if (value != _NewGroupName)
                {
                    _NewGroupName = value;
                    this.OnPropertyChanged("NewGroupName");
                }
            }
        }
        
        public AddFriend()
        {
            InitializeComponent();
        }

        public void SetUser(int UserID)
        {
            _UserID = UserID;            
        }

        public void SetGroups(ObservableCollection<FriendGroup> Groups)
        {
            _Groups = Groups;

            DataContext = this;
        }

        public void SetDefaultGroup(FriendGroup SelectGroup)
        {
            if (SelectGroup == null)
                return;

            int pos = cbGroup.Items.IndexOf(SelectGroup);
            cbGroup.SelectedIndex = pos;
        }

        public void IntegratedWith(FriendList.FriendsList IntegratedModule)
        {
            _IntegratedModule = IntegratedModule;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbGroup.SelectedItem == null && NewGroupName == null)
            {
                MessageBox.Show("Hãy nhập tên nhóm bạn cần thêm bạn bè vào");
                OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName));
                return;
            }

            FriendGroup SelectedGroup = (FriendGroup)cbGroup.SelectedItem;

            int NewGroupID = 0;

            if (NewGroupName != null)
                if (!AddNewGroup(_UserID, NewGroupName, ref NewGroupID))
                    OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName));

            if (AddNewFriend(_UserID, FriendName, NewGroupID != 0 ? NewGroupID : SelectedGroup.GroupID))              
                OnAddFriendSuccess(this, new AddFriendArgs(FriendName, NewGroupName != null ? NewGroupName : SelectedGroup.Name));            
            else
                OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName != null ? NewGroupName : SelectedGroup.Name));
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
