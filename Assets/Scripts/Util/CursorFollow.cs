using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    [SerializeField] private Vector2 offset;

    private void Update()
    {
        transform.position = Input.mousePosition + (Vector3)offset;
        Cursor.visible = !IsCursorInGameWindow();
    }

    private bool IsCursorInGameWindow()
    {
        Vector3 mousePosition = Input.mousePosition;
        return mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
               mousePosition.y >= 0 && mousePosition.y <= Screen.height;
    }
}
