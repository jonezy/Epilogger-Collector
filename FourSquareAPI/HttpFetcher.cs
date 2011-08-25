using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for HttpFetcher
/// </summary>
public class HttpFetcher
{
    public static string Get(string url)
    {
        return Get(url, null);
    }

    public static string Get(string url, string auth)
    {
        HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(url);

        if (auth != null)
        {
            req.Headers.Add("Authorization", auth);
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (StreamReader sr = new StreamReader(ex.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }

            return "";
        }
    }
}
