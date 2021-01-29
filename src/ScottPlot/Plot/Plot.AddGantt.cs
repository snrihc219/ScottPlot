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
    }
}