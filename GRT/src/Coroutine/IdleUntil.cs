// using System;
// using System.Collections;
// 
// namespace GRT.Coroutine
// {
//     public class IdleUntil : IEnumerator
//     {
//         public Func<bool> Condition { get; private set; }
// 
//         public object Current => Condition;
// 
//         public bool MoveNext() => !Condition?.Invoke() ?? false;
// 
//         public void Reset() => Condition = null;
// 
//         public IdleUntil(Func<bool> condition) => Condition = condition;
//     }
// }