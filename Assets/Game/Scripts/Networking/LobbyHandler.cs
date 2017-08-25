using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LobbyHandler : MonoBehaviour
{
    [Header("Settings")]
    public bool autoLogin = true;

    [Header("Panels")]
    public GameObject logRegisterPanel;
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public GameObject roomNamePanel;

    public Text errorMessage;

    [Header("Login")]
    public InputField loginUsername;
    public InputField loginPassword;
    public Button loginToRegisterBtn;
    public Button loginBtn;
    public Toggle rememberAcc;

    [Header("Register")]
    public InputField registerUsername;
    public InputField registerPassword;
    public InputField registerNickname;
    public Button registerToLoginBtn;
    public Button registerBtn;


    [Header("NetworkInfo")]
    private string titleID = "15A3";
    private string photonId = "bfe5901d-b5ca-4959-9487-689e0ab08b76";

    private void Awake()
    {
        // Are we already connected?
        if(PlayerPersistentData.alreadyConnected)
        {
            logRegisterPanel.SetActive(false);
            roomPanel.SetActive(true);
        }
    } 

    private void Start()
    {
        if(PlayerPrefs.HasKey("RememberAccount"))
        {
            rememberAcc.isOn = PlayerPrefs.GetInt("RememberAccount") == 1 ? true : false;
            loginUsername.text = PlayerPrefs.GetString("LoginUsername");
            loginPassword.text = PlayerPrefs.GetString("LoginPassword");

            if(autoLogin)
            {
                Login();
            }
        }
    }

    public void Login ()
    {
        // Clean 
        errorMessage.text = "";

        if (string.IsNullOrEmpty(loginUsername.text))
        {
            errorMessage.text = "Se requiere un nombre de usuario obligatorio.";
            return;
        }
        else if (string.IsNullOrEmpty(loginPassword.text) || loginPassword.text.Length < 6)
        {
            errorMessage.text = "Se requiere una contraseña de minimo 6 letras.";
            return;
        }

        // Disable login buttons
        loginToRegisterBtn.interactable = false;
        loginBtn.interactable = false;

        if(rememberAcc.isOn)
        {
            PlayerPrefs.SetInt("RememberAccount", 1);
            PlayerPrefs.SetString("LoginUsername", loginUsername.text);
            PlayerPrefs.SetString("LoginPassword", loginPassword.text);
        }

        // Login with playfab
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            TitleId = titleID,
            Username = loginUsername.text,
            Password = loginPassword.text
        }, LoginCallback, PlayfabErrorCallback, null);
	}

    private void LoginCallback (LoginResult result)
    {
        // Get needed data
        PlayerPersistentData.playerID = result.PlayFabId;

        // Get photon token
        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest
        {
            PhotonApplicationId = photonId
        }, GetTokenCallback, PlayfabErrorCallback, null);
    }

    private void GetTokenCallback (GetPhotonAuthenticationTokenResult result)
    {
        // Clean 
        errorMessage.text = "";

        string photonToken = result.PhotonCustomAuthenticationToken;
        Debug.Log(string.Format("Yay, logged in in session token: {0}", photonToken));
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.AddAuthParameter("username", PlayerPersistentData.playerID);
        PhotonNetwork.AuthValues.AddAuthParameter("Token", photonToken);
        PhotonNetwork.ConnectToBestCloudServer("1.0");

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { }, GetUserData, PlayfabErrorCallback, null);
    }

    private void GetUserData(GetAccountInfoResult result)
    {
        // Grab name
        PlayerPersistentData.playerName = result.AccountInfo.TitleInfo.DisplayName;
        PhotonNetwork.playerName = result.AccountInfo.TitleInfo.DisplayName;

        // Open lobby
        logRegisterPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void RegisterNewUser ()
    {
        // Clean 
        errorMessage.text = "";

        if (string.IsNullOrEmpty(registerUsername.text))
        {
            errorMessage.text = "Se requiere un nombre de usuario obligatorio.";
            return;
        }
        else if (string.IsNullOrEmpty(registerPassword.text) || registerPassword.text.Length < 6)
        {
            errorMessage.text = "Se requiere una contraseña de minimo 6 letras.";
            return;
        }
        else if (string.IsNullOrEmpty(registerNickname.text))
        {
            errorMessage.text = "Por favor escriba el nombre con el que lo veran los demas jugadores.";
            return;
        }

        // Disable register buttons
        registerToLoginBtn.interactable = false;
        registerBtn.interactable = false;

        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = registerUsername.text,
            Password = registerPassword.text,
            DisplayName = registerNickname.text,
            RequireBothUsernameAndEmail = false,
            TitleId = titleID
        }, RegisterCallback, PlayfabErrorCallback, null);
    }

    private void RegisterCallback (RegisterPlayFabUserResult result)
    {
        // Get needed data
        PlayerPersistentData.playerID = result.PlayFabId;

        // Get photon token
        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest
        {
            PhotonApplicationId = photonId
        }, GetTokenCallback, PlayfabErrorCallback, null);
    }

    private void PlayfabErrorCallback (PlayFabError error)
    {
        Debug.Log(error.Error);
        Debug.Log(error.ErrorMessage);

        if(error.Error == PlayFabErrorCode.AccountNotFound)
        {
            errorMessage.text = "El usuario o el password son incorrectos";
        }

        loginToRegisterBtn.interactable = true;
        loginBtn.interactable = true;

        registerToLoginBtn.interactable = true;
        registerBtn.interactable = true;
    }
}
