using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementBar : MonoBehaviour
{
    private Slider slider;

    private float fillSpeed = 0.5f;
    public float targetProgress = 0;
    public int maxNumberOfMoves = 10;

    private LevelManager levelManager;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {   
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
        }
        if (slider.value > targetProgress)
        {
            slider.value -= fillSpeed * Time.deltaTime;
        }
    }

    public void IncrementProgress(int multiplier = 1)
    {
        float progress = slider.maxValue / maxNumberOfMoves;
        targetProgress = slider.value + multiplier * progress;
        if (targetProgress >= slider.maxValue)
        {
            targetProgress = 0;
            slider.value = 0;
            levelManager.ResetLevel();
        }
    }

    public void DecrementProgress(int multiplier = 1)
    {
        float progress = slider.maxValue / maxNumberOfMoves;
        targetProgress = slider.value - multiplier * progress;
        if (targetProgress <= 0)
        {
            targetProgress = 0;
            slider.value = 0;
        }
    }
}
