namespace BurningKnight.PanelManager
{
#if UNITY_2019_1_OR_NEWER
    public static class CreateNewPanelFromTemplate
    {
        #region Templates

        private const string PANEL_TEMPLATE         = "NewPanelTemplate.cs.txt";
        private const string PANEL_SCREEN_TEMPLATE  = "NewScreenTemplate.cs.txt";
        private const string PANEL_OVERLAY_TEMPLATE = "NewOverlayTemplate.cs.txt";

        private const string PANEL_WITH_MODEL_TEMPLATE         = "NewPanelWithModelTemplate.cs.txt";
        private const string PANEL_WITH_MODEL_SCREEN_TEMPLATE  = "NewScreenWithModelTemplate.cs.txt";
        private const string PANEL_WITH_MODEL_OVERLAY_TEMPLATE = "NewOverlayWithModelTemplate.cs.txt";

        private const string MODEL_TEMPLATE = "NewPanelModelTemplate.cs.txt";

        #endregion

        private const string MENU_ITEM = "Assets/Create/PanelManager/";

        #region Menu Items

        #region Panel without model

        [UnityEditor.MenuItem(MENU_ITEM + "New Panel Script", false, 101)]
        public static void CreatePanelFromTemplate()
        {
            CreateFromTemplate(PANEL_TEMPLATE);
        }

        [UnityEditor.MenuItem(MENU_ITEM + "New Panel Script (Screen)", false, 102)]
        public static void CreatePanelScreenFromTemplate()
        {
            CreateFromTemplate(PANEL_SCREEN_TEMPLATE);
        }

        [UnityEditor.MenuItem(MENU_ITEM + "New Panel Script (Overlay)", false, 103)]
        public static void CreatePanelOverlayFromTemplate()
        {
            CreateFromTemplate(PANEL_OVERLAY_TEMPLATE);
        }

        #endregion

        #region Model

        [UnityEditor.MenuItem(MENU_ITEM + "New Panel Model Script", false, 120)]
        public static void CreatePanelModelFromTemplate()
        {
            CreateFromTemplate(MODEL_TEMPLATE);
        }

        #endregion

        #region Panel with model

        [UnityEditor.MenuItem(MENU_ITEM + "New Panel Script With Model", false, 141)]
        public static void CreatePanelWithModelFromTemplate()
        {
            CreateFromTemplate(PANEL_WITH_MODEL_TEMPLATE);
        }

        [UnityEditor.MenuItem(MENU_ITEM + "New Panel Script (Screen) With Model", false, 142)]
        public static void CreatePanelScreenWithModelFromTemplate()
        {
            CreateFromTemplate(PANEL_WITH_MODEL_SCREEN_TEMPLATE);
        }

        [UnityEditor.MenuItem(MENU_ITEM + "New Panel Script (Overlay) With Model", false, 143)]
        public static void CreatePanelOverlayWithModelFromTemplate()
        {
            CreateFromTemplate(PANEL_WITH_MODEL_OVERLAY_TEMPLATE);
        }

        #endregion

        #endregion

        #region Private API

        private static void CreateFromTemplate(string template)
        {
            var name = System.IO.Path.ChangeExtension(template, null);
            name = name.Replace("Template", string.Empty);
            var path = FindTemplate(template);
            UnityEditor.ProjectWindowUtil.CreateScriptAssetFromTemplateFile(path, name);
        }

        private static string FindTemplate(string name)
        {
            var res = System.IO.Directory.GetFiles(UnityEngine.Application.dataPath, name,
                System.IO.SearchOption.AllDirectories);

            if (res.Length != 1)
            {
                throw new System.ArgumentException($"Script template {name} not found!");
            }

            return res[0];
        }

        #endregion
    }
#endif
}