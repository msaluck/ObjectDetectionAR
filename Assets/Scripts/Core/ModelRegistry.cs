using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

namespace ObjectDetectionAR.Core
{
    public class ModelRegistry : MonoBehaviour
    {
        [System.Serializable]
        public class ModelConfig
        {
            public string Name;
            public NNModel Model;
        }

        [SerializeField]
        private List<ModelConfig> models = new();

        private Dictionary<string, NNModel> registry;

        private void Awake()
        {
            registry = new Dictionary<string, NNModel>();

            foreach (var model in models)
            {
                registry[model.Name] = model.Model;
            }
        }

        public NNModel Get(string name)
        {
            if (registry.TryGetValue(name, out var model))
                return model;

            throw new KeyNotFoundException($"Model '{name}' is not registered.");
        }

        public List<string> RegisteredModels
        {
            get
            {
                return new List<string>(registry.Keys);
            }
        }

        public IReadOnlyList<string> GetModelNames()
        {
            return RegisteredModels;
        }
    }
}