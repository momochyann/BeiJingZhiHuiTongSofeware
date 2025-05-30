using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Michsky.MUIP;
public class 通知控制 : MonoBehaviour
{
    [SerializeField] private NotificationManager myNotification; // Variable
    public void 播送通知(string 标题, string 内容)
    {
        myNotification.title = 标题; // Change title
        myNotification.description = 内容; // Change desc
        myNotification.enableTimer = true;
        myNotification.UpdateUI(); // Update UI

        myNotification.Open(); // Open notification
        //myNotification.Close(); // Close notification
        // myNotification.onOpen.AddListener(TestFunction); // Invoke open events
         myNotification.onClose.AddListener(TestFunction); // Invoke close events    
    }


    void TestFunction()
    {
        Destroy(gameObject);
    }
}
