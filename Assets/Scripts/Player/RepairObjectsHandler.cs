using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairObjectsHandler : MonoBehaviour
{
    private PlayerController controller;

    public List<Item> items;

    private bool isNearItem = false;
    public List<Item> nearbyItems = null;

    private bool isNearPiece = false;
    private Piece nearbyPiece;

    void Start()
    {
        controller = GetComponent<PlayerController>();

        items = new List<Item>();
        nearbyItems = new List<Item>();
    }

    void Update()
    {
        if (Input.GetButtonDown("InteractButton"))
        {
            if (isNearItem && !isNearPiece)
            {
                PickItem();
            }
            else if (isNearPiece)
            {
                RequestRepair();
            }
        }
    }

    //PICK
    void PickItem()
    {
        Item itemToGrab = GetClosestItem();
        if (itemToGrab.Take())
        {
            items.Add(itemToGrab);
            Piece.UpdateAll(items);
            controller.movementStatus = PlayerController.MovementStates.running;
        }
    }

    void RemoveNearItem(int toRemoveIndex)
    {
        nearbyItems.RemoveAt(toRemoveIndex);
        if (nearbyItems.Count <= 0)
        {
            isNearItem = false;
        }
    }

    void AddNearItem(Item item)
    {
        nearbyItems.Add(item);

        if (!isNearItem)
        {
            isNearItem = true;
        }
    }

    private void RequestRepair()
    {
        if (nearbyPiece.Repair(items) == 1)
        {
            WipeUsedItems(nearbyPiece.itemsNeeded);
            UpdatePieceStatus(false);
            Debug.Log("La pièce est réparée et inventaire vidé");
        }
    }

    private void WipeUsedItems(List<Item> usedItems)
    {
        int size = usedItems.Count;
        string keyToSearch;
        int toRemoveIndex = 0;

        for (int i = 0; i < size; i++)
        {
            if (Contains(items, usedItems[i]))
            {
                items.RemoveAt(GetIndexInList(items, usedItems[i]));
            }
        }
        Piece.UpdateAll(items);
    }

    private void UpdatePieceStatus(bool updateValue, Piece newPiece = null)
    {
        isNearPiece = updateValue;
        nearbyPiece = newPiece;
        controller.movementStatus = PlayerController.MovementStates.walking;
    }


    //List tools
    int GetIndexInList(List<Item> listToLook, Item itemToSearch)
    {
        int size = listToLook.Count;

        for (int i = 0; i < size; i++)
        {
            if (listToLook[i].id == itemToSearch.id)
            {
                return i;
            }
        }

        return -1;
    }

    private bool Contains(List<Item> listToLook, Item itemToSearch)
    {
        int size = listToLook.Count;
        foreach (Item item in listToLook)
        {
            if (item.id == itemToSearch.id)
            {
                return true;
            }
        }
        return false;
    }

    Item GetClosestItem()
    {
        float currentMinDistance = Vector2.Distance(transform.position, nearbyItems[0].transform.position);
        Item closestItem = nearbyItems[0];

        for (int i = 1; i < nearbyItems.Count; i++)
        {
            float newDistance = Vector2.Distance(transform.position, nearbyItems[i].transform.position);
            if (newDistance < currentMinDistance)
            {
                currentMinDistance = newDistance;
                closestItem = nearbyItems[i];
            }
        }
        return closestItem;
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Item otherItem = other.GetComponent<Item>();
            RemoveNearItem(GetIndexInList(nearbyItems, otherItem));
        }
        else if (other.CompareTag("Piece"))
        {
            UpdatePieceStatus(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            isNearItem = true;
            Item otherItem = other.GetComponent<Item>();
            if (!Contains(nearbyItems, otherItem))
            {
                AddNearItem(other.GetComponent<Item>());
            }
        }
        else if (other.CompareTag("Piece"))
        {
            Piece otherPiece = other.gameObject.GetComponent<Piece>();
            UpdatePieceStatus(true, otherPiece);
        }
    }
}
