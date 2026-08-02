using System;

namespace ObjectDetectionAR.Benchmark
{
    [Serializable]
    public class BenchmarkEnvironment
    {
        // Framework
        public string BenchmarkFrameworkVersion;

        // Time
        public string Timestamp;

        // Unity
        public string UnityVersion;
        public string Platform;

        // Operating System
        public string OperatingSystem;

        // CPU
        public string ProcessorType;
        public int ProcessorCount;

        // Memory
        public int SystemMemoryMB;

        // GPU
        public string GraphicsDeviceName;
        public string GraphicsDeviceVendor;
        public int GraphicsMemoryMB;
        public string GraphicsAPI;

        // Device
        public string DeviceModel;
        public string DeviceName;
        public string DeviceType;
    }
}
