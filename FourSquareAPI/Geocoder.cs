using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for Geocoder
/// </summary>
public class Geocoder
{
    public static string FormatAddress(string city, string state, string zip)
    {
        string address = "";
        if (city != "")
        {
            address = city;
        }
        if (state != "")
        {
            address += " " + state;
        }
        if (zip != "")
        {
            address += " " + zip;
        }
        return address.Trim();
    }

    public static void Geocode(string city, string state, string zip, out decimal lat, out decimal lng)
    {
        string address = FormatAddress(city, state, zip);

        string geocodeURL = string.Format(
            "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false",
            address);

        string xml = HttpFetcher.Get(geocodeURL);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        XmlNode root = doc.DocumentElement;

        lat = Convert.ToDecimal(GetNodeText(root, "result/geometry/location/lat"));
        lng = Convert.ToDecimal(GetNodeText(root, "result/geometry/location/lng"));
    }

    private static string GetNodeText(XmlNode node, string xpath)
    {
        XmlNode n = node.SelectSingleNode(xpath);
        return n == null ? null : n.InnerText;
    }
}
