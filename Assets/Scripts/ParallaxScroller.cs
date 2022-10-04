using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [Space]
    [Header("Parallax Settings")]
    [SerializeField] Transform[] backgrounds;
    [SerializeField] float smoothing = 1f;
    float[] parallaxScales;
    Transform cameraTransform;
    Vector3 previousCamPosition;

    void Awake() 
    {
        cameraTransform = Camera.main.transform;
    }
    void Start()
    {
        previousCamPosition = cameraTransform.position;

        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z;
        }
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (cameraTransform.position.x - previousCamPosition.x) * parallaxScales[i];
            Vector3 backgroundTargetPos = new Vector3(backgrounds[i].position.x + parallax, backgrounds[i].position.y - (parallax / 4), backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPosition = cameraTransform.position;
    }
}
