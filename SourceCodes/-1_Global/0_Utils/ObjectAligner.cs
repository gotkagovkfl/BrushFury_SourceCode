using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UIElements;

namespace BW
{
    public static class Util
    {
        /// <summary>
        /// X축으로 정렬 (가로)
        /// </summary>
        /// <param name="transforms"></param>
        /// <param name="center"></param>
        /// <param name="space"></param>
        public static void AlignObjects_X(this List<Transform> transforms, Vector3 center, float space = 5f)
        {
            int count = transforms.Count;
            // 짝수
            if(count % 2 == 0)
            {
                for(int i=0;i<count;i+=2)
                {
                    float offset = space * i/2 + space*0.5f;
                    transforms[i].position = center + new Vector3( -offset, 0, 0 );
                    transforms[i+1].position = center + new Vector3( offset, 0, 0 );
                }
                
                
            }
            // 홀수
            else
            {
                transforms[0].position = center;
                
                for(int i=1;i<count;i+=2)
                {
                    float offset = space * ( (i+1)/2 );
                    transforms[i].position = center + new Vector3( -offset, 0, 0 );
                    transforms[i+1].position = center + new Vector3( offset, 0, 0 );
                }
            }
        
        }


        /// <summary>
        /// Z축으로 정렬 (세로 )
        /// </summary>
        /// <param name="transforms"></param>
        /// <param name="center"></param>
        /// <param name="space"></param>
        public static void AlignObjects_Z(this List<Transform> transforms, Vector3 center, float space = 5f)
        {
            int count = transforms.Count;
            // 짝수
            if(count % 2 == 0)
            {
                for(int i=0;i<count;i+=2)
                {
                    float offset = space * i/2 + space*0.5f;
                    transforms[i].position = center + new Vector3(  0, 0,   -offset);
                    transforms[i+1].position = center + new Vector3(  0, 0, offset );
                }
                
                
            }
            // 홀수
            else
            {
                transforms[0].position = center;
                
                for(int i=1;i<count;i+=2)
                {
                    float offset = space * ( (i+1)/2 );
                    transforms[i].position = center + new Vector3(  0, 0,   -offset );
                    transforms[i+1].position = center + new Vector3(  0, 0, offset );
                }
            }
        
        }

    }
}

