using Unity.Barracuda;
using UnityEngine;
using ObjectDetectionAR.Interfaces;

namespace ObjectDetectionAR.Preprocessing
{
    public class ImagePreprocessor : MonoBehaviour, IImagePreprocessor
    {
        [SerializeField]
        private int inputWidth = 640;

        [SerializeField]
        private int inputHeight = 640;

        public int InputWidth => inputWidth;
        public int InputHeight => inputHeight;

        private RenderTexture resizeRT;

        public Tensor Preprocess(Texture texture)
        {
            Graphics.Blit(texture, resizeRT);

            Tensor input = new Tensor(resizeRT, channels: 3);

            return input;
        }

        private void Awake()
        {
            resizeRT = new RenderTexture(
                inputWidth,
                inputHeight,
                0,
                RenderTextureFormat.ARGB32);

            resizeRT.enableRandomWrite = false;

            resizeRT.Create();
        }

        private void OnDestroy()
        {
            if (resizeRT != null)
            {
                resizeRT.Release();
                Destroy(resizeRT);
            }
        }
    }
}