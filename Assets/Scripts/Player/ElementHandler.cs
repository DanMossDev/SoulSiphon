using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementHandler : MonoBehaviour
{
    static string currentElement;
    GameObject currentElementObject;
    [SerializeField] Transform bulletSpawn;

    Dictionary<string, int> elementLookup = new Dictionary<string, int>();
    
    void OnEnable() 
    {
        PlayerStats.OnChangeElement += ChangeElement;
    }

    void OnDisable() 
    {
        PlayerStats.OnChangeElement -= ChangeElement;
    }
    void Start() 
    {
        elementLookup["wind"] = 0;
        elementLookup["fire"] = 1;
        elementLookup["void"] = 2;
        elementLookup["holy"] = 3;

        ChangeElement();
    }
    
    void ChangeElement()
    {
        currentElement = PlayerStats.playerElement;
        int i = 0;
        foreach (Transform element in bulletSpawn.transform)
        {
            element.gameObject.SetActive(i == elementLookup[currentElement]);
            i++;
        }
    }
}
