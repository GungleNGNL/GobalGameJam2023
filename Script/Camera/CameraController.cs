using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoSingleton<CameraController>
{
    private CameraControls m_CameraActions;
    private Camera m_Cam;
    public CameraControls CameraActions => m_CameraActions;
    [SerializeField] LayerMask m_MapLayer;
    [SerializeField] Vector2Int bounds;
    [SerializeField] float m_Speed;
    [SerializeField] float m_MaxY, m_MinY;
    [SerializeField] float m_StepSize;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float m_RotationSpeed;
    [Range(0.0f, 1.0f)]
    [SerializeField] float edgeTolerance;
    Transform m_CameraTransform;
    private bool isRotateCamera;
    protected override void Awake()
    {
        m_CameraActions = new();
        m_CameraActions.Camera.MoveCamera.performed += ctx => MoveCamera(ctx.ReadValue<Vector2>());
        m_CameraActions.Camera.MoveCamera.canceled += _ => moveDirection = Vector3.zero;
        m_CameraActions.Camera.MouseMove.performed += _ => CheckMouseAtScreenEdge();
        m_CameraActions.Camera.ZoomCamera.performed += ctx => ZoomCamera(ctx.ReadValue<Vector2>());
        m_CameraActions.Camera.MouseMove.performed += ctx => CameraRotation(ctx.ReadValue<Vector2>());
        m_CameraActions.Camera.RotateCamera.performed += _ => isRotateCamera = true;
        m_CameraActions.Camera.RotateCamera.canceled += _ => isRotateCamera = false;
        m_Cam = GetComponentInChildren<Camera>();
        m_CameraTransform = m_Cam.transform;
    }
    Vector3 moveDirection = Vector3.zero;
    private void MoveCamera(Vector2 dir)
    {
        moveDirection = Vector3.zero;
        if (dir.x != 0)
        {
            if (dir.x > 0)
            {
                moveDirection += GetCameraRight();
            }
            else
            {               
                moveDirection -= GetCameraRight();
            }
        }
        if (dir.y != 0)
        {
            if (dir.y > 0)
            {              
                moveDirection += GetCameraForward();
            }
            else
            {
                moveDirection -= GetCameraForward();
            }
        }
        //targetPosition += moveDirection.normalized;
    }

    [SerializeField] Vector3 m_MousePos;
    [SerializeField] Vector3 m_Target;
    private void CameraRotation(Vector2 input)
    {
        if (!isRotateCamera) return;
        //transform.rotation = Quaternion.Euler(0f, input.x * m_RotationSpeed + transform.rotation.eulerAngles.y, 0f);
        //transform.Rotate(Vector3.up, input.x * m_RotationSpeed);
        m_MousePos = Mouse.current.position.ReadValue();
        m_MousePos.z = 0.3f;
        RaycastHit hit;
        if (Physics.Raycast(m_CameraTransform.position, m_CameraTransform.forward, out hit, 600.0f, m_MapLayer))//Physics.Raycast(ray, out hit, 600.0f, m_MapLayer))
        {
            m_Target = hit.point;
        }

        transform.RotateAround(m_Target, Vector3.up, input.x * m_RotationSpeed);
    }

    private void ZoomCamera(Vector2 value)
    {
        float inputValue = -value.y / 100f;
        float zoomHeight = 0;
        float result;
        if (Mathf.Abs(inputValue) > 0.1f)
        {
            zoomHeight = -inputValue * m_StepSize;
            result = m_CameraTransform.localPosition.z + zoomHeight;
            if (result < m_MaxY || result > m_MinY)
                zoomHeight = 0;
        }
        Vector3 des = Vector3.zero;
        des.z = zoomHeight;
        m_CameraTransform.localPosition += des;
    }
    Vector3 des = Vector3.zero;
    private void FixedUpdate()
    {
        targetPosition = moveDirectionM.normalized + moveDirection.normalized;
        if (targetPosition.magnitude > 0)
            transform.position += Vector3.Lerp(transform.position, new Vector3(targetPosition.x, 0, targetPosition.z) * m_Speed * Time.fixedDeltaTime, m_Speed * Time.fixedDeltaTime);
        des = transform.position;
        if (transform.position.x > bounds.x)
        {
            des.x = bounds.x;
        }
        else if (transform.position.x < -bounds.x)
        {
            des.x = -bounds.x;
        }
        if (transform.position.z > bounds.y)
        {
            des.z = bounds.y;
        }
        else if (transform.position.z < -bounds.y)
        {
            des.z = -bounds.y;
        }
        transform.position = des;           
    }
    Vector3 moveDirectionM = Vector3.zero;
    private void CheckMouseAtScreenEdge()
    {
        //mouse position is in pixels
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        moveDirectionM = Vector3.zero;
        //horizontal scrolling
        if (mousePosition.x < edgeTolerance * Screen.width)
            moveDirectionM += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirectionM += GetCameraRight();

        //vertical scrolling
        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirectionM += -GetCameraForward();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirectionM += GetCameraForward();
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = m_CameraTransform.forward;
        forward.y = 0f;
        return forward;
    }

    //gets the horizontal right vector of the camera
    private Vector3 GetCameraRight()
    {
        Vector3 right = m_CameraTransform.right;
        right.y = 0f;
        return right;
    }

    private void OnEnable()
    {
        m_CameraActions.Enable();
    }

    private void OnDisable()
    {
        m_CameraActions.Disable();
    }
}
