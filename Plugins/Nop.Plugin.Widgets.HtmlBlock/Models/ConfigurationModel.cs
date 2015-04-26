using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.HtmlBlock.Models
{	public class ConfigurationModel : BaseNopModel
	{
		[NopResourceDisplayName("Plugins.Widgets.HtmlBlock.HtmlContents")]
		[AllowHtml]		
		public string HtmlContents { get; set; }

		public bool HtmlContents_OverrideForStore { get; set; }
	}
}
