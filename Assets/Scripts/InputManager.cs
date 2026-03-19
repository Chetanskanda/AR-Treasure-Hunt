using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera _arCamera;

    void Start()
    {
        _arCamera = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive()) return;

        // --- Mobile Touch ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                TryHitCoin(touch.position);
            }
        }

#if UNITY_EDITOR
        // --- Editor Mouse Click (for testing in Unity) ---
        if (Input.GetMouseButtonDown(0))
        {
            TryHitCoin(Input.mousePosition);
        }
#endif
    }

    void TryHitCoin(Vector2 screenPosition)
    {
        Ray ray = _arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 15f))
        {
            TreasureItem item = hit.collider.GetComponent<TreasureItem>();
            if (item != null)
            {
                item.Collect();
            }
        }
    }
}