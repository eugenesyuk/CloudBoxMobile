using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace CloudBoxMobile {
	public partial class MainWindow : PhoneApplicationPage {
		public static MainWindow thread;
		PhotoChooserTask photoChooserTask;
		public MainWindow() {
			InitializeComponent();
			photoChooserTask = new PhotoChooserTask();
			photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
			thread = this;
			Service.GetRoot();
		}

		private void Upload_Click(object sender, RoutedEventArgs e) {
			photoChooserTask.Show();
		}

		void photoChooserTask_Completed(object sender, PhotoResult e) {
			if(e.TaskResult == TaskResult.OK) {
				var name = System.IO.Path.GetFileName(e.OriginalFileName);
				Service.Upload(e.ChosenPhoto, name);

			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {

			NavigationService.RemoveBackEntry();

		}

		private void Refresh_Click(object sender, RoutedEventArgs e) {
			Service.GetRoot();
		}

		private void Files_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if(Files.SelectedIndex != -1) {
				FSObject data = (sender as ListBox).SelectedItem as FSObject;
				NavigationService.Navigate(new Uri("/FileOpt.xaml?title=" + data.Title + "&id=" + data.Id + "&size=" + data.Size + "&index=" + Files.SelectedIndex, UriKind.Relative));
			}

		}


	}
}