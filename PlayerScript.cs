using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class PlayerScript : NetworkBehaviour
{
    public float ForwardSpeed = 10;
    public float RotateSpeed = 200;

    private NetworkVariable<string> UserName = new NetworkVariable<string>();
    private NetworkVariable<Color> UserColor = new NetworkVariable<Color>();

    void UserNameValueChanged(string prevV, string newV)
    {
        transform.Find("PlayerText").GetComponent<TextMesh>().text = newV;
    }
    void UserColorValueChanged(Color prevV, Color newV)
    {
        GetComponent<MeshRenderer>().material.color = newV;
    }

    public override void NetworkStart()
    {
        print("Network Start , IsOwner:"+IsOwner.ToString());

        UserName.Settings.WritePermission = NetworkVariablePermission.OwnerOnly;
        UserName.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        UserColor.Settings.WritePermission = NetworkVariablePermission.OwnerOnly;
        UserColor.Settings.ReadPermission = NetworkVariablePermission.Everyone;

        UserName.OnValueChanged += UserNameValueChanged;
        UserColor.OnValueChanged += UserColorValueChanged;

        if(!IsOwner)
        {
            UserNameValueChanged("", UserName.Value);
            UserColorValueChanged(Color.black, UserColor.Value);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            CameraScript.SingleTone.target = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            float r = Input.GetAxis("Horizontal");
            float f = Input.GetAxis("Vertical");

            transform.Rotate(0, r * RotateSpeed * Time.deltaTime, 0);
            transform.Translate(0,0,f * ForwardSpeed * Time.deltaTime);

            //transform.Translate(x * Speed * Time.deltaTime, 0, z * Speed * Time.deltaTime);

            if (CanvasScript.UserName != UserName.Value)
            {
                UserName.Value = CanvasScript.UserName;
                UserColor.Value = CanvasScript.UserColor;
            }

        }
        
    }
}
