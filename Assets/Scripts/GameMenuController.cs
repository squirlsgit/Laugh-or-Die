using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    public GameObject manual;
    public void OpenMenual()
    {
        manual.SetActive(true);
    }
    public void CloseMenual()
    {
        manual.SetActive(false);
    }
}
