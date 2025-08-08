using System.Collections.Generic;
using Components.Runtime.Components;
using Components.Runtime.Components.Animation;
using Components.Runtime.Components.Game;
using Components.Runtime.Input;
using Components.Runtime.Service;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

namespace Components.Runtime.Testing
{
    public class ComponentDemo : MonoBehaviour
    {
        private ComponentControls Input => InputService.Input;
        private WindowComponent window3;
        private ButtonComponent _copyButton;
        
        // Service
        private InputService _inputService;

        private void Start()
        {
            _inputService = new InputService();
            var canvas = GUIService.GetCanvas();
            var canvasT = canvas.GetTransform();
            TextComponent.GlobalFont(Resources.Load<TMPro.TMP_FontAsset>("Font/GoogleSansCode SDF"));
            Vector2 nativeScaleSize = new Vector2(5, 5);

            var backdrop = ComponentBuilder.N<ImageComponent>("Brr", canvasT)
                    .FullScreen(GUIService.GetCanvas())
                    .Sprite("player", "Tile")
                    .ImageType(Image.Type.Tiled)
                    .PixelsPerUnitMultiplier(0.1f)
                    .ToggleVisibilityUsing(Input.UI.ShowWindow)
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
                .Size(30, 30)
                .SetActive()
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
            hotbar.SetFocusGroup(1);

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
                    .Build(Input.UI.Debug, "Debug")
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
            
            var debugWindow2 = ComponentBuilder.N<WindowComponent>("Debug", canvasT)
                    .Build(Input.UI.Debug, "Debug 2")
                    .Size(400, 300)
                    .ContentPadding(5)
                    .Offset(0, -510)
                ;
            debugWindow2.ConfigureContent()
                .Create(ScrollViewDirection.Vertical, ScrollRect.MovementType.Clamped, false)
                .AddVerticalLayout(10, TextAnchor.UpperLeft)
                .ContentPadding(PaddingSide.All, 10)
                .SizeContent(400, 300)
                .ScrollToTop()
                ;

            var removeSlots = ComponentBuilder.N<ButtonComponent>("Remove Slots", canvasT)
                    .Create("Remove Slots", () => hotbar.RemoveSlot(0), ImageService.GetSpriteFromAsset("player", "rock"))
                    .FitToContents()
                    .ContentPadding(PaddingSide.All, 10)
                    .Sprite("Slice/Circle Orange", Image.Type.Sliced)
                    .Cast<ButtonComponent>()
                ;
            removeSlots.ForegroundSize(40, 40).ContentSpacing(10).GetTextComponent().AutoSize(maxSize:26).Bold();

            var addSlots = removeSlots.Copy().Text("Add Slots").ClearAllFunctions().Function(() => hotbar.AddNewSlot(ComponentBuilder.N<HotbarSlot>().Sprite("player", "Inventory Slot").Cast<HotbarSlot>()));
            var refocus = removeSlots.Copy().Text("Refocus").ClearAllFunctions().Function(IFocusable.FocusLastFocused);
            refocus.GetForeground().Sprite("player", "paddels");
            var unfocus = removeSlots.Copy().Parent(canvasT).Text("Unfocus [0]").ClearAllFunctions().Function(() => IFocusable.FocusGroup(0));
            var currentlyFocused = removeSlots.Copy().Parent(canvasT).Text("Current Focus [0]").ClearAllFunctions()
                .Function(() => Debug.Log(IFocusable.GetFocusedElement(0)));
            debugWindow.AddContent(removeSlots, addSlots, refocus, unfocus, currentlyFocused);
            
            var image = ComponentBuilder.N<ImageComponent>("animation test", canvasT).Offset(-100, 200);
            Sprite[] frames = new Sprite[4];
            frames[0] = ImageService.GetSpriteFromAsset("player", "head");
            frames[1] = ImageService.GetSpriteFromAsset("player", "rock");
            frames[2] = ImageService.GetSpriteFromAsset("player", "paddels");
            frames[3] = ImageService.GetSpriteFromAsset("player", "backpack");
            SpriteAnimation animation = new SpriteAnimation(frames, 2);
            var animator = image.AddAnimator();
            animator.CreateAnimation(animation, SpriteAnimator.Type.PingPong).NativeSizing(4, 4);
            animator.Play();
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
            var freezeInput = removeSlots.Copy().Text("Toggle Freeze").ClearAllFunctions()
                .Function(hotbar.ToggleFreeze).Foreground(null);
            debugWindow.AddContent(freezeInput,playAnimation, pauseAnimation, resetAnimation, restartAnimation, toggleType);

            var wheelMenu = ComponentBuilder.N<WheelMenu>("Wheel Menu", canvasT)
                .Radius(200);
            for (int index = 0; index < 8; index++)
            {
                var mockMenuButton = ComponentBuilder.N<WheelMenuButton>()
                        .ConfigureSprites(ImageService.GetSpriteFromAsset("player", "Menu Wheel Slot"), ImageService.GetSpriteFromAsset("player", "Menu Wheel Slot Selected"))
                        .Size(200, 200)
                        .Create(foreground:ImageService.GetSpriteFromAsset("player", "rock"))
                        .Cast<WheelMenuButton>()
                    ;
                mockMenuButton.SetFocusGroup(8);
                mockMenuButton.GetTextComponent().Color(Color.white).Bold();
                var wheelAnimator = mockMenuButton.GetForeground().AddAnimator();
                Sprite[] rockAnimFrames = new[] { ImageService.GetSpriteFromAsset("player", "rock"), ImageService.GetSpriteFromAsset("player", "rock_anim")};
                SpriteAnimation rockAnim = new SpriteAnimation(rockAnimFrames, new float[] { 0.5f, 3 });
                wheelAnimator.CreateAnimation(rockAnim, SpriteAnimator.Type.Loop).NativeSizing(4, 4);
                wheelAnimator.Play();
                wheelMenu.AddContent(mockMenuButton);
            }

            var popup = ComponentBuilder.N<PopupComponent>("popup", canvasT)
                    .Build(canvas, Input.UI.Escape)
                    .AddContent(wheelMenu)
                ;
            popup.onPopupOpen.AddListener(() => hotbar.Freeze());
            popup.onPopupClose.AddListener(() => hotbar.UnFreeze());
            
            var popupControls = removeSlots.Copy().Text("Popup").ClearAllFunctions()
                .Function(popup.Open).Foreground(ImageService.GetSpriteFromAsset("player", "Key"));
            
            debugWindow2.AddContent(popupControls);
            
            var animationTest = ComponentBuilder.N<ImageComponent>("test", canvasT).Offset(-300, -200);
            Sprite[] framesT = new Sprite[4];
            framesT[0] = ImageService.GetSpriteFromAsset("player", "1");
            framesT[1] = ImageService.GetSpriteFromAsset("player", "2");
            framesT[2] = ImageService.GetSpriteFromAsset("player", "3");
            framesT[3] = ImageService.GetSpriteFromAsset("player", "4");
            SpriteAnimation animationT = new SpriteAnimation(framesT, 1);
            var animatorT = animationTest.AddAnimator();
            animatorT.CreateAnimation(animationT, SpriteAnimator.Type.Loop);
            var textDescription = ComponentBuilder.N<TextComponent>("test text", animatorT.transform)
                    .Text("loop", Color.white)
                    .FitToContents()
                    .Bold()
                    .AlignCenter() 
                    .VAlignCenter()
                    .Pivot(PivotPosition.UpperCenter)
                    .AnchoredTo(PivotPosition.LowerCenter)
                ;

            var animationTest2 = animationTest.Copy().Offset(200, 0);
            animationTest2.GetAnimator().Configure(SpriteAnimator.Type.PingPong);
            var text2 = textDescription.Copy().Parent(animationTest2.transform).Text("ping pong");
            var animationTest3 = animationTest2.Copy().Offset(200, 0);
            animationTest3.GetAnimator().Configure(SpriteAnimator.Type.Once);
            var text3 = textDescription.Copy().Parent(animationTest3.transform).Text("once");
            
            var playAnimationT = removeSlots.Copy().Text("Play Animations").ClearAllFunctions()
                .Function(() =>
                {
                    animatorT.Play();
                    animationTest2.GetAnimator().Play();
                    animationTest3.GetAnimator().Play();
                }).Foreground(null);
            var pauseAnimationT = removeSlots.Copy().Text("Pause Animations").ClearAllFunctions()
                .Function(() =>
                {
                    animatorT.Pause();
                    animationTest2.GetAnimator().Pause();
                    animationTest3.GetAnimator().Pause();
                }).Foreground(null);
            
            var resetAnimationT= removeSlots.Copy().Text("Reset Animations").ClearAllFunctions()
                .Function(() =>
                {
                    animatorT.ResetAnimation();
                    animationTest2.GetAnimator().ResetAnimation();
                    animationTest3.GetAnimator().ResetAnimation();
                }).Foreground(null);

            debugWindow2.AddContent(playAnimationT, pauseAnimationT, resetAnimationT);

            var materialDemo1 = ComponentBuilder.N<ImageComponent>("Material Demo 1", canvasT)
                .Sprite(ImageService.GetSpriteFromAsset("player", "Walkie Talkie"))
                .NativeSize(5, 5)
                .Material(MaterialService.GetMaterial("ColorFade"))
                ;
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
            Input?.Enable();
        }

        private void OnDisable()
        {
            Input?.Disable();
        }
    }
}
