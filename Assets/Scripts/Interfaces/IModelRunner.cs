using Unity.Barracuda;

namespace ObjectDetectionAR.Interfaces
{
    public interface IModelRunner
{
    string ModelName { get; }

    string BackendName { get; }

    bool IsLoaded { get; }

    void LoadModel(
        string modelName,
        NNModel model);

    Tensor Execute(Tensor input);

    void Dispose();
}
}