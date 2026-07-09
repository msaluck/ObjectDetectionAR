using UnityEngine;
using Unity.Barracuda;
using ObjectDetectionAR.Preprocessing;
using ObjectDetectionAR.ImageSources;

public class ImagePreprocessorTest : MonoBehaviour
{
    [SerializeField]
    private StaticImageSource imageSource;

    [SerializeField]
    private ImagePreprocessor preprocessor;

    private void Start()
    {
        Texture image = imageSource.GetFrame();

        Tensor tensor = preprocessor.Preprocess(image);

        // Debug.Log(tensor.shape);

        tensor.Dispose();
    }
}