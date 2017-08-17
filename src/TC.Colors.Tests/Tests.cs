#define HIGHLIGHT_ROUNDTRIP_DIFFERENCES

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TC.Colors;
using static System.Math;

namespace TC.Colors.Tests
{

    [TestClass]
    public class ColorTests
    {

        [StructLayout(LayoutKind.Explicit, Size = 3)]
        private struct BitmapRGB
        {
            [FieldOffset(0)]
            public byte B;
            [FieldOffset(1)]
            public byte G;
            [FieldOffset(2)]
            public byte R;

            public BitmapRGB(RGB other)
            {
                R = other.R;
                G = other.G;
                B = other.B;
            }
        }

        [TestMethod]
        public void GenerateHSVCharts()
        {
            var pixelFormat = PixelFormat.Format24bppRgb;

            Directory.CreateDirectory("hsv");

            for(var v = 0; v < 256; v++)
            {
                using(var bitmap = new Bitmap(360, 256, pixelFormat))
                {
                    var bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, pixelFormat);
                    try
                    {
                        unsafe
                        {
                            void* pData = bits.Scan0.ToPointer();
                            byte* pRow = (byte*)pData;

                            for(var s = 0; s < 256; s++)
                            {
                                BitmapRGB* pCol = (BitmapRGB*)pRow;

                                for(var h = 0; h < 360; h++)
                                {
                                    var hsv = new HSV((ushort)h, (byte)s, (byte)v);
                                    var rgb = hsv.ToRGB();

                                    pCol->R = rgb.R;
                                    pCol->G = rgb.G;
                                    pCol->B = rgb.B;

                                    pCol++;
                                }

                                pRow += bits.Stride;
                            }
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(bits);
                    }

                    bitmap.Save($@"hsv\{v}.bmp", ImageFormat.Bmp);
                }
            }
        }

        [TestMethod]
        public void GenerateHSLCharts()
        {
            var pixelFormat = PixelFormat.Format24bppRgb;

            Directory.CreateDirectory("hsl");

            for(var l = 0; l < 256; l++)
            {
                using(var bitmap = new Bitmap(360, 256, pixelFormat))
                {
                    var bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, pixelFormat);
                    try
                    {
                        unsafe
                        {
                            void* pData = bits.Scan0.ToPointer();
                            byte* pRow = (byte*)pData;

                            for(var s = 0; s < 256; s++)
                            {
                                BitmapRGB* pCol = (BitmapRGB*)pRow;

                                for(var h = 0; h < 360; h++)
                                {
                                    var hsl = new HSL((ushort)h, (byte)s, (byte)l);
                                    var rgb = hsl.ToRGB();

                                    pCol->R = rgb.R;
                                    pCol->G = rgb.G;
                                    pCol->B = rgb.B;

                                    pCol++;
                                }

                                pRow += bits.Stride;
                            }
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(bits);
                    }

                    bitmap.Save($@"hsl\{l}.bmp", ImageFormat.Bmp);
                }
            }
        }

        [TestMethod]
        public void GenerateHSVRoundtripCharts()
        {
            var pixelFormat = PixelFormat.Format24bppRgb;

            Directory.CreateDirectory("rgb_to_hsv");

            for(var b = 0; b < 256; b++)
            {
                using(var bitmap = new Bitmap(256, 256, pixelFormat))
                {
                    var bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, pixelFormat);
                    try
                    {
                        unsafe
                        {
                            void* pData = bits.Scan0.ToPointer();
                            byte* pRow = (byte*)pData;

                            for(var g = 0; g < 256; g++)
                            {
                                BitmapRGB* pCol = (BitmapRGB*)pRow;

                                for(var r = 0; r < 256; r++)
                                {
                                    if(r == 255 && g == 4 && b == 0)
                                        Debugger.Break();

                                    var rgb1 = new RGB((byte)r, (byte)g, (byte)0);
                                    var hsv = rgb1.ToHSV();
                                    var rgb2 = hsv.ToRGB();

                                    var rgb = new RGB();

#if HIGHLIGHT_ROUNDTRIP_DIFFERENCES
                                    rgb.R = (byte)(Abs(rgb1.R - rgb2.R));
                                    rgb.G = (byte)(Abs(rgb1.G - rgb2.G));
                                    rgb.B = (byte)(Abs(rgb1.B - rgb2.B));
#else
                                    rgb.R = (byte)Math.Abs(rgb1.R - rgb2.R);
                                    rgb.G = (byte)Math.Abs(rgb1.G - rgb2.G);
                                    rgb.B = (byte)Math.Abs(rgb1.B - rgb2.B);
#endif

                                    pCol->R = rgb.R;
                                    pCol->G = rgb.G;
                                    pCol->B = rgb.B;

                                    pCol++;
                                }

                                pRow += bits.Stride;
                            }
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(bits);
                    }

                    bitmap.Save($@"rgb_to_hsv\{b}.bmp", ImageFormat.Bmp);
                }
            }
        }

