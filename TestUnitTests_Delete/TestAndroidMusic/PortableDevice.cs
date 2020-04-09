using PortableDeviceApiLib;

namespace TestAndroidMusic {
	public class PortableDevice {
		private bool _isConnected;
		private readonly PortableDevice _device;

		public PortableDevice(string deviceId) {
			_device  = new PortableDevice(deviceId);
			DeviceId = deviceId;
		}

		public string DeviceId { get; set; }

		public void Connect() {
			if (_isConnected) { return; }

			var clientInfo = (IPortableDeviceValues)new PortableDeviceValues();
			_device.Open(DeviceId, clientInfo);
			_isConnected = true;
		}

		public void Disconnect() {
			if (!this._isConnected) { return; }
			_device.Close();
			_isConnected = false;
		}
	}
}
