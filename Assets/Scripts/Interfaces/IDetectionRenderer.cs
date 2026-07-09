using System.Collections.Generic;
using ObjectDetectionAR.Core;

namespace ObjectDetectionAR.Interfaces
{
    public interface IDetectionRenderer
    {
        void Render(List<Detection> detections);

        void Clear();
    }
}