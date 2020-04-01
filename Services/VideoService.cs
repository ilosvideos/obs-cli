using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obs_cli.Services
{
    public static class VideoService
    {
        public const int MAX_FPS = 30;
        public const int MIN_FPS = 4;

        /// <summary>
        /// Gets the appropriate frame rate.
        /// </summary>
        /// <param name="frameRate"></param>
        /// <returns></returns>
        public static uint GetFrameRate(uint frameRate)
        {
            int fps = (int)frameRate;

            if (fps < 4)
            {
                fps = MIN_FPS;
            }
            else if (fps > MAX_FPS)
            {
                fps = MAX_FPS;
            }

            return (uint)fps;
        }
    }
}
