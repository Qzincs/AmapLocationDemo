using UnityEngine;

/// <summary>
/// ʵ�ָߵµ�AMapLocationListener�ӿڣ���Unity�ű���ʵ�ֻص��ľ����߼�
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
