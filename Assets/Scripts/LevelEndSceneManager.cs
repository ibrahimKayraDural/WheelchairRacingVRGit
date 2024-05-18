using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEndSceneManager : MonoBehaviour
{
    [SerializeField] int _SceneToOpenIndex;
    [SerializeField] Image _BlackoutImage;

    [SerializeField] float _WaitSeconds = 5f;
    [SerializeField] float _BlackoutDuration = 3f;

    void Start()
    {
        Invoke(nameof(StartBlackout), _WaitSeconds);
    }

    public bool GetSceneIsValid(int buildIndex) => -1 != SceneUtility.
        GetBuildIndexByScenePath(SceneUtility.GetScenePathByBuildIndex(buildIndex));

    void StartBlackout()
    {
        StartCoroutine(BlackoutIENUM());
    }
    IEnumerator BlackoutIENUM()
    {
        int iterations = Mathf.CeilToInt(20 * _BlackoutDuration);
        float iterationDuration = _BlackoutDuration / iterations;
        float stepAmount = 1f / (float)iterations;
        Color color = _BlackoutImage.color;
        color.a = 0;

        for (int i = 0; i < iterations; i++)
        {
            color.a += stepAmount;
            _BlackoutImage.color = color;
            yield return new WaitForSeconds(iterationDuration);
        }

        color.a = 1;
        _BlackoutImage.color = color;

        BlackoutEnded();
    }
    void BlackoutEnded()
    {
        if (GetSceneIsValid(_SceneToOpenIndex) == false)
        {
            Debug.LogError("Scene of index " + _SceneToOpenIndex + " is not in build");
            Application.Quit();
            return;
        }

        SceneManager.LoadScene(_SceneToOpenIndex);
    }
}
