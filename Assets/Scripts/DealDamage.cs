using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public int damage = 1;

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}