using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HAP = HtmlAgilityPack;

/*
        <tr class="js-navigation-item">
          <td class="icon">
            <svg class="octicon octicon-file" viewBox="0 0 12 16" version="1.1" width="12" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M6 5H2V4h4v1zM2 8h7V7H2v1zm0 2h7V9H2v1zm0 2h7v-1H2v1zm10-7.5V14c0 .55-.45 1-1 1H1c-.55 0-1-.45-1-1V2c0-.55.45-1 1-1h7.5L12 4.5zM11 5L8 2H1v12h10V5z"/></svg>
            <img width="16" height="16" class="spinner" alt="" src="https://assets-cdn.github.com/images/spinners/octocat-spinner-32.gif" />
          </td>
          <td class="content">
            <span class="css-truncate css-truncate-target"><a class="js-navigation-open" title="LDM-2013-10-07.md" id="eecdcddc275adb1e0d15ebdb5f3a14ae-6b06555dec4c76e0845eb423fecf1e9c6530e8c3" href="/dotnet/csharplang/blob/master/meetings/2013/LDM-2013-10-07.md">LDM-2013-10-07.md</a></span>
          </td>
          <td class="message">
            <span class="css-truncate css-truncate-target">
                  <a data-pjax="true" title="Added 2013 and 2014 LDM meeting notes.  (#26)" class="message" href="/dotnet/csharplang/commit/172a371cd929d32cc0ef0344a2cee04745975e9d">Added 2013 and 2014 LDM meeting notes. (</a><a class="issue-link js-issue-link" data-error-text="Failed to load issue title" data-id="205475550" data-permission-text="Issue title is private" data-url="https://github.com/dotnet/csharplang/issues/26" href="https://github.com/dotnet/csharplang/pull/26">#26</a><a data-pjax="true" title="Added 2013 and 2014 LDM meeting notes.  (#26)" class="message" href="/dotnet/csharplang/commit/172a371cd929d32cc0ef0344a2cee04745975e9d">)</a>
            </span>
          </td>
          <td class="age">
            <span class="css-truncate css-truncate-target"><time-ago datetime="2017-02-09T17:25:09Z">Feb 9, 2017</time-ago></span>
          </td>
        </tr>
*/

namespace DowloadCSharpLangMeetings {
	public partial class DowloadCSharpLangMeetings : Form {
		string BaseDir = "https://github.com/dotnet/csharplang/tree/master/meetings";
		public DowloadCSharpLangMeetings() {
			InitializeComponent();

			for (int year = 2013; year < 2030; year++) {
				string BaseYear = Path.Combine(BaseDir, $"{year}");
				var web = new HAP.HtmlWeb();
				var doc = web.Load(BaseYear);
				var nodes = from node in doc.DocumentNode.SelectNodes("//tr")
							where node.GetAttributeValue("class", string.Empty) == "js-navigation-item"
							select node;
				foreach (var node in nodes) {
					Console.WriteLine(node.InnerText.Trim());
					Console.WriteLine("==============");
				}
			}
		}
	}
}
