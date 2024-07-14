using System;
using TMPro;
using UnityEngine;

public class LocationTest : MonoBehaviour
{
    AndroidJavaClass unityPlayerClass;
    AndroidJavaObject currentAcitivity;
    AndroidJavaObject locationClient;
    AndroidJavaClass locationClientClass;
    AndroidJavaObject locationOption;
    LocationListener locationListener;

    public TextMeshProUGUI InfoText;

    public void StartLocation()
    {
        // 通过UnityPlayer类获取当前Activity
        unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentAcitivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        // 同意授权政策和隐私协议
        locationClientClass = new AndroidJavaClass("com.amap.api.location.AMapLocationClient");
        locationClientClass.CallStatic("updatePrivacyAgree", currentAcitivity, true);
        locationClientClass.CallStatic("updatePrivacyShow", currentAcitivity, true, true);
        // 创建定位客户端
        locationClient = new AndroidJavaObject("com.amap.api.location.AMapLocationClient", currentAcitivity);
        locationOption = new AndroidJavaObject("com.amap.api.location.AMapLocationClientOption");
        // 此处可以对定位参数进行设置，参考 https://lbs.amap.com/api/android-location-sdk/guide/android-location/getlocation#configure
        locationClient.Call("setLocationOption", locationOption);
        locationListener = new LocationListener();
        // 注册定位监听
        locationListener.OnLocationChangedEvent += OnLocationChanged;
        // 设置定位监听
        locationClient.Call("setLocationListener", locationListener);
        locationClient.Call("startLocation");
    }

    public void StopLocation()
    {
        locationClient?.Call("stopLocation");
        locationClient?.Call("onDestroy");
        InfoText.text = "Location Stopped";
    }

    /// <summary>
    /// 定位回调
    /// </summary>
    /// <param name="location">定位信息，参考<a href="https://amappc.cn-hangzhou.oss-pub.aliyun-inc.com/lbs/static/unzip/Android_Location_Doc/index.html">官方文档</a></param>
    /// 
    public void OnLocationChanged(AndroidJavaObject location)
    {
        if(location != null)
        {
            // 成功获取定位信息
            if (location.Call<int>("getErrorCode") == 0)
            {
                try
                {
                    InfoText.text = $"{location.Call<long>("getTime")}: {location.Call<double>("getLongitude")}, {location.Call<double>("getLatitude")}";
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                InfoText.text = $"Location Error：{location.Call<int>("getErrorCode")} {location.Call<string>("getErrorInfo")}";
            }
        }
    }
}
