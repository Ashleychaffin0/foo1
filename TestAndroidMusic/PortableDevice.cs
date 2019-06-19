using PortableDeviceApiLib;

namespace TestAndroidMusic {
	public class PortableDevice {
		private bool _isConnected;
		private readonly PortableDevice _device;

		public PortableDevice(string deviceId) {
			this._device = new PortableDevice(deviceId);
			this.DeviceId = deviceId;
		}

		public string DeviceId { get; set; }

		public void Connect() {
			if (this._isConnected) { return; }

			var clientInfo = (IPortableDeviceValues)new PortableDeviceValues();
			this._device.Open(this.DeviceId, clientInfo);
			this._isConnected = true;
		}

		public void Disconnect() {
			if (!this._isConnected) { return; }
			this._device.Close();
			this._isConnected = false;
		}
	}
}
