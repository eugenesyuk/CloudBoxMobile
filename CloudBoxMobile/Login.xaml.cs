using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace CloudBoxMobile {
	public partial class MainPage : PhoneApplicationPage {
		public static MainPage thread;
		public MainPage() {
			InitializeComponent();
			thread = this;
		}

		private void Login_GotFocus(object sender, RoutedEventArgs e) {
			if(Login.Text == "Login")
				Login.Text = "";
		}

		private void Password_GotFocus(object sender, RoutedEventArgs e) {
			if(Password.Text == "Password")
				Password.Text = "";
		}

		private void Login_LostFocus(object sender, RoutedEventArgs e) {
			if(Login.Text == "")
				Login.Text = "Login";
		}

		private void Password_LostFocus(object sender, RoutedEventArgs e) {
			if(Password.Text == "")
				Password.Text = "Password";
		}

		private void register_Click(object sender, RoutedEventArgs e) {
			NavigationService.Navigate(new Uri("/Registration.xaml",UriKind.Relative));
		}

		private void login_Click(object sender, RoutedEventArgs e) {
			if(Login.Text != "" && Password.Text != "" && Login.Text != "Login" && Password.Text != "Password") {
				if(ServiceContainer.Instance.User == null)
					ServiceContainer.Instance.User = new User();
				ServiceContainer.Instance.User.Email = Login.Text;
				ServiceContainer.Instance.User.Password = MD5Core.GetHashString(Password.Text);
				User user = ServiceContainer.Instance.User;
				try {
					Service.AuthorizeUser(ref user);
				} catch(ServiceException m) {
					Console.WriteLine(m);
				}
			} 
		}
	}
}