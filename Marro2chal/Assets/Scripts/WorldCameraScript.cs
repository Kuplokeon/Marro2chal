using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldCameraScript : MonoBehaviour
{
    public static WorldCameraScript instance;

    public Transform cam;
    public Transform target;
    public float looseness = 0.05f;
    public float rotateSpeed = 1;

    Dictionary<string, CamSetup> cameraSetups = new Dictionary<string, CamSetup>
    {
        { 
            "Wandering", 
            new CamSetup (
                new Vector3 (0, 6, -6),
                new Vector3 (0, 2, -2), 
                30,
                15
                ) 
        },
    };

    string camSetupInUse = "";

    float zoom = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (camSetupInUse == "")
        {
            camSetupInUse = cameraSetups.Keys.ToArray()[0];
        }

        if (target == null)
        {
            target = PlayerCharacter.instance.transform;
        }
    }

    public Vector3 RotateVectorToCamera(Vector3 inputVector)
    {
        Vector3 direction = GetDirectionFacing();

        return
            (inputVector.z * direction) +
            (inputVector.x * transform.right);
    }

    /// <summary>
    /// returns transform.forward, excluding the Y axis
    /// </summary>
    public Vector3 GetDirectionFacing()
    {
        Vector3 output = transform.forward;
        output.y = 0;
        return output.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
        RunCamSetup();
        RunRotateCamera();
    }

    void RunRotateCamera()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotateSpeed);
    }

    public void SetNewTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void FollowTarget()
    {
        transform.position += (target.position - transform.position) * looseness;
    }

    void RunCamSetup()
    {
        RunZoomControl();

        cam.transform.localPosition = CalculateZoomPos();
        cam.transform.localRotation = Quaternion.Euler(CalculateCameraXRotation(), 0, 0);
    }

    CamSetup GetCurrentSetup()
    {
        return cameraSetups[camSetupInUse];
    }

    float CalculateCameraXRotation()
    {
        CamSetup currentSetup = GetCurrentSetup();
        return currentSetup.minXRotation + ((currentSetup.maxXRotation - currentSetup.minXRotation) * zoom);
    }

    Vector3 CalculateZoomPos()
    {
        CamSetup currentSetup = GetCurrentSetup();
        return currentSetup.minZoom + ((currentSetup.maxZoom - currentSetup.minZoom) * zoom);
    }

    void RunZoomControl()
    {
        zoom += Input.mouseScrollDelta.y * 0.1f;

        if (zoom > 1)
        {
            zoom = 1;
        }

        if (zoom < 0)
        {
            zoom = 0;
        }
    }
}

class CamSetup
{
    public Vector3 minZoom;
    public Vector3 maxZoom;
    public float minXRotation;
    public float maxXRotation;

    public CamSetup(Vector3 minZoom, Vector3 maxZoom, float minXRotation, float maxXRotation)
    {
        this.minZoom = minZoom;
        this.maxZoom = maxZoom;
        this.minXRotation = minXRotation;
        this.maxXRotation = maxXRotation;
    }
}
