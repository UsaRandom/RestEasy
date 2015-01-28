using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestEasy.Web
{

internal delegate void ServerErrorHandler(RestHttpServer restHttpServer, Exception exception);
internal delegate void ServerRequestHandler(HttpListenerRequest httpRequest, HttpListenerResponse httpResponse);

internal class RestHttpServer
{

    public event ServerErrorHandler Error;
    public event ServerRequestHandler Request;
    


    public RestHttpServer(int port)
    {
        m_port = port;
        m_httpListener = new HttpListener();
    }

    public void Listen()
    {
        if (!HttpListener.IsSupported)
            throw new NotSupportedException("Windows XP SP2, Server 2003 or later required.");

        if (m_httpListener.IsListening)
            throw new ApplicationException("Http listener is already listening.");

        m_httpListener = new HttpListener();

        SetupPrefixes();

        Run();
    }




    private void Run()
    {
        m_httpListener.Start();

        ThreadPool.QueueUserWorkItem((o) =>
        {
            try
            {
                while (m_httpListener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem((listenerContext) =>
                    {
                        var context = listenerContext as HttpListenerContext;

                        try
                        {
                            //allow access from anywhere
                            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                            Request(context.Request, context.Response);

                        }
                        catch (Exception requestException)
                        {
                            Error(this, requestException);
                        }
                        finally
                        {
                            context.Response.OutputStream.Close();
                        }


                    }, m_httpListener.GetContext());
                    
                }
            }
            catch (Exception serverException) 
            {
                //need to handle this, send it back to the RestService object
                Error(this, serverException);
            }

        });
    }


    private void SetupPrefixes()
    {
        bool usingPortEighty = m_port == 80;
        bool hasElevatedPermissions = HasElevatedPermissions();

        if (m_port < 1024 && !hasElevatedPermissions)
        {
            throw new UnauthorizedAccessException("Ports below 1024 require elevated permissions.");
        }

        /*  Not using wildcards until I can figure out how to get around the  "conflicts with an existing registration on the machine" error*/
      //  if (hasElevatedPermissions)
      //  {
      //      //elevated is easy, just accept everything!
      //      m_httpListener.Prefixes.Add("http://*" + (usingPortEighty ? "/" : ":" + m_port + "/"));
     //   }
       // else
     //   {

        //localhost, 127.0.0.1, machine name, and all ip addresses assigned from dns
        m_httpListener.Prefixes.Add("http://localhost" + (usingPortEighty ? "/" : ":" + m_port + "/"));
        m_httpListener.Prefixes.Add("http://127.0.0.1" + (usingPortEighty ? "/" : ":" + m_port + "/"));
        m_httpListener.Prefixes.Add("http://" + Environment.MachineName + (usingPortEighty ? "/" : ":" + m_port + "/"));

        foreach (var ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                m_httpListener.Prefixes.Add("http://" + ipAddress.ToString() + (usingPortEighty ? "/" : ":" + m_port + "/"));
        }
     //   }
    }


    private bool HasElevatedPermissions()
    {
        try
        {
            // Wildcard prefixes require elevated permissions.
            // This is a simple test to check to see if we can use them.
            var testListener = new HttpListener();
            testListener.Prefixes.Add("http://*:"+m_port+"/");
            testListener.Start();
        }
        catch(Exception)
        {
            return false;
        }
        return true;
    }


    private readonly int m_port;
    private HttpListener m_httpListener;
}
}
