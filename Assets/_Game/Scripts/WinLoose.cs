using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoose : MonoBehaviour
{
    private bool gameEnded;

    public void Win()
    {
        if (!gameEnded)
        {
            Debug.Log("You Win!");
            gameEnded = true;
        }
    }
    public void Loose()
    {
        if(!gameEnded)
        {
            Debug.Log("You Loose!");
            gameEnded = true;
        }
    }
}
