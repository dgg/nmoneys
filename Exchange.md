# NMoney's Vision #
_NMoneys_ has a simple vision: providing a .Net implementation of the Money Value Object that abides to the ISO 4417 specification.
Converting monetary quantities from one currency into another was not in this vision. It still isn't and probably ever will. But some people demanded such features. And if it is helpful and reasonable I went and tried a couple of things and...

# NMoneys.Exchange #

... _NMoneys.Exchange_ is born. This twin project complements _NMoneys_ in such a way that allows conversions of monetary quantities in a given currency into another currency.
Not to disappoint anyone, _NMoneys.Exchange_ is a framework. It is not a currency exchange service of any sort.
Instead, you can use the extensibility points provided by _NMoneys.Exchange_ to plug in your favorite provider of data, but out-of-the-box it is an unisteresting: a most than likely incorrect one to one exchange rate is applied in order to convert whichever pair of monetary quantities.

## How do I Use it Then? ##
Being a framework, being some sort of .Net developer is a must. You are likely to be interested in [ExchangeQuickStart](ExchangeQuickStart.md)