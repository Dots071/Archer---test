using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is in charge of aiming the bow once clicked on the mouse.
public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float mouseSensitivity = 100f;

    private float xRotation = 0f;



    private void LateUpdate()
    {
        if (PlayerController.Instance.CurrentBowState == PlayerController.BowState.PULLED)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerTransform.Rotate(Vector3.up, mouseX);
        }
    }
}
