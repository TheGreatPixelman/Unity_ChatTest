using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    GameObject _chatContentPane;

    [SerializeField]
    TMP_InputField _chatInputField;

    [SerializeField]
    ScrollRect _scrollRect;

    [SerializeField]
    ChatItem _prefab_ChatItem;

    private void Awake()
    {
        //Verifies that all references are set before doing anything
        if (_chatContentPane is null || _chatInputField is null || _scrollRect is null || _prefab_ChatItem is null)
            Debug.LogError($"There is a reference missing, check {this.name} references");


        //Assigns the onSubmit event to the AddMessageToChat Method 
        _chatInputField.onSubmit.AddListener(delegate { AddMessageToChat(_chatInputField.text); });
    }

    // Start is called before the first frame update
    void Start()
    {
        //resets the chat position to the bottom
        _scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    /// <summary>
    /// sends a message to the chat
    /// </summary>
    /// <param name="message"></param>
    private void AddMessageToChat(string message)
    {
        //trims the message to not have empty space messages
        string trimmedMessage = message.Trim();

        //if the message is empty don't send anything
        if (string.IsNullOrEmpty(trimmedMessage))
            return;

        //Creates the message item and instantiate it's text value
        ChatItem chatItem = Instantiate(_prefab_ChatItem, _chatContentPane.transform);
        chatItem.text.text = trimmedMessage;

        //Gets the actual size of the message by iterating each lines of the text
        float chatItemHeight = 0;
        foreach (var line in chatItem.text.GetTextInfo(trimmedMessage).lineInfo)
        {
            chatItemHeight += line.lineHeight;
        }

        //Checks if the chatItemHeight is below the minimum of 5, if it ain't then we apply the new height) 
        chatItemHeight = chatItemHeight < 5 ? 5 : chatItemHeight;

        RectTransform chatItemRectTransform = chatItem.GetComponent<RectTransform>();
        //SizeDelta.x is the Width of the RectTransform (this could be done in the ChatItem component)
        chatItemRectTransform.sizeDelta = new Vector2(chatItemRectTransform.sizeDelta.x, chatItemHeight);

        //Dynamically sets the height of the chat (this could be done in a ChatContent component)
        RectTransform chatContentRecTransform = _chatContentPane.GetComponent<RectTransform>();
        chatContentRecTransform.sizeDelta = new Vector2(chatContentRecTransform.sizeDelta.x, chatContentRecTransform.sizeDelta.y + chatItemHeight + chatContentRecTransform.GetComponent<VerticalLayoutGroup>().spacing);

        //Puts the view back to the bottom so that the new message isn't hidden under the content view
        _scrollRect.normalizedPosition = new Vector2(0,0);

        ClearInput();
    }

    /// <summary>
    /// Clears the chat input form the sent message
    /// </summary>
    private void ClearInput()
    {
        _chatInputField.text = "";
    }
}
