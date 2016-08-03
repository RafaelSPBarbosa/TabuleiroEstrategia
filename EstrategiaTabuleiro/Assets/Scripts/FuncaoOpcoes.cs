using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

//Function to change options in the menu

public class FuncaoOpcoes : MonoBehaviour
{

    [Space(20)]
    public Slider BarraVolume;  //Volume bar
    public Toggle CaixaModoJanela;  //Window mode
    public Dropdown Resolucoes;     //Resolution    
    public Button BotaoSalvarPref;  //Save preferences
    [Space(20)]
    private float VOLUME; //:^)
    private int modoJanelaAtivo, resolucaoSalveIndex;
    private bool telaCheiaAtivada;
    private Resolution[] resolucoesSuportadas;

    void Awake()
    {
        if (!Application.isEditor)
        {
            resolucoesSuportadas = Screen.resolutions;
        }
    }

    void Start()
    {
        if (!Application.isEditor)
        {
            ChecarResolucoes();
        }
        //
        Cursor.visible = true;
        Time.timeScale = 1;

        //=============== SAVES===========//
        if (PlayerPrefs.HasKey("VOLUME"))
        {
            VOLUME = PlayerPrefs.GetFloat("VOLUME");
            BarraVolume.value = VOLUME;
        }
        else {
            PlayerPrefs.SetFloat("VOLUME", 0.3f);
            BarraVolume.value = 0.3f;
        }
        //=============MODO JANELA===========//                     //WINDOW MODE
        if (!Application.isEditor)
        {
            if (PlayerPrefs.HasKey("modoJanela"))
            {
                modoJanelaAtivo = PlayerPrefs.GetInt("modoJanela");
                if (modoJanelaAtivo == 1)
                {
                    Screen.fullScreen = false;
                    CaixaModoJanela.isOn = true;
                }
                else {
                    Screen.fullScreen = true;
                    CaixaModoJanela.isOn = false;
                }
            }
            else {
                modoJanelaAtivo = 0;
                PlayerPrefs.SetInt("modoJanela", modoJanelaAtivo);
                CaixaModoJanela.isOn = false;
                Screen.fullScreen = true;
            }

            //========RESOLUCOES========//                           //RESOLUTION
            if (modoJanelaAtivo == 1)
            {
                telaCheiaAtivada = false;
            }
            else {
                telaCheiaAtivada = true;
            }
            if (PlayerPrefs.HasKey("RESOLUCAO"))
            {
                resolucaoSalveIndex = PlayerPrefs.GetInt("RESOLUCAO");
                Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
                Resolucoes.value = resolucaoSalveIndex;
            }
            else {
                resolucaoSalveIndex = (resolucoesSuportadas.Length - 1);
                Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
                PlayerPrefs.SetInt("RESOLUCAO", resolucaoSalveIndex);
                Resolucoes.value = resolucaoSalveIndex;
            }
        }
        // =========SETAR BOTOES==========//
        BotaoSalvarPref.onClick.AddListener(() => SalvarPreferencias());
    }
    //=========VOIDS DE CHECAGEM==========//                    //Check available resolutions
    private void ChecarResolucoes()
    {
        Resolution[] resolucoesSuportadas = Screen.resolutions;
        Resolucoes.options.Clear();
        for (int y = 0; y < resolucoesSuportadas.Length; y++)
        {
            Resolucoes.options.Add(new Dropdown.OptionData() { text = resolucoesSuportadas[y].width + "x" + resolucoesSuportadas[y].height });
        }
        Resolucoes.captionText.text = "Resolution";
    }
    //=========VOIDS DE SALVAMENTO==========//                  //Saves preferences
    private void SalvarPreferencias()
    {
        if (!Application.isEditor)
        {
            if (CaixaModoJanela.isOn == true)
            {
                modoJanelaAtivo = 1;
                telaCheiaAtivada = false;
            }
            else {
                modoJanelaAtivo = 0;
                telaCheiaAtivada = true;
            }
        }
        PlayerPrefs.SetFloat("VOLUME", BarraVolume.value);
        if (!Application.isEditor)
        {
            PlayerPrefs.SetInt("modoJanela", modoJanelaAtivo);
            PlayerPrefs.SetInt("RESOLUCAO", Resolucoes.value);
            resolucaoSalveIndex = Resolucoes.value;
        }
        PlayerPrefs.Save();
        AplicarPreferencias();
    }
    private void AplicarPreferencias()
    {
       // VOLUME = PlayerPrefs.GetFloat("VOLUME");
        GameObject.Find("ConfigManager").GetComponent<MusicManager>().UpdateVolume();
        if (!Application.isEditor)
        {
            Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
        }
    }
}
