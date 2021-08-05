using System.Diagnostics;

namespace MonoGameHerex.src.view
{
    public interface IScreen
    {
        public bool IsActive { get; set; }
        
        public void Update()
        {
            // TODO: Remove if not needed.
        }
        public void Draw()
        {
            
        }
    }
}