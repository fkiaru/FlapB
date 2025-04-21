using UnityEngine;
using TMPro;
using System.Collections;

public class HighScoreAnimation : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public ParticleSystem fireworks;
    public float displayDuration = 2.5f;
    public float fadeDuration = 1.5f;

    private void OnEnable()
    {
        StartCoroutine(PlayAnimation());
    }
        IEnumerator PlayAnimation()
    {
        // Activer texte et particules
        highScoreText.color = new Color(highScoreText.color.r, highScoreText.color.g, highScoreText.color.b, 1);
        fireworks.Play();

        // Affiche pendant quelques secondes
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        float t = 0;
        Color startColor = highScoreText.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            highScoreText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Désactiver tout
        fireworks.Stop();
        gameObject.SetActive(false);
    }
}

