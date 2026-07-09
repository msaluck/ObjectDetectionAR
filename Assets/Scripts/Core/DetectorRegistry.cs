using System.Collections.Generic;
using ObjectDetectionAR.Interfaces;

namespace ObjectDetectionAR.Core
{
    public class DetectorRegistry
    {
        private readonly Dictionary<string, IDetectionDecoder> decoders
            = new();
        public IEnumerable<string> RegisteredModels =>
            decoders.Keys;
        public void Register(
            string name,
            IDetectionDecoder decoder)
        {
            decoders[name] = decoder;
        }

        public IDetectionDecoder Get(string name)
        {
            if (decoders.TryGetValue(name, out var decoder))
                return decoder;

            throw new System.ArgumentException(
                $"Detector '{name}' is not registered.");
        }
    }
}