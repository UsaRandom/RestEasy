using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.Web
{
internal class ParameterUriRequestHandlerNode: UriRequestHandlerNode
{
    public ParameterUriRequestHandlerNode(RestDigestibleUri uri, RestMethod method, RestRequestHandler handler)
    {
        m_parameterPattern = uri.GetCurrentNode();
        m_parameterName = m_parameterPattern.Replace("]", string.Empty).Replace("[", string.Empty);

        AddRestRequestHandler(uri, method, handler);
    }



    public override bool MatchesUriPattern(RestDigestibleUri uri)
    {
        if (uri.GetCurrentNode().Length != 0)
            return true;
        return false;
    }

    protected override int GetSearchPriority() {
        return 1;
    }
    
    protected override void HandleParameters(RestDigestibleUri uri, ref RestRequestParameters parameters)
    {
        parameters[m_parameterName] = uri.GetCurrentNode();
    }

    private readonly string m_parameterName;
    private readonly string m_parameterPattern;


}
}
