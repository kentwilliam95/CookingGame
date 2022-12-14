using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class GameController : MonoBehaviour, Core.EventListener<EventFoodServed>
    {
        public enum GameType
        {
            ServeBase = 0,
            TimeBase = 1
        }

        [System.Serializable]
        public struct Settings
        {
            public int serveAmount;
            public float timeDuration;
            public int totalCustomer;
            public float spawnCustomerEveryXSeconds;
        }

        private Camera mainCam;

        private GameType gameType;
        private float duration;
        private float spawnCustomerCountdown;
        private int totalSpawnCustomer;
        private int totalServed;
        private bool isFinish;
        private bool isInitialize;
        private List<Customer> customers;
        private FryingPan[] fryingPans;
        private Plate[] plates;

        public Settings gameSetting;
        public Database.FoodDatabase foodDatabase;
        public GameMenuController gameMenuController;

        [Header("Audio")]
        public AudioClip audioClipWin;
        public AudioClip audioClipLose;

        [Space]
        public Customer customerGO;
        public TrangbulanShop trangbulanStand;

        private void Awake()
        {
            Core.EventManager<EventFoodServed>.AddListener(this);
        }

        private void OnDestroy()
        {
            Core.EventManager<EventFoodServed>.RemoveListener(this);
        }

        public void Start()
        {
            if (Database.UserData.SelectedGroup == 1)
                Initialize(GameType.TimeBase);
            else
                Initialize(GameType.ServeBase);
        }

        public void Initialize(GameType gameType)
        {
            this.gameType = gameType;
            duration = gameSetting.timeDuration;
            spawnCustomerCountdown = gameSetting.spawnCustomerEveryXSeconds;
            isInitialize = true;

            mainCam = Core.CameraController.Instance.mainCamera;

            plates = GetComponentsInChildren<Plate>();
            fryingPans = GetComponentsInChildren<FryingPan>();
            customers = new List<Customer>();
            gameMenuController.Initialize(gameType);
            gameMenuController.UpdateTimer(0, true);
            Core.PanelFade.Instance.Hide();
        }

        private void Update()
        {
            if (isFinish || !isInitialize)
                return;
            
            HandleInputGame();
            SpawnCustomer();

            if (gameType == GameType.TimeBase)
            {
                duration -= Time.deltaTime;
                gameMenuController.UpdateTimer(Mathf.CeilToInt(duration), true);
                if (duration <= 0f)
                    CheckWinConditionTimeBase();
            }
        }

        private void HandleInputGame()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var doughJar = GetRayCastComponent<DoughJar>();
                if (doughJar)
                {
                    for (int i = 0; i < fryingPans.Length; i++)
                    {
                        if (!fryingPans[i].isBusy)
                        {
                            //Debug.Log("Add Dough");
                            var go = doughJar.GetDough();
                            IIngredient ingredient = go.GetComponent<IIngredient>();
                            ingredient.Initialize();
                            fryingPans[i].CookIngredient(ingredient);
                            break;
                        }
                    }
                }

                var fryingPan = GetRayCastComponent<FryingPan>();
                if (fryingPan)
                {
                    for (int i = 0; i < plates.Length; i++)
                    {
                        if (!plates[i].isBusy)
                        {
                            var ingredient = fryingPan.GetIngredient();
                            if (ingredient != default)
                            {
                                //Debug.Log($"Assign to Plate {i}");
                                plates[i].AssignIngredient(ingredient);
                            }
                        }
                    }
                }

                var plate = GetRayCastComponent<Plate>();
                if (plate)
                {
                    //Debug.Log("Plate", plate);
                    if (plate.isBusy)
                    {
                        for (int i = 0; i < trangbulanStand.customerPositions.Count; i++)
                        {
                            Customer c = trangbulanStand.customerPositions[i].customer;
                            if (c != null && c.IsReadyToServe)
                            {
                                plate.Serve(c, (cust) =>
                                {
                                    trangbulanStand.RemoveCustomer(cust);
                                });
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void SpawnCustomer()
        {
            if (gameType == GameType.ServeBase)
            {
                if (totalSpawnCustomer >= gameSetting.totalCustomer)
                    return;
            }

            spawnCustomerCountdown -= Time.deltaTime;
            if (spawnCustomerCountdown <= 0)
            {
                var data = trangbulanStand.GetCustomerPosition();
                if (data != null)
                {
                    spawnCustomerCountdown = gameSetting.spawnCustomerEveryXSeconds;
                    //var customer = Instantiate(customerGO);
                    var customer = Core.ObjectPool.Instance.Spawn("Customer").GetComponent<Customer>();

                    customer.Initialize(foodDatabase.food[0], data.position);
                    customer.OnCustomerLeave = Customer_OnLeave;
                    customer.OnCustomerFinishGivenFood = Customer_OnGivenFood;
                    data.customer = customer;
                    totalSpawnCustomer += 1;
                }
            }
        }

        private void CheckWinConditionTimeBase()
        {
            if (totalServed >= gameSetting.serveAmount)
            {
                //Debug.Log("Show win result");
                Database.UserData.CompleteLevel(Database.UserData.SelectedGroup);
                gameMenuController.ShowWin();
                Core.AudioManager.Instance.PlaySfx(audioClipWin);
            }
            else
            {
                //Debug.Log("Show lose result");
                gameMenuController.ShowLose();
                Core.AudioManager.Instance.PlaySfx(audioClipLose);
            }

            isFinish = true;
        }

        private void CheckWinConditionServeBase()
        {
            if (isFinish || gameType == GameType.TimeBase)
                return;

            if (totalServed >= gameSetting.serveAmount)
            {
                //Debug.Log("Show win result");
                Database.UserData.CompleteLevel(Database.UserData.SelectedGroup);
                gameMenuController.ShowWin();
                Core.AudioManager.Instance.PlaySfx(audioClipWin);
                isFinish = true;
            }
            else if (totalSpawnCustomer >= gameSetting.totalCustomer)
            {
                gameMenuController.ShowLose();
                Core.AudioManager.Instance.PlaySfx(audioClipLose);
                isFinish = true;
            }
        }

        public void Customer_OnLeave(Customer customer)
        {
            trangbulanStand.RemoveCustomer(customer);
            CheckWinConditionServeBase();
        }

        public void Customer_OnGivenFood(Customer customer)
        {
            gameMenuController.SpawnCoin(customer);
        }

        public T GetRayCastComponent<T>() where T : Component
        {
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                return hit.collider.GetComponent<T>();
            }
            return null;
        }

        public void OnInvoke(EventFoodServed evt)
        {
            totalServed += 1;
            Database.UserData.AddCoins(10);
            if (isFinish || gameType == GameType.TimeBase)
                return;

            gameMenuController.UpdateTimer(totalServed, true);
            CheckWinConditionServeBase();
        }
    }
}