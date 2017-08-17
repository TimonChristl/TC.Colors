using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TC.Colors
{

    public class ColorPalette
    {

        private struct Stop : IEquatable<Stop>, IComparable<Stop>
        {
            public readonly float Position;
            public readonly RGB Color;

            public Stop(float position, RGB color)
            {
                Position = position;
                Color = color;
            }

            public override bool Equals(object obj)
            {
                if(obj is Stop)
                    return CompareCore(this, (Stop)obj) == 0;
                return false;
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }

            public override string ToString()
            {
                return $"Stop({Position}, {Color}";
            }

            public bool Equals(Stop other)
            {
                return CompareCore(this, other) == 0;
            }

            public int CompareTo(Stop other)
            {
                return CompareCore(this, other);
            }

            public static bool operator ==(Stop a, Stop b)
            {
                return CompareCore(a, b) == 0;
            }

            public static bool operator !=(Stop a, Stop b)
            {
                return CompareCore(a, b) != 0;
            }

            public static bool operator <(Stop a, Stop b)
            {
                return CompareCore(a, b) < 0;
            }

            public static bool operator >(Stop a, Stop b)
            {
                return CompareCore(a, b) > 0;
            }

            private static int CompareCore(Stop a, Stop b)
            {
                if(a.Position < b.Position)
                    return -1;
                if(a.Position > b.Position)
                    return +1;
                return 0;
            }
        }

        private List<Stop> stops = new List<Stop>();

        public ColorPalette()
        {
        }

        public void Add(float position, RGB color)
        {
            var newStop = new Stop(position, color);
            var index = stops.BinarySearch(newStop);
            if(index < 0)
                index = ~index;
            stops.Insert(index, newStop);
        }

        public void Add(float position, HSL color)
        {
            Add(position, color.ToRGB());
        }

        public void Add(float position, HSV color)
        {
            Add(position, color.ToRGB());
        }

        public RGB Get(float position)
        {
            if(stops.Count == 0)
                return new RGB();

            if(position >= stops[stops.Count - 1].Position)
                return stops[stops.Count - 1].Color;

            if(position <= stops[0].Position)
                return stops[0].Color;

            var searchStop = new Stop(position, new RGB());
            var index = stops.BinarySearch(searchStop);
            if(index < 0)
                index = ~index;

            var t = (position - stops[index - 1].Position) / (stops[index].Position - stops[index - 1].Position);

            return Lerp(stops[index - 1].Color, stops[index].Color, t);
        }

        private RGB Lerp(RGB color1, RGB color2, float t)
        {
            var oneMinusT = 1.0f - t;
            var r = (byte)(color1.R * oneMinusT + color2.R * t);
            var g = (byte)(color1.G * oneMinusT + color2.G * t);
            var b = (byte)(color1.B * oneMinusT + color2.B * t);

            return new RGB(r, g, b);
        }

    }

}
