using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbersBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh = default;

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    public void SetText(string to) 
    {
        textMesh.text = to;
    }
}