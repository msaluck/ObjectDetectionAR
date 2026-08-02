using UnityEngine;
using ObjectDetectionAR.Core;
using ObjectDetectionAR.Rendering;

public class DetectionRunnerTest : MonoBehaviour
{
    [SerializeField] private DetectionRunner runner;
    [SerializeField] private BoundingBoxRenderer renderer;

    void Start()
    {
        // var detections = runner.Run();
        // DetectionResult result = runner.Run();

        // Debug.Log($"Detections : {result.Detections.Count}");
        // Debug.Log(result);

        // renderer.Render(result);

        // foreach (var detection in result.Detections)
        {
            // Debug.Log(detection);
        }
    }
}