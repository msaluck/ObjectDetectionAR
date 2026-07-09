using UnityEngine;

namespace ObjectDetectionAR.Interfaces
{
    public interface IImageSource
    {
        Texture2D GetFrame();
    }
}