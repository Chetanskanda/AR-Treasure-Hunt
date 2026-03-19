using System.Collections;
using UnityEngine;

public class TreasureItem : MonoBehaviour
{
    [Header("Coin Settings")]
    public float rotateSpeed = 120f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.025f;
    public int pointValue = 10;

    [Header("Audio")]
    public AudioClip collectSound;

    private Vector3 _startPos;
    private bool _collected = false;

    private AudioSource _audioSource;

void Start()
{
    _startPos = transform.position;
    transform.localScale = new Vector3(0.04f, 0.008f, 0.04f);
    _audioSource = GetComponent<AudioSource>();
}

    void Update()
    {
        if (_collected) return;

        // Spin around Y axis
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        // Bob up and down
        float newY = _startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(_startPos.x, newY, _startPos.z);
    }

    public void Collect()
    {
        if (_collected) return;
        _collected = true;

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(pointValue);

        if (_audioSource != null && collectSound != null)
{
    _audioSource.PlayOneShot(collectSound);
}

        StartCoroutine(CollectAnimation());
    }

    IEnumerator CollectAnimation()
    {
        float duration = 0.35f;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Shrink to zero
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            // Float upward
            transform.position = startPos + Vector3.up * (t * 0.1f);

            yield return null;
        }

        TreasureSpawner spawner = FindObjectOfType<TreasureSpawner>();
        if (spawner != null) spawner.RemoveTreasure(gameObject);
        Destroy(gameObject);
    }
}