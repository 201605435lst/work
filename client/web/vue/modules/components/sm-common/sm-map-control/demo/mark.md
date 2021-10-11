
<cn>
#### 地图标注
</cn>

<us>
#### 地图标注
</us>

```tpl
<template>
  <div>
    <div>
      请输入坐标： x:<a-input v-model='x'></a-input> y:<a-input v-model='y'></a-input>
      请输入缩放范围： <a-input v-model='num'></a-input>
      <a-checkbox v-model='showPoint'>
        是否显示标注
      </a-checkbox>
    </div>
    <br/>
    <sm-map-control :height='600' mode='mark' :position='position' :zoom='zoom' :showPoint='showPoint' @click='mapclick'/>
  </div>

</template>
<script>
export default {
  data(){
    return {
      position:[],
      showPoint:false,
      num:'10',
      x:0,
      y:0
    }
  },
  computed:{
    zoom(){
      return parseInt(this.num);
    }
  },
  created(){
  },
  methods: {
    mapclick(point){
      this.x=point[0];
      this.y=point[1];
    }
  }
}
</script>
*** 