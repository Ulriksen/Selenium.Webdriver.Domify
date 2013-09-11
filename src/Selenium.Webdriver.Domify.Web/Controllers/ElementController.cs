﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Selenium.Webdriver.Domify.Core;
using Selenium.Webdriver.Domify.Elements;

namespace Selenium.Webdriver.Domify.Web.Controllers
{
    public class ElementController : Controller
    {
        public ActionResult Element(string tag, string id, string type="", string text = "")
        {
            var elements = GetElements().SelectMany(GetAttributes<DOMElementAttribute>).Where(t => t.Tag.Equals(tag, StringComparison.InvariantCultureIgnoreCase));
            if(!string.IsNullOrEmpty(type))
            {
                elements = elements.Where(e => !string.IsNullOrEmpty(e.Type)).Where(e => e.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase));
            }
            
            var htmlElement = CreateHtmlElementModel(elements.FirstOrDefault(), text);
            htmlElement.AddAttribute("id", id);
            return View("AllElements", new List<HtmlElementModel> (){ htmlElement} );
        }


        public ActionResult AllElements()
        {
            var elements = GetElements().SelectMany(GetAttributes<DOMElementAttribute>).Where(t => t.Tag != "*")
                .Select(CreateHtmlElementModel).ToList();


            return View(elements);
        }
        private HtmlElementModel CreateHtmlElementModel(DOMElementAttribute att)
        {
            return CreateHtmlElementModel(att, "");
        }
        private HtmlElementModel CreateHtmlElementModel(DOMElementAttribute att, string text)
        {

            List<AttributeViewModel> attributes = new List<AttributeViewModel>();
            if (!string.IsNullOrEmpty(att.Type))
            {
                attributes.Add(new AttributeViewModel("type", att.Type));
            }
            if (NonSelfClosingTags.Contains(att.Tag.ToLower()))
            {
                text = string.IsNullOrEmpty(text) ? "This is a " + att.Tag : text;
            }
            return new HtmlElementModel(att.Tag, attributes.ToArray(), text);
        }

        private IEnumerable<T> GetAttributes<T>(Type element)
        {
            return element.GetCustomAttributes(typeof(T), false).Cast<T>();
        }
        public IEnumerable<Type> GetElements()
        {
            foreach (var types in typeof(H1).Module.GetTypes().Where(t => t.BaseType == typeof(WebElement)))
            {
                yield return types;
            }
        }

        public string[] NonSelfClosingTags
        {
            get { return ReadFileWithTags().ToArray(); }
        }

        private IEnumerable<string> ReadFileWithTags()
        {
            using (var sr = new StreamReader(Server.MapPath("~/app_data/tags.txt")))
            {
                while(!sr.EndOfStream)
                yield return sr.ReadLine();
            }
        }
    }

    public class HtmlElementModel
    {
        public string TagName { get; set; }
        public List<AttributeViewModel> Attributes { get; private set; }
        public string Text { get; set; }
        public void AddAttribute(string name, string value)
        {
            Attributes.Add(new AttributeViewModel(name, value));
        }
        public HtmlElementModel(string tagName, AttributeViewModel[] attributes, string text = null)
        {
            TagName = tagName;
            Attributes = new List<AttributeViewModel>(attributes);
            Text = text;
        }
    }

    public class AttributeViewModel
    {
        public AttributeViewModel()
        {

        }
        public AttributeViewModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}