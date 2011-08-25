﻿using System;
using Hammock;
using Hammock.Streaming;
using Hammock.Web;

namespace TweetSharp
{
    partial class TwitterService
    {
        private readonly RestClient _userStreamingClient;

        /// <summary>
        /// Cancels pending streaming actions from this service.
        /// </summary>
        public virtual void CancelStreaming()
        {
            if(_userStreamingClient != null)
            {
                _userStreamingClient.CancelStreaming();
            }
        }


        /// <summary>
        /// Accesses an asynchronous Twitter user stream indefinitely, until terminated.
        /// </summary>
        /// <seealso href="http://dev.twitter.com/pages/user_streams" />
        /// <param name="action"></param>
        /// <returns></returns>
#if !WINDOWS_PHONE
        public virtual IAsyncResult StreamUser(Action<TwitterUserStreamArtifact, TwitterResponse> action)
#else
        public void StreamUser(Action<TwitterUserStreamArtifact, TwitterResponse> action)
#endif
        {
            var options = new StreamOptions { ResultsPerCallback = 1 };

#if !WINDOWS_PHONE
            return 
#endif
            WithHammockUserStreaming(options, action, "user.json");
        }

#if !WINDOWS_PHONE
        private IAsyncResult WithHammockUserStreaming<T>(StreamOptions options, Action<T, TwitterResponse> action, string path) where T : class
#else
        private void WithHammockUserStreaming<T>(StreamOptions options, Action<T, TwitterResponse> action, string path) where T : class
#endif
        {
            var request = PrepareHammockQuery(path);
#if !WINDOWS_PHONE
            return 
#endif 
            WithHammockStreamingImpl(_userStreamingClient, request, options, action);
        }

#if !WINDOWS_PHONE
        private IAsyncResult WithHammockStreamingImpl<T>(RestClient client, RestRequest request, StreamOptions options, Action<T, TwitterResponse> action)
#else
        private static void WithHammockStreamingImpl<T>(RestClient client, RestRequest request, StreamOptions options, Action<T, TwitterResponse> action)
#endif
        {
            request.StreamOptions = options;
            request.Method = WebMethod.Get;
#if SILVERLIGHT
            request.AddHeader("X-User-Agent", client.UserAgent); 
#endif
            
#if !WINDOWS_PHONE
            return
#endif
            client.BeginRequest(request, new RestCallback<T>((req, response, state) =>
                                                 {
                                                     if (response == null)
                                                     {
                                                         return;
                                                     }
#if !SILVERLIGHT
                                                     SetResponse(response);
#endif
                                                     var entity = response.ContentEntity;
                                                     action.Invoke(entity, new TwitterResponse(response));
                                                 }));
        }
    }
}
