namespace FoursquareNET.Exception
{
    public class FoursquareException : System.Exception
    {
        public FoursquareException(string message) 
            : base(message)
        {
        }
    }

    public class FoursquareWebException : System.Net.WebException
    {
        public FoursquareWebException(string message)
            : base(message)
        {
        }
    }

    public class RequiredParameterException : FoursquareException
    {
        public RequiredParameterException(string parameter) 
            : base(string.Format("Missing required parameter: {0}", parameter))
        {
        }
    }

    public class OptionalParameterException : FoursquareException
    {
        public OptionalParameterException(string message)
            : base(message)
        {
        }
    }

    public class WebException : FoursquareWebException
    {
        public WebException(string message)
            : base(message)
        {

        }
    }

    public class RateLimitedException : FoursquareWebException
    {
        public RateLimitedException(string message)
            : base(message)
        {

        }
    }

    public class UnauthorizedException : FoursquareWebException
    {
        public UnauthorizedException(string message)
            : base(message)
        {

        }
    }
}
