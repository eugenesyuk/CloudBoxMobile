using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CloudBoxMobile {
	public class User {
		[JsonIgnore]
		[XmlElement("UserId")]
		public int Id {
			get;
			set;
		}

		[JsonPropertyAttribute("FirstName")]
		public string FirstName {
			get;
			set;
		}

		[JsonPropertyAttribute("LastName")]
		public string LastName {
			get;
			set;
		}

		[JsonPropertyAttribute("Email")]
		public string Email {
			get;
			set;
		}

		[JsonPropertyAttribute("Password")]
		public string Password {
			get;
			set;
		}

		[JsonIgnore]
		public DateTime SessionExpiresAt {
			get;
			set;
		}

		[JsonIgnore]
		public string Token {
			get;
			set;
		}

		public override string ToString() {
			return string.Format("{0} {1}", FirstName, LastName);
		}
	}
}
