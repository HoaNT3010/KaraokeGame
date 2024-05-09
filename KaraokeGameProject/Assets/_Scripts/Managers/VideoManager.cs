using KaraokeGame.Invidious.Api;
using KaraokeGame.Invidious.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using YoutubePlayer.Components;

public class VideoManager : Singleton<VideoManager>
{
    public class SearchVideosEventArgs : EventArgs
    {
        public List<SearchVideoInfo> videos;
    }
    public event EventHandler<SearchVideosEventArgs> OnSearchVideoComplete;

    [SerializeField] private InvidiousVideoPlayer invidiousVideoPlayer;
    [SerializeField] private string searchVideoKeyword;
    [SerializeField] private int searchVideoPage;

    public InvidiousInstance invidiousInstance;

    public override void Awake()
    {
        base.Awake();

        if (invidiousInstance == null)
        {
            Debug.LogWarning("Video Manager: InvidiousInstance is not set");
        }

        searchVideoKeyword = string.Empty;
        searchVideoPage = 1;
    }

    public async void Prepare()
    {
        Debug.Log("Loading video...");
        await invidiousVideoPlayer.PrepareVideoAsync();
        Debug.Log("Video ready");
    }

    [ContextMenu("Search Videos")]
    private async void SearchVideo()
    {
        var instanceUrl = await invidiousInstance.GetInstanceUrl();
        var videoList = await InvidiousApiHelper.SearchVideosByKeyword(instanceUrl, searchVideoKeyword, searchVideoPage);
        OnSearchVideoComplete?.Invoke(this, new SearchVideosEventArgs { videos = videoList });
    }
}
