using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

using ObjectDetectionAR.Interfaces;

namespace ObjectDetectionAR.Core
{
    public class DetectionRunner : MonoBehaviour
    {
        [Header("Pipeline")]

        [SerializeField]
        private MonoBehaviour imageSourceBehaviour;

        [SerializeField]
        private MonoBehaviour preprocessorBehaviour;

        [SerializeField]
        private MonoBehaviour modelRunnerBehaviour;

        [SerializeField]
        private MonoBehaviour decoderBehaviour;

        private IImageSource imageSource;

        private IImagePreprocessor preprocessor;

        private IModelRunner modelRunner;

        private IDetectionDecoder decoder;
        private readonly DetectorRegistry registry = new DetectorRegistry();
        [SerializeField] private string currentModel = "YOLOv8n";
        [SerializeField] private ModelRegistry modelRegistry;
        private string loadedModelName;

        private void InitializePipeline()
        {
            imageSource = imageSourceBehaviour as IImageSource;

            preprocessor = preprocessorBehaviour as IImagePreprocessor;

            modelRunner = modelRunnerBehaviour as IModelRunner;

            decoder = decoderBehaviour as IDetectionDecoder;
        }
        private void RegisterDetectors()
        {
            registry.Register("YOLOv8n", decoder);
        }
        private void Awake()
        {
            InitializePipeline();
            RegisterDetectors();
        }

        public DetectionResult Run()
        {
            DetectionResult result = new DetectionResult();
            // TODO: result.ModelName = modelRunner.ModelName;
            result.ModelName = currentModel;

            var model = modelRegistry.Get(result.ModelName);
            
            modelRunner.LoadModel(model);

            Texture image = imageSource.GetFrame();
            
            Tensor input = preprocessor.Preprocess(image);
            
            Tensor output = modelRunner.Execute(input);
            
            result.ModelWidth = 640;
            
            result.ModelHeight = 640;
            
            result.SourceImage = image;
            
            result.ImageWidth = image.width;
            
            result.ImageHeight = image.height;
            
            var detector = registry.Get(result.ModelName);
            
            result.Detections = detector.Decode(output);
            
            result.PreprocessTimeMs = 0;
            
            result.InferenceTimeMs = 0;
            
            result.DecodeTimeMs = 0;
            
            result.BackendName = modelRunner.BackendName;
            
            Utils.Logger.Log($"DetectionRunner.Run() : {result.Detections.Count} detections");
            
            Utils.Logger.Log($"DetectionRunner.Run() : {result}");
            
            input.Dispose();
            
            output.Dispose();
            
            return result;
        }
    }
}