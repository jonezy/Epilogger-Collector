using System;
using System.Collections.Generic;

namespace TweetSharp
{
    /// <summary>
    /// Defines a contract for a <see cref="TwitterService" /> implementation.
    /// </summary>
    /// <seealso href="http://dev.twitter.com/doc" />
    public partial interface ITwitterService
    {
#if !WINDOWS_PHONE
        IAsyncResult StreamFilter(int count, bool delimited, string follow, string track, Action<IEnumerable<TwitterStatus>, TwitterResponse> action);
        IAsyncResult StreamFirehose(int count, bool delimited, Action<IEnumerable<TwitterStatus>, TwitterResponse> action);
        IAsyncResult StreamRetweet(bool delimited, Action<IEnumerable<TwitterStatus>, TwitterResponse> action);
        IAsyncResult StreamSample(int count, bool delimited, Action<IEnumerable<TwitterStatus>, TwitterResponse> action);
#else
        void StreamFilter(int count, bool delimited, string follow, string track, Action<IEnumerable<TwitterStatus>, TwitterResponse> action);
        void StreamFirehose(int count, bool delimited, Action<IEnumerable<TwitterStatus>, TwitterResponse> action);
        void StreamRetweet(bool delimited, Action<IEnumerable<TwitterStatus>, TwitterResponse> action);
        void StreamSample(int count, bool delimited, Action<IEnumerable<TwitterStatus>, TwitterResponse> action);
#endif
    }
}