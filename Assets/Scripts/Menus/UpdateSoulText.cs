using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateSoulText : MonoBehaviour
{
    TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "This will replace your " + PlayerStats.playerElement + " soul";
        if (PlayerStats.playerElement == "wind") text.text = "";
    }
}
