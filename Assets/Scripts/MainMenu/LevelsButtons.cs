using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsButtons : MonoBehaviour
{
    public Button button;
    public GameObject canvas;

    private FaderController faderController;

    // Start is called before the first frame update
    void Start()
    {
        faderController = FindObjectOfType<FaderController>();

        //instantiate button for every level
        int n = 1;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos =
                    canvas.transform.position + new Vector3(-350 + j * 60, 100 + i * -60, 0);
                SetupButton(n++, pos);
            }
        }
    }

    // Update is called once per frame
    void Update() { }

    private void SetupButton(int levelNumber, Vector3 pos)
    {
        Button newButton = Instantiate(button);
        newButton.transform.SetParent(canvas.transform, true);

        newButton.onClick.AddListener(
            delegate()
            {
                this.ButtonClicked(levelNumber);
            }
        );

        newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{levelNumber}";
        newButton.transform.position = pos;
    }

    private void ButtonClicked(int levelNumber)
    {
        faderController.FadeOut($"Level_0{levelNumber}");
    }
}
