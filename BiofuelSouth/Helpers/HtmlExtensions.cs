using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BiofuelSouth.Helpers
{
	public static class HtmlExtensions
	{
		/// <summary>
		/// Returns the display name of a property
		/// Can be used as Html.GetDisplayName(x => x.SomeProperty);
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static MvcHtmlString GetDisplayName<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression
		)
		{
			var metaData = ModelMetadata.FromLambdaExpression<TModel, TProperty>( expression, htmlHelper.ViewData );
			string value = metaData.DisplayName ?? ( metaData.PropertyName ?? ExpressionHelper.GetExpressionText( expression ) );
			return MvcHtmlString.Create( value );
		}
	}
}
