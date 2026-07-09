using UnityEngine;
using ObjectDetectionAR.ImageSources;

public class StaticImageTest : MonoBehaviour
{
    [SerializeField]
    private StaticImageSource imageSource;

    void Start()
    {
        Texture2D image = imageSource.GetFrame();

        // Debug.Log($"Image : {image.name}");

        // Debug.Log($"{image.width} x {image.height}");
    }
}