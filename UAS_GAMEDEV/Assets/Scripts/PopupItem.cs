using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupItem : InteractableObject
{
    [SerializeField][TextArea(1, 3)] private string[] interact_text;

    public override void TryInteract()
    {
        DialogueManager.instance.startDialogue(interact_text);
    }
}
