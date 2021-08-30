using System.Collections.Generic;
using MonoGameHerex.src.view;

namespace MonoGameHerex.Handlers
{
    public class SwitchScreenHelper
    {
        private List<IScreen> _views;
        public SwitchScreenHelper(List<IScreen> views)
        {
            _views = views;
        }
        
        // Id list :
        //      - 0 : Game Screen
        //      - 1 : Main Menu
        public void SetView(int id)
        {
            ResetViews();
            _views[id].IsActive = true;
        }

        private void ResetViews()
        {
            foreach (var v in _views)
            {
                v.IsActive = false;
            }
        }
    }
}