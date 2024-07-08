using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathSceneManager : MonoBehaviour
{
    public GameObject DeathCanvasUI;
    public Button RespawnButton;

    void Start()
    {
        // Hide the death UI at the start
        DeathCanvasUI.SetActive(false);

        // Add listener to the retry button
        RespawnButton.onClick.AddListener(Respawn);
    }

    public void ShowDeathUI()
    {
        // Show the death UI and freeze the game
        DeathCanvasUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        // Reload the main scene and unfreeze the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
