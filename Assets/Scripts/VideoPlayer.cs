using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoPlayer : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer videoPlayer;
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
        SceneManager.LoadScene(levelToLoad);
    }
}
