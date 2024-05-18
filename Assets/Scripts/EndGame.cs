using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] int _SceneToOpenIndex;

    public bool GetSceneIsValid(int buildIndex) => -1 != SceneUtility.
        GetBuildIndexByScenePath(SceneUtility.GetScenePathByBuildIndex(buildIndex));

    void EndGameMethod()
    {
        if (GetSceneIsValid(_SceneToOpenIndex) == false)
        {
            Debug.LogError("Scene of index " + _SceneToOpenIndex + " is not in build");
            Application.Quit();
            return;
        }

        SceneManager.LoadScene(_SceneToOpenIndex);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        EndGameMethod();
    }
}
