using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using ObjectDetectionAR.Interfaces;
using ObjectDetectionAR.Core;
using ObjectDetectionAR.PostProcessing;

namespace ObjectDetectionAR.Decoders
{
    public class YOLOv8Decoder :
        MonoBehaviour,
        IDetectionDecoder
    {
        [SerializeField] private float confidenceThreshold = 0.25f;
        [SerializeField] private float iouThreshold = 0.45f;
        private readonly NMSProcessor nmsProcessor = new NMSProcessor();
        [SerializeField] private int classOffset = 4;

        #region Public API
        public float ConfidenceThreshold => confidenceThreshold;

        public float NmsThreshold => iouThreshold;

        public List<Detection> Decode(Tensor output)
        {
            List<Detection> detections = new();

            int predictionCount = output.shape.width;

            int totalPredictions = predictionCount;
            int acceptedPredictions = 0;

            for (int i = 0; i < predictionCount; i++)
            {
                // Decode one prediction

                var detection = DecodePrediction(output, i);

                if (detection != null)
                {
                    detections.Add(detection);
                    acceptedPredictions++;
                }
            }
            Utils.Logger.Log(
                $"Predictions: {totalPredictions}, Accepted: {acceptedPredictions}");
            return nmsProcessor.Apply(detections, iouThreshold);
        }
        #endregion
        #region Decode Helpers
        private Detection DecodePrediction(Tensor output, int index)
        {
            ClassPrediction prediction = FindBestClass(output, index);

            if (prediction.Score < confidenceThreshold)
                return null;

            Rect box = ReadBoundingBox(output, index);

            return new Detection
            {
                BoundingBox = box,
                ClassId = prediction.ClassId,
                Confidence = prediction.Score
            };
        }

        private Rect ReadBoundingBox(Tensor output, int index)
        {
            float cx = output[0, 0, index, 0];
            float cy = output[0, 0, index, 1];
            float w = output[0, 0, index, 2];
            float h = output[0, 0, index, 3];

            return new Rect(
                cx - w * 0.5f,
                cy - h * 0.5f,
                w,
                h);
        }

        private ClassPrediction FindBestClass(
    Tensor output,
    int index)
        {
            float bestScore = 0f;
            int bestClass = 0;

            for (int c = classOffset; c < output.shape.channels; c++)
            {
                float score = output[0, 0, index, c];

                if (score > bestScore)
                {
                    bestScore = score;
                    bestClass = c - classOffset;
                }
            }

            return new ClassPrediction
            {
                ClassId = bestClass,
                Score = bestScore
            };
        }
        #endregion
    }
}