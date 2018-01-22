---
layout: API
title: ITextDisplay
subtitle: Interface describing text-only displays.
---

# Info

There are a variety of displays (mostly LCD) that are designed to display text (or custom characters), rather than free-form graphics. This interface describes those devices.

# API

## Properties

#### TextDisplayConfig DisplayConfig { get; }

Returns the [`TextDisplayConfig`](/API/Displays/TextDisplayConfig) object that describes the display.

## Methods

#### void WriteLine(ushort lineNumber, string text)

Writes the specified string to the specified line number on the display.

#### void ClearLine(ushort lineNumber)

Clears the specified line of characters on the display.