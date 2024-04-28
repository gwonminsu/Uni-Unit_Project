using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginScreenToggle : MonoBehaviour
{
    private VisualElement loginScreen;
    private VisualElement joinScreen;
    private Button openLoginScreenButton;
    private Button openJoinScreenButton;
    private Button closeLoginButton;
    private Button closeJoinButton;
    private TextField usernameField;
    private TextField passwordField;
    private TextField joinUsernameField;
    private TextField joinPasswordField;
    
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

        //ui 요소 찾기
        loginScreen = rootVisualElement.Q<VisualElement>("LoginScreen");
        joinScreen = rootVisualElement.Q<VisualElement>("JoinScreen");
        openLoginScreenButton = rootVisualElement.Q<Button>("openLoginScreen-btn");
        openJoinScreenButton = rootVisualElement.Q<Button>("openJoinScreen-btn");
        closeLoginButton = rootVisualElement.Q<Button>("closeLogin-btn");
        closeJoinButton = rootVisualElement.Q<Button>("closeJoin-btn");
        usernameField = rootVisualElement.Q<TextField>("username");
        passwordField = rootVisualElement.Q<TextField>("password");
        joinUsernameField = rootVisualElement.Q<TextField>("joinusername");
        joinPasswordField = rootVisualElement.Q<TextField>("joinpassword");


        // 초기 화면
        CloseLoginScreen();
        CloseJoinScreen();

        //힌트 설정
        SetTextFieldHint(usernameField, "아이디");
        SetTextFieldHint(passwordField, "패스워드");
        SetTextFieldHint(joinUsernameField, "아이디");
        SetTextFieldHint(joinPasswordField, "패스워드");

        // 이벤트 콜백 등록
        openLoginScreenButton.clicked += OpenLoginScreen;
        openJoinScreenButton.clicked += OpenJoinScreen;
        closeLoginButton.clicked += CloseLoginScreen;
        closeJoinButton.clicked += CloseJoinScreen;

    }

    private void SetTextFieldHint(TextField textField, string hintText)
    {
        textField.RegisterCallback<FocusInEvent>(e => 
        {
            if (textField.value == hintText)
            {
                textField.value = "";
                textField.RemoveFromClassList("hintText"); // hintText 스타일 제거
            }
        });

        textField.RegisterCallback<FocusOutEvent>(e => 
        {
            if (string.IsNullOrEmpty(textField.value))
            {
                textField.value = hintText;
                textField.AddToClassList("hintText"); // hintText 스타일 추가
            }
        });

        // 초기 힌트 텍스트 설정
        textField.value = hintText;
        textField.AddToClassList("hintText"); // CSS 클래스를 통해 스타일 지정
    }

    private void OpenLoginScreen()
    {
        // Set the login screen to be flexibly displayed.
        loginScreen.style.display = DisplayStyle.Flex;
    }

    private void CloseLoginScreen()
    {
        // Hide the login screen.
        loginScreen.style.display = DisplayStyle.None;
    }

    private void OpenJoinScreen()
    {
        // Set the login screen to be flexibly displayed.
        joinScreen.style.display = DisplayStyle.Flex;
    }

    private void CloseJoinScreen()
    {
        // Hide the login screen.
        joinScreen.style.display = DisplayStyle.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
