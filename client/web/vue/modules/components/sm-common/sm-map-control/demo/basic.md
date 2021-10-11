
<cn>
#### 基本方法
</cn>

<us>
#### 基本方法
</us>

```tpl
<template>
  <div>
    <sm-map-control :height='600' mode='mark' :position='position' :zoom='zoom' :showPoint='showPoint'/>
  </div>

</template>
<script>
export default {
  data(){
    return {
      position:[],
      showPoint:false,
      zoom:10,
      x:0,
      y:0
    }
  },
  created(){
  },
  methods: {
  }
}
</script>
*** 