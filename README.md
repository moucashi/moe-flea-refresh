# Moe Flea Refresh

适用于 SPT 4.1.x 的服务端模组，可按配置主动刷新跳蚤市场的 AI 商品报价。

## 功能

- 一场战局正常结束后刷新（转移地图 `Transit` 不触发）
- 黑商（Fence）刷新时同步刷新
- 每天到达指定的一个或多个本地时间点时刷新
- 每经过固定时间间隔后刷新

刷新时仅替换由 SPT 生成的 `FakePlayer` 报价。玩家挂单与商人报价不会被删除。

## 安装

将发布包内容解压到 SPT 游戏根目录。服务端文件应位于：

`SPT/user/mods/Moe-FleaRefresh/`

## 配置

编辑模组目录中的 `config.json`，重启服务端后生效。

```json
{
  "refreshAfterRaid": true,
  "refreshWhenFenceRefreshes": false,
  "scheduledTimes": {
    "enabled": false,
    "times": ["08:00", "12:00", "18:00", "00:00"]
  },
  "fixedInterval": {
    "enabled": false,
    "minutes": 60
  }
}
```

`scheduledTimes.times` 使用服务端所在机器的本地时间，格式必须为 24 小时制 `HH:mm`。固定间隔从服务端加载模组时开始计算，`minutes` 可使用小数且必须大于 0。

四种触发方式可以同时启用。若全部关闭，模组不会主动刷新市场。

## 构建

```powershell
dotnet build -c Release
dotnet test -c Release
```

构建产物位于 `dist/SPT/user/mods/Moe-FleaRefresh/`。
