# RestEasy

> Inspired by ExpressJS and Node, RestEasy is designed to have minimul features, and provide maxiumum productivity.

I like C#. It's a cool language. But Microsoft's tools and frameworks are bloatware (sorry Microsoft). When trudging around in ASP.NET, you can smell the tarpit. Anything that should be easy is non-trivial. Especially when it comes to REST.

That is why RestEasy was born. To provide a simple way to prototype RESTful APIs in C#, without all that bloat.

## Usage

#### Hello World Example:
Going to 'http://localhost/Test' in browser will show "Hello World"

```c#
using RestEasy;
public class HelloWorld
{
	public static void Main()
	{
		var rest = new RestService();

		rest.Register(RestMethod.GET, "/Test", (req, res) => { res.Send("Hello World"); });

		rest.Listen();
	}
}
```

#### Api Example:

```c#
using RestEasy;
public class Program 
{
	public static void Main()
	{
		var rest = new RestService();
		var fileCache = new RestFileCache(4000); //Max in-memory cache size (kb)

		//Setup API
		rest.Register(RestMethod.DELETE, "/object/[id]/delete", (RestRequest request, RestResponse response) => {

			var id = request.Parameters.GetInt32("id");
			// or use dynamics: 
			//dynamic id = request.Parameters["id"]; 

			//Delete logic here

			response.Send(RestStatus.OK);
		});

		//Static Resources (Code on Demand n' stuff)
		rest.Register(RestMethod.GET, "/js/codefile.min.js", (RestRequest request, RestResponse response) => {

			//send code file, will be cached based on last time file was written to (write timestamp)
			response.Send(fileCache.ReadFile(".../js/codefile.min.js"));

		});

		//...
		//... other api methods
		//...

		//subscribe to exceptions
		rest.Error += (s, e) => { Console.WriteLine(e); };

		//start up the rest service on port 8080
		rest.Listen(8080);
	}
}
```


### Todo's

 - Tests
 - and everything else :P

License
----

Public Domain

