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

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Interaction logic for FriendsList.xaml
    /// </summary>
    public partial class FriendsList : UserControl
    {
        public delegate void SearchHandler(bool Status);
        public event SearchHandler OnSearchSuccess;
        public event SearchHandler OnSearchFail;
        private ContextMenu _contextMenu;

        public FriendsList()
        {
            InitializeComponent();
        }

        private void TreeFriend_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FriendGroup fg = TreeFriend.SelectedItem as FriendGroup;

            if (fg == null)
                return;

            foreach (FriendGroup itemobj in TreeFriend.Items)
            {                                
                if(fg.GroupID == itemobj.GroupID)
                {
                    ItemContainerGenerator groupContainer = TreeFriend.ItemContainerGenerator;
                    TreeViewItem groupControl = groupContainer.ContainerFromItem(itemobj) as TreeViewItem;

                    if (groupControl.ContextMenu == null)
                    {
                        ContextMenu groupCtxMenu = new ContextMenu();
                        groupCtxMenu.Items.Add("Remove group: " + fg.Name);                        

                        groupControl.ContextMenu = groupCtxMenu;
                    }
                }
            }
        }

        public void SetupData(int UserID)
        {
            GroupFriendList Friends = FriendList(UserID);
            TreeFriend.ItemsSource = Friends.FriendGroups;
        }

        private void GroupItem_Click(object source, MouseButtonEventArgs e)
        {
            if (source is GroupItem)
            {
                GroupItem ItemClicked = (GroupItem)source;

                GroupInfo g = new GroupInfo
                {
                    ID = ItemClicked.GroupID,
                    Name = ItemClicked.GroupName
                };

                _contextMenu = new ContextMenu();
                _contextMenu.Items.Add("Phuc Tho Tran");
                _contextMenu.Items.Add("Test menu");

                foreach (Object itemobj in TreeFriend.Items)
                {
                    TreeViewItem groupControl = null;
                    if(itemobj.Equals(source))
                    {
                        ItemContainerGenerator groupContainer = TreeFriend.ItemContainerGenerator;
                        groupControl = groupContainer.ContainerFromItem(itemobj) as TreeViewItem;
                    }
                                        
                    //groupControl.ContextMenu = _contextMenu;
                }

                //OnGroupItemClick(g);
            }
        }

        private void FriendItem_Click(object source, MouseButtonEventArgs e)
        {
            if (source is FriendItem)
            {
                FriendItem ItemClicked = (FriendItem)source;

                FriendInfo f = new FriendInfo
                {
                    ID = ItemClicked.UserID,
                    Name = ItemClicked.Username
                };

                //OnFriendItemClick(f);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //List<FriendGroup> fg = (List<FriendGroup>)TreeFriend.ItemsSource;

            //for (int i = 0; i < fg.Count; i++)
            //{
            //    FriendGroup group = fg.ElementAt(i);

            //    for (int j = 0; j < group.Friends.Count; j++)
            //    {
            //        Users friend = group.Friends.ElementAt(j);

            //        if (!txtSearch.Text.ToLower().Contains(friend.Username))
            //        {
            //            group.Friends.RemoveAt(j);

            //            if (group.Friends.Count == 0)
            //                fg.RemoveAt(i);
            //        }
            //    }
            //}                       

            string SearchText = txtSearch.Text.ToLower();
            TreeFriend.UpdateLayout();
            
            foreach (FriendGroup GroupItemObj in TreeFriend.Items)
            {
                TreeViewItem GroupItem = TreeFriend.ItemContainerGenerator.ContainerFromItem(GroupItemObj) as TreeViewItem;

                ItemContainerGenerator GroupItemContainer = TreeFriend.ItemContainerGenerator;
                ItemsControl GroupControl = GroupItemContainer.ContainerFromItem(GroupItemObj) as ItemsControl;

                TreeFriend.SelectItem(GroupItemObj.Friends[0]);

                ItemContainerGenerator ChildItemContainer = GroupControl.ItemContainerGenerator;
                ItemsControl ChildControl = ChildItemContainer.ContainerFromItem(GroupItemObj.Friends[0]) as ItemsControl;

                if (GroupItem != null)
                {
                    GroupItem.IsExpanded = true;

                    List<TreeViewItem> lstChild = GetChild(GroupItem);

                    GroupItem.IsExpanded = false;
                }
            }
            
            //TreeFriend.ItemsSource = fg;
            //TreeFriend.Items.Refresh();

            //foreach (object item in TreeFriend.Items)
            //{
            //    TreeViewItem treeItem = TreeFriend.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

            //    if (treeItem != null)
            //    {
            //        ExpandAll(treeItem, true);
            //        treeItem.IsExpanded = true;
            //    }
            //}
        }

        private List<TreeViewItem> GetChild(ItemsControl GroupItem)
        {
            List<TreeViewItem> lstTree = new List<TreeViewItem>();

            foreach (object ChildObj in GroupItem.Items)
            {
                ItemsControl Child = GroupItem.ItemContainerGenerator.ContainerFromItem(ChildObj) as TreeViewItem;

                if (Child != null)
                    GetChild(Child);

                TreeViewItem item = Child as TreeViewItem;

                if (item != null)
                    lstTree.Add(item);
            }

            return lstTree;
        }

        private void HideAll(ItemsControl GroupItem)
        {
            foreach (object ChildItemObj in GroupItem.Items)
            {
                ItemsControl ChildItem = GroupItem.ItemContainerGenerator.ContainerFromItem(ChildItemObj) as ItemsControl;

                if (ChildItem != null)
                    HideAll(ChildItem);

                TreeViewItem item = ChildItem as TreeViewItem;

                if (item != null)
                    item.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ExpandAll(ItemsControl items, bool expand)
        {
            foreach (object obj in items.Items)
            {
                ItemsControl childControl = items.ItemContainerGenerator.ContainerFromItem(obj) as ItemsControl;

                if (childControl != null)
                    ExpandAll(childControl, expand);

                TreeViewItem item = childControl as TreeViewItem;
                if (item != null)
                    item.IsExpanded = true;
            }
        }
    }

    public static class Extension
    {
        public static void SelectItem(this TreeView treeView, object item)
        {
            ExpandAndSelectItem(treeView, item);
        }

        private static bool ExpandAndSelectItem(ItemsControl parentContainer, object itemToSelect)
        {
            //check all items at the current level
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                //if the data item matches the item we want to select, set the corresponding
                //TreeViewItem IsSelected to true
                if (item == itemToSelect && currentContainer != null)
                {
                    currentContainer.IsSelected = true;
                    currentContainer.BringIntoView();
                    currentContainer.Focus();

                    //the item was found
                    return true;
                }
            }

            //if we get to this point, the selected item was not found at the current level, so we must check the children
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                //if children exist
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    //keep track of if the TreeViewItem was expanded or not
                    bool wasExpanded = currentContainer.IsExpanded;

                    //expand the current TreeViewItem so we can check its child TreeViewItems
                    currentContainer.IsExpanded = true;

                    //if the TreeViewItem child containers have not been generated, we must listen to
                    //the StatusChanged event until they are
                    if (currentContainer.ItemContainerGenerator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                    {
                        //store the event handler in a variable so we can remove it (in the handler itself)
                        EventHandler eh = null;
                        eh = new EventHandler(delegate
                        {
                            if (currentContainer.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                            {
                                if (ExpandAndSelectItem(currentContainer, itemToSelect) == false)
                                {
                                    //The assumption is that code executing in this EventHandler is the result of the parent not
                                    //being expanded since the containers were not generated.
                                    //since the itemToSelect was not found in the children, collapse the parent since it was previously collapsed
                                    currentContainer.IsExpanded = false;
                                }

                                //remove the StatusChanged event handler since we just handled it (we only needed it once)
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        });
                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else //otherwise the containers have been generated, so look for item to select in the children
                    {
                        if (ExpandAndSelectItem(currentContainer, itemToSelect) == false)
                        {
                            //restore the current TreeViewItem's expanded state
                            currentContainer.IsExpanded = wasExpanded;
                        }
                        else //otherwise the node was found and selected, so return true
                        {
                            return true;
                        }
                    }
                }
            }

            //no item was found
            return false;
        }
    }
}
