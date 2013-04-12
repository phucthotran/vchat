﻿#pragma checksum "..\..\..\..\..\View\Controls\Login.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B190CAFB4352881CAC3F6D186E28DF6F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Elysium.Controls;
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
using vChat.Templates;


namespace vChat.View.Controls {
    
    
    /// <summary>
    /// Login
    /// </summary>
    public partial class Login : vChat.Templates.vChatController, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\..\View\Controls\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbUser;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\..\View\Controls\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbPass;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\..\View\Controls\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btSubmit;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\..\View\Controls\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Elysium.Controls.ToggleSwitch blRemember;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\..\View\Controls\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\View\Controls\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label2;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\View\Controls\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btSignUp;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\..\View\Controls\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label3;
        
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
            System.Uri resourceLocater = new System.Uri("/vChatClient;component/view/controls/login.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\Controls\Login.xaml"
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
            this.tbUser = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.tbPass = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.btSubmit = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\..\..\View\Controls\Login.xaml"
            this.btSubmit.Click += new System.Windows.RoutedEventHandler(this.btSubmit_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.blRemember = ((Elysium.Controls.ToggleSwitch)(target));
            return;
            case 5:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.label2 = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.btSignUp = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\..\..\View\Controls\Login.xaml"
            this.btSignUp.Click += new System.Windows.RoutedEventHandler(this.btSignUp_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.label3 = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

