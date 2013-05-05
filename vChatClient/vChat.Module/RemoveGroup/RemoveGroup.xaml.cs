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
using System.Collections.ObjectModel;
using vChat.Model.Entities;
using System.ComponentModel;

namespace vChat.Module.RemoveGroup
{
    /// <summary>
    /// Interaction logic for RemoveGroup.xaml
    /// </summary>
    public partial class RemoveGroup : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private FriendList.FriendsList _IntegratedModule;
        private ObservableCollection<FriendGroup> _Groups;
        private FriendGroup _GroupToRemove;
        private int _UserID;
        private String _NewGroupName;
        private bool _IsRemoveContact;
        private bool _IsMoveContact = true; //Default

        public ObservableCollection<FriendGroup> Groups
        {
            get { return _Groups; }
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

        public bool IsRemoveContact
        {
            get { return _IsRemoveContact; }
            set
            {
                if (value != _IsRemoveContact)
                {
                    _IsRemoveContact = value;
                    this.OnPropertyChanged("IsRemoveContact");
                }
            }
        }

        public bool IsMoveContact
        {
            get { return _IsMoveContact; }
            set
            {
                if (value != _IsMoveContact)
                {
                    _IsMoveContact = value;
                    this.OnPropertyChanged("IsMoveContact");
                }
            }
        }

        public RemoveGroup()
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

        public void SetGroupToRemove(FriendGroup GroupToRemove)
        {
            _GroupToRemove = GroupToRemove;            
        }

        public void IntegratedWith(FriendList.FriendsList IntegratedModule)
        {
            _IntegratedModule = IntegratedModule;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            FriendGroup GroupMoveTo = cbGroupMoveTo.SelectedItem as FriendGroup;

            if (_IsRemoveContact && (GroupMoveTo != null && GroupMoveTo.Equals(_GroupToRemove)))
            {
                MessageBox.Show("Nhóm bị xóa trùng với nhóm chuyển liên lạc đến. Vui lòng chọn nhóm khác");
                return;
            }

            int NewGroupID = 0;

            if (NewGroupName != null)
                if (!AddNewGroup(_UserID, NewGroupName, ref NewGroupID))
                    MessageBox.Show("Thêm nhóm mới không thành công");

            if (_IsMoveContact)
                _IntegratedModule.DoRemoveGroup(RemoveContact: false, GroupToRemove: _GroupToRemove, GroupMoveTo: NewGroupID != 0 ? GetGroup(NewGroupID) : GroupMoveTo);
            else if (_IsRemoveContact)
                _IntegratedModule.DoRemoveGroup(RemoveContact: true, GroupToRemove: _GroupToRemove);
        }
    }
}
