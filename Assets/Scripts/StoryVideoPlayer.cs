using UnityEngine;
using UnityEngine.Video;

public class StoryVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public LevelManager manager;
    public string levelToLoad;

    private long videoLength;

    // Start is called before the first frame update
    void Start()
    {
        videoLength = (long)videoPlayer.frameCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.frame == videoLength)
        {
            EndReached();
        }
    }

    void EndReached()
    {
        manager.FadeToLevel(levelToLoad);
    }
}
