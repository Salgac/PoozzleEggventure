using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator animator;
    private string levelToLoad;
    private PlayerMovement playerMovement;
    public Component shitCanvas;
    public Component levelCanvas;

    public MovementController movementBar;
    public EndLevelController endLevelController;

    private bool brown = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update() { }

    public void ShowLevelEndScreen(int levelToLoad)
    {
        StopPlayer();
        this.levelToLoad = $"Level_0{levelToLoad}"; // TODO redo this

        endLevelController.SetMenuTexts(levelToLoad - 1, movementBar.movesNumber, brown);
        levelCanvas.gameObject.SetActive(true);
    }

    public void FadeToLevel(string levelToLoad)
    {
        StopPlayer();
        this.levelToLoad = levelToLoad;
        animator.SetTrigger("FadeOut");
    }

    public void FadeToLevel()
    {
        StopPlayer();
        animator.SetTrigger("FadeOut");
    }

    public void ResetLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().name);
    }

    public void ApplyShitscreen()
    {
        // show shit overlay
        brown = true;
        shitCanvas.gameObject.SetActive(true);
    }

    public void OnFadeComplete()
    {
        // fallback to main menu
        if (SceneManager.GetSceneByName(levelToLoad).IsValid())
        {
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void StopPlayer()
    {
        if (playerMovement != null)
        {
            playerMovement.StopAllCoroutines();
        }
    }
}
