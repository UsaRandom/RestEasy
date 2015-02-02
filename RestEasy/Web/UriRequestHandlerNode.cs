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


	public RestRequestHandler HttpPostRequestHandler
	{
		get;
		set;
	}



	public RestRequestHandler GetRestRequestHandler(RestDigestibleUri uri, RestMethod method, RestRequestParameters parameters)
	{
        HandleParameters(uri, parameters);

		if(uri.IsLastNode || this is WildCardUriRequestHandlerNode)
		{
			switch (method)
			{
				case RestMethod.GET:
					return HttpGetRequestHandler;
				case RestMethod.POST:
					return HttpPostRequestHandler;
				default:
					throw new ApplicationException("Unknown REST method.");
			}
		}

        uri.NextNode();

		foreach (var childNode in ChildNodes)
		{
			if (childNode.MatchesUriPattern(uri))
			{
				return childNode.GetRestRequestHandler(uri, method, parameters);
			}
		}

		return null;
	}

    public void AddRestRequestHandler(RestDigestibleUri uri, RestMethod method, RestRequestHandler handler)
	{
        if (uri.IsLastNode || this is WildCardUriRequestHandlerNode)
        {
            switch (method)
            {
                case RestMethod.GET:
                    if (HttpGetRequestHandler != null)
                        throw new Exception("Handler already defined");
                    HttpGetRequestHandler = handler;
                    return;
                case RestMethod.POST:
                    if (HttpPostRequestHandler != null)
                        throw new Exception("Handler already defined");
                    HttpPostRequestHandler = handler;
                    return;
                default:
                    throw new Exception("Unknown REST Method.");
            }
        }

        uri.NextNode();

        foreach (var childNode in ChildNodes)
        {
            if (childNode.MatchesUriPattern(uri) && ((childNode is ParameterUriRequestHandlerNode && uri.IsCurrentNodeParameterDefinition) ||
                                                      childNode is NamedUriRequestHandlerNode && !uri.IsCurrentNodeParameterDefinition))
            {
                childNode.AddRestRequestHandler(uri, method, handler);
                return;
            }
        }

        UriRequestHandlerNode newChildNode;
		
		if (uri.IsCurrentNodeParameterDefinition)
			newChildNode = new ParameterUriRequestHandlerNode(uri, method, handler);
		else if (uri.IsWildCardNodeDefinition)
			newChildNode = new WildCardUriRequestHandlerNode(uri, method, handler);
		else
			newChildNode = new NamedUriRequestHandlerNode(uri, method, handler);

        ChildNodes.Add(newChildNode);

        ChildNodes = ChildNodes.OrderBy(n => n.GetSearchPriority()).ToList();
	}


    public abstract bool MatchesUriPattern(RestDigestibleUri uri);

    protected abstract void HandleParameters(RestDigestibleUri uri, RestRequestParameters parameters);

    protected abstract int GetSearchPriority();
    

    protected ICollection<UriRequestHandlerNode> ChildNodes;


}
}
