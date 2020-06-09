using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    Text Stats;
    public float speed = 100;
    private Vector2 movement;
    public Rigidbody2D rb;
    GameObject[] fruits = null;
    Transform[] fruitsPos = new Transform[4];

    public string Name = "John";
    public string LastName = "Doe";
    public string Gender = "Male";
    public int Age = 23;

    public int NeedHunger = 66;
    public int NeedSleep = 90;
    public string Doing = "Wandering around...";

    void Start()
    {
        Stats = GameObject.Find("HumanStats").GetComponent<Text>();
        InvokeRepeating("DecreaseNeeds", 2.5f, 2.5f);
        InvokeRepeating("CheckNeeds", 0f, 2.5f);

        fruits = GameObject.FindGameObjectsWithTag("Veggies");
        for (int i = 0; i < fruitsPos.Length; i++)
        {
            fruitsPos[i] = fruits[i].transform;
        }
    }

    void FixedUpdate()
    {
        string formattedText = System.String.Format("Hunger: {0}\nSleep: {1}\n{2}", NeedHunger, NeedSleep, Doing);
        Stats.text = formattedText;
        rb.AddForce(movement * speed);

    }

    // Human custom classes
    void DecreaseNeeds()
    {
        NeedHunger -= 3;
        NeedSleep -= 2;
    }

    void CheckNeeds()
    {
        // try to find food and stuff if too low
        // else random movement
        if (NeedHunger <= 50)
        {
            Transform closest = GetClosestObject(fruitsPos);
            // transform.position = Vector3.MoveTowards(transform.position, closest.position, speed * Time.deltaTime);
            transform.position = Vector2.Lerp(transform.position, closest.position, speed * Time.deltaTime);
            Doing = "Looking for food.";
        } else
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            movement = movement.normalized * speed * Time.deltaTime;
            Doing = "Wandering around...";
        }
    }

    Transform GetClosestObject(Transform[] obj)
    {
        // TODO: these need to be vector2
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in obj)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
