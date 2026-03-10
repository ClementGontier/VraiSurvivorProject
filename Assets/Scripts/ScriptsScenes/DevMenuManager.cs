using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevMenuManager : MonoBehaviour
{
    private GameObject menuPanel;
    private bool isVisible = false;

    private Transform weaponsContainer;
    private TMP_Text monsterLabel;
    private TMP_Text quantityLabel;
    private TMP_Text spawnPauseBtnText;

    private int monsterIndex = 0;
    private int spawnQuantity = 1;

    private static readonly string[] allMonsters =
    {
        "enemyPrefab0", "enemyPrefab1", "enemyPrefab2",
        "enemyPrefab3", "enemyPrefab4", "enemyPrefab5",
        "enemyPrefab6", "enemyPrefab7", "enemyPrefab8",
        "miniBossPrefab", "bossPrefab"
    };

    void Start()
    {
        BuildMenu();
    }

    void Update()
    {
        // Touche ² sur clavier AZERTY français (= BackQuote sur QWERTY)
        if (Input.GetKeyDown(KeyCode.BackQuote))
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "MainMenu" || scene == "Victoire") return;

        isVisible = !isVisible;
        menuPanel.SetActive(isVisible);

        if (isVisible)
            RefreshWeapons();
    }

    // ── Construction du menu ──────────────────────────────────────────────────

    private void BuildMenu()
    {
        menuPanel = new GameObject("DevMenuPanel");
        menuPanel.transform.SetParent(transform, false);

        RectTransform panelRT = menuPanel.AddComponent<RectTransform>();
        panelRT.anchorMin        = new Vector2(0f, 1f);
        panelRT.anchorMax        = new Vector2(0f, 1f);
        panelRT.pivot            = new Vector2(0f, 1f);
        panelRT.anchoredPosition = new Vector2(5f, -32f);
        panelRT.sizeDelta        = new Vector2(230f, 300f);

        menuPanel.AddComponent<Image>().color = new Color(0.05f, 0.05f, 0.12f, 0.88f);

        // ScrollRect principal (tout le contenu est scrollable si besoin)
        Transform content = BuildMainScroll(menuPanel.transform);

        // ── Titre
        AddLabel(content, "[ DEV MENU  ² ]", 13f, Color.cyan, FontStyles.Bold);
        AddSeparator(content);

        // ── ARMES
        AddLabel(content, "— ARMES —", 11f, Color.yellow, FontStyles.Bold);
        weaponsContainer = BuildScrollArea(content, 70f);
        AddSeparator(content);

        // ── NIVEAU : bouton pleine largeur directement dans le content
        AddLabel(content, "— NIVEAU —", 11f, Color.yellow, FontStyles.Bold);
        MakeButton(content, "+1 Niveau", () =>
        {
            Singleton s = Singleton.Instance;
            if (s != null) s.AddXP(s.expToNextLevel - s.playerXP);
        }, new Color(0.15f, 0.45f, 0.15f), 22f);
        Transform levelNavRow = MakeRow(content, 22f, expandWidth: true);
        MakeButton(levelNavRow, "< Precedent", () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
        }, new Color(0.2f, 0.2f, 0.5f), 22f);
        MakeButton(levelNavRow, "Suivant >", () =>
        {
            ManageScenes.instance.NextLevel();
        }, new Color(0.2f, 0.2f, 0.5f), 22f);
        AddSeparator(content);

        // ── SPAWN
        AddLabel(content, "— SPAWN —", 11f, Color.yellow, FontStyles.Bold);

        // Sélecteur de monstre
        Transform monsterRow = MakeRow(content, 22f);
        MakeButton(monsterRow, "<", () =>
        {
            monsterIndex = (monsterIndex - 1 + allMonsters.Length) % allMonsters.Length;
            monsterLabel.text = allMonsters[monsterIndex];
        }, new Color(0.25f, 0.25f, 0.25f), 22f, fixedWidth: 22f);
        monsterLabel = AddLabel(monsterRow, allMonsters[0], 9f, Color.white, FontStyles.Normal, flexWidth: 1f);
        MakeButton(monsterRow, ">", () =>
        {
            monsterIndex = (monsterIndex + 1) % allMonsters.Length;
            monsterLabel.text = allMonsters[monsterIndex];
        }, new Color(0.25f, 0.25f, 0.25f), 22f, fixedWidth: 22f);

        // Quantité
        Transform qtyRow = MakeRow(content, 22f);
        AddLabel(qtyRow, "Qté:", 10f, Color.white, FontStyles.Normal, fixedWidth: 30f);
        MakeButton(qtyRow, "-", () =>
        {
            if (spawnQuantity > 1) spawnQuantity--;
            quantityLabel.text = spawnQuantity.ToString();
        }, new Color(0.25f, 0.25f, 0.25f), 22f, fixedWidth: 22f);
        quantityLabel = AddLabel(qtyRow, "1", 12f, Color.white, FontStyles.Bold, fixedWidth: 28f);
        MakeButton(qtyRow, "+", () =>
        {
            spawnQuantity++;
            quantityLabel.text = spawnQuantity.ToString();
        }, new Color(0.25f, 0.25f, 0.25f), 22f, fixedWidth: 22f);

        // Spawn + Pause sur la même ligne, chacun prend la moitié
        Transform spawnRow = MakeRow(content, 24f, expandWidth: true);
        MakeButton(spawnRow, "SPAWN", () =>
        {
            spawnEnnemies sp = FindFirstObjectByType<spawnEnnemies>();
            if (sp != null)
                for (int i = 0; i < spawnQuantity; i++)
                    sp.SpawnEnemy(allMonsters[monsterIndex]);
        }, new Color(0.55f, 0.15f, 0.05f), 24f);

        GameObject pauseGO = MakeButton(spawnRow, "Pause Spawn", () =>
        {
            spawnEnnemies sp = FindFirstObjectByType<spawnEnnemies>();
            if (sp != null)
            {
                sp.spawnPaused = !sp.spawnPaused;
                spawnPauseBtnText.text = sp.spawnPaused ? "Reprendre" : "Pause Spawn";
            }
        }, new Color(0.15f, 0.15f, 0.45f), 24f);
        spawnPauseBtnText = pauseGO.GetComponentInChildren<TMP_Text>();

        menuPanel.SetActive(false);
    }

    // ── ScrollRect principal ──────────────────────────────────────────────────

    private Transform BuildMainScroll(Transform parent)
    {
        GameObject scrollGO = new GameObject("MainScroll");
        scrollGO.transform.SetParent(parent, false);

        RectTransform rt = scrollGO.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(2f, 2f);
        rt.offsetMax = new Vector2(-2f, -2f);

        ScrollRect sr = scrollGO.AddComponent<ScrollRect>();
        sr.horizontal = false;

        // Viewport
        GameObject vp = new GameObject("Viewport");
        vp.transform.SetParent(scrollGO.transform, false);
        RectTransform vpRT = vp.AddComponent<RectTransform>();
        vpRT.anchorMin = Vector2.zero;
        vpRT.anchorMax = Vector2.one;
        vpRT.offsetMin = Vector2.zero;
        vpRT.offsetMax = Vector2.zero;
        vp.AddComponent<Mask>().showMaskGraphic = false;
        vp.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.01f);

        // Content
        GameObject content = new GameObject("Content");
        content.transform.SetParent(vp.transform, false);
        RectTransform cRT = content.AddComponent<RectTransform>();
        cRT.anchorMin = new Vector2(0f, 1f);
        cRT.anchorMax = new Vector2(1f, 1f);
        cRT.pivot     = new Vector2(0.5f, 1f);
        cRT.offsetMin = Vector2.zero;
        cRT.offsetMax = Vector2.zero;

        VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
        vlg.spacing              = 3f;
        vlg.padding              = new RectOffset(5, 5, 5, 5);
        vlg.childControlWidth    = true;
        vlg.childControlHeight   = true;   // <-- donne réellement leur hauteur aux enfants
        vlg.childForceExpandWidth  = true;
        vlg.childForceExpandHeight = false;

        content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        sr.content  = cRT;
        sr.viewport = vpRT;

        return content.transform;
    }

    // ── ScrollRect pour la liste des armes ───────────────────────────────────

    private Transform BuildScrollArea(Transform parent, float height)
    {
        GameObject scrollGO = new GameObject("WeaponsScroll");
        scrollGO.transform.SetParent(parent, false);

        LayoutElement le = scrollGO.AddComponent<LayoutElement>();
        le.preferredHeight = height;
        le.minHeight       = height;

        scrollGO.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.2f);

        ScrollRect sr = scrollGO.AddComponent<ScrollRect>();
        sr.horizontal = false;

        // Viewport
        GameObject vp = new GameObject("Viewport");
        vp.transform.SetParent(scrollGO.transform, false);
        RectTransform vpRT = vp.AddComponent<RectTransform>();
        vpRT.anchorMin = Vector2.zero;
        vpRT.anchorMax = Vector2.one;
        vpRT.offsetMin = Vector2.zero;
        vpRT.offsetMax = Vector2.zero;
        vp.AddComponent<Mask>().showMaskGraphic = false;
        vp.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.01f);

        // Content
        GameObject content = new GameObject("Content");
        content.transform.SetParent(vp.transform, false);
        RectTransform cRT = content.AddComponent<RectTransform>();
        cRT.anchorMin = new Vector2(0f, 1f);
        cRT.anchorMax = new Vector2(1f, 1f);
        cRT.pivot     = new Vector2(0.5f, 1f);
        cRT.offsetMin = Vector2.zero;
        cRT.offsetMax = Vector2.zero;

        VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
        vlg.spacing              = 2f;
        vlg.childControlWidth    = true;
        vlg.childControlHeight   = true;   // <-- idem
        vlg.childForceExpandWidth  = true;
        vlg.childForceExpandHeight = false;

        content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        sr.content  = cRT;
        sr.viewport = vpRT;

        return content.transform;
    }

    // ── Rafraîchit la liste des armes ────────────────────────────────────────

    public void RefreshWeapons()
    {
        if (weaponsContainer == null) return;

        for (int i = weaponsContainer.childCount - 1; i >= 0; i--)
            Destroy(weaponsContainer.GetChild(i).gameObject);

        Singleton s = Singleton.Instance;
        if (s == null || s.wm == null) return;

        foreach (IWeapon w in new List<IWeapon>(s.wm.activeWeapons))
        {
            IWeapon cap = w;
            MakeButton(weaponsContainer, "[-]  " + cap.GetName(), () =>
            {
                s.wm.eneleverArme(cap);
                RefreshWeapons();
            }, new Color(0.45f, 0.08f, 0.08f), 20f);
        }

        foreach (IWeapon w in new List<IWeapon>(s.wm.lockedWeapons))
        {
            IWeapon cap = w;
            MakeButton(weaponsContainer, "[+]  " + cap.GetName(), () =>
            {
                s.wm.ajoutArme(cap);
                RefreshWeapons();
            }, new Color(0.08f, 0.35f, 0.08f), 20f);
        }
    }

    // ── Helpers UI ───────────────────────────────────────────────────────────

    // Crée une ligne horizontale
    private Transform MakeRow(Transform parent, float height, bool expandWidth = false)
    {
        GameObject row = new GameObject("Row");
        row.transform.SetParent(parent, false);

        HorizontalLayoutGroup hl = row.AddComponent<HorizontalLayoutGroup>();
        hl.spacing               = 3f;
        hl.childControlWidth     = true;
        hl.childControlHeight    = true;
        hl.childForceExpandWidth  = expandWidth;
        hl.childForceExpandHeight = true; // enfants remplissent la hauteur de la ligne

        LayoutElement le = row.AddComponent<LayoutElement>();
        le.preferredHeight = height;
        le.minHeight       = height;

        return row.transform;
    }

    // Crée un label TMP
    private TMP_Text AddLabel(Transform parent, string text, float fontSize, Color color,
                              FontStyles style, float fixedWidth = -1f, float flexWidth = -1f)
    {
        GameObject obj = new GameObject("Lbl");
        obj.transform.SetParent(parent, false);

        TMP_Text tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text               = text;
        tmp.fontSize           = fontSize;
        tmp.color              = color;
        tmp.fontStyle          = style;
        tmp.alignment          = TextAlignmentOptions.Center;
        tmp.overflowMode       = TextOverflowModes.Overflow;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;

        LayoutElement le = obj.AddComponent<LayoutElement>();
        le.preferredHeight = 16f;
        le.minHeight       = 16f;
        if (fixedWidth > 0f) { le.preferredWidth = fixedWidth; le.minWidth = fixedWidth; }
        if (flexWidth  > 0f) le.flexibleWidth = flexWidth;

        return tmp;
    }

    // Crée une ligne séparatrice
    private void AddSeparator(Transform parent)
    {
        GameObject sep = new GameObject("Sep");
        sep.transform.SetParent(parent, false);
        sep.AddComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.3f);
        LayoutElement le = sep.AddComponent<LayoutElement>();
        le.preferredHeight = 1f;
        le.minHeight       = 1f;
    }

    // Crée un bouton, retourne le GameObject pour accéder au TMP_Text si besoin
    private GameObject MakeButton(Transform parent, string label, System.Action onClick,
                                  Color bg, float height, float fixedWidth = -1f)
    {
        GameObject go = new GameObject("Btn");
        go.transform.SetParent(parent, false);

        go.AddComponent<Image>().color = bg;

        Button btn = go.AddComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.highlightedColor = bg * 1.5f;
        cb.pressedColor     = bg * 0.6f;
        btn.colors = cb;

        LayoutElement le = go.AddComponent<LayoutElement>();
        le.preferredHeight = height;
        le.minHeight       = height;
        if (fixedWidth > 0f) { le.preferredWidth = fixedWidth; le.minWidth = fixedWidth; }

        // Texte qui remplit le bouton
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(go.transform, false);

        TMP_Text tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text               = label;
        tmp.fontSize           = 10f;
        tmp.color              = Color.white;
        tmp.alignment          = TextAlignmentOptions.Center;
        tmp.overflowMode       = TextOverflowModes.Overflow;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;

        RectTransform trt = textGO.GetComponent<RectTransform>();
        trt.anchorMin = Vector2.zero;
        trt.anchorMax = Vector2.one;
        trt.offsetMin = Vector2.zero;
        trt.offsetMax = Vector2.zero;

        btn.onClick.AddListener(() => onClick());
        return go;
    }
}
