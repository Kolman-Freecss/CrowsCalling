using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Toolbars;
using UnityEditor;
using UnityEngine;

[EditorToolbarElement(id, typeof(SceneView))]
public class ButtonPingPrefab : EditorToolbarButton
{


        // This ID is used to populate toolbar elements.

        public const string id = "EditorToolbar/PingPrefab";

        // Because this is a VisualElement, it is appropriate to place initialization logic in the constructor.

        // In this method you can also register to any additional events as required. In this example there is a tooltip, an icon, and an action.

        public ButtonPingPrefab()
        {

            // A toolbar element can be either text, icon, or a combination of the two. Keep in mind that if a toolbar is docked horizontally the text will be clipped, so it's a good idea to specify an icon.

            text = "Ping Me";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/OverlayPingPrefab/pingIcon.png");
            tooltip = "Ping the object in the hierarchy";
            clicked += OnClick;
        }

    void OnClick()
        {
        Ping();
        }
    private void Ping()
    {
        if (!Selection.activeObject)
        {
            Debug.Log("Select an object to ping");
            return;
        }

        EditorGUIUtility.PingObject(Selection.activeObject);
    }
}
