using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CloudBoxMobile {
	public class FSObject {
		[JsonPropertyAttribute("ID")]
		public int Id {
			get;
			set;
		}

		[JsonPropertyAttribute("ParentID")]
		public string ParentId {
			get;
			set;
		}

		public string Title {
			get;
			set;
		}

		public string Type {
			get;
			set;
		}

		[JsonPropertyAttribute("ModifiedDate")]
		public DateTime Modified {
			get;
			set;
		}

		[JsonIgnore]
		[XmlIgnoreAttribute]
		public int UserId {
			get;
			set;
		}

		public string Size {
			get;
			set;
		}

		
	}
}
