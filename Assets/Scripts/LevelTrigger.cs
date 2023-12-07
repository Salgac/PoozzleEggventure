using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public int levelToLoad;
    public LevelManager manager;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Finish! " + SceneManager.GetActiveScene().name);
            manager.ShowLevelEndScreen(levelToLoad);
        }
    }
}
