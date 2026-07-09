using Unity.Barracuda;
using UnityEngine;

namespace ObjectDetectionAR.Interfaces
{
    public interface IImagePreprocessor
    {
        Tensor Preprocess(Texture texture);

        int InputWidth { get; }

        int InputHeight { get; }
    }
}