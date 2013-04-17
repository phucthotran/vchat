﻿#pragma checksum "..\..\..\FriendList\FriendsList.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "052B829AB79663AC5E40F02EB0953328"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
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
using vChat.Module.FriendList;


namespace vChat.Module.FriendList {
    
    
    /// <summary>
    /// FriendsList
    /// </summary>
    public partial class FriendsList : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSearch;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal vChat.Control.ImageButton btnEdit;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal vChat.Control.ImageButton btnSelectAll;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal vChat.Control.ImageButton btnDeselectAll;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView TreeFriend;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel completeTask;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbTask;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblNewGroup;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbNewGroup;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkDone;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\FriendList\FriendsList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDone;
        
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
            this.txtSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\..\FriendList\FriendsList.xaml"
            this.txtSearch.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtSearch_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.btnEdit = ((vChat.Control.ImageButton)(target));
            
            #line 30 "..\..\..\FriendList\FriendsList.xaml"
            this.btnEdit.Click += new System.Windows.RoutedEventHandler(this.btnEdit_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSelectAll = ((vChat.Control.ImageButton)(target));
            return;
            case 5:
            this.btnDeselectAll = ((vChat.Control.ImageButton)(target));
            return;
            case 6:
            this.TreeFriend = ((System.Windows.Controls.TreeView)(target));
            
            #line 38 "..\..\..\FriendList\FriendsList.xaml"
            this.TreeFriend.SelectedItemChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(this.TreeFriend_SelectedItemChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.completeTask = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.cbTask = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.tblNewGroup = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.cbNewGroup = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 11:
            this.chkDone = ((System.Windows.Controls.CheckBox)(target));
            
            #line 78 "..\..\..\FriendList\FriendsList.xaml"
            this.chkDone.Checked += new System.Windows.RoutedEventHandler(this.chkDone_Checked);
            
            #line default
            #line hidden
            
            #line 78 "..\..\..\FriendList\FriendsList.xaml"
            this.chkDone.Unchecked += new System.Windows.RoutedEventHandler(this.chkDone_Unchecked);
            
            #line default
            #line hidden
            return;
            case 12:
            this.btnDone = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

