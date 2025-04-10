using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Linq;



namespace BW
{
    public static class DataProcessor 
    {
        public static Vector3 WithStandardHeight(this Vector3 pos)
        {
            return new Vector3(pos.x, 1f, pos.z);
        }

        public static Vector3 WithFloorHeight(this Vector3 pos)
        {
            return new Vector3(pos.x, 0f, pos.z);
        }


        // public static float GetSqrDistWith(this Vector3 pos, Vector3 targetPos)
        // {
        //     return (targetPos- pos).sqrMagnitude;
        // }
        #region Data structure\


        /// <summary>
        /// 해당 리스트의 사이즈를 targetCount로 고정한다. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="targetCount"></param>
        public static void EnsureListSize<T>(this List<T> list, int targetCount)
        {
            if (list.Count >targetCount)
            {
                list.RemoveRange(targetCount, list.Count - targetCount); // 5개 초과 시 초과된 원소 삭제
            }
            else if (list.Count < targetCount)
            {
                list.AddRange(Enumerable.Repeat(default(T), targetCount - list.Count)); // 부족한 원소 추가
            }
        }









        /// <summary>
        /// 중복 없이 dataNum개의 데이터를 list에서 가져온다.
        /// </summary>
        /// <param name="targetNum"></param>
        /// <returns></returns>
        public static List<T> GetRandomDataWithException<T>(this List<T> sourceList,  int targetNum, List<T> exception=null)
        {
            // 
            List<T> temp = sourceList.GetItemsWithExption(exception);
            List<T> ret = temp.GetRandomUniqueItems(targetNum).ToList();
            //
            return ret;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<T> GetRandomUniqueItems<T>(this List<T> sourceList, int n) 
        {
            if (n > sourceList.Count)
            {
                n = sourceList.Count;
            }
            
            System.Random random = new System.Random();
            // 랜덤하게 섞기
            return sourceList.OrderBy(x => random.Next()).Take(n).ToList();
        }

        /// <summary>
        /// 특정 데이터를 제외한 데이터를 받아온다. 
        /// </summary>
        /// <param name="sourceList"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static List<T> GetItemsWithExption<T>(this List<T> sourceList, List<T> exception)
        {
            if (exception == null || exception.Count == 0)
            {
                return sourceList;
            }

            return sourceList.Except(exception).ToList(); 
        }


        #endregion
    }

    public static class Math
    {
        public static int GetRandom(int min, int max)
        {
            // byte[] buffer = new byte[4];
            // RandomNumberGenerator.Fill(buffer);
            // uint randomUint = BitConverter.ToUInt32(buffer, 0);

            // // 정수 범위로 변환
            // return (int)(randomUint % (max - min)) + min;
            // UnityEngine.Random.seed
            return UnityEngine.Random.Range(min, max); 
        }

        public static float GetRandom(float min, float max)
        {
            byte[] buffer = new byte[4];
            RandomNumberGenerator.Fill(buffer);
            uint randomUint = BitConverter.ToUInt32(buffer, 0);

            // 부동소수점 범위로 변환
            return (randomUint / (float)uint.MaxValue) * (max - min) + min;
        }


        public static Vector3 GetPointOnUnitCircleCircumference(float radius = 1)
        {
            float randomAngle = GetRandom(0f, Mathf.PI * 2f);
            return new Vector3(Mathf.Sin(randomAngle),0, Mathf.Cos(randomAngle)) * radius;
        }

        public static Vector3 GetRandomPointOnCircle(float maxRadius = 1f)
        {
            float randRadius = GetRandom(0, maxRadius);

            float randomAngle = GetRandom(0f, Mathf.PI * 2f);
            return new Vector3(Mathf.Sin(randomAngle),0, Mathf.Cos(randomAngle)) * randRadius ;
        }
    }
}

