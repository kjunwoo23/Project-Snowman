using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public CinemachineVirtualCamera vcam;
    public Transform camLookAtDefault;
    public Transform map;
    public Transform mapPlane;

    public float moveSpeed;
    public float zoomSpeed;
    Transform cam;

    bool drag = false;
    Vector2 prevPos = Vector2.zero;
    float prevDistance = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //cam = Camera.main.transform;
        cam = vcam.transform;
    }

    private void FixedUpdate()
    {
        if (drag) OnDrag();
    }

    private void OnDrag()
    {
        int touchCount = Input.touchCount;
        //Debug.Log(Input.touchCount);
        if (touchCount == 1)
        {
            if (prevPos == Vector2.zero)
            {
                prevPos = Input.GetTouch(0).position;
                return;
            }
            float gap = (Input.GetTouch(0).position - prevPos).magnitude;
            Vector2 dir = (Input.GetTouch(0).position - prevPos).normalized;
            Vector3 vec = new Vector3(dir.x, 0, dir.y);

            //cam.position -= vec * moveSpeed * Time.deltaTime;
            if (!CubeController.instance.cubeMoving && !CubeController.instance.cubeMixing)
                if (dir.x < 0)
                    map.Rotate(mapPlane.up * moveSpeed * gap * Time.fixedDeltaTime);
                else if (dir.x > 0)
                    map.Rotate(-mapPlane.up * moveSpeed * gap * Time.fixedDeltaTime);

            prevPos = Input.GetTouch(0).position;
        }
        else if (touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1Before = touch1.position - touch1.deltaPosition;
            Vector2 touch2Before = touch2.position - touch2.deltaPosition;

            float beforePosDistance = (touch1Before - touch2Before).magnitude;
            float currentPosDistance = (touch1.position - touch2.position).magnitude;

            float zoomSize = (touch1.deltaPosition - touch2.deltaPosition).magnitude * zoomSpeed;


            if (beforePosDistance > currentPosDistance)
            {
                if (cam.position.y < 15)
                    cam.position += -cam.forward * zoomSize * Time.fixedDeltaTime;
            }
            else if (beforePosDistance < currentPosDistance)
                if (cam.position.y > 2)
                    cam.position += cam.forward * zoomSize * Time.fixedDeltaTime;
            if (cam.position.y < 2)
                cam.position = new Vector3(-2.783301e-09f, 2.018785f, -2.603629f);
        }

    }
    public void BeginDrag()
    {
        drag = true;
    }
    public void ExitDrag()
    {
        drag = false;
        prevPos = Vector2.zero;
        prevDistance = 0f;
    }

    public IEnumerator ResetMapRotation()
    {
        //vcam.transform.position = new Vector3(0, 4, -6);
        //Debug.Log(map.rotation.y);
        if (map.rotation.y > 0.05f)
            while (map.rotation.y > 0.05f)
            {
                map.Rotate(-mapPlane.up * moveSpeed * 5 * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
        else if (map.rotation.y < -0.05f)
            while (map.rotation.y < -0.05f)
            {
                map.Rotate(mapPlane.up * moveSpeed * 5 * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
        map.rotation = Quaternion.Euler(0, 0, 0);
    }
}
