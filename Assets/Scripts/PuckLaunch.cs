using UnityEngine;
using UnityEngine.EventSystems;

public class PuckLaunch : MonoBehaviour, IDragHandler,IEndDragHandler
{
    [SerializeField] private float _power;
    [SerializeField] private float _tossing;
    [SerializeField] private TrajectoryRenderer _trajectory;

    private GameObject _puck;
    private Camera _mainCamera;
    private Vector3 _direction;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_puck!=null)
        {
            FindTrajectoryForPuck(_puck);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_puck != null)
        {
            LaunchPuck();
        }
    }
    
    private void FindTrajectoryForPuck(GameObject puck)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        new Plane(Vector3.up, puck.transform.position).Raycast(ray, out float enter);
        Vector3 mouseInWorld = ray.GetPoint(enter);

        _direction = (mouseInWorld - puck.transform.position) * _power;
        _direction.y = (mouseInWorld.z - puck.transform.position.z) * _tossing;
        _trajectory.ShowTrajectory(puck.transform.position, _direction);
    }

    private void LaunchPuck()
    {
        _trajectory.HideTrajectory();
        _puck.GetComponent<Rigidbody>().AddForce(_direction, ForceMode.VelocityChange);
        _puck.GetComponent<Puck>().PuckLaunched();
        _puck = null;
    }

    public void SetNewPuck(GameObject puck)
    {
        _puck = puck;
    }
}
