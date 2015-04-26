using Nop.Services.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Plugins;
using System.Web.Routing;

namespace Nop.Plugin.Widgets.HtmlBlock
{
	public class HtmlBlockPlugin : BasePlugin, IWidgetPlugin
	{
		public void GetConfigurationRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
		{
			actionName = "Configure";
			controllerName = "WidgetsHtmlBlock";
			routeValues = new RouteValueDictionary()
			{
				{ "Namespaces", "Nop.Plugin.Widgets.HtmlBlock.Controllers" },
				{ "area", null }
			};
		}

		public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
		{
			controllerName = "WidgetsHtmlBlock";
			actionName = "HtmlBlock";
			routeValues = new RouteValueDictionary()
			{
				{ "Namespaces", "Nop.Plugin.Widgets.HtmlBlock.Controllers" },
				{ "area", null },
				{ "widgetZone", widgetZone}
			};		
                
		}

		public IList<string> GetWidgetZones()
		{
			return new List<string>
			{
				"left_side_column_after"
			};			
		}

		//public void Install()
		//{			
		//}

		//public void Uninstall()
		//{		
		//}
	}
}
