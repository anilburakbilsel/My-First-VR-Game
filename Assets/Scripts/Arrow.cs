using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    private bool isAttached = false;
    private bool isFired = false;

    private Quaternion q;
    private Vector3 v3;
    private bool hasHit = false;

    void OnTriggerStay(Collider collider) {
        if (collider.gameObject.tag.Equals("Golden Bow")) {
            AttachArrow();
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isFired) {
            transform.LookAt(transform.position + transform.GetComponent<Rigidbody>().velocity);
        }
	}

    public void Fired() {
        isFired = true;
        isAttached = false;
    }

    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag.Equals("Golden Bow")) {
            AttachArrow();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(!collision.gameObject.tag.Equals("Golden Bow") && !collision.gameObject.tag.Equals("Arrow")) {
            hasHit = true;
        }
    }

    void LateUpdate() {
        if (hasHit) {
            transform.position = v3;
            transform.rotation = q;
            this.GetComponent<Rigidbody>().isKinematic = true;
            Object.Destroy(this.gameObject, 5.0f);
            hasHit = false;
        } else {
            v3 = transform.position;
            q = transform.rotation;
        }
    }

    private void AttachArrow() {
        var device = SteamVR_Controller.Input((int)ArrowManager.Instance.trackedObj.index);
        if(device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            ArrowManager.Instance.AttachBowToArrow();
            isAttached = true;
        }
    }

}
