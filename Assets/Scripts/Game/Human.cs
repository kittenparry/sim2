using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    Text textStatus;
    public float speed = 100;
    private Vector2 movement;
    public Rigidbody2D rb;

    GameObject[] fruits = null;
    Transform[] fruitsPos = new Transform[4];
    GameObject[] beds = null;
    Transform[] bedsPos = new Transform[1];

    public string firstName = "John";
    public string lastName = "Doe";
    public string gender = "Male";
    public int age = 23;

    public int needHunger;
    public int needSleep;
    public string charStatus = "Wandering around...";
    public bool isEating = false;
    public bool isSleeping = false;

    void Start()
    {
        needHunger = Random.Range(54, 98);
        needSleep = Random.Range(44, 77);
        textStatus = GameObject.Find("HumanStats").GetComponent<Text>();
        InvokeRepeating("DecreaseNeeds", 2.5f, 2.5f);
        InvokeRepeating("CheckNeeds", 0f, 2.5f);

        // a very inefficient method of doing these
        fruits = GameObject.FindGameObjectsWithTag("Veggies");
        for (int i = 0; i < fruitsPos.Length; i++)
        {
            fruitsPos[i] = fruits[i].transform;
        }
        beds = GameObject.FindGameObjectsWithTag("RestSpots");
        for (int i = 0; i < beds.Length; i++)
        {
            bedsPos[i] = beds[i].transform;
        }
    }

    void FixedUpdate()
    {
        string formattedText = System.String.Format("Hunger: {0}\nSleep: {1}\n{2}", needHunger, needSleep, charStatus);
        textStatus.text = formattedText;
        if (needHunger > 50 && needSleep > 30 && !isSleeping && !isEating)
        {
            rb.AddForce(movement * speed);
        } else if (isEating || isSleeping) { }
        else if (needSleep <= 12 && !isSleeping)
        {
            charStatus = "Passed out!";
        } else if (needHunger <= 50 && !isEating)
        {
            Transform closest = GetClosestObject(fruitsPos);
            rb.transform.position = Vector2.MoveTowards(rb.transform.position, closest.position, 3.0f * Time.deltaTime);
            if (closest.position == rb.transform.position)
            {
                isEating = true;
            }
        } else if (needSleep <= 30 && !isSleeping) 
        {
            Transform closest = GetClosestObject(bedsPos);
            rb.transform.position = Vector2.MoveTowards(rb.transform.position, closest.position, 3.0f * Time.deltaTime);
            if (closest.position == rb.transform.position)
            {
                isSleeping = true;
            }
        }

    }

    // Human custom classes
    void DecreaseNeeds()
    {
        if (!isEating) needHunger -= 3;
        if (!isSleeping) needSleep -= 2;
    }

    void CheckNeeds()
    {
        // try to find food and stuff if too low
        // else random movement
        if (isSleeping)
        {
            isEating = false;
            charStatus = "Sleeping.";
            Sleep();
        } else if (needSleep <= 10 && !isSleeping)
        {
            isEating = false;
            charStatus = "Passed out!";
            isSleeping = true;
            Sleep();
        } else if (needHunger <= 50 && !isEating)
        {
            isSleeping = false;
            charStatus = "Looking for food...";
        } else if (isEating)
        {
            isSleeping = false;
            charStatus = "Eating some vegetables.";
            EatFood();
        } else if (needSleep <= 30 && !isSleeping)
        {
            charStatus = "Going to sleep...";
        } else
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            movement = movement.normalized * speed * Time.deltaTime;
            charStatus = "Wandering around...";
        }
    }

    void EatFood()
    {
        // veggies for now but should probably add other stuff
        if (needHunger < 100) needHunger = needHunger + 30 <= 100 ? needHunger + 30 : 100;
        else {
            isEating = false;
            charStatus = "Wandering around...";
        };
    }

    void Sleep()
    {
        if (needSleep < 100) needSleep = needSleep + 15 <= 100 ? needSleep + 15 : 100;
        else
        {
            isSleeping = false;
            charStatus = "Wandering around...";
        }
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
