
<cn>
#### 预览弹窗
</cn>

<us>
#### 预览弹窗
</us>

```tpl
<template>
  <div>
    <a-button @click="play">视频预览</a-button>
    
    <sm-video ref='player' title='视频预览' :visible='visible' :width='900' :height='500'/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'
export default {
  data(){
    return {
      axios,
      visible:false,
      srouce:'http://vjs.zencdn.net/v/oceans.mp4'
    }
  },
  created(){
  },
  methods: {
    play(){
      let url=this.srouce;
      let player=this.$refs.player;
      player.preview(true,url,'测试视频');
    }
  }
}
</script>