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
        public GanttPlot AddGantt(double[] spans, double[] starts, int[] groupIndicator, 
            string[] seriesLabels, Color? color = null)
        {
            var plottable = new GanttPlot(spans, starts, groupIndicator, seriesLabels)
            {
                FillColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupLabels">组标签</param>
        /// <param name="seriesLabels">序列标签</param>
        /// <param name="spans">X长度</param>
        /// <param name="starts">X起始位置</param>
        /// <param name="yIndicator">Y的位置</param>
        /// <param name="colors">颜色</param>
        /// <returns></returns>
        public GanttPlot[] AddGantts( double[,] spans, double[,] starts, int[] yIndicator, 
            string[] groupLabels, string[,] seriesLabels, Color?[] colors = null)
        {
            if (groupLabels is null || seriesLabels is null 
                || spans is null || starts is null || yIndicator is null)
                throw new ArgumentException("labels, spans, starts and yIndicator cannot be null");

            if (spans.GetLength(0) != starts.GetLength(0) || spans.GetLength(1) != starts.GetLength(1))
                throw new ArgumentException("starts and spans must have identical size");

            if (seriesLabels.GetLength(0) != starts.GetLength(0) || seriesLabels.GetLength(1) != seriesLabels.GetLength(1))
                throw new ArgumentException("starts and seriesLabels must have identical size");

            if (yIndicator.Length != starts.Length)
                throw new ArgumentException("starts and yIndicator must have the same element number");

            if (starts.GetLength(0) != groupLabels.Length)
                throw new ArgumentException("groupLabels must have the same length as the width of starts");

            //每行一个组
            int groupNumber = starts.GetLength(0);
            //每组长度
            int groupLength = starts.GetLength(1);
            GanttPlot[] gantts = new GanttPlot[groupNumber];

            for (int i = 0; i < groupNumber; i++)
            {
                var currentYIndicator = new int[groupLength];
                Array.Copy(yIndicator, i * groupLength, currentYIndicator, 0, groupLength);

                var plottable = new GanttPlot(spans.SliceRow(i).ToArray(), 
                    starts.SliceRow(i).ToArray(), 
                    currentYIndicator, 
                    seriesLabels.SliceRow(i).ToArray())
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
            var plottable = new ComboGanttPlot(spans, starts, groupIndicator, groupLabels, seriesLabels);
            Add(plottable);
            
            return plottable;
        }
    }
}