using UnityEngine;
using UnityEngine.Video;

public class StoryVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public FaderController faderController;
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
        faderController.FadeOut(levelToLoad);
    }
}
