namespace S1LV3Rman.RockFall
{
    public class DefaultUIPanel : BaseUIPanel
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