//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=2.0.50727.42.
// 


/// <remarks/>
namespace TestLLImporter {

	[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
	// [System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name = "LLImporterSoap", Namespace = "http://www.LeadsLightning.com/LeadsLightningWS")]
	public partial class LLImporter : System.Web.Services.Protocols.SoapHttpClientProtocol {

		private System.Threading.SendOrPostCallback ImportOperationCompleted;

		/// <remarks/>
		public LLImporter() {
			this.Url = "http://www.leadslightning.com/leadslightningws/LLimporter/LLimporter.asmx";
		}

		/// <remarks/>
		public event ImportCompletedEventHandler ImportCompleted;

		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.LeadsLightning.com/LeadsLightningWS/Import", RequestNamespace = "http://www.LeadsLightning.com/LeadsLightningWS", ResponseNamespace = "http://www.LeadsLightning.com/LeadsLightningWS", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string Import(string UserId, string Password, string EventName, string SwipeData, string MapCfgFile, int MapType, string TerminalId) {
			object[] results = this.Invoke("Import", new object[] {
                    UserId,
                    Password,
                    EventName,
                    SwipeData,
                    MapCfgFile,
                    MapType,
                    TerminalId});
			return ((string)(results[0]));
		}

		/// <remarks/>
		public System.IAsyncResult BeginImport(string UserId, string Password, string EventName, string SwipeData, string MapCfgFile, int MapType, string TerminalId, System.AsyncCallback callback, object asyncState) {
			return this.BeginInvoke("Import", new object[] {
                    UserId,
                    Password,
                    EventName,
                    SwipeData,
                    MapCfgFile,
                    MapType,
                    TerminalId}, callback, asyncState);
		}

		/// <remarks/>
		public string EndImport(System.IAsyncResult asyncResult) {
			object[] results = this.EndInvoke(asyncResult);
			return ((string)(results[0]));
		}

		/// <remarks/>
		public void ImportAsync(string UserId, string Password, string EventName, string SwipeData, string MapCfgFile, int MapType, string TerminalId) {
			this.ImportAsync(UserId, Password, EventName, SwipeData, MapCfgFile, MapType, TerminalId, null);
		}

		/// <remarks/>
		public void ImportAsync(string UserId, string Password, string EventName, string SwipeData, string MapCfgFile, int MapType, string TerminalId, object userState) {
			if ((this.ImportOperationCompleted == null)) {
				this.ImportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnImportOperationCompleted);
			}
			this.InvokeAsync("Import", new object[] {
                    UserId,
                    Password,
                    EventName,
                    SwipeData,
                    MapCfgFile,
                    MapType,
                    TerminalId}, this.ImportOperationCompleted, userState);
		}

		private void OnImportOperationCompleted(object arg) {
			if ((this.ImportCompleted != null)) {
				System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
				this.ImportCompleted(this, new ImportCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
			}
		}

		/// <remarks/>
		public new void CancelAsync(object userState) {
			base.CancelAsync(userState);
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
	public delegate void ImportCompletedEventHandler(object sender, ImportCompletedEventArgs e);

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class ImportCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {

		private object[] results;

		internal ImportCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
			:
				base(exception, cancelled, userState) {
			this.results = results;
		}

		/// <remarks/>
		public string Result {
			get {
				this.RaiseExceptionIfNecessary();
				return ((string)(this.results[0]));
			}
		}
	}
}
