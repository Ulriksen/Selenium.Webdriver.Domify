using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Selenium.Webdriver.Domify.Elements
{
    public class UL : WebElement
    {
        public ListItem this[int index]{
            get { return ListItems.ToList()[index]; }}

        public static UL Create(IWebElement element)
        {
            return new UL(element);
        }

        public IList<ListItem>  OwnListItems
        {
            get { return ListItems.ToList(); }
        }

        private IEnumerable<ListItem> ListItems
        {
            get { return FindElements(By.TagName("li")).Select(ListItem.Create); }
        }

        private UL(IWebElement element) :
            base(element)
        {

        }
    }
}