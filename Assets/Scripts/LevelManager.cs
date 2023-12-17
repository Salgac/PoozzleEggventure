using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private string levelToLoad;
    private PlayerMovement playerMovement;
    public Component shitCanvas;
    public Component levelCanvas;

    public MovementController movementBar;
    public EndLevelController endLevelController;
    public FaderController faderController;

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
        this.levelToLoad = $"Level_{levelToLoad}";

        endLevelController.SetMenuTexts(levelToLoad - 1, movementBar.movesNumber, brown);
        levelCanvas.gameObject.SetActive(true);
    }

    public void FadeToLevel(string levelToLoad)
    {
        StopPlayer();
        this.levelToLoad = levelToLoad;
        faderController.FadeOut(levelToLoad);
    }

    public void FadeToLevel()
    {
        StopPlayer();
        faderController.FadeOut(levelToLoad);
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

    private void StopPlayer()
    {
        if (playerMovement != null)
        {
            playerMovement.StopAllCoroutines();
        }
    }
}
