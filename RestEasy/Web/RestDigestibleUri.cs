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

        if (m_nodes[m_position].Contains('?'))
        {
            return m_nodes[m_position].Substring(0, m_nodes[m_position].IndexOf('?'));
        }
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

	public bool IsWildCardNodeDefinition
	{
		get
		{
			return GetCurrentNode() == ASTRISK;
		}
	}

    public bool ContainsWildCardNodeDefinition
    {
        get
        {
            if (m_nodes == null || m_nodes.Length == 0)
                return false;

            for (int i = 0; i < m_nodes.Length; i++)
            {

                if (m_nodes[i].Contains('?'))
                {
                    if(m_nodes[i].Substring(0, m_nodes[i].IndexOf('?')) == ASTRISK)
                        return true;
                }
            }
            return false;
               
        }
    }

	
	private const string ASTRISK = "*";
    private int m_position;
    private string[] m_nodes;
}
}
