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
        public GanttPlot AddGantt(double[] spans, double[] starts, string[] seriesLabels = null, Color? color = null)
        {
            var plottable = new GanttPlot(spans, starts, seriesLabels)
            {
                FillColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        public GanttPlot[] AddGantts(string[] groupLabels, string[] seriesLabels, double[,] spans, double[,] starts, Color?[] colors = null)
        {
            if (groupLabels is null || seriesLabels is null || spans is null || starts is null)
                throw new ArgumentException("labels, spans and starts cannot be null");

            if (spans.GetLength(0) != starts.GetLength(0) || spans.GetLength(1) != starts.GetLength(1))
                throw new ArgumentException("starts and spans must have identical size");

            if (seriesLabels.Length != starts.GetLength(1))
                throw new ArgumentException("groupLabels and starts must be the same length");

            if (starts.GetLength(0) != groupLabels.Length)
                throw new ArgumentException("all arrays inside starts must be the same length as groupLabels");

            int groupSeries = starts.GetLength(0);
            GanttPlot[] gantts = new GanttPlot[groupSeries];

            for (int i = 0; i < groupSeries; i++)
            {
                var plottable = new GanttPlot(spans.SliceRow(i).ToArray(), starts.SliceRow(i).ToArray(), seriesLabels)
                {
                    Label = groupLabels[i],
                    FillColor = colors == null ? GetNextColor() : colors[i] ?? GetNextColor()
                };
                Add(plottable);
            }

            return gantts;
        }

        public ComboGanttPlot AddComboGantt(string[] groupLabels, string[] seriesLabels, 
            double[,] spans, double[,] starts, int[] groupIndicator, Color?[] colors = null)
        {
            var plottable = new ComboGanttPlot(spans, starts, groupIndicator);
            Add(plottable);
            return plottable;
        }
    }
}