using System.Collections.Generic;
using Components.Runtime.Input;
using UnityEngine;

namespace Components.Runtime.Components.Game
{
    public class Hotbar : ButtonComponent
    {
        protected List<HotbarSlot> Slots = new List<HotbarSlot>();
        private int _selectedSlotIndex = 0;

        private ComponentControls _input;

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

            this.FitToContents(25);
            this.GetTextComponent().SetActive(false);
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
        
        public Hotbar AddNewSlot(HotbarSlot template)
        {
            AddSlots(template.Copy());
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