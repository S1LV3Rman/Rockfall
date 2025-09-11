namespace S1LV3Rman.RockFall
{
    public class DefaultUIPanel : UIPanel
    {
        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}