using UnityEngine;

public class Cookable : MonoBehaviour
{
    enum CookState
    {
        Raw,
        Cooked,
        Burnt
    }
    [SerializeField] private CookState cookState = CookState.Raw;
    [SerializeField] private float currentCookScore = 0f;
    [SerializeField] private float scoreToCook = 100f;
    [SerializeField] private float scoreToBurn = 200f;
    [SerializeField] private Material rawMaterial;
    [SerializeField] private Material cookedMaterial;
    [SerializeField] private Material burntMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateMaterial();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Station"))
        {
            currentCookScore += Time.deltaTime * 5f;
            currentCookScore = Mathf.Clamp(currentCookScore, 0f, scoreToBurn);
            if (currentCookScore >= scoreToBurn && cookState != CookState.Burnt)
            {
                cookState = CookState.Burnt;
                UpdateMaterial();
            }
            else if (currentCookScore >= scoreToCook && currentCookScore < scoreToBurn && cookState != CookState.Cooked)
            {
                cookState = CookState.Cooked;
                UpdateMaterial();
            }
        }
    }

    private void UpdateMaterial()
    {
        if (meshRenderer == null) return;
        
        switch (cookState)
        {
            case CookState.Raw:
                meshRenderer.material = rawMaterial;
                break;
            case CookState.Cooked:
                meshRenderer.material = cookedMaterial;
                break;
            case CookState.Burnt:
                meshRenderer.material = burntMaterial;
                break;
        }
    }
}
