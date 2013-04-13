﻿#pragma checksum "..\..\..\..\..\View\Windows\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4E0C6BE082739652180B22545D070A7B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Elysium;
using Elysium.Controls;
using Elysium.Converters;
using Elysium.Parameters;
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


namespace vChat.View.Windows {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\..\View\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Glyphs ThemeGlyph;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\View\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Glyphs AccentGlyph;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\..\..\View\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Glyphs ContrastGlyph;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\..\..\View\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Grid;
        
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
            System.Uri resourceLocater = new System.Uri("/vChatClient;component/view/windows/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\Windows\MainWindow.xaml"
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
            this.ThemeGlyph = ((System.Windows.Documents.Glyphs)(target));
            
            #line 12 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            this.ThemeGlyph.Initialized += new System.EventHandler(this.ThemeGlyphInitialized);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 21 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.LightClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 23 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DarkClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AccentGlyph = ((System.Windows.Documents.Glyphs)(target));
            
            #line 30 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            this.AccentGlyph.Initialized += new System.EventHandler(this.AccentGlyphInitialized);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 39 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 46 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 53 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 60 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 67 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 74 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 81 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 88 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 95 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 102 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 109 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 116 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 123 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 130 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AccentClick);
            
            #line default
            #line hidden
            return;
            case 19:
            this.ContrastGlyph = ((System.Windows.Documents.Glyphs)(target));
            
            #line 142 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            this.ContrastGlyph.Initialized += new System.EventHandler(this.ContrastGlyphInitialized);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 151 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.WhiteClick);
            
            #line default
            #line hidden
            return;
            case 21:
            
            #line 153 "..\..\..\..\..\View\Windows\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.BlackClick);
            
            #line default
            #line hidden
            return;
            case 22:
            this.Grid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

