<template>
  <view class="content">
    <!-- <image class="logo" src="/static/logo.png"></image>
		<view class="text-area">
			<text class="title">{{title}}</text>
		</view> -->
    <button @click="onScan">扫码</button>

    <!-- <view><web-view :webview-styles="webviewStyles" src="http://localhost:8080" @message="onMessage" @onPostMessage="onMessage"></web-view></view> -->
  </view>
</template>

<script>
export default {
  data() {
    return {
      title: 'Hello',
      webviewStyles: {
        progress: {
          color: '#FF3333'
        }
      }
    };
  },
  onLoad() {
    console.log('注册 window message 事件');

    // window.addEventListener('message', (data) => {
    //   console.log('window on message: ',data)
    // }, false);
  },
  methods: {
    onScan() {
      uni.scanCode({
        success: function(res) {
          console.log('条码类型：' + res.scanType);
          console.log('条码内容：' + res.result);
        }
      });
    },
    onMessage(data) {
      console.log(`----------------UniApp收到消息:${data}`);
      console.dir(data);
    }
  }
};
</script>

<style>
.content {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.logo {
  height: 200rpx;
  width: 200rpx;
  margin-top: 200rpx;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 50rpx;
}

.text-area {
  display: flex;
  justify-content: center;
}

.title {
  font-size: 36rpx;
  color: #8f8f94;
}
</style>
