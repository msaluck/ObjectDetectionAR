using System.Collections.Generic;
using UnityEngine;
using ObjectDetectionAR.Core;

namespace ObjectDetectionAR.PostProcessing
{
    public class NMSProcessor
    {
        public List<Detection> Apply(
            List<Detection> detections,
            float iouThreshold)
        {
            detections.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));

            List<Detection> results = new();
            
            while (detections.Count > 0)
            {
                Detection best = detections[0];

                results.Add(best);

                detections.RemoveAt(0);

                for (int i = detections.Count - 1; i >= 0; i--)
                {
                    float iou = CalculateIoU(
                        best.BoundingBox,
                        detections[i].BoundingBox);

                    if (iou > iouThreshold)
                    {
                        detections.RemoveAt(i);
                    }
                }
            }

            return results;
        }

        private float CalculateIoU(Rect a, Rect b)
        {
            // Compute the intersection rectangle
            float xMin = Mathf.Max(a.xMin, b.xMin);
            float yMin = Mathf.Max(a.yMin, b.yMin);

            float xMax = Mathf.Min(a.xMax, b.xMax);
            float yMax = Mathf.Min(a.yMax, b.yMax);
            // Intersection width and height
            float width = Mathf.Max(0f, xMax - xMin);
            float height = Mathf.Max(0f, yMax - yMin);
            // Intersection Area
            float intersection = width * height;
            // Areas
            float areaA = a.width * a.height;
            float areaB = b.width * b.height;
            // Union
            float union = areaA + areaB - intersection;
            // IoU
            if (union <= 0f)
                return 0f;
            return intersection / union;
        }
    }
}