﻿namespace PlowSense.Models
{
	public class FarmInfo	
	{
		public string Farmer { get; set; }
		public string Location { get; set; }
		public string Farm { get; set; }
		public System.Collections.Generic.List<CropInfo> Crops {get; set;}
	}
}