
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <input style="width:200px" v-model="srouce"/>
    <a-button @click="play">播放</a-button>
    测试地址
    <ul>
      <li>https://v-cdn.zjol.com.cn/280443.mp4</li>
      <li>https://v-cdn.zjol.com.cn/276982.mp4</li>
      <li>https://v-cdn.zjol.com.cn/276984.mp4</li>
      <li>https://v-cdn.zjol.com.cn/276985.mp4</li>
      <li>https://v-cdn.zjol.com.cn/276986.mp4</li>
      <li>https://v-cdn.zjol.com.cn/276987.mp4</li>
    </ul>
    <sm-video-base :autoplay="true" ref="player" url=""/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
import SmVideoBase from '../src/SmVideoBase'
export default {
  components:{
    SmVideoBase,
  },
  data(){
    return {
      axios,
      srouce:'http://vjs.zencdn.net/v/oceans.mp4'
    }
  },
  created(){
  },
  methods: {
    play(){
      let url=this.srouce;
      let player=this.$refs.player;
      player.play(url);
    }
  }
}
</script>