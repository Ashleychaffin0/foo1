using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ApplicationModel.Environments;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace TextAdornment1 {
	#region Adornment Factory
	/// <summary>
	/// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
	/// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
	/// </summary>
	[Export(typeof(IWpfTextViewCreationListener))]
	[ContentType("text")]
	[TextViewRole(PredefinedTextViewRoles.Document)]
	internal sealed class EditorAdornmentFactory : IWpfTextViewCreationListener {

#pragma warning disable 649 // no warning needed because this is an export
		/// <summary>
		/// Defines the adornment layer for the scarlet adornment. This layer is ordered 
		/// after the selection layer in the Z-order
		/// </summary>
		[Export(typeof(AdornmentLayerDefinition))]
		[Name("ScarletCharacter")]
		[Order(After = DefaultAdornmentLayers.Selection)]
		[TextViewRole(PredefinedTextViewRoles.Document)]
		public AdornmentLayerDefinition editorAdornmentLayer;
#pragma warning restore 649

		/// <summary>
		/// Creates a ScarletCharacter adornment manager when a textview is created
		/// </summary>
		/// <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
		/// <param name="context">Is not currently used and will likely be removed</param>
		public void TextViewCreated(IWpfTextView textView, IEnvironment context) {
			new ScarletCharacter(textView);
		}
	}
	#endregion //Adornment Factory
}
