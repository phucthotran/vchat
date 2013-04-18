using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace vChat.Module.FriendList
{
    #region GROUP TREE VIEW COMMAND

    public partial class GroupTreeViewModel
    {
        private class SearchTask : BaseCommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public SearchTask(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public override void Execute(object Parameter)
            {
                _GroupTree.PerformSearch();
            }
        }

        private class EditTask : BaseCommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public EditTask(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }                      

            public override void Execute(object Parameter)
            {
                _GroupTree.PerformEdit();
            }
        }

        private class CancelEditTask : BaseCommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public CancelEditTask(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }                      

            public override void Execute(object Parameter)
            {
                _GroupTree.PerformCancelEdit();
            }
        }

        private class SelectTask : BaseCommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public SelectTask(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public override void Execute(object Parameter)
            {
                _GroupTree.PerformSelect();
            }
        }

        private class DeselectTask : BaseCommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public DeselectTask(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public override void Execute(object Parameter)
            {
                _GroupTree.PerformDeselect();
            }
        }

        private class MoveTask : BaseCommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public MoveTask(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public override void Execute(object parameter)
            {
                _GroupTree.PerformMove();
            }
        }

        private class RemoveTask : BaseCommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public RemoveTask(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public override void Execute(object parameter)
            {
                _GroupTree.PerformRemove();
            }
        }
    }

    #endregion

    #region REQUEST VIEW MODEL COMMAND

    public partial class RequestViewModel
    {
        private class AcceptTask : BaseCommand
        {
            private RequestViewModel _RequestVM;

            public AcceptTask(RequestViewModel RequestVM)
            {
                _RequestVM = RequestVM;
            }

            public override void Execute(object parameter)
            {
                _RequestVM.AcceptFriend();
            }
        }

        private class IgnoreTask : BaseCommand
        {
            private RequestViewModel _RequestVM;

            public IgnoreTask(RequestViewModel RequestVM)
            {
                _RequestVM = RequestVM;
            }

            public override void Execute(object parameter)
            {
                _RequestVM.IgnoreFriend();
            }
        }
    }

    #endregion
}
