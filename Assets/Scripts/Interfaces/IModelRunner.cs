using Unity.Barracuda;

namespace ObjectDetectionAR.Interfaces
{
    public interface IModelRunner
    {
        void LoadModel(NNModel model);

        Tensor Execute(Tensor input);

        void Dispose();

        bool IsLoaded { get; }

        string BackendName { get; }
    }
}