﻿#pragma checksum "D:\VdictOnline\VdictOnline\Dictionary.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FAE8136772EAAFB3044B51429A14843A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18331
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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


namespace VdictOnline {
    
    
    public partial class Dictionary : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.ListPicker ListLanguage;
        
        internal System.Windows.Controls.TextBox Textbox_Search;
        
        internal System.Windows.Controls.Button Button_Search;
        
        internal System.Windows.Controls.TextBlock Textbox_Pronounciation;
        
        internal System.Windows.Controls.Button Button_Listens;
        
        internal System.Windows.Controls.ProgressBar ProgressBar2;
        
        internal System.Windows.Controls.ListBox listData;
        
        internal System.Windows.Controls.MediaElement media;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton saveWord;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton goWord;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/VdictOnline;component/Dictionary.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ListLanguage = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("ListLanguage")));
            this.Textbox_Search = ((System.Windows.Controls.TextBox)(this.FindName("Textbox_Search")));
            this.Button_Search = ((System.Windows.Controls.Button)(this.FindName("Button_Search")));
            this.Textbox_Pronounciation = ((System.Windows.Controls.TextBlock)(this.FindName("Textbox_Pronounciation")));
            this.Button_Listens = ((System.Windows.Controls.Button)(this.FindName("Button_Listens")));
            this.ProgressBar2 = ((System.Windows.Controls.ProgressBar)(this.FindName("ProgressBar2")));
            this.listData = ((System.Windows.Controls.ListBox)(this.FindName("listData")));
            this.media = ((System.Windows.Controls.MediaElement)(this.FindName("media")));
            this.saveWord = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("saveWord")));
            this.goWord = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("goWord")));
        }
    }
}

