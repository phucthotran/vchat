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
        /// <summary>
        /// Command dùng cho tìm kiếm bạn bè
        /// </summary>
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

        /// <summary>
        /// Command dùng cho chỉnh sửa bạn bè
        /// </summary>
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

        /// <summary>
        /// Command dùng cho hủy thao tác chỉnh sửa bạn bè
        /// </summary>
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

        /// <summary>
        /// Command dùng cho thao tác chọn tất cả bạn bè
        /// </summary>
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

        /// <summary>
        /// Command dùng cho thao tác bỏ chọn tất cả bạn bè
        /// </summary>
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

        /// <summary>
        /// Command dùng cho thao tác di chuyển bạn bè
        /// </summary>
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

        /// <summary>
        /// Command dùng cho thao tác xóa bạn bè
        /// </summary>
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
        /// <summary>
        /// Command dùng cho thao tác chấp nhận yêu cầu kết bạn
        /// </summary>
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


        /// <summary>
        /// Command dùng cho thao tác từ chối yêu cầu kết bạn
        /// </summary>
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
