using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public delegate void InputListener(Collider2D collider2d);
    public event InputListener OnClickOnScreen;

    public delegate void InputConversationListener();
    public event InputConversationListener OnClickOnScreenContConv;

    public bool CanTouch;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CanTouch = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ConversationManager.Instance.IsConversationRunning)
            {
                if (OnClickOnScreenContConv != null)
                {
                    OnClickOnScreenContConv();
                }
            }
            else
            {
                Vector3 mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ClickOnSreen(new Vector2(mouseToWorldPos.x, mouseToWorldPos.y));
            }
        }
    }

    private void ClickOnSreen(Vector2 mouseToWorldPos)
    {
        Collider2D collider2d = Physics2D.OverlapPoint(new Vector2(mouseToWorldPos.x, mouseToWorldPos.y));
        if (OnClickOnScreen != null && CanTouch == true)
        {
            OnClickOnScreen(collider2d);
        }
    }
}
