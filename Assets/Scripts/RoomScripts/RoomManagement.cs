using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManagement : MonoBehaviour
{
    public List<string> roomCodes;
    public List<Room> rooms;
    public Room currentRoom;
    public List<GameObject> roomTypes;
    public GameObject door;
    public GameObject defaultRoomTemplate;
    public List<GameObject> enemyTypes;
    public List<GameObject> obstacleTypes;
    private GameObject obstacleHolder, enemyHolder;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        obstacleHolder = GameObject.Find("obstacles");
        enemyHolder = GameObject.Find("enemies");

        Room defaultRoom = new()
        {
            door1 = "-1",
            door2 = "inactive",
            door3 = "inactive",
            door4 = "inactive",
            roomCode = "0",
            roomType = defaultRoomTemplate,
            obstacles = new List<Obstacle>(),
            enemies = new List<Enemy>()
        };
        currentRoom = defaultRoom;
        rooms.Add(defaultRoom);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

/*    public void CreateNewRoom()
    {
        Room newRoom = new Room();
        newRoom.roomCode = "1";
        // etc...
    }*/

    public void GoToRoom(string nextRoomCode, string doorPOS)
    {
        if (nextRoomCode == "-1")
        {
            // itterate through current room enemies and update their health and position
            UpdateEnemyValues();
            UpdateObstacleValues();

            // generate a new room
            List<int> availableDoors = new List<int> {1, 2, 3, 4};
            List<int> doorsToPlace = new List<int>();
            int AmountDoorsToPlace = Random.Range(1, 4);
            int obstacleCount = Random.Range(20, 50);
            int enemyCount = Random.Range(10, 21);

            Room newRoom = new()
            {
                roomCode = rooms.Count.ToString(),
                roomType = roomTypes[Random.Range(0, roomTypes.Count)],
                obstacles = new List<Obstacle>(),
                enemies = new List<Enemy>(),
                door1 = "inactive",
                door2 = "inactive",
                door3 = "inactive",
                door4 = "inactive"
            };

            //Debug.Log("NEW ROOM NAME" + newRoom.roomType.name);
            //Debug.Log("NEW ROOM TEMPLATE NUMBER" + newRoom.roomType.GetComponent<RoomTemplateData>().roomTemplateNumber);

            List<Vector2> availablePlacements = newRoom.roomType.GetComponent<RoomTemplateData>().GetAvailablePlacements(newRoom.roomType.GetComponent<RoomTemplateData>().roomTemplateNumber);
            //Debug.Log("Available placements count: " + availablePlacements.Count + " after get");
            //int[][] arrayAvailablePlacements = [[0,0], [1,1]];

            // Get an array of all available positions that are not blocked by default walls. (hard coded)
            // as enemies and obstacles are placed, remove the used locations from the array so that nothing overlaps.

            // assigning old roon code to corrisponding door
            switch (doorPOS) { 
            
                case "north":
                    newRoom.door3 = currentRoom.roomCode;
                    currentRoom.door1 = newRoom.roomCode;
                    doorsToPlace.Add(3);
                    availableDoors.Remove(3);
                    break;
                case "east":
                    newRoom.door4 = currentRoom.roomCode;
                    currentRoom.door2 = newRoom.roomCode;
                    doorsToPlace.Add(4);
                    availableDoors.Remove(4);
                    break;
                case "south":
                    newRoom.door1 = currentRoom.roomCode;
                    currentRoom.door3 = newRoom.roomCode;
                    doorsToPlace.Add(1);
                    availableDoors.Remove(1);
                    break;
                case "west":
                    newRoom.door2 = currentRoom.roomCode;
                    currentRoom.door4 = newRoom.roomCode;
                    doorsToPlace.Add(2);
                    availableDoors.Remove(2);
                    break;
                default:
                    break;
            }

            // itterates through the list of available doors and adds some to the list of doors to generate.
            for (int newDoors = 0; newDoors < AmountDoorsToPlace; newDoors++)
            {
                int newDoorNum = (Random.Range(0, availableDoors.Count));
                doorsToPlace.Add(availableDoors[newDoorNum]);
                switch (availableDoors[newDoorNum]) {
                    case 1:
                        newRoom.door1 = "-1";
                        break;
                    case 2:
                        newRoom.door2 = "-1";
                        break;
                    case 3:
                        newRoom.door3 = "-1";
                        break;
                    case 4:
                        newRoom.door4 = "-1";
                        break;
                    default:
                        Debug.LogError("NEW DOOR GENERATION HIT DEFAULT");
                        break;
                }
                availableDoors.RemoveAt(newDoorNum);

            }

            for (int newEnemy = 0; newEnemy < enemyCount; newEnemy++)
            {
                Enemy enemy = new();
                enemy.enemyCode = "Enemy" + newEnemy.ToString();
                enemy.enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];
                int availablePlacementIndex = Random.Range(0, availablePlacements.Count);
                enemy.xPOS = availablePlacements[availablePlacementIndex].x;
                enemy.yPOS = availablePlacements[availablePlacementIndex].y;
                enemy.isAlive = true;

                newRoom.enemies.Add(enemy);
                availablePlacements.RemoveAt(availablePlacementIndex);
            }

            for (int newObstacle = 0; newObstacle < obstacleCount; newObstacle++)
            {
                Obstacle obstacle = new();
                obstacle.obstacleID = "Obstacle" + newObstacle.ToString();
                obstacle.obstacleType = obstacleTypes[Random.Range(0, obstacleTypes.Count)];
                int availablePlacementIndex = Random.Range(0, availablePlacements.Count);
                obstacle.xPOS = availablePlacements[availablePlacementIndex].x;
                obstacle.yPOS = availablePlacements[availablePlacementIndex].y;
                obstacle.isActive = true;

                newRoom.obstacles.Add(obstacle);
                availablePlacements.RemoveAt(availablePlacementIndex);
            }
            // TODO RANDOMLY POPULATE ENEMY AND OBSTACLE LIST!



            // add newly created room to the rooms list
            rooms.Add(newRoom);

            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            ResestRoom();

            // load the new room into the scene
            Instantiate(newRoom.roomType);

            // place the doors for the room
            if (newRoom.door1 != "inactive")
            {
                GameObject newDoor1 = Instantiate(door, new Vector3(0, 5, -6), new Quaternion(0, 0, 0, 0));
                newDoor1.GetComponent<DoorScript>().SetValues(newRoom.door1, "north");
            }
            if (newRoom.door2 != "inactive")
            {
                GameObject newDoor2 = Instantiate(door, new Vector3(9, 0, -6), new Quaternion(0, 0, 0.707f, 0.707f));
                newDoor2.GetComponent<DoorScript>().SetValues(newRoom.door2, "east");
            }
            if (newRoom.door3 != "inactive")
            {
                GameObject newDoor3 = Instantiate(door, new Vector3(0, -5, -6), new Quaternion(0, 0, 0, 0));
                newDoor3.GetComponent<DoorScript>().SetValues(newRoom.door3, "south");
            }
            if (newRoom.door4 != "inactive")
            {
                GameObject newDoor4 = Instantiate(door, new Vector3(-9, 0, -6), new Quaternion(0, 0, 0.707f, 0.707f));
                newDoor4.GetComponent<DoorScript>().SetValues(newRoom.door4, "west");
            }

            // place the obstacles
            for (int obs = 0; obs < newRoom.obstacles.Count; obs++)
            {   
                if (newRoom.obstacles[obs].isActive)
                {
                    GameObject newObstacle = Instantiate(newRoom.obstacles[obs].obstacleType, new Vector3(newRoom.obstacles[obs].xPOS, newRoom.obstacles[obs].yPOS, 0), transform.rotation, obstacleHolder.transform);
                    newRoom.obstacles[obs].maxHealth = newObstacle.GetComponent<ObstacleScript>().maxHealth;
                    newObstacle.GetComponent<ObstacleScript>().SetCurrentHealth(newRoom.obstacles[obs].maxHealth);
                    newObstacle.name = newRoom.obstacles[obs].obstacleID;
                }
            }

            // place the enemies
            for (int enm = 0; enm < newRoom.enemies.Count; enm++)
            {
                if (newRoom.enemies[enm].isAlive)
                {
                    GameObject newEnemy = Instantiate(newRoom.enemies[enm].enemyType, new Vector3(newRoom.enemies[enm].xPOS, newRoom.enemies[enm].yPOS, 0), transform.rotation, enemyHolder.transform);
                    newRoom.enemies[enm].maxHealth = newEnemy.GetComponent<EnemyMovement>().maxHealth;
                    newEnemy.GetComponent<EnemyMovement>().SetCurrentHealth(newRoom.enemies[enm].maxHealth);
                    newEnemy.name = newRoom.enemies[enm].enemyCode;
                    // newEnemy.GetComponent<BaseEnemyScript>().SetHealth(nextRoom.enemies[enm].curHealth);
                }
            }

            // set new room as the current room (updates current room code)
            currentRoom = newRoom;


        }
        else
        {
            // itterate through current room enemies and update their health and position
            UpdateEnemyValues();
            UpdateObstacleValues();

            // restart the scene so it is empty
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            ResestRoom();

            // search the rooms list for the correct room code
            for (int i=0; i<rooms.Count; i++)
            {
                if (rooms[i].roomCode == nextRoomCode)
                {
                    // instantiate the new room
                    Room nextRoom = rooms[i];
                    Instantiate(nextRoom.roomType);

                    // place the doors for the room
                    if (nextRoom.door1 != "inactive")
                    {
                        GameObject newDoor1 = Instantiate(door, new Vector3(0, 5, -6), new Quaternion(0, 0, 0, 0));
                        newDoor1.GetComponent<DoorScript>().SetValues(nextRoom.door1, "north");
                    }
                    if (nextRoom.door2 != "inactive")
                    {
                        GameObject newDoor2 = Instantiate(door, new Vector3(9, 0, -6), new Quaternion(0, 0, 0.707f, 0.707f));
                        newDoor2.GetComponent<DoorScript>().SetValues(nextRoom.door2, "east");
                    }
                    if (nextRoom.door3 != "inactive")
                    {
                        GameObject newDoor3 = Instantiate(door, new Vector3(0, -5, -6), new Quaternion(0, 0, 0, 0));
                        newDoor3.GetComponent<DoorScript>().SetValues(nextRoom.door3, "south");
                    }
                    if (nextRoom.door4 != "inactive")
                    {
                        GameObject newDoor4 = Instantiate(door, new Vector3(-9, 0, -6), new Quaternion(0, 0, 0.707f, 0.707f));
                        newDoor4.GetComponent<DoorScript>().SetValues(nextRoom.door4, "west");
                    }

                    for (int obs = 0; obs < nextRoom.obstacles.Count; obs++)
                    {
                        if (nextRoom.obstacles[obs].isActive)
                        {
                            GameObject newObstacle = Instantiate(nextRoom.obstacles[obs].obstacleType, new Vector3(nextRoom.obstacles[obs].xPOS, nextRoom.obstacles[obs].yPOS, 0), transform.rotation, obstacleHolder.transform);
                            newObstacle.GetComponent<ObstacleScript>().SetCurrentHealth(nextRoom.obstacles[obs].curHealth);
                            newObstacle.name = nextRoom.obstacles[obs].obstacleID;
                        }
                    }
                    for (int enm = 0; enm < nextRoom.enemies.Count; enm++)
                    {
                        if (nextRoom.enemies[enm].isAlive)
                        {
                            GameObject newEnemy = Instantiate(nextRoom.enemies[enm].enemyType, new Vector3(nextRoom.enemies[enm].xPOS, nextRoom.enemies[enm].yPOS, 0), transform.rotation, enemyHolder.transform);
                            Debug.Log("setting enemy " + nextRoom.enemies[enm].enemyCode + "'s to: " + nextRoom.enemies[enm].curHealth);
                            newEnemy.GetComponent<EnemyMovement>().SetCurrentHealth(nextRoom.enemies[enm].curHealth);
                            newEnemy.name = nextRoom.enemies[enm].enemyCode;
                        }
                    }

                    // Enable doors incase all enemies are already killed
                    ActivateDoors();

                    // set new room as the current room (updates current room code)
                    currentRoom = nextRoom;
                }
            }
        }
    }

    private void UpdateEnemyValues()
    {
        for (int enm = 0; enm < currentRoom.enemies.Count; enm++)
        {
            if (currentRoom.enemies[enm].isAlive)
            {
                GameObject enemy = GameObject.Find(currentRoom.enemies[enm].enemyCode);
                if (enemy != null)
                {
                    currentRoom.enemies[enm].curHealth = enemy.GetComponent<EnemyMovement>().GetCurHealth();
                    Vector2 pos = enemy.GetComponent<EnemyMovement>().GetPOS();
                    currentRoom.enemies[enm].xPOS = pos.x;
                    currentRoom.enemies[enm].yPOS = pos.y;
                } else
                {
                    currentRoom.enemies[enm].isAlive = false;
                }

            }
        }
    }

    private void UpdateObstacleValues()
    {
        for (int obs = 0; obs < currentRoom.obstacles.Count; obs++)
        {
            if (currentRoom.obstacles[obs].isActive)
            {
                GameObject obstacle = GameObject.Find(currentRoom.obstacles[obs].obstacleID);
                if (obstacle != null)
                {
                    currentRoom.obstacles[obs].curHealth = obstacle.GetComponent<ObstacleScript>().GetCurrentHealth();
                } else
                {
                    currentRoom.obstacles[obs].isActive = false;
                }

            }
        }
    }


    private void ResestRoom()
    {
        //var objects = GameObject.FindGameObjectsWithTag("Default");

        var objectsDefault = GameObject.FindGameObjectsWithTag("Default");
        var objectsEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        var objectsBullets = GameObject.FindGameObjectsWithTag("Bullet");
        var objectsDoors = GameObject.FindGameObjectsWithTag("Door");
        var objectsPowerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        var objectsObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        //List<GameObject> objectsList = new List<GameObject>();

        DestroyObjectsList(objectsDefault);
        DestroyObjectsList(objectsEnemies);
        DestroyObjectsList(objectsBullets);
        DestroyObjectsList(objectsDoors);
        DestroyObjectsList(objectsPowerUps);
        DestroyObjectsList(objectsObstacles);

    }

    private void DestroyObjectsList(GameObject[] objects)
    {
        //Debug.Log("Found this many objects: " + objects.Length);
        for (int i = 0; i < objects.Length; i++)
        {
            //Debug.Log("Destroying: " + objects[i].name);
            Destroy(objects[i]);
        }
    }

    public void UpdateEnemyOnDeath(string enemyName)
    {
        for (int i=0; i < currentRoom.enemies.Count; i++)
        {
            if (currentRoom.enemies[i].enemyCode == enemyName)
            {
                currentRoom.enemies[i].isAlive = false;
            }
        }
        ActivateDoors();
    }

    private void ActivateDoors()
    {
        if (LastEnemyCheck())
        {

            //Debug.Log("Last enemy check result: True");
            var objectsDoors = GameObject.FindGameObjectsWithTag("Door");
            for (int i = 0; i < objectsDoors.Length; i++)
            {
                //Debug.Log("Activating door: " + objectsDoors[i].name);
                objectsDoors[i].GetComponent<DoorScript>().ActivateDoor();
            }
        }
    }

    private bool LastEnemyCheck()
    {
        bool allEnemiesDead = true;
        for (int i = 0; i < currentRoom.enemies.Count; i++)
        {
            if (currentRoom.enemies[i].isAlive == true)
            {
                return false;
            }
        }

        return allEnemiesDead;
    }
}
