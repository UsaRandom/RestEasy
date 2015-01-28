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

        foreach (var key in m_httpListenerRequest.QueryString.AllKeys)
        {
            if (parameters[key] != null)
            {
                throw new Exception("Parameters of same name provided in request");
            }
            parameters[key] = m_httpListenerRequest.QueryString[key];
        }
	}


    public RestRequestParameters Parameters
    {
        get;
        private set;
    }


	private HttpListenerRequest m_httpListenerRequest;
}
}
