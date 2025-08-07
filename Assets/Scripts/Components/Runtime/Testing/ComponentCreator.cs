using System.Collections.Generic;
using Components.Runtime.Components;
using Components.Runtime.Components.Animation;
using Components.Runtime.Components.Game;
using Components.Runtime.Input;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

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
            TextComponent.GlobalFont(Resources.Load<TMPro.TMP_FontAsset>("Font/GoogleSansCode SDF"));
            _input = new ComponentControls();

            Vector2 nativeScaleSize = new Vector2(5, 5);

            var backdrop = ComponentBuilder.N<ImageComponent>("Brr", canvasT)
                    .FullScreen(GUIService.GetCanvas())
                    .Sprite("player", "Tile")
                    .ImageType(Image.Type.Tiled)
                    .PixelsPerUnitMultiplier(0.1f)
                    .ToggleVisibilityUsing(_input.UI.ShowWindow)
                    .SetActive(false)
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
                .SetActive(false)
                ;
            
            var hotbar = ComponentBuilder.N<Hotbar>(canvasT)
                    .ImageType(Image.Type.Sliced)
                    .PixelsPerUnitMultiplier(0.3f)
                    .Pivot(PivotPosition.LowerCenter, true)
                    .Offset(100, 50)
                    .Size(1050, 150)
                    .Sprite("Slice/Circle Gray")
                    .Alpha(0.9f)
                    .AddHorizontalLayout(10)
                    .Cast<Hotbar>()
                ;

            var slot0 = ComponentBuilder.N<HotbarSlot>().Sprite("player", "Inventory Slot").Cast<HotbarSlot>();
            var slot1 = slot0.Copy();
            var slot2 = slot0.Copy();
            var slot3 = slot0.Copy();
            var slot4 = slot0.Copy();
            var slot5 = slot0.Copy();
            var slot6 = slot0.Copy();
            var slot7 = slot0.Copy();
            var slot8 = slot0.Copy();

            hotbar.AddSlots(slot0, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8);
            
            // Debug
            var debugWindow = ComponentBuilder.N<WindowComponent>("Debug", canvasT)
                    .Build(_input.UI.Debug, "Debug")
                    .Size(400, 500)
                    .ContentPadding(5)
                ;
            debugWindow.ConfigureContent()
                .Create(ScrollViewDirection.Vertical, ScrollRect.MovementType.Clamped, false)
                .AddVerticalLayout(10, TextAnchor.UpperLeft)
                .ContentPadding(PaddingSide.All, 10)
                .SizeContent(400, 800)
                .ScrollToTop()
                ;

            var removeSlots = ComponentBuilder.N<ButtonComponent>("Remove Slots", canvasT)
                    .Create("Remove Slots", () => hotbar.RemoveSlot(0), ImageService.GetSpriteFromAsset("player", "rock"))
                    .FitToContents()
                    .ContentPadding(PaddingSide.All, 10)
                    .Sprite("Slice/Circle Orange", Image.Type.Sliced)
                    .Cast<ButtonComponent>()
                ;
            removeSlots.ForegroundSize(40, 40).ContentSpacing(10).GetTextComponent().AutoSize(maxSize:32).Bold();

            var addSlots = removeSlots.Copy().Text("Add Slots").ClearAllFunctions().Function(() => hotbar.AddNewSlot(ComponentBuilder.N<HotbarSlot>().Sprite("player", "Inventory Slot").Cast<HotbarSlot>()));
            var refocus = removeSlots.Copy().Text("Refocus").ClearAllFunctions().Function(IFocusable.FocusLastFocused);
            refocus.GetForeground().Sprite("player", "paddels");
            var unfocus = removeSlots.Copy().Parent(canvasT).Text("Unfocus").ClearAllFunctions().Function(() => IFocusable.FocusedElement.UnFocus());
            var currentlyFocused = removeSlots.Copy().Parent(canvasT).Text("Current Focus").ClearAllFunctions()
                .Function(() => Debug.Log(IFocusable.FocusedElement));
            debugWindow.AddContent(removeSlots, addSlots, refocus, unfocus, currentlyFocused);
            
            var image = ComponentBuilder.N<ImageComponent>("animation test", canvasT)
                    .Sprite("player", "eyes")
                ;
            Sprite[] frames = new Sprite[4];
            frames[0] = ImageService.GetSpriteFromAsset("player", "head");
            frames[1] = ImageService.GetSpriteFromAsset("player", "rock");
            frames[2] = ImageService.GetSpriteFromAsset("player", "paddels");
            frames[3] = ImageService.GetSpriteFromAsset("player", "backpack");
            SpriteAnimation animation = new SpriteAnimation(frames, 1);
            var animator = image.AddAnimator();
            animator.CreateAnimation(animation, SpriteAnimator.Type.PingPong).UseNativeSizing(4, 4);
            //animator.Play();

            var playAnimation = removeSlots.Copy().Text("Play Animation").ClearAllFunctions()
                .Function(animator.Play).Foreground(null);
            var pauseAnimation = removeSlots.Copy().Text("Pause Animation").ClearAllFunctions()
                .Function(animator.Pause).Foreground(null);
            var resetAnimation = removeSlots.Copy().Text("Reset Animation").ClearAllFunctions()
                .Function(animator.ResetAnimation).Foreground(null);
            var restartAnimation = removeSlots.Copy().Text("Restart Animation").ClearAllFunctions()
                .Function(animator.RestartAnimation).Foreground(null);
            var toggleType = removeSlots.Copy().Text("Toggle Type").ClearAllFunctions()
                .Function(() => ToggleAnimationType(animator)).Foreground(ImageService.GetSpriteFromAsset("player", "Walkie Talkie"));
            debugWindow.AddContent(playAnimation, pauseAnimation, resetAnimation, restartAnimation, toggleType);
        }

        private void ToggleAnimationType(SpriteAnimator animator)
        {
            if (animator.AnimationType == SpriteAnimator.Type.Once)
                animator.Configure(SpriteAnimator.Type.Loop, speed:3);
            else 
                animator.Configure(SpriteAnimator.Type.Once, speed:1);
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