using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform cameraTarget;
    public NetworkObject networkObject;
    // Start is called before the first frame update
    void Start()
    {
        if(!networkObject.IsOwner)
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraTarget.position;
        transform.rotation = cameraTarget.rotation;
    }
}
