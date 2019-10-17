using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
	public class SlaDto : EntityDto
	{

		override public long getEntityId ()
		{
			return sysid;
		}
		/* Primärschlüssel */
		public long sysid { get; set; }
		// ...
		public bool Aktiv { get; set; }
		public bool Pause { get; set; }

		public String ServiceLevel { get; set; }
		public String Metrik { get; set; }
		public String Status { get; set; }
		public String NextStatus { get; set; }

		public DateTime? StatusDate { get; set; }
		public DateTime? NextStatusDate { get; set; }
		public long StatusTime { get; set; }
		public long NextStatusTime { get; set; }
		
		// toString-Converter
		public String StatusDateGuiString
		{
			get
			{
				return String.Format ("{0:dd.MM.yyyy}", StatusDate);
			}
			set { }
		}

		// toString-Converter
		public String NextStatusDateGuiString
		{
			get
			{
				return String.Format ("{0:dd.MM.yyyy}", NextStatusDate);
			}
			set { }
		}
		/// <summary>
		/// StatusTimeGUI-Helper
		/// </summary>
		public DateTime? StatusTimeGUI
		{
			get
			{
				return DateTimeHelper.ClarionTimeToDateTimeNoException ((int) StatusTime);
			}
			set
			{
				int? val = DateTimeHelper.DateTimeToClarionTime (value);
				if (val.HasValue)
					StatusTime = (long) val.Value;
				else
					StatusTime = 0;
			}
		}
		// toString-Converter
		public String StatusTimeGuiString
		{
			get
			{
				return String.Format ("{0:HH:mm}", StatusTimeGUI);
			}
			set { }
		}
		/// <summary>
		/// NextStatusTimeGUI-Helper
		/// </summary>
		public DateTime? NextStatusTimeGUI
		{
			get
			{
				return DateTimeHelper.ClarionTimeToDateTimeNoException ((int) NextStatusTime);
			}
			set
			{
				int? val = DateTimeHelper.DateTimeToClarionTime (value);
				if (val.HasValue)
					NextStatusTime = (long) val.Value;
				else
					NextStatusTime = 0;
			}
		}
		// toString-Converter
		public String NextStatusTimeGuiString
		{
			get
			{
				return String.Format ("{0:HH:mm}", NextStatusTimeGUI);
			}
			set { }
		}
	}
}