using UnityEngine;
using System.Collections;

public class MenuSwitcher : MonoBehaviour {
    public MenuTextSelector menuController;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightController" || other.tag == "LeftController")
        {
            menuController.ChangeText();
        }
    }
}
