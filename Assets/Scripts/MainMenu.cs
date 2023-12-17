using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button levelsButton;
    public Button creditsButton;
    public Button backButton;

    public List<Component> menuContainers;

    public FaderController faderController;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayClicked);
        levelsButton.onClick.AddListener(LevelsClicked);
        creditsButton.onClick.AddListener(CreditsClicked);
        backButton.onClick.AddListener(BackClicked);
    }

    // Update is called once per frame
    void Update() { }

    private void PlayClicked()
    {
        faderController.FadeOut("VideoPlayer");
    }

    private void LevelsClicked()
    {
        //show level container
        SelectContainer(1);

        // TODO: generate buttons from levels
    }

    private void CreditsClicked()
    {
        SelectContainer(2);

        //TODO: implement credits menu
    }

    private void BackClicked()
    {
        // show main container
        SelectContainer(0);
    }

    private void SelectContainer(int index)
    {
        menuContainers.ForEach(c => c.gameObject.SetActive(false));
        menuContainers[index].gameObject.SetActive(true);
        backButton.gameObject.SetActive(index != 0);
    }
}
