using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TweetSharp
{
    /// <summary>
    /// The results of a request to the Search API.
    /// </summary>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("{ResultsPerPage} results on page {Page} from {Source}")]
    [JsonObject(MemberSerialization.OptIn)]

    public class ATwitterSearchResult : ITwitterModel
    {
        [JsonProperty("results")]
        [DataMember]
        public virtual IEnumerable<TwitterSearchStatus> Statuses { get; set; }

        [DataMember]
        public virtual long SinceId { get; set; }

        [DataMember]
        public virtual long MaxId { get; set; }


        [DataMember]
        public virtual string RefreshUrl { get; set; }

        [DataMember]
        public virtual int ResultsPerPage { get; set; }


        [DataMember]
        public virtual string NextPage { get; set; }


        [DataMember]
        public virtual string PreviousPage { get; set; }


        [DataMember]
        public virtual double CompletedIn { get; set; }


        [DataMember]
        public virtual int Page { get; set; }

        [DataMember]
        public virtual string Query { get; set; }

        [DataMember]
        public virtual string Warning { get; set; }

        [DataMember]
        public virtual string Source { get; set; }

        [DataMember]
        public virtual int Total { get; set; }

        /// <summary>
        /// The source content used to deserialize the model entity instance.
        /// Can be XML or JSON, depending on the endpoint used.
        /// </summary>
        [DataMember]
        public virtual string RawSource { get; set; }
    }
}