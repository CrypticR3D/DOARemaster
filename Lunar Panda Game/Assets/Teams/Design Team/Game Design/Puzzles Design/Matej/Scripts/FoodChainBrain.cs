using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodChainBrain : MonoBehaviour
{
    [Header("Code & Animation")]
    public RandomNumber tiger;
    public RandomNumber wolf;
    public RandomNumber fox;
    public RandomNumber rab;

    int firstNumber;
    int secondNumber;
    int thirdNumber;
    int fourthNumber;

    public string theCode;



    public void Start()
    {
        firstNumber = tiger.GetNumber();
        secondNumber = wolf.GetNumber();
        thirdNumber = fox.GetNumber();
        fourthNumber = rab.GetNumber();
        theCode = firstNumber + "" + secondNumber + "" + thirdNumber + "" + fourthNumber;
    }
    public void TurnDiscs(char size)
    {

    }

}
