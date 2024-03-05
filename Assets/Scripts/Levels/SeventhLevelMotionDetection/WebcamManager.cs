using UnityEngine;
using Unity.Barracuda;

public class WebCamManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("webcam manager kodu yorumda, emir abiden güncel veri seti bekleniyor");
    }
   /* public NNModel modelAsset;
    private Model runtimeModel;
    private IWorker worker;

    private WebCamTexture webcamTexture;
    public int webcamWidth = 640;
    public int webcamHeight = 480;

    void Start()
    {
        webcamTexture = new WebCamTexture(webcamWidth, webcamHeight);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();

        runtimeModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, runtimeModel);

        Debug.Log("Model Loaded with Input Count: " + runtimeModel.inputs.Count);
        Debug.Log("Model Input Description: " + runtimeModel.inputs[0]);
    }

    void Update()
    {
        if (webcamTexture.didUpdateThisFrame)
        {
            using (Tensor inputTensor = TransformInput(webcamTexture))
            {
                Debug.Log("Input Tensor Shape: " + inputTensor.shape);

                // Modeli çalıştır ve çıktıyı elde et
                worker.Execute(inputTensor);
                Tensor output = worker.PeekOutput();

                Debug.Log("Output Tensor Shape: " + output.shape);

                ProcessOutput(output);

                output.Dispose();
            }
        }
    }

    Tensor TransformInput(WebCamTexture webCamTexture)
    {
        // Tensor oluşturma ve dönüştürme işlemleri...
        // ...
        return tensor;
    }

    void ProcessOutput(Tensor output)
    {
        // Model çıktısını işleme...
        // ...
    }

    void OnDestroy()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }
        if (worker != null)
        {
            worker.Dispose();
        }
    }*/
}
