using UnityEngine;
using System.Collections;
using System.IO;  
using System.Xml;  
using System.Linq;  
using System.Text;  
using System.Collections.Generic;


public class AnyXml  {

	public static string dataPath="";
	public  AnyXml()
	{
		XMLEnter ();
	}
	public virtual void XMLStart()
	{

	}
	public virtual void XMLProceed(XmlElement node)
	{
		
	}
	public void XMLEnter()
	{
		XMLStart ();
		if (File.Exists (dataPath)) {
			XmlDocument xmlDoc = new XmlDocument();  
			xmlDoc.Load(dataPath);  
			XmlNodeList node = xmlDoc.SelectSingleNode("datas").ChildNodes;  
			foreach (XmlElement nodeList in node) {  
				XMLProceed(nodeList);

			}
		}

	}
}
