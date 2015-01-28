using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.Web
{
internal class RestDigestibleUri
{
    public RestDigestibleUri(string uri)
    {
        m_nodes = uri.Split('/');

        m_position = 0;

    }

    public string GetCurrentNode()
    {
        if (m_nodes.Length == 0)
            return string.Empty;

        return m_nodes[m_position];
    }


    public void NextNode()
    {
        if (IsLastNode)
            throw new Exception("No more nodes on Uri");

        m_position++;
    }

    public bool IsLastNode
    {
        get
        {
            return m_position == m_nodes.Length - 1 || m_nodes.Length == 0;
        }
    }

    public bool IsCurrentNodeParameterDefinition
    {
        get
        {
            return GetCurrentNode().StartsWith("[") && GetCurrentNode().EndsWith("]");
        }
    }

    public IList<KeyValuePair<string, dynamic>> GetParameters
    {
        get
        {
            throw new NotImplementedException();
        }
    }


    private int m_position;
    private string[] m_nodes;
}
}
