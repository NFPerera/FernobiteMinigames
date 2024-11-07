using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Scripts.MemoTest
{
    public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
    {
        private string gameId = "TU_GAME_ID";       // ID de tu juego
        private string placementId = "TU_PLACEMENT_ID"; // ID de colocación del anuncio
        private bool testMode = true; // Cambiar a false en producción
        private bool adLoaded = false; // Estado para saber si el anuncio está cargado

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

        // Implementación de IUnityAdsLoadListener
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("Ad Loaded: " + placementId);
            adLoaded = true; // Marca el anuncio como listo
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Failed to load Ad {placementId}: {error.ToString()} - {message}");
        }

        // Implementación de IUnityAdsShowListener
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
            LoadAd(); // Carga el anuncio después de la inicialización
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Unity Ads Initialization failed: {error} - {message}");
        }
    }
}
