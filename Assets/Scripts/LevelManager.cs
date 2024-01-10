using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int currentSceneIndex => SceneManager.GetActiveScene().buildIndex;

    public void GoToNextLevel()
    {
        if (!GetSceneIsValid(currentSceneIndex + 1))
        { Debug.Log("Scene by build index " + (currentSceneIndex + 1) + " can not be found."); return; }

        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void GoToPreviousLevel()
    {
        if (!GetSceneIsValid(currentSceneIndex - 1))
        { Debug.Log("Scene by build index " + (currentSceneIndex - 1) + " can not be found."); return; }

        SceneManager.LoadScene(currentSceneIndex - 1);
    }
    public void GoToLevel(int index)
    {
        if (!GetSceneIsValid(index))
        { Debug.Log("Scene by build index " + index + " can not be found."); return; }

        SceneManager.LoadScene(index);
    }

    bool GetSceneIsValid(int buildIndex) => -1 != SceneUtility.GetBuildIndexByScenePath(SceneUtility.GetScenePathByBuildIndex(buildIndex));
    bool GetSceneIsValid(string scenePath) => -1 != SceneUtility.GetBuildIndexByScenePath(scenePath);
}
