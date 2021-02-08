using ScottPlot.Plottable;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ScottPlot
{
    public partial class Plot
    {
        /// <summary>
        /// Add a bar plot (values +/- errors) using defined positions
        /// </summary>
        public GanttPlot AddGantt(double[] spans, double[] starts, Color? color = null)
        {
            var plottable = new GanttPlot(spans, starts)
            {
                FillColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        public GanttPlot[] AddGantts(string[] groupLabels, string[] seriesLabels, double[][] spans, double[][] starts, Color?[] colors = null)
        {
            if (groupLabels is null || seriesLabels is null || spans is null || starts is null)
                throw new ArgumentException("labels, spans and starts cannot be null");

            if (spans.GetLength(0) != starts.GetLength(0))
                throw new ArgumentException("starts and spans must have identical size");

            if (seriesLabels.Length != starts.Length)
                throw new ArgumentException("groupLabels and starts must be the same length");

            foreach (double[] subArray in starts)
                if (subArray.Length != groupLabels.Length)
                    throw new ArgumentException("all arrays inside starts must be the same length as groupLabels");

            int seriesCount = starts.Length;
            GanttPlot[] gantts = new GanttPlot[seriesCount];

            for (int i = 0; i < seriesCount; i++)
            {
                var plottable = new GanttPlot(spans[i], starts[i])
                {
                    Label = seriesLabels[i],
                    FillColor = colors == null ? GetNextColor() : colors[i] ?? GetNextColor()
                };
                Add(plottable);
            }

            return gantts;
        }
    }
}