# RestEasy

> Inspired by ExpressJS and Node, RestEasy is designed to have minimul features, and provide maxiumum productivity.

I like C#. It's a cool language. But Microsoft's tools and frameworks are bloatware (sorry Microsoft). When trudging around in ASP.NET, you can smell the tarpit. Anything that should be easy is non-trivial. Especially when it comes to REST.

That is why RestEasy was born. To provide a simple way to prototype RESTful APIs in C#, without all that bloat.

## Usage

#### Hello World Example:
Going to 'http://localhost:8080/Test' in browser will show "Hello World"

```c#
using System;
using RestEasy;
public class HelloWorld
{
	public static void Main()
	{
		var rest = new RestService(8080);

		rest.Register(RestMethod.GET, "/Test", (req, res) => { res.Send("Hello World"); });

		rest.Listen();

		Console.ReadKey();
	}
}
```

#### Example:

```c#
using System;
using RestEasy;
using RestEasy.IO;

namespace RestEasyExample
{
class Program
{
    private static RestResourceCache resourceCache;

    static void Main()
    {
        resourceCache = new RestResourceCache(40000); //40mb in-memory cache

        //register individual files (relative to executable path)
        resourceCache.RegisterFile("index.html");
        
        //register all files in folder
        resourceCache.RegisterFolder("js");
        resourceCache.RegisterFolder("img");

        //register all files in folder and all sub folders
        resourceCache.RegisterFolderAndSubFolders("css");
        
        var restService = new RestService(80);

        //listen to errors
        restService.Error += (service, error) => Console.WriteLine(error);

        //http://localhost/
        restService.Register(RestMethod.GET, "/", (req, res) =>
        {
            IRestResourceFile index = resourceCache.FetchFile("index.html");

            //Send index file
            res.Send(index);
        });

        //send off static files (only from these folders)
        restService.Register(RestMethod.GET, "/js/*", StaticFileRequestHandler);
        restService.Register(RestMethod.GET, "/img/*", StaticFileRequestHandler);
        restService.Register(RestMethod.GET, "/css/*", StaticFileRequestHandler);

        //parameters
        restService.Register(RestMethod.GET, "/user/[username]", (req, res) => {

            //http://localhost/users/RestEasyUser?token=4920cfjh30dk4n
            string username = req.Parameters["username"];

            string token = req.Parameters["token"];

            //... do stuff

            //sends "You entered a username of : RestEasyUser. With a token of : 4920cfjh30dk4n"
            res.Send("You entered a username of : " + username +". With a token of : "+ token);
        });

        restService.Register(RestMethod.POST, "/user/[username]", (req, res) => {
            string username = req.Parameters["username"];

            //delete username
        
            res.Send(); 
        });

        restService.Listen();

        Console.ReadKey();

    }

    private static void StaticFileRequestHandler(RestRequest request, RestResponse response)
    {
        var file = resourceCache.FetchFile(request.Url);
        response.Send(file);
    }
}
}
```


### Build

 [mstauber/RestEasyBuild](https://github.com/mstauber/RestEasyBuild)

License
----

Public Domain

