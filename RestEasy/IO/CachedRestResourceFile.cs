using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.IO
{
public class CachedRestResourceFile : RestResourceFile
{
	public CachedRestResourceFile(string fileLocation)
		: base(fileLocation)
	{

	}

	public override byte[] GetAllBytes()
	{
		if (m_fileBytes == null || LastModifiedDateTimeUTC > LastLoadDateTimeUTC)
		{
			m_fileBytes = File.ReadAllBytes(FileLocation);
		}
		return m_fileBytes;
	}

	public override string GetAllText()
	{
		if (m_fileText == null || LastModifiedDateTimeUTC > LastLoadDateTimeUTC)
		{
			m_fileText = File.ReadAllText(FileLocation);
		}
		return m_fileText;
	}

	
	private string m_fileText;
	private byte[] m_fileBytes;
}
}
