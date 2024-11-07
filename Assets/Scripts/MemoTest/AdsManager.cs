using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Scripts.MemoTest
{
    public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
    {
        private string gameId = "TU_GAME_ID";       // ID de tu juego
        private string placementId = "TU_PLACEMENT_ID"; // ID de colocaci�n del anuncio
        private bool testMode = true; // Cambiar a false en producci�n
        private bool adLoaded = false; // Estado para saber si el anuncio est� cargado

        void Start()
        {
            Advertisement.Initialize(gameId, testMode, this);
            LoadAd();
        }

        public void LoadAd()
        {
            adLoaded = false; // Resetea el estado de carga
            Advertisement.Load(placementId, this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ShowAd();
            }
        }


        public void ShowAd()
        {
            if (adLoaded)
            {
                Advertisement.Show(placementId, this);
            }
            else
            {
                Debug.LogWarning("Ad not ready yet. Loading...");
                LoadAd();
            }
        }

        // Implementaci�n de IUnityAdsLoadListener
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("Ad Loaded: " + placementId);
            adLoaded = true; // Marca el anuncio como listo
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Failed to load Ad {placementId}: {error.ToString()} - {message}");
        }

        // Implementaci�n de IUnityAdsShowListener
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log("Ad Shown Successfully: " + placementId);
            adLoaded = false; // Marca el anuncio como no cargado y carga uno nuevo
            LoadAd();
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"Failed to show Ad {placementId}: {error.ToString()} - {message}");
            LoadAd();
        }

        public void OnUnityAdsShowStart(string placementId) { }
        public void OnUnityAdsShowClick(string placementId) { }
        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialized successfully");
            LoadAd(); // Carga el anuncio despu�s de la inicializaci�n
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Unity Ads Initialization failed: {error} - {message}");
        }
    }
}
