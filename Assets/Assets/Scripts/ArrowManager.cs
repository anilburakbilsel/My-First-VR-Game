using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour {

    public static ArrowManager Instance;

    public SteamVR_TrackedObject trackedObj;

    private GameObject currentArrow;

    public GameObject arrowPrefab;

    public GameObject stringAttachPoint;
    public GameObject arrowStartPoint;
    public GameObject stringStartPoint;

    private bool isAttached = false;

    void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AttachArrow();
        PullString();
	}

    private void PullString() {
        if(isAttached) {
            float dist = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
            stringAttachPoint.transform.localPosition = stringStartPoint.transform.localPosition + new Vector3(10f * dist, 0f, 0f);

            var device = SteamVR_Controller.Input((int)ArrowManager.Instance.trackedObj.index);
            if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
                FireArrow(dist);
            }
        }
    }

    private void FireArrow(float dist) {
        currentArrow.transform.parent = null;
        currentArrow.GetComponent<Arrow>().Fired();

        var arrow = currentArrow.GetComponent<Rigidbody>();
        arrow.velocity = currentArrow.transform.forward * 30f*dist;
        arrow.useGravity = true;

        currentArrow.GetComponent<Collider>().isTrigger = false;

        currentArrow = null;
        isAttached = false;
        stringAttachPoint.transform.position = stringStartPoint.transform.position;
    }

    private void AttachArrow() {
        if(currentArrow == null) {
            currentArrow = Instantiate(arrowPrefab);
            currentArrow.transform.parent = trackedObj.transform;
            currentArrow.transform.localPosition = new Vector3(0f, 0f, 0.342f);
            currentArrow.transform.localRotation = Quaternion.identity;
        }
    }

    public void AttachBowToArrow() {
        currentArrow.transform.parent = stringAttachPoint.transform;
        currentArrow.transform.rotation = arrowStartPoint.transform.rotation;
        currentArrow.transform.localPosition = arrowStartPoint.transform.localPosition;

        isAttached = true;
    }
}
