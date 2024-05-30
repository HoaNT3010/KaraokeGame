using System;
using System.Text.RegularExpressions;
using System.Web;
using UnityEngine;

namespace KaraokeGame.Utilities
{
    public static class YouTubeUrlHelper
    {
        public const string YOUTUBE_PRIMARY_DOMAIN = "www.youtube.com";
        public const string YOUTUBE_SHORTENED_DOMAIN = "youtu.be";
        private static readonly Regex YouTubeIdRegex = new Regex(@"^[a-zA-Z0-9_-]{11}$", RegexOptions.Compiled);

        /// <summary>
        /// Extracts the video ID from a YouTube URL.
        /// </summary>
        /// <param name="url">The YouTube video URL.</param>
        /// <returns>The video ID or null if the URL is invalid.</returns>
        public static string ExtractVideoIdFromUrl(string url)
        {
            try
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
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }

        public static bool ValidateVideoId(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
            {
                return false;
            }

            return YouTubeIdRegex.IsMatch(videoId);
        }
    }
}
