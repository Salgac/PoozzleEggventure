using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaderController : MonoBehaviour
{
    public Animator animator;
    private string levelToLoad;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void FadeOut(string levelToLoad)
    {
        this.levelToLoad = levelToLoad;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        // fallback to main menu - TODO
        Debug.Log(SceneManager.GetSceneByName(levelToLoad).IsValid());
        SceneManager.LoadScene(levelToLoad);
    }
}
