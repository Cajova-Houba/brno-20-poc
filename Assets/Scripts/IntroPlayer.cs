using Assets.Scripts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts
{
    /// <summary>
    /// Simple class which deactivates video panel after the video has ended. 
    /// </summary>
    public class IntroPlayer : MonoBehaviour
    {
        public VideoPlayer videoPlayer;

        public GameObject videoPanel;

        public AbstractLevel level;

        void Start()
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SkipIntro();
            }
        }

        void SkipIntro()
        {
            videoPlayer.playbackSpeed = 1000f;
        }

        void OnVideoEnd(VideoPlayer vp)
        {
            videoPanel.SetActive(false);
            if (level != null)
            {
                level.OnIntroEnd();
            }
        }
    }
}
