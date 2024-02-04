using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSens = 2f;
    float cameraRot = 0;

    internal Ray ScreenPointToRay(Vector3 mousePosition)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        /*Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;*/
    }

    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSens;
        float inputY = Input.GetAxis("Mouse Y") * mouseSens;

        cameraRot -= inputY;
        cameraRot = Mathf.Clamp(cameraRot, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraRot;

        player.Rotate(Vector3.up * inputX);
    }
}
