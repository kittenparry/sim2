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

    public string name = "John";
    public string lastName = "Doe";
    public string gender = "Male";
    public int age = 23;

    public int needHunger = 57;
    public int needSleep = 90;
    public string charStatus = "Wandering around...";
    public bool isEating = false;

    void Start()
    {
        textStatus = GameObject.Find("HumanStats").GetComponent<Text>();
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
        string formattedText = System.String.Format("Hunger: {0}\nSleep: {1}\n{2}", needHunger, needSleep, charStatus);
        textStatus.text = formattedText;
        if (needHunger > 50 && !isEating)
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
        if (!isEating) needHunger -= 3;
        needSleep -= 2;
    }

    void CheckNeeds()
    {
        // try to find food and stuff if too low
        // else random movement
        if (needHunger <= 50 && !isEating)
        {
            charStatus = "Looking for food...";
        } else if (isEating)
        {
            charStatus = "Eating some vegetables.";
            EatFood();
        }
        else
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
