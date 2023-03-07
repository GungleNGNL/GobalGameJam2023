using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridController : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale = new Vector3(2f, 2f, 2f);
    [SerializeField] private float duration = 0.5f;
    private void OnEnable()
    {
        transform.DOScale(1, duration);
    }

    public IEnumerator DisableObject()
    {
        transform.DOScale(0, duration);
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
