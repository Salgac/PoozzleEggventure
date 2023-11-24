using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator animator;
    private string levelToLoad;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void FadeToLevel(string levelToLoad)
    {
        this.levelToLoad = levelToLoad;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
