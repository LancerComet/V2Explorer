using System;
using System.Collections.Generic;

namespace V2EX.Service.EventBus {
  public class EventItem {
    public delegate void Callback (object value);
    public string name { get; set; }
    public Callback callback { get; set; }
    public EventItem (string name, Callback callback) {
      this.name = name;
      this.callback = callback;
    }
  }

  public class EventBus {
    private static List<EventItem> eventList = new List<EventItem> { };

    public static void emit (string eventName, object value) {
      var eventItem = eventList.Find(item => item.name == eventName);
      if (eventItem != null) {
        eventItem.callback.DynamicInvoke(value);
      }
    }

    public static void on (string eventName, EventItem.Callback eventFunc = null) {
      if (eventList.Find(item => item.name == eventName) == null) {
        eventList.Add(new EventItem(eventName, eventFunc));
      }
    }
  }
}
