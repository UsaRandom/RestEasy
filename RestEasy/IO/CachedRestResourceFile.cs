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
        if (m_fileBytes == null || NeedsToBeRefreshed)
		{
			m_fileBytes = File.ReadAllBytes(FileLocation);
            LastLoadDateTimeUTC = DateTime.UtcNow;
		}
		return m_fileBytes;
	}

	public override string GetAllText()
	{
		if (m_fileText == null || NeedsToBeRefreshed)
		{
			m_fileText = File.ReadAllText(FileLocation);
            LastLoadDateTimeUTC = DateTime.UtcNow;
		}
		return m_fileText;
	}

    internal bool NeedsToBeRefreshed
    {
        get
        {
            var fileInfo = new FileInfo(FileLocation);
            fileInfo.Refresh();

            return m_needsToBeRefreshed || fileInfo.LastWriteTimeUtc > LastLoadDateTimeUTC || fileInfo.CreationTimeUtc > LastLoadDateTimeUTC;
        }
        set
        {
            m_needsToBeRefreshed = value;
        }
    }



    private bool m_needsToBeRefreshed;
	private string m_fileText;
	private byte[] m_fileBytes;
}
}
