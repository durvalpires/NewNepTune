using UnityEngine;
using UnityEngine.Events;

public class ActiveStateWatcher : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnBecameActive;
    public UnityEvent OnBecameInactive;

    private bool previousActiveState;

    private void OnEnable()
    {
        OnBecameActive?.Invoke();
    }

    private void OnDisable()
    {
        OnBecameInactive?.Invoke();
    }
}