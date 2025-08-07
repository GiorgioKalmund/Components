using System.Collections.Generic;
using Components.Runtime.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Runtime.Components.Game
{
    public class Hotbar : ButtonComponent
    {
        public List<HotbarSlot> Slots = new List<HotbarSlot>();
        private int _selectedSlotIndex = 0;
        public int SelectedSlotIndex
        {
            get => _selectedSlotIndex;
            set
            {
                if (value < 0)
                    value = Slots.Count - 1;
                _selectedSlotIndex = value % Slots.Count;
                Slots[_selectedSlotIndex].Focus();
            }
        }
        
        // -- Input Handling -- //
        // probably needs to be outsourced to somewhere more central at some point
        private ComponentControls _input;
        private InputAction _scrollAction;
        public bool Frozen => !_scrollAction?.enabled ?? true;

        public override void Awake()
        {
            base.Awake();
            _input = new ComponentControls();
            _scrollAction = _input.UI.ScrollWheel;

            _scrollAction.performed += context =>
            {
                Vector2 scrollDelta = context.ReadValue<Vector2>();

                if (scrollDelta.y > 0f)
                    SelectedSlotIndex++;
                else if (scrollDelta.y < 0f)
                    SelectedSlotIndex--;
                
            };

            FitToContents(25).DisabledColor(UnityEngine.Color.white).Lock();
            GetTextComponent().SetActive(false);
        }

        public override void Start()
        {
            base.Start();
            Slots[0].Focus();
        }

        public void Freeze()
        {
            _scrollAction?.Disable();
        } 
        public void UnFreeze()
        {
            _scrollAction?.Enable();
        }

        public void ToggleFreeze()
        {
            if (Frozen)
                UnFreeze();
            else
                Freeze();
        }

        public Hotbar AddSlots(params HotbarSlot[] slots)
        {
            foreach (var hotbarSlot in slots)
            {
                if (!hotbarSlot)
                    return this;
                
                Slots.Add(hotbarSlot);
                hotbarSlot.Parent(this);
                hotbarSlot.SetActive();
            }
            
            return this;
        }

        public Hotbar RemoveSlot(int index)
        {
            if (index >= 0 && index < Slots.Count)
            {
                var toRemove = Slots[index];
                Slots.RemoveAt(index);
                Destroy(toRemove.gameObject);
            }
            else
            {
                Debug.LogWarning("Trying to access invalid hotbar index: " + index);
            }
            return this;
        }
        
        public Hotbar AddNewSlot(HotbarSlot template, bool copy = false)
        {
            AddSlots(copy ? template : template.Copy());
            return this;
        }

        public void OnEnable()
        {
            _input?.Enable();
        }
        public void OnDisable()
        {
            _input?.Disable();
        }
    }
}