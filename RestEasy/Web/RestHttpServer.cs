using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
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
    


    public RestHttpServer(int port, bool useSsl)
    {
        m_port = port;
		m_useSsl = useSsl;


		if(m_port < 1024 && !HasElevatedPermissions())
		{
            throw new UnauthorizedAccessException("Ports below 1024 require elevated permissions.");
		}
    }

    public void Listen()
    {
        if (!HttpListener.IsSupported)
            throw new NotSupportedException("Windows XP SP2, Server 2003 or later required.");

        if (m_httpListener != null && m_httpListener.IsListening)
            throw new ApplicationException("Http listener is already listening.");

		if(!IsPortAvailable())
			throw new ApplicationException(string.Format("Requested port {0} is already in use.", m_port));



        m_httpListener = new HttpListener();

        SetupPrefixes();

        Run();
    }


    private void Run()
    {
		AppDomain.CurrentDomain.ProcessExit += ProcessExit;

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
		var protocol =  GetProtocol();

        if (HasElevatedPermissions())
        {
            //elevated is easy, just accept everything!
            m_httpListener.Prefixes.Add(protocol + "://+" + (usingPortEighty ? "/" : ":" + m_port + "/"));
        }
        else
        {

			//localhost, 127.0.0.1, machine name, and all ip addresses assigned from dns
			m_httpListener.Prefixes.Add(protocol + "://localhost" + (usingPortEighty ? "/" : ":" + m_port + "/"));
			m_httpListener.Prefixes.Add(protocol + "://127.0.0.1" + (usingPortEighty ? "/" : ":" + m_port + "/"));
			m_httpListener.Prefixes.Add(protocol + "://" + Environment.MachineName + (usingPortEighty ? "/" : ":" + m_port + "/"));

			foreach (var ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
					m_httpListener.Prefixes.Add(protocol + "://" + ipAddress.ToString() + (usingPortEighty ? "/" : ":" + m_port + "/"));
			}
        }
    }


    private bool HasElevatedPermissions()
    {
        var identity = WindowsIdentity.GetCurrent();

		if(identity == null)
		{
			return false;
		}

        var pricipal = new WindowsPrincipal(identity);
        return pricipal.IsInRole(WindowsBuiltInRole.Administrator);
    }


	private bool IsPortAvailable()
	{
		var tcpConnInfoArray = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();

		return tcpConnInfoArray.All(tcpi => tcpi.LocalEndPoint.Port != m_port);
	}

	private void ProcessExit(object sender, EventArgs e)
	{
		//close http listener if running
		if(m_httpListener != null && m_httpListener.IsListening)
		{
			try
			{
				m_httpListener.Close();
			}
			catch { }
		}
	}


	private string GetProtocol()
	{
		return m_useSsl ? HTTPS : HTTP;
	}

	private const string HTTPS = "https";
	private const string HTTP = "http";
	private readonly bool m_useSsl;
    private readonly int m_port;
    private HttpListener m_httpListener;
}
}
