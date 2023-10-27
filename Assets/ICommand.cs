using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.CommandNew
{
    public interface ICommand<T0, T1> 
        where T0 : struct
        where T1 : struct
    {
        T1 Execute(T0 args);
        void Undo(T1 args);
    }
}