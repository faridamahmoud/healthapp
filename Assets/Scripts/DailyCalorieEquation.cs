using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DailyCalorieEquation : MonoBehaviour
{
    
    public TMP_InputField Weightfield;
    public TMP_InputField Heightfield;
    public TMP_InputField Agefield;
    public TMP_Dropdown Gender;
    public TMP_Text Result;
    

    public void CalorieCalculator()
    {
        float Weight = float.Parse(Weightfield.text);
        float Height = float.Parse(Heightfield.text);
        int Age = int.Parse(Agefield.text);
        bool isMale = Gender.value == 1;

        float DailyCalories;
        if (isMale)
        {
            DailyCalories = 88.362f + (13.39f * Weight) + (4.799f * Height) - (5.677f * Age) ;
        }
        else
        {
            DailyCalories = 447.593f + (9.24f * Weight) + (3.098f * Height) - (4.33f * Age);
        }

        Result.text = "Your Daily Calories:" + DailyCalories.ToString("F0");
    }
    
    
    



}
