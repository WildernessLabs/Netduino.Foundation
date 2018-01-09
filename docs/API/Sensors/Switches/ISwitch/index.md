---
layout: API
title: ISwitch
subtitle: Interface describing switch classes.
---

# Info

Minimum required definition for switch classes.

# API

## Events

#### `event EventHandler Changed`

Raised when the switch circuit circuit is opened or closed.

## Properties

#### `public bool IsOn`

Describes whether or not the switch circuit is closed/connected (`IsOn = true`), or open (`IsOn = false`).