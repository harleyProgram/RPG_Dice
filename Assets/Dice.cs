using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    public enum diceType { D4, D6, D8, D10, D12, D20 }
    public diceType DiceType;
    public Rigidbody rb;
    public float thrust;
    private GameController gameController;
    // Start is called before the first frame update

    public List<Vector3> directions;
    public List<int> sideValues;
    private int finalResult;
    private bool dieChecked;

    void Awake()
    {
        dieChecked = true;
        gameController = FindObjectOfType<GameController>();
        rb = gameObject.GetComponent<Rigidbody>();
        // For the sake of this example we assume a regular cube dice if 
        // directions haven't been specified in the editor. Sum of opposite
        // sides is 7, haven't consider exact real layout though.
        if (directions.Count == 0)
        {
            // Object space directions
            directions.Add(Vector3.up);
            sideValues.Add(1); // up
            directions.Add(Vector3.down);
            sideValues.Add(6); // down

            directions.Add(Vector3.left);
            sideValues.Add(2); // left
            directions.Add(Vector3.right);
            sideValues.Add(4); // right

            directions.Add(Vector3.forward);
            sideValues.Add(3); // fw
            directions.Add(Vector3.back);
            sideValues.Add(5); // back
        }

        // Assert equal side of lists
        if (directions.Count != sideValues.Count)
        {
            Debug.LogError("Not consistent list sizes");
        }
    }
    void Start()
    {
        // For sake of example, get number based on current orientation
        // This makes it possible to test by just rotating it in the editor and hitting play
        // Allowing 30 degrees error so will give (the side that is mostly upwards)
        // but will give -1 on "tie"
        Debug.Log("The side world up has value: " + GetNumber(Vector3.up, 30f));
    }

    // Gets the number of the side pointing in the same direction as the reference vector,
    // allowing epsilon degrees error.
    public int GetNumber(Vector3 referenceVectorUp, float epsilonDeg = 5f)
    {
        // here I would assert lookup is not empty, epsilon is positive and larger than smallest possible float etc
        // Transform reference up to object space
        Vector3 referenceObjectSpace = transform.InverseTransformDirection(referenceVectorUp);

        // Find smallest difference to object space direction
        float min = float.MaxValue;
        int mostSimilarDirectionIndex = -1;
        for (int i = 0; i < directions.Count; ++i)
        {
            float a = Vector3.Angle(referenceObjectSpace, directions[i]);
            if (a <= epsilonDeg && a < min)
            {
                min = a;
                mostSimilarDirectionIndex = i;
            }
        }

        // -1 as error code for not within bounds
        return (mostSimilarDirectionIndex >= 0) ? sideValues[mostSimilarDirectionIndex] : -1;
    }
private void Update()
    {
        DiceRoll();
    }

    private void SendScore(int score)
    {
        if (!dieChecked)
        {
            gameController.diceResults.Add(score);
            dieChecked = true;
        }
    }

    private void ClearScore()
    {
        dieChecked = false;
        gameController.diceResults.Clear();
    }
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude == 0)
        {
          
        }

        if (rb.IsSleeping())
        {
            Debug.Log(GetNumber(Vector3.up, 30f));
            finalResult = GetNumber(Vector3.up, 30f);
            SendScore(finalResult);
        }
    }
    private void DiceRoll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClearScore();
           thrust = Random.Range(-20, 20);
           rb.useGravity = true;
           rb.AddForce(thrust, thrust, thrust, ForceMode.Impulse);
           rb.AddTorque(thrust, thrust, thrust);
        }
    }
}
