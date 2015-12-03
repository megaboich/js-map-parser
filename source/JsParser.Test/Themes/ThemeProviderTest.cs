using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.UI.UI;
using NUnit.Framework;

namespace JsParser.Test.Themes
{
    [TestFixture]
    public class ThemeProviderTest
    {
        [Test]
        public void TestDefaults()
        {
            var tp = ThemeProvider.Deserialize("something wrong");
            var themes = tp.GetThemes();
            Assert.AreEqual(1, themes.Count());

            var defTheme = themes.First();
            Assert.AreEqual("Blue", defTheme.Name);
        }

        [Test]
        public void TestAddAndSerialize()
        {
            var tp = ThemeProvider.Deserialize("something wrong");
            tp.AddTheme("new");
            var themes = tp.GetThemes();
            Assert.AreEqual(2, themes.Count());

            var serialized = tp.Serialize();
            var ntp = ThemeProvider.Deserialize(serialized);

            Assert.AreEqual(2, ntp.GetThemes().Count());
            var serialized2 = ntp.Serialize();

            Assert.AreEqual(serialized, serialized2);
        }
    }
}
