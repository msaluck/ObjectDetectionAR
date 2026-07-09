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
        #region Public API
        public List<Detection> Decode(Tensor output)
        {
            List<Detection> detections = new();

            int predictionCount = output.shape.width;

            for (int i = 0; i < predictionCount; i++)
            {
                // Decode one prediction

                var detection = DecodePrediction(output, i);

                if (detection != null)
                {
                    detections.Add(detection);
                }
            }

            return nmsProcessor.Apply(detections, iouThreshold);
        }
        #endregion
        #region Decode Helpers
        private Detection DecodePrediction(Tensor output, int index)
        {
            Rect box = ReadBoundingBox(output, index);

            ClassPrediction prediction = FindBestClass(output, index);
            
            if (index == 8250)
            {
                Utils.Logger.Log($"YOLOv8Decoder.DecodePrediction() : Box={box}, Class={prediction.ClassId}, Score={prediction.Score:F5}");
            }

            return CreateDetection(
                box,
                prediction.ClassId,
                prediction.Score);
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

            for (int c = 4; c < output.shape.channels; c++)
            {
                float score = output[0, 0, index, c];

                if (score > bestScore)
                {
                    bestScore = score;
                    bestClass = c - 4;
                }
            }

            return new ClassPrediction
            {
                ClassId = bestClass,
                Score = bestScore
            };
        }

        private Detection CreateDetection(Rect boundingBox, int classId, float score)
        {
            if (score < confidenceThreshold)
                return null;

            return new Detection
            {
                BoundingBox = boundingBox,
                ClassId = classId,
                Confidence = score
            };
        }
        #endregion
    }
}