using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : EventListener
{
    List<int> activeSceneIndices;

    [System.Serializable]
    public class SceneGroup
    {
        public List<int> indices;
    }

    [SerializeField] private List<SceneGroup> sceneGroups;


    [SerializeField] private GameObject loadScreenCamera;
    [SerializeField] private GameObject loadScreenCanvas;
    private int numLoadedScenes = -1;
    private int numScenesMaintained = 0;
    private EventManager eventManager;
    private Coroutine isLoading = null;

    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 90;
        //Cursor.visible = false;
        activeSceneIndices = new List<int>();
        LoadNewScenes(sceneGroups[0].indices);

        eventManager = EventManager.GetInstance();

        eventManager.AddListener(this, EventType.BALL_COLLISION);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += CheckSceneLoading;
        SceneManager.sceneUnloaded += CheckSceneUnloading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= CheckSceneLoading;
        SceneManager.sceneUnloaded -= CheckSceneUnloading;

    }

    void LoadNewScenes(List<int> indices)
    {   
        //if we aren't already loading something
        if (isLoading == null)
        {
            //activate loading screen
            loadScreenCanvas.SetActive(true);
            loadScreenCamera.SetActive(true);

            //then start the loading coroutine
            isLoading = StartCoroutine(LoadScenes(indices));
        }
    }

    IEnumerator LoadScenes(List<int> indices)
    {   
        //unload all active scenes
        for (int i = 0; i < activeSceneIndices.Count; ++i)
        {
            SceneManager.UnloadSceneAsync(activeSceneIndices[i]);
        }


        //wait for all old scenes to be unloaded
        do
        {
            yield return new WaitForEndOfFrame();
        } while (numLoadedScenes != numScenesMaintained);  


        //clear old scene indices
        activeSceneIndices.Clear();

        //load the new set of scenes and store their indices
        for (int i = 0; i < indices.Count; ++i)
        {
            activeSceneIndices.Add(indices[i]);
            SceneManager.LoadSceneAsync(indices[i], LoadSceneMode.Additive);
        }


        //wait for all new scenes to be loaded
        do
        {
            yield return new WaitForEndOfFrame();
        } while (numLoadedScenes != numScenesMaintained + indices.Count);


        //deactivate the loading screen
        loadScreenCanvas.SetActive(false);
        loadScreenCamera.SetActive(false);
        isLoading = null;
    }

    void CheckSceneLoading(Scene scene, LoadSceneMode mode)
    {
        ++numLoadedScenes;
    }

    void CheckSceneUnloading(Scene scene)
    {
        --numLoadedScenes;
    }


    public override void HandleEvent(Event incomingEvent)
    {
        base.HandleEvent(incomingEvent);

        switch (incomingEvent.GetEventType())
        {
            case EventType.BALL_COLLISION:
                LoadNewScenes(sceneGroups[1].indices);
                break;
        }
    }

    //return to scene 1
    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
}
