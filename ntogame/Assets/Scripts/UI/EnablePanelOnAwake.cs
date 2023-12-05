using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnablePanelOnAwake : MonoBehaviour
{
    void Awake()
    {
        Image panel = GetComponent<Image>();
        panel.enabled = true;
    }
}
