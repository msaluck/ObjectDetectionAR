using Unity.Barracuda;
using UnityEngine;
using ObjectDetectionAR.Interfaces;

namespace ObjectDetectionAR.ModelRunner
{
    public class BarracudaRunner : MonoBehaviour, IModelRunner
    {
        [SerializeField] private NNModel modelAsset;

        [SerializeField] private string modelName;

        public string ModelName => modelName;

        private Model runtimeModel;

        private IWorker worker;

        [SerializeField] private WorkerFactory.Type workerType = WorkerFactory.Type.Auto;

        private void Awake()
        {
            LoadModel(modelName, modelAsset);
        }

        public void LoadModel(string name, NNModel model)
        {
            if (model == null)
                throw new System.ArgumentNullException(nameof(model));
            Dispose();
            modelName = name;
            runtimeModel = ModelLoader.Load(model);

            worker = WorkerFactory.CreateWorker(
                workerType,
                runtimeModel);
        }

        public Tensor Execute(Tensor input)
        {
            worker.Execute(input);

            Tensor output = worker.PeekOutput();

            return output;
        }

        public void Dispose()
        {
            worker?.Dispose();
            worker = null;
            runtimeModel = null;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public bool IsLoaded
        {
            get
            {
                return worker != null;
            }
        }
        public string BackendName
        {
            get
            {
                return workerType.ToString();
            }
        }
    }
}