using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    [SerializeField] Text header;
    [SerializeField] Image bg;
    [SerializeField] Color winColor;
    [SerializeField] Color loseColor;
    [SerializeField] DynamicJoystick joystick;
    [SerializeField] HUD hud;

    private void Start()
    {
        joystick.gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        header.text = text;
    }

    public void SetColor(bool isWinner)
    {
        bg.color = isWinner ? winColor : loseColor;
    }

    public void OnReplay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
