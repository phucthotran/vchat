﻿#pragma checksum "..\..\..\Avatar\Avatar.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F1CE0CC10493D7737C34F7DC3353B814"
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


namespace vChat.Module.Avatar {
    
    
    /// <summary>
    /// Avatar
    /// </summary>
    public partial class Avatar : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\Avatar\Avatar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgAvatar;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Avatar\Avatar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbChangeAvatar;
        
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
            System.Uri resourceLocater = new System.Uri("/vChat.Module;component/avatar/avatar.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Avatar\Avatar.xaml"
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
            this.imgAvatar = ((System.Windows.Controls.Image)(target));
            return;
            case 2:
            this.tbChangeAvatar = ((System.Windows.Controls.TextBlock)(target));
            
            #line 12 "..\..\..\Avatar\Avatar.xaml"
            this.tbChangeAvatar.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.tbChangeAvatar_MouseDown);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\Avatar\Avatar.xaml"
            this.tbChangeAvatar.MouseEnter += new System.Windows.Input.MouseEventHandler(this.tbChangeAvatar_MouseEnter);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\Avatar\Avatar.xaml"
            this.tbChangeAvatar.MouseLeave += new System.Windows.Input.MouseEventHandler(this.tbChangeAvatar_MouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

