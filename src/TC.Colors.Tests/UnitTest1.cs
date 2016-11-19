using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TC.Colors;

namespace TC.Colors.Tests
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void Verify_That_RGB_To_HSV_Roundtrips()
        {
            for(byte r = 0; r < 255; r++)
                for(byte g = 0; g < 255; g++)
                    for(byte b = 0; b < 255; b++)
                    {
                        var rgb = new RGB(r, g, b);
                        var hsv = rgb.ToHSV();
                        var rgb2 = hsv.ToRGB();

                        Assert.AreEqual(rgb, rgb2);

                        break;
                    }
        }
        [TestMethod]
        public void Verify_That_RGB_To_HSL_Roundtrips()
        {
            for(byte r = 0; r < 255; r++)
                for(byte g = 0; g < 255; g++)
                    for(byte b = 0; b < 255; b++)
                    {
                        var rgb = new RGB(r, g, b);
                        var hsl = rgb.ToHSL();
                        var rgb2 = hsl.ToRGB();

                        Assert.AreEqual(rgb, rgb2);

                        break;
                    }
        }
    }
}
