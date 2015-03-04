using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {
    public float score = 0;
    bool isStarted = false;

    void Start()
    {
        isStarted = true;
        DontDestroyOnLoad(gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
        if (isStarted)
            Destroy(gameObject);
    }
}
