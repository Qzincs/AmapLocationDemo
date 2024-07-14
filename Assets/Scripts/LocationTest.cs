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
        // ͨ��UnityPlayer���ȡ��ǰActivity
        unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentAcitivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        // ͬ����Ȩ���ߺ���˽Э��
        locationClientClass = new AndroidJavaClass("com.amap.api.location.AMapLocationClient");
        locationClientClass.CallStatic("updatePrivacyAgree", currentAcitivity, true);
        locationClientClass.CallStatic("updatePrivacyShow", currentAcitivity, true, true);
        // ������λ�ͻ���
        locationClient = new AndroidJavaObject("com.amap.api.location.AMapLocationClient", currentAcitivity);
        locationOption = new AndroidJavaObject("com.amap.api.location.AMapLocationClientOption");
        // �˴����ԶԶ�λ�����������ã��ο� https://lbs.amap.com/api/android-location-sdk/guide/android-location/getlocation#configure
        locationClient.Call("setLocationOption", locationOption);
        locationListener = new LocationListener();
        // ע�ᶨλ����
        locationListener.OnLocationChangedEvent += OnLocationChanged;
        // ���ö�λ����
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
    /// ��λ�ص�
    /// </summary>
    /// <param name="location">��λ��Ϣ���ο�<a href="https://amappc.cn-hangzhou.oss-pub.aliyun-inc.com/lbs/static/unzip/Android_Location_Doc/index.html">�ٷ��ĵ�</a></param>
    /// 
    public void OnLocationChanged(AndroidJavaObject location)
    {
        if(location != null)
        {
            // �ɹ���ȡ��λ��Ϣ
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
                InfoText.text = $"Location Error��{location.Call<int>("getErrorCode")} {location.Call<string>("getErrorInfo")}";
            }
        }
    }
}
