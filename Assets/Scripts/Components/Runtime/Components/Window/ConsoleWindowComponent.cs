using System;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public class ConsoleWindowComponent : WindowComponent
    {
        public UnityEvent<string> onCommandEntered;

        private BaseComponent _inputFooter;
        private InputComponent _input;
        private ButtonComponent _sendButton;
        
        private InputAction _prevCommandAction;
        private InputAction _nextCommandAction;

        private float _footerHeight = 30f;

        private LayoutElement _scrollLayout;

        public Color consoleColor = Color.white;
        public Color resetColor = Color.white;
        
        public override void Awake()
        {
            base.Awake();

            onCommandEntered = new TMP_InputField.SubmitEvent();
            
            ContentPadding(PaddingSide.Bottom, _footerHeight);

            MinimumSize(150, 100);
            
            Configure()
                .ScrollingDirection(ScrollViewDirection.Vertical)
                .FittingDirection(ScrollViewDirection.Vertical)
                .AddVerticalLayout(10, TextAnchor.LowerLeft)
                .ContentPadding(PaddingSide.All, 10)
                ;
            Configure()
                .ClearSprite().Color(Color.black);

            _inputFooter = ComponentBuilder.N<BaseComponent>("Footer", transform)
                    .Size(111, _footerHeight)
                    .Pivot(PivotPosition.LowerLeft, true)
                ;

            _sendButton = ComponentBuilder.N<ButtonComponent>("Send Button", _inputFooter.transform)
                    .Create("Send >")
                    .Size(70, _footerHeight - 4)
                    .Pivot(PivotPosition.MiddleRight, true)
                    .Function(SendCommand)
                    .Color(Color.gray2)
                    .Cast<ButtonComponent>()
                ;
            
            _sendButton.GetTextComponent().AutoSize().Color(Color.white);

            _input = ComponentBuilder.N<InputComponent>("Command Input", _inputFooter.transform)
                    .Size(112, _footerHeight - 4)
                    .Create("Enter command...")
                    .Pivot(PivotPosition.MiddleLeft, true)
                    .Padding(PaddingSide.Leading, 4)
                ;
            _input.FontSize(18f).Color(Color.white).ColorBackdrop(Color.gray1);

            ScrollContent.content.Pivot(PivotPosition.LowerCenter, true);
            
            _input.onSubmit.AddListener(_ => SendCommand());
        }

        public override void Start()
        {
            base.Start();
            Configure().ScrollToBottom();
        }
        
        private void SendCommand()
        {
            var message = _input.GetText();
            message = CleanString(message);
            if (string.IsNullOrEmpty(message))
                return;
            
            AddCommand(message);
            _input.Clear();
            _input.Focus();
        }

        public static string ParseCommand(string expectedDelimiter, string command, bool requiresEmptySuffix = false)
        {
            if (!command.StartsWith(expectedDelimiter))
                return null;
            if (requiresEmptySuffix && !string.IsNullOrEmpty(command.Replace(expectedDelimiter, "")))
                return null;
            return command.Replace(expectedDelimiter, "").TrimStart();
        }

        public static string ParseCommand(char expectedDelimiter, string command)
        {
            return ParseCommand(expectedDelimiter + "", command);
        }
        
              
        string CleanString(string s)
        {
            if (s == null) return null;
            s = s.Normalize(NormalizationForm.FormC);
            s = Regex.Replace(s, @"\p{Cf}", "");
            s = s.Replace('\u00A0', ' ').Trim();
            return s;
        }

        private void AddCommand(string command, Color? color = null, FontStyles fontStyle = FontStyles.Normal)
        {
            
            var text = ComponentBuilder.N<TextComponent>("Command: " + command)
                    .NoWrap()
                    .FitToContents()
                    .FontStyle(fontStyle)
                    .FontSize(18f)
                ;
            
            var clearCommand = ParseCommand("/clear", command.ToLowerInvariant(), true);
            if (clearCommand != null)
            {
                ClearConsole();
                AddCommand("<CONSOLE>:Cleared!", Color.gray3, fontStyle:FontStyles.Italic | FontStyles.Bold);
                return;                
            }

            var colorCommand = ParseCommand("/c", command.ToLowerInvariant());
            if (colorCommand != null)
            {
                if (colorCommand.Equals("clear"))
                {
                    ResetColor();
                    return;
                }
                var validColor = ColorUtility.TryParseHtmlString(colorCommand, out Color c);
                if (validColor)
                {
                    _input.Color(c);
                    AddCommand("<CONSOLE>:Changed color to " + colorCommand, Color.gray3, FontStyles.Italic | FontStyles.Bold);
                    consoleColor = c;
                    return;
                }
            }

            if (color.HasValue)
            {
                text.Color(color.Value);
            } else
            {
                text.Color(consoleColor);
            }

            text.Text(command);
            
            ScrollContent.AddContent(text);
            onCommandEntered.Invoke(command);
            Configure().ScrollToBottom();
        }

        public void ResetColor()
        {
            AddCommand($"/c #{ColorUtility.ToHtmlStringRGB(resetColor)}");
        }

        public void ClearConsole()
        {
            Debug.Log("Clearing console!");
            var t = ScrollContent.content.transform;
            foreach (Transform tChild in t)
            {
                Destroy(tChild.gameObject);
            }
        }

        public override BaseComponent HandleSizeChanged(float x, float y)
        {
            base.HandleSizeChanged(x, y);
            _inputFooter.Width(x - WindowResizer.GetWidth() - 8f);
            _input.Width(_inputFooter.GetWidth() - _sendButton.GetWidth() - 8f);
            ScrollContent.content.Width(ScrollContent.GetWidth());
            return this;
        }

        public InputComponent GetInputField()
        {
            return _input;
        }

        public override void Collapse()
        {
            base.Collapse();
            _inputFooter.SetActive(false);
        }

        public override void Expand()
        {
            base.Expand();
            _inputFooter.SetActive();
        }
    }
}