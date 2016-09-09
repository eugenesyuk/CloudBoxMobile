using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CloudBoxMobile;

namespace CloudBoxMobile {
	public partial class Registration : PhoneApplicationPage {
		public Registration() {
			InitializeComponent();
		}

		private void name_GotFocus(object sender, RoutedEventArgs e) {
			if(name.Text == "First Name")
				name.Text = "";
		}

		private void surname_GotFocus(object sender, RoutedEventArgs e) {
			if(surname.Text == "Last Name")
				surname.Text = "";
		}

		private void email_GotFocus(object sender, RoutedEventArgs e) {
			if(email.Text == "E-mail")
				email.Text = "";
		}

		private void password_GotFocus(object sender, RoutedEventArgs e) {
			if(password.Text == "Password")
				password.Text = "";
		}

		private void confpassword_GotFocus(object sender, RoutedEventArgs e) {
			if(confpassword.Text == "Confirm Password")
				confpassword.Text = "";
		}

		private void name_LostFocus(object sender, RoutedEventArgs e) {
			if(name.Text == "")
				name.Text = "First Name";
		}

		private void surname_LostFocus(object sender, RoutedEventArgs e) {
			if(surname.Text == "")
				surname.Text = "Last Name";
		}

		private void email_LostFocus(object sender, RoutedEventArgs e) {
			if(email.Text == "")
				email.Text = "E-mail";
		}

		private void password_LostFocus(object sender, RoutedEventArgs e) {
			if(password.Text == "")
				password.Text = "Password";
		}

		private void confpassword_LostFocus(object sender, RoutedEventArgs e) {
			if(confpassword.Text == "")
				confpassword.Text = "Confirm Password";
		}

		private void name_TextChanged(object sender, TextChangedEventArgs e) {

		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			if(name.Text == "" || name.Text == "First Name" || surname.Text == "" || surname.Text == "Last Name" || email.Text == "" || email.Text == "E-mail" || password.Text == "" || password.Text == "Password" || confpassword.Text == "" || confpassword.Text == "Confirm Password") {
				lbl.Text = "Please complete all fields";
			} else if(password.Text != confpassword.Text) {
				lbl.Text = "Password's do not match";
			} else {
				lbl.Text = "Registration...";
				ServiceContainer.Instance.User = new User {
					FirstName = name.Text,
					LastName = surname.Text,
					Password = MD5Core.GetHashString(password.Text),
					Email = email.Text
				};
				if(Service.RegisterUser(ServiceContainer.Instance.User))
				{
				lbl.Text = "User has been created";
				} else {
					lbl.Text = "Error";
				}
			}
		}
	}
}