using UnityEngine;
using System.Collections;
using Game.GameLogic;

public class RLTriggerHandler : MonoBehaviour
{
    public GLMissile itself;
    public void OnTriggerEnter2D()
    {
        itself.Destroy();
    }
}
