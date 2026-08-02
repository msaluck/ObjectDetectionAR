# ObjectDetectionAR

Unity-based mobile AR object detection project that combines AR Foundation, ARCore, and Barracuda inference for on-device object detection.

## Overview

This project captures camera frames, preprocesses them into Barracuda tensors, runs inference with an ONNX model, and decodes detections into bounding boxes and labels for display in the scene.

Verified from the current project state:

- Unity Editor version: `2021.3.45f2`
- AR stack: `AR Foundation 4.2.10`, `ARCore 4.2.10`, `XR Management 4.5.2`
- Inference runtime: `Barracuda 3.0.2`
- Included models: `yolov5su.onnx`, `yolov8n.onnx`
- Included scene: `Assets/Scenes/SampleScene.unity`

## Features

- Real-time object detection pipeline for mobile AR scenarios
- Runtime model switching through the UI dropdown
- Multiple detector decoders, including YOLOv5, YOLOv8, and SSD code paths
- COCO label mapping for common object classes
- Benchmark runner with CSV and JSON export
- Alternative image-source scripts for AR camera, webcam, video, and static image workflows

## Project Structure

```text
Assets/
  Models/             ONNX model files
  Scenes/             Unity scenes
  Scripts/
    Benchmark/        Benchmark runner and exporters
    Core/             Detection pipeline, registries, result types
    Decoders/         YOLOv5, YOLOv8, SSD output decoders
    ImageSources/     AR camera, webcam, video, static-image inputs
    ModelRunner/      Barracuda model execution
    Preprocessing/    Texture -> Tensor preprocessing
    Rendering/        Bounding-box and label rendering
    UI/               Model selector and runtime UI
ProjectSettings/      Unity project configuration
Packages/             Unity package manifest and lock file
```

## Detection Pipeline

The main runtime flow is implemented around the following components:

1. `DetectionRunner` orchestrates the end-to-end pipeline.
2. An image source provides the current frame.
3. `ImagePreprocessor` resizes the input to `640x640` and converts it to a Barracuda tensor.
4. `BarracudaRunner` loads the selected `NNModel` and executes inference.
5. A decoder such as `YOLOv8Decoder` converts model output into detections.
6. Rendering scripts display boxes and labels in the UI.

The current label set is COCO-based and lives in `Assets/Scripts/Labels/CocoLabels.cs`.

## Requirements

- Unity Hub
- Unity Editor `2021.3.45f2`
- Android Build Support in Unity Hub if you want to build to device
- An ARCore-compatible Android device for the AR experience

## Getting Started

### 1. Open the project

Open this folder in Unity Hub:

```text
UnityProject/ObjectDetectionAR
```

Let Unity finish package restore and asset import on first launch.

### 2. Open the sample scene

Open:

```text
Assets/Scenes/SampleScene.unity
```

Note: `ProjectSettings/EditorBuildSettings.asset` currently has no scenes listed, so if you plan to create a build you should add `SampleScene` to Build Settings manually.

### 3. Verify model assets

The repository already includes these ONNX models:

- `Assets/Models/yolov5su.onnx`
- `Assets/Models/yolov8n.onnx`

If you add more models, register them through the project's model registry and ensure a matching decoder is configured.

### 4. Build for Android

For device testing:

1. Switch the active platform to Android.
2. Confirm ARCore support is enabled in XR settings.
3. Connect an ARCore-capable Android device.
4. Build and run from Unity.

The repository root also contains a prebuilt APK artifact:

- `object-detection-ar-barracuda.apk`

## Model Selection

The runtime UI includes a model selector backed by `ModelRegistry` and `ModelSelector`. On startup, the dropdown is populated from registered models, and selecting a new model reloads Barracuda with that asset.

## Benchmarking

Benchmark support is implemented in `Assets/Scripts/Benchmark`.

`BenchmarkRunner` can execute repeated runs for one model or every registered model and exports results to Unity's persistent data path under a timestamped folder:

```text
Benchmarks/<yyyy-MM-dd_HHmmss>/
```

Exported files include:

- `benchmark_runs.csv`
- `benchmark_summary.csv`
- `environment.json`
- `benchmark_config.json`

## Development Notes

- The default preprocessor input size is `640x640`.
- The current scene/build configuration is minimal and may require manual wiring in the Unity Editor depending on which source and renderer components you want active.
- Several utility and demo scripts exist under `Assets/Scripts/Demo` for local experimentation.
- Non-AR testing paths are available in code through webcam, video, and static-image source scripts.

## Useful Files

- `ProjectSettings/ProjectVersion.txt`
- `Packages/manifest.json`
- `Assets/Scripts/Core/DetectionRunner.cs`
- `Assets/Scripts/ModelRunner/BarracudaRunner.cs`
- `Assets/Scripts/Preprocessing/ImagePreprocessor.cs`
- `Assets/Scripts/Decoders/YOLOv8Decoder.cs`
- `Assets/Scripts/Benchmark/BenchmarkRunner.cs`

## Known Gaps

- Build Settings do not currently list the sample scene.
- Some repository scripts appear to be placeholders or editor-time scaffolding, so scene wiring in the Inspector remains the source of truth for the exact runtime setup.

## License

No license file is currently present in this repository. Add one if you intend to distribute the project.