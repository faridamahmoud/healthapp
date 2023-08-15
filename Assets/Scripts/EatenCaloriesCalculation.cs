using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using System.Linq;
using Firebase.Extensions;
using Debug = UnityEngine.Debug;

public class EatenCaloriesCalculation : MonoBehaviour
{
    public TMP_Text text1;
    public TMP_Text Text2;
    public TMP_Dropdown dropdown1;
    public TMP_Dropdown dropdown2;
    public TMP_Dropdown dropdown3;

    // public int DailyCaloriesLimit = 200; 

    private DatabaseReference databaseRef;
    private Dictionary<string, FoodItems> _foodItems;

    void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _foodItems = new Dictionary<string, FoodItems>();
        GetFoodCaloriesData();
        // StartCoroutine(StartExerciseWithDelay());
    }

    // fetech food items and add them in dictionary
    private void GetFoodCaloriesData()
    {
        databaseRef.Child("FoodItems").GetValueAsync().ContinueWith(task =>
        {
            if (!task.IsCompletedSuccessfully)
                return;

            DataSnapshot snapshot = task.Result;
            if (snapshot != null && snapshot.Exists)
            {
                foreach (var child in snapshot.Children)
                {
                    var foodItem = JsonUtility.FromJson<FoodItems>(child.GetRawJsonValue());
                    _foodItems[foodItem.foodName] = foodItem;
                }
            }
        });
    }

    // display user eaten calories and accordingly may or may not display recommended exercise
    // with time calcculated to burn those extra calories

    public void Exercise()
    {
        
        //if (dropdown1.value > 0)
        //{
        //    string selectedFoodName = dropdown1.options[dropdown1.value].text;
        //    int caloriesFromDropdown1 = RetrieveCaloriesData(selectedFoodName);
        //    totalCalories += caloriesFromDropdown1;
        //    Debug.Log(caloriesFromDropdown1);
        //}

        var selectedFoodList = new List<string>();

        
        if (!string.IsNullOrEmpty(dropdown1.options[dropdown1.value].text))
        {
            string selectedFoodName = dropdown1.options[dropdown1.value].text;
            selectedFoodList.Add(selectedFoodName);
        }

        if (dropdown2.value > 0)
        {
            string selectedFoodName = dropdown2.options[dropdown2.value].text;
            selectedFoodList.Add(selectedFoodName);
        }

       
        if (dropdown3.options.Count > dropdown3.value) 
        {
            string selectedFoodName = dropdown3.options[dropdown3.value].text;
            selectedFoodList.Add(selectedFoodName);
        }

        var totalCalories = RetrieveCaloriesData(selectedFoodList);

        text1.text = "Your Eaten Calories: " + totalCalories;
        databaseRef.Child("UserInfo").Child("Daily Calories").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                
                int dailyCalories = int.Parse(snapshot.Value.ToString());
                RecommendExercise(dailyCalories, totalCalories);
               
            }
        });
    }


    // fetch exercise data and randomly select a type of exercise 
    private void RecommendExercise(int dailyCalories, int totalCalories)
    {
        if (totalCalories > dailyCalories)
        {
            // Get a random exercise from the database
            databaseRef.Child("Exercise").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Failed to read exercises data.");
                }
                else if (task.IsCompleted)
                {
                    try
                    {
                        DataSnapshot snapshot = task.Result;

                        var random = new System.Random();
                        int randomIndex = random.Next(0, (int)snapshot.ChildrenCount);

                        var randomExerciseSnapshot = snapshot.Children.ToList()[randomIndex];
                        string randomExercise = randomExerciseSnapshot.Child("exerciseName").Value.ToString();

                        // Calculate the exercise duration needed to burn off the extra calories
                        int extraCalories = totalCalories - dailyCalories;
                        int caloriesBurnedPerMinute = 12; // You can adjust this value based on your exercise data
                        int exerciseDuration = extraCalories / caloriesBurnedPerMinute;

                        Text2.text = "You need to do " + randomExercise + " for " + exerciseDuration + " minutes.";
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            });
        }
    }
    //calculate total calories based on user selection of food items
    private int RetrieveCaloriesData(List<string> foodsList)
    {
        int foodCalories = 0;

        foreach (var foodId in foodsList)
        {
            foodCalories += _foodItems[foodId].calories;
        }

        return foodCalories;
    }
}









