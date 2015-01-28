using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.Web
{
internal class NamedUriRequestHandlerNode : UriRequestHandlerNode
{
    public NamedUriRequestHandlerNode(RestDigestibleUri uri, RestMethod method, RestRequestHandler handler)
    {
        m_nodeName = uri.GetCurrentNode();

        AddRestRequestHandler(uri, method, handler);
    }

    public override bool MatchesUriPattern(RestDigestibleUri uri)
    {
        return uri.GetCurrentNode() == m_nodeName;

    }

    protected override int GetSearchPriority()
    {
        return 0;
    }

    protected override void HandleParameters(RestDigestibleUri uri, ref RestRequestParameters parameters)
    {    }

    private readonly string m_nodeName;
}
}
