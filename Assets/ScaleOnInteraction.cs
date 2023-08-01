using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScaleOnInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // The scale factor for the UI element when it scales up
    public float scaleMultiplier = 1.2f;

    // The time it takes for the scaling animation to complete
    public float animationTime = 0.2f;

    // The initial scale of the UI element
    private Vector3 initialScale;

    // The initial sorting order of the canvas
    private int initialSortingOrder;

    private void Start()
    {
        // Store the initial scale of the UI element
        initialScale = transform.localScale;

        // Get the initial sorting order of the canvas
        initialSortingOrder = GetComponentInParent<Canvas>().sortingOrder;
    }

    // Called when the mouse pointer enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Increase the sorting order to bring the UI element to the front
        GetComponentInParent<Canvas>().sortingOrder = int.MaxValue;

        // Smoothly scale up the UI element
        transform.DOScale(initialScale * scaleMultiplier, animationTime).SetEase(Ease.OutExpo);
    }

    // Called when the mouse pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore the UI element's initial scale
        transform.DOScale(initialScale, animationTime).SetEase(Ease.OutExpo);

        // Restore the initial sorting order of the canvas
        GetComponentInParent<Canvas>().sortingOrder = initialSortingOrder;
    }
}
