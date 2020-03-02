using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Dice[] dices;
    public List<int> diceResults; 
    // Start is called before the first frame update
    private void Awake()
    {
        dices = FindObjectsOfType<Dice>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
