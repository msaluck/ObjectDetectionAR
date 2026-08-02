using System;

namespace ObjectDetectionAR.Benchmark
{
    [Serializable]
    public class ModelBenchmarkResult
    {
        public string ModelName;

        public string Backend;

        public int Runs;

        public float AverageTimeMs;

        public float AverageFPS;

        public float BestTimeMs;

        public float WorstTimeMs;

        public override string ToString()
        {
            return
                $"{ModelName,-15}" +
                $"{AverageTimeMs,10:F2} ms" +
                $"{AverageFPS,10:F2} FPS";
        }
    }
}