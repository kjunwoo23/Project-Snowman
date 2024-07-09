using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int cubeNum;
    public Transform[] axis;
    public float moveSpeed;
    public bool focused;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FocusMe()
    {
        focused = true;

        CubeController.instance.controlTarget = cubeNum;
        CameraController.instance.vcam.LookAt = transform;
        CubeController.instance.joystick.gameObject.SetActive(true);
    }

    public void StartMoveCube(Vector2 joyStickDir)
    {
        //Debug.Log(joyStickDir);
        StartCoroutine(MoveCube(joyStickDir));
    }
    IEnumerator MoveCube(Vector2 joyStickDir)
    {

        yield return CameraController.instance.StartCoroutine(CameraController.instance.ResetMapRotation());

        CubeController.instance.RoundCubesPos();

        if (CubeController.instance.CheckBlocked(cubeNum, joyStickDir))
        {
            CubeController.instance.RoundCubesPos();
            CubeController.instance.cubeMoving = false;
            yield break;
        }


        //CubeController.instance.RoundCubesPos();
        Transform rotateAxis = null;

        if (!CubeController.instance.CheckUp(cubeNum, joyStickDir))
        {
            for (int i = 0; i < axis.Length; i++)
            {
                if (axis[i].transform.position.y < transform.position.y - 0.25f)
                    if (Mathf.Abs(axis[i].transform.position.x - (transform.position.x + joyStickDir.x * 0.5f)) < 0.1f)
                        if (Mathf.Abs(axis[i].transform.position.z - (transform.position.z + joyStickDir.y * 0.5f)) < 0.1f)
                        {
                            rotateAxis = axis[i];
                            break;
                        }
            }
            //GetComponent<Rigidbody>().freezeRotation = false;
            while (transform.position.y >= 0.5f)
            {
                if (joyStickDir.x < 0)
                    transform.RotateAround(rotateAxis.position, CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.x > 0)
                    transform.RotateAround(rotateAxis.position, -CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y < 0)
                    transform.RotateAround(rotateAxis.position, -CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y > 0)
                    transform.RotateAround(rotateAxis.position, CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
                //Debug.Log(transform.rotation.x);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            for (int i = 0; i < axis.Length; i++)
            {
                if (axis[i].transform.position.y > transform.position.y + 0.25f)
                    if (Mathf.Abs(axis[i].transform.position.x - (transform.position.x + joyStickDir.x * 0.5f)) < 0.1f)
                        if (Mathf.Abs(axis[i].transform.position.z - (transform.position.z + joyStickDir.y * 0.5f)) < 0.1f)
                        {
                            rotateAxis = axis[i];
                            break;
                        }
            }
            //GetComponent<Rigidbody>().freezeRotation = false;
            while (transform.position.y <= 1.5f)
            {
                if (joyStickDir.x < 0)
                    transform.RotateAround(rotateAxis.position, CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.x > 0)
                    transform.RotateAround(rotateAxis.position, -CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y < 0)
                    transform.RotateAround(rotateAxis.position, -CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y > 0)
                    transform.RotateAround(rotateAxis.position, CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
                //Debug.Log(transform.rotation.x);
                yield return new WaitForFixedUpdate();
            }
            while (transform.position.y >= 1.5f)
            {
                if (joyStickDir.x < 0)
                    transform.RotateAround(rotateAxis.position, CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.x > 0)
                    transform.RotateAround(rotateAxis.position, -CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y < 0)
                    transform.RotateAround(rotateAxis.position, -CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
                else if (joyStickDir.y > 0)
                    transform.RotateAround(rotateAxis.position, CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
                //Debug.Log(transform.rotation.x);
                yield return new WaitForFixedUpdate();
            }
        }

        //GetComponent<Rigidbody>().freezeRotation = true;
        CubeController.instance.RoundCubesPos();

        if (transform.position == new Vector3(-2, 0.5f, 2))
            if (!CubeController.instance.CheckUp(cubeNum, new Vector2(1, 0)))
                yield return StartCoroutine(PushCube());

        if (!CubeController.instance.cleared)
        {
            bool failed = false;

            for (int i = 0; i < CubeController.instance.allCubes.Length; i++)
            {

                //Debug.Log(CubeController.instance.allCubes[i].transform.forward + ", " + CubeController.instance.allCubes[i].transform.up);
                if ((CubeController.instance.allCubes[i].transform.forward != CameraController.instance.map.forward)
                    || (CubeController.instance.allCubes[i].transform.up != CameraController.instance.map.up)
                    || (i > 0 && CubeController.instance.allCubes[i].transform.position.y <= CubeController.instance.allCubes[i - 1].transform.position.y))
                {
                    failed = true;
                    break;
                }
            }
            //Debug.Log(failed);
            if (!failed)
            {
                CubeController.instance.PuzzleCleared();
            }
        }
        CubeController.instance.cubeMoving = false;
    }
    /*IEnumerator MoveCubeUp(Vector2 realDir)
    {
        yield return StartCoroutine(CameraController.instance.ResetMapRotation());

        //CubeController.instance.RoundCubesPos();
        Transform rotateAxis = null;
        for (int i = 0; i < axis.Length; i++)
        {
            if (axis[i].transform.position.y > transform.position.y + 0.25f)
                if (Mathf.Abs(axis[i].transform.position.x - (transform.position.x + realDir.x * 0.5f)) < 0.1f)
                    if (Mathf.Abs(axis[i].transform.position.z - (transform.position.z + realDir.y * 0.5f)) < 0.1f)
                    {
                        rotateAxis = axis[i];
                        break;
                    }
        }
        //GetComponent<Rigidbody>().freezeRotation = false;
        while (transform.position.y <= 1.5f)
        {
            if (realDir.x < 0)
                transform.RotateAround(rotateAxis.position, CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
            else if (realDir.x > 0)
                transform.RotateAround(rotateAxis.position, -CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
            else if (realDir.y < 0)
                transform.RotateAround(rotateAxis.position, -CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
            else if (realDir.y > 0)
                transform.RotateAround(rotateAxis.position, CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
            //Debug.Log(transform.rotation.x);
            yield return new WaitForFixedUpdate();
        }
        while (transform.position.y >= 1.5f)
        {
            if (realDir.x < 0)
                transform.RotateAround(rotateAxis.position, CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
            else if (realDir.x > 0)
                transform.RotateAround(rotateAxis.position, -CameraController.instance.map.forward, moveSpeed * Time.fixedDeltaTime);
            else if (realDir.y < 0)
                transform.RotateAround(rotateAxis.position, -CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
            else if (realDir.y > 0)
                transform.RotateAround(rotateAxis.position, CameraController.instance.map.right, moveSpeed * Time.fixedDeltaTime);
            //Debug.Log(transform.rotation.x);
            yield return new WaitForFixedUpdate();
        }

        //GetComponent<Rigidbody>().freezeRotation = true;
        CubeController.instance.RoundCubesPos();
        CubeController.instance.cubeMoving = false;
    }*/

    IEnumerator PushCube()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionZ
            | RigidbodyConstraints.FreezeRotation;
        GetComponent<Rigidbody>().AddForce(400, 0, 0);
        while (transform.position.x < -1.5f) yield return null;
        while (transform.position.x < -1 && GetComponent<Rigidbody>().velocity.x > 0.05f) yield return null;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Rigidbody>().velocity *= 0;
        CubeController.instance.RoundCubesPos();
        CubeController.instance.pushCount--;
        CubeController.instance.pushCountText.text = CubeController.instance.pushCount.ToString() + " / 2";
    }
}