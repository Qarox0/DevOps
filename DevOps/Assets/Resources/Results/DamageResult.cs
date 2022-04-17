using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageResult : MonoBehaviour, IEventResult
{

    public void DoResult(string _params)
    {
        var dict = _params.HandleParams();
        if (dict.ContainsKey("_head"))
        {
            //Player.GetInstance().DealDamage(Player.BodyPart.HEAD,int.Parse(dict["_head"]));
        }
    }
}
