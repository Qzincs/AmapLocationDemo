using UnityEngine;

/// <summary>
/// 实现高德的AMapLocationListener接口，在Unity脚本中实现回调的具体逻辑
/// </summary>
public class LocationListener : AndroidJavaProxy
{
    public delegate void LocationChangedDelegate(AndroidJavaObject location);
    public event LocationChangedDelegate OnLocationChangedEvent;

    public LocationListener() : base("com.amap.api.location.AMapLocationListener") {  }

    public void onLocationChanged(AndroidJavaObject location)
    {
        OnLocationChangedEvent?.Invoke(location);
    }
}
