using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Cookstation : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOn = false;
    [SerializeField] private List<Cookable> cookables = new List<Cookable>();
    [SerializeField] private float cookRate = 5f;


    private void Update()
    {
        if (isOn)
        {
            CookObjectsOnStation();
        }
    }

    private void CookObjectsOnStation()
    {
        float cookAmount = cookRate * Time.deltaTime;
        foreach (Cookable f in cookables)
        {
            f.Cook(cookAmount);
        }  
    }

    private void OnCollisionEnter(Collision collision)
    {
        Cookable c = collision.gameObject.GetComponent<Cookable>();
        if (c != null && !cookables.Contains(c))
        {
            cookables.Add(c);
            Debug.Log($"Added cookable {c.gameObject.name} to cookstation list.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Cookable c = collision.gameObject.GetComponent<Cookable>();
        if (c != null && cookables.Contains(c))
        {
            cookables.Remove(c);
            Debug.Log($"Removed cookable {c.gameObject.name} from cookstation list.");
        }
    }

    public void Interact(GameObject interactor)
    {
        isOn = !isOn;
    }

    public string GetInteractionPrompt()
    {
        return isOn ? "Turn off" : "Turn on";
    }
}
