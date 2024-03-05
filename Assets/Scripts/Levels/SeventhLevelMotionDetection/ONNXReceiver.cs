using UnityEngine;
using Unity.Barracuda;

public class ONNXReceiver : MonoBehaviour
{
    public NNModel modelAsset;
    private Model model;
    private IWorker worker;

    void Start()
    {
        model = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    void Update()
    {
        // Girdi verilerini hazırla ve modeli çalıştır
        Tensor input = new Tensor(1, 3, 224, 224); // Örnek bir girdi tensoru
        worker.Execute(input);
        Tensor output = worker.PeekOutput();

        // Burada çıktıyı işleyin

        input.Dispose();
        output.Dispose();
    }

    void OnDestroy()
    {
        worker.Dispose();
    }
}
