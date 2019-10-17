using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
	public enum DeepLinkType
	{
		UNKNOWN = 0,
		WEB = 1,
		WINDOWS = 2,
		EXTERN = 3,
        IFRAME = 4
	}
	public class DeepLnkDto
	{
		public long SYSDEEPLNK { get; set; }
		public String AREA { get; set; }
		public String CODE { get; set; }
		public String EXECEXPRESSION { get; set; }
		public String DESCRIPTION { get; set; }
		public int CLIENTTYPE { get; set; }
		public String ALTERNATEBASISURL { get; set; }
		public String PARAMEXPRESSION { get; set; }
		public int TARGETTYPE { get; set; }
		public long SYSEAIART { get; set; }
		public String PARAMSIGN { get; set; }
		public String CODERFU { get; set; }
		public String CODERMO { get; set; }

		public String EVALPARAM01 { get; set; }
		public String EVALPARAM02 { get; set; }
		public String EVALPARAM03 { get; set; }
		public String EVALPARAM04 { get; set; }
		public String EVALPARAM05 { get; set; }
		public String EVALPARAM06 { get; set; }
		public String EVALPARAM07 { get; set; }
		public String EVALPARAM08 { get; set; }
		public String EVALPARAM09 { get; set; }
		public String EVALPARAM10 { get; set; }

		public int USEINTERNALVIEWER { get; set; }
		public int CLOSETARGETAPP { get; set; }
		public int TARGETSUBCODE { get; set; }
		public int USEINBOXFLAG { get; set; }
		public String POSTEXPRESSION { get; set; }

		/// <summary>
		/// Returns the parsed expression, which is:
		/// EXECEXPRESSIONS <PARAMSIGN>p<pnumber> like bla?xy=:p01 with paramsign=: (default)
		/// will be replaced with the content of the corresponding EVALPARAM01, when not empty
		/// syntax is {{$object.area}} to access wctx's area-field for example, so EVALPARAM01 would contain "{{$object.area}}" and would result in bla?xy=<area from WorkflowContext>
		/// </summary>
		/// <param name="wctx"></param>
		/// <returns></returns>
		public String getParsedExpression (WorkflowContext wctx)
		{
			return getParsedExpression (wctx, EXECEXPRESSION);
		}
		/// <summary>
		/// Returns the parsed expression, which is:
		/// EXECEXPRESSIONS <PARAMSIGN>p<pnumber> like bla?xy=:p01 with paramsign=: (default)
		/// will be replaced with the content of the corresponding EVALPARAM01, when not empty
		/// syntax is {{$object.area}} to access wctx's area-field for example, so EVALPARAM01 would contain "{{$object.area}}" and would result in bla?xy=<area from WorkflowContext>
		/// </summary>
		/// <param name="wctx"></param>
		/// <param name="expr">expression to parse</param>
		/// <returns></returns>
		public String getParsedExpression (WorkflowContext wctx, String expr)
		{
			if (expr == null)
				return "";
			String rval = expr;
			String sig = ":";
			if (PARAMSIGN != null)
				sig = PARAMSIGN;

			HtmlReportBo bo = new HtmlReportBo (new StringHtmlTemplateDao (null));

			String p = "";
			if (EVALPARAM01 != null)
			{
				p = bo.ReplaceText (EVALPARAM01, wctx, true);
				rval = rval.Replace (sig + "p01", p);
			}
			if (EVALPARAM02 != null)
			{
				p = bo.ReplaceText (EVALPARAM02, wctx, true);
				rval = rval.Replace (sig + "p02", p);
			}
			if (EVALPARAM03 != null)
			{
				p = bo.ReplaceText (EVALPARAM03, wctx, true);
				rval = rval.Replace (sig + "p03", p);
			}
			if (EVALPARAM04 != null)
			{
				p = bo.ReplaceText (EVALPARAM04, wctx, true);
				rval = rval.Replace (sig + "p04", p);
			}
			if (EVALPARAM05 != null)
			{
				p = bo.ReplaceText (EVALPARAM05, wctx, true);
				rval = rval.Replace (sig + "p05", p);
			}
			if (EVALPARAM06 != null)
			{
				p = bo.ReplaceText (EVALPARAM06, wctx, true);
				rval = rval.Replace (sig + "p06", p);
			}
			if (EVALPARAM07 != null)
			{
				p = bo.ReplaceText (EVALPARAM07, wctx, true);
				rval = rval.Replace (sig + "p07", p);
			}
			if (EVALPARAM08 != null)
			{
				p = bo.ReplaceText (EVALPARAM08, wctx, true);
				rval = rval.Replace (sig + "p08", p);
			}
			if (EVALPARAM09 != null)
			{
				p = bo.ReplaceText (EVALPARAM09, wctx, true);
				rval = rval.Replace (sig + "p09", p);
			}
			if (EVALPARAM10 != null)
			{
				p = bo.ReplaceText (EVALPARAM10, wctx, true);
				rval = rval.Replace (sig + "p10", p);
			}
			return rval;
		}
	}
}
