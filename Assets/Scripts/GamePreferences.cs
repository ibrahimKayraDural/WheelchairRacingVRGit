using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePreferences : MonoBehaviour
{
    public static GamePreferences Instance = null;

    public float Height;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this) Destroy(gameObject);
        }
    }
}
