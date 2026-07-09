using UnityEngine;
using ObjectDetectionAR.Interfaces;

namespace ObjectDetectionAR.ImageSources
{
    /// <summary>
    /// Supplies a single static image.
    /// Used for debugging and benchmarking.
    /// </summary>
    public class StaticImageSource : MonoBehaviour, IImageSource
    {
        [Header("Input Image")]

        [SerializeField]
        private Texture2D image;

        public Texture2D GetFrame()
        {
            return image;
        }

        public void SetImage(Texture2D newImage)
        {
            image = newImage;
        }
    }
}