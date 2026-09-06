using Planets;
using SpaceShip;
using UnityEngine;

public class TargetDirectionResolver : MonoBehaviour
{
    [SerializeField] private SpaceShipController shipController;
    [SerializeField] private Orbiter targetTransform;
    [SerializeField] private RectTransform horizontalTargetMarker;
    [SerializeField] private RectTransform horizontalCompassBarRect;
    
    [SerializeField] private RectTransform verticalTargetMarker;
    [SerializeField] private RectTransform verticalCompassBarRect;

    [SerializeField] private RectTransform onScreenMarker;
    
    [SerializeField] private float horizontalFOV = 30f; // Field of view for horizontal tape
    [SerializeField] private float verticalFOV = 30f;   // Field of view for vertical tape

    private Vector3 targetDir;
    private Vector3 targetScreenPoint;
    private Camera playerCam;

    public void SetTarget(Orbiter target) => targetTransform = target;

    public void SetCamera(Camera cam) => playerCam = cam;
    
    private void LateUpdate()
    {
        if (targetTransform == null || playerCam == null) return;

        // Calculate direction to target
        targetDir = (targetTransform.GetPosition() - shipController.transform.position).normalized;

        targetScreenPoint = playerCam.WorldToScreenPoint(targetTransform.GetPosition());
        var lerpedPos = LerpPosition(onScreenMarker.position, targetScreenPoint);
        onScreenMarker.position = lerpedPos;

        // Update horizontal marker
        UpdateMarker(horizontalTargetMarker, horizontalCompassBarRect, targetScreenPoint.x, targetScreenPoint, true);
        
        // Update vertical marker
        UpdateMarker(verticalTargetMarker, verticalCompassBarRect, targetScreenPoint.y, targetScreenPoint, false);
    }

    private Vector3 _currentVelocity;
    private Vector3 LerpPosition(Vector3 from, Vector3 to)
    {
        return Vector3.SmoothDamp(from, to, ref _currentVelocity, 0.2f);
    }

    private void UpdateMarker(RectTransform marker, RectTransform compassRect, float angle, Vector2 markerPos, bool isHorizontal)
    {
        if (marker == null || compassRect == null) return;

        // Check if target is within field of view
        if (Vector3.Dot(playerCam.transform.forward, targetTransform.GetPosition()) > 0.5)
        {
            marker.gameObject.SetActive(false);
            onScreenMarker.gameObject.SetActive(true);
            // onScreenMarker.position = markerPos;
        }
        else
        {
            marker.gameObject.SetActive(true);
            // onScreenMarker.gameObject.SetActive(false);
            
            // Calculate position on the tape
            if (isHorizontal)
            {
                float xPos = angle;// * compassRect.rect.width;
                marker.position = new Vector2(xPos, marker.position.y);
            }
            else
            {
                float yPos = angle;// * compassRect.rect.height;
                marker.position = new Vector2(marker.position.x, yPos);
            }
        }
    }
    
    public Vector3 GetTargetDirection()
    {
        return targetDir;
    }
}