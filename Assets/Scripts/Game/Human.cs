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
    public bool isEating = false;

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
        if (NeedHunger > 50 && !isEating)
        {
            rb.AddForce(movement * speed);
        } else
        {
            Transform closest = GetClosestObject(fruitsPos);
            rb.transform.position = Vector2.MoveTowards(rb.transform.position, closest.position, 3.0f * Time.deltaTime);
            if (closest.position == rb.transform.position)
            {
                isEating = true;
            }
        }

    }

    // Human custom classes
    void DecreaseNeeds()
    {
        if (!isEating) NeedHunger -= 3;
        NeedSleep -= 2;
    }

    void CheckNeeds()
    {
        // try to find food and stuff if too low
        // else random movement
        if (NeedHunger <= 50 && !isEating)
        {
            Doing = "Looking for food...";
        } else if (isEating)
        {
            Doing = "Eating some vegetables.";
            EatFood();
        }
        else
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            movement = movement.normalized * speed * Time.deltaTime;
            Doing = "Wandering around...";
        }
    }

    void EatFood()
    {
        // veggies for now but should probably add other stuff
        if (NeedHunger < 100) NeedHunger = NeedHunger + 30 <= 100 ? NeedHunger + 30 : 100;
        else isEating = false;
    }

    Transform GetClosestObject(Transform[] obj)
    {
        // TODO: these need to be vector2
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector2 currentPosition = transform.position;
        foreach (Transform potentialTarget in obj)
        {
            float directionToTarget = Vector2.Distance(potentialTarget.position, currentPosition);
            if (directionToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = directionToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
