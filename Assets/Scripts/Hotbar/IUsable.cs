using UnityEngine;

public interface IUsable
{
    void Use(GameObject user);
    bool CanUse();
    string GetUseText();
}
