using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ObjectDetectionAR.Core;

namespace ObjectDetectionAR.Benchmark
{
    public class BenchmarkRunner : MonoBehaviour
    {
        [SerializeField]
        private DetectionRunner detectionRunner;
        private readonly List<BenchmarkResult> results = new();
        private readonly List<BenchmarkResult> allResults = new();
        [SerializeField] private int benchmarkRuns = 5;
        private readonly List<ModelBenchmarkResult> modelResults = new();
        [SerializeField] private ModelRegistry modelRegistry;
        private readonly BenchmarkExporter exporter = new();
        public IReadOnlyList<BenchmarkResult> AllResults => allResults;
        public IReadOnlyList<ModelBenchmarkResult> ModelResults => modelResults;
        private BenchmarkResult CreateBenchmark(DetectionResult detection, int runNumber)
        {
            return new BenchmarkResult
            {
                ModelName = detection.ModelName,
                BackendName = detection.BackendName,

                PreprocessTimeMs = detection.PreprocessTimeMs,
                InferenceTimeMs = detection.InferenceTimeMs,
                DecodeTimeMs = detection.DecodeTimeMs,
                TotalTimeMs = detection.TotalTimeMs,

                FPS = detection.FPS,
                DetectionCount = detection.Detections.Count,

                RunNumber = runNumber,
                IsWarmup = runNumber == 1,
                Timestamp = DateTime.Now
            };
        }
        private ModelBenchmarkResult CreateSummary()
        {
            if (results.Count == 0)
            {
                throw new System.InvalidOperationException(
                    "Cannot create benchmark summary because no benchmark runs were recorded.");
            }

            int startIndex = results.Count > 1 ? 1 : 0;

            float totalTime = 0f;
            float totalFPS = 0f;

            float bestTime = float.MaxValue;
            float worstTime = float.MinValue;

            for (int i = startIndex; i < results.Count; i++)
            {
                var result = results[i];

                totalTime += result.TotalTimeMs;
                totalFPS += result.FPS;

                if (result.TotalTimeMs < bestTime)
                    bestTime = result.TotalTimeMs;

                if (result.TotalTimeMs > worstTime)
                    worstTime = result.TotalTimeMs;
            }

            int measuredRuns = results.Count - startIndex;

            return new ModelBenchmarkResult
            {
                ModelName = results[^1].ModelName,
                Backend = results[^1].BackendName,

                Runs = measuredRuns,

                AverageTimeMs = measuredRuns > 0 ? totalTime / measuredRuns : 0f,

                AverageFPS = measuredRuns > 0 ? totalFPS / measuredRuns : 0f,

                BestTimeMs = bestTime,

                WorstTimeMs = worstTime
            };
        }

        private void PrintSummary(ModelBenchmarkResult summary)
        {
            Utils.Logger.Log($@"
                Benchmark Summary
                --------------------------------
                Model        : {summary.ModelName}
                Runs         : {summary.Runs}

                Average Time : {summary.AverageTimeMs:F2} ms
                Average FPS  : {summary.AverageFPS:F2}

                Best Time    : {summary.BestTimeMs:F2} ms
                Worst Time   : {summary.WorstTimeMs:F2} ms
                ");
        }

        private void RunMeasuredLoop()
        {
            results.Clear();

            for (int i = 0; i < benchmarkRuns; i++)
            {
                int runNumber = i + 1;
                DetectionResult detection = detectionRunner.Run();

                BenchmarkResult benchmark =
                    CreateBenchmark(detection, runNumber);

                results.Add(benchmark);
                allResults.Add(benchmark);

                Utils.Logger.Log(
                    $"Run {runNumber}: {benchmark}");
            }
        }

        public void RunBenchmark()
        {
            RunMeasuredLoop();

            ModelBenchmarkResult summary = CreateSummary();
            modelResults.Add(summary);
            PrintSummary(summary);
        }

        public void RunBenchmark(string modelName)
        {
            detectionRunner.SetModel(modelName);

            RunMeasuredLoop();

            ModelBenchmarkResult summary = CreateSummary();
            modelResults.Add(summary);
            PrintSummary(summary);
        }

        public void RunAllBenchmarks()
        {
            allResults.Clear();
            modelResults.Clear();

            foreach (string modelName in modelRegistry.GetModelNames())
            {
                RunBenchmark(modelName);
            }

            PrintComparisonTable();
            ExportBenchmarkResults();
        }

        private void PrintComparisonTable()
        {
            Utils.Logger.Log("==================================================");
            Utils.Logger.Log("Benchmark Comparison");
            Utils.Logger.Log("==================================================");

            foreach (var summary in modelResults)
            {
                Utils.Logger.Log(summary);
            }

            Utils.Logger.Log("==================================================");
        }

        private void ExportBenchmarkResults()
        {
            string sessionName =
                DateTime.Now.ToString("yyyy-MM-dd_HHmmss");

            string directoryPath = Path.Combine(
                Application.persistentDataPath,
                "Benchmarks",
                sessionName);

            exporter.ExportRuns(
                AllResults,
                directoryPath);

            exporter.ExportSummary(
                ModelResults,
                directoryPath);

            exporter.ExportEnvironment(
                directoryPath);

            exporter.ExportConfiguration(
                BuildConfiguration(),
                directoryPath);

            Utils.Logger.Log(
                $"Benchmark session exported to: {directoryPath}");
        }

        private BenchmarkConfiguration BuildConfiguration()
        {
            if (modelResults.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cannot build benchmark configuration because no benchmark results are available.");
            }

            // The most recently benchmarked model in this session. RunAllBenchmarks()
            // benchmarks every registered model into one shared session folder, so a
            // single config file can't describe every model — it records the last one.
            ModelBenchmarkResult lastModel = modelResults[^1];

            var decoder = detectionRunner.GetDecoder(lastModel.ModelName);

            return new BenchmarkConfiguration
            {
                BenchmarkRuns = benchmarkRuns,
                WarmupRuns = benchmarkRuns > 1 ? 1 : 0,

                ModelName = lastModel.ModelName,
                Backend = lastModel.Backend,

                // Standardized input resolution; not yet exposed from ImagePreprocessor.
                InputWidth = 640,
                InputHeight = 640,

                ConfidenceThreshold = decoder.ConfidenceThreshold,
                NmsThreshold = decoder.NmsThreshold,

                DecoderType = decoder.GetType().Name
            };
        }
    }

}