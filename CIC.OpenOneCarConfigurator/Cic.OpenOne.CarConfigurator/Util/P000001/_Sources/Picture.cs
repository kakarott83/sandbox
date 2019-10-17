// OWNER: BK, 15-04-2008
using Cic.OpenOne.Util.Reflection;
namespace Cic.P000001.Common
{
	[System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
	public sealed class Picture : Cic.P000001.Common.IPicture, IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private string _Title;
		private Cic.P000001.Common.PictureTypeConstants _PictureTypeConstant;
		private byte[] _Content;
		private Cic.P000001.Common.ImageFileTypeConstants _ImageFileTypeConstant;
		private int _Width;
		private int _Height;
		private string _Hash;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY PictureTestFixture.CheckProperties
		public Picture()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY PictureTestFixture.ConstructWithoutTitle
		// TESTEDBY PictureTestFixture.ConstructWithEmptyTitle
		// TESTEDBY PictureTestFixture.ConstructWithSpaceTitle
		// TESTEDBY PictureTestFixture.ConstructWithInvalidPictureTypeConstant
		// TESTEDBY PictureTestFixture.ConstructWithInvalidImageFileTypeConstant
		// TESTEDBY PictureTestFixture.ConstructWithInvalidWidth
		// TESTEDBY PictureTestFixture.ConstructWithInvalidHeight
		// TESTEDBY PictureTestFixture.CheckProperties
		public Picture(string title, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, byte[] content, Cic.P000001.Common.ImageFileTypeConstants imageFileTypeConstant, int width, int height, string hash)
		{
			// Check title
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(title))
			{
				// Throw exception
				throw  new System.ArgumentException("title");
			}

			// Check picture type constant
			if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.PictureTypeConstants), pictureTypeConstant))
			{
				// Throw exception
				throw  new System.ArgumentException("pictureTypeConstant");
			}

			// Check image file type constant
			if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.ImageFileTypeConstants), imageFileTypeConstant))
			{
				// Throw exception
				throw  new System.ArgumentException("imageFileTypeConstant");
			}

			// Check width
			if (width <= 0)
			{
				// Throw exception
				throw  new System.ArgumentException("width");
			}

			// Check height
			if (height <= 0)
			{
				// Throw exception
				throw  new System.ArgumentException("height");
			}

			// Check hash
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(hash))
			{
				// Throw exception
				throw  new System.ArgumentException("hash");
			}

			// Set values
			_Title = title.Trim();
			_PictureTypeConstant = pictureTypeConstant;
			_Content = content;
			_ImageFileTypeConstant = imageFileTypeConstant;
			_Width = width;
			_Height = height;
			_Hash = hash.Trim();
		}
		#endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.Picture")]
		// TESTEDBY PictureTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct
					// Create new instance
					new Cic.P000001.Common.Picture(_Title, _PictureTypeConstant, _Content, _ImageFileTypeConstant, _Width, _Height, _Hash);
				}
				catch
				{
					// Throw caught exception
					throw;
				}

				// Reset state
				_Parameterless = false;
			}
		}
		#endregion

		#region IPictureBase properties
        [System.Runtime.Serialization.DataMember]
		public string Title
		{
			// TESTEDBY PictureTestFixture.CheckProperties
			get
			{
				// Return
				return _Title;
			}
			// NOTE BK, For serialization
			// TESTEDBY PictureTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Title = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember]
		public Cic.P000001.Common.PictureTypeConstants PictureTypeConstant
		{
			// TESTEDBY PictureTestFixture.CheckProperties
			get
			{
				// Return
				return _PictureTypeConstant;
			}
			// NOTE BK, For serialization
			// TESTEDBY PictureTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_PictureTypeConstant = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember]
		public byte[] Content
		{
			// TESTEDBY PictureTestFixture.CheckProperties
			get
			{
				// Return
				return _Content;
			}
			// NOTE BK, For serialization
			// TESTEDBY PictureTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Content = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember]
		public Cic.P000001.Common.ImageFileTypeConstants ImageFileTypeConstant
		{
			// TESTEDBY PictureTestFixture.CheckProperties
			get
			{
				// Return
				return _ImageFileTypeConstant;
			}
			// NOTE BK, For serialization
			// TESTEDBY PictureTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ImageFileTypeConstant = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember]
		public int Width
		{
			// TESTEDBY PictureTestFixture.CheckProperties
			get
			{
				// Return
				return _Width;
			}
			// NOTE BK, For serialization
			// TESTEDBY PictureTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Width = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember]
		public int Height
		{
			// TESTEDBY PictureTestFixture.CheckProperties
			get
			{
				// Return
				return _Height;
			}
			// NOTE BK, For serialization
			// TESTEDBY PictureTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Height = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember]
		public string Hash
		{
			// TESTEDBY PictureTestFixture.CheckProperties
			get
			{
				// Return
				return _Hash;
			}
			// NOTE BK, For serialization
			// TESTEDBY PictureTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Hash = value;
				}
			}
		}
		#endregion
	}
}
