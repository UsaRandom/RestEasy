using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.Web
{
internal class WildCardUriRequestHandlerNode : UriRequestHandlerNode
{
    public WildCardUriRequestHandlerNode(RestDigestibleUri uri, RestMethod method, RestRequestHandler handler)
    {
        AddRestRequestHandler(uri, method, handler);
    }

    public override bool MatchesUriPattern(RestDigestibleUri uri)
    {
        return true;

    }

    protected override int GetSearchPriority()
    {
        return 2;
    }

    protected override void HandleParameters(RestDigestibleUri uri, RestRequestParameters parameters)
    {    }

	private const string ASTRISK = "*";
}
}
