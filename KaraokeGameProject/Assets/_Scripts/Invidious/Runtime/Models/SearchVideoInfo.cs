using Newtonsoft.Json;
using System.Collections.Generic;

namespace KaraokeGame.Invidious.Models
{
    public class SearchVideoInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("authorId")]
        public string AuthorId { get; set; }

        [JsonProperty("authorUrl")]
        public string AuthorUrl { get; set; }

        [JsonProperty("videoThumbnails")]
        public List<VideoThumbnail> VideoThumbnails { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("descriptionHtml")]
        public string DescriptionHtml { get; set; }

        [JsonProperty("viewCount")]
        public long ViewCount { get; set; }

        [JsonProperty("published")]
        public long Published { get; set; }

        [JsonProperty("publishedText")]
        public string PublishedText { get; set; }

        [JsonProperty("lengthSeconds")]
        public int LengthSeconds { get; set; }

        [JsonProperty("liveNow")]
        public bool LiveNow { get; set; }

        [JsonProperty("paid")]
        public bool Paid { get; set; }

        [JsonProperty("premium")]
        public bool Premium { get; set; }
    }

    public class VideoThumbnail
    {
        [JsonProperty("quality")]
        public string Quality { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}
