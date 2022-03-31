using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3;
    [HideInInspector]public bool canMove = true;
    [HideInInspector]public SpriteRenderer sr;
    public Vector3 cameraOffset = new Vector3(0, 0, -10);
    private Camera mainCamera;

    private void Start()
    {
        canMove = true;
        sr = GetComponent<SpriteRenderer>();
        mainCamera = FindObjectOfType<Camera>();
        sr.color = UIManager.instance.CharacterBaseColors[0];
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = transform.position + cameraOffset;
    }

    /// <summary>
    /// Basic Movement Function
    /// </summary>
    public void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            sr.flipX = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            sr.flipX = false;
        }

    }
}
