using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.IO
{
public class RestResourceCache
{

	public RestResourceCache(int maxSizeInKb)
	{
		MaxSizeInKb = maxSizeInKb;
		m_files = new Dictionary<string, IRestResourceFile>();
	}


	public void RegisterFile(string fileLocation)
	{
		fileLocation = Path.Combine(Environment.CurrentDirectory, fileLocation);
		if(m_files.ContainsKey(fileLocation))
			return;

		IRestResourceFile newRestResourceFile;

		var fileSizeInKb = (int)new FileInfo(fileLocation).Length/1000;
		
		if (CurrentCacheSizeInKb + fileSizeInKb < MaxSizeInKb)
		{
			newRestResourceFile = new CachedRestResourceFile(fileLocation);

			CurrentCacheSizeInKb += fileSizeInKb;
		}
		else
		{
			newRestResourceFile = new LocalRestResourceFile(fileLocation);
		}

		m_files.Add(fileLocation, newRestResourceFile);
	}

	public void RegisterFolder(string folderLocation)
	{
		folderLocation = Path.Combine(Environment.CurrentDirectory, folderLocation);
		foreach(var file in Directory.GetFiles(folderLocation))
		{
			RegisterFile(file);
		}
	}


	public void RegisterFolderAndSubFolders(string rootFolderLocation)
	{
		rootFolderLocation = Path.Combine(Environment.CurrentDirectory, rootFolderLocation);
		foreach(var file in Directory.GetFiles(rootFolderLocation))
		{
			RegisterFile(file);
		}

		foreach(var folder in Directory.GetDirectories(rootFolderLocation))
		{
			RegisterFolderAndSubFolders(folder);
		}
	}

	public IRestResourceFile FetchFile(string fileLocation)
	{
		fileLocation = Path.Combine(Environment.CurrentDirectory, fileLocation.Replace("/", @"\").TrimStart(new char[] {'\\', '/' }));
		IRestResourceFile file = null;

		if(m_files.TryGetValue(fileLocation, out file))
		{
			return file;
		}

		return null;
	}


	public int MaxSizeInKb
	{
		get;
		private set;
	}

	public int CurrentCacheSizeInKb
	{
		get;
		private set;
	}


	private IDictionary<string, IRestResourceFile> m_files; 
}
}
