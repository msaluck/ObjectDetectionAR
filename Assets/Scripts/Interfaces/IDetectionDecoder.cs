using System.Collections.Generic;
using Unity.Barracuda;
using ObjectDetectionAR.Core;

namespace ObjectDetectionAR.Interfaces
{
    public interface IDetectionDecoder
    {
        float ConfidenceThreshold { get; }

        float NmsThreshold { get; }

        List<Detection> Decode(Tensor output);
    }
}