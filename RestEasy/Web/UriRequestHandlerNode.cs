using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.Web
{
internal abstract class UriRequestHandlerNode : IUriRequestHandlerNode
{

	protected UriRequestHandlerNode()
	{
		ChildNodes = new List<IUriRequestHandlerNode>();
	}

	public RestRequestHandler HttpGetRequestHandler
	{
		get;
		set;
	}

	public RestRequestHandler HttpPutRequestHandler
	{
		get;
		set;
	}

	public RestRequestHandler HttpPostRequestHandler
	{
		get;
		set;
	}

	public RestRequestHandler HttpDeleteRequestHandler
	{
		get;
		set;
	}


	public RestRequestHandler GetRestRequestHandler(string uri, RestMethod method)
	{
		if(IsLastNode(uri))
		{
			switch (method)
			{
				case RestMethod.GET:
					return HttpGetRequestHandler;
				case RestMethod.PUT:
					return HttpPutRequestHandler;
				case RestMethod.POST:
					return HttpPostRequestHandler;
				case RestMethod.DELETE:
					return HttpDeleteRequestHandler;
				default:
					throw new ApplicationException("Unknown REST method.");
			}
		}

		var nextUriPart = StripUri(uri);

		foreach (var childNode in ChildNodes)
		{
			if (childNode.MatchesUriPattern(nextUriPart))
			{
				return childNode.GetRestRequestHandler(nextUriPart, method);
			}
		}

		return null;
	}

	public void AddRestRequestHandler(string uri, RestMethod method, RestRequestHandler handler)
	{

	}

	
	public abstract bool MatchesUriPattern(string uri);

	protected abstract bool IsLastNode(string uri);

	protected abstract string StripUri(string uri);

	protected ICollection<IUriRequestHandlerNode> ChildNodes;


}
}
