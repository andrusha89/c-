//******************************************************************************
//
/*
Copyright (c) 2016 Appium Committers. All rights reserved.

Licensed to you under the Apache License, Version 2.0 (the
"License"); you may not use this file except in compliance
with the License.  You may obtain a copy of the License at
http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing,
software distributed under the License is distributed on an
"AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, either express or implied.  See the License for the
specific language governing permissions and limitations
under the License.
*/
//
//******************************************************************************

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace CalculatorTest
{
    [TestClass]
    public class BasicScenarios
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/wd/hub";
        private static RemoteWebDriver _calculatorSession;
        protected static RemoteWebElement CalculatorResult;
        protected static string OriginalCalculatorMode;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch the calculator app
            var appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            appCapabilities.SetCapability("platformName", "Windows");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            _calculatorSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(_calculatorSession);

            _calculatorSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            // Ensure we're in standard mode
            var menuButton = _calculatorSession.FindElementByXPath("//Button[starts-with(@Name, 'Menu')]");
            menuButton.Click();
            OriginalCalculatorMode = _calculatorSession.FindElementByXPath("//List[@AutomationId='FlyoutNav']//ListItem[@IsSelected='True']").Text;
            var standardModeButton = _calculatorSession.FindElementByXPath("//ListItem[@Name='Standard Calculator']");
            standardModeButton.Click();

            // Initialize calculator result element
            var clearButton = _calculatorSession.FindElementByXPath("//Button[@Name='Clear']");
            clearButton.Click();
            var sevenButton = _calculatorSession.FindElementByXPath("//Button[@Name='Seven']");
            sevenButton.Click();
            CalculatorResult = _calculatorSession.FindElementByName("Display is  7") as RemoteWebElement;
            Assert.IsNotNull(CalculatorResult);
        }

        [ClassCleanup]
        public static void TearDown()
        {
            // Restore original mode before closing down
            var menuButton = _calculatorSession.FindElementByXPath("//Button[starts-with(@Name, 'Menu')]");
            menuButton.Click();
            var originalModeButton = _calculatorSession.FindElementByXPath($"//ListItem[@Name='{OriginalCalculatorMode}']");
            originalModeButton.Click();

            CalculatorResult = null;
            _calculatorSession.Dispose();
            _calculatorSession = null;
        }

        [TestInitialize]
        public void Clear()
        {
            var clearButton = _calculatorSession.FindElementByName("Clear");
            clearButton.Click();
            Assert.AreEqual("Display is  0", CalculatorResult.Text.Trim());
        }

        [TestMethod]
        public void Addition()
        {
            var oneButton = _calculatorSession.FindElementByName("One");
            var plusButton = _calculatorSession.FindElementByName("Plus");
            var sevenButton = _calculatorSession.FindElementByName("Seven");
            var equalsButton = _calculatorSession.FindElementByName("Equals");

            oneButton.Click();
            plusButton.Click();
            sevenButton.Click();
            equalsButton.Click();

            Assert.AreEqual("Display is  8", CalculatorResult.Text.Trim());
        }

        [TestMethod]
        public void Combination()
        {
            var sevenButton = _calculatorSession.FindElementByXPath("//Button[@Name='Seven']");
            var multiplyButton = _calculatorSession.FindElementByXPath("//Button[@Name='Multiply by']");
            var nineButton = _calculatorSession.FindElementByXPath("//Button[@Name='Nine']");
            var plusButton = _calculatorSession.FindElementByXPath("//Button[@Name='Plus']");
            var oneButton = _calculatorSession.FindElementByXPath("//Button[@Name='One']");
            var divideButton = _calculatorSession.FindElementByXPath("//Button[@Name='Divide by']");
            var eightButton = _calculatorSession.FindElementByXPath("//Button[@Name='Eight']");
            var equalsButton = _calculatorSession.FindElementByXPath("//Button[@Name='Equals']");

            sevenButton.Click();
            multiplyButton.Click();
            nineButton.Click();
            plusButton.Click();
            oneButton.Click();
            equalsButton.Click();
            divideButton.Click();
            eightButton.Click();
            equalsButton.Click();

            Assert.AreEqual("Display is  8", CalculatorResult.Text.Trim());
        }

        [TestMethod]
        public void Division()
        {
            var eightButton = _calculatorSession.FindElementByName("Eight");
            var divideButton = _calculatorSession.FindElementByName("Divide by");
            var oneButton = _calculatorSession.FindElementByName("One");
            var equalsButton = _calculatorSession.FindElementByName("Equals");

            eightButton.Click();
            eightButton.Click(); // Click twice to enter 88
            divideButton.Click();
            oneButton.Click();
            oneButton.Click(); // Click twice to enter 11
            equalsButton.Click();

            Assert.AreEqual("Display is  8", CalculatorResult.Text.Trim());
        }

        [TestMethod]
        public void Multiplication()
        {
            var nineButton = _calculatorSession.FindElementByName("Nine");
            var multiplyButton = _calculatorSession.FindElementByName("Multiply by");
            var equalsButton = _calculatorSession.FindElementByName("Equals");

            nineButton.Click();
            multiplyButton.Click();
            nineButton.Click();
            equalsButton.Click();

            Assert.AreEqual("Display is  81", CalculatorResult.Text.Trim());
        }

        [TestMethod]
        public void Subtraction()
        {
            var nineButton = _calculatorSession.FindElementByName("Nine");
            var minusButton = _calculatorSession.FindElementByName("Minus");
            var oneButton = _calculatorSession.FindElementByName("One");
            var equalsButton = _calculatorSession.FindElementByName("Equals");

            nineButton.Click();
            minusButton.Click();
            oneButton.Click();
            equalsButton.Click();

            Assert.AreEqual("Display is  8", CalculatorResult.Text.Trim());
        }
    }
}
