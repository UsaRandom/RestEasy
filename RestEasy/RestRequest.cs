using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy
{
public class RestRequest
{
	internal RestRequest(HttpListenerRequest httpListenerRequest, RestRequestParameters parameters)
	{
		m_httpListenerRequest = httpListenerRequest;
        Parameters = parameters;
	}


    public RestRequestParameters Parameters
    {
        get;
        private set;
    }

	private HttpListenerRequest m_httpListenerRequest;
}
}
