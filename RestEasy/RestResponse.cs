using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy
{
public class RestResponse
{

    internal RestResponse(HttpListenerResponse httpResponse)
    {
        m_httpResponse = httpResponse;
    }


    private HttpListenerResponse m_httpResponse;
}
}
