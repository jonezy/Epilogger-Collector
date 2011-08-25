using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Safely wraps an IDictionary<string, object> so that getting a key that doesn't
/// exist doesn't throw.  Also works if the object passed in isn't a dictionary
/// or is null.  Will just return null values for all keys in this case.
/// </summary>
public class SafeDictionary
{
    private IDictionary<string, object> m_dict;

    public SafeDictionary(object dictionary)
    {
        m_dict = dictionary as IDictionary<string, object>;
    }

    public object this[string key]
    {
        get
        {
            if (m_dict != null && m_dict.ContainsKey(key))
            {
                return m_dict[key];
            }

            return null;
        }
    }
}
