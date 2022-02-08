using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Experimental.XR;


public class ARTapToPlaceObject : MonoBehaviour
{

    public GameObject objectToPlace;

    public GameObject placementIndicator;

    private ARRaycastManager rayCastMgr;
    private bool placementPoseIsValid = false;
    private Pose placeMentPose;

    // Start is called before the first frame update
    void Start()
    {
        rayCastMgr = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            PlaceObject();
        }

    }

    private void UpdatePlacementPose(){
        
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        rayCastMgr.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid){
            placeMentPose = hits[0].pose; 

            var cameraFoward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraFoward.x, 0, cameraFoward.z).normalized;
            placeMentPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
    }

    private void UpdatePlacementIndicator(){

        if (placementPoseIsValid){
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placeMentPose.position, placeMentPose.rotation);
        } else {
            placementIndicator.SetActive(false);
        }

    }

    private void PlaceObject(){
        Instantiate(objectToPlace, placeMentPose.position, placeMentPose.rotation);
    }
}
