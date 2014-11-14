using UnityEngine;
using System.Collections;
using Game.GameLogic;
using Game.RepresentLogic;

public class RLTriggerHandler : MonoBehaviour
{
    public GLMissile itself;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "npc")
        {
            return;
        }

        GLNpc npc = other.GetComponent<RLNpc>().m_GLNpc;
        itself.warhead.OnTriggerEnter(npc);
    }
}
