using System;

namespace S1LV3Rman.RockFall
{
    public abstract class BasePool<T> : DisposableList<T> where T : IDisposable
    {
    }
}