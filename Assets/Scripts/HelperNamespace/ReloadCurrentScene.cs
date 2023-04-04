using UnityEngine.SceneManagement;

namespace Helper
{
    public static class ReloadCurrentScene
    {
        public static void Reload() 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}