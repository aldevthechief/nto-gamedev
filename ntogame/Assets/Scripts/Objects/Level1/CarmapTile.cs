using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Level1
{
    public class CarmapTile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image Image;
        [SerializeField] private bool Block;
        [SerializeField] private bool Red;
        private CarMap Map = null;

        public bool _Block { get { return Block; } set { Block = value; } }

        public void SetInfo(CarMap map, bool red)
        {
            Map = map;
            Red = red;

            if (Red)
            {
                Image.color = new Color(1, 0, 0, 1);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Red)
            {
                return;
            }

            Block = !Block;

            Map.BuildWay();

            if (Block)
            {
                Image.color = new Color(0, 1, 0.5f, 1);
            }
            else
            {
                Image.color = new Color(0, 0, 0, 1);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Red)
            {
                return;
            }
            Image.color = new Color(0, 1, 0.5f, 2) / 2;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Red)
            {
                return;
            }
            if (Block)
            {
                Image.color = new Color(0, 1, 0.5f, 1);
            }
            else
            {
                Image.color = new Color(0, 0, 0, 1);
            }
        }
    }
}
