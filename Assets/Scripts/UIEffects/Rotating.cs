using UnityEngine;

/// <summary>
/// A script that just keeps rotating a ui element.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class Rotating : MonoBehaviour
{
    public float speed = 500;
    private RectTransform thisRect;

    void Start()
    {
        thisRect = GetComponent<RectTransform>();
    }
    
    void Update()
    {
        thisRect.Rotate(Vector3.back * speed * Time.deltaTime);
    }
}
