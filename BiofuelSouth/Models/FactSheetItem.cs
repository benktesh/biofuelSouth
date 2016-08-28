using System;
using System.Collections.Generic;
using BiofuelSouth.Enum;

namespace BiofuelSouth.Models
{
	public class FactSheetItem
	{
		public List<FactSheetContent> ContentKeys { get; set; }
		public Dictionary<int, object> Data { get; set; }
		public CropType CropType { get; set; }
		public bool CanPDFFactsheet { get; set; }

		public FactSheetItem()
		{
			Data = new Dictionary<int, object>();
			ContentKeys = new List<FactSheetContent> {};
		}
	}

	public class FactSheetContent
	{
		public int Order { get; set; }
		public int Key { get; set; }
		public  DataType DataType { get; set; }

		public Object Data { get; set;  }

		public string Description { get; set;  }

		public bool HideKeyAsSectionHead { get;set; }
	}

	public class ImageTuple
	{
		public string ImageUri { get; set; }
		public string ImageCaption { get; set; }
	}

	public class TableData
	{
		public string Caption { get; set; }
		public string Data { get; set;  } 
	}

	public enum DataType { HtmlString, Text, Table, Image }

}