using ScottPlot;
using ScottPlot.Drawing;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Gantt plots display a gantt chart. 
    /// Starts are defined by Ys.
    /// SPans are defined by Xs (relative to BaseValue and YOffsets).
    /// </summary>
    public class GanttPlot : IPlottable
    {
        // data
        public double[] Spans;
        public double Offset;
        public double[] Ys;
        public double[] Starts;
        public string[] SeriesLabels;
        public int[] YIndicator;

        // customization
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public string Label;
        public Color FillColor = Color.Green;
        public Color FillColorNegative = Color.Red;
        public Color FillColorHatch = Color.Blue;
        public HatchStyle HatchStyle = HatchStyle.None;
        public Color BorderColor = Color.Black;
        public float BorderLineWidth = 1;

        public readonly ScottPlot.Drawing.Font Font = new ScottPlot.Drawing.Font();
        public string FontName { set => Font.Name = value; }
        public float FontSize { set => Font.Size = value; }
        public bool FontBold { set => Font.Bold = value; }
        public Color FontColor { set => Font.Color = value; }

        public double BarWidth = .5;
        public double BaseValue = 0;
        public bool ShowValuesAboveBars;

        public GanttPlot(double[] spans, double[] starts, int[] yIndicator, 
            string[] seriesLabels)
        {
            if (spans is null || spans.Length == 0)
                throw new InvalidOperationException("spans must be an array that contains elements");

            var groups = yIndicator.Max() + 1;
            Ys = DataGen.Consecutive(groups);
            Spans = spans;
            Starts = starts;
            SeriesLabels = seriesLabels;
            YIndicator = yIndicator;
        }

        public AxisLimits GetAxisLimits()
        {
            double valueMin = double.PositiveInfinity;
            double valueMax = double.NegativeInfinity;
            double positionMin = double.PositiveInfinity;
            double positionMax = double.NegativeInfinity;

            for (int i = 0; i < Spans.Length; i++)
            {
                valueMin = Math.Min(valueMin, Spans[i] + Starts[i]);
                valueMax = Math.Max(valueMax, Spans[i] + Starts[i]);
            }
            positionMin = Math.Min(positionMin, Ys.Min());
            positionMax = Math.Max(positionMax, Ys.Max()) + BarWidth / 2;

            valueMin = Math.Min(valueMin, BaseValue);
            valueMax = Math.Max(valueMax, BaseValue);

            if (ShowValuesAboveBars)
                valueMax += (valueMax - valueMin) * .1; // increase by 10% to accomodate label

            positionMin -= BarWidth / 2;
            positionMax += BarWidth / 2;

            positionMin += Offset;
            positionMax += Offset;

            return new AxisLimits(valueMin, valueMax, positionMin, positionMax);
        }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements("spans", Spans);
            Validate.AssertHasElements("ys", Ys);
            Validate.AssertHasElements("starts", Starts);
            Validate.AssertEqualLength("spans and starts", Spans, Starts);

            if (deep)
            {
                Validate.AssertAllReal("spans", Spans);
                Validate.AssertAllReal("ys", Ys);
                Validate.AssertAllReal("starts", Starts);
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            for (int barIndex = 0; barIndex < Spans.Length; barIndex++)
            {
                RenderBarHorizontal(dims, gfx, 
                    Spans[barIndex] + Offset, Starts[barIndex], 
                    Ys[YIndicator[barIndex]], SeriesLabels[barIndex]);
            }
        }

        private void RenderBarHorizontal(PlotDimensions dims, Graphics gfx, double value, 
            double xOffset, double position, string label)
        {
            // bar body
            float centerPx = dims.GetPixelY(position);
            double edge2 = position + BarWidth / 2;
            double value1 = Math.Min(BaseValue, value) + xOffset;
            double value2 = Math.Max(BaseValue, value) + xOffset;
            double valueSpan = value2 - value1;
            var rect = new RectangleF(
                x: dims.GetPixelX(value1),
                y: dims.GetPixelY(edge2),
                height: (float)(BarWidth * dims.PxPerUnitY),
                width: (float)(valueSpan * dims.PxPerUnitX));

            using (var fillBrush = GDI.Brush((value < 0) ? FillColorNegative : FillColor, FillColorHatch, HatchStyle))
                gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

            if (BorderLineWidth > 0)
                using (var outlinePen = new Pen(BorderColor, BorderLineWidth))
                    gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

            using var valueTextFont = GDI.Font(Font);
            using var valueTextBrush = GDI.Brush(Font.Color);
            using var sf = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
            gfx.DrawString(label, valueTextFont, valueTextBrush, rect.X + rect.Width / 2, rect.Y - rect.Height / 2, sf);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableBar{label} with {PointCount} points";
        }

        public int PointCount { get => Ys is null ? 0 : Ys.Length; }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
            {
                label = Label,
                color = FillColor,
                lineWidth = 10,
                markerShape = MarkerShape.none,
                hatchColor = FillColorHatch,
                hatchStyle = HatchStyle,
                borderColor = BorderColor,
                borderWith = BorderLineWidth
            };
            return new LegendItem[] { singleItem };
        }
    }
}
