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
    }

    public void StartMoveCube(Vector2 joyStickDir, bool up)
    {
        //Debug.Log(joyStickDir);
        if (!up)
            StartCoroutine(MoveCube(joyStickDir));
        else
            StartCoroutine(MoveCubeUp(joyStickDir));
    }
    IEnumerator MoveCube(Vector2 realDir)
    {
        yield return StartCoroutine(CameraController.instance.ResetMapRotation());
        //CubeController.instance.RoundCubesPos();
        Transform rotateAxis = null;
        for (int i = 0; i < axis.Length; i++)
        {
            if (axis[i].transform.position.y < transform.position.y - 0.25f)
                if (Mathf.Abs(axis[i].transform.position.x - (transform.position.x + realDir.x * 0.5f)) < 0.5f)
                    if (Mathf.Abs(axis[i].transform.position.z - (transform.position.z + realDir.y * 0.5f)) < 0.5f)
                    {
                        rotateAxis = axis[i];
                        break;
                    }
        }
        //GetComponent<Rigidbody>().freezeRotation = false;
        while (transform.position.y >= 0.5f)
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
    }
    IEnumerator MoveCubeUp(Vector2 realDir)
    {
        yield return StartCoroutine(CameraController.instance.ResetMapRotation());
        //CubeController.instance.RoundCubesPos();
        Transform rotateAxis = null;
        for (int i = 0; i < axis.Length; i++)
        {
            if (axis[i].transform.position.y > transform.position.y + 0.25f)
                if (Mathf.Abs(axis[i].transform.position.x - (transform.position.x + realDir.x * 0.5f)) < 0.5f)
                    if (Mathf.Abs(axis[i].transform.position.z - (transform.position.z + realDir.y * 0.5f)) < 0.5f)
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
    }
}
