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
            RenderMenu();
        }

        protected void RenderMenu()
        {
            // clear the display
            _display.Clear();

            // if there are no items to render, get out.
            if (_currentMenuPage.MenuItems.Count <= 0) return;
            
            // calculate the first item index that should be displayed
            int displayStartIndex = CalculateDisplayedItemStartIndex(_currentMenuPage.MenuItems.Count, _currentMenuPage.ScrollPosition);
            int lastDisplayableItem = CalculateLastDisplayableItem(_currentMenuPage.ScrollPosition, _currentMenuPage.MenuItems.Count);

            byte lineNumber = 0;
            
            for (int i = _currentMenuPage.ScrollPosition; i < lastDisplayableItem; i++) {
                IMenuItem item = _currentMenuPage.MenuItems[i] as IMenuItem;
                _display.WriteLine(item.Text, lineNumber);

                // if it's the current scroll position, select the line
                // TODO: add selection to ITextDisplay

                lineNumber++;
            }

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
            return scrollPosition - rows;
        }

        protected int CalculateLastDisplayableItem(int scrollPosition, int itemCount)
        {
            int rows = _display.DisplayConfig.Height;

            // if the number of items is less than or equal to the row count
            if (itemCount <= rows) return itemCount;

            // if the scroll position is at the last item or on the last page of items
            // for instance, if scroll position = 3, item count = 4, and rows = 2
            // then we should show items 3 and 4
            if (scrollPosition <= itemCount && scrollPosition > (itemCount - rows) ) return itemCount;

            // otherwise, it's just the scroll position plus row count
            // for instance, if there's 10 items, and scroll position is 6, then last item is 8
            return (scrollPosition + rows);
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
                    page = (page.MenuItems[page.ScrollPosition] as IMenuItem).Children;
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
            RenderMenu();

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
            RenderMenu();

            return true;
        }

        public bool SelectCurrentItem()
        {
            return true;
        }

    }
}
