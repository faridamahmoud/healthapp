//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;


//public class PrefabDropdown : MonoBehaviour, IPointerDownHandler
//{
//    //prefab
//    public GameObject Dropdownprefab;

//    //gameobject parent for dropdowns
//    public GameObject Parentdropdown;


    
//    public void OnPointerDown(PointerEventData eventData)
//    {
//        //create new dropdown
//        GameObject dropdown = Instantiate(Dropdownprefab);

//        //set new dropdown to be child of the gameobject parent
//        dropdown.transform.SetParent(Parentdropdown.transform);

//        //to make sure the new dropdown is in the same position
//        dropdown.transform.position = new Vector3(Parentdropdown.transform.position.x, Parentdropdown.transform.position.y - 110 ,Parentdropdown.transform.position.z);   

//        //make sure the new dropdown will be shown
//        dropdown.SetActive(true);  
//    }
  
    
//}
