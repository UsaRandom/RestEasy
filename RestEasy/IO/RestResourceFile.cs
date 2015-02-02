using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.IO
{
public abstract class RestResourceFile : IRestResourceFile
{

	protected RestResourceFile(string fileLocation)
	{
		FileLocation = fileLocation;
		Setup();	
	}

	private void Setup()
	{
		SetMimeTypeAndDetermineBinaryOrText(Path.GetExtension(FileLocation).Remove(0, 1));
		FileName = Path.GetFileName(FileLocation);
		LastLoadDateTimeUTC = DateTime.MinValue;
	}

	private void SetMimeTypeAndDetermineBinaryOrText(string extension)
	{
		switch (extension.ToLower())
		{
			/* Common */
			case EXT_JS:
				ContentType = MIME_JS;
				ShouldSendAsBinary = false;
				break;
			case EXT_HTML:
				ContentType = MIME_HTML;
				ShouldSendAsBinary = false;
				break;
			case EXT_CSS:
				ContentType = MIME_CSS;
				ShouldSendAsBinary = false;
				break;
			case EXT_JSON:
				ContentType = MIME_JSON;
				ShouldSendAsBinary = false;
				break;

			/* Images */
			case EXT_JPG:
				goto  case EXT_JPEG;
			case EXT_JPEG:
				ContentType = MIME_JPG_JPEG;
				ShouldSendAsBinary = true;
				break;
			case EXT_GIF:
				ContentType = MIME_GIF;
				ShouldSendAsBinary = true;
				break;
			case EXT_PNG:
				ContentType = MIME_PNG;
				ShouldSendAsBinary = true;
				break;
			case EXT_SVG:
				ContentType = MIME_SVG;
				ShouldSendAsBinary = false;
				break;


			/* Fonts */
			case EXT_TFF:
				ContentType = MIME_TFF;
				ShouldSendAsBinary = true;
				break;
			case EXT_WOFF:
				ContentType = MIME_WOFF;
				ShouldSendAsBinary = true;
				break;
			case EXT_WOFF2:
				ContentType = MIME_WOFF2;
				ShouldSendAsBinary = true;
				break;
			case EXT_EOT:
				ContentType = MIME_EOT;
				ShouldSendAsBinary = true;
				break;
			case EXT_OTF:
				ContentType = MIME_OTF;
				ShouldSendAsBinary = true;
				break;

			default:
				throw new Exception("Unknown Mime type for file");
		}
	}

	public abstract byte[] GetAllBytes();

	public abstract string GetAllText();

	public string ContentType
	{
		get;
		private set;
	}

	public string FileName
	{
		get;
		private set;
	}

	public bool ShouldSendAsBinary
	{
		get;
		private set;
	}

	public long Size
	{
		get
		{
			
			return new FileInfo(FileLocation).Length;

		}
	}

	public long TimesRequested
	{
		get;
		protected set;
	}

	public DateTime LastModifiedDateTimeUTC
	{
		get
		{
			return new FileInfo(FileLocation).LastWriteTimeUtc;
		}
	}

	protected DateTime LastLoadDateTimeUTC
	{
		get;
		set;
	}


	public string FileLocation
	{
		get;
		private set;
	}





	#region Common

	private const string EXT_JS = "js";
	private const string MIME_JS = "application/javascript";

	private const string EXT_CSS = "css";
	private const string MIME_CSS = "text/css";
	
	private const string EXT_HTML = "html";
	private const string MIME_HTML = "text/html";

	private const string EXT_JSON = "json";
	private const string MIME_JSON = "application/json";

	#endregion




	#region Images

	private const string EXT_JPG = "jpg";
	private const string EXT_JPEG = "jpeg";
	private const string MIME_JPG_JPEG = "image/jpeg";

	private const string EXT_GIF = "gif";
	private const string MIME_GIF = "image/gif";

	private const string EXT_PNG = "png";
	private const string MIME_PNG = "image/png";

	private const string EXT_SVG = "svg";
	private const string MIME_SVG = "image/svg+xml";

	#endregion


	private const string EXT_EOT = "eot";
	private const string MIME_EOT = "application/vnd.ms-fontobject";

	private const string EXT_TFF = "ttf";
	private const string MIME_TFF = "application/x-font-ttf";
	
	private const string EXT_WOFF = "woff";
	private const string MIME_WOFF = "application/x-font-woff";

	private const string EXT_OTF = "otf";
	private const string MIME_OTF = "application/x-font-opentype";

	private const string EXT_WOFF2 = "woff2";
	private const string MIME_WOFF2 = "font/woff2";

}
}
