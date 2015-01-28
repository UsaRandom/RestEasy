using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestEasy;

namespace RestEasyExample
{
class Program
{
    static void Main(string[] args)
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
            //path provided from get, example url:
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
}
