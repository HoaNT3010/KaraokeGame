using System;

namespace KaraokeGame.Recording.Models
{
    public enum PerformanceMode
    {
        Single,
        Multiple,
    }

    [Serializable]
    public class PlayerPerformance
    {
        public Guid PerformanceId { get; set; }
        public string PerformanceName { get; set; } = string.Empty;
        public PerformanceMode PerformanceMode { get; set; } = PerformanceMode.Single;
        public string YouTubeVideoId { get; set; } = string.Empty;
        public string VoiceRecordingLocation { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
