using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Project.Scripts.Skin.Configs.Editor
{
    public class TestGGUI : EditorWindow
    {
        private ReorderableList materialsList;
        //private SkinMaterialConfig skinMaterialConfig;
        private List<Material> materials = new List<Material>();
        private bool draggingOverWindow = false;
        private Vector2 scrollPosition = Vector2.zero;

        private bool showMaterials = true;

        [MenuItem("Window/Skin Config Editor Window")]
        public static void ShowWindow()
        {
            // GetWindow<SkinConfigCreator>("Skin Config Creator Window");
        }

        private void OnEnable()
        {
            materialsList = new ReorderableList(materials, typeof(Material), true, true, true, true);

            materialsList.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Materials"); };

            materialsList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                materials[index] = (Material)EditorGUI.ObjectField(rect, materials[index], typeof(Material), false);
            };

            materialsList.onAddCallback = list => { materials.Add(null); };

            materialsList.onRemoveCallback = list => { materials.RemoveAt(list.index); };
        }

        private void OnGUI()
        {
//Горизонтальная группа
            EditorGUILayout.BeginHorizontal();

//Горизонтальная выпадающий список
            showMaterials = EditorGUILayout.Foldout(showMaterials, "Materials");

//Input Field с размерами массива
            EditorGUI.indentLevel++;
            int newCount = EditorGUILayout.IntField("", materials.Count, GUILayout.Width(80));
// реализуем изменение размера списка
            while (newCount < materials.Count)
                materials.RemoveAt(materials.Count - 1);

            while (newCount > materials.Count)
                materials.Add(null);
            EditorGUI.indentLevel--;

            EditorGUILayout.EndHorizontal();

            if (showMaterials)
            {
// Начало области прокрутки
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
                EditorGUILayout.Space(10);
                materialsList.DoLayoutList();
// Конец области прокрутки
                EditorGUILayout.EndScrollView();
            }


            EditorGUILayout.Space(10);

            // skinMaterialConfig = (SkinMaterialConfig)EditorGUILayout.ObjectField("Skin Material Config",
            //     skinMaterialConfig, typeof(SkinMaterialConfig), false);


            if (GUILayout.Button("Set material"))
            {
                PrintMaterialNames();
            }

            HandleDragAndDrop();
        }

        private void HandleDragAndDrop()
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                {
// Если перетаскиваемый объект является материалом.
                    if (DragAndDrop.objectReferences.OfType<Material>().Any())
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                        if (e.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();
                            foreach (Material material in DragAndDrop.objectReferences.OfType<Material>())
                            {
                                materials.Add(material);
                            }

                            GUI.changed = true;
                        }

                        Event.current.Use();
                    }

                    break;
                }
            }
        }

        private void PrintMaterialNames()
        {
            // if (skinMaterialConfig == null)
            // {
            //     Debug.LogError("SkinMaterialConfig is null");
            //     return;
            // }
            //
            // if (materials.Count == 0)
            // {
            //     Debug.LogError("Materials is empty");
            //     return;
            // }
            //
            // List<SkinGuidProvider> skinGuidProviders = new List<SkinGuidProvider>();
            // for (int i = 0; i < materials.Count; i++)
            // {
            //     Material material = materials[i];
            //
            //     if (material != null)
            //     {
            //         skinGuidProviders.Add(new SkinGuidProvider
            //         {
            //             SkinMaterial = material,
            //             SkinGuid = Guid.NewGuid().ToString(),
            //             IsOpened = false
            //         });
            //     }
            // }
            //
            // skinMaterialConfig.SetSkinData(skinGuidProviders);
            //
            // skinMaterialConfig.SetDirty();
            //
            // AssetDatabase.SaveAssetIfDirty(skinMaterialConfig);
            //
            // Debug.Log("Set Materials complete! Count Material: " + materials.Count);
        }
    }
}