using System;
using System.Collections;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using Netduino.Foundation.Displays;

namespace Netduino.Foundation.Displays.TextDisplayMenu
{
    public class Menu
    {
        protected ITextDisplay _display = null;

        protected int _navigatedDepth = 0;
        protected MenuPage _rootMenuPage = null;
        protected MenuPage _currentMenuPage = null;
        

        public Menu(ITextDisplay display, MenuPage menuTree)
        {
            _display = display;
            _rootMenuPage = menuTree;
            UpdatedCurrentMenuPage();
            RenderCurrentMenuPage();

            // Save our custom characters
            _display.SaveCustomCharacter(TextCharacters.RightArrow.CharMap, TextCharacters.RightArrow.MemorySlot);
            _display.SaveCustomCharacter(TextCharacters.RightArrowSelected.CharMap, TextCharacters.RightArrow.MemorySlot);
            _display.SaveCustomCharacter(TextCharacters.BoxSelected.CharMap, TextCharacters.BoxSelected.MemorySlot);
        }

        protected void RenderCurrentMenuPage()
        {
            // clear the display
            _display.Clear();

            // if there are no items to render, get out.
            if (_currentMenuPage.MenuItems.Count <= 0) return;
            
            // calculate the first item index that should be displayed
            int displayStartIndex = CalculateDisplayedItemStartIndex(_currentMenuPage.MenuItems.Count, _currentMenuPage.ScrollPosition);
            int lastDisplayableItem = CalculateLastDisplayableItem(displayStartIndex, _currentMenuPage.MenuItems.Count);

            Debug.Print("Scroll: " + _currentMenuPage.ScrollPosition.ToString() + ", start: " + displayStartIndex + ", end: " + lastDisplayableItem.ToString());

            byte lineNumber = 0;
            
            // 
            for (int i = displayStartIndex; i < lastDisplayableItem; i++) {
                IMenuItem item = _currentMenuPage.MenuItems[i] as IMenuItem;

                // trim and add selection
                string lineText = GetItemText(item, (i == _currentMenuPage.ScrollPosition));
                _display.WriteLine(lineText, lineNumber);

                lineNumber++;
            }

        }

        protected string GetItemText(IMenuItem item, bool isSelected)
        {
            string itemText = "";

            if (isSelected)
            {
                // calculate any neccessary padding to put selector on far right
                int paddingLength = (_display.DisplayConfig.Width - 1 - item.Text.Length);
                string padding = "";
                if (paddingLength > 0) padding = new string(' ', paddingLength);
                //
                itemText = item.Text.Substring(0, (item.Text.Length >= _display.DisplayConfig.Width - 1) ? _display.DisplayConfig.Width - 1 : item.Text.Length) + padding + TextCharacters.BoxSelected.ToChar();
            } else
            {
                itemText = item.Text.Substring(0, (item.Text.Length >= _display.DisplayConfig.Width) ? _display.DisplayConfig.Width : item.Text.Length);
            }

            return itemText;
        }

        protected int CalculateDisplayedItemStartIndex(int menuItemCount, int scrollPosition)
        {
            int rows = _display.DisplayConfig.Height;

            // if there aren't enough items to scroll
            if (menuItemCount <= rows) return 0;

            // if the current scroll position doesn't require scrolling to view
            if (scrollPosition <= (rows - 1)) return 0;

            // otherwise, it should be the current scroll position minus the number of rows
            // so if the scroll position is 4, and there are 2 rows, we should start at item
            // index 2
            return scrollPosition - rows + 1;
        }

        protected int CalculateLastDisplayableItem(int displayStartIndex, int itemCount)
        {
            int rows = _display.DisplayConfig.Height;

            // if the number of items is less than or equal to the row count, there's
            // only one page
            if (itemCount <= rows) return itemCount;

            // if we're on the last page
            if (itemCount - (displayStartIndex + 1) <= rows) return itemCount - 1;

            // otherwise, it's just the first item plus row count
            return (displayStartIndex + rows);
        }

        /// <summary>
        /// Updates the _currentMenuPage based on the current navigation depth
        /// </summary>
        protected void UpdatedCurrentMenuPage()
        {
            if (_navigatedDepth == 0) _currentMenuPage = _rootMenuPage;
            else
            {
                MenuPage page = _rootMenuPage;
                for (int i = 0; i < _navigatedDepth; i++)
                {
                    page = (page.MenuItems[page.ScrollPosition] as IMenuItem).SubMenu;
                }
                _currentMenuPage = page;
            }
        }

        public bool MoveNext()
        {
            Debug.Print("MoveNext");

            // if outside of valid range return false
            if (_currentMenuPage.ScrollPosition >= _currentMenuPage.MenuItems.Count) return false;

            // increment scroll position
            _currentMenuPage.ScrollPosition++;
            Debug.Print("New Position: " + _currentMenuPage.ScrollPosition);

            // re-render menu
            RenderCurrentMenuPage();

            return true;
        }

        public bool MovePrevious()
        {
            Debug.Print("MoveNext");

            // if outside of valid range return false
            if (_currentMenuPage.ScrollPosition <= 0) return false;

            // increment scroll position
            _currentMenuPage.ScrollPosition--;
            Debug.Print("New Position: " + _currentMenuPage.ScrollPosition);

            // re-render menu
            RenderCurrentMenuPage();

            return true;
        }

        public bool SelectCurrentItem()
        {
            return true;
        }

    }
}
