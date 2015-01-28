using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestEasy;
using System.IO;

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

        //http://localhost:8080/home
        service.Register(RestMethod.GET, "/home", (req, res) =>
        {

            res.SendHtml("<!DOCTYPE html><html><head><title>hi</title></head><body><h1>Hello World!</h1></body></html>");

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
}
