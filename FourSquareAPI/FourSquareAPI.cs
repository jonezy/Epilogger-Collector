using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for FourSquareAPI
/// </summary>
public class FourSquareAPI
{
    private JavaScriptSerializer m_ser;
    private string m_auth;

    public FourSquareAPI(string auth)
    {
        m_auth = auth;

        m_ser = new JavaScriptSerializer();
        m_ser.RegisterConverters(new JavaScriptConverter[] { new VenueDataConverter() });
    }

    public Venue GetVenueDetails(int venueID)
    {
        string url = string.Format(
            "http://api.foursquare.com/v1/venue.json?vid={0}", venueID);

        string js = HttpFetcher.Get(url, m_auth);
        return m_ser.Deserialize<Venue>(js);
    }

    public IEnumerable<Venue> GetMatchingVenues(decimal lat, decimal lng, string name)
    {
        string url = string.Format(
            "http://api.foursquare.com/v1/venues.json?geolat={0}&geolong={1}&q={2}",
            lat, lng, name);

        string js = HttpFetcher.Get(url, m_auth);
        IEnumerable<VenueGroup> groups = m_ser.Deserialize<IEnumerable<VenueGroup>> (js);

        VenueGroup nearby = (
            from g in groups
            where g.Type == "Matching Places"
            select g).FirstOrDefault();

        return nearby == null ? new Venue[] {} : nearby.Venues;
    }
}

class VenueDataConverter : JavaScriptConverter
{
    public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
    {
        if (type == typeof(IEnumerable<VenueGroup>))
        {
            List<VenueGroup> groups = new List<VenueGroup>();
            if (dictionary.ContainsKey("groups"))
            {
                ArrayList list = dictionary["groups"] as ArrayList;
                foreach (IDictionary<string, object> data in list)
                {
                    groups.Add(new VenueGroup(data));
                }
            }

            return groups;
        }
        else if (type == typeof(Venue))
        {
            return new Venue(dictionary["venue"] as Dictionary<string, object>);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
    {
        // we only need Deserialize
        throw new NotImplementedException();
    }

    public override IEnumerable<Type> SupportedTypes
    {
        get 
        {
            return new Type[] { typeof(Venue), typeof(IEnumerable<VenueGroup>) };
        }
    }
}

public class VenueGroup
{
    public string Type { get; set; }
    public IEnumerable<Venue> Venues { get; set; }

    public VenueGroup(IDictionary<string, object> dict)
    {
        SafeDictionary data = new SafeDictionary(dict);

        Type = data["type"] as string;

        List<Venue> venues = new List<Venue>();
        foreach (IDictionary<string, object> venuedata in data["venues"] as ArrayList)
        {
            venues.Add(new Venue(venuedata));
        }
        Venues = venues;
    }
}

public class User
{
    public int? ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Photo { get; set; }

    public User(IDictionary<string, object> dict)
    {
        SafeDictionary data = new SafeDictionary(dict);

        ID = data["id"] as int?;
        FirstName = data["firstname"] as string;
        LastName = data["lastname"] as string;
        Photo = data["photo"] as string;
    }
}

public class Tip
{
    public int? ID { get; set; }
    public string Text { get; set; }
    public User User { get; set; }

    public Tip(IDictionary<string, object> dict)
    {
        SafeDictionary data = new SafeDictionary(dict);

        ID = data["id"] as int?;
        Text = data["text"] as string;
        User = new User(data["user"] as IDictionary<string, object>);
    }
}

public class Checkin
{
    public int? ID { get; set; }
    public DateTime Created { get; set; }
    public string TimeZone { get; set; }
    public User User { get; set; }

    public Checkin(IDictionary<string, object> dict)
    {
        SafeDictionary data = new SafeDictionary(dict);

        ID = data["id"] as int?;
        Created = DateTime.Parse(data["created"] as string);
        TimeZone = data["timezone"] as string;
        User = new User(data["user"] as IDictionary<string, object>);
    }
}

public class Category
{
    public int? ID { get; set; }
    public string FullPathName { get; set; }
    public string IconURL { get; set; }

    public Category(IDictionary<string, object> dict)
    {
        SafeDictionary data = new SafeDictionary(dict);
        ID = data["id"] as int?;
        FullPathName = data["fullpathname"] as string;
        IconURL = data["iconurl"] as string;
    }
}

public class Venue
{
    public int? ID { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public int? Checkins { get; set; }
    public User Mayor { get; set; }
    public int? MayorCheckins { get; set; }
    public IEnumerable<Tip> Tips { get; set; }
    public IEnumerable<Checkin> HereNow { get; set; }
    public Category PrimaryCategory { get; set; }

    public Venue(IDictionary<string, object> dict)
    {
        SafeDictionary data = new SafeDictionary(dict);

        ID = data["id"] as int?;
        Name = data["name"] as string;
        Address = data["address"] as string;
        City = data["city"] as string;
        State = data["state"] as string;
        Zip = data["zip"] as string;

        PrimaryCategory = new Category(data["primarycategory"] as IDictionary<string, object>);

        SafeDictionary stats = new SafeDictionary(data["stats"]);
        Checkins = stats["checkins"] as int?;

        SafeDictionary mayor = new SafeDictionary(stats["mayor"]);
        MayorCheckins = mayor["count"] as int?;
        Mayor = new User(mayor["user"] as Dictionary<string, object>);

        List<Checkin> checkins = new List<Checkin>();
        if (data["checkins"] != null)
        {
            foreach (IDictionary<string, object> checkindata in data["checkins"] as ArrayList)
            {
                checkins.Add(new Checkin(checkindata));
            }
        }
        HereNow = checkins;

        List<Tip> tips = new List<Tip>();
        if (data["tips"] != null)
        {
            foreach (IDictionary<string, object> tipdata in data["tips"] as ArrayList)
            {
                tips.Add(new Tip(tipdata));
            }
        }
        Tips = tips;
    }
}