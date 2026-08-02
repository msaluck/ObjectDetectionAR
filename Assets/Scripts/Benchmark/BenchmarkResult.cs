using System;

namespace ObjectDetectionAR.Benchmark
{
    [Serializable]
    public class BenchmarkResult
    {
        public string ModelName;
        public string BackendName;
        public float PreprocessTimeMs;
        public float InferenceTimeMs;
        public float DecodeTimeMs;
        public float TotalTimeMs;
        public float FPS;
        public int DetectionCount;
        public int RunNumber { get; set; }
        public bool IsWarmup { get; set; }
        public DateTime Timestamp { get; set; }
        public override string ToString()
        {
            return
                $"{ModelName} | " +
                $"{BackendName} | " +
                $"{FPS:F2} FPS | " +
                $"{TotalTimeMs:F2} ms";
        }
    }
}