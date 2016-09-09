using System.Collections.Generic;


namespace CloudBoxMobile {
	public class ServiceContainer {
		private static ServiceContainer _Instance;

		public static ServiceContainer Instance {
			get {
				if(_Instance == null)
					_Instance = new ServiceContainer();
				return _Instance;
			}
			set {
				_Instance = value;
			}
		}

		public User User {
			get;
			set;
		}

		public List<FSObject> Files {
			get;
			set;
		}
	}
}
