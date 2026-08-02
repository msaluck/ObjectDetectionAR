using System.Diagnostics;
using System;
using Unity.Barracuda;
using UnityEngine;

using ObjectDetectionAR.Interfaces;
using ObjectDetectionAR.UI;

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
        [Serializable]
        private class DecoderConfig
        {
            public string ModelName;

            public MonoBehaviour DecoderBehaviour;
        }

        [Header("Decoders")]
        [SerializeField]
        private DecoderConfig[] decoders;
        private IImageSource imageSource;
        private IImagePreprocessor preprocessor;
        private IModelRunner modelRunner;
        private readonly DetectorRegistry registry = new DetectorRegistry();
        [SerializeField] private ModelSelector modelSelector;
        [SerializeField] private ModelRegistry modelRegistry;
        private string loadedModelName;
        private void InitializePipeline()
        {
            imageSource = imageSourceBehaviour as IImageSource;

            preprocessor = preprocessorBehaviour as IImagePreprocessor;

            modelRunner = modelRunnerBehaviour as IModelRunner;
        }
        private void RegisterDetectors()
        {
            if (decoders == null || decoders.Length == 0)
            {
                throw new InvalidOperationException(
                    "No decoders are configured.");
            }
                
            foreach (var config in decoders)
            {
                if (config == null)
                {
                    throw new InvalidOperationException(
                        "Decoder configuration is null.");
                }

                if (string.IsNullOrWhiteSpace(config.ModelName))
                {
                    throw new InvalidOperationException(
                        "Decoder configuration has an empty model name.");
                }
                
                if (config.DecoderBehaviour is not IDetectionDecoder decoder)
                {
                    throw new InvalidOperationException(
                        $"Decoder for model '{config.ModelName}' " +
                        "does not implement IDetectionDecoder.");
                }

                registry.Register(config.ModelName, decoder);
            }
        }
        private void Awake()
        {
            InitializePipeline();
            RegisterDetectors();
        }
        private string ResolveCurrentModelName()
        {
            string currentModel = modelSelector != null ? modelSelector.CurrentModel : null;

            if (!string.IsNullOrWhiteSpace(currentModel))
                return currentModel;

            var registeredModels = modelRegistry?.RegisteredModels;

            if (registeredModels == null || registeredModels.Count == 0)
                throw new InvalidOperationException("No model is available. Ensure ModelRegistry is configured with at least one model.");

            // Handles frame-order races where ModelSelector.Start has not run yet.
            return registeredModels[0];
        }
        public IDetectionDecoder GetDecoder(string modelName) =>
            registry.Get(modelName);

        public void SetModel(string modelName)
        {
            if (loadedModelName == modelName)
                return;
            if (!registry.Contains(modelName))
            {
                throw new InvalidOperationException(
                    $"No decoder is configured/registered for model '{modelName}'.");
            }
            var model = modelRegistry.Get(modelName);

            modelRunner.LoadModel(modelName, model);

            loadedModelName = modelName;
        }
        public DetectionResult Run()
        {
            DetectionResult result = new DetectionResult();

            if (loadedModelName == null)
                SetModel(ResolveCurrentModelName());

            result.ModelName = loadedModelName;

            Texture image = imageSource.GetFrame();

            var timer = Stopwatch.StartNew();

            Tensor input = preprocessor.Preprocess(image);

            timer.Stop();

            result.PreprocessTimeMs = (float)timer.Elapsed.TotalMilliseconds;

            timer.Restart();

            Tensor output = modelRunner.Execute(input);

            timer.Stop();

            result.InferenceTimeMs = (float)timer.Elapsed.TotalMilliseconds;

            result.ModelWidth = input.width;

            result.ModelHeight = input.height;

            result.SourceImage = image;

            result.ImageWidth = image.width;

            result.ImageHeight = image.height;

            timer.Restart();

            var detector = registry.Get(result.ModelName);

            result.Detections = detector.Decode(output);

            timer.Stop();

            result.DecodeTimeMs = (float)timer.Elapsed.TotalMilliseconds;

            result.BackendName = modelRunner.BackendName;

            Utils.Logger.Log($"DetectionRunner.Run() : {result.Detections.Count} detections");

            Utils.Logger.Log($"DetectionRunner.Run() : {result}");

            input.Dispose();

            output.Dispose();

            return result;
        }
    }
}