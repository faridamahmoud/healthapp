using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using UnityEngine.UIElements;
using System;

public class FirebaseController : MonoBehaviour
{
    //public TMP_Dropdown foodDropdown;
    public TMP_InputField  weight;
    public TMP_InputField height;
    public TMP_InputField age;
    public TMP_Dropdown gender;


   


    DatabaseReference databaseReference;


    void Start()
    {

        //get the root refernce of the database

       databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        //did this one-time to save my data of food
        // SaveFoodItems();
       //did this one-time to save my data of exercise type
        //SaveExerciseData();
    }



    //function to upload the data of food in the database
    public void SaveFoodItems()
    {
       List <FoodItems>  fooditem = new List <FoodItems>();
        
        //Add many food items 
        fooditem.Add(new FoodItems { foodId = 1,foodName = "Apple", calories = 95});
        fooditem.Add(new FoodItems { foodId = 2, foodName = "Banana", calories = 111 });
        fooditem.Add(new FoodItems {foodId = 3, foodName= " Mango", calories = 202 });
        fooditem.Add(new FoodItems { foodId = 4, foodName = " Orange", calories = 62 });
        fooditem.Add(new FoodItems { foodId = 5, foodName = " Cheddar cheese", calories = 89 });
        fooditem.Add(new FoodItems { foodId = 6, foodName = " Feta cheese", calories = 74 });
        fooditem.Add(new FoodItems { foodId = 7, foodName = " Milk", calories = 149 });
        fooditem.Add(new FoodItems { foodId = 8, foodName = " Beef steak", calories = 355 });
        fooditem.Add(new FoodItems { foodId = 9, foodName = "Chicken Breast", calories = 344 });
        fooditem.Add(new FoodItems { foodId = 10, foodName = " Tuna", calories = 203 });
        fooditem.Add(new FoodItems { foodId = 11, foodName = " Salmon", calories = 367 });
        fooditem.Add(new FoodItems { foodId = 12, foodName = " White Bread", calories = 67 });
        fooditem.Add(new FoodItems { foodId = 13, foodName = " Spagetti", calories = 370 });
        fooditem.Add(new FoodItems { foodId = 14, foodName = " Hamburger", calories = 279 });
        fooditem.Add(new FoodItems { foodId = 15, foodName = "Fried Rice", calories = 662 });
        fooditem.Add(new FoodItems { foodId = 16, foodName = " Fettucine", calories = 353 });
        fooditem.Add(new FoodItems {foodId = 17, foodName = " Margherita Pizza", calories = 173 });
        fooditem.Add(new FoodItems {foodId = 18, foodName = " BBQ Chicken Pizza", calories = 309 });
        fooditem.Add(new FoodItems {foodId = 19, foodName = " Coke", calories = 149 });
        fooditem.Add(new FoodItems {foodId = 20, foodName = " 7up", calories = 156 });
        fooditem.Add(new FoodItems { foodId = 21, foodName = "Iced Tea", calories = 96 });
        fooditem.Add(new FoodItems { foodId = 22, foodName = " Chicken Cream Soup", calories = 117 });
        fooditem.Add(new FoodItems { foodId = 23, foodName = "Tomato Soup", calories = 74 });
        fooditem.Add(new FoodItems { foodId = 24, foodName = "French Fries", calories = 222 });
        fooditem.Add(new FoodItems { foodId = 25, foodName = "Fried Shrimp", calories = 75 });
        fooditem.Add(new FoodItems { foodId = 26, foodName = " Mac&Cheese", calories = 699 });
        fooditem.Add(new FoodItems { foodId = 27, foodName = " Kebab", calories = 774 });
        fooditem.Add(new FoodItems { foodId = 28, foodName = " Mashed Potatoes", calories = 174 });


        foreach(FoodItems food in fooditem)
        {
            string json = JsonUtility.ToJson(food);
            databaseReference.Child("FoodItems").Push().SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log(" Food Data is uploaded successfully");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("Failed to upload food data" + task.Exception);
                }
            });

        }
    }

    //a function to save data of different exercise types in the database

    public void SaveExerciseData()
    {
        List<Exercise> exercises = new List<Exercise>();

        exercises.Add(new Exercise { exerciseId=1,exerciseName="Running" });
        exercises.Add(new Exercise { exerciseId = 2, exerciseName = "Cycling" });
        exercises.Add(new Exercise { exerciseId = 3, exerciseName = "Squat" });
        exercises.Add(new Exercise { exerciseId = 4, exerciseName = "Lunge" });
        exercises.Add(new Exercise { exerciseId = 5, exerciseName = "Burpee" });
        exercises.Add(new Exercise { exerciseId = 6, exerciseName = "Pushup" });
        exercises.Add(new Exercise { exerciseId = 7, exerciseName = "Plank" });
        exercises.Add(new Exercise { exerciseId = 8, exerciseName = "Plank Jack" });
        exercises.Add(new Exercise { exerciseId = 9, exerciseName = "Jumping Jack" });
        exercises.Add(new Exercise { exerciseId = 10, exerciseName = "Walking" });
        exercises.Add(new Exercise { exerciseId = 11, exerciseName = "Curtsy Lunge" });



        foreach (Exercise exercise in exercises)
        {   string json = JsonUtility.ToJson(exercise);

            databaseReference.Child("Exercise").Push().SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log(" Exercise Data is uploaded successfully");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("Failed to upload exercise data" + task.Exception);
                }
            });

        }
    }

    //this function calculates the daily calories and save it into the realtime database
    public void CalculateAndSaveDailyCalories()
    {
        float Weight = float.Parse(weight.text);
        float Height = float.Parse(height.text);
        int Age = int.Parse(age.text);
        bool isMale = gender.value == 1;

        float DailyCalories;
        if (isMale)
        {
            DailyCalories = 88.362f + (13.39f * Weight) + (4.799f * Height) - (5.677f * Age);
        }
        else
        {
            DailyCalories = 447.593f + (9.24f * Weight) + (3.098f * Height) - (4.33f * Age);
        }

       DailyCalories = Convert.ToInt32(DailyCalories);
       
      databaseReference.Child("UserInfo").Child("Daily Calories").SetValueAsync(DailyCalories).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Daily Calories is uploaded successfully");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to upload " + task.Exception);
            }
        });
    }
}
    
  
    
  




   




