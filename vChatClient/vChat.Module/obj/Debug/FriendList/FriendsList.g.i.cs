﻿#pragma checksum "..\..\..\FriendList\FriendsList.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C23DF000C1F69622B2096B9E38F1C14D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using vChat.Control;
using vChat.Model.Entities;
using vChat.Module.Avatar;
using vChat.Module.FriendList;


namespace vChat.Module.FriendList {
    
    
    /// <summary>
    /// FriendsList
    /// </summary>
    public partial class FriendsList : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 16 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal vChat.Module.Avatar.Avatar changeAvatarModule;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSearch;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal vChat.Control.ImageButton btnAddFriend;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal vChat.Control.ImageButton btnEdit;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Expander friendRequestZone;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel requestTaskZone;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbRequestTask;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbRequestGroup;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbGroupName;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkRequestTaskDone;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRequestTaskDone;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView treeFriend;
        
        #line default
        #line hidden
        
        
        #line 165 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal vChat.Control.ImageButton btnSelectAll;
        
        #line default
        #line hidden
        
        
        #line 166 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal vChat.Control.ImageButton btnDeselectAll;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border editTaskZone;
        
        #line default
        #line hidden
        
        
        #line 181 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbTask;
        
        #line default
        #line hidden
        
        
        #line 185 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblNewGroup;
        
        #line default
        #line hidden
        
        
        #line 186 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbNewGroup;
        
        #line default
        #line hidden
        
        
        #line 187 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblAddNewGroup;
        
        #line default
        #line hidden
        
        
        #line 188 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbAddNewGroup;
        
        #line default
        #line hidden
        
        
        #line 191 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkDone;
        
        #line default
        #line hidden
        
        
        #line 192 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDone;
        
        #line default
        #line hidden
        
        
        #line 193 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/vChat.Module;component/friendlist/friendslist.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\FriendList\FriendsList.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.changeAvatarModule = ((vChat.Module.Avatar.Avatar)(target));
            return;
            case 2:
            this.txtSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\..\FriendList\FriendsList.xaml"
            this.txtSearch.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtSearch_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.btnAddFriend = ((vChat.Control.ImageButton)(target));
            
            #line 36 "..\..\..\FriendList\FriendsList.xaml"
            this.btnAddFriend.Click += new System.Windows.RoutedEventHandler(this.btnAddFriend_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnEdit = ((vChat.Control.ImageButton)(target));
            
            #line 37 "..\..\..\FriendList\FriendsList.xaml"
            this.btnEdit.Click += new System.Windows.RoutedEventHandler(this.btnEdit_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.friendRequestZone = ((System.Windows.Controls.Expander)(target));
            return;
            case 7:
            this.requestTaskZone = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.cbRequestTask = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.cbRequestGroup = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 10:
            this.tbGroupName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.chkRequestTaskDone = ((System.Windows.Controls.CheckBox)(target));
            
            #line 95 "..\..\..\FriendList\FriendsList.xaml"
            this.chkRequestTaskDone.Checked += new System.Windows.RoutedEventHandler(this.chkRequestTaskDone_Checked);
            
            #line default
            #line hidden
            
            #line 95 "..\..\..\FriendList\FriendsList.xaml"
            this.chkRequestTaskDone.Unchecked += new System.Windows.RoutedEventHandler(this.chkRequestTaskDone_Unchecked);
            
            #line default
            #line hidden
            return;
            case 12:
            this.btnRequestTaskDone = ((System.Windows.Controls.Button)(target));
            
            #line 96 "..\..\..\FriendList\FriendsList.xaml"
            this.btnRequestTaskDone.Click += new System.Windows.RoutedEventHandler(this.btnRequestTaskDone_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.treeFriend = ((System.Windows.Controls.TreeView)(target));
            
            #line 112 "..\..\..\FriendList\FriendsList.xaml"
            this.treeFriend.SelectedItemChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(this.TreeFriend_SelectedItemChanged);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 128 "..\..\..\FriendList\FriendsList.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.mnuAddFriend_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 129 "..\..\..\FriendList\FriendsList.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.mnuRemoveGroup_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 132 "..\..\..\FriendList\FriendsList.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.mnuFriendDetail_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            this.btnSelectAll = ((vChat.Control.ImageButton)(target));
            return;
            case 19:
            this.btnDeselectAll = ((vChat.Control.ImageButton)(target));
            return;
            case 20:
            this.editTaskZone = ((System.Windows.Controls.Border)(target));
            return;
            case 21:
            this.cbTask = ((System.Windows.Controls.ComboBox)(target));
            
            #line 181 "..\..\..\FriendList\FriendsList.xaml"
            this.cbTask.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbTask_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 22:
            this.tblNewGroup = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 23:
            this.cbNewGroup = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 24:
            this.tblAddNewGroup = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 25:
            this.tbAddNewGroup = ((System.Windows.Controls.TextBox)(target));
            return;
            case 26:
            this.chkDone = ((System.Windows.Controls.CheckBox)(target));
            
            #line 191 "..\..\..\FriendList\FriendsList.xaml"
            this.chkDone.Checked += new System.Windows.RoutedEventHandler(this.chkDone_Checked);
            
            #line default
            #line hidden
            
            #line 191 "..\..\..\FriendList\FriendsList.xaml"
            this.chkDone.Unchecked += new System.Windows.RoutedEventHandler(this.chkDone_Unchecked);
            
            #line default
            #line hidden
            return;
            case 27:
            this.btnDone = ((System.Windows.Controls.Button)(target));
            
            #line 192 "..\..\..\FriendList\FriendsList.xaml"
            this.btnDone.Click += new System.Windows.RoutedEventHandler(this.btnDone_Click);
            
            #line default
            #line hidden
            return;
            case 28:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 193 "..\..\..\FriendList\FriendsList.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 14:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Control.MouseDoubleClickEvent;
            
            #line 115 "..\..\..\FriendList\FriendsList.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.TreeFriend_MouseDoubleClick);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

