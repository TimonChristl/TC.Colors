using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace TC.Colors
{

    public struct RGB : IEquatable<RGB>
    {

        public byte R;
        public byte G;
        public byte B;

        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public override bool Equals(object obj)
        {
            if(obj is RGB)
                return EqualsCore(this, (RGB)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return R << 16 | G << 8 | B << 0;
        }

        public override string ToString()
        {
            return $"RGB({R}, {G}, {B}";
        }

        public bool Equals(RGB other)
        {
            return EqualsCore(this, other);
        }

        public static bool operator ==(RGB a, RGB b)
        {
            return EqualsCore(a, b);
        }

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

            var H = (ushort)(60 * H_prime);

            var mPlusM = M + m;

            var L = (byte)(mPlusM / 2);

            var divisor = (255 - Abs(mPlusM - 255));
            var S_hsl = divisor == 0
                ? 0
                : C * 255 / divisor;

            return new HSL(H, (byte)S_hsl, L);
        }

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

            var H = (ushort)(60 * H_prime);

            var V = M;

            var S_hsv = V == 0
                ? (byte)0
                : (byte)(C * 255 / V);

            return new HSV(H, S_hsv, V);
        }

    }

    public struct HSL : IEquatable<HSL>
    {

        public ushort H;
        public byte S;
        public byte L;

        public HSL(ushort h, byte s, byte l)
        {
            if(h > 360)
                throw new ArgumentException("h must not be greater than 360");

            H = h;
            S = s;
            L = l;
        }

        public override bool Equals(object obj)
        {
            if(obj is HSL)
                return EqualsCore(this, (HSL)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return H << 16 | S << 8 | L << 0;
        }

        public override string ToString()
        {
            return $"HSL({H}, {S}, {L}";
        }

        public bool Equals(HSL other)
        {
            return EqualsCore(this, other);
        }

        public static bool operator ==(HSL a, HSL b)
        {
            return EqualsCore(a, b);
        }

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

    public struct HSV : IEquatable<HSV>
    {

        public ushort H;
        public byte S;
        public byte V;

        public HSV(ushort h, byte s, byte v)
        {
            if(h > 360)
                throw new ArgumentException("h must not be greater than 360");

            H = h;
            S = s;
            V = v;
        }

        public override bool Equals(object obj)
        {
            if(obj is HSV)
                return EqualsCore(this, (HSV)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return H << 16 | S << 8 | V << 0;
        }

        public override string ToString()
        {
            return $"HSV({H}, {S}, {V}";
        }

        public bool Equals(HSV other)
        {
            return EqualsCore(this, other);
        }

        public static bool operator ==(HSV a, HSV b)
        {
            return EqualsCore(a, b);
        }

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
