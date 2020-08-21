﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Ganss.Excel;
using PlowSense.Models;


namespace PlowSense
{
	public partial class Farms : Form
	{
		private List<FarmInfo> _farms;
		public Farms()
		{
			InitializeComponent();
		}
		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		private static extern IntPtr CreateRoundRectRgn
		(
			int nLeftRect,
			int nTopRect,
			int nRightRect,
			int nBottomRect,
			int nWidthEllipse,
			int nHeightEllipse
		);
		private void Farms_Load(object sender, EventArgs e)
		{
			FarmLoad();
			LineLoad();
		}
		int _tag;

		#region ExcelHelpers
		//should be only run once
		private void GetExcelData()
		{
			ExcelMapper excelFile = new ExcelMapper(@"D:\PlowSenseFiles\Farms.xlsx");
			_farms = excelFile.Fetch<FarmInfo>().ToList();
			List<CropInfo> crops = excelFile.Fetch<CropInfo>(1).ToList();
			foreach (FarmInfo farm in _farms)
			{
				farm.Crops = crops.Where(c => c.Farm == farm.Farm).ToList();
			}
		}

		private void SaveDataToExcel()
		{
			List<CropInfo> crops = _farms.SelectMany(f => f.Crops).ToList();
			if (!File.Exists(@"E:\PlowSenseFiles\Farms.xlsx"))
			{
				File.Create(@"E:\PlowSenseFiles\Farms.xlsx").Dispose();
			}
			ExcelMapper excelFile = new ExcelMapper();
			excelFile.Save(@"E:\PlowSenseFiles\Farms.xlsx", _farms);
			excelFile.Save(@"E:\PlowSenseFiles\Farms.xlsx", crops, 1);
		}
		#endregion

		void FarmLoad()
		{
			string directory = Directory.Exists(@"D:\") ? @"D:\PlowSenseFiles" : @"C:\PlowSenseFiles";
			Directory.CreateDirectory(directory);
			string path = Path.Combine(directory, "FarmsCSV.csv");

			if (!File.Exists(path)) File.Create(path);
			//Extract data
			StreamReader csvReader = new StreamReader(path);
			while (csvReader.Peek() != -1)
			{
				string entryString = csvReader.ReadLine();
				string[] entry = entryString.Split(',');
				int cTag = 0;
				Panel p = new Panel
				{
					Tag = _tag,
					BackColor = System.Drawing.Color.FromArgb(9, 105, 54),
					Size = new Size(452, 150),
					ForeColor = System.Drawing.Color.White,
					AutoSize = false,
				};
				Label name = new Label
				{
					Tag = _tag,
					Font = new Font("Arial", 18),
					Text = entry[2],
					Location = new Point(10, 15),
					ForeColor = System.Drawing.Color.White,
					AutoSize = true
				};
				p.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, p.Width, p.Height, 30, 30));
				FlowLayoutPanel fp = new FlowLayoutPanel
				{
					Tag = _tag,
					BackColor = System.Drawing.Color.FromArgb(196, 222, 186),
					Location = new Point(10, 45),
					Size = new Size(430, 120),
					ForeColor = System.Drawing.Color.White,
					AutoSize = false,
					AutoScroll = true,
					WrapContents = false,
				};
				fp.VerticalScroll.Enabled = false;
				fp.VerticalScroll.Visible = false;
				fp.HorizontalScroll.Visible = false;
				fp.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, fp.Width, fp.Height, 30, 30));
				myFarmsFlowPanel.Controls.Add(p);
				p.Controls.Add(name);
				p.Controls.Add(fp);
				string fileName = directory + "\\N" + entry[0] + "L" + entry[1] + "FN" + entry[2] + ".csv";
				StreamReader cropInfo = new StreamReader(@fileName);
				while (cropInfo.Peek() != -1)
				{
					string cEntryString = cropInfo.ReadLine();
					string[] cEntry = cEntryString.Split(',');
					Panel cp = new Panel
					{
						Tag = cTag,
						BackColor = System.Drawing.Color.FromArgb(222, 205, 5),
						Location = new Point(0, 50),
						ForeColor = System.Drawing.Color.White,
						AutoSize = false,
						Size = new Size(80, 95),
					};
					cp.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, cp.Width, cp.Height, 20, 20));
					PictureBox cropPic = new PictureBox
					{
						Image = Properties.Resources.wfield_208px,
						SizeMode = PictureBoxSizeMode.StretchImage,
						Size = new Size(70, 70),
						Location = new Point(5, 10)
					};
					Label nName = new Label
					{
						Tag = cTag,
						Font = new Font("Arial", 10, FontStyle.Bold),
						Text = cEntry[3],
						Location = new Point(3, 78),
						ForeColor = System.Drawing.Color.White,
						AutoSize = true
					};
					fp.Controls.Add(cp);
					cp.Controls.Add(cropPic);
					cp.Controls.Add(nName);
					cTag++;
				}
				cropInfo.Close();
				_tag++;
			}
			csvReader.Close();
		}
		void LineLoad()
		{
			farmChart.Series = new SeriesCollection
			{
				new LineSeries
				{
					Title = "Line 1",
					Values = new ChartValues<int> {56, 65, 77, 52, 65, 45, 35, 62, 74, 65},
					Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(80,9, 105, 54)),
					PointGeometry = null,
					LineSmoothness = 100,
					Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(9, 105, 54))
				},
			};
			List<string> l = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
			farmChart.AxisX.Add(new Axis
			{
				Labels = l
			});
			farmChart.AxisY.Add(new Axis
			{
				MinValue = 0
			});
		}
		private void addFarmBtn_Click(object sender, EventArgs e)
		{
			AddFarmForm addFarmForm = new AddFarmForm();
			addFarmForm.ShowDialog();
		}
		private void refreshBtn_Click(object sender, EventArgs e)
		{
			Refresh();
		}

	}
}
