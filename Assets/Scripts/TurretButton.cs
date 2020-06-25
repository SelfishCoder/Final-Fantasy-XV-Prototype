using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class TurretButton : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int turretIndex;
    [SerializeField] private Text goldText; 
    private Camera mainCamera;
    private Vector3 initialPosition;
    private RaycastHit hit;
    
    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        goldText.text = TurretManager.Instance.towers[turretIndex].GetComponent<Turret>().gold.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.transform.localScale = Vector3.one * 2;
        initialPosition = this.transform.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.transform.position = initialPosition;
        this.transform.localScale = Vector3.one;
        if (CastRay(Input.mousePosition))
        {
            BuildArea buildArea = hit.collider.gameObject.GetComponent<BuildArea>();
            if (buildArea)
            {
                TurretManager.Instance.BuildTower(buildArea,turretIndex);
            }
        }
    }

    private bool CastRay(Vector3 rayOrigin)
    {
        Ray ray = mainCamera.ScreenPointToRay(rayOrigin);
        return Physics.Raycast(ray,out hit, 250);
    }
}