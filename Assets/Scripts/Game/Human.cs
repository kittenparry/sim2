using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    Text Stats;
    public string Name = "John";
    public string LastName = "Doe";
    public string Gender = "Male";
    public int Age = 23;

    public int NeedHunger = 66;
    public int NeedSleep = 90;

    void Start()
    {
        Stats = GameObject.Find("HumanStats").GetComponent<Text>();
        InvokeRepeating("DecreaseNeeds", 5.0f, 2.5f);
    }

    void FixedUpdate()
    {
        string formattedText = System.String.Format("Hunger: {0}\nSleep: {1}", NeedHunger, NeedSleep);
        Stats.text = formattedText;
    }

    void DecreaseNeeds()
    {
        NeedHunger -= 1;
        NeedSleep -= 1;
    }

}
