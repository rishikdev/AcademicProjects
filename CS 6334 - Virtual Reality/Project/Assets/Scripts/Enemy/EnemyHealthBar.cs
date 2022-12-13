using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image foregroundImage;

    private Vector3 direction;
    private bool isBehind;

    void LateUpdate()
    {
        direction = (target.position - Camera.main.transform.position).normalized;
        isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;
        backgroundImage.enabled = !isBehind;
        foregroundImage.enabled = !isBehind;

        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    public void SetHealthBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
