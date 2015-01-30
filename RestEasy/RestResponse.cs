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
        m_handled = false;
    }


    public void Send(string content)
    {
        ErrorOnDoubleHandle();

        m_httpResponse.AddHeader("Content-Type", "text/plain");

        byte[] buf = Encoding.UTF8.GetBytes(content);
        m_httpResponse.ContentLength64 = buf.Length;
        m_httpResponse.OutputStream.Write(buf, 0, buf.Length);

    }


    public void SendHtml(string html)
    {        
        ErrorOnDoubleHandle();

        m_httpResponse.AddHeader("Content-Type", "text/html");

        byte[] buf = Encoding.UTF8.GetBytes(html);
        m_httpResponse.ContentLength64 = buf.Length;
        m_httpResponse.OutputStream.Write(buf, 0, buf.Length);
    }

    public void SendFile(string fileName, byte[] content)
    {
        ErrorOnDoubleHandle();

        m_httpResponse.AddHeader("Content-Type", "application/octet-stream");
        m_httpResponse.AddHeader("Content-Disposition", "attachment; filename="+fileName);
        m_httpResponse.AddHeader("Content-Transfer-Encoding", "binary");
        m_httpResponse.AddHeader("Accept-Ranges", "bytes");

        m_httpResponse.ContentLength64 = content.Length;
        m_httpResponse.OutputStream.Write(content, 0, content.Length);
    }

    private void ErrorOnDoubleHandle()
    {
        if (m_handled)
            throw new Exception("Response already handled!");
		m_handled = true;
    }

    private HttpListenerResponse m_httpResponse;
    private bool m_handled;
}
}
