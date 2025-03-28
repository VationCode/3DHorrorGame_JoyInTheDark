using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyController : MonoBehaviour
{
    [SerializeField] Canvas InteractionUI_key;
    [SerializeField] Canvas InteractionUI_door;

    public List<KeyObject> keyList = new List<KeyObject>();
    public List<KeyObject> nearkeyList = new List<KeyObject>();

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sound_pick;

    public Door nearDoor;

    private void Update()
    {
        // Key ���ͷ���
        if (nearkeyList.Count > 0)
        {
            InteractionUI_key.enabled = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                KeyObject key = nearkeyList[nearkeyList.Count - 1];
                nearkeyList.RemoveAt(nearkeyList.Count - 1);
                PickUpKey(key);
            }
        } else
        {
            InteractionUI_key.enabled = false;
        }

        // Door ���ͷ���
        if (nearDoor)
        {
            InteractionUI_door.enabled = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (nearDoor.isOpened)
                {
                    CloseDoor(nearDoor);
                } else
                {
                    OpenDoor(nearDoor);
                }
            }
        } else
        {
            InteractionUI_door.enabled = false;
        }
    }

    public void OpenDoor(Door door)
    {
        if (door.isUnlocked) 
        {
            door.Open();
            return;
        }

        KeyObject key = keyList.Find((KeyObject key) => key.id == door.keyID);
        if (key != null)
        {
            door.Unlock();
        }

        door.Open();
    }

    public void CloseDoor(Door door)
    {
        door.Close();
    }

    public void PickUpKey(KeyObject key)
    {
        if (keyList.Contains(key))
        {
            Debug.LogError($"���� Ű �ι� ȹ�� �߻� {key}");
            return;
        }

        audioSource.PlayOneShot(sound_pick);

        key.gameObject.SetActive(false);
        keyList.Add(key);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyObject"))
        {
            KeyObject key = other.GetComponent<KeyObject>();

            if (!nearkeyList.Contains(key))
            {
                nearkeyList.Add(key);
            }
        } 
        else if (other.CompareTag("Door"))
        {
            nearDoor = other.GetComponent<Door>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("KeyObject"))
        {
            KeyObject key = other.GetComponent<KeyObject>();

            if (nearkeyList.Contains(key))
            {
                nearkeyList.Remove(key);
            }
        }
        else if (other.CompareTag("Door"))
        {
            nearDoor = null;
        }
    }
}
