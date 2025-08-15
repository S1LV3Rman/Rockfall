using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class NameIndicator : UIBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel;

        public void SetName(string name)
        {
            _nameLabel.SetText(name);
        }

        public void SetColor(Color color) => _nameLabel.color = color;
    }
}