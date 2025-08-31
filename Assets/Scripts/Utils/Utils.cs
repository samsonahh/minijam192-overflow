using Eflatun.SceneReference;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class Utils
{
    public static SceneReference GetCurrentScene() => SceneReference.FromScenePath(SceneManager.GetActiveScene().path);

    public static float ConvertVolumeToDecibels(float volume) => Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
    public static float ConvertDecibelsToVolume(float decibels) => Mathf.Pow(10f, decibels / 20f);

    public static string FloatToString(float value, int decimalPlaces = 2)
    {
        string format = "F" + decimalPlaces;
        return value.ToString(format);
    }
}