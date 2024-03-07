// /* ****************************************************************
//  * @File Name   :   WaitForEndOfAnimation
//  * @Author      :   GF47
//  * @Date        :   2015/12/17/星期四 17:20:48
//  * @Description :   to do
//  * @Edit        :   2015/12/17/星期四 17:20:48
//  * ***************************************************************/
// using UnityEngine;
// using System.Collections;
// 
// namespace GRT.Coroutine
// {
//     /// <summary>
//     /// A example class in Unity
//     /// </summary>
//     public class WaitForEndOfAnimation : IEnumerator
//     {
//         private Animation _animation;
// 
//         public bool MoveNext()
//         {
//             return _animation.isPlaying;
//         }
// 
//         public void Reset()
//         {
//             _animation.Stop();
//             _animation.Play();
//         }
// 
//         public object Current { get { return _animation[_animation.clip.name]; } }
// 
//         public WaitForEndOfAnimation(Animation animation)
//         {
//             _animation = animation;
//             _animation.Play();
//         }
//     }
// }
