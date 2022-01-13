using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : Singleton<WeaponUI>
{
    [SerializeField] TMP_Text bulletCountText;

    public void SetBulletCount(int bulletCount, int maxBulletCount)
    {
        bulletCountText.text = string.Format("{0}/{1}", bulletCount, maxBulletCount);
    }
}
