using RestEasy.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy
{

public delegate void RestInfoMessageHandler(RestService restService, string message);
public delegate void RestRequestHandler(RestRequest request, RestResponse response);
public delegate void RestErrorHandler(RestService restService, Exception exception);

public class RestService
{
    public event RestErrorHandler Error;
    public event RestInfoMessageHandler Message;

	public RestService(int port, bool useSsl, params string[] domains)
	{
		IsListening = false;
		IgnoreFaviconRequests = false;
		m_requestTree = new RestRequestTree();
        UsingSsl = useSsl;

        m_httpServer = new RestHttpServer(port, useSsl, domains);

        m_httpServer.Message += OnServerMessage;
        m_httpServer.Error += OnServerError;
        m_httpServer.Request += OnHttpRequest;
	}

    /*
    //TODO: need a decent way for developers to provide certificates...
    public string SslCertificateFriendlyName
    {
        get
        {
            return m_httpServer.SslCertificateFriendlyName;
        }
        set
        {
            m_httpServer.SslCertificateFriendlyName = value;
        }
    }
    */

    public bool UsingSsl
    {
        get;
        private set;
    }

    public void Register(RestMethod method, string uri, RestRequestHandler handler)
    {
        //check if running
		if (IsListening)
			throw new ApplicationException("RestService is listening, cannot register new methods");

        m_requestTree.AddRequestHandler(uri, method, handler);

        SendMessage("[" + method + "] Registered Uri: " + uri);
    }


    public void Listen()
    {
        if (IsListening)
            throw new ApplicationException("RestService is already listening");

        IsListening = true;


        m_httpServer.Listen();

        SendMessage("Rest Service started listening.");
    }

	public bool IsListening { get; private set; }

	public bool IgnoreFaviconRequests
	{
		get;
		set;
	}

    private void OnHttpRequest(HttpListenerRequest httpRequest, HttpListenerResponse httpResponse)
    {
		var url = httpRequest.RawUrl;

		if(url == FAVICON_URL && IgnoreFaviconRequests)
			return;
		
        RestRequestParameters parameters;

        var method = HttpMethodToRestMethod(httpRequest.HttpMethod);

        var handler = m_requestTree.GetRequestHandler(url, method, out parameters);

		if (handler == null)
		{
			//resource not found
			httpResponse.StatusCode = 404;
			return;
		}

        handler(new RestRequest(httpRequest, parameters), new RestResponse(httpResponse));
    }

    private void OnServerError(RestHttpServer restHttpServer, Exception exception)
    {
        if(Error != null)
		    Error(this, exception);
    }

    private void OnServerMessage(RestHttpServer restHttpServer, string message)
    {
       SendMessage(message);
    }

    private void SendMessage(string message)
    {
        if (Message != null && message != null)
            Message(this, message);
    }

    private RestMethod HttpMethodToRestMethod(string method)
    {
        switch (method)
        {
            case "GET":
                return RestMethod.GET;
            case "POST":
                return RestMethod.POST;
            default:
                throw new Exception("Bad http method");
        }
    }

	private RestHttpServer m_httpServer;
	private RestRequestTree m_requestTree;

	private const string FAVICON_URL = "/favicon.ico";
}
}
