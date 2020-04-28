using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverState : GameState
{
    private bool done;
    private bool loading;

    public GameOverState(MainGameManager mgm)
    {

        GameObject screen = Object.Instantiate(mgm.gameOverScreenPrefab, mgm.mainUICanvas.transform);

        GameOverScreen gameOver = screen.GetComponent<GameOverScreen>();
        gameOver.returnToTitle.onClick.AddListener(() => {
            if (done) return;
            done = true;
        });

        done = false;
        loading = false;

        mgm.cashLedgerToggle.gameObject.SetActive(false);
        mgm.financialStatementToggle.gameObject.SetActive(false);
        mgm.gameStateDisplay.gameObject.SetActive(false);
    }

    public override GameState Update(MainGameManager mgm)
    {
        if(done && !loading)
        {
            SceneManager.LoadSceneAsync("TitleScreen");
            loading = true;
        }
        return this;
    }
}
