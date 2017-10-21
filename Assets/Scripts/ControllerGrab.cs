using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControllerGrab : MonoBehaviour {

    private SteamVR_TrackedController controller;
    private SteamVR_Controller.Device device;
    private List<Collider> collisions = new List<Collider>();
    private GameObject holding;

    private void OnEnable()
    {
        controller = transform.parent.GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += HandleTriggerClick;
        controller.TriggerUnclicked += HandleTriggerUnclick;
    }

    // Use this for initialization
    void Start()
    {
        device = SteamVR_Controller.Input((int)controller.controllerIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // For when you're really feeling triggered
    private void HandleTriggerClick(object sender, ClickedEventArgs e)
    {
        Collider toPickUp = collisions.FirstOrDefault(col => col.gameObject.tag == Tag.GRABABLE);
        if (toPickUp != null)
        {
            // Pick up behaviour
            toPickUp.GetComponent<Rigidbody>().isKinematic = true;
            toPickUp.transform.SetParent(controller.transform);
            holding = toPickUp.gameObject;

        } else
        {
            // Other trigger behaviour
            // IM TRIGERRRRRRRRRRRRRED
        }
    }

    private void HandleTriggerUnclick(object sender, ClickedEventArgs e)
    {
        if (holding != null)
        {
            if (holding.transform.parent == controller.transform)
            {
                device = SteamVR_Controller.Input((int)controller.controllerIndex);
                Debug.Log(device.index);
                Debug.Log(device.velocity);
                Debug.Log("Object thrown");
                holding.transform.SetParent(null);
                holding.GetComponent<Rigidbody>().isKinematic = false;
                holding.GetComponent<Rigidbody>().velocity = device.velocity;
                holding.GetComponent<Rigidbody>().angularVelocity = device.angularVelocity;
            }
            holding = null;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == Tag.GRABABLE)
        {
            Debug.Log("Object collided with is grabable.");
            SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(1000);
        }
        col.SendMessage("OnControllerStartTouch", transform, SendMessageOptions.DontRequireReceiver);
        collisions.Add(col);
    }

    private void OnTriggerExit(Collider col)
    {
        col.SendMessage("OnControllerEndTouch", transform, SendMessageOptions.DontRequireReceiver);
        collisions.Remove(col);
    }

    
}
