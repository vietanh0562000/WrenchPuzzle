using DxCoder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRotate : MonoBehaviour
{
    private bool isRotating = false;
    private bool hasCollided = false;
    private float rotationSpeed = 600.0f; // Adjust the speed as needed
    private float rotationAmount = 0.0f;
    private Quaternion originalRotation;
    private Coroutine rotationCoroutine;
    bool reverse;
    public float desiredAngle = 0f;  // Set your desired rotation angle here
    public float Speed = 10f;
    Rigidbody rb;
    public float m_Thrust = 20f;
    public TrailRenderer tr;
    public Rigidbody screwRb;
    public bool isClickable;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        reverse = false;
        // Store the original rotation of the object
        originalRotation = transform.rotation;
        isClickable = true;
    }

    private void Update()
    {
        if (hasCollided)
        {
            // If a collision occurred, reset the rotation and allow rotation again
            isRotating = false;
          //  reverse = true;
            StartCoroutine(RotateToAngle(desiredAngle, Speed));
            // ReverseRotation();
        }

        if (Input.GetMouseButtonDown(0) && !hasCollided && isClickable)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // Start rotating the object
                isRotating = true;
            }
        }
       
        if (isRotating)
        {
            // Rotate the object smoothly on the Z-axis
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            rotationAmount += rotationSpeed * Time.deltaTime;

            // Check if the rotation has completed (360 degrees)
            if (rotationAmount >= 360.0f)
            {
                DoResolve();
            }
        }
    }

    public void DoResolve()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Instance.coin);

        rb.isKinematic = false;
        tr.enabled = false;
        rb.AddForce(Vector3.back*m_Thrust, ForceMode.Impulse);
        screwRb.isKinematic=false;
        screwRb.AddForce(Vector3.back * m_Thrust*2F, ForceMode.Impulse);
        screwRb.AddTorque(Vector3.forward * 5,ForceMode.Impulse);
        rotationAmount = 0.0f;
        isRotating = false;
        isClickable = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Wrench"))
        {
            if(SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Instance.error);

            // If a collision occurs, set the "hasCollided" flag
            hasCollided = true;
        }
        else if (collision.gameObject.CompareTag("bolt"))
        {


            // If a collision occurs, set the "hasCollided" flag
            hasCollided = true;
        }
    }

    IEnumerator RotateToAngle(float targetAngle, float speed)
    {
       // isRotating = true;

        while (Mathf.Abs(transform.eulerAngles.z - targetAngle) > 2f)
        {
            float step = Mathf.Sign(targetAngle - transform.eulerAngles.z) * speed * Time.deltaTime;
            transform.Rotate(0f, 0f, step);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0f, 0f, targetAngle);
        //  isRotating = false;
        hasCollided = false;
        rotationAmount = 0.0f;
    }


}
