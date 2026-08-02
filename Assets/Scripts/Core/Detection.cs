using UnityEngine;
using ObjectDetectionAR.Labels;

namespace ObjectDetectionAR.Core
{
    /// <summary>
    /// Represents a single object detection produced by an object detector.
    /// This is the common data model shared by all detectors, decoders,
    /// renderers and evaluation tools.
    /// </summary>
    [System.Serializable]
    public class Detection
    {
        /// <summary>
        /// COCO class index (0 = person, etc.)
        /// </summary>
        public int ClassId;

        /// <summary>
        /// Human readable class label.
        /// </summary>
        // public string Label;
        public string Label
        {
            get
            {
                return CocoLabels.GetLabel(ClassId);
            }
        }

        /// <summary>
        /// Detection confidence.
        /// Range: 0.0 - 1.0
        /// </summary>
        public float Confidence;

        /// <summary>
        /// Bounding box in image coordinates.
        /// x = left
        /// y = top
        /// width
        /// height
        /// </summary>
        public Rect BoundingBox;

        /// <summary>
        /// Rendering color.
        /// </summary>
        public Color DisplayColor = Color.red;

        /// <summary>
        /// Center of the bounding box.
        /// </summary>
        public Vector2 Center =>
            BoundingBox.center;

        /// <summary>
        /// Area of the detection.
        /// </summary>
        public float Area =>
            BoundingBox.width * BoundingBox.height;

        public override string ToString()
        {
            return 
                $"{Label} ({Confidence * 100f:F1}%) " +
                $"[{BoundingBox.x:F1}, {BoundingBox.y:F1}, " +
                $"{BoundingBox.width:F1}, {BoundingBox.height:F1}]";
        }

        public Detection(Rect boundingBox, int classId, float confidence)
        {
            BoundingBox = boundingBox;
            ClassId = classId;
            Confidence = confidence;
        }

        public Detection(){}
    }
}