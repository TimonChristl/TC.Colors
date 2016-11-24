using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace TC.Colors
{

    /// <summary>
    /// Represents a color in the red/green/blue model.
    /// </summary>
    public struct RGB : IEquatable<RGB>
    {

        /// <summary>
        /// The red component of the color, from 0 to 255.
        /// </summary>
        public byte R;

        /// <summary>
        /// The green component of the color, from 0 to 255.
        /// </summary>
        public byte G;

        /// <summary>
        /// The blue component of the color, from 0 to 255.
        /// </summary>
        public byte B;

        /// <summary>
        /// Initializes the <see cref="RGB"/> struct with the given values for red, green and blue.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if(obj is RGB)
                return EqualsCore(this, (RGB)obj);

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return R << 16 | G << 8 | B << 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"RGB({R}, {G}, {B}";
        }

        /// <inheritdoc/>
        public bool Equals(RGB other)
        {
            return EqualsCore(this, other);
        }

        /// <summary>
        /// Returns <c>true</c> if the two colors are equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(RGB a, RGB b)
        {
            return EqualsCore(a, b);
        }

        /// <summary>
        /// Returns <c>true</c> if the two colors are not equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(RGB a, RGB b)
        {
            return !EqualsCore(a, b);
        }

        private static bool EqualsCore(RGB a, RGB b)
        {
            return a.R == b.R
                && a.G == b.G
                && a.B == b.B;
        }

        /// <summary>
        /// Converts this color to a <see cref="HSL"/> struct.
        /// </summary>
        /// <returns></returns>
        public HSL ToHSL()
        {
            var M = Max(R, Max(G, B));
            var m = Min(R, Min(G, B));
            var C = M - m;

            double H_prime;
            if(C == 0)
                H_prime = 0.0;
            else if(M == R)
                H_prime = ((G - B) / (double)C + 6) % 6;
            else if(M == G)
                H_prime = (B - R) / (double)C + 2;
            else if(M == B)
                H_prime = (R - G) / (double)C + 4;
            else
                H_prime = 0;

            var H = (ushort)(60 * H_prime + 0.5);

            var mPlusM = M + m;

            var L = (byte)(mPlusM / 2);

            var divisor = (255 - Abs(mPlusM - 255));
            var S_hsl = divisor == 0
                ? 0
                : C * 255 / divisor;

            return new HSL(H, (byte)S_hsl, L);
        }

        /// <summary>
        /// Converts this color to a <see cref="HSV"/> struct.
        /// </summary>
        /// <returns></returns>
        public HSV ToHSV()
        {
            var M = Max(R, Max(G, B));
            var m = Min(R, Min(G, B));
            var C = M - m;

            double H_prime;
            if(C == 0)
                H_prime = 0.0;
            else if(M == R)
                H_prime = ((G - B) / (double)C + 6) % 6;
            else if(M == G)
                H_prime = (B - R) / (double)C + 2;
            else if(M == B)
                H_prime = (R - G) / (double)C + 4;
            else
                H_prime = 0;

            var H = (ushort)(60 * H_prime + 0.5);

            var V = M;

            var S_hsv = V == 0
                ? (byte)0
                : (byte)(C * 255 / V);

            return new HSV(H, S_hsv, V);
        }

    }

    /// <summary>
    /// Represents a color in the hue/saturation/lightness model.
    /// </summary>
    public struct HSL : IEquatable<HSL>
    {

        /// <summary>
        /// The hue of the color, from 0 to 360.
        /// </summary>
        public ushort H;

        /// <summary>
        /// The saturation of the color, from 0 to 255.
        /// </summary>
        public byte S;

        /// <summary>
        /// The lightness of the color, from 0 to 255.
        /// </summary>
        public byte L;

        /// <summary>
        /// Initializes the <see cref="HSL"/> struct with the given values for hue, saturation and lightness.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <exception cref="ArgumentException">if the hue is greater than 360</exception>
        public HSL(ushort h, byte s, byte l)
        {
            if(h > 360)
                throw new ArgumentException("h must not be greater than 360");

            H = h;
            S = s;
            L = l;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if(obj is HSL)
                return EqualsCore(this, (HSL)obj);

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return H << 16 | S << 8 | L << 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"HSL({H}, {S}, {L}";
        }

        /// <inheritdoc/>
        public bool Equals(HSL other)
        {
            return EqualsCore(this, other);
        }

        /// <summary>
        /// Returns <c>true</c> if the two colors are equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(HSL a, HSL b)
        {
            return EqualsCore(a, b);
        }

        /// <summary>
        /// Returns <c>true</c> if the two colors are not equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(HSL a, HSL b)
        {
            return !EqualsCore(a, b);
        }

        private static bool EqualsCore(HSL a, HSL b)
        {
            return a.H == b.H
                && a.S == b.S
                && a.L == b.L;
        }

        /// <summary>
        /// Converts this color to a <see cref="RGB"/> struct.
        /// </summary>
        /// <returns></returns>
        public RGB ToRGB()
        {
            var C = (255 - Abs(2 * L - 255)) * S / 255;
            var H_prime = H / 60.0;
            var X = C * (1.0 - Abs(H_prime % 2 - 1.0));

            byte m = (byte)(L - C / 2);

            switch((int)H_prime)
            {
                case 0:
                    return new RGB((byte)(C + m), (byte)(X + m), m);
                case 1:
                    return new RGB((byte)(X + m), (byte)(C + m), m);
                case 2:
                    return new RGB(m, (byte)(C + m), (byte)(X + m));
                case 3:
                    return new RGB(m, (byte)(X + m), (byte)(C + m));
                case 4:
                    return new RGB((byte)(X + m), m, (byte)(C + m));
                case 5:
                    return new RGB((byte)(C + m), m, (byte)(X + m));
                default:
                    return new RGB(m, m, m);
            }
        }

    }

    /// <summary>
    /// Represents a color in the hue/saturation/value model.
    /// </summary>
    public struct HSV : IEquatable<HSV>
    {

        /// <summary>
        /// The hue of the color, from 0 to 360.
        /// </summary>
        public ushort H;

        /// <summary>
        /// The saturation of the color, from 0 to 255.
        /// </summary>
        public byte S;

        /// <summary>
        /// The value of the color, from 0 to 255.
        /// </summary>
        public byte V;

        /// <summary>
        /// Initializes the <see cref="HSV"/> struct with the given values for hue, saturation and value.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <exception cref="ArgumentException">if the hue is greater than 360</exception>
        public HSV(ushort h, byte s, byte v)
        {
            if(h > 360)
                throw new ArgumentException("h must not be greater than 360");

            H = h;
            S = s;
            V = v;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if(obj is HSV)
                return EqualsCore(this, (HSV)obj);

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return H << 16 | S << 8 | V << 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"HSV({H}, {S}, {V}";
        }

        /// <inheritdoc/>
        public bool Equals(HSV other)
        {
            return EqualsCore(this, other);
        }

        /// <summary>
        /// Returns <c>true</c> if the two colors are equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(HSV a, HSV b)
        {
            return EqualsCore(a, b);
        }

        /// <summary>
        /// Returns <c>true</c> if the two colors are not equal, <c>false</c> otherwise.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(HSV a, HSV b)
        {
            return !EqualsCore(a, b);
        }

        private static bool EqualsCore(HSV a, HSV b)
        {
            return a.H == b.H
                && a.S == b.S
                && a.V == b.V;
        }

        /// <summary>
        /// Converts this color to a <see cref="RGB"/> struct.
        /// </summary>
        /// <returns></returns>
        public RGB ToRGB()
        {
            var C = V * S / 255;
            var H_prime = H / 60.0;
            var X = C * (1.0 - Abs(H_prime % 2 - 1.0));

            byte m = (byte)(V - C);

            switch((int)H_prime)
            {
                case 0:
                    return new RGB((byte)(C + m), (byte)(X + m), m);
                case 1:
                    return new RGB((byte)(X + m), (byte)(C + m), m);
                case 2:
                    return new RGB(m, (byte)(C + m), (byte)(X + m));
                case 3:
                    return new RGB(m, (byte)(X + m), (byte)(C + m));
                case 4:
                    return new RGB((byte)(X + m), m, (byte)(C + m));
                case 5:
                    return new RGB((byte)(C + m), m, (byte)(X + m));
                default:
                    return new RGB(m, m, m);
            }
        }

    }

}
