using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    Text Stats;
    public float speed = 100;
    private Vector2 movement;
    public Rigidbody2D rb;

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
        InvokeRepeating("DecreaseNeeds", 5.0f, 2.5f);
        InvokeRepeating("CheckNeeds", 0f, 2.5f);
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
        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        movement = movement.normalized * speed * Time.deltaTime;
        Doing = "Wandering around...";
    }

}
