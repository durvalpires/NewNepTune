using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourcesLoader : IResourceProvider
{

    public override void ClearCache()
    {
        _spriteCache.Clear();
    }
}
