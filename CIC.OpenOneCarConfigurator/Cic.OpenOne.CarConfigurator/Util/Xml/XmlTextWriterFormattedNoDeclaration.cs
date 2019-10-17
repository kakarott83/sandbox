// OWNER: BK, 02-02-2009
namespace Cic.OpenOne.Util.Xml
{
	//[System.CLSCompliant(true)]
	public class XmlTextWriterFormattedNoDeclaration : System.Xml.XmlTextWriter
	{
		#region Constructors
		// TEST BK 0 BK, Not tested
		internal XmlTextWriterFormattedNoDeclaration(System.IO.Stream stream, System.Text.Encoding encoding)
			: base(stream, encoding)
		{
			// Set formatting
			this.Formatting = System.Xml.Formatting.Indented;
		}
		#endregion

		#region Override methods
		// TEST BK 0 BK, Not tested
		public override void WriteStartDocument() 
		{
			// Do nothing
		}

		//// TEST BK 0 BK, Not tested
		//public override void WriteBase64(byte[] buffer, int index, int count)
		//{
		//    // base
		//    base.WriteBase64(buffer, index, count); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteBinHex(byte[] buffer, int index, int count)
		//{
		//    // base
		//    base.WriteBinHex(buffer, index, count); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteCData(string text)
		//{
		//    // base
		//    base.WriteCData(text); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteCharEntity(char ch)
		//{
		//    // base
		//    base.WriteCharEntity(ch); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteChars(char[] buffer, int index, int count)
		//{
		//    // base
		//    base.WriteChars(buffer, index, count); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteComment(string text)
		//{
		//    // base
		//    base.WriteComment(text); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteDocType(string name, string pubid, string sysid, string subset)
		//{
		//    // base
		//    base.WriteDocType(name, pubid, sysid, subset); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteEndAttribute()
		//{
		//    // base
		//    base.WriteEndAttribute(); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteEndDocument()
		//{
		//    // base
		//    base.WriteEndDocument(); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteEndElement()
		//{
		//    // base
		//    base.WriteEndElement(); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteEntityRef(string name)
		//{
		//    // base
		//    base.WriteEntityRef(name); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteFullEndElement()
		//{
		//    // base
		//    base.WriteFullEndElement(); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteName(string name)
		//{
		//    // base
		//    base.WriteName(name); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteNmToken(string name)
		//{
		//    // base
		//    base.WriteNmToken(name);
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteProcessingInstruction(string name, string text)
		//{
		//    // base
		//    base.WriteProcessingInstruction(name, text); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteQualifiedName(string localName, string ns)
		//{
		//    // base
		//    base.WriteQualifiedName(localName, ns); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteRaw(string data)
		//{
		//    // base
		//    base.WriteRaw(data); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteRaw(char[] buffer, int index, int count)
		//{
		//    // base
		//    base.WriteRaw(buffer, index, count); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteStartAttribute(string prefix, string localName, string ns)
		//{
		//    // base
		//    base.WriteStartAttribute(prefix, localName, ns); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteStartDocument()
		//{
		//    // base
		//    base.WriteStartDocument(); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteStartDocument(bool standalone)
		//{
		//    // base
		//    base.WriteStartDocument(standalone); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteStartElement(string prefix, string localName, string ns)
		//{
		//    // base
		//    base.WriteStartElement(prefix, localName, ns); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteString(string text)
		//{
		//    // base
		//    base.WriteString(text); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		//{
		//    // base
		//    base.WriteSurrogateCharEntity(lowChar, highChar); 
		//}

		//// TEST BK 0 BK, Not tested
		//public override void WriteWhitespace(string ws)
		//{
		//    // base
		//    base.WriteWhitespace(ws); 
		//}
		#endregion
	}
}
