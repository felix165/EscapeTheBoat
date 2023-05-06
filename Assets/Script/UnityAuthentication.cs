using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class UnityAuthentication : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);

        await SignInAnonymouslyAsync();
    }
    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            //Show how to get a playerID
            Debug.Log($"PlayerID:{AuthenticationService.Instance.PlayerId}");

            //Show how to get an access token
            Debug.Log($"Access Token:{AuthenticationService.Instance.AccessToken}");
        };
        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player sessoin could not be refreshed and expired.");
        };
    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            //Show how to get a playerID
            Debug.Log($"PlayerID:{AuthenticationService.Instance.PlayerId}");
        }catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
