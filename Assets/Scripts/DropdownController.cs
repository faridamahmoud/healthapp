using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;


public class DropdownController : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public DatabaseReference dataCalories;


    void Start()
    {
        //got refernce for the FoodItems node in the realtime database 
        DatabaseReference dataCalories = FirebaseDatabase.DefaultInstance.GetReference("FoodItems");

        dataCalories.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<FoodItems> fooditems = new List<FoodItems>();
                foreach (var childSnapshot in snapshot.Children)
                {
                    string json = childSnapshot.GetRawJsonValue();
                    FoodItems fooditem = JsonUtility.FromJson<FoodItems>(json);

                    fooditems.Add(fooditem);
                }

                InsertDataDropdown(fooditems);
            }
            else if (task.IsFaulted)
            {

                Debug.LogError("Failed to retrieve the data" + task.Exception);
            }

        });
      
    }

    public void InsertDataDropdown(List<FoodItems> fooditems)
    {
        dropdown.ClearOptions();

        List <string> foodNames = new List<string>();

        

        foreach( FoodItems fooditem in fooditems)
        {
           foodNames.Add(fooditem.foodName);

        }
        dropdown.AddOptions(foodNames);
        //dropdown.value = 0;

        //dropdown.RefreshShownValue();
    }

    
}








    
   

