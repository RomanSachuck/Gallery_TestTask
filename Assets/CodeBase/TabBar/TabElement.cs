using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.TabBar
{
    [Serializable]
    public class TabElement
    {
        [field:SerializeField] public TabFilterType FilterType { get; private set; }
        [field:SerializeField] public Button Button { get; private set; }
        [field:SerializeField] public TextMeshProUGUI Text { get; private set; }
    }
}