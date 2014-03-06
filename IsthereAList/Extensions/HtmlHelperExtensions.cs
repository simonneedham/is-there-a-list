using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace IsThereAList.Extensions
{
    // Lifted & then adapted from the Microsoft MVC5 project on CodePlex
    // Specificaly from:
    // ../src/Microsoft.Web.Mvc/Html/HtmlHelperExtensions.cs
    // ../src/System.Web.Mvc/Html/SelectExtensions.cs
    // ../src/System.Web.Mvc/HtmlHelper.cs

    //Where still applicable .Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. 

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString BootstrapEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var addtionalViewData = new { htmlAttributes = new { @class = "form-control" } };
            return html.EditorFor<TModel, TValue>(expression, addtionalViewData);
        }

        public static MvcHtmlString BootstrapEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string cssClasses)
        {
            var addtionalViewData = new { htmlAttributes = new { @class = "form-control " + cssClasses } };
            return html.EditorFor<TModel, TValue>(expression, addtionalViewData);
        }

        public static MvcHtmlString ListItemLink<TModel>(this HtmlHelper<TModel> htmlHelper, string linkText, int listId, string action, int? listItemId = null, string cssClass = null, string id = null, string style = null)
        {
            return htmlHelper.ListItemLink(linkText, listId, action, listItemId, Attributes(cssClass, id, style));
        }

        public static MvcHtmlString ListItemLink<TModel>(this HtmlHelper<TModel> htmlHelper, string linkText, int listId, string action, int? listItemId)
        {
            return htmlHelper.ListItemLink(linkText, listId, action, listItemId, null);
        }

        public static MvcHtmlString ListItemLink<TModel>(this HtmlHelper<TModel> htmlHelper, string linkText, int listId, string action, object htmlAttributes)
        {
            return htmlHelper.ListItemLink(linkText, listId, action, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ListItemLink<TModel>(this HtmlHelper<TModel> htmlHelper, string linkText, int listId, string action, int? listItemId, object htmlAttributes)
        {
            return htmlHelper.ListItemLink(linkText, listId, action, listItemId, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        //IDictionary<string, object> htmlAttributes
        public static MvcHtmlString ListItemLink<TModel>(this HtmlHelper<TModel> htmlHelper, string linkText, int listId, string action, int? listItemId, IDictionary<string, object> htmlAttributes)
        {
            string url;

            if (listItemId.HasValue)
                url = String.Format("/list/{0}/listitem/{1}", listId, listItemId);
            else
                url = String.Format("/list/{0}/listitem", listId);

            var tb = new TagBuilder("a");
            tb.MergeAttributes(htmlAttributes);
            tb.MergeAttribute("href", url);
            tb.SetInnerText(linkText);

            return MvcHtmlString.Create(tb.ToString());
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList = null, string cssClass = null, string dir = null, bool disabled = false, string id = null, string lang = null, int? size = null, string style = null, int? tabIndex = null, string title = null)
        {
            return htmlHelper.CheckBoxListFor(
                expression,
                selectList,
                SelectAttributes(cssClass, dir, disabled, id, lang, size, style, tabIndex, title));
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, null /* htmlAttributes */);
        }

        public static MvcHtmlString ListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            return CheckBoxListFor(htmlHelper,
                                 metadata,
                                 ExpressionHelper.GetExpressionText(expression),
                                 selectList,
                                 htmlAttributes);
        }

        private static MvcHtmlString CheckBoxListFor(HtmlHelper htmlHelper, ModelMetadata metadata, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            bool usedViewData = false;

            // If we got a null selectList, try to use ViewData to get the list of items.
            if (selectList == null)
            {
                selectList = htmlHelper.GetSelectData(name);
                usedViewData = true;
            }

            //object defaultValue = (allowMultiple) ? htmlHelper.GetModelStateValue(fullName, typeof(string[])) : htmlHelper.GetModelStateValue(fullName, typeof(string));
            object defaultValue = htmlHelper.ViewData.Eval(name);

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (defaultValue == null && !String.IsNullOrEmpty(name))
            {
                if (!usedViewData)
                {
                    defaultValue = htmlHelper.ViewData.Eval(name);
                }
                else if (metadata != null)
                {
                    defaultValue = metadata.Model;
                }
            }

            //Apply default values to select list items
            //
            if (defaultValue != null)
                selectList = GetSelectListWithDefaultValue(selectList, defaultValue, true);

            //Build List of check box items
            var checkListBuilder = new StringBuilder();
            foreach (var selectListItem in selectList)
            {
                var tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.MergeAttribute("type", "checkbox", true);
                tagBuilder.MergeAttribute("name", fullName, true);

                if (selectListItem.Value != null)
                    tagBuilder.Attributes["value"] = selectListItem.Value;

                if (selectListItem.Selected)
                    tagBuilder.Attributes["checked"] = "checked";

                checkListBuilder.Append(tagBuilder.ToString(TagRenderMode.Normal));

                tagBuilder = new TagBuilder("span")
                {
                    InnerHtml = selectListItem.Text ?? String.Empty
                };
                tagBuilder.AddCssClass("checkboxText");
                tagBuilder.AddCssClass(String.Format("value_{0}", selectListItem.Value ?? String.Empty));

                checkListBuilder.Append(tagBuilder.ToString(TagRenderMode.Normal));
                checkListBuilder.Append("<BR/>");
            }

            return MvcHtmlString.Create(checkListBuilder.ToString());
        }


        // Helper methods
        private static IEnumerable<SelectListItem> GetSelectListWithDefaultValue(IEnumerable<SelectListItem> selectList, object defaultValue, bool allowMultiple)
        {
            IEnumerable defaultValues;

            if (allowMultiple)
            {
                defaultValues = defaultValue as IEnumerable;
                if (defaultValues == null || defaultValues is string)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.CurrentCulture,
                            "Select Expression Not Enumerable",
                            "expression"));
                }
            }
            else
            {
                defaultValues = new[] { defaultValue };
            }

            IEnumerable<string> values = from object value in defaultValues
                                         select Convert.ToString(value, CultureInfo.CurrentCulture);

            // ToString() by default returns an enum value's name.  But selectList may use numeric values.
            IEnumerable<string> enumValues = from Enum value in defaultValues.OfType<Enum>()
                                             select value.ToString("d");
            values = values.Concat(enumValues);

            HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
            List<SelectListItem> newSelectList = new List<SelectListItem>();

            foreach (SelectListItem item in selectList)
            {
                item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                newSelectList.Add(item);
            }
            return newSelectList;
        }

        private static IEnumerable<SelectListItem> GetSelectData(this HtmlHelper htmlHelper, string name)
        {
            object o = null;
            if (htmlHelper.ViewData != null)
            {
                o = htmlHelper.ViewData.Eval(name);
            }
            if (o == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "Missing select data",
                        name,
                        "IEnumerable<SelectListItem>"));
            }
            IEnumerable<SelectListItem> selectList = o as IEnumerable<SelectListItem>;
            if (selectList == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "Wrong select data type",
                        name,
                        o.GetType().FullName,
                        "IEnumerable<SelectListItem>"));
            }
            return selectList;
        }

        //internal static string ListItemToOption(SelectListItem item)
        //{
        //    TagBuilder builder = new TagBuilder("option")
        //    {
        //        InnerHtml = HttpUtility.HtmlEncode(item.Text)
        //    };
        //    if (item.Value != null)
        //    {
        //        builder.Attributes["value"] = item.Value;
        //    }
        //    if (item.Selected)
        //    {
        //        builder.Attributes["selected"] = "selected";
        //    }
        //    return builder.ToString(TagRenderMode.Normal);
        //}

        private static object GetModelStateValue<TModel>(ViewDataDictionary<TModel> viewData, string key, Type destinationType)
        {
            ModelState modelState;
            if (viewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }

        private static void AddOptional(this IDictionary<string, object> dictionary, string key, bool value)
        {
            if (value)
            {
                dictionary[key] = key;
            }
        }

        private static void AddOptional(this IDictionary<string, object> dictionary, string key, object value)
        {
            if (value != null)
            {
                dictionary[key] = value;
            }
        }

        private static IDictionary<string, object> Attributes(string cssClass, string id, string style)
        {
            var htmlAttributes = new RouteValueDictionary();

            htmlAttributes.AddOptional("class", cssClass);
            htmlAttributes.AddOptional("id", id);
            htmlAttributes.AddOptional("style", style);

            return htmlAttributes;
        }

        private static IDictionary<string, object> SelectAttributes(string cssClass, string dir, bool disabled, string id, string lang, int? size, string style, int? tabIndex, string title)
        {
            var htmlAttributes = Attributes(cssClass, id, style);

            htmlAttributes.AddOptional("dir", dir);
            htmlAttributes.AddOptional("disabled", disabled);
            htmlAttributes.AddOptional("lang", lang);
            htmlAttributes.AddOptional("size", size);
            htmlAttributes.AddOptional("tabindex", tabIndex);
            htmlAttributes.AddOptional("title", title);

            return htmlAttributes;
        }
    }
}
