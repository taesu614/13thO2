using UnityEngine;

public class DragCamera : MonoBehaviour
{
    private Vector3 dragOrigin;
    private bool isDragging = false;

    public float dragSpeed = 2f;
    public BoxCollider2D cameraBounds;

    private void Start()
    {
        AudioManager.instance.PlaySFX(AudioManager.SFX.closeClick);  // 클릭시 임시 효과음
    }
    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

            transform.Translate(move, Space.World);

            // 제한된 영역 내에서만 이동
            ClampCameraPosition();

            dragOrigin = Input.mousePosition;
        }
    }

    void ClampCameraPosition()
    {
        if (cameraBounds == null)
            return;

        Vector3 clampedPosition = transform.position;

        float cameraHalfHeight = Camera.main.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        clampedPosition.x = Mathf.Clamp(transform.position.x, cameraBounds.bounds.min.x + cameraHalfWidth, cameraBounds.bounds.max.x - cameraHalfWidth);
        clampedPosition.y = Mathf.Clamp(transform.position.y, cameraBounds.bounds.min.y - cameraHalfHeight, cameraBounds.bounds.max.y + cameraHalfHeight);

        transform.position = clampedPosition;
    }
}