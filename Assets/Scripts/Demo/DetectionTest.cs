using UnityEngine;
using ObjectDetectionAR.Core;

public class DetectionTest : MonoBehaviour
{
    void Start()
    {
        Detection d = new Detection
        {
            ClassId = 0,
            Confidence = 0.92f,
            BoundingBox = new Rect(120, 60, 220, 450)
        };

        // Debug.Log(d);
    }
}