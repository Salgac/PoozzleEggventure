using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowTo : MonoBehaviour
{

    public FaderController faderController;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Reset());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Reset()
    {
        for (float t = 0.0f; t < 4.0f; t += Time.deltaTime)
        {
            yield return null;
        }
        EndReached();
    }

    void EndReached()
    {
        faderController.FadeOut("MainMenu");
    }
}
