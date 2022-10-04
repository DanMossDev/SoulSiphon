using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPTracker : MonoBehaviour
{
    [SerializeField] int currentHP;

    public void ChangeHP(int change)
    {
        currentHP += change;
    }
}
