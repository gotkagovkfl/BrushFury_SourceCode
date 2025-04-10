using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;
using Spine.Unity;

namespace BW
{
    public static class BwTask 
    {
        public static async UniTask<bool> CanNextStep(this UniTask task, CancellationToken token)
        {
            await task.SuppressCancellationThrow();
            return token.IsCancellationRequested == false;
        }

        public static async UniTask TaskWhile(this Action updateAction, float duration, CancellationToken token)
        {
            //
            if(updateAction==null)
            {
                return;
            }

            //
            float elapsed = 0;
            while(elapsed<duration)
            {
                updateAction();
                
                elapsed += Time.deltaTime;
                if(await UniTask.Yield(PlayerLoopTiming.Update, token).CanNextStep(token)==false) return;
            }
        }
    }
}

