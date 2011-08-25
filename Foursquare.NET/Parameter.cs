namespace FoursquareNET
{
    internal class Parameter
    {

        //public Parameter(string key)
        //{
        //    Key = key;
        //}

        public Parameter(string alias, string key, bool isRequired)
        {
            Key = key;
            _Required = isRequired;
            _Alias = alias;
        }

        override public string ToString()
        {
            return !string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(Value)
                       ? string.Format("{0}={1}", Key, Value)
                       : string.Empty;
        }

        public string Key { get; set; }
        public string Value { get; set; }

        private bool _Required;
        public bool Required { get { return _Required; } }

        private string _Alias;
        public string Alias { get { return _Alias; } }
    }
}
