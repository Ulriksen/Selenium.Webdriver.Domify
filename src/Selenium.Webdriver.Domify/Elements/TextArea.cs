using OpenQA.Selenium;
using Selenium.Webdriver.Domify.Attributes;
using Selenium.Webdriver.Domify.Core;

namespace Selenium.Webdriver.Domify.Elements
{
    [DOMElement("textarea")]
    public class TextArea : WebElement
    {
        public override string Text
        {
            get { return Value; }
            set { Value = value; }
        }

        public string Value
        {
            get { return this.GetAttribute("value"); }
            set
            {
                this.SetElementValue("value");
                base.Text = value;
                this.TriggerJavascriptChange();
            }
        }
    }
}