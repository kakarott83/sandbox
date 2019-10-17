// OWNER: BK, 01-00-2008
using Cic.OpenOne.Util.Xml;
namespace Cic.OpenOne.CarConfigurator.Util.Xml
{
	[System.CLSCompliant(true)]
	public static class SerializationHelper
	{
		#region Methods
		// NOTE BK, "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
		// TEST BK 0 BK, Not tested
		public static string SerializeUTF8FormattedNoDeclaration(object value)
		{
			try
			{
				// My
				return MySerializeUTF8(value, true, false);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// NOTE BK, "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
		// TEST BK 0 BK, Not tested
		public static string SerializeUTF8WithoutNamespace(object value)
		{
			try
			{
				// My
				return MySerializeUTF8(value, false, true);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// NOTE BK, "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
		// TEST BK 0 BK, Not tested
		public static string SerializeUTF8(object value)
		{
			try
			{
				// My
				return MySerializeUTF8(value, false, false);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}

		// NOTE BK, "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
		// TEST BK 0 BK, Not tested
		public static object DeserializeUTF8(string value, System.Type type)
		{
			try
			{
				// My
				return MyDeserializeUTF8(value, type);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region My methods
		// NOTE BK, "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
		private static string MySerializeUTF8(object value, bool formattedNoDeclaration, bool withoutNamespace)
		{
			System.Text.UTF8Encoding UTF8Encoding;
			System.IO.MemoryStream MemoryStream;
			System.Xml.Serialization.XmlSerializer XmlSerializer;
			System.Xml.XmlTextWriter XmlTextWriter;
			System.Xml.Serialization.XmlSerializerNamespaces XmlSerializerNamespaces;
			string Result = null;

			// Check value
			if (value != null)
			{
				// New memory stream
				using (MemoryStream = new System.IO.MemoryStream())
				{
					try
					{
						// New serializer
						XmlSerializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
					}
					catch
					{
						// Throw caught exception
						throw;
					}

					// New encoding
					UTF8Encoding = new System.Text.UTF8Encoding();

					// Check state
					if (formattedNoDeclaration)
					{
						// New xml writer
						XmlTextWriter = new XmlTextWriterFormattedNoDeclaration(MemoryStream, UTF8Encoding);
					}
					else
					{
						// New xml writer
						XmlTextWriter = new System.Xml.XmlTextWriter(MemoryStream, UTF8Encoding);
					}

					try
					{
						// Check state
						if (withoutNamespace)
						{
							// New namespaces
							XmlSerializerNamespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
							// Add empty namspace
							XmlSerializerNamespaces.Add("", "");
							// Serialize
							XmlSerializer.Serialize(XmlTextWriter, value, XmlSerializerNamespaces);
						}
						else
						{
							// Serialize
							XmlSerializer.Serialize(XmlTextWriter, value);
						}
					}
					catch
					{
						// Throw caught exception
						throw;
					}

					// Get string
					Result = UTF8Encoding.GetString(MemoryStream.ToArray());
					// Close stream
					MemoryStream.Close();
				}
			}

			// Return
			return Result;
		}

		// NOTE BK, "UTF8" is the prefered notation inside the System.Text namespace (not in the system xml namespace)
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UTF")]
		private static object MyDeserializeUTF8(string value, System.Type type)
		{
			System.Text.UTF8Encoding UTF8Encoding;
			System.IO.MemoryStream MemoryStream;
			System.Xml.Serialization.XmlSerializer XmlSerializer;
			object Result = null;

			// Check value
			if (value != null)
			{
				// New encoding
				UTF8Encoding = new System.Text.UTF8Encoding();
				// New memory stream
				using (MemoryStream = new System.IO.MemoryStream(UTF8Encoding.GetBytes(value)))
				{
					// New serializer
					XmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
					// Deserialize
					Result = XmlSerializer.Deserialize(MemoryStream);
					// Close stream
					MemoryStream.Close();
				}
			}

			// Return
			return Result;
		}
		#endregion
	}
}
