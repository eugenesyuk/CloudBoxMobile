﻿#pragma checksum "D:\CloudBoxMobile\CloudBoxMobile\CloudBoxMobile\FileOpt.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4480E5C7498C9F5C577857DF12662C0E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace CloudBoxMobile {
    
    
    public partial class FileOpt : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Viewbox ContentPanel;
        
        internal System.Windows.Controls.TextBlock title;
        
        internal System.Windows.Controls.TextBlock size;
        
        internal System.Windows.Controls.Button download;
        
        internal System.Windows.Controls.Button delete;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/CloudBoxMobile;component/FileOpt.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Viewbox)(this.FindName("ContentPanel")));
            this.title = ((System.Windows.Controls.TextBlock)(this.FindName("title")));
            this.size = ((System.Windows.Controls.TextBlock)(this.FindName("size")));
            this.download = ((System.Windows.Controls.Button)(this.FindName("download")));
            this.delete = ((System.Windows.Controls.Button)(this.FindName("delete")));
        }
    }
}

