using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class WWWHttps {

    public string text;
    public bool isDone;
    public string error;

	public WWWHttps(string url, NameValueCollection form)
    {
        PostForm(url, form);
    }

    void PostForm(string url, NameValueCollection form)
    {
        ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.ContentType = "application/x-www-form-urlencoded";
        request.Method = form == null? "GET": "POST";

        StringBuilder postVars = new StringBuilder();
        foreach (string key in form)
            postVars.AppendFormat("{0}={1}&", key, form[key]);
        postVars.Length -= 1; // clip off the remaining &

        //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //{
        //    streamWriter.Write(postVars.ToString());
        //    text = streamWriter.ToString();
        //}

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        text = reader.ReadToEnd();

        isDone = true;
    }

    private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        Debug.Log("TrustCertificate");
        // all Certificates are accepted
        return true;
    }
}
