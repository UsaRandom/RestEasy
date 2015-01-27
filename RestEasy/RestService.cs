using RestEasy.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy
{

public delegate void RestRequestHandler(RestRequest request, RestResponse response);
public delegate void RestErrorHandler(RestService restService, EventArgs eventArgs);

public class RestService
{

    public event RestErrorHandler Error;

    public void Register(RestMethod method, string uri, RestRequestHandler handler)
    {
        //check if running

        //validate uri

        //place in bucket

    }


    public void Listen(int port)
    {
        if (IsListening)
            throw new ApplicationException("RestService is already listening");

        IsListening = true;

        m_httpServer = new RestHttpServer(port);

        m_httpServer.Error += OnServerError;
        m_httpServer.Request += OnHttpRequest;
    
    }

    void
    OnHttpRequest(HttpListenerRequest httpRequest, HttpListenerResponse httpResponse)
    {
        throw new NotImplementedException();
    }

    void
    OnServerError(RestHttpServer restHttpServer, Exception exception)
    {
        throw new NotImplementedException();
    }


    public bool IsListening
    {
        get
        {
            return m_isListening;
        }
        private set
        {
            m_isListening = value;
        }
    }

    private RestHttpServer m_httpServer;

    private bool m_isListening = false;
}
}
