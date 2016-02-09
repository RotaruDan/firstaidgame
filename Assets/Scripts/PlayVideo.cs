using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]

public class PlayVideo : MonoBehaviour
{

    private MovieTexture movie;

    // Use this for initialization
    void Start()
    {
        HideMovie();
    }

    void HideMovie()
    {
        this.gameObject.SetActive(false);
        movie = null;
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
        Debug.Log("Mouse Down");
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
