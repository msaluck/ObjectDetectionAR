using System.Collections.Generic;
using UnityEngine;

namespace ObjectDetectionAR.Core
{
    [System.Serializable]
    public class DetectionResult
    {
        // Original image used for inference
        public Texture SourceImage;

        // Final detections
        public List<Detection> Detections = new();

        //---------------------------------
        // Model
        //---------------------------------
        public string ModelName;
        public int ModelWidth;
        public int ModelHeight;

        //---------------------------------
        // Input Image
        //---------------------------------
        public int ImageWidth;
        public int ImageHeight;

        // Performance metrics
        public float PreprocessTimeMs;
        public float InferenceTimeMs;
        public float DecodeTimeMs;
        public string BackendName;
        public float FPS;
        public long PeakMemory;

        public int DetectionCount
        {
            get { return Detections.Count; }
        }

        public override string ToString()
        {
            return
                $@"DetectionResult
                --------------------------------
                Model        : {ModelName}
                Backend      : {BackendName}
                Model Size   : {ModelWidth} x {ModelHeight}
                Image Size   : {ImageWidth} x {ImageHeight}
                Detections   : {DetectionCount}
                Preprocess   : {PreprocessTimeMs:F2} ms
                Inference    : {InferenceTimeMs:F2} ms
                Decode       : {DecodeTimeMs:F2} ms";
        }
    }
}