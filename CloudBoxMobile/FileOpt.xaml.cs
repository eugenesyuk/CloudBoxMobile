using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CloudBoxMobile {
	public partial class FileOpt : PhoneApplicationPage {
		public static FileOpt thread;
		private static string id;
		private static int index;
		public FileOpt() {
			InitializeComponent();
			thread = this;
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
			base.OnNavigatedTo(e);
			string msg = string.Empty;  
			if(NavigationContext.QueryString.TryGetValue("title", out msg)) {
				title.Text = msg;  
			}
			msg = string.Empty;         

			if(NavigationContext.QueryString.TryGetValue("size", out msg)) {
				size.Text = msg;  
			}

			if(NavigationContext.QueryString.TryGetValue("id", out msg)) {
				id = msg;
			}

			if(NavigationContext.QueryString.TryGetValue("index", out msg)) {
				index = Convert.ToInt32(msg);
			}
		}

		private void delete_Click(object sender, RoutedEventArgs e) {
			Service.DeleteFile(id,index);
		}

		private void download_Click(object sender, RoutedEventArgs e) {
			Service.DownloadFile(id, title.Text);
		}
	}
}