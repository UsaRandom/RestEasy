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
        : this(maxSizeInKb, Environment.CurrentDirectory)
	{
	}

    public RestResourceCache(int maxSizeInKb, string rootDirectory)
    {
        MaxSizeInKb = maxSizeInKb;
        m_rootDirectory = rootDirectory;
        m_files = new Dictionary<string, IRestResourceFile>();
        m_folderWatchers = new Dictionary<string, FileSystemWatcher>();
    }

	public void RegisterFile(string fileLocation)
	{
        fileLocation = Path.Combine(m_rootDirectory, CleanFileLocation(fileLocation));

		if(m_files.ContainsKey(fileLocation))
			return;

        var directory = Path.GetDirectoryName(fileLocation);

        if (!m_folderWatchers.ContainsKey(directory))
        {
            var newWatcher = new FileSystemWatcher(directory);

            newWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite;

            newWatcher.Changed += new FileSystemEventHandler(OnFileChanged);
            newWatcher.Created += new FileSystemEventHandler(OnFileChanged);

            newWatcher.EnableRaisingEvents = true;

            m_folderWatchers.Add(directory, newWatcher);
        }

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
        folderLocation = Path.Combine(m_rootDirectory, folderLocation);
		foreach(var file in Directory.GetFiles(folderLocation))
		{
			RegisterFile(file);
		}
	}


	public void RegisterFolderAndSubFolders(string rootFolderLocation)
	{
        rootFolderLocation = Path.Combine(m_rootDirectory, rootFolderLocation);
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
        fileLocation = Path.Combine(m_rootDirectory, CleanFileLocation(fileLocation));

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

    private string CleanFileLocation(string fileLocation)
    {
        return fileLocation.Replace("/", @"\").TrimStart(new char[] { '\\', '/' });
    }

    private void OnFileChanged(object source, FileSystemEventArgs fileSystemEventArgs)
    {
        var filePath = fileSystemEventArgs.FullPath;

        var resourceFile = FetchFile(filePath) as CachedRestResourceFile;

        if(resourceFile == null)
        {
            return;
        }
        Console.WriteLine(filePath + " needs refresh");
        resourceFile.NeedsToBeRefreshed = true;
    }

    private readonly string m_rootDirectory;
    private IDictionary<string, FileSystemWatcher> m_folderWatchers;
	private IDictionary<string, IRestResourceFile> m_files; 
}
}
