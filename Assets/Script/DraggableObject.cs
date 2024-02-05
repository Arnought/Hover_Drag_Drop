using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public float pickupDist = 2f;
    public Camera playerCamera;

    private bool isPickedUp = false;
    private Vector3 offset;

    void Start()
    {
        if (playerCamera == null) { playerCamera = Camera.main; }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 20;

        // Display pickup/drop text only when looking at the object
        if (isPickedUp)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 40), "Drop", style);
        }
        else if (IsLookingAtObject())
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 40), "Pick up", style);
        }
    }

    void Update()
    {
        CheckProximity();

        if (isPickedUp)
        {
            UpdatePosition();
            CheckDrop();
        }
    }

    void CheckProximity()
    {
        // Create a ray from the player's camera to the current mouse position on the screen.
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        // Perform a raycast to detect all objects intersecting with the ray within the specified pickup distance.
        RaycastHit[] hits = Physics.RaycastAll(ray, pickupDist);

        foreach (RaycastHit hit in hits)
        {
            DraggableObject draggableObject = hit.collider.GetComponent<DraggableObject>();

            if (draggableObject != null && draggableObject == this)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PickUp();
                }

                return;
            }
        }
    }

    bool IsLookingAtObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDist))
        {
            return hit.collider.GetComponent<DraggableObject>() == this;
        }

        return false;
    }

    void PickUp()
    {
        isPickedUp = !isPickedUp;

        if (isPickedUp)
        {
            offset = transform.position - playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, pickupDist));
        }
    }

    void UpdatePosition()
    {
        Vector3 currPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pickupDist);
        Vector3 newPos = playerCamera.ScreenToWorldPoint(currPos) + offset;

        // Update the x, y, and z positions of the object to follow the camera's rotation
        transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
    }

    void CheckDrop()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PickUp(); // Toggle state on mouse release
        }
    }
}
