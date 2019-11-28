using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ForRealRecordings.Web.TagHelpers
{
	[HtmlTargetElement(Attributes = "is-active")]
	public class IsActiveTagHelper : TagHelper
	{
		private readonly IHttpContextAccessor _contextAccessor;

		public IsActiveTagHelper(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			base.Process(context, output);

			if (ShouldBeActive(context.AllAttributes["href"].Value.ToString()))
			{
				MakeActive(output);
			}

			output.Attributes.RemoveAll("is-active");
		}

		private bool ShouldBeActive(string href)
		{
			var path = _contextAccessor.HttpContext.Request.Path.Value.ToLower();

			switch (href)
			{
				case "/":
					return path == href || path == "/index";
				default:
					return path == href || path.StartsWith(href);
			}
		}

		private void MakeActive(TagHelperOutput output)
		{
			var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
			if (classAttr == null)
			{
				classAttr = new TagHelperAttribute("class", "active");
				output.Attributes.Add(classAttr);
			}
			else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf("active") < 0)
			{
				output.Attributes.SetAttribute("class", classAttr.Value == null
					? "active"
					: classAttr.Value.ToString() + " active");
			}
		}
	}
}