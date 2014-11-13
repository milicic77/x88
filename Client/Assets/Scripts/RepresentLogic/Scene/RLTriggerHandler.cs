using UnityEngine;
using System.Collections;
using Game.GameLogic;
using Game.RepresentLogic;
using Game.GameEvent;

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
        EventDef.NpcHurtArgs args = new EventDef.NpcHurtArgs();
        args.npc = npc;
        args.missile = itself;
        EventCenter.Event_NpcHurt(null, args);
        itself.Destroy();
    }
}
