# Moe Flea Refresh

[English](README.md) | [简体中文](README.zh-CN.md)

A server-side mod for SPT 4.1.x that proactively regenerates AI flea market offers at configurable times.

## Features

- Refresh after a local raid ends; map-to-map `Transit` does not trigger a refresh
- Refresh when Fence regenerates his assortment
- Refresh at one or more configured local times every day
- Refresh after a configurable fixed interval

A refresh removes and regenerates only SPT's `FakePlayer` offers. Real player listings and trader offers are preserved. Items whose AI offers have sold out can receive newly generated listings after the refresh.

## Installation

Extract the release archive into the SPT game directory. The server mod should be located at:

`SPT/user/mods/Moe-FleaRefresh/`

## Configuration

Edit `config.json` in the installed mod directory, then restart the SPT server. The example below uses `jsonc` only to explain each option; keep the actual configuration file as valid JSON without comments.

```jsonc
{
  // Refresh after a completed local raid. Enabled by default.
  // All raid results except map-to-map Transit are eligible.
  "refreshAfterRaid": true,

  // Refresh whenever Fence regenerates his assortment.
  "refreshWhenFenceRefreshes": false,

  "scheduledTimes": {
    // Enable daily refreshes at the local server times listed below.
    "enabled": false,

    // One or more 24-hour times in strict HH:mm format.
    // These use the local clock of the machine running the SPT server.
    "times": ["08:00", "12:00", "18:00", "00:00"]
  },

  "fixedInterval": {
    // Enable refreshes at a repeating interval measured from mod startup.
    "enabled": false,

    // Interval length in minutes. Decimal values are supported; must be greater than 0.
    "minutes": 60
  }
}
```

All four triggers can be enabled at the same time. If every trigger is disabled, the mod will not proactively refresh the flea market.

Every successful refresh writes a server log entry similar to:

```text
[Moe Flea Refresh] 已刷新跳蚤市场（战局结束），替换 12345 条 AI 报价
```

## Building

```powershell
dotnet build -c Release
dotnet test -c Release
```

Build output is written to `dist/SPT/user/mods/Moe-FleaRefresh/`.
