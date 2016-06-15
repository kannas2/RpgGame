using UnityEngine;
using System.Collections;

public class TestMove : MonoBehaviour {

    public void Start()
    {
        Handheld.PlayFullScreenMovie("ending.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput | FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.Fill);
    }
        //void Update()
        //{
        //    if (Input.GetButtonDown("Jump"))
        //    {

        //        Renderer r = GetComponent<Renderer>();
        //        MovieTexture movie = (MovieTexture)r.material.mainTexture;
        //    Handheld.PlayFullScreenMovie("movie.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput | FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.Fill);
        //    if (movie.isPlaying)
        //        {
        //            movie.Pause();
        //        }
        //        else {
        //            movie.Play();
        //        }
        //    }
        //}
    }

