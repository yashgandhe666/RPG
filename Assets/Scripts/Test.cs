using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<int> list = new List<int>();
    public int item = 1;
    public bool isAdd = false;
    public int index = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (isAdd)
            {
                list.Add(item);
            }
            else
            {
                if (index < list.Count)
                {
                    list.RemoveAt(index);
                }
                else
                {
                    Debug.Log("index is out of range...");
                }
            }
        }
    }

    public void Call(int id)
    {
        Debug.Log("Button press and player is response at id " + id);
    }
}
