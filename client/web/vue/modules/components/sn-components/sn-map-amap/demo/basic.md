<cn>
#### 高德地图
</cn>

<us>
#### 高德地图
</us>

```tpl
<template>
  <div>
    <sn-map-amap/>
  </div>
</template>
<script>

export default {
  data(){
    return{
      info:null,
    }
  },
  created(){
  },
  methods: {
    click(value){
      console.log(this.info)
    }
  }
}
</script>
```
