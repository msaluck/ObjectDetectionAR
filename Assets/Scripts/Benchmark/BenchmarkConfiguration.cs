using System;

namespace ObjectDetectionAR.Benchmark
{
    [Serializable]
    public class BenchmarkConfiguration
    {
        // Benchmark
        public int BenchmarkRuns;
        public int WarmupRuns;

        // Model
        public string ModelName;
        public string Backend;

        // Input
        public int InputWidth;
        public int InputHeight;

        // Detection
        public float ConfidenceThreshold;
        public float NmsThreshold;

        // Decoder
        public string DecoderType;
    }
}
