using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    public class ColorGenerator
    {

        private int index = 0;
        private IntensityGenerator intensityGenerator = new IntensityGenerator();

        static string[] ColorValues = new string[] {
        "FF0000", "00FF00", "0000FF", "FFFF00", "FF00FF", "00FFFF", "000000",
        "800000", "008000", "000080", "808000", "800080", "008080", "808080",
        "C00000", "00C000", "0000C0", "C0C000", "C000C0", "00C0C0", "C0C0C0",
        "400000", "004000", "000040", "404000", "400040", "004040", "404040",
        "200000", "002000", "000020", "202000", "200020", "002020", "202020",
        "600000", "006000", "000060", "606000", "600060", "006060", "606060",
        "A00000", "00A000", "0000A0", "A0A000", "A000A0", "00A0A0", "A0A0A0",
        "E00000", "00E000", "0000E0", "E0E000", "E000E0", "00E0E0", "E0E0E0",
        };

        public Color NextColor()
        {
            string color = string.Format(PatternGenerator.NextPattern(index),
                intensityGenerator.NextIntensity(index));
            index++;
            return Color.FromArgb(0xFF, 
                Color.FromArgb(Int32.Parse(color, System.Globalization.NumberStyles.HexNumber)));
        }
    }

    public class PatternGenerator
    {
        public static string NextPattern(int index)
        {
            switch (index % 7)
            {
                case 0: return "{0}0000";
                case 1: return "00{0}00";
                case 2: return "0000{0}";
                case 3: return "{0}{0}00";
                case 4: return "{0}00{0}";
                case 5: return "00{0}{0}";
                case 6: return "{0}{0}{0}";
                default: throw new Exception("Math error");
            }
        }
    }

    public class IntensityGenerator
    {
        private IntensityValueWalker walker;
        private int current;

        public string NextIntensity(int index)
        {
            if (index == 0)
            {
                current = 255;
            }
            else if (index % 7 == 0)
            {
                if (walker == null)
                {
                    walker = new IntensityValueWalker();
                }
                else
                {
                    walker.MoveNext();
                }
                current = walker.Current.Value;
            }
            string currentText = current.ToString("X");
            if (currentText.Length == 1) currentText = "0" + currentText;
            return currentText;
        }
    }

    public class IntensityValue
    {

        private IntensityValue mChildA;
        private IntensityValue mChildB;

        public IntensityValue(IntensityValue parent, int value, int level)
        {
            if (level > 7) throw new Exception("There are no more Colors left");
            Value = value;
            Parent = parent;
            Level = level;
        }

        public int Level { get; set; }
        public int Value { get; set; }
        public IntensityValue Parent { get; set; }

        public IntensityValue ChildA
        {
            get
            {
                return mChildA ?? (mChildA = new IntensityValue(this, this.Value - (1 << (7 - Level)), Level + 1));
            }
        }

        public IntensityValue ChildB
        {
            get
            {
                return mChildB ?? (mChildB = new IntensityValue(this, Value + (1 << (7 - Level)), Level + 1));
            }
        }
    }

    public class IntensityValueWalker
    {

        public IntensityValueWalker()
        {
            Current = new IntensityValue(null, 1 << 7, 1);
        }

        public IntensityValue Current { get; set; }

        public void MoveNext()
        {
            if (Current.Parent == null)
            {
                Current = Current.ChildA;
            }
            else if (Current.Parent.ChildA == Current)
            {
                Current = Current.Parent.ChildB;
            }
            else
            {
                int levelsUp = 1;
                Current = Current.Parent;
                while (Current.Parent != null && Current == Current.Parent.ChildB)
                {
                    Current = Current.Parent;
                    levelsUp++;
                }
                if (Current.Parent != null)
                {
                    Current = Current.Parent.ChildB;
                }
                else
                {
                    levelsUp++;
                }
                for (int i = 0; i < levelsUp; i++)
                {
                    Current = Current.ChildA;
                }

            }
        }
    }
}
