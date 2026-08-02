using UnityEngine;
using ObjectDetectionAR.Benchmark;

public class BenchmarkRunnerTest : MonoBehaviour
{
    [SerializeField] private BenchmarkRunner benchmarkRunner;

    void Start()
    {
        benchmarkRunner.RunAllBenchmarks();
    }
}