using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.IO
{
public class LocalRestResourceFile : RestResourceFile
{
	public LocalRestResourceFile(string fileLocation)
		: base (fileLocation)
	{

	}

	public override byte[] GetAllBytes()
	{
		return File.ReadAllBytes(FileLocation);
	}

	public override string GetAllText()
	{
		return File.ReadAllText(FileLocation);
	}
}
}
