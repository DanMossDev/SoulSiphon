using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static GameObject instance;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }
}
