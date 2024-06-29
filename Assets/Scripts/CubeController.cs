using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public static CubeController instance;
    public Cube[] allCubes;
    public int controlTarget;
    //public Cube controlTarget;
    public bool cubeMoving;

    RaycastHit hit;
    float focusChangeCool;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (focusChangeCool >= 0)
            focusChangeCool -= Time.deltaTime;
        if (focusChangeCool < 0)
            if (Input.touchCount == 1)
            {
                Vector3 touchPos = Input.GetTouch(0).position;
                touchPos.z = Camera.main.nearClipPlane;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);
                if (Physics.Raycast(worldPos, (worldPos - CameraController.instance.vcam.transform.position).normalized, out hit, 999, LayerMask.GetMask("Cube")))
                {
                    //Debug.Log(touchPos + ", " + worldPos);
                    //Debug.Log("hit point : " + hit.point + ", distance : " + hit.distance + ", name : " + hit.collider.name);
                    Debug.DrawRay(worldPos, (worldPos - CameraController.instance.vcam.transform.position).normalized * hit.distance, Color.red);
                    if (controlTarget != -1) allCubes[controlTarget].focused = false;
                    hit.collider.GetComponent<Cube>().FocusMe();
                    focusChangeCool = 0.3f;
                }
                else
                {
                    if (controlTarget != -1)
                    {
                        UnFocus();
                        focusChangeCool = 0.3f;
                    }
                    Debug.DrawRay(worldPos, (worldPos - CameraController.instance.vcam.transform.position).normalized * 999, Color.red);
                }
            }
        if (controlTarget != -1)
            if (!cubeMoving)
            {
                bool somethingUp = false;
                bool up = false;
                for (int i = 0; i < allCubes.Length; i++)
                    if (i != controlTarget)
                        if (Mathf.Abs(allCubes[i].transform.position.x - allCubes[controlTarget].transform.position.x) < 0.2f
                            && Mathf.Abs(allCubes[i].transform.position.z - allCubes[controlTarget].transform.position.z) < 0.2f
                            && allCubes[i].transform.position.y > allCubes[controlTarget].transform.position.y)
                        {
                            somethingUp = true;
                            break;
                        }

                if (!somethingUp)
                {
                    Vector2 joyStickDir = Vector2.zero;
                    if (Input.GetKey(KeyCode.DownArrow))
                        joyStickDir = new Vector2(0, -1);
                    else if (Input.GetKey(KeyCode.UpArrow))
                        joyStickDir = new Vector2(0, 1);
                    else if (Input.GetKey(KeyCode.RightArrow))
                        joyStickDir = new Vector2(1, 0);
                    else if (Input.GetKey(KeyCode.LeftArrow))
                        joyStickDir = new Vector2(-1, 0);

                    for (int i = 0; i < allCubes.Length; i++)
                        if (i != controlTarget)
                            if (Mathf.Abs(allCubes[i].transform.position.x - (allCubes[controlTarget].transform.position.x + joyStickDir.x)) < 0.2f
                            && Mathf.Abs(allCubes[i].transform.position.y - allCubes[controlTarget].transform.position.y) < 0.2f
                            && Mathf.Abs(allCubes[i].transform.position.z - (allCubes[controlTarget].transform.position.z + joyStickDir.y)) < 0.2f)
                            {
                                up = true;
                                break;
                            }
                    if (joyStickDir != Vector2.zero)
                    {
                        cubeMoving = true;
                        allCubes[controlTarget].StartMoveCube(joyStickDir, up);
                    }
                }
            }
    }
    void UnFocus()
    {
        allCubes[controlTarget].focused = false;
        controlTarget = -1;
        CameraController.instance.vcam.LookAt = CameraController.instance.camLookAtDefault;
    }
    public void RoundCubesPos()
    {
        for (int i = 0; i < allCubes.Length; i++)
        {
            allCubes[i].transform.position = new Vector3(Mathf.Round(allCubes[i].transform.position.x),
            Mathf.Round(allCubes[i].transform.position.y * 2) / 2f, Mathf.Round(allCubes[i].transform.position.z));
            //Debug.Log("Before: " + allCubes[i].transform.rotation.x + ", " + allCubes[i].transform.rotation.y + ", " + allCubes[i].transform.rotation.z);
            //allCubes[i].transform.rotation = Quaternion.Euler(Mathf.Round(allCubes[i].transform.rotation.x * 2) * 90, Mathf.Round(allCubes[i].transform.rotation.y * 2) * 90, Mathf.Round(allCubes[i].transform.rotation.z * 2) * 90);
            allCubes[i].transform.eulerAngles = new Vector3(Mathf.Round(allCubes[i].transform.eulerAngles.x / 90) * 90, Mathf.Round(allCubes[i].transform.eulerAngles.y / 90) * 90, Mathf.Round(allCubes[i].transform.eulerAngles.z / 90) * 90);
            //Debug.Log("After: " + allCubes[i].transform.rotation.x + ", " + allCubes[i].transform.rotation.y + ", " + allCubes[i].transform.rotation.z);
        }
    }
}
