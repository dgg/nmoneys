![logo](https://raw.githubusercontent.com/dgg/nmoneys/wiki/NMoneys_long.png)

Implementation of the Money Value Object to support representing moneys in the currencies defined in the ISO 4217 standard

[![Build status](https://ci.appveyor.com/api/projects/status/n8378j7abyui3hwb?svg=true)](https://ci.appveyor.com/project/dgg/nmoneys)
[![Coverage Status](https://coveralls.io/repos/github/dgg/nmoneys/badge.svg?branch=master)](https://coveralls.io/github/dgg/nmoneys?branch=master)

# What

_NMoneys_ (plural) is a simple .Net library to represent monetary quantities.

What does _NMoneys_ provide?
 * types for __representing__ _currencies_ that conform to the ISO 4217 standard and quantities of _money_ in a given currency.
 * __simple but extensible operations__ with monetary quantities of the __same currency__, including allocations of several sorts.
 * ways of __formatting__ the representation of monetary quantities
 * a simple way of __contributing__ to improve the completeness and correctness of the library

## What not
_NMoneys_ __does not provide__ any support for __exchanging__ monetary quantities in different currencies.
That means that you could not "convert", for example, 10 Euro into the equivalent quantity in dollars using an internal exchange rate in the library.
Instead, you could, for example, perform the conversion with numeric types and then display the resultant quantity in a meaningful way by using types provided by NMoneys.

If you need to convert monetary quantities into other currencies you can use the framework provided by _NMoneys.Exchange_ alongside your trusted currency rates feed.

NMoneys does not provide, as of now, __complex monetary or financial operations__

## What might
The aim of _NMoneys_ is being simple and to-the-point: represent monetary quantities.

But, one of the reasons of making it Open-Source is that people with knowledge in the areas related with the subject of the library, that is money, can contribute with correct and useful ways to operate with the concepts in the library without cluttering its original purpose.

# Why

## The .NET Framework
.Net does not provide a good way of representing and operating with monetary quantities.
Nonetheless, it does support numeric types that can be used to represent monetary quantities and it also provides support for formatting those numeric values in different cultures. But it is surprisingly easy to mix the concept of "_10 represented euros when was saved_" with "_now, 10 represents something else because of the current culture of the thread_".

The .Net Framework mixes numbers, currencies, cultures and formats in a way that it becomes confusing, difficult and/or impossible to represent something as simple as "one Euro" or "ten-and-the-half Zambian kwacha".

On top of that mixture of concepts, it does not support a complete implementation for the ISO standard and for the subset implemented, the information may be outdated or even wrong. Fixes might be issued for wrong/outdated data, but they may take too long to be rolled-out and to add further confusion currency formatting information can be modified by the user.

## Other libraries
There might be others libraries or simple code snippets that might cover a necessity.
But they did not cover mine for one or another reason: different goals, lack of activity/outdated, not suitable API,...

## Open source
I have been using different incarnations of this library in commercial projects for some time now. It started with a limited set of well-known currencies. Then it grew to include some others until I decided to support all ISO currencies.
At that point I realised that there was no way I could support correctly all scenarios for multiple reasons:
 * lack of cultural knowledge (e.g. how does one represent decimals in Swaziland?)
 * lack of technical knowledge (e.g how does one distribute an amount of money amongst a number of parties?)

With the realisation came the proposal to my employer to Open-Source the library and modify it so that is easy enough for people to help out, even if they are not .Net programmers.

And here we are.
