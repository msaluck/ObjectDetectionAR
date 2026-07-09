using UnityEngine;

namespace ObjectDetectionAR.Utils
{
    public static class Logger
    {
        public static bool EnableDebug { get; set; } = true;

        public static void Log(object message)
        {
            if (EnableDebug)
                Debug.Log(message);
        }

        public static void Warning(object message)
        {
            Debug.LogWarning(message);
        }

        public static void Error(object message)
        {
            Debug.LogError(message);
        }
    }
}