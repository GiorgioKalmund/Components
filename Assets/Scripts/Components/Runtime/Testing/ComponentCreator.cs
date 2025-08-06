using System;
using System.Collections;
using System.Threading.Tasks;
using Components.Runtime.Components;
using Components.Runtime.Components.Game;
using Components.Runtime.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Runtime.Testing
{
    public class ComponentCreator : MonoBehaviour
    {
        private ComponentControls _input;
        private WindowComponent window3;
        private ButtonComponent _copyButton;

        private void Start()
        {
            var canvas = GUIService.GetCanvas();
            var canvasT = canvas.GetTransform();
            TextComponent.GlobalFont(Resources.Load<TMPro.TMP_FontAsset>("Font/Main"));
            _input = new ComponentControls();

            Vector2 nativeScaleSize = new Vector2(5, 5);

            var backdrop = ComponentBuilder.N<ImageComponent>("Brr", canvasT)
                    .FullScreen(GUIService.GetCanvas())
                    .Sprite("player", "Tile")
                    .ImageType(Image.Type.Tiled)
                    .PixelsPerUnitMultiplier(0.1f)
                    .ToggleVisibilityUsing(_input.UI.ShowWindow)
                ;

            // Health and Sprint
            var healthBarEmpty = ComponentBuilder.N<ImageComponent>(canvasT)
                    .Sprite("player", "Empty Bar")
                    .NativeSize(nativeScaleSize)
                    .Pivot(PivotPosition.LowerLeft, true)
                    .Offset(100, 100)
                ;
            var healthBar = healthBarEmpty.Copy(false).Parent(healthBarEmpty).Sprite("player", "Health Bar").ImageTypeFilled(Image.FillMethod.Horizontal);
            
            var sprintBarEmpty = ComponentBuilder.N<ImageComponent>(healthBarEmpty)
                    .Sprite("player", "Power Bar Empty")
                    .NativeSize(nativeScaleSize)
                    .Pivot(PivotPosition.UpperLeft)
                    .AnchoredTo(PivotPosition.LowerLeft)
                    .Offset(0, -5)
                ;
            var sprintBar = sprintBarEmpty.Copy(false).Parent(sprintBarEmpty).Sprite("player", "Power Bar").ImageTypeFilled(Image.FillMethod.Horizontal, 0.66f);
            
            var airBar = healthBar.Copy().Sprite("player", "Air Bar").ImageTypeFilled(Image.FillMethod.Horizontal, 0.3f);
            
            var healthBarHints = healthBar.Copy(false).Sprite("player", "Health Bar Hints").ImageTypeFilled(Image.FillMethod.Horizontal);
            
            var crosshair = ComponentBuilder.N<ImageComponent>(canvasT)
                .Sprite("player", "Crosshair")
                .Size(50, 50)
                ;
            
            var hotbar = ComponentBuilder.N<ImageComponent>(canvasT)
                    .ImageType(Image.Type.Sliced)
                    .PixelsPerUnitMultiplier(0.3f)
                    .Pivot(PivotPosition.LowerCenter, true)
                    .Offset(0, 50)
                    .Size(800, 100)
                    .Sprite("Slice/Circle Gray")
                    .Alpha(0.9f)
                ;

            StartCoroutine(SpriteSwap(hotbar));
        }

        private IEnumerator SpriteSwap(ImageComponent c)
        {
            yield return new WaitForEndOfFrame();
        }

        private void OnEnable()
        {
            _input?.Enable();
        }

        private void OnDisable()
        {
            _input?.Disable();
        }
    }
}