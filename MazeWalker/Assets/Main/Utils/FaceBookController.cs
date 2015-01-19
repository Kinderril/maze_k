using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class FaceBookController : MonoBehaviour {

    // Use this for initialization
    Facebook.FacebookDelegate fbDelegate;
    public Text debugt;

	void Awake () {
       // FB.Init(SetInit, OnHideUnity);  
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SetInit()
    {
        //Util.Log("SetInit");
        //enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
        if (FB.IsLoggedIn)
        {
           // Util.Log("Already logged in");
            OnLoggedIn();
        }
        else
        {

           // debugt.text = "NOT !!! Logged in. ID: " + FB.UserId;
            CallFBLogin();

        }
    }

    private void OnHideUnity(bool isGameShown)
    {
       // Util.Log("OnHideUnity");
        if (!isGameShown)
        {
            // pause the game - we will need to hide                                             
            Time.timeScale = 0;
        }
        else
        {
            // start the game back up - we're getting focus again                                
            Time.timeScale = 1;
        }
    }

    void OnLoggedIn()
    {
       // debugt.text = "Logged in. ID: " + FB.UserId;
        Debug.Log("Logged in. ID: " + FB.UserId);
        MyPictureCallback();
        //CallFBFeed();
        fbDelegate = cb;
    }
    private void CallFBLogin()
    {
        FB.Login("email,publish_actions", LoginCallback);
    }

    void LoginCallback(FBResult result)
    {
        if (result.Error != null)
        {
         //   debugt.text = "Error Response:\n" + result.Error;
        }
        else if (!FB.IsLoggedIn)
        {
            // debugt.text = "Login cancelled by Player";
        }
        else
        {
            // debugt.text = "Login was successful!";
            MyPictureCallback();
        }
    }

    private void cb(FBResult result)
    {
        Debug.Log("result  " + result);
       // debugt.text = "MyPictureCallback";
    }


    void MyPictureCallback()
    {
        FB.Feed(
            linkCaption: "I just smashed " +  " friends! Can you beat it?",
            picture: "http://www.friendsmash.com/images/logo_large.jpg",
            linkName: "Checkout my Friend Smash greatness!",
            link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest"),
            callback: fbDelegate
            ); 


    }

    public string FeedToId = "";
    public string FeedLink = "";
    public string FeedLinkName = "";
    public string FeedLinkCaption = "";
    public string FeedLinkDescription = "";
    public string FeedPicture = "";
    public string FeedMediaSource = "";
    public string FeedActionName = "";
    public string FeedActionLink = "";
    public string FeedReference = "";
    public bool IncludeFeedProperties = false;
    private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();

    private void CallFBFeed()
    {
        Dictionary<string, string[]> feedProperties = null;
        if (IncludeFeedProperties)
        {
            feedProperties = FeedProperties;
        }
        FB.Feed(
            toId: FeedToId,
            link: FeedLink,
            linkName: FeedLinkName,
            linkCaption: FeedLinkCaption,
            linkDescription: FeedLinkDescription,
            picture: FeedPicture,
            mediaSource: FeedMediaSource,
            actionName: FeedActionName,
            actionLink: FeedActionLink,
            reference: FeedReference,
            properties: feedProperties,
            callback: Callback
        );
    }

    protected void Callback(FBResult result)
    {
        string lastResponse; 
        //lastResponseTexture = null;
        // Some platforms return the empty string instead of null.
        if (!String.IsNullOrEmpty(result.Error))
        {
            lastResponse = "Error Response:\n" + result.Error;
        }
        else if (!String.IsNullOrEmpty(result.Text))
        {
            lastResponse = "Success Response:\n" + result.Text;
        }
        else if (result.Texture != null)
        {
            //lastResponseTexture = result.Texture;
            lastResponse = "Success Response: texture\n";
        }
        else
        {
            lastResponse = "Empty Response\n";
        }
    }



    internal void SendImage()
    {
        FB.Init(SetInit, OnHideUnity); 
    }
}
