using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.IO
{
public interface IRestResourceFile
{
	byte[] GetAllBytes();
 
	string GetAllText();

	string ContentType
	{
		get;
	}

	string FileName
	{
		get;
	}

	bool ShouldSendAsBinary
	{
		get;
	}

	long Size
	{
		get;
	}

	long TimesRequested
	{
		get;
	}

	DateTime LastModifiedDateTimeUTC
	{
		get;
	}
	string FileLocation
	{
		get;
	}
}
}
