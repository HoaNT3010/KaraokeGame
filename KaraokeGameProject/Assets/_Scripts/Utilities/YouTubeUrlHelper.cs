using System;
using System.Web;
using UnityEngine;

namespace KaraokeGame.Utilities
{
    public static class YouTubeUrlHelper
    {
        public const string YOUTUBE_PRIMARY_DOMAIN = "www.youtube.com";
        public const string YOUTUBE_SHORTENED_DOMAIN = "youtu.be";

        /// <summary>
        /// Extracts the video ID from a YouTube URL.
        /// </summary>
        /// <param name="url">The YouTube video URL.</param>
        /// <returns>The video ID or null if the URL is invalid.</returns>
        public static string ExtractVideoIdFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                // Handle invalid input (null or empty URL)
                Debug.Log("The given url is invalid (Null or empty).");
                return null;
            }
            Uri uri = new Uri(url);
            if (uri.Host == YOUTUBE_PRIMARY_DOMAIN)
            {
                // For primary YouTube URLs (www.youtube.com)
                var query = HttpUtility.ParseQueryString(uri.Query);
                return query["v"];
            }
            else if (uri.Host == YOUTUBE_SHORTENED_DOMAIN)
            {
                // For shortened YouTube URLs (youtu.be)
                var segments = uri.Segments;
                if (segments.Length > 1)
                {
                    // Extract the video ID from the path segment
                    return segments[1].TrimEnd('/');
                }
            }
            // Invalid URL (not a YouTube host)
            Debug.Log("The given url is invalid (Not an url with YouTube host).");
            return null;
        }
    }
}
