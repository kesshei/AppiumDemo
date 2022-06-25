using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace AppiumDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = new Uri("http://127.0.0.1:4723/wd/hub");
            var capabilities = new AppiumOptions();
            capabilities.AddAdditionalCapability(MobileCapabilityType.AutomationName, AutomationName.AndroidUIAutomator2);
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "10");
            capabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, "2ff57aa0");

            var _driver = new AndroidDriver<AppiumWebElement>(url, capabilities, TimeSpan.FromSeconds(180));
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //启动快手APP
            _driver.StartActivity("com.smile.gifmaker", "com.yxcorp.gifshow.HomeActivity");
            //滑动两次
            var touchAction = new TouchAction(_driver);
            touchAction.Press(507, 1612).Wait(1000).MoveTo(576, 500).Release().Perform();
            Thread.Sleep(5 * 1000);

            var touchAction2 = new TouchAction(_driver);
            touchAction2.Press(434, 1622).Wait(1000).MoveTo(494, 564).Release().Perform();
            Thread.Sleep(5 * 1000);

            //录屏，目前没声音
            _driver.StartRecordingScreen(AndroidStartScreenRecordingOptions
            .GetAndroidStartScreenRecordingOptions()
            .WithTimeLimit(TimeSpan.FromSeconds(20)));
            var result = _driver.StopRecordingScreen();
            byte[] decode = Convert.FromBase64String(result);
            //录屏的MP4保存到本地
            var fileName = "VideoRecording_test.mp4";
            File.WriteAllBytes(fileName, decode);

            //截屏
            var Screen = _driver.GetScreenshot();
            using (MemoryStream memoryStream = new MemoryStream(Screen.AsByteArray))
            {
                using var img = Image.FromStream(memoryStream);
                img.Save("123.jpg", ImageFormat.Jpeg);
            }
            //结束app
            _driver.TerminateApp("com.smile.gifmaker");
            _driver?.Quit();
            Console.ReadLine();
        }
    }
}
