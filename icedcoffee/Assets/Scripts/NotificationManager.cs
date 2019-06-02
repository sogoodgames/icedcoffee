using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NotificationManager : MonoBehaviour
{
    private class NotifInfo {
        public Sprite sprite;
        public string text;
        public float timeLeft;
        public App app;

        public NotifInfo(Sprite s, string t, App a, float maxTime) {
            sprite = s;
            text = t;
            app = a;
            timeLeft = maxTime;
        }
    }

    // UI references
    public GameObject NotificationUI;
    public Image Icon;
    public Text Text;
    public Button Button;

    // game references
    public PhoneOS PhoneOS;
    public ChatApp ChatApp;
    public NotesApp NotesApp;
    public ForumApp ForumApp;

    // tuning
    public float LingerSeconds = 2.0f;
    public string NewContactNotifText = "New contact: ";

    // internal
    private Queue<NotifInfo> m_notificationQueue;
    private AudioSource notifSound;

    void Start () {
        m_notificationQueue = new Queue<NotifInfo>();
        notifSound = GetComponent<AudioSource>();
    }

    void Update () {
        if(m_notificationQueue.Count > 0) {
            NotifInfo notif = m_notificationQueue.Peek();
            // pass time for notification
            notif.timeLeft -= Time.deltaTime;

            // check if notif's time has run out
            if(notif.timeLeft <= 0) {
                TryPlayNextNotif();
            }
        }
    }

    private void DisplayNotif (NotifInfo notif) {
        Icon.sprite = notif.sprite;
        Text.text = notif.text;
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(delegate{NotificationClicked(notif.app);});
        NotificationUI.SetActive(true);

        notifSound.Play();
    }

    private void QueueNotif (Sprite sprite, string text, App app) {
        // add this notif to the queue
        m_notificationQueue.Enqueue( new NotifInfo (
            sprite, text, app, LingerSeconds
        ));

        // if this is the only notif, play it immediately
        if(m_notificationQueue.Count == 1) {
            DisplayNotif(m_notificationQueue.Peek());
        }
    }

    public void FoundClueNotif (ClueID id) {
        Clue clue = PhoneOS.GetClue(id);
        QueueNotif(NotesApp.Icon, clue.Note, NotesApp);
    }

    public void NewContactNotif (Friend friend) {
        QueueNotif(ChatApp.Icon, NewContactNotifText + friend.ToString(), ChatApp);
    }

    public void ForumPostNotif (ForumPost post) {
        QueueNotif(ForumApp.Icon, "New Ruddit post!",  ForumApp);
    }

    public void NotificationClicked (App app) {
        TryPlayNextNotif();
        PhoneOS.OpenApp(app);
    }

    private void TryPlayNextNotif () {
        m_notificationQueue.Dequeue();
        // check if we have another queued
        if(m_notificationQueue.Count > 0) {
            NotifInfo nextNotif = m_notificationQueue.Peek();
            // if we do, play it
            DisplayNotif(nextNotif);
        } else {
            // if we don't, close notif bar
            Close();
        }
    }

    private void Close () {
        NotificationUI.SetActive(false);
        Button.onClick.RemoveAllListeners();
    }
}