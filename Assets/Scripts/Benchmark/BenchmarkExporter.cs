using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace ObjectDetectionAR.Benchmark
{
    public class BenchmarkExporter
    {
        private const string FrameworkVersion = "1.0.0";

        public void ExportRuns(
            IReadOnlyList<BenchmarkResult> results,
            string directoryPath)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            if (results.Count == 0)
                throw new InvalidOperationException(
                    "Cannot export benchmark runs because no results were recorded.");

            Directory.CreateDirectory(directoryPath);

            string filePath =
                Path.Combine(directoryPath, "benchmark_runs.csv");

            var csv = new StringBuilder();

            csv.AppendLine(
                "Timestamp,Model,Backend,Run,IsWarmup," +
                "Detections,PreprocessMs,InferenceMs," +
                "DecodeMs,TotalMs,FPS");

            foreach (BenchmarkResult result in results)
            {
                csv.AppendLine(string.Join(",",
                    result.Timestamp.ToString("O", CultureInfo.InvariantCulture),
                    EscapeCsv(result.ModelName),
                    EscapeCsv(result.BackendName),
                    result.RunNumber.ToString(CultureInfo.InvariantCulture),
                    result.IsWarmup ? "true" : "false",
                    result.DetectionCount.ToString(CultureInfo.InvariantCulture),
                    result.PreprocessTimeMs.ToString("F4", CultureInfo.InvariantCulture),
                    result.InferenceTimeMs.ToString("F4", CultureInfo.InvariantCulture),
                    result.DecodeTimeMs.ToString("F4", CultureInfo.InvariantCulture),
                    result.TotalTimeMs.ToString("F4", CultureInfo.InvariantCulture),
                    result.FPS.ToString("F4", CultureInfo.InvariantCulture)
                ));
            }

            File.WriteAllText(
                filePath,
                csv.ToString(),
                new UTF8Encoding(false));

            Debug.Log($"Benchmark runs exported to: {filePath}");
        }

        public void ExportSummary(
            IReadOnlyList<ModelBenchmarkResult> results,
            string directoryPath)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            if (results.Count == 0)
                throw new InvalidOperationException(
                    "Cannot export benchmark summary because no results were recorded.");

            Directory.CreateDirectory(directoryPath);

            string filePath =
                Path.Combine(directoryPath, "benchmark_summary.csv");

            var csv = new StringBuilder();

            csv.AppendLine(
                "Model,Backend,MeasuredRuns," +
                "AverageTimeMs,AverageFPS,BestTimeMs,WorstTimeMs");

            foreach (ModelBenchmarkResult result in results)
            {
                csv.AppendLine(string.Join(",",
                    EscapeCsv(result.ModelName),
                    EscapeCsv(result.Backend),
                    result.Runs.ToString(CultureInfo.InvariantCulture),
                    result.AverageTimeMs.ToString("F4", CultureInfo.InvariantCulture),
                    result.AverageFPS.ToString("F4", CultureInfo.InvariantCulture),
                    result.BestTimeMs.ToString("F4", CultureInfo.InvariantCulture),
                    result.WorstTimeMs.ToString("F4", CultureInfo.InvariantCulture)
                ));
            }

            File.WriteAllText(
                filePath,
                csv.ToString(),
                new UTF8Encoding(false));

            Debug.Log($"Benchmark summary exported to: {filePath}");
        }

        public void ExportEnvironment(
            string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);

            string filePath =
                Path.Combine(directoryPath, "environment.json");

            BenchmarkEnvironment env = new()
            {
                BenchmarkFrameworkVersion = FrameworkVersion,

                Timestamp = DateTime.Now.ToString("O"),

                UnityVersion = Application.unityVersion,
                Platform = Application.platform.ToString(),

                OperatingSystem = SystemInfo.operatingSystem,

                ProcessorType = SystemInfo.processorType,
                ProcessorCount = SystemInfo.processorCount,

                SystemMemoryMB = SystemInfo.systemMemorySize,

                GraphicsDeviceName = SystemInfo.graphicsDeviceName,
                GraphicsDeviceVendor = SystemInfo.graphicsDeviceVendor,
                GraphicsMemoryMB = SystemInfo.graphicsMemorySize,
                GraphicsAPI = SystemInfo.graphicsDeviceType.ToString(),

                DeviceModel = SystemInfo.deviceModel,
                DeviceName = SystemInfo.deviceName,
                DeviceType = SystemInfo.deviceType.ToString(),
            };

            string json =
                JsonUtility.ToJson(
                    env,
                    true);

            File.WriteAllText(
                filePath,
                json,
                new UTF8Encoding(false));

            Debug.Log($"Benchmark environment exported to: {filePath}");
        }

        public void ExportConfiguration(
            BenchmarkConfiguration config,
            string directoryPath)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            Directory.CreateDirectory(directoryPath);

            string filePath =
                Path.Combine(directoryPath, "benchmark_config.json");

            string json =
                JsonUtility.ToJson(
                    config,
                    true);

            File.WriteAllText(
                filePath,
                json,
                new UTF8Encoding(false));

            Debug.Log($"Benchmark configuration exported to: {filePath}");
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            bool requiresQuotes =
                value.Contains(",") ||
                value.Contains("\"") ||
                value.Contains("\n") ||
                value.Contains("\r");

            if (!requiresQuotes)
                return value;

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}