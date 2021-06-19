using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] DynamicJoystick joystick;
    [SerializeField] HUD hud;

    private void Start()
    {
        joystick.gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
    }

    public void OnStart()
    {
        joystick.gameObject.SetActive(true);
        hud.gameObject.SetActive(true);
        hud.Init();
        gameObject.SetActive(false);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
