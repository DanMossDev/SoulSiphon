using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowHideText : MonoBehaviour
{
    [Space]
    [Header("Area")]
    [SerializeField] Vector2 area = new Vector2(10, 15);
    TextMeshProUGUI element;

    void Start()
    {
        element = GetComponent<TextMeshProUGUI>();
        element.CrossFadeAlpha(0, 0, true);
    }
    void FixedUpdate() {
        Collider2D[] playerCheck = Physics2D.OverlapBoxAll(transform.position, area, 0, LayerMask.GetMask("Player"));

        if (playerCheck.Length == 0) HideMe();
        else ShowMe();
    }

    void HideMe()
    {
        element.CrossFadeAlpha(0, 0.75f, false);
    }

    void ShowMe()
    {
        element.CrossFadeAlpha(1, 0.75f, false);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, area);
    }

}
