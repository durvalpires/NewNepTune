using System;
using TMPro;
using UnityEngine;

public class ProfileDevTemp : MonoBehaviour
{
    [SerializeField] private TMP_Text _profileName;
    [SerializeField] private TMP_InputField _inputName;
    [SerializeField] private TMP_InputField _inputPass;
    [SerializeField] private TMP_InputField _inputTestData;

    private void Start()
    {
        UpdateName();
    }

    public void Create()
    {
        ProfilesController.CreateProfile(_inputName.text, _inputPass.text);
        UpdateName();
    }
    public void Login()
    {
        ProfilesController.SwitchProfile(_inputName.text, _inputPass.text);
        UpdateName();
    }
   
    public void SwitchToGuest()
    {
        ProfilesController.SwitchToGuest();
        UpdateName();
    }

    private void UpdateName()
    {
        _profileName.text = ProfilesController.CurrentProfile.NickName;
    }
    public void PrintState()
    {
        Debug.Log("Current: "+ProfilesController.CurrentProfile.NickName);
        var all = ProfilesController.GetAllProfiles();
        ;
        Debug.Log("Found: "+all.Count+" Profiles");
        var list = ProfilesController.GetAllProfiles();
        foreach (var profile in list)
        {
            Debug.Log(profile.NickName);
        }
    }

    public void SetTestData()
    {
        PlayerModel.SetCustomData("TestData", _inputTestData.text);
    }
    public void GetTestData()
    {
        _inputTestData.text = PlayerModel.GetCustomData("TestData");
        Debug.Log(PlayerModel.GetCustomData("TestData"));
    }
}
