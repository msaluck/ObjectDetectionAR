using System.Collections.Generic;
using Unity.Barracuda;
using ObjectDetectionAR.Core;

namespace ObjectDetectionAR.Interfaces
{
    public interface IDetectionDecoder
    {
        List<Detection> Decode(Tensor output);
    }
}