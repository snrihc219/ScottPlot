﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ScottPlot;
using ScottPlot.Cookbook;
using ScottPlot.Drawing;

namespace WpfGantt
{
    public class Gantt : IRecipe
    {
        public string Category => "Gantt";
        public string ID => "gantt";
        public string Title => "Gantt Graph";
        public string Description => "Gantt Graph";
        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] spans = { 26, 20,  };
            double[] starts = { 0, 14,  };

            double[] spans2 = { 23, 7, 16 };
            double[] starts2 = { 50, 60, 55 };

            double[][] spanss = { new double[]{ 26, 20, 15 }, new double[]{ 23, 7, 16 } };
            double[][] startss = { new double[] { 0, 14, 20 }, new double[] { 50, 60, 55 } };

            // add a gantt graph to the plot
            //plt.AddGantt(spans, starts);
            //plt.AddGantt(spans2, starts2);

            plt.AddGantts(new string[]{ "g1", "g2", "g3" }, new string[]{ "s1", "s2" }, spanss, startss);

            // adjust axis limits so there is no padding below the bar graph
            //plt.SetAxisLimits(yMin: 0);
        }
    }
}
