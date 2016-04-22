using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class NotificationManager {

	private static NotificationManager instance;
	private static NotificationManager Instance {
		get { 
			if (instance == null)
				instance = new NotificationManager ();
			return instance;
		}
	}

	public static void Observe(Component observer, string callback, string eventName) {
        Instance.notifications.Add(new Notification(observer, callback, eventName));
	}

	public static void Remove(Component observer) {
        // Remove all notifications that are using this component
        Instance.notifications.RemoveAll(note => note.comp == observer);
    }

    public static void Clear() {
        Instance.notifications = new List<Notification>();
    }

    public static void PostNotification(string eventName, Dictionary<string, object> parameters) {
        foreach (Notification note in Instance.notifications) {
            if (note.eventName == eventName)
                note.Notify(parameters);
        }
    }

    public NotificationManager()
    {
        notifications = new List<Notification>();
    }

    private List<Notification> notifications;

    public class Notification {
        public Component comp;
        private string callbackName;
        public string eventName;

        public Notification(Component component, string callback, string eventName) {
            this.comp = component;
            this.callbackName = callback;
            this.eventName = eventName;
        }

        public void Notify(Dictionary<string, object> parameters) {
            comp.SendMessage(callbackName, parameters, SendMessageOptions.RequireReceiver);
        }
    }

}
