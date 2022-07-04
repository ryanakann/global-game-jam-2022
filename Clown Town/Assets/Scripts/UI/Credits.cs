using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
    Animator anim;
    TextMeshProUGUI text;

    string[] freakingCredits = new string[]
        {
            "clownhead in command - Wyclown Clowner",
            "megaclown - Ryclown Clownann",
            "event clownmaster - Siclown Clownle",
            "junior clownman - Edclown Clownsi-Clownson",
            "baby clownster - Jaclown Clownivan",
            "the big clown on top - Clownby Dyclown",
            "clownlet - Gilclown Clownke",
            "background clown - Ticlown Clownyan",
        };

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
