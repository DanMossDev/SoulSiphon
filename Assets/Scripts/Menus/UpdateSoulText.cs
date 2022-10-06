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
        text.text = "This will replace your " + GameSession.playerElement + " soul";
        if (GameSession.playerElement == "wind") text.text = "";
    }
}
