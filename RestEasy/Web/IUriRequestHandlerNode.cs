using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.Web
{
internal interface IUriRequestHandlerNode
{

	RestRequestHandler HttpGetRequestHandler
	{
		get;
		set;
	}
	RestRequestHandler HttpPutRequestHandler
	{
		get;
		set;
	}

	RestRequestHandler HttpPostRequestHandler
	{
		get;
		set;
	}

	RestRequestHandler HttpDeleteRequestHandler
	{
		get;
		set;
	}


	bool MatchesUriPattern(string uri);

	RestRequestHandler GetRestRequestHandler(string uri, RestMethod method);
 
	void AddRestRequestHandler(string uri, RestMethod method, RestRequestHandler handler);
}
}
