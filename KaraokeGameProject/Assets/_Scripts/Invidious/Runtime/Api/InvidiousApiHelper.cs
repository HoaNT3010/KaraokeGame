using KaraokeGame.Api;
using KaraokeGame.Invidious.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KaraokeGame.Invidious.Api
{
    public class InvidiousApiHelper
    {
        const string INVIDIOUS_SEARCH_VIDEO_PATH = "/api/v1/search";

        // Search video query parameter NAMES
        const string KEYWORD_QUERY_PARAM = "q";
        const string PAGE_QUERY_PARAM = "page";
        const string SORT_BY_QUERY_PARAM = "sort_by";
        const string DATE_QUERY_PARAM = "date";
        const string DURATION_QUERY_PARAM = "duration";
        const string TYPE_QUERY_PARAM = "type";
        const string FEATURES_QUERY_PARAM = "features";
        const string REGION_QUERY_PARAM = "region";

        // Search video query parameter DEFAULT VALUES
        const string SORT_BY_DEFAULT_VALUE = "relevance";
        const string DURATION_DEFAULT_VALUE = "short";
        const string TYPE_DEFAULT_VALUE = "video";
        const string REGION_DEFAULT_VALUE = "VN";


        public static async Task<List<SearchVideoInfo>> SearchVideosByKeyword(string invidiousUrl, string keyword, int page, CancellationToken cancellationToken = default)
        {
            List<SearchVideoInfo> result = null;

            var builder = new UriBuilder(invidiousUrl);
            builder.Path = INVIDIOUS_SEARCH_VIDEO_PATH;

            string keywordQueryParam = $"{KEYWORD_QUERY_PARAM}={keyword}";
            string pageQueryParam = $"&{PAGE_QUERY_PARAM}={page}";
            string sortByQueryParam = $"&{SORT_BY_QUERY_PARAM}={SORT_BY_DEFAULT_VALUE}";
            string durationQueryParam = $"&{DURATION_QUERY_PARAM}={DURATION_DEFAULT_VALUE}";
            string typeQueryParam = $"&{TYPE_QUERY_PARAM}={TYPE_DEFAULT_VALUE}";
            string regionQueryParam = $"&{REGION_QUERY_PARAM}={REGION_DEFAULT_VALUE}";
            string combinedQuery = string.Concat(keywordQueryParam, pageQueryParam, sortByQueryParam, typeQueryParam, regionQueryParam);

            builder.Query = combinedQuery;
            Debug.Log("Search video GET api call: " + builder.Uri.ToString());

            result = await WebRequest.GetAsync<List<SearchVideoInfo>>(builder.Uri.ToString());
            return result;
        }
    }
}
