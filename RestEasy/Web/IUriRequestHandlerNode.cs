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

	RestRequestHandler HttpPostRequestHandler
	{
		get;
		set;
	}



	bool MatchesUriPattern(RestDigestibleUri uri);

	RestRequestHandler GetRestRequestHandler(RestDigestibleUri uri, RestMethod method, ref RestRequestParameters parameters);

    void AddRestRequestHandler(RestDigestibleUri uri, RestMethod method, RestRequestHandler handler);
}
}
