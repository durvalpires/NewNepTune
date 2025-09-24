using UnityEngine;

public class UITextPresenter : MonoBehaviour
{
   [SerializeField] private UITextField view;

    public void UpdateView(string newValue)
    {
        view.SetText(newValue);
    }
}