using DialogueEditor;
using UnityEngine;

public class NPCWithDialog : MonoBehaviour
{
	private bool _inPlayerRadius;
	[SerializeField] private NPCConversation conversation;


	private void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0) && _inPlayerRadius)
			ConversationManager.Instance.StartConversation(conversation);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_inPlayerRadius = true;
	}
	private void OnTriggerStay(Collider other)
	{
		_inPlayerRadius = true;
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		_inPlayerRadius = false;
	}

}
