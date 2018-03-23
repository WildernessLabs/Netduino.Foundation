---
layout: API
title: IDigitalPort
subtitle: Interface contract for digital GPIO ports
---

# Info

`IDigitalPort` implements [`IPort`](/API/GPIO/IPort) and is the base interface for all digital port classes.

# API

### Properties

#### `bool State { get; set; }`

Gets or sets the port state, either high (`true`), or low (`false`).