using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;
using Inference;

public class ClueEditor : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset;

    private TextField filePathField;
    private TextField fileNameField;
    private Toggle buildToggle;
    private ScrollView cluesScroll;
    private Button addClueButton;
    private ScrollView recipesScroll;
    private Button addRecipeButton;
    private Button loadButton;
    private Button saveButton;

    private List<Clue> clues = new List<Clue>();
    private List<ClueRecipe> recipes = new List<ClueRecipe>();

    [MenuItem("추리 시스템/단서 편집기")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<ClueEditor>();
        wnd.titleContent = new GUIContent("Clue Editor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var ui = m_VisualTreeAsset.Instantiate();
        root.Add(ui);

        // USS 적용 (선택)
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/ClueEditor.uss");
        if (styleSheet != null) root.styleSheets.Add(styleSheet);

        // UI 연결
        filePathField = root.Q<TextField>("filePathField");
        fileNameField = root.Q<TextField>("fileNameField");
        buildToggle = root.Q<Toggle>("buildToggle");
        cluesScroll = root.Q<ScrollView>("cluesScroll");
        addClueButton = root.Q<Button>("addClueButton");
        recipesScroll = root.Q<ScrollView>("recipesScroll");
        addRecipeButton = root.Q<Button>("addRecipeButton");
        loadButton = root.Q<Button>("loadButton");
        saveButton = root.Q<Button>("saveButton");

        // 버튼 이벤트 연결
        addClueButton.clicked += AddClue;
        addRecipeButton.clicked += AddRecipe;
        loadButton.clicked += LoadJson;
        saveButton.clicked += SaveJson;

        RefreshCluesUI();
        RefreshRecipesUI();
    }

    #region Clues UI

    private void RefreshCluesUI()
    {
        cluesScroll.Clear();
        for (int i = 0; i < clues.Count; i++)
        {
            int index = i; // 캡처
            var clue = clues[i];

            var box = new Box();
            box.style.marginBottom = 5;

            // ID, Name, Image Path
            var idField = new TextField("ID") { value = clue.Id };
            idField.RegisterValueChangedCallback(evt => clue.Id = evt.newValue);
            box.Add(idField);

            var nameField = new TextField("Name") { value = clue.Name };
            nameField.RegisterValueChangedCallback(evt => clue.Name = evt.newValue);
            box.Add(nameField);

            var imgField = new TextField("Image Path") { value = clue.imagePath };
            imgField.RegisterValueChangedCallback(evt => clue.imagePath = evt.newValue);
            box.Add(imgField);

            // Tags
            box.Add(new Label("Tags"));
            for (int t = 0; t < clue.Tags.Length; t++)
            {
                int ti = t;
                var tagField = new TextField() { value = clue.Tags[t] };
                tagField.RegisterValueChangedCallback(evt => clue.Tags[ti] = evt.newValue);
                var h = new VisualElement();
                h.style.flexDirection = FlexDirection.Row;
                h.Add(tagField);
                var delTag = new Button(() => { RemoveClueTag(clue, ti); RefreshCluesUI(); }) { text = "X" };
                h.Add(delTag);
                box.Add(h);
            }
            var addTagBtn = new Button(() => { AddClueTag(clue); RefreshCluesUI(); }) { text = "Add Tag" };
            box.Add(addTagBtn);

            // DescriptionLength
            int descLength = EditorGUILayout.IntField("DescriptionLength", clue.DescriptionLength);
            if (descLength != clue.Description.Length)
            {
                var descArray = clue.Description;
                for (int d = 0; d < descLength; d++)
                {
                    descArray[i] = new ClueDescription(EditorGUILayout.TextField($"Description {i}", descArray[i]?.description ?? "Nothing"), i);
                    clue.Description = descArray;
                }
            }
            for (int d = 0; d < clue.Description.Length; d++)
            {
                int di = d;
                var descField = new TextField($"Description {d}") { value = clue.Description[d].description };
                descField.RegisterValueChangedCallback(evt => clue.Description[di].description = evt.newValue);
                box.Add(descField);
            }

            // LinkedClues
            box.Add(new Label("Linked Clues"));
            for (int l = 0; l < clue.linkedClueId.Length; l++)
            {
                int li = l;
                var linkedField = new TextField() { value = clue.linkedClueId[l] };
                linkedField.RegisterValueChangedCallback(evt => clue.linkedClueId[li] = evt.newValue);
                box.Add(linkedField);
            }

            // Remove Clue
            var removeBtn = new Button(() => { clues.RemoveAt(index); RefreshCluesUI(); }) { text = "Remove Clue" };
            box.Add(removeBtn);

            cluesScroll.Add(box);
        }
    }

    private void AddClue()
    {
        clues.Add(Clue.Instantiate("new_id", "New Clue", new string[0], new string[0], 1));
        RefreshCluesUI();
    }

    private void AddClueTag(Clue clue)
    {
        var list = new List<string>(clue.Tags);
        list.Add("");
        clue.Tags = list.ToArray();
    }

    private void RemoveClueTag(Clue clue, int index)
    {
        var list = new List<string>(clue.Tags);
        list.RemoveAt(index);
        clue.Tags = list.ToArray();
    }

    #endregion

    #region Recipes UI

    private void RefreshRecipesUI()
    {
        recipesScroll.Clear();
        for (int i = 0; i < recipes.Count; i++)
        {
            int index = i;
            var recipe = recipes[i];
            var box = new Box();
            box.style.marginBottom = 5;

            // ID, Name
            var idField = new TextField("ID") { value = recipe.Id };
            idField.RegisterValueChangedCallback(evt => recipe.Id = evt.newValue);
            box.Add(idField);

            var nameField = new TextField("Name") { value = recipe.Name };
            nameField.RegisterValueChangedCallback(evt => recipe.Name = evt.newValue);
            box.Add(nameField);

            // Tags
            box.Add(new Label("Tags"));
            for (int t = 0; t < recipe.Tags.Length; t++)
            {
                int ti = t;
                var tagField = new TextField() { value = recipe.Tags[t] };
                tagField.RegisterValueChangedCallback(evt => recipe.Tags[ti] = evt.newValue);
                var h = new VisualElement();
                h.style.flexDirection = FlexDirection.Row;
                h.Add(tagField);
                var delTag = new Button(() => { RemoveRecipeTag(recipe, ti); RefreshRecipesUI(); }) { text = "X" };
                h.Add(delTag);
                box.Add(h);
            }
            var addTagBtn = new Button(() => { AddRecipeTag(recipe); RefreshRecipesUI(); }) { text = "Add Tag" };
            box.Add(addTagBtn);

            // DescriptionLength
            int descLength = EditorGUILayout.IntField("DescriptionLength", recipe.DescriptionLength);
            if (descLength != recipe.Description.Length)
            {
                var descArray = recipe.Description;
                for (int d = 0; d < descLength; d++)
                {
                    descArray[i] = new ClueDescription(EditorGUILayout.TextField($"Description {i}", descArray[i]?.description ?? "Nothing"), i);
                    recipe.Description = descArray; // 다시 속성에 대입
                }
            }
            for (int d = 0; d < recipe.Description.Length; d++)
            {
                int di = d;
                var descField = new TextField($"Description {d}") { value = recipe.Description[d].description };
                descField.RegisterValueChangedCallback(evt => recipe.Description[di].description = evt.newValue);
                box.Add(descField);
            }

            // ClueIds
            box.Add(new Label("Clue IDs"));
            for (int c = 0; c < recipe.clueIds.Count; c++)
            {
                int ci = c;
                var field = new TextField() { value = recipe.clueIds[c] };
                field.RegisterValueChangedCallback(evt => recipe.clueIds[ci] = evt.newValue);
                var h = new VisualElement();
                h.style.flexDirection = FlexDirection.Row;
                h.Add(field);
                var delBtn = new Button(() => { recipe.clueIds.RemoveAt(ci); RefreshRecipesUI(); }) { text = "X" };
                h.Add(delBtn);
                box.Add(h);
            }
            var addClueIdBtn = new Button(() => { recipe.clueIds.Add(""); RefreshRecipesUI(); }) { text = "Add Clue ID" };
            box.Add(addClueIdBtn);

            // Remove Recipe
            var removeBtn = new Button(() => { recipes.RemoveAt(index); RefreshRecipesUI(); }) { text = "Remove Recipe" };
            box.Add(removeBtn);

            recipesScroll.Add(box);
        }
    }

    private void AddRecipe()
    {
        recipes.Add(ClueRecipe.CreateRecipe("", "", new string[0], new List<string>(), 0));
        RefreshRecipesUI();
    }

    private void AddRecipeTag(ClueRecipe recipe)
    {
        var list = new List<string>(recipe.Tags);
        list.Add("");
        recipe.Tags = list.ToArray();
    }

    private void RemoveRecipeTag(ClueRecipe recipe, int index)
    {
        var list = new List<string>(recipe.Tags);
        list.RemoveAt(index);
        recipe.Tags = list.ToArray();
    }

    #endregion

    #region JSON

    private void LoadJson()
    {
        string directory = Path.Combine(Application.dataPath, filePathField.text);
        string path = Path.Combine(directory, fileNameField.text + ".json");
        if (!File.Exists(path))
        {
            Debug.LogWarning($"File not found: {path}");
            return;
        }
        string json = File.ReadAllText(path);
        DTClues dtClue = JsonUtility.FromJson<DTClues>(json);
        clues = new List<Clue>(dtClue.clues ?? new Clue[0]);
        recipes = new List<ClueRecipe>(dtClue.recipes ?? new ClueRecipe[0]);
        RefreshCluesUI();
        RefreshRecipesUI();
        Debug.Log($"Loaded JSON: {path}");
    }

    private void SaveJson()
    {
        string directory = Path.Combine(Application.dataPath, filePathField.text);
        Directory.CreateDirectory(directory);
        DTClues dtClue = new DTClues(clues.ToArray(), recipes.ToArray());
        string json = JsonUtility.ToJson(dtClue, true);
        string path = Path.Combine(directory, fileNameField.text + ".json");
        File.WriteAllText(path, json);
        Debug.Log($"Saved JSON: {path}");
    }

    #endregion
}

