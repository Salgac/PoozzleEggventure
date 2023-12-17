using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaderController : MonoBehaviour
{
    public Animator animator;
    private string levelToLoad;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        audioSource = GameObject.FindFirstObjectByType<AudioSource>();

    }

    // Update is called once per frame
    void Update() { }

    public void FadeOut(string levelToLoad)
    {
        this.levelToLoad = levelToLoad;
        animator.SetTrigger("FadeOut");
        try
        {
            StartCoroutine(audioFadeOut());
        }
        catch { }
        
    }

    public void OnFadeComplete()
    {
        // fallback to main menu - TODO
        if (audioSource)
            audioSource.Stop();
        Debug.Log(SceneManager.GetSceneByName(levelToLoad).IsValid());
        SceneManager.LoadScene(levelToLoad);
    }

    public IEnumerator audioFadeOut()
    {
        if (audioSource)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / 1.0f;

                yield return null;
            }
            audioSource.volume = startVolume;
        }
        
    }
}
