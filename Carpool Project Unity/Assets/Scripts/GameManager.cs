/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private Player player;
    public PlayerData playerData;
    public NivelData nivelData;
    public InputField username;
    public InputField password;
    public Text loginText;
    public int character_id = 1;
    public float music_volume;
    public int ramapreferida;
    public float tiempoJuego;
    public string jsonData;
    public bool logged;
    public bool completeGame;
    public bool[] levels = new bool[4];
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }
    public void Update()
    {
        tiempoJuego += Time.deltaTime;
    }

    public void login()
    {
        StartCoroutine(getUserInfo());
    }
    IEnumerator getUserInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);
        UnityWebRequest www = UnityWebRequest.Post("http://52.171.199.75:8000/login_unity", form);
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            Debug.LogError("www.error: " + www.error);
        }
        else
        {
            if (www.downloadHandler.text == "{}")
            {
                loginText.text = "Nombre de usuario o constraseña incorrectos";
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                player = JsonUtility.FromJson<Player>(www.downloadHandler.text);
                playerData.username = username.text;
                playerData.id = player.id;
                playerData.firstName = player.first_name;
                playerData.sesionID = player.sesionID;
                playerData.ciencia = player.ciencia;
                playerData.tec = player.tec;
                playerData.ing = player.ing;
                playerData.mat = player.mat;
                levels[0] = playerData.ciencia;
                levels[1] = playerData.tec;
                levels[2] = playerData.ing;
                levels[3] = playerData.mat;
                playerData.profesor = player.profesor;
                logged = true;
                loginText.text = "Inicio de sesión exitoso \t Bienvenido de vuelta " + playerData.firstName;
            }
        }
        checkCompleteGame();
    }

    public void sendLevelData()
    {
        Nivel nivel = new Nivel(nivelData.rama, nivelData.completado, nivelData.tiempo, playerData.username);
        jsonData = JsonUtility.ToJson(nivel);
        StartCoroutine(saveLevelData());
    }
    IEnumerator saveLevelData()
    {
        if (!playerData.profesor)
        {
            UnityWebRequest www = new UnityWebRequest("http://52.171.199.75:8000/level_unity", "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.error != null)
            {
                Debug.Log("www.error: " + www.error);
            }
            else
            {
                Debug.Log("Received information");
            }
        }
    }

    public void startGame()
    {
        if (logged)
        {
            FindObjectOfType<PauseMenu>().goLevelSelect();
        }
        else
        {
            loginText.text = "Por favor inicia sesión primero";
        }
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(logout());
    }

    IEnumerator logout()
    {
        if (!playerData.profesor)
        {
            WWWForm form = new WWWForm();
            form.AddField("userID", playerData.id);
            form.AddField("characterID", character_id);
            form.AddField("sesionID", player.sesionID);
            UnityWebRequest www = UnityWebRequest.Post("http://52.171.199.75:8000/close_unity", form);
            yield return www.SendWebRequest();
            if (www.error != null)
            {
                Debug.LogError("www.error: " + www.error);
            }
        }
    }

    public void sendSTEM()
    {
        StartCoroutine(sendStemBranch());
    }

    IEnumerator sendStemBranch()
    {
        if (!playerData.profesor)
        {
            WWWForm form = new WWWForm();
            form.AddField("userID", playerData.id);
            form.AddField("ramaID", ramapreferida);
            UnityWebRequest www = UnityWebRequest.Post("http://52.171.199.75:8000/ramaSteam", form);
            yield return www.SendWebRequest();
            if (www.error != null)
            {
                FindObjectOfType<SurveryMenu>().confirmation.text = "Error al enviar la solicitud    ";
            }
            else
            {
                FindObjectOfType<SurveryMenu>().confirmation.text = "Solicitud enviada con éxito";
            }
        }
    }

    public void checkCompleteGame()
    {
        StartCoroutine(checkGameComplete());
    }

    IEnumerator checkGameComplete()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i])
            {
                completeGame = true;
            }
            else
            {
                completeGame = false;
                break;
            }
            yield return null;
        }
    }
}
*/