using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestEasy.IO;

namespace RestEasy
{
public class RestResponse
{

    internal RestResponse(HttpListenerResponse httpResponse)
    {
        m_httpResponse = httpResponse;
        m_handled = false;
    }
	
	public WebHeaderCollection Headers
	{
		get
		{
			return m_httpResponse.Headers;
		}
		set
		{
			m_httpResponse.Headers = value;
		}
	}

	public void Send()
	{
        ErrorOnDoubleHandle();
		
		m_httpResponse.StatusCode = 204;

	}

    public void Send(string content)
    {
        ErrorOnDoubleHandle();

        m_httpResponse.AddHeader("Content-Type", "text/plain");
		m_httpResponse.StatusCode = 200;

        byte[] buf = Encoding.UTF8.GetBytes(content);
        m_httpResponse.ContentLength64 = buf.Length;
        m_httpResponse.OutputStream.Write(buf, 0, buf.Length);
    }


	public void Send(IRestResourceFile file)
	{
        ErrorOnDoubleHandle();

		m_httpResponse.AddHeader("Content-Type", file.ContentType);
		
		m_httpResponse.StatusCode = 200;

		byte[] allBytes;

		if (file.ShouldSendAsBinary)
		{
			allBytes = file.GetAllBytes();
			m_httpResponse.AddHeader("Content-Disposition", "inline; filename="+file.FileName);
			m_httpResponse.AddHeader("Content-Transfer-Encoding", "binary");
			m_httpResponse.AddHeader("Accept-Ranges", "bytes");

			m_httpResponse.ContentLength64 = allBytes.Length;
			m_httpResponse.OutputStream.Write(allBytes, 0, allBytes.Length);
		}
		else
		{
			allBytes = Encoding.UTF8.GetBytes(file.GetAllText());
			m_httpResponse.ContentLength64 = allBytes.Length;
			m_httpResponse.OutputStream.Write(allBytes, 0, allBytes.Length);
		}

	}

	public void NotFound()
	{
        ErrorOnDoubleHandle();

		m_httpResponse.StatusCode = 404;
	}


	public void Created(string content)
	{
        ErrorOnDoubleHandle();

		m_httpResponse.StatusCode = 201;
	}
	
	public void BadRequest()
	{
        ErrorOnDoubleHandle();

		m_httpResponse.StatusCode = 400;
	}
	
	public void UseCached()
	{
        ErrorOnDoubleHandle();

		m_httpResponse.StatusCode = 304;
	}

	
	public void NotAuthenticated()
	{
        ErrorOnDoubleHandle();

		m_httpResponse.StatusCode = 401;
	}

	public void NotAuthorized()
	{
        ErrorOnDoubleHandle();

		m_httpResponse.StatusCode = 403;
	}

	public void NotAuthorized(string content)
	{
        ErrorOnDoubleHandle();

        m_httpResponse.AddHeader("Content-Type", "text/plain");
		m_httpResponse.StatusCode = 403;

        byte[] buf = Encoding.UTF8.GetBytes(content);
        m_httpResponse.ContentLength64 = buf.Length;
        m_httpResponse.OutputStream.Write(buf, 0, buf.Length);
	}

	public void Conflict()
	{
        ErrorOnDoubleHandle();

		m_httpResponse.StatusCode = 409;
	}

	public void Conflict(string content)
	{
        ErrorOnDoubleHandle();

        m_httpResponse.AddHeader("Content-Type", "text/plain");
		m_httpResponse.StatusCode = 409;

        byte[] buf = Encoding.UTF8.GetBytes(content);
        m_httpResponse.ContentLength64 = buf.Length;
        m_httpResponse.OutputStream.Write(buf, 0, buf.Length);
	}

	public void InternalError()
	{
        ErrorOnDoubleHandle();

		m_httpResponse.StatusCode = 500;
	}

	public void InternalError(string content)
	{
        ErrorOnDoubleHandle();

        m_httpResponse.AddHeader("Content-Type", "text/plain");
		m_httpResponse.StatusCode = 500;

        byte[] buf = Encoding.UTF8.GetBytes(content);
        m_httpResponse.ContentLength64 = buf.Length;
        m_httpResponse.OutputStream.Write(buf, 0, buf.Length);
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
