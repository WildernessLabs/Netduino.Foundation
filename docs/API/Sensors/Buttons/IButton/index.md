---
layout: API
title: IButton
subtitle: Interface describing button classes.
---

# Info

Minimum required definition for button classes.

# API

## Properties

#### `public bool State`

Returns the current raw state of the switch. If the switch is pressed (connected), returns true, otherwise false.

## Events

#### `public event EventHandler PressStarted`

Raised when a press starts (the button is pushed down; circuit is closed).

#### `public event EventHandler PressEnded`

Raised when a press ends (the button is released; circuit is opened).

#### `public event EventHandler Clicked`

Raised when the button circuit is re-opened after it has been closed (at the end of a "press".