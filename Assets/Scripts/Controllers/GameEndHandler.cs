using UnityEngine;

public class GameEndHandler : MonoBehaviour
{
    [SerializeField] GameEnd gameEndPopup;
    
    private bool gameResult;

    private void Start()
    {
        gameResult = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            player.PlayVictorySequence(DeclareVictory);
        }
    }

    public void DeclareVictory()
    {
        if (gameResult) return;

        gameResult = true;
        gameEndPopup.gameObject.SetActive(true);
        gameEndPopup.SetText("YOU WIN!");
        gameEndPopup.SetColor(true);
    }

    public void DeclareLoss()
    {
        if (gameResult) return;

        gameResult = true;
        gameEndPopup.gameObject.SetActive(true);
        gameEndPopup.SetText("YOU LOSE!");
        gameEndPopup.SetColor(false);
    }
}
