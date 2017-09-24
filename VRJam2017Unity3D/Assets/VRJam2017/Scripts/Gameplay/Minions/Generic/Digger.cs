using UnityEngine;
using System.Collections;

public class Digger : MonoBehaviour
{
    public float DigTime = 2;

    public float DigDistance = 2;
    
    void Start()
    {
        StartCoroutine(CheckDig());
    }

    IEnumerator CheckDig()
    {
        while(true)
        {
            Vector3 tileInFront = transform.position + transform.forward*DigDistance;

            Vector2 tilePosition = LevelHelpers.TilePosFromWorldPos(tileInFront);

            LevelTile tile = LevelController.Instance.Model.Tiles[(int)tilePosition.x, (int)tilePosition.y];

            if(!tile.Opened)
            {
                ActionQueue queue = GetComponent<ActionQueue>();

                queue.InsertBeforeCurrent(new DigAction(gameObject, tilePosition));

                yield return new WaitForSeconds(DigTime);
            }

            yield return 0;
        }
    }
    
}