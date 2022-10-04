using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    [Header("Sprites to animate")]
    [SerializeField] Sprite[] sprites;
    [Header("Time between frames")]
    [SerializeField] float frameTime = 0.02f;
    Image image;

    int spriteIndex = 0;
    bool isDone;

    void Start() {
        image = GetComponent<Image>();
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(frameTime);

        if (spriteIndex >= sprites.Length) spriteIndex = 0;

        image.sprite = sprites[spriteIndex];
        spriteIndex++;
        StartCoroutine(PlayAnimation());
    }
}
