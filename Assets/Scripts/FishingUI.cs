using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class FishingUI : MonoBehaviour
{
    [System.Serializable]
    public class Result
    {
        public string result;
        public int id;
        public int bassCaught;
        public int muskieCaught;
        public int blueGillCaught;
        public int bassTotal;
        public int muskieTotal;
        public int blueGillTotal;
        public int coin;  
    }

    public static FishingUI instance;

    // Fishing Main UI
    public GameObject main;
    public GameObject signinMenu;
    public GameObject signupMenu;  
    public GameObject inventoryMenu; 
    public GameObject startMenu; 
    public InputField nameInput;
    public InputField passwordInput;
    public InputField name1Input;
    public InputField password1Input;
    public InputField password2Input;  
    public GameObject alert;
    public Text alertText;
    public Text bluegillCaughtText;
    public Text bluegillSellText;
    public Text bluegillTotalText;
    public Text bassCaughtText;
    public Text bassSellText;
    public Text bassTotalText;
    public Text muskieCaughtText;
    public Text muskieSellText;
    public Text muskieTotalText;
    public Text priceText;
    float showingTime = 0;
    bool isShowing = false;

    // Api URL
    public string absURL = "";
    string signupURL;
    string signinURL;
    string readURL;
    string sellURL;

    void Awake()
    {
        instance = this;
    }

    void Start()             // controller 
    {
        main.transform.localScale = new Vector3(Screen.width / 1366f, Screen.height / 768f, 1f);
        signupURL = absURL + "api/signup";
        signinURL = absURL + "api/login";
        readURL = absURL + "api/getUserData";
        sellURL = absURL + "api/sell";
    }

    void Update()
    {
        main.transform.localScale = new Vector3(Screen.width / 1366f, Screen.height / 768f, 1f);

        // Show alert(Warning).        
        if(isShowing)
        {
            alert.SetActive(true);
            showingTime -= Time.deltaTime;
        }
        else
        {
            alert.SetActive(false);
        }

        if(showingTime < 0)
        {
            isShowing = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // SignIn Menu
    public void SigninBtnClick()   
    {
        if(nameInput.text != "" && passwordInput.text != "")  // if input text user name and password is not left empty
        {            
            // Request "sign in" Api.
            StopAllCoroutines();   // pause  frame
            StartCoroutine(PostRequestSign(signinURL, nameInput.text, passwordInput.text)); // call IElimanator to verify account info
        }
        else
        {
            isShowing = true;  //show screen to enter info
            showingTime = 2f;   // display message 2 frames
            alertText.text = "Please fill all fields."; // message text
        }        
    }

    public void Signup0BtnClick()    // sign up button switch screen
    {
        signinMenu.SetActive(false);
        signupMenu.SetActive(true);     // switch screens
        nameInput.text = "";
        passwordInput.text = "";   //intilize to empty
    }    

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //SignUp Menu
    public void SignupBtnClick()  // sign up after user enters info
    {
        if(name1Input.text != "" && password1Input.text != "" && password2Input.text != "")   // if input text user name and password is not left empty
        {
            if(password1Input.text == password2Input.text)   // if password match
            {
                // Request "sign up" Api
                StopAllCoroutines();   // pause frame
                StartCoroutine(PostRequestSign(signupURL, name1Input.text, password1Input.text));        // call IEliminator to create and sign in account                        
            }
            else
            {                              // display incorrect message & initilize pasword 2 to blank
                isShowing = true;
                showingTime = 2f;
                alertText.text = "Please type password again.";
                password2Input.text = "";
            }  
        }
        else
        {              // displays error message for all fields
            isShowing = true;
            showingTime = 2f;
            alertText.text = "Please fill all fields.";
        }                     
    }

    public void SignupBackBtnClick()  // back button
    {
        signinMenu.SetActive(true);
        signupMenu.SetActive(false);   // switches screens
        name1Input.text = "";
        password1Input.text = "";
        password2Input.text = "";
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Inventory Menu    
    public void InventoryBackBtnClick()    // back button for inventory
    {
        inventoryMenu.SetActive(false);
        startMenu.SetActive(true);  // brings user back to start menue
    }

    public void SellBtnClick()
    {
        // Requrest "sell" Api
        StopAllCoroutines();  // pause frame
        StartCoroutine(RequestSell(sellURL));    // call IElimtinator to sell            
    }

    public void PlusBlueGillBtnClick()
    {
        if(int.Parse(bluegillCaughtText.text) > int.Parse(bluegillSellText.text))
        {
            bluegillSellText.text = (int.Parse(bluegillSellText.text) + 1).ToString();  //  add 1 on display sell count
        }
    }

    public void MinusBlueGillBtnClick()
    {
        if(int.Parse(bluegillSellText.text) > 0)  // checks if not 0
        {
            bluegillSellText.text = (int.Parse(bluegillSellText.text) - 1).ToString();    //  minus 1 on display sell count
        }
    }

    public void PlusBassBtnClick()
    {
        if(int.Parse(bassCaughtText.text) > int.Parse(bassSellText.text))
        {
            bassSellText.text = (int.Parse(bassSellText.text) + 1).ToString();
        }
    }

    public void MinusBassBtnClick()
    {
        if(int.Parse(bassSellText.text) > 0)
        {
            bassSellText.text = (int.Parse(bassSellText.text) - 1).ToString();
        }
    }

    public void PlusMuskieBtnClick()
    {
        if(int.Parse(muskieCaughtText.text) > int.Parse(muskieSellText.text))
        {
            muskieSellText.text = (int.Parse(muskieSellText.text) + 1).ToString();
        }
    }

    public void MinusMuskieBtnClick()
    {
        if(int.Parse(muskieSellText.text) > 0)
        {
            muskieSellText.text = (int.Parse(muskieSellText.text) - 1).ToString();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Start Menu
    public void StartBtnClick()
    {
        // Start the game.
        SceneManager.LoadScene("Game");
    }

    public void SignoutBtnClick()    
    {
        nameInput.text = "";
        passwordInput.text = "";             
        startMenu.SetActive(false);
        signinMenu.SetActive(true);
    }

    public void InventoryBtnClick()     // shows inventory
    {
        bassSellText.text = "0";
        bluegillSellText.text = "0";
        muskieSellText.text = "0";
        startMenu.SetActive(false);
        inventoryMenu.SetActive(true);

        // Request "read data" Api
        StopAllCoroutines();
        StartCoroutine(RequestRead(readURL));   // calls IEliminator to read user game info for inventory
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Apis
    IEnumerator PostRequestSign(string url, string name, string password)
    {               
        // Send user name and password.
        WWWForm form = new WWWForm();   // allows unity to send info/field
        form.AddField("name", name);
        form.AddField("password", password);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);  // send request
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)  // if not connected 
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            // Recieve data from backend.
            Result loadData = JsonUtility.FromJson<Result>(uwr.downloadHandler.text);
            string result = loadData.result;
            int userId = loadData.id;

            if(url == signinURL)   // checks account with database & determine output
            {
                if(string.Equals(result, "1")) // Sign in successfully.
                {
                    PlayerPrefs.SetInt("USER_ID", userId);
                    signinMenu.SetActive(false);
                    signupMenu.SetActive(false);
                    startMenu.SetActive(true);                                        
                }
                else if(string.Equals(result, "2")) // User doesn't exist
                {
                    isShowing = true;
                    showingTime = 2f;
                    alertText.text = "Not registered";
                }
                else // Enter wrong password.
                {
                    isShowing = true;
                    showingTime = 2f;
                    alertText.text = "Wrong password";
                    passwordInput.text = "";
                }
            }
            else if(url == signupURL)
            {
                if(string.Equals(result, "1")) // User already exists
                {
                    isShowing = true;
                    showingTime = 2f;
                    alertText.text = "Already Exists";
                }
                else // Sign up succesfully.
                {
                    signinMenu.SetActive(true);
                    signupMenu.SetActive(false);
                    isShowing = true;
                    showingTime = 2f;
                    alertText.text = "Successful";
                }                                                
            }
        }
    }

    IEnumerator RequestRead(string url)
    {               
        // Send user_id to server.
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetInt("USER_ID"));        

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            // Receive user data
            Result loadData = JsonUtility.FromJson<Result>(uwr.downloadHandler.text);
            bluegillCaughtText.text = loadData.blueGillCaught.ToString();   
            bassCaughtText.text = loadData.bassCaught.ToString();
            muskieCaughtText.text = loadData.muskieCaught.ToString();
            bluegillTotalText.text = loadData.blueGillTotal.ToString();
            bassTotalText.text = loadData.bassTotal.ToString();
            muskieTotalText.text = loadData.muskieTotal.ToString();    
            priceText.text = loadData.coin.ToString();     
        }
    }

    IEnumerator RequestSell(string url)
    {               
        // Send "sell data" to server
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetInt("USER_ID")); 
        form.AddField("bass", int.Parse(bassSellText.text));
        form.AddField("blueGill", int.Parse(bluegillSellText.text));
        form.AddField("muskie", int.Parse(muskieSellText.text));    
        form.AddField("coin", int.Parse(bluegillSellText.text) * 120 + int.Parse(bassSellText.text) * 150
                            + int.Parse(muskieSellText.text) * 100);  

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            // Receive results from server.
            Result loadData = JsonUtility.FromJson<Result>(uwr.downloadHandler.text);
            bassCaughtText.text = loadData.bassCaught.ToString();
            bluegillCaughtText.text = loadData.blueGillCaught.ToString();
            muskieCaughtText.text = loadData.muskieCaught.ToString();
            priceText.text = loadData.coin.ToString();
            bluegillSellText.text = "0";
            bassSellText.text = "0";
            muskieSellText.text = "0";                           
        }
    }
}
