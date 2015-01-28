# RestEasy

> Inspired by ExpressJS and Node, RestEasy is designed to have minimul features, and provide maxiumum productivity.

I like C#. It's a cool language. But Microsoft's tools and frameworks are bloatware (sorry Microsoft). When trudging around in ASP.NET, you can smell the tarpit. Anything that should be easy is non-trivial. Especially when it comes to REST.

That is why RestEasy was born. To provide a simple way to prototype RESTful APIs in C#, without all that bloat.

## Usage

#### Hello World Example:
Going to 'http://localhost:8080/Test' in browser will show "Hello World"

```c#
using RestEasy;
public class HelloWorld
{
	public static void Main()
	{
		var rest = new RestService();

		rest.Register(RestMethod.GET, "/Test", (req, res) => { res.Send("Hello World"); });

		rest.Listen(8080);

		Console.ReadKey();
	}
}
```

#### Example:

```c#
using RestEasy;
public class Program 
{
	public static void Main()
	{
 		var service = new RestService();

        //http://localhost:8080/
        service.Register(RestMethod.GET, "/", (req, res) =>
        {
            res.Send("Hello World");
        });

        service.Register(RestMethod.POST, "/user/[name]/update", (req, res) =>
        {
            var name = req.Parameters["name"]; //case sensitive (for no reason whatsoever)
            //do stuff...
        });

        service.Register(RestMethod.GET, "/downloadfile/[name]", (req, res) =>
        {
            //path provided from url parameters, example:
            //http://localhost:8080/downloadfile/Testfile.exe?path=C%3A%5CFile.exe
            res.SendFile(req.Parameters["name"], System.IO.File.ReadAllBytes(req.Parameters["path"]));
        });

        service.Error += (serv, error) =>
        {
            Console.WriteLine(error);
        };

        service.Listen(8080);

        Console.ReadKey();
	}
}
```


### Todo's

 - Clean Up

License
----

Public Domain

