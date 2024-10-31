using System.Collections.Generic;
using Plugins.MonoCache;
using Reflex.Attributes;

namespace Reflex
{
    public class Greeter : MonoCache
    {
        [Inject] private readonly IEnumerable<string> _strings;
    }
}