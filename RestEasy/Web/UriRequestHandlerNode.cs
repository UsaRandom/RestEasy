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
        ChildNodes = new List<UriRequestHandlerNode>();
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


	public RestRequestHandler GetRestRequestHandler(RestDigestibleUri uri, RestMethod method, ref RestRequestParameters parameters)
	{
        HandleParameters(uri, ref parameters);

		if(uri.IsLastNode)
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

        uri.NextNode();

		foreach (var childNode in ChildNodes)
		{
			if (childNode.MatchesUriPattern(uri))
			{
				return childNode.GetRestRequestHandler(uri, method, ref parameters);
			}
		}

		return null;
	}

    public void AddRestRequestHandler(RestDigestibleUri uri, RestMethod method, RestRequestHandler handler)
	{
        if (uri.IsLastNode)
        {
            switch (method)
            {
                case RestMethod.GET:
                    if (HttpGetRequestHandler != null)
                        throw new Exception("Handler already defined");
                    HttpGetRequestHandler = handler;
                    return;
                case RestMethod.PUT:
                    if (HttpPutRequestHandler != null)
                        throw new Exception("Handler already defined");
                    HttpPutRequestHandler = handler;
                    return;
                case RestMethod.POST:
                    if (HttpPostRequestHandler != null)
                        throw new Exception("Handler already defined");
                    HttpPostRequestHandler = handler;
                    return;
                case RestMethod.DELETE:
                    if (HttpDeleteRequestHandler != null)
                        throw new Exception("Handler already defined");
                    HttpDeleteRequestHandler = handler;
                    return;
                default:
                    throw new Exception("Unknown REST Method.");
            }
        }

        uri.NextNode();

        foreach (var childNode in ChildNodes)
        {
            if (childNode.MatchesUriPattern(uri))
            {
                childNode.AddRestRequestHandler(uri, method, handler);
                return;
            }
        }

        var newChildNode = uri.IsCurrentNodeParameterDefinition ? (UriRequestHandlerNode) new ParameterUriRequestHandlerNode(uri, method, handler) :
                                                        (UriRequestHandlerNode) new NamedUriRequestHandlerNode(uri, method, handler);

        ChildNodes.Add(newChildNode);

        ChildNodes = ChildNodes.OrderBy(n => n.GetSearchPriority()).ToList();
	}


    public abstract bool MatchesUriPattern(RestDigestibleUri uri);

    protected abstract void HandleParameters(RestDigestibleUri uri, ref RestRequestParameters parameters);

    protected abstract int GetSearchPriority();
    

    protected ICollection<UriRequestHandlerNode> ChildNodes;


}
}
