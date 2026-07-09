using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using ObjectDetectionAR.ModelRunner;
using ObjectDetectionAR.Preprocessing;
using ObjectDetectionAR.ImageSources;
using ObjectDetectionAR.Decoders;
using ObjectDetectionAR.Core;

public class BarracudaRunnerTest : MonoBehaviour
{
    [SerializeField]
    private StaticImageSource imageSource;

    [SerializeField]
    private ImagePreprocessor preprocessor;

    [SerializeField]
    private BarracudaRunner runner;

    [SerializeField]
    private YOLOv8Decoder decoder;

    private void Start()
    {
        Texture image = imageSource.GetFrame();

        Tensor input = preprocessor.Preprocess(image);

        Tensor output = runner.Execute(input);

        List<Detection> detections = decoder.Decode(output);

        // Debug.Log($"Detections = {detections.Count}");

        foreach (var d in detections)
        {
            // Debug.Log(d);
        }

        // Debug.Log(output.shape);

        input.Dispose();
    }
}