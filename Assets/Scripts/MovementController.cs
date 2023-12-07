using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    private Slider slider;

    public int movesNumber = 0;

    private float fillSpeed = 0.5f;
    public float targetProgress = 0;
    public int maxNumberOfMoves = 10; // add aditional move on each level to work corectly

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
        movesNumber++;

        float progress = slider.maxValue / maxNumberOfMoves;
        targetProgress = slider.value + multiplier * progress;

        // bar is full
        if (targetProgress >= slider.maxValue - progress)
        {
            targetProgress = 0;
            slider.value = 0;

            // shit overlay
            levelManager.ApplyShitscreen();
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
