using UnityEngine;

public class BlowTorch : MonoBehaviour, IUsable
{
    [SerializeField] private bool isOn = false;
    [SerializeField] private float cookRate = 30f;
    [SerializeField] private float range;

    private void Update()
    {
        if (isOn)
        {

        }
    }

    public void Use(GameObject user)
    {
        isOn = !isOn;
    }
    public bool CanUse()
    {
        return true;
    }
    public string GetUseText()
    {
        return isOn ? "Turn off" : "Turn on";
    }
}