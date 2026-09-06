# Moe Flea Refresh

[English](README.md) | [简体中文](README.zh-CN.md)

适用于 SPT 4.1.x 的服务端模组，可在配置指定的时机主动重新生成跳蚤市场的 AI 商品报价。

## 功能

- 本地战局结束后刷新；地图间转移 `Transit` 不触发
- 黑商（Fence）重新生成商品时同步刷新
- 每天到达指定的一个或多个本地时间点时刷新
- 每经过一段固定时间后刷新

刷新时仅删除并重新生成由 SPT 创建的 `FakePlayer` 报价。玩家挂单与商人报价不会被删除。某件商品的 AI 报价被买空后，可以在刷新时重新获得生成的挂单。

## 安装

将发布包内容解压到 SPT 游戏根目录。服务端文件应位于：

`SPT/user/mods/Moe-FleaRefresh/`

## 配置

编辑已安装模组目录中的 `config.json`，然后重启 SPT 服务端。下面使用 `jsonc` 仅用于逐项解释配置；实际配置文件必须保持为不含注释的合法 JSON。

```jsonc
{
  // 本地战局完成结算后刷新，默认启用。
  // 除地图间转移 Transit 之外的战局结果均可触发。
  "refreshAfterRaid": true,

  // 黑商（Fence）重新生成商品时刷新。
  "refreshWhenFenceRefreshes": false,

  "scheduledTimes": {
    // 是否启用每天指定时间点刷新。
    "enabled": false,

    // 一个或多个严格采用 HH:mm 格式的 24 小时时间点。
    // 使用运行 SPT 服务端的机器本地时间。
    "times": ["08:00", "12:00", "18:00", "00:00"]
  },

  "fixedInterval": {
    // 是否启用固定间隔刷新；间隔从模组加载完成时开始计算。
    "enabled": false,

    // 间隔分钟数，支持小数且必须大于 0。
    "minutes": 60
  }
}
```

四种触发方式可以同时启用。若全部关闭，模组不会主动刷新跳蚤市场。

每次成功刷新都会在服务端写入类似下面的日志：

```text
[Moe Flea Refresh] 已刷新跳蚤市场（战局结束），替换 12345 条 AI 报价
```

运行时消息会自动跟随 SPT 服务端语言。中文语言环境使用简体中文，其他语言环境使用英文。

## 构建

```powershell
dotnet build -c Release
dotnet test -c Release
```

构建产物位于 `dist/SPT/user/mods/Moe-FleaRefresh/`。
