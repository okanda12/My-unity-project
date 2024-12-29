using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ARMAadd", menuName = "Effects/ARMAadd")]

public class ARMAadd : MinionEffect
{

    public int addAmount;//’Ç‰Áƒ}ƒi—Ê
    GameObject ARMAdev;

    public override void ApplyEffect(CardController source, CardController target)
    {
        ARMAdev = GameObject.Find("ARMAdevice(Clone)");

        Debug.Log(ARMAdev);
        ARMAdevice ARMAdev_script = ARMAdev.GetComponent<ARMAdevice>();


        Debug.Log(ARMAdev_script);
        ARMAdev_script.AddCost(addAmount);

        //BattleManager.Instance.manasys.AddEmptyMana(RestoreAmount);


    }


}



