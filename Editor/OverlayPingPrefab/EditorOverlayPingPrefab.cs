using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Overlays;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine.UIElements;
using NUnit.Framework.Internal;
using System;
using UnityEditor.Toolbars;

[Overlay(typeof(SceneView), "PingPrefabToolbar", true)]
[Icon("Assets/Editor/OverlayPingPrefab/pingIcon.png)")]
public class EditorOverlayPingPrefab : ToolbarOverlay
{
    EditorOverlayPingPrefab() : base(ButtonPingPrefab.id) { }
}

