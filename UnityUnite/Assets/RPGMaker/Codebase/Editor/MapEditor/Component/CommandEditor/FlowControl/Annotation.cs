using RPGMaker.Codebase.CoreSystem.Helper;
using RPGMaker.Codebase.CoreSystem.Knowledge.DataModel.Event;
using RPGMaker.Codebase.Editor.Common;
using RPGMaker.Codebase.Editor.Common.View;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace RPGMaker.Codebase.Editor.MapEditor.Component.CommandEditor.FlowControl
{
    public class Annotation : AbstractCommandEditor
    {
        private const string SettingUxml =
            "Assets/RPGMaker/Codebase/Editor/MapEditor/Asset/Uxml/EventCommand/inspector_mapEvent_annotation.uxml";


        public Annotation(
            VisualElement rootElement,
            List<EventDataModel> eventDataModels,
            int eventIndex,
            int eventCommandIndex
        )
            : base(rootElement, eventDataModels, eventIndex, eventCommandIndex) {
        }

        public override void Invoke() {
            var commandTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(SettingUxml);
            VisualElement commandFromUxml = commandTree.CloneTree();
            EditorLocalize.LocalizeElements(commandFromUxml);
            commandFromUxml.style.flexGrow = 1;
            RootElement.Add(commandFromUxml);

            UnityEditorWrapper.AssetDatabaseWrapper.Refresh();
            if (EventDataModels[EventIndex].eventCommands[EventCommandIndex].parameters.Count == 0)
            {
                EventDataModels[EventIndex].eventCommands[EventCommandIndex].parameters.Add("");
                EventManagementService.SaveEvent(EventDataModels[EventIndex]);
                UnityEditorWrapper.AssetDatabaseWrapper.Refresh();
            }

            ImTextField annotationText =
                RootElement.Q<VisualElement>("command_annotation").Query<ImTextField>("annotationText");
            annotationText.value =
                EventDataModels[EventIndex].eventCommands[EventCommandIndex].parameters[0];
            annotationText.RegisterCallback<FocusOutEvent>(evt =>
            {
                EventDataModels[EventIndex].eventCommands[EventCommandIndex].parameters[0] =
                    annotationText.value;
                Save(EventDataModels[EventIndex]);
            });
        }
    }
}