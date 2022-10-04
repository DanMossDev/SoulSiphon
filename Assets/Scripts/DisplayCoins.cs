using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayCoins : MonoBehaviour
{
    public void UpdateCount(int coinCount)
    {
        GetComponent<TextMeshProUGUI>().SetText("x " + coinCount);
    }
}
