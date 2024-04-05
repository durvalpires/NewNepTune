using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPianoCameraManager : MonoBehaviour
{
    public GameObject virtualPianoCamera;
    private CameraMovementG1L1 _cameraMovementG1L1;
    [SerializeField]
    private float startPoint, endPoint;
    
    void Start()
    {
        _cameraMovementG1L1 = FindObjectOfType<CameraMovementG1L1>();
        MoveCamera();
    }
    
    private void MoveCamera()
    {
        StartCoroutine(MoveCameraCoroutine());
    }

    private IEnumerator MoveCameraCoroutine()
    {
        float duration = 10f; // Set the duration for the camera movement
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xPosition = Mathf.Lerp(startPoint, endPoint, elapsedTime / duration);
            virtualPianoCamera.transform.position = new Vector3(xPosition, virtualPianoCamera.transform.position.y, virtualPianoCamera.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera ends exactly at the end point
        virtualPianoCamera.transform.position = new Vector3(endPoint, virtualPianoCamera.transform.position.y, virtualPianoCamera.transform.position.z);
    }
}
