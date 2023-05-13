using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC_1 : NPC
{
    private void Start()
    {
        npcName = "NPC 1";

        TMP_Text nm = GetComponentInChildren<TMP_Text>();
        nm.text = npcName;
    }


}
