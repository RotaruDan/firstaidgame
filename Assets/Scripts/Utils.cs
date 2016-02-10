using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ReiniciarFlags()
    {
        Flags.Reininciar();
    }

    public void ActivateFlag(string flag)
    {
        Flags.Agregar(flag, true);
    }

    public void DeactivateFlag(string flag)
    {
        Flags.Agregar(flag, false);
    }
}
