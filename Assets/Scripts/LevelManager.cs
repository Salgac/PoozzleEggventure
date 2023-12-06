using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator animator;
    private string levelToLoad;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
     { 
        playerMovement = FindObjectOfType<PlayerMovement>();
     }

    // Update is called once per frame
    void Update() { }

    public void FadeToLevel(string levelToLoad)
    {
        playerMovement.StopAllCoroutines();
        this.levelToLoad = levelToLoad;
        animator.SetTrigger("FadeOut");
    }

    public void ResetLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().name);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
