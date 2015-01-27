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
	internal RestRequest(HttpListenerRequest httpListenerRequest)
	{
		m_httpListenerRequest = httpListenerRequest;
	}


	private HttpListenerRequest m_httpListenerRequest;
}
}
