using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGFanGame.Screens.Menu
{
    internal class NewGameScreen : Screen
    {
        private readonly MenuBackgroundRenderer _backgroundRenderer;
        private readonly Screen _preScreen;

        public NewGameScreen(Screen preScreen, MenuBackgroundRenderer backgroundRenderer)
        {
            _preScreen = preScreen;
            _backgroundRenderer = backgroundRenderer;
        }

        public override void Draw()
        {
            _backgroundRenderer.Draw();
        }

        public override void Update()
        {
            _backgroundRenderer.Update();
        }
    }
}
