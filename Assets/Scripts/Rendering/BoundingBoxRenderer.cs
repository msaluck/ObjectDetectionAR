using System.Collections.Generic;
using UnityEngine;

using ObjectDetectionAR.Core;

namespace ObjectDetectionAR.Rendering
{
    public class BoundingBoxRenderer : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform container;
        [SerializeField] private GameObject boxPrefab;
        [SerializeField] private RectTransform imageRect;
        private readonly List<GameObject> activeBoxes = new();
        public void Render(DetectionResult result)
        {
            Utils.Logger.Log("BoundingBoxRenderer.Render()");
            
            Clear();

            foreach (var detection in result.Detections)
            {
                CreateBox(detection, result);
            }
        }

        private void Clear()
        {
            foreach (var box in activeBoxes)
            {
                Destroy(box);
            }
            activeBoxes.Clear();
        }

        private void CreateBox(Detection detection, DetectionResult result)
        {
            float scaleX = imageRect.rect.width / result.ModelWidth;

            float scaleY = imageRect.rect.height / result.ModelHeight;

            Rect box = detection.BoundingBox;

            float x = box.x * scaleX;
            float y = box.y * scaleY;

            float w = box.width * scaleX;
            float h = box.height * scaleY;

            GameObject go = Instantiate(boxPrefab, container);

            RectTransform rect = go.GetComponent<RectTransform>();

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.pivot = new Vector2(0, 1);

            rect.anchoredPosition = new Vector2(x, -y);

            rect.sizeDelta = new Vector2(w, h);

            activeBoxes.Add(go);
        }
    }
}