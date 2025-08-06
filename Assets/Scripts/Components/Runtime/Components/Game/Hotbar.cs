using System.Collections.Generic;
using Components.Runtime.Input;
using UnityEngine;

namespace Components.Runtime.Components.Game
{
    public class Hotbar : ButtonComponent
    {
        public List<HotbarSlot> Slots = new List<HotbarSlot>();
        public int activeSlot = 0;

        private ComponentControls _input;

        public int SelectedSlotIndex
        {
            get => activeSlot;
            set
            {
                if (value < 0)
                    value = Slots.Count - 1;
                activeSlot = value % Slots.Count;
                Slots[activeSlot].Focus();
            }
        }

        public override void Awake()
        {
            base.Awake();
            _input = new ComponentControls();

            _input.UI.ScrollWheel.performed += context =>
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