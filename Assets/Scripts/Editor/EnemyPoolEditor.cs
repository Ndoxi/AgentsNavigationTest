using AgentsTest.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AgentsTest.Editors
{
    [CustomEditor(typeof(EnemyPool))]
    public class EnemyPoolEditor : Editor
    {
        private SerializedProperty _enemyPrefab;
        private SerializedProperty _container;
        private SerializedProperty _initialSize;
        private SerializedProperty _enemies;

        void OnEnable()
        {
            _enemyPrefab = serializedObject.FindProperty("_enemyPrefab");
            _container = serializedObject.FindProperty("_container");
            _initialSize = serializedObject.FindProperty("_initialSize");
            _enemies = serializedObject.FindProperty("_enemies");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_enemyPrefab);
            EditorGUILayout.PropertyField(_container);
            EditorGUILayout.PropertyField(_initialSize);

            GUILayout.Space(5);
            if (GUILayout.Button("POPULATE"))
            {
                Populate();
            }
            if (GUILayout.Button("CLEAR"))
            {
                Clear();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void Populate()
        {
            Clear();
            for (int i = 0; i < _initialSize.intValue; i++)
            {
                Unit newEnemy = (Unit)Instantiate(_enemyPrefab.objectReferenceValue, (Transform)_container.objectReferenceValue);
                _enemies.InsertArrayElementAtIndex(i);
                _enemies.GetArrayElementAtIndex(i).objectReferenceValue = newEnemy;
            }
        }

        private void Clear()
        {
            for (int i = _enemies.arraySize - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                    Destroy(_enemies.GetArrayElementAtIndex(i).objectReferenceValue);
                else
                    DestroyImmediate(_enemies.GetArrayElementAtIndex(i).objectReferenceValue);
            }
            _enemies.ClearArray();
        }
    }
}