        [TestMethod]
        public void GenerateHSLRoundtripCharts()
        {
            var pixelFormat = PixelFormat.Format24bppRgb;

            Directory.CreateDirectory("rgb_to_hsl");

            for(var b = 0; b < 256; b++)
            {
                using(var bitmap = new Bitmap(256, 256, pixelFormat))
                {
                    var bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, pixelFormat);
                    try
                    {
                        unsafe
                        {
                            void* pData = bits.Scan0.ToPointer();
                            byte* pRow = (byte*)pData;

                            for(var g = 0; g < 256; g++)
                            {
                                BitmapRGB* pCol = (BitmapRGB*)pRow;

                                for(var r = 0; r < 256; r++)
                                {
                                    var rgb1 = new RGB((byte)r, (byte)g, (byte)b);
                                    var hsl = rgb1.ToHSL();
                                    var rgb2 = hsl.ToRGB();

                                    var rgb = new RGB();

#if HIGHLIGHT_ROUNDTRIP_DIFFERENCES
                                    rgb.R = (byte)(Abs(rgb1.R - rgb2.R));
                                    rgb.G = (byte)(Abs(rgb1.G - rgb2.G));
                                    rgb.B = (byte)(Abs(rgb1.B - rgb2.B));
#else
                                    rgb.R = (byte)Math.Abs(rgb1.R - rgb2.R);
                                    rgb.G = (byte)Math.Abs(rgb1.G - rgb2.G);
                                    rgb.B = (byte)Math.Abs(rgb1.B - rgb2.B);
#endif

                                    pCol->R = rgb.R;
                                    pCol->G = rgb.G;
                                    pCol->B = rgb.B;

                                    pCol++;
                                }

                                pRow += bits.Stride;
                            }
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(bits);
                    }

                    bitmap.Save($@"rgb_to_hsl\{b}.bmp", ImageFormat.Bmp);
                }
            }
        }

        [TestMethod]
        public void GenerateRGBReferenceCharts()
        {
            var pixelFormat = PixelFormat.Format24bppRgb;

            Directory.CreateDirectory("rgb");

            for(var b = 0; b < 256; b++)
            {
                using(var bitmap = new Bitmap(256, 256, pixelFormat))
                {
                    var bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, pixelFormat);
                    try
                    {
                        unsafe
                        {
                            void* pData = bits.Scan0.ToPointer();
                            byte* pRow = (byte*)pData;

                            for(var g = 0; g < 256; g++)
                            {
                                BitmapRGB* pCol = (BitmapRGB*)pRow;

                                for(var r = 0; r < 256; r++)
                                {
                                    pCol->R = (byte)r;
                                    pCol->G = (byte)g;
                                    pCol->B = (byte)b;

                                    pCol++;
                                }

                                pRow += bits.Stride;
                            }
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(bits);
                    }

                    bitmap.Save($@"rgb\{b}.bmp", ImageFormat.Bmp);
                }
            }
        }

        [TestMethod]
        public void PaletteTest1()
        {
            var palette = new ColorPalette();
            palette.Add(0.0f, new RGB(255, 0, 0));
            palette.Add(1.0f, new RGB(0, 255, 0));

            // Before first stop
            Assert.AreEqual(new RGB(255, 0, 0), palette.Get(-1.0f));

            // At first stop
            Assert.AreEqual(new RGB(255, 0, 0), palette.Get(0.0f));

            // 25% between the two stops
            Assert.AreEqual(new RGB(191, 63, 0), palette.Get(0.25f));

            // 50% between the two stops
            Assert.AreEqual(new RGB(127, 127, 0), palette.Get(0.5f));

            // 75% between the two stops
            Assert.AreEqual(new RGB(63, 191, 0), palette.Get(0.75f));

            // At last stop
            Assert.AreEqual(new RGB(0, 255, 0), palette.Get(1.0f));

            // After last stop
            Assert.AreEqual(new RGB(0, 255, 0), palette.Get(2.0f));
        }

    }

}
