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

        service.Register(RestMethod.GET, "/home", (req, res) =>
        {


        });

        service.Register(RestMethod.POST, "/home/[id]/delete", (req, res) =>
        {
            Console.WriteLine("Posted this data of " + req.Parameters["id"]);

        });


        service.Register(RestMethod.GET, "/home/[id]", (req, res) =>
        {
            Console.WriteLine("Requested an id of " + req.Parameters["id"]);

        });


        service.Register(RestMethod.GET, "/home/[id]/delete", (req, res) =>
        {
            Console.WriteLine("Requested to delete id of " + req.Parameters["id"]);
    
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
