using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class WorldAmbience : MonoBehaviour  // Changed to inherit from MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip track_1;
    public AudioClip track_2;
    public AudioClip track_3;
    public AudioClip track_4;
    public AudioClip track_5;

    [SerializeField] private float fadeTime = 2f;  // Time to fade in/out
    [SerializeField] private float delayBetweenTracks = 5f;  // Delay between tracks
    [SerializeField] private float maxVolume = 1f;  // Maximum volume for tracks

    private AudioClip[] tracks;
    private AudioClip currentTrack;
    private bool isPlaying = false;

    async Task Start()
    {
        // Initialize audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0;
        audioSource.loop = false;

        // Initialize tracks array
        tracks = new AudioClip[] { track_1, track_2, track_3, track_4, track_5 };
        await Task.Delay(15000);
        // Start playing music
        StartCoroutine(PlayMusicSequence());
    }

    private IEnumerator PlayMusicSequence()
    {
        while (true)
        {
            if (!isPlaying)
            {
                // Select random track different from the current one
                AudioClip newTrack;
                do
                {
                    newTrack = tracks[Random.Range(0, tracks.Length)];
                } while (newTrack == currentTrack && tracks.Length > 1);

                currentTrack = newTrack;
                StartCoroutine(PlayTrackWithFade(currentTrack));
            }
            yield return new WaitForSeconds(1f); // Check every second
        }
    }

    private IEnumerator PlayTrackWithFade(AudioClip track)
    {
        isPlaying = true;

        // Set the track
        audioSource.clip = track;
        audioSource.Play();

        // Fade in
        float currentTime = 0;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, maxVolume, currentTime / fadeTime);
            yield return null;
        }

        // Wait until near the end of the track
        float waitTime = track.length - fadeTime;
        yield return new WaitForSeconds(waitTime);

        // Fade out
        currentTime = 0;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(maxVolume, 0, currentTime / fadeTime);
            yield return null;
        }

        audioSource.Stop();
        isPlaying = false;

        // Wait before playing next track
        yield return new WaitForSeconds(delayBetweenTracks);
    }

    // Optional: Method to stop all music
    public void StopMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeTime);
            yield return null;
        }

        audioSource.Stop();
        isPlaying = false;
    }
}