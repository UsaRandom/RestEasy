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

      

        service.Register(RestMethod.GET, "/downloadfile/[name]", (req, res) =>
        {
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
