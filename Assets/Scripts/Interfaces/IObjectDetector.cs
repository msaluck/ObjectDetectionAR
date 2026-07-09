using UnityEngine;
using System.Collections.Generic;
using ObjectDetectionAR.Core;

namespace ObjectDetectionAR.Interfaces
{
    public interface IObjectDetector
    {
        void Initialize();

        List<Detection> Detect(Texture2D image);

        void Dispose();
    }
}