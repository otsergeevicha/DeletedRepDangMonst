using System;

namespace Loots
{
    public interface ILoot
    {
        void Open(Action opened);
        string GetName();
    }
}