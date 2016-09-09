using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace CloudBoxMobile {
	public static class Service {
		private static string BaseUrl = "http://eefcaa9c75ef45da84b173711d874366.cloudapp.net/CloudBoxService.svc";
		private static JObject resSet;
		private static byte[] img;
		private static int IndexToDelete;
		private static JObject metaSet;
		private static String Filename;
		private static IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
		private static IsolatedStorageFileStream streamToWriteTo; 
		public static List<FSObject> Filelist = new List<FSObject>();

		public static bool RegisterUser(User user) {
			try {
				WebClient client = new WebClient();
				Uri url = new Uri(string.Format("{0}/user/create", BaseUrl));
				client.UploadStringAsync(url, "POST", JsonConvert.SerializeObject(user));
				return true;
			} catch(Exception e) {
				System.Console.WriteLine(e);
				return false;
			}
		}

		public static bool AuthorizeUser(ref User user) {

			HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(string.Format("{0}/user/authorize?email={1}&password={2}", BaseUrl, user.Email, user.Password));
			request.BeginGetResponse(AuthorizeCallback, request);
			return true;
		}

		private static void AuthorizeCallback(IAsyncResult result) {
			HttpWebRequest request = result.AsyncState as HttpWebRequest;
			if(request != null) {
				try {
					WebResponse response = request.EndGetResponse(result);
					Stream toRead = response.GetResponseStream();
					StreamReader reader = new StreamReader(toRead);
					resSet = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
					ServiceContainer.Instance.User.Id = resSet["UserID"].Value<int>();
					ServiceContainer.Instance.User.SessionExpiresAt = resSet["ExpiredAt"].Value<DateTime>();
					ServiceContainer.Instance.User.Token = resSet["Token"].Value<string>();
					Deployment.Current.Dispatcher.BeginInvoke(delegate() {
						MessageBox.Show("Succesfully authorized");
						(Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainWindow.xaml", UriKind.RelativeOrAbsolute));
					});
					reader.Close();
					response.Close();
				} catch(WebException) {
					Deployment.Current.Dispatcher.BeginInvoke(delegate() {
						MessageBox.Show("User not found");
					});
					return;
				}
			}
		}


		public static void GetRoot() {
			Filelist.Clear();
			HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(string.Format("{0}/getmetadata/Root/?token={2}", BaseUrl, ServiceContainer.Instance.User.Id, ServiceContainer.Instance.User.Token));
			req.BeginGetResponse(GetRCallback, req);
		}

		private static void GetRCallback(IAsyncResult result) {
			HttpWebRequest request = result.AsyncState as HttpWebRequest;
			if(request != null) {
				try {
					WebResponse response = request.EndGetResponse(result);
					StreamReader reader = new StreamReader(response.GetResponseStream());
					var respObj = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
					if(respObj["Contents"] != null) {
						foreach(var arrItem in respObj["Contents"].Value<JArray>()) {
							Filelist.Add(new FSObject {
								Id = arrItem["ID"].Value<int>(),
								Type = arrItem["Type"].Value<string>(),
								Title = arrItem["Title"].Value<string>(),
								ParentId = arrItem["ParentID"].Value<string>(),
								Modified = arrItem["ModifiedDate"].Value<DateTime>(),
								UserId = arrItem["UserID"].Value<int>(),
								Size = Converter.GetSizeReadable(arrItem["Bytes"].Value<int>())
							});
							reader.Close();
							response.Close();
							MainWindow.thread.Dispatcher.BeginInvoke(delegate() {
								MainWindow.thread.Files.DataContext = Filelist;

							});
						}
					} else {
						MainWindow.thread.Dispatcher.BeginInvoke(delegate() {
							MainWindow.thread.Status.Text = "No files";
						});
					}
				} catch(WebException ex) {

				}

			}




		}

		public static void Upload(Stream stream, string name) {
			HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(string.Format("{0}/fileops/upload_file/Root?token={1}", BaseUrl, ServiceContainer.Instance.User.Token));
			Filename = name;
			req.Method = "POST";
			int len = (int) stream.Length;
			img = new byte[len];
			stream.Read(img, 0, len);
			stream.Close();
			req.BeginGetRequestStream(new AsyncCallback(UploadCallback), req);
		}

		private static void UploadCallback(IAsyncResult asynchronousResult) {
			HttpWebRequest request = (HttpWebRequest) asynchronousResult.AsyncState;
			Stream postStream = request.EndGetRequestStream(asynchronousResult);
			postStream.Write(img, 0, img.Length);
			postStream.Close();

			request.BeginGetResponse(UpCall, request);
		}

		private static void UpCall(IAsyncResult result) {
			HttpWebRequest request = result.AsyncState as HttpWebRequest;
			if(request != null) {
				try {
					WebResponse response = request.EndGetResponse(result);
					StreamReader reader = new StreamReader(response.GetResponseStream());
					resSet = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
					resSet["Title"] = Filename;
					SetFileMetadata(resSet);
					reader.Close();
					response.Close();
					Filelist.Add(new FSObject {
						Id = resSet["ID"].Value<int>(),
						Type = resSet["Type"].Value<string>(),
						Title = Filename,
						ParentId = resSet["ParentID"].Value<string>(),
						Modified = resSet["ModifiedDate"].Value<DateTime>(),
						UserId = resSet["UserID"].Value<int>(),
						Size = Converter.GetSizeReadable(resSet["Bytes"].Value<int>())
					});
					Deployment.Current.Dispatcher.BeginInvoke(delegate() {
						MessageBox.Show("Succesfully uploaded");
					});
				} catch(WebException) {
					Deployment.Current.Dispatcher.BeginInvoke(delegate() {
						MessageBox.Show("Error");
					});
					return;
				}
			}
		}

		public static void SetFileMetadata(JObject info) {
			HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(string.Format("{0}/setmetadata?token={1}", BaseUrl, ServiceContainer.Instance.User.Token));
			metaSet = info;
			req.Method = "POST";
			req.BeginGetRequestStream(new AsyncCallback(SetMetadataCallback), req);
		}

		private static void SetMetadataCallback(IAsyncResult callbackResult) {
			HttpWebRequest myRequest = (HttpWebRequest) callbackResult.AsyncState;
			Stream postStream = myRequest.EndGetRequestStream(callbackResult);
			var bytesToSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(metaSet));
			postStream.Write(bytesToSend, 0, bytesToSend.Length);
			postStream.Close();
			myRequest.BeginGetResponse(new AsyncCallback(SetMCall), myRequest);
		}

		private static void SetMCall(IAsyncResult result) {
			HttpWebRequest request = result.AsyncState as HttpWebRequest;
			if(request != null) {
				try {
					WebResponse response = request.EndGetResponse(result);
					StreamReader reader = new StreamReader(response.GetResponseStream());
					resSet = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
					reader.Close();
					response.Close();
					Deployment.Current.Dispatcher.BeginInvoke(delegate() {
						MessageBoxResult m = MessageBox.Show("File list will be reloaded now", "Window reload", MessageBoxButton.OK);
						if(m == MessageBoxResult.OK) {
							MainWindow.thread.Dispatcher.BeginInvoke(delegate() {
								MainWindow.thread.Files.DataContext = null;
								MainWindow.thread.Files.UpdateLayout();
								MainWindow.thread.Files.DataContext = Filelist;
								MainWindow.thread.Files.UpdateLayout();
							});

						}
					});
				} catch(WebException) {
					Deployment.Current.Dispatcher.BeginInvoke(delegate() {
						MessageBox.Show("Error");
					});
					return;
				}
			}
		}

		public static void DeleteFile(string id, int index) {
			HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(string.Format("{0}/remove/{1}?token={2}", BaseUrl, id, ServiceContainer.Instance.User.Token));
			IndexToDelete = index;
			req.BeginGetResponse(DeleteCallback, req);
		}

		private static void DeleteCallback(IAsyncResult result) {
			HttpWebRequest request = result.AsyncState as HttpWebRequest;
			if(request != null) {
				try {
					WebResponse response = request.EndGetResponse(result);
					response.GetResponseStream();
					response.Close();
					Filelist.RemoveAt(IndexToDelete);
					Deployment.Current.Dispatcher.BeginInvoke(delegate() {
						MessageBoxResult m = MessageBox.Show("Succesfully deleted", "File info", MessageBoxButton.OK);
						if(m == MessageBoxResult.OK) {
							MainWindow.thread.Dispatcher.BeginInvoke(delegate() {
								MainWindow.thread.Files.DataContext = null;
								MainWindow.thread.Files.UpdateLayout();
								MainWindow.thread.Files.DataContext = Filelist;
								MainWindow.thread.Files.UpdateLayout();
								MainWindow.thread.Dispatcher.BeginInvoke(delegate() {
									MainWindow.thread.NavigationService.GoBack();
								});
							});
						}
					});
				} catch(WebException) {
					Deployment.Current.Dispatcher.BeginInvoke(delegate() {
						MessageBox.Show("Error");
					});
				}
			}
		}

		public static void DownloadFile(string id,string name) {
			streamToWriteTo = new IsolatedStorageFileStream(name, FileMode.Create, file);
			HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(string.Format("{0}/fileops/download_file/{1}?token={2}", BaseUrl, id, ServiceContainer.Instance.User.Token));
			req.Method = "GET";
			req.AllowReadStreamBuffering = false;
			req.BeginGetResponse(new AsyncCallback(GetData), req);
		}

		private static void GetData(IAsyncResult result) {
			HttpWebRequest request = (HttpWebRequest) result.AsyncState;
			HttpWebResponse response = (HttpWebResponse) request.EndGetResponse(result);

			Stream str = response.GetResponseStream();

			byte[] data = new byte[16 * 1024];
			int read;
			long totalValue = response.ContentLength;
			while((read = str.Read(data, 0, data.Length)) > 0) {
				if(streamToWriteTo.Length != 0)
				streamToWriteTo.Write(data, 0, read);
			}
			streamToWriteTo.Close();
			Deployment.Current.Dispatcher.BeginInvoke(delegate() {
				MessageBox.Show("Succesfully downloaded");
			});
		}

	}
}
