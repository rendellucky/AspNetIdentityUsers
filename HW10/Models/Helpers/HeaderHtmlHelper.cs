using HW10.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetMvc.Models.Helpers;

public static class HeaderHtmlHelper
{
	public static HtmlString ListItemGroup(this IHtmlHelper helper, Group group)
	{
		return new HtmlString(string.Format("<li>({1}) - {0}</li>", group.Title, group.Id));
	}

	public static HtmlString ListItemSkill(this IHtmlHelper helper, Skill skill)
	{
		return new HtmlString(string.Format("<li>({1}) - {0}</li>", skill.Name, skill.Id));
	}
}
