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
        private bool _IsRemoveContact;
        private bool _IsMoveContact = true; //Default

        public ObservableCollection<FriendGroup> Groups
        {
            get { return _Groups; }
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

        public void SetupData(ObservableCollection<FriendGroup> Groups)
        {
            _Groups = Groups;

            DataContext = this;
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

            if (_IsMoveContact)
                _IntegratedModule.DoRemoveGroup(RemoveContact : false, GroupMoveTo : GroupMoveTo);
            else if(_IsRemoveContact)
                _IntegratedModule.DoRemoveGroup(RemoveContact : true);
        }
    }
}
