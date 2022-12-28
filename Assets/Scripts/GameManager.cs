using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager Instance;

    //get prefabs
    [SerializeField]
    private GameObject _ballPrefab;
    [SerializeField]
    private GameObject _pillowPrefab;
    [SerializeField]
    private GameObject _basketPrefab;
    [SerializeField]
    private GameObject _obstacleKiller;
    [SerializeField]
    private GameObject _obstacleDefault;
    [SerializeField]
    private GameObject _obstacleWayChanger;
    
    private GameObject _ball;
    private GameObject _pillow;
    private GameObject _basket;
    private List<GameObject> _obstacles = new List<GameObject>();

    private int _activeLevel = 1;


    // a scene object consist of following fields, that can be ball, pillow, basket or obstacles
    protected class SceneObject{
        public GameObject objectPrefab {get; private set;}
        public Vector2 objectPosition {get; private set;}
        public Quaternion objectRotation {get; private set;}
        public Vector2 objectScale {get; private set;}

        public Vector2 wayChangerVelocity {get; private set;}

        public SceneObject(GameObject objectPrefab, Vector2 objectPosition, Quaternion objectRotation, Vector3 objectScale ){
            this.objectPrefab = objectPrefab;
            this.objectPosition = objectPosition;
            this.objectRotation = objectRotation;
            this.objectScale = objectScale;
        }

        public SceneObject(GameObject objectPrefab, Vector2 objectPosition, Quaternion objectRotation, Vector3 objectScale, Vector2 wayChangerVelocity){
            this.objectPrefab = objectPrefab;
            this.objectPosition = objectPosition;
            this.objectRotation = objectRotation;
            this.objectScale = objectScale;
            this.wayChangerVelocity = wayChangerVelocity;
        }
    }

    // a level is a bunch of sceneObjects
    protected class Level : IEnumerable {
        public SceneObject ball {get; private set;}
        public SceneObject pillow {get; private set;}
        public SceneObject basket {get; private set;}
        public SceneObject[] obstacles {get; private set;}

        public Level(SceneObject ball, SceneObject pillow, SceneObject basket, SceneObject[] obstacles){
            this.ball = ball;
            this.pillow = pillow;
            this.basket = basket;
            this.obstacles = obstacles;
        }


        public IEnumerator GetEnumerator(){
            return obstacles.GetEnumerator();
        }
    }

    private IDictionary<int, Level> levelDictionary = new Dictionary<int, Level>();

    //we are using the same scene for each level till level10
    //updating the position and rotations of ball, pillow and basket
    //destroying and creating new obstacles
    //thus, level issue is solved without creating many scenes.

    private void Awake() {
        if(Instance == null){
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != null){
            Destroy(this);
        }
    }

    private void Start() {
        this.CreateLevels();

        this.InitializeLevel(_activeLevel);
    }


    private void InitializeLevel(int levelIndex){
        //Since object instantiating is heavy, and destoying causes a lot of garbage, 
        //destroying and reinstantiating object in each level causes performance decreases
        Level level;
        this.levelDictionary.TryGetValue(levelIndex, out level);

        if(_ball == null){
            _ball = Instantiate(level.ball.objectPrefab.gameObject);
            SceneManager.MoveGameObjectToScene(_ball, SceneManager.GetActiveScene());

        }
        if(_pillow == null){
            _pillow = Instantiate(level.pillow.objectPrefab.gameObject);
            SceneManager.MoveGameObjectToScene(_pillow, SceneManager.GetActiveScene());

        }
        if(_basket == null){
            _basket = Instantiate(level.basket.objectPrefab.gameObject);
            SceneManager.MoveGameObjectToScene(_basket, SceneManager.GetActiveScene());
        }

        RepositionObject(_ball, level.ball.objectPosition, level.ball.objectRotation, level.ball.objectScale);
        RepositionObject(_pillow, level.pillow.objectPosition, level.pillow.objectRotation, level.pillow.objectScale);
        RepositionObject(_basket, level.basket.objectPosition, level.basket.objectRotation, level.basket.objectScale);


        // instantiate all the obstacles (including an obstacle is not a must for a level so can be null)

        if(level.obstacles==null) return;

        foreach(SceneObject obstacle in level){
            
            GameObject instantiatedObstacle = Instantiate(obstacle.objectPrefab, obstacle.objectPosition, obstacle.objectRotation);
            instantiatedObstacle.transform.localScale = obstacle.objectScale;

            //customize how fast does the way changer obstacle bounces
            if(obstacle.wayChangerVelocity!=Vector2.zero){
                instantiatedObstacle.GetComponent<WayChangerObstacle>().SetNewVelocity(obstacle.wayChangerVelocity);
            }
            

            _obstacles.Add(instantiatedObstacle);

            SceneManager.MoveGameObjectToScene(instantiatedObstacle, SceneManager.GetActiveScene());
        }

    }

    private void RepositionObject(GameObject gameObject, Vector2 position, Quaternion rotation, Vector3 scale){
        Rigidbody2D objectRigid = gameObject.GetComponent<Rigidbody2D>();

        if(objectRigid != null){
            objectRigid.velocity = Vector2.zero;
            objectRigid.angularVelocity = 0f;
        }

        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.transform.localScale = scale;
    }

    public void GameOver(){
        Debug.Log("game over is called");
        Level level1;
        levelDictionary.TryGetValue(_activeLevel, out level1);
        Rigidbody2D ballRigid = _ball.GetComponent<Rigidbody2D>();
        ballRigid.velocity = Vector2.zero;
        ballRigid.angularVelocity = 0f;
        _ball.transform.position = new Vector3(level1.ball.objectPosition.x, level1.ball.objectPosition.y, 0f);
    }

    public void LevelComplete(){
        this._activeLevel++;
        this.DestroyCurrentLevel();
        this.InitializeLevel(_activeLevel);
    }

    private void DestroyCurrentLevel(){
        //obstacles have to be destroyed and reinstantiated each level since they widely vary in each level..
        foreach(GameObject obstacleObject in _obstacles){
            Destroy(obstacleObject);
        }
        //check if list is empty
    }




    private void CreateLevels(){
        Level level1 = new Level(
            new SceneObject(this._ballPrefab, new Vector2(7.913f, -0.731f), Quaternion.Euler(0f,0f,0f), new Vector3(0.025f, 0.025f, 1f)),
            new SceneObject(this._pillowPrefab, new Vector2(7.93f, -1.33f), Quaternion.Euler(0f, 0f, 5.38f), new Vector3(0.3f, 0.3f, 1f)),
            new SceneObject(this._basketPrefab, new Vector2(-0.03f, -1.33f), Quaternion.Euler(0f, 0f, -60f), new Vector3(0.2f, 0.2f, 1f)),
            null
        );

        Level level2 = new Level(
            new SceneObject(this._ballPrefab, new Vector2(7.913f, -0.731f), Quaternion.Euler(0f,0f,0f), new Vector3(0.025f, 0.025f, 1f)),
            new SceneObject(this._pillowPrefab, new Vector2(7.93f, -1.33f), Quaternion.Euler(0f, 0f, 5.38f), new Vector3(0.3f, 0.3f, 1f)),
            new SceneObject(this._basketPrefab, new Vector2(-9.03f, -1.33f), Quaternion.Euler(0f, 0f, -60f), new Vector3(0.2f, 0.2f, 1f)),
            new SceneObject[]{
                new SceneObject(this._obstacleDefault, new Vector2(-0.9f, -1.27f), Quaternion.Euler(0f, 0f, 39.4f), new Vector3(0.7f, 0.7f, 1f)),
                new SceneObject(this._obstacleWayChanger, new Vector2(-0.72f, 4.97f), Quaternion.Euler(0f, 0f, 0f), new Vector3(0.7f, 0.7f, 1f), new Vector2(0f, -7f)),
                new SceneObject(this._obstacleKiller, new Vector2(1.52f, -0.2f), Quaternion.Euler(0f, 0f, 0f), new Vector3(0.25f, 0.25f, 1f)),
                new SceneObject(this._obstacleKiller, new Vector2(1.52f, 1.49f), Quaternion.Euler(0f, 0f, 0f), new Vector3(0.25f, 0.25f, 1f)),
                new SceneObject(this._obstacleKiller, new Vector2(-3.25f, 3.18f), Quaternion.Euler(0f, 0f, 0f), new Vector3(0.25f, 0.25f, 1f)),
                new SceneObject(this._obstacleKiller, new Vector2(-3.25f, 1.49f), Quaternion.Euler(0f, 0f, 0f), new Vector3(0.25f, 0.25f, 1f))
            }
        );

        Level level3 = new Level(
            new SceneObject(this._ballPrefab, new Vector2(5.033f, -2.129f), Quaternion.Euler(0f,0f,0f), new Vector3(0.025f, 0.025f, 1f)),
            new SceneObject(this._pillowPrefab, new Vector2(5.07f, -2.79f), Quaternion.Euler(0f, 0f, 5.38f), new Vector3(0.3f, 0.3f, 1f)),
            new SceneObject(this._basketPrefab, new Vector2(-9.03f, 1.26f), Quaternion.Euler(0f, 0f, -145f), new Vector3(0.2f, 0.2f, 1f)),
            new SceneObject[]{
                new SceneObject(this._obstacleWayChanger, new Vector2(0.3f, 3.46f), Quaternion.Euler(Vector3.zero), new Vector3(0.7f, 0.7f, 1f), new Vector2(0f, -10f)),
                new SceneObject(this._obstacleWayChanger, new Vector2(-3.12f, 3.46f), Quaternion.Euler(Vector3.zero), new Vector3(0.7f, 0.7f, 1f), new Vector2(0f, -10f)),
                new SceneObject(this._obstacleWayChanger, new Vector2(-6.54f, 3.46f), Quaternion.Euler(Vector3.zero), new Vector3(0.7f, 0.7f, 1f), new Vector2(0f, -10f)),
                new SceneObject(this._obstacleDefault, new Vector2(0.59f, -0.79f), Quaternion.Euler(0f, 0f, 25f), new Vector3(0.83f, 1f, 1f)),
                new SceneObject(this._obstacleDefault, new Vector2(-2.83f, -0.79f), Quaternion.Euler(0f, 0f, 25f), new Vector3(0.83f, 1f, 1f)),
                new SceneObject(this._obstacleDefault, new Vector2(-6.25f, -0.79f), Quaternion.Euler(0f, 0f, 25f), new Vector3(0.83f, 1f, 1f))
            }
        );

        this.levelDictionary.Add(1, level1);
        this.levelDictionary.Add(2, level2);
        this.levelDictionary.Add(3, level3);
    }

}
