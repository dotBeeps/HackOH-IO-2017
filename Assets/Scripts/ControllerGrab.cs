using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControllerGrab : MonoBehaviour {

    private SteamVR_TrackedController controller;
    private List<Collider> collisions = new List<Collider>();
    private GameObject holding;

    private void OnEnable()
    {
        controller = transform.parent.GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += HandleTriggerClick;
        controller.TriggerUnclicked += HandleTriggerUnclick;
    }

    // For when you're really feeling triggered
    private void HandleTriggerClick(object sender, ClickedEventArgs e)
    {
        Collider toPickUp = collisions.First(col => col.gameObject.tag == Tag.GRABABLE);
        if (toPickUp != null)
        {
            // Pick up behaviour
            toPickUp.GetComponent<Rigidbody>().isKinematic = true;
            toPickUp.transform.parent = controller.transform;

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
            holding.GetComponent<Rigidbody>().isKinematic = false;
            holding.transform.parent = controller.transform.parent.parent;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == Tag.GRABABLE)
        {
            SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse();
        }
        col.SendMessage("OnControllerStartTouch", transform, SendMessageOptions.DontRequireReceiver);
        collisions.Add(col);
    }

    private void OnTriggerExit(Collider col)
    {
        col.SendMessage("OnControllerEndTouch", transform, SendMessageOptions.DontRequireReceiver);
        collisions.Remove(col);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
