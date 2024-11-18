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
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
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

            // generate a new room
            List<int> availableDoors = new List<int> {1, 2, 3, 4};
            List<int> doorsToPlace = new List<int>();
            int AmountDoorsToPlace = Random.Range(1, 4);
            int obstacleCount = Random.Range(0, 6);
            int enemyCount = Random.Range(1, 5);

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
                enemy.xPOS = Random.Range(-7f, 7f);
                enemy.yPOS = Random.Range(-3f, 3f);
                enemy.isAlive = true;

                newRoom.enemies.Add(enemy);
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
                GameObject newDoor1 = Instantiate(door, new Vector3(0, 5, 0), new Quaternion(0, 0, 0, 0));
                newDoor1.GetComponent<DoorScript>().SetValues(newRoom.door1, "north");
            }
            if (newRoom.door2 != "inactive")
            {
                GameObject newDoor2 = Instantiate(door, new Vector3(9, 0, 0), new Quaternion(0, 0, 0.707f, 0.707f));
                newDoor2.GetComponent<DoorScript>().SetValues(newRoom.door2, "east");
            }
            if (newRoom.door3 != "inactive")
            {
                GameObject newDoor3 = Instantiate(door, new Vector3(0, -5, 0), new Quaternion(0, 0, 0, 0));
                newDoor3.GetComponent<DoorScript>().SetValues(newRoom.door3, "south");
            }
            if (newRoom.door4 != "inactive")
            {
                GameObject newDoor4 = Instantiate(door, new Vector3(-9, 0, 0), new Quaternion(0, 0, 0.707f, 0.707f));
                newDoor4.GetComponent<DoorScript>().SetValues(newRoom.door4, "west");
            }

            // place the obstacles
            for (int obs = 0; obs < newRoom.obstacles.Count; obs++)
            {
                Instantiate(newRoom.obstacles[obs].obstacleType, new Vector3(newRoom.obstacles[obs].xPOS, newRoom.obstacles[obs].yPOS, 0), transform.rotation);
            }

            // place the enemies
            for (int enm = 0; enm < newRoom.enemies.Count; enm++)
            {
                if (newRoom.enemies[enm].isAlive)
                {
                    GameObject newEnemy = Instantiate(newRoom.enemies[enm].enemyType, new Vector3(newRoom.enemies[enm].xPOS, newRoom.enemies[enm].yPOS, 0), transform.rotation);
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
                        GameObject newDoor1 = Instantiate(door, new Vector3(0, 5, 0), new Quaternion(0, 0, 0, 0));
                        newDoor1.GetComponent<DoorScript>().SetValues(nextRoom.door1, "north");
                    }
                    if (nextRoom.door2 != "inactive")
                    {
                        GameObject newDoor2 = Instantiate(door, new Vector3(9, 0, 0), new Quaternion(0, 0, 0.707f, 0.707f));
                        newDoor2.GetComponent<DoorScript>().SetValues(nextRoom.door2, "east");
                    }
                    if (nextRoom.door3 != "inactive")
                    {
                        GameObject newDoor3 = Instantiate(door, new Vector3(0, -5, 0), new Quaternion(0, 0, 0, 0));
                        newDoor3.GetComponent<DoorScript>().SetValues(nextRoom.door3, "south");
                    }
                    if (nextRoom.door4 != "inactive")
                    {
                        GameObject newDoor4 = Instantiate(door, new Vector3(-9, 0, 0), new Quaternion(0, 0, 0.707f, 0.707f));
                        newDoor4.GetComponent<DoorScript>().SetValues(nextRoom.door4, "west");
                    }

                    for (int obs = 0; obs < nextRoom.obstacles.Count; obs++)
                    {
                        Instantiate(nextRoom.obstacles[obs].obstacleType, new Vector3(nextRoom.obstacles[obs].xPOS, nextRoom.obstacles[obs].yPOS, 0), transform.rotation);
                    }
                    for (int enm = 0; enm < nextRoom.enemies.Count; enm++)
                    {
                        if (nextRoom.enemies[enm].isAlive)
                        {
                            GameObject newEnemy = Instantiate(nextRoom.enemies[enm].enemyType, new Vector3(nextRoom.enemies[enm].xPOS, nextRoom.enemies[enm].yPOS, 0), transform.rotation);
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

    
    private void ResestRoom()
    {
        //var objects = GameObject.FindGameObjectsWithTag("Default");

        var objectsDefault = GameObject.FindGameObjectsWithTag("Default");
        var objectsEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        var objectsBullets = GameObject.FindGameObjectsWithTag("Bullet");
        var objectsDoors = GameObject.FindGameObjectsWithTag("Door");
        //List<GameObject> objectsList = new List<GameObject>();

        DestroyObjectsList(objectsDefault);
        DestroyObjectsList(objectsEnemies);
        DestroyObjectsList(objectsBullets);
        DestroyObjectsList(objectsDoors);

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

            Debug.Log("Last enemy check result: True");
            var objectsDoors = GameObject.FindGameObjectsWithTag("Door");
            for (int i = 0; i < objectsDoors.Length; i++)
            {
                Debug.Log("Activating door: " + objectsDoors[i].name);
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
