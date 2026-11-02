TOD Unity Package
===

## Description
This package contains the client for settling store purchases on a payment server. Server addresses stay in your project, never in this package.

## Dependencies
Add this to `Packages/manifest.json` before this package, or it cannot compile:
- [noname-oneup-singleton](https://github.com/sajadimn/noname-oneup-singleton)

## How to use
1. Add this package. It creates `TodConfig` in `Assets/Settings/TOD/Resources`.
2. Open `OneUp/TOD/Config` and enter `settleUrl`. Cafe Bazaar and Myket use `settleHttpUrl`.
3. Fill `TodServerAPIData` and call `TodServerAPI.Instance.Request`. For ZarinPal, also set `merchant_id`. No prefab or scene object is needed.

## Examples

✅️ Settle a purchase, then give the reward
```c#
TodServerAPI.Instance.Request(todServerAPIData, OnSettled, OnFailed);
```

Every request answers exactly once, with its own callbacks. `AppStore` and unknown stores call `failed`.

## Retrying
The package does not retry a failed settle, because a repeated settle could count twice unless your server blocks it. Keep the purchase unconsumed so it can be settled on the next launch.
