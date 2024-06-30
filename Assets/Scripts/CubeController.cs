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
    public bool cubeMixing;
    public float mixSpeed;
    public VariableJoystick joystick;


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

        //if (Input.GetKeyDown(KeyCode.R))
            //StartMixCubes();

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
                        focusChangeCool = 0.3f;
                        UnFocus();
                    }
                    Debug.DrawRay(worldPos, (worldPos - CameraController.instance.vcam.transform.position).normalized * 999, Color.red);
                }
            }
        if (controlTarget != -1)
            if (!cubeMoving)
            {
                bool somethingUp = false;
                //bool blocked = false;
                //bool up = false;
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

                    /*if (Input.GetKey(KeyCode.DownArrow))
                        joyStickDir = new Vector2(0, -1);
                    else if (Input.GetKey(KeyCode.UpArrow))
                        joyStickDir = new Vector2(0, 1);
                    else if (Input.GetKey(KeyCode.RightArrow))
                        joyStickDir = new Vector2(1, 0);
                    else if (Input.GetKey(KeyCode.LeftArrow))
                        joyStickDir = new Vector2(-1, 0);*/

                    //Debug.Log(joystick.Direction);

                    //Debug.Log(joystick.Vertical + ", " + joystick.Horizontal);

                    if (joystick.Direction != Vector2.zero)
                        if (Mathf.Abs(joystick.Vertical) > Mathf.Abs(joystick.Horizontal))
                        {
                            if (joystick.Vertical < -0.5f)
                                joyStickDir = new Vector2(0, -1);
                            else if (joystick.Vertical > 0.5f)
                                joyStickDir = new Vector2(0, 1);
                        }
                        else if (Mathf.Abs(joystick.Vertical) < Mathf.Abs(joystick.Horizontal))
                        {
                            if (joystick.Horizontal > 0.5f)
                                joyStickDir = new Vector2(1, 0);
                            else if(joystick.Horizontal < -0.5f)
                                joyStickDir = new Vector2(-1, 0);
                        }

                    /*
                    for (int i = 0; i < allCubes.Length; i++)
                        if (i != controlTarget)
                            if (Mathf.Abs(allCubes[i].transform.position.x - (allCubes[controlTarget].transform.position.x + joyStickDir.x)) < 0.2f
                            && Mathf.Abs(allCubes[i].transform.position.y - allCubes[controlTarget].transform.position.y) < 0.2f
                            && Mathf.Abs(allCubes[i].transform.position.z - (allCubes[controlTarget].transform.position.z + joyStickDir.y)) < 0.2f)
                            {
                                up = true;
                                break;
                            }
                    if (Mathf.Abs(allCubes[controlTarget].transform.position.x + joyStickDir.x) > 2.5f
                        || Mathf.Abs(allCubes[controlTarget].transform.position.z + joyStickDir.y) > 2.5f)
                        blocked = true;*/

                    if (joyStickDir != Vector2.zero)
                    {
                        cubeMoving = true;
                        allCubes[controlTarget].StartMoveCube(joyStickDir);
                    }
                }
            }
    }
    void UnFocus()
    {
        if (controlTarget == -1) return;
        if (joystick.Direction != Vector2.zero)
        {
            focusChangeCool = 0.1f;
            return;
        }
        joystick.gameObject.SetActive(false);
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
    public bool CheckUp(Vector2 joyStickDir)
    {
        bool up = false;
        for (int i = 0; i < allCubes.Length; i++)
            if (i != controlTarget)
                if (Mathf.Abs(allCubes[i].transform.position.x - (allCubes[controlTarget].transform.position.x + joyStickDir.x)) < 0.2f
                && Mathf.Abs(allCubes[i].transform.position.y - allCubes[controlTarget].transform.position.y) < 0.2f
                && Mathf.Abs(allCubes[i].transform.position.z - (allCubes[controlTarget].transform.position.z + joyStickDir.y)) < 0.2f)
                {
                    up = true;
                    break;
                }
        return up;
    }
    public bool CheckUp(int cubeNum, Vector2 joyStickDir)
    {
        bool up = false;
        for (int i = 0; i < allCubes.Length; i++)
            if (i != cubeNum)
                if (Mathf.Abs(allCubes[i].transform.position.x - (allCubes[cubeNum].transform.position.x + joyStickDir.x)) < 0.2f
                && Mathf.Abs(allCubes[i].transform.position.y - allCubes[cubeNum].transform.position.y) < 0.2f
                && Mathf.Abs(allCubes[i].transform.position.z - (allCubes[cubeNum].transform.position.z + joyStickDir.y)) < 0.2f)
                {
                    up = true;
                    break;
                }
        return up;
    }

    public bool CheckBlocked(Vector2 joyStickDir)
    {
        bool blocked = false;
        if (Mathf.Abs(allCubes[controlTarget].transform.position.x + joyStickDir.x) > 2.5f
                        || Mathf.Abs(allCubes[controlTarget].transform.position.z + joyStickDir.y) > 2.5f)
            blocked = true;
        return blocked;
    }
    public bool CheckBlocked(int cubeNum, Vector2 joyStickDir)
    {
        bool blocked = false;
        if (Mathf.Abs(allCubes[cubeNum].transform.position.x + joyStickDir.x) > 2.5f
                        || Mathf.Abs(allCubes[cubeNum].transform.position.z + joyStickDir.y) > 2.5f)
            blocked = true;
        return blocked;
    }

    public void StartMixCubes()
    {
        if (!cubeMixing)
            StartCoroutine(MixCubes());

        if (MenuManager.instance.menuAnimator.GetBool("appear"))
            MenuManager.instance.menuAnimator.SetBool("appear", false);
    }
    public IEnumerator MixCubes()
    {
        if (cubeMoving || cubeMixing) yield break;
        cubeMixing = true;
        UnFocus();
        
        yield return StartCoroutine(CameraController.instance.ResetMapRotation());

        do
        {
            for (int i = 0; i < allCubes.Length; i++)
            {
                allCubes[i].transform.position = new Vector3(Random.Range(-2.499f, 2.499f), 0.5f, Random.Range(-2.499f, 2.499f));
                allCubes[i].transform.Rotate(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
            }
            RoundCubesPos();
        }
        while (allCubes[0].transform.position.x == allCubes[1].transform.position.x
        && allCubes[0].transform.position.z == allCubes[1].transform.position.z);

        /*for (int j = 0; j < 10; j++)
            for (int i = 0; i < allCubes.Length; i++)
            {
                controlTarget = i;
                yield return StartCoroutine(MixCube(allCubes[i], Random.Range(0, 4)));
            }
        controlTarget = -1;*/
        cubeMixing = false;
    }
    /*
    IEnumerator MixCube(Cube cube, int mixDir)
    {
        yield return StartCoroutine(CameraController.instance.ResetMapRotation());

        Vector2 joyStickDir = new Vector2(0, -1);
        switch (mixDir)
        {
            case 0: joyStickDir = new Vector2(0, -1); break;
            case 1: joyStickDir = new Vector2(0, 1); break;
            case 2: joyStickDir = new Vector2(1, 0); break;
            case 3: joyStickDir = new Vector2(-1, 0); break;
        }
        if (CheckBlocked(joyStickDir))
        {
            RoundCubesPos();
            yield break;
        }


        //CubeController.instance.RoundCubesPos();
        Transform rotateAxis = null;

        if (!CheckUp(joyStickDir))
        {
            for (int i = 0; i < cube.axis.Length; i++)
            {
                if (cube.axis[i].transform.position.y < cube.transform.position.y - 0.25f)
                    if (Mathf.Abs(cube.axis[i].transform.position.x - (cube.transform.position.x + joyStickDir.x * 0.5f)) < 0.1f)
                        if (Mathf.Abs(cube.axis[i].transform.position.z - (cube.transform.position.z + joyStickDir.y * 0.5f)) < 0.1f)
                        {
                            rotateAxis = cube.axis[i];
                            break;
                        }
            }
            //GetComponent<Rigidbody>().freezeRotation = false;
            while (transform.position.y >= 0.5f)
            {
                if (joyStickDir.x < 0)
                    cube.transform.RotateAround(rotateAxis.position, CameraController.instance.map.forward, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.x > 0)
                    cube.transform.RotateAround(rotateAxis.position, -CameraController.instance.map.forward, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y < 0)
                    cube.transform.RotateAround(rotateAxis.position, -CameraController.instance.map.right, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y > 0)
                    cube.transform.RotateAround(rotateAxis.position, CameraController.instance.map.right, mixSpeed * Time.fixedDeltaTime);
                //Debug.Log(transform.rotation.x);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            for (int i = 0; i < cube.axis.Length; i++)
            {
                if (cube.axis[i].transform.position.y > transform.position.y + 0.25f)
                    if (Mathf.Abs(cube.axis[i].transform.position.x - (cube.transform.position.x + joyStickDir.x * 0.5f)) < 0.1f)
                        if (Mathf.Abs(cube.axis[i].transform.position.z - (cube.transform.position.z + joyStickDir.y * 0.5f)) < 0.1f)
                        {
                            rotateAxis = cube.axis[i];
                            break;
                        }
            }
            //GetComponent<Rigidbody>().freezeRotation = false;
            while (cube.transform.position.y <= 1.5f)
            {
                if (joyStickDir.x < 0)
                    cube.transform.RotateAround(rotateAxis.position, CameraController.instance.map.forward, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.x > 0)
                    cube.transform.RotateAround(rotateAxis.position, -CameraController.instance.map.forward, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y < 0)
                    cube.transform.RotateAround(rotateAxis.position, -CameraController.instance.map.right, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y > 0)
                    cube.transform.RotateAround(rotateAxis.position, CameraController.instance.map.right, mixSpeed * Time.fixedDeltaTime);
                //Debug.Log(transform.rotation.x);
                yield return new WaitForFixedUpdate();
            }
            while (cube.transform.position.y >= 1.5f)
            {
                if (joyStickDir.x < 0)
                    cube.transform.RotateAround(rotateAxis.position, CameraController.instance.map.forward, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.x > 0)
                    cube.transform.RotateAround(rotateAxis.position, -CameraController.instance.map.forward, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y < 0)
                    cube.transform.RotateAround(rotateAxis.position, -CameraController.instance.map.right, mixSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y > 0)
                    cube.transform.RotateAround(rotateAxis.position, CameraController.instance.map.right, mixSpeed * Time.fixedDeltaTime);
                //Debug.Log(transform.rotation.x);
                yield return new WaitForFixedUpdate();
            }
        }

        //GetComponent<Rigidbody>().freezeRotation = true;
        RoundCubesPos();
    }*/
}
