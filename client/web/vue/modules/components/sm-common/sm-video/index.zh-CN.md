# Sm-Video API 说明

## sm-video-base

### 1.属性

| 名称          | 类型    | 描述                              |
| ------------- | ------- | --------------------------------- |
| url           | string  | 视频播放地址                      |
| controls      | Boolean | 是否显示视频控制条                |
| muted         | Boolean | 是否静音播放                      |
| autoplay      | Boolean | 是否自动播放                      |
| preload       | string  | 加载<video>标签后是否自动加载视频 |
| width         | string  | 播放器的宽度                      |
| height        | string  | 播放器的高度                      |
| playbackRates | Array   | 倍速配置                          |

### 2.方法

| 名称 | 参数  | 说明                     |
| ---- | ----- | ------------------------ |
| play | (url) | 传入视频地址，并播放视频 |

## sm-video

视频预览弹窗

### 1、属性

| 名称    | 类型    | 描述           |
| ------- | ------- | -------------- |
| title   | String  | 模态框标题名称 |
| visible | Boolean | 模态框状态     |
| url     | String  | 默认的视频地址 |
| width   | Number  | 模态框宽度     |
| height  | Number  | 模态框高度     |

### 2、方法

| 名称    | 参数                | 说明                                       |
| ------- | ------------------- | ------------------------------------------ |
| preview | （state,url,title?) | 预览视频，可传入模态框状态，视频地址，标题 |

