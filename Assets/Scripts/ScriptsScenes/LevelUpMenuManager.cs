using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenuManager : MonoBehaviour
{
    private GameObject menuPanel;

    public void AfficherMenu(List<UpgradeOption> options)
    {
        if (menuPanel != null) Destroy(menuPanel);

        Time.timeScale = 0f;
        ConstruireMenu(options);
    }

    private void ConstruireMenu(List<UpgradeOption> options)
    {
        // Overlay sombre plein écran
        menuPanel = new GameObject("LevelUpMenu");
        menuPanel.transform.SetParent(transform, false);

        RectTransform overlayRect = menuPanel.AddComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;

        Image overlayBg = menuPanel.AddComponent<Image>();
        overlayBg.color = new Color(0, 0, 0, 0.7f);

        // Titre "LEVEL UP!"
        GameObject titleObj = new GameObject("Titre");
        titleObj.transform.SetParent(menuPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.2f, 0.82f);
        titleRect.anchorMax = new Vector2(0.8f, 0.95f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;
        TMP_Text titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "LEVEL UP!";
        titleText.fontSize = 48;
        titleText.color = Color.yellow;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontStyle = FontStyles.Bold;

        // Container horizontal pour les cartes
        GameObject container = new GameObject("Container");
        container.transform.SetParent(menuPanel.transform, false);
        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.05f, 0.1f);
        containerRect.anchorMax = new Vector2(0.95f, 0.78f);
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;

        HorizontalLayoutGroup layout = container.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;
        layout.padding = new RectOffset(20, 20, 10, 10);

        for (int i = 0; i < options.Count; i++)
        {
            CreerCarte(container.transform, options[i]);
        }
    }

    private void CreerCarte(Transform parent, UpgradeOption option)
    {
        // Carte (bouton cliquable)
        GameObject card = new GameObject("Carte");
        card.transform.SetParent(parent, false);

        Image cardBg = card.AddComponent<Image>();
        cardBg.color = new Color(0.15f, 0.15f, 0.25f, 0.95f);

        Button btn = card.AddComponent<Button>();
        ColorBlock colors = btn.colors;
        colors.highlightedColor = new Color(0.3f, 0.3f, 0.5f, 1f);
        colors.pressedColor = new Color(0.2f, 0.2f, 0.4f, 1f);
        btn.colors = colors;

        // Layout vertical dans la carte
        VerticalLayoutGroup cardLayout = card.AddComponent<VerticalLayoutGroup>();
        cardLayout.spacing = 8;
        cardLayout.padding = new RectOffset(10, 10, 15, 15);
        cardLayout.childAlignment = TextAnchor.UpperCenter;
        cardLayout.childForceExpandWidth = true;
        cardLayout.childForceExpandHeight = false;

        // Icône de l'arme
        if (option.icone != null)
        {
            GameObject iconObj = new GameObject("Icone");
            iconObj.transform.SetParent(card.transform, false);
            Image iconImg = iconObj.AddComponent<Image>();
            iconImg.sprite = option.icone;
            iconImg.preserveAspect = true;
            LayoutElement iconLayout = iconObj.AddComponent<LayoutElement>();
            iconLayout.preferredHeight = 64;
            iconLayout.preferredWidth = 64;
        }

        // Titre
        GameObject titleObj = new GameObject("Titre");
        titleObj.transform.SetParent(card.transform, false);
        TMP_Text titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = option.titre;
        titleText.fontSize = 22;
        titleText.color = option.estUnlock ? Color.green : Color.white;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontStyle = FontStyles.Bold;
        LayoutElement titleLayout = titleObj.AddComponent<LayoutElement>();
        titleLayout.preferredHeight = 30;

        // Sous-titre
        GameObject subObj = new GameObject("SousTitre");
        subObj.transform.SetParent(card.transform, false);
        TMP_Text subText = subObj.AddComponent<TextMeshProUGUI>();
        subText.text = option.sousTitre;
        subText.fontSize = 16;
        subText.color = new Color(0.9f, 0.85f, 0.3f, 1f);
        subText.alignment = TextAlignmentOptions.Center;
        subText.fontStyle = FontStyles.Italic;
        LayoutElement subLayout = subObj.AddComponent<LayoutElement>();
        subLayout.preferredHeight = 22;

        // Description
        GameObject descObj = new GameObject("Description");
        descObj.transform.SetParent(card.transform, false);
        TMP_Text descText = descObj.AddComponent<TextMeshProUGUI>();
        descText.text = option.description;
        descText.fontSize = 14;
        descText.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        descText.alignment = TextAlignmentOptions.Center;
        LayoutElement descLayout = descObj.AddComponent<LayoutElement>();
        descLayout.preferredHeight = 50;

        // Action au clic
        btn.onClick.AddListener(() =>
        {
            option.appliquer();
            FermerMenu();
        });
    }

    private void FermerMenu()
    {
        if (menuPanel != null) Destroy(menuPanel);
        menuPanel = null;
        Time.timeScale = 1f;

        // Vérifie s'il reste des level ups en attente
        Singleton.Instance.VerifierLevelUp();
    }
}
