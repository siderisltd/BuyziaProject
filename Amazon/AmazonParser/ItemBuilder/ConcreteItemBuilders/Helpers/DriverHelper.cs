namespace AmazonParser.ItemBuilder.ConcreteItemBuilders.Helpers
{
    using Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using SeleniumExtensions;

    internal sealed class DriverHelper
    {
        private DriverHelper() { }

        private static volatile DriverHelper instance;

        private static object syncLock = new object();

        public static DriverHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        // Always make double check because some one can be just entered in the
                        // first conditional statement
                        if (instance == null)
                        {
                            instance = new DriverHelper();
                        }
                    }
                }
                return instance;
            }
        }

        public bool CheckIfElementExists(ICustomWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
