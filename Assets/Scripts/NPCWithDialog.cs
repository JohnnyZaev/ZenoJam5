using System;
using DialogueEditor;
using UnityEngine;

public class NPCWithDialog : MonoBehaviour
{
	private bool _inPlayerRadius;
	[SerializeField] private NPCConversation conversation;
	

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && _inPlayerRadius)
			ConversationManager.Instance.StartConversation(conversation);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_inPlayerRadius = true;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		_inPlayerRadius = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_inPlayerRadius = false;
	}

}
