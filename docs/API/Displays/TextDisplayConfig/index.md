---
layout: API
title: TextDisplayConfig
subtitle: Describes text/character only display screens..
---

# Info

There are a variety of displays (mostly LCD) that are designed to display text (or custom characters), rather than free-form graphics. This class is used to describe the parameters of those screens (such as number of character columns and line rows), and is used when constructing new [`ITextDisplay`](/API/Displays/ITextDisplay) objects.

# API

## Properties

#### public ushort Height

Height of the display in lines (usually 2 or 4).

#### public ushort Width

Width of the display in columns (normally 16 or 20).
