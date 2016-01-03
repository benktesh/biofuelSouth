using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BiofuelSouth.Resources
{
    public class TooltipAttribute : DescriptionAttribute
    {
        public TooltipAttribute()
            : base("")
        {

        }

        public TooltipAttribute(string description)
            : base(description)
        {

        }
    }

    public static class HtmlHelpers
    {
        public static MvcHtmlString ToolTipFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var exp = (MemberExpression)expression.Body;
            foreach (Attribute attribute in exp.Expression.Type.GetProperty(exp.Member.Name).GetCustomAttributes(false))
            {
                if (typeof(TooltipAttribute) == attribute.GetType())
                {
                    return MvcHtmlString.Create(((TooltipAttribute)attribute).Description);
                }
            }
            return MvcHtmlString.Create("");
        }
    }
}