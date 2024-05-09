using System;

namespace KaraokeGame.Recording.Models
{
    public enum PerformanceMode
    {
        Single,
        Multiple,
    }

    public enum MusicSourceOption
    {
        YouTube,
        Instrument,
    }

    [Serializable]
    public class PlayerPerformance
    {
        private Guid id;
        private PerformanceMode mode;
        private MusicSourceOption musicSourceOption;
        private string youtubeVideoId;
        private string voiceRecordingLocation;
    }
}
