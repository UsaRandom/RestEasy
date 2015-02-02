using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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


	public byte[] BodyAsBytes
	{
		get
		{
			if(m_requestBodyInBytes != null)
				return m_requestBodyInBytes;

			m_requestBodyInBytes = new byte[m_httpListenerRequest.ContentLength64];
			
			m_httpListenerRequest.InputStream.Read(m_requestBodyInBytes, 0, m_requestBodyInBytes.Length);

			return m_requestBodyInBytes;
		}
	}
	
	public string Body
	{
		get
		{
			return Encoding.UTF8.GetString(BodyAsBytes);
		}
	}

	public string ContentType
	{
		get
		{
			return m_httpListenerRequest.ContentType;
		}
	}

	public NameValueCollection Headers
	{
		get
		{
			return m_httpListenerRequest.Headers;
		}
	}

	public string Url
	{
		get
		{
			return m_httpListenerRequest.RawUrl;
		}
	}

	public IPAddress ClientIpAddress
	{
		get
		{
			return m_httpListenerRequest.RemoteEndPoint.Address;
		}
	}


    public RestRequestParameters Parameters
    {
        get;
        private set;
    }


	private byte[] m_requestBodyInBytes;
	private HttpListenerRequest m_httpListenerRequest;
}
}
