---
layout: API
title: IButton
subtitle: Interface describing button classes.
---

# Info

Minimum required definition for button classes.

# API

## Events

#### `public event EventHandler PressStarted`

Raised when a press starts (the button is pushed down; circuit is closed).

#### `public event EventHandler PressEnded`

Raised when a press ends (the button is released; circuit is opened).

#### `public event EventHandler Clicked`

Raised when the button circuit is re-opened after it has been closed (at the end of a "press".