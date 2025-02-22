using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CoinData
{
    public Transform targetPosition;
    public bool isDestination;
    public Animator destinationAnimator;
}
public class Coin : MonoBehaviour
{
    public List<CoinData> targetPositions;
    public float moveSpeed = 2f;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;

    private EventManager em;
    private bool isMoving = false;
    private Vector3 initialPosition;

    private bool done = false;
    private bool collided = false;

    private bool reachedDestination = false;
    private int destinationIndex;

    void Start()
    {
        gameObject.tag = "Coin";
        em = GameObject.FindWithTag("GameController").GetComponent<EventManager>();
    }

    void Update()
    {
        if (em.gameOver) return;
        if (!reachedDestination || !em.undoModeActivated)
        {
            if (done || collided) return;
        }

        if (em.undoModeActivated && destinationIndex == 0) return;

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector3 clickPosition;
            if (Input.touchCount > 0)
            {
                clickPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null && hit.transform == transform)
            {
                if (!isMoving && targetPositions.Count > 0)
                {
                    done = true;

                    if (em.undoModeActivated && reachedDestination)
                    {
                        ReverseMove();
                    }
                    else
                    {
                        StartCoroutine(MoveThroughTargets(targetPositions));
                    }
                }
            }
        }
    }

    //Call it when need travel to birth place
    private void ReverseMove()
    {
        em.ToggleUndoMode();
        targetPositions[destinationIndex].targetPosition.GetComponent<Vacancy>().isVacant = true;
        List<CoinData> data = new List<CoinData>(targetPositions);
        data = RemoveUpcommingItems(destinationIndex, data);
        data.Reverse();
        StartCoroutine(MoveThroughTargets(data, true));
    }

    private List<CoinData> RemoveUpcommingItems(int index, List<CoinData> data)
    {
        if (data.Count - 1 == index) return data;
        for (int i = data.Count - 1; i > index; i--)
        {
            data.RemoveAt(i);
        }
        return data;
    }

    private IEnumerator MoveThroughTargets(List<CoinData> targetPositions, bool reverseMove = false)
    {
        isMoving = true;

        foreach (CoinData target in targetPositions)
        {
            while (Vector3.Distance(transform.position, target.targetPosition.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.targetPosition.position, moveSpeed * Time.deltaTime);
                if (collided) { isMoving = false; yield break; }
                yield return null;
            }

            if (!reverseMove)
            {
                if (target.isDestination)
                {
                    if (!CheckOtherTargetsHaveDestination(target))
                    {
                        target.targetPosition.GetComponent<Vacancy>().isVacant = false;
                        target.destinationAnimator.SetTrigger("Reached"); isMoving = false; reachedDestination = true; destinationIndex = targetPositions.IndexOf(target); em.CheckLevelComplete(); yield break;
                    }
                }
            }
        }

        if (reverseMove)
        {
            reachedDestination = false;
            done = false;
            destinationIndex = 0;
        }
        else
        {
            reachedDestination = true;
        }
        isMoving = false;
    
    }

    private bool CheckOtherTargetsHaveDestination(CoinData target)
    {
        bool result = false;
        int index = targetPositions.IndexOf(target);

        if (targetPositions.Count - 1 < index + 1) return result;

        for (int i = index + 1; i < targetPositions.Count; i++)
        {
            if (targetPositions[i].isDestination)
            {
                Debug.Log("Trying to access target " + i);
                    
                //check if that destination is occupied
                //if occupied return false else true
                Vacancy status = targetPositions[i].targetPosition.GetComponent<Vacancy>();
                if (!status.isVacant)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }

                break;
            }
        }

        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            OnObjectCollided();
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SpriteRenderer sr = collision.transform.GetChild(0).GetComponent<SpriteRenderer>();
            SpriteRenderer sr2 = transform.GetComponent<SpriteRenderer>();
            if (sr.color != sr2.color)
            {
                OnObjectCollided();
            }
            else
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Animator>().SetTrigger("Break");
            }
        }
    }

    private void OnObjectCollided()
    {
        Handheld.Vibrate();
        em.TriggerLevelFailScreen();
        transform.GetComponent<CircleCollider2D>().enabled = false;
        collided = true;

        StartCoroutine(ShakeObject(shakeDuration, shakeMagnitude));
    }

    private IEnumerator ShakeObject(float duration, float magnitude)
    {
        float elapsed = 0f;
        initialPosition = transform.position;

        while (elapsed < duration)
        {
            float x = initialPosition.x + Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;
    }

    public bool ReachedDestination { get { return reachedDestination; } }
}
