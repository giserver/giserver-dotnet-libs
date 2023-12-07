# tiles-collection

收集所有切片，包含：raster tiles，vector tiles，3D tiles

## 支持的格式

- raster
  - xyz
- vector
  - mvt
  - geojson
  - geobuf
- 3D
  - 3dtiles

## 支持的功能

1. 添加 : ui 需提供增加 tiles 数据界面，必须提供地图供查看(验证是否正确)
1. 查询 : 根据名称、年份、描述，进行筛选查询

## 数据库字段

| col         | type  | required | description |
| ----------- | ----- | -------- | ----------- |
| id          | uuid  | y        |             |
| type        | text  | y        | 切片类型    |
| data        | jsonb | y        | 切片数据    |
| name        | text  | y        | 切片名字    |
| year        | int4  | y        | 年代        |
| description | text  | n        | 描述        |
