using System.Collections.Generic;
using UnityEngine;

public abstract class IResourceProvider
{
    protected Dictionary<string, Sprite> _spriteCache = new Dictionary<string, Sprite>();

    public enum IconsId
    {
        money,
        spin
    }

    public abstract void ClearCache();

    public Sprite LoadSprite(string path)
    {
        if (_spriteCache.ContainsKey(path)) return _spriteCache[path];
        var sprite = Resources.Load<Sprite>(path);
        if(!sprite) sprite = Resources.Load<Sprite>("Def/Sprite");
        else if(!_spriteCache.ContainsKey(path)) _spriteCache.Add(path,sprite);
        return sprite;
    }
    protected Sprite LoadSprite(string path, string defPath)
    {
        
        var sprite = Resources.Load<Sprite>(path);
        if(sprite == null)
            sprite = Resources.Load<Sprite>(defPath);
        return sprite;
    }
}