using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]

public class PlayVideo : MonoBehaviour
{
    public UnityEngine.Events.UnityAction videoEnded;
    private MovieTexture movie;

    // Use this for initialization
    void Start()
    {
    }

    void HideMovie()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
        movie = null;
        if (videoEnded != null)
        {
            videoEnded();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (movie != null && !movie.isPlaying && this.gameObject.activeSelf)
        {
            HideMovie();
        }
    }
    
    public void OnMouseDown()
    {
        if (movie != null && movie.isPlaying)
        {
            movie.Stop();
        }
        HideMovie();
    }

    public void playVideo(MovieTexture texture)
    {
        this.movie = texture;
        this.gameObject.SetActive(true);
        GetComponent<RawImage>().texture = movie as MovieTexture;
        movie.Stop();
        movie.Play();
    }
}
