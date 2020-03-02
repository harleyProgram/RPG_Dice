using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DiceV2 : MonoBehaviour
{
    public enum diceType { D4, D6, D8, D10, D12, D20 }
    public diceType DiceType;
    public Rigidbody rb;
    public float thrust;
    private GameController gameController;

    [SerializeField]
    private List<Transform> faceList;
    [SerializeField]
    private List<Transform> finalValues;
    // Start is called before the first frame update
    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        rb = gameObject.GetComponent<Rigidbody>();
       faceList = (this.gameObject.GetComponentsInChildren<Transform>().ToList());
        faceList.Remove(this.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        DiceRoll();
        FaceValueCheck();
    }

    private void DiceRoll()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            thrust = Random.Range(-20, 20);
            rb.useGravity = true;
            rb.AddForce(thrust, thrust, thrust, ForceMode.Impulse);
            rb.AddTorque(thrust, thrust, thrust);
        }
    }

    private void FaceValueCheck()
    {
        if(rb.IsSleeping() && rb.velocity.x >= 0){
           finalValues = faceList.OrderBy(faceList => faceList.transform.localPosition.y).ToList();
            Debug.Log("sorted");
        }
    }

}


