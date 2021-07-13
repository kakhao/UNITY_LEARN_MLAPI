using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public static string UserName = "";
    private GameObject StartPanel;
    private GameObject ExitButton;
    public static Color UserColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        StartPanel = transform.Find("Panel").gameObject;
        ExitButton = transform.Find("butExit").gameObject;
        OnStartStop(false);

        UserName = PlayerPrefs.GetString("UserName", "");
        FindInputField("UserName").text = UserName;

        FindInputField("IP").text = 
            myNetworkManager.GetComponent<MLAPI.Transports.UNET.UNetTransport>().ConnectAddress;

    }

    // Update is called once per frame
    void Update()
    {

    }

    NetworkManager myNetworkManager
    {
        get
        {
            return NetworkManager.Singleton;
        }
    }


    InputField FindInputField(string FieldName)
    {
        return transform.Find("Panel").Find(FieldName).GetComponent<InputField>();
    }

    public void OnUpdateUserName()
    {
        UserName = FindInputField("UserName").text;
        PlayerPrefs.SetString("UserName", UserName);
    }

    public void ButtonHost()
    {
        myNetworkManager.StartHost();
        OnStartStop(true);
    }

    public void ButtonClient()
    {
        myNetworkManager.GetComponent<MLAPI.Transports.UNET.UNetTransport>().ConnectAddress = 
                    FindInputField("IP").text;
        myNetworkManager.StartClient();
        OnStartStop(true);
    }

    public void OnStartStop(bool IsStart)
    {
        StartPanel.SetActive(!IsStart);
        ExitButton.SetActive(IsStart);
    }


    public void ButtonExit()
    {
        if (NetworkManager.Singleton.IsClient)
            myNetworkManager.StopClient();
        else
            myNetworkManager.StopHost();

        OnStartStop(false);
    }

    public void ChangeColor()
    {
        UserColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        transform.Find("Panel").Find("butChangeColor").GetComponent<Image>().color = UserColor;
    }


}
