using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class BossIntroUI : MonoBehaviour
{
    [Header("보스 프리팹 4개")]
    public GameObject[] bossPrefabs;

    [Header("보스 위치 보정")]
    public float bossSpawnOffsetY = 0.8f;

    [Header("배너")]
    public GameObject bannerPanel;
    public RectTransform bannerRect;

    private GameObject[] spawnedBosses;

    public float fadeTime = 1f;

    public static bool IsPlaying;

    public void PlayBossIntro(Vector3 bossPos, Action<GameObject> onComplete)
    {
        IsPlaying = true;

        StartCoroutine(PlayRoutine(bossPos, onComplete));
    }

    IEnumerator PlayRoutine(Vector3 bossSpawnPos, Action<GameObject> onComplete)
    {
        spawnedBosses = new GameObject[bossPrefabs.Length];

        Vector3 spawnPos =
            bossSpawnPos + Vector3.up * bossSpawnOffsetY;

        for (int i = 0; i < bossPrefabs.Length; i++)
        {
            spawnedBosses[i] =
                Instantiate(bossPrefabs[i], spawnPos, Quaternion.identity);
        }

        for (int i = 0; i < spawnedBosses.Length - 1; i++)
        {
            SpriteRenderer sr =
                spawnedBosses[i].GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                yield return sr
                    .DOFade(0f, fadeTime)
                    .WaitForCompletion();
            }

            Destroy(spawnedBosses[i]);
        }

        GameObject finalBoss =
            spawnedBosses[spawnedBosses.Length - 1];

        yield return PlayBanner();

        IsPlaying = false;

        onComplete?.Invoke(finalBoss);
    }

    IEnumerator PlayBanner()
    {
        bannerPanel.SetActive(true);

        bannerRect.pivot = new Vector2(0f, 0.5f);
        bannerRect.localScale = new Vector3(0f, 1f, 1f);

        yield return bannerRect
            .DOScaleX(1f, 0.35f)
            .WaitForCompletion();

        yield return new WaitForSeconds(1.2f);

        bannerRect.pivot = new Vector2(1f, 0.5f);

        yield return bannerRect
            .DOScaleX(0f, 0.35f)
            .WaitForCompletion();

        bannerPanel.SetActive(false);
    }
}