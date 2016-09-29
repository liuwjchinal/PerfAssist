﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class CoroutineTrackerWindow : EditorWindow
{
    // bound variables
    bool _enableTracking = true;
    Vector2 _scrollPositionLeft;
    Vector2 _scrollPositionRight;

    Panel_CoGraph _graphPanel = new Panel_CoGraph();

    [MenuItem("Window/CoroutineTracker")]
    static void Create()
    {
        CoroutineTrackerWindow w = EditorWindow.GetWindow<CoroutineTrackerWindow>();
        w.Show();

        if (w != null)
        {
            var rect = w.position;
            rect.width = Mathf.Max(1280, rect.width);
            rect.height = Mathf.Max(720, rect.height);
            if (!Mathf.Approximately(rect.width, w.position.width) || 
                !Mathf.Approximately(rect.height, w.position.height))
            {
                w.position = rect;
            }
        }
    }

    void OnEnable()
    {
        EditorApplication.update += Repaint;
    }

    void OnDisable()
    {
        EditorApplication.update -= Repaint;
    }

    void OnGUI()
    {
        if (Event.current.commandName == "AppStarted")
        {
            CoroutineStatisticsV2.Instance.OnBroadcast += CoroutineEditorReceived.Instance.Receive;
        }

        GUILayout.BeginHorizontal();
        _enableTracking = GUILayout.Toggle(_enableTracking, "EnableTracking", GUILayout.Height(GuiConstants.ToobarHeight));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            Rect r = new Rect(0, GuiConstants.ToobarHeight, position.width - GuiConstants.DataTableWidth, position.height - GuiConstants.ToobarHeight);
            GUILayout.BeginArea(r);
            {
                _scrollPositionLeft = GUILayout.BeginScrollView(_scrollPositionLeft, GUIStyle.none, GUI.skin.verticalScrollbar);
                _graphPanel.DrawGraphs(r);
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
        {
            Rect r1 = new Rect(position.width - GuiConstants.DataTableWidth, GuiConstants.ToobarHeight, GuiConstants.DataTableWidth, position.height - GuiConstants.ToobarHeight);
            GUILayout.BeginArea(r1);
            {
                _scrollPositionRight = GUILayout.BeginScrollView(_scrollPositionRight, GUIStyle.none, GUI.skin.verticalScrollbar);
                Panel_CoTable.Instance.DrawTable();
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
        GUILayout.EndHorizontal();
    }
}
