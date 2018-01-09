---
layout: API
title: ArrayEventArgs
subtitle: Event arguments for events coming from an array.
---

# Info

`ArrayEventArgs` provides additional information for events that are raised by classes that need to pass array index and item information. This class is used in conjunction with the `ArrayEventHandler` class in instances where a component might have multiple items that raise the same event. For example, the [`DipSwitch`](/API/Sensors/Switches/DipSwitch) class contains an array of `ISwitch` objects. Its `Changed` event uses the `ArrayEventArgs` to indicate which switch was changed, and passes a reference to the switch causing the event.

# API

## Properties

#### `public int ItemIndex { get; set; }`

The index of the item in the source array.

#### `public object Item { get; set; }`

The item of the array.

## Constructors

#### `public ArrayEventArgs(int itemIndex, object item)`

Creates a new `ArrayEventArgs` object with the specified `itemIndex` and array `item`.


