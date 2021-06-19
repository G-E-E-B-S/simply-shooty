using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] int coins = 0;
    [SerializeField] int frenzyPoints = 0;
    [SerializeField] int maxFrenzyPoints = 100;
    [SerializeField] float frenzyModifier = 3f;
    [SerializeField] float frenzyDuration = 8f;
    [SerializeField] Text coinText;
    [SerializeField] Image frenzyMeter;
    [SerializeField] Color meterFilling;
    [SerializeField] Color meterActive;

    private bool frenzyTimeActive;
    private PlayerAttack m_player;

    public void Init()
    {
        coins = 0;
        coinText.text = coins.ToString();
        frenzyPoints = 0;
        frenzyTimeActive = false;
        m_player = FindObjectOfType<PlayerAttack>();
    }

    public void AddCoins(int val)
    {
        coins += val;
        coinText.text = coins.ToString();
    }

    public void AddFrenzyPoints(int val)
    {
        if (frenzyTimeActive) return;

        frenzyPoints = Mathf.Min(maxFrenzyPoints, frenzyPoints + val);
        frenzyMeter.fillAmount = Mathf.Min(1, (float)frenzyPoints/maxFrenzyPoints);

        if (frenzyPoints == maxFrenzyPoints) StartFrenzyTime();
    }

    private void StartFrenzyTime()
    {
        if (!m_player) return;

        frenzyTimeActive = true;
        m_player.SetROFModifier(frenzyModifier);
        frenzyMeter.color = meterActive;
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(frenzyDuration);
        StopFrenzyTime();
    }

    private void StopFrenzyTime()
    {
        frenzyTimeActive = false;

        if (!m_player) return;

        m_player.SetROFModifier(1f);
        frenzyPoints = 0;
        frenzyMeter.fillAmount = 0;
        frenzyMeter.color = meterFilling;
    }

}
