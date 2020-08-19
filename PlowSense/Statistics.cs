﻿using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using PlowSense.Models;

namespace PlowSense
{
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
        }
		BrushConverter converter = new System.Windows.Media.BrushConverter();
		Func<ChartPoint, string> labelPoint = chartpoint => string.Format("{1:P}", chartpoint.Y, chartpoint.Participation);
		private void Statistics_Load(object sender, EventArgs e)
		{
			PieLoad();
<<<<<<< HEAD
			CropsLoad();
=======
			LineLoad();
>>>>>>> DashboardDesign
		}
		
		void PieLoad()
		{
			statsPie.Series= new SeriesCollection
			{
				new PieSeries
				{
					Title = "Title 1",
					Values = new ChartValues<double> {100},
					Fill = (System.Windows.Media.Brush)converter.ConvertFromString("#9CE74C"),
					DataLabels = true,
					LabelPoint = labelPoint,
					StrokeThickness = 0,
				},
				new PieSeries
				{
					Title = "Title 2",
					Values = new ChartValues<double> {300},
					Fill = (System.Windows.Media.Brush)converter.ConvertFromString("#DECD05"),
					DataLabels = true,
					LabelPoint = labelPoint,
					StrokeThickness = 0
				},
				new PieSeries
				{
					Title = "Title 3",
					Values = new ChartValues<double> {250},
					Fill = (System.Windows.Media.Brush)converter.ConvertFromString("#BBB324"),
					DataLabels = true,
					LabelPoint = labelPoint,
					StrokeThickness = 0
				},
			};
			DefaultLegend customLegend = new DefaultLegend();
			customLegend.BulletSize = 10;
			customLegend.Foreground = System.Windows.Media.Brushes.White;
			customLegend.FontSize = 15;
			statsPie.DefaultLegend = customLegend;
			statsPie.LegendLocation = LegendLocation.Right;
		}

		void CropsLoad()
		{
			List<string> availableCrops = new List<string>();
			foreach (var farm in MainForm.Transactions.Values)
			{
				if (!availableCrops.Contains(farm.Crop))
				{
					availableCrops.Add(farm.Crop);
					statsCmbBox.Items.Add(farm.Crop);
				}
			}
		}

		void GenerateChartData(Dictionary<DateTime, TransactionHistory> values)
		{
			//Create filtered dictionary
			
			statsChart.Series.Clear();
			statsChart.AxisX.Clear();

			//Add new values and labels
			statsChart.Series = new SeriesCollection
			{
				new LineSeries
				{
					Title = statsCmbBox.SelectedItem.ToString(),
					Values = new ChartValues<int>(values.Values.Select(o => o.AmountSold)),
					Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(80, 9, 105, 54)),
					PointGeometry = DefaultGeometries.Square,
					LineSmoothness = 100,
					Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(9, 105, 54))
				},
			};

			statsChart.AxisX.Add(new Axis
			{
				Title = "Month",
				Labels = values.Keys.Select(a => a.ToString("MM/dd/yyyy")).ToList()
			});
		}

		void GenerateTableData(Dictionary<DateTime, TransactionHistory> values)
		{
			foreach (var item in values)
			{
				statsDataGrid.Rows.Add(item.Key.ToString("MM/dd/yyyy"),
					item.Value.FarmRep, item.Value.Crop, item.Value.AmountSold);
			}
		}
		private void statsCmbBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Dictionary<DateTime, TransactionHistory> filteredValues = MainForm.Transactions
				.Where(o => o.Value.Crop == statsCmbBox.SelectedItem.ToString())
				.ToDictionary(x => x.Key, x => x.Value);

			GenerateChartData(filteredValues);
			GenerateTableData(filteredValues);
		}
	}
}
