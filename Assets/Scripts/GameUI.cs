using UnityEngine;

public class GameUI : MonoBehaviour
{
    private void Start()
    {
        transform.GetChild(GameManager.Instance.activeSpot).gameObject.SetActive(false);
    }

    public void MoveLeftPressed()
    {
        GameManager.Instance.SwitchSpot(0);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void MoveRightPressed()
    {
        GameManager.Instance.SwitchSpot(1);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
