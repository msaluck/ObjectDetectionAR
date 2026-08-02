using System.Collections.Generic;
using System;
using ObjectDetectionAR.Interfaces;

namespace ObjectDetectionAR.Core
{
    public class DetectorRegistry
    {
        private readonly Dictionary<string, IDetectionDecoder> decoders = new();
        public IEnumerable<string> RegisteredModels => decoders.Keys;
        public void Register(string modelName, IDetectionDecoder decoder)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                throw new ArgumentException("Detector name cannot be null or empty.", nameof(modelName));

            if (decoder == null)
                throw new ArgumentNullException(nameof(decoder));

            decoders[modelName] = decoder;
        }

        public IDetectionDecoder Get(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                throw new ArgumentException("Detector name cannot be null or empty.", nameof(modelName));

            if (decoders.TryGetValue(modelName, out var decoder))
                return decoder;

            throw new System.ArgumentException($"Detector '{modelName}' is not registered.");
        }

        public bool Contains(string modelName)
        {
            return decoders.ContainsKey(modelName);
        }
    }
